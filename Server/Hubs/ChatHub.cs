using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Hubs
{
    public class ChatHub : Hub
    {
        private readonly static ConnectionMapping<string> _connections = new ConnectionMapping<string>();
        private readonly static List<Model.Message> Messages = new List<Model.Message>();

        public void Send(string namerecevier, string username, string message)
        {
            foreach (var connectionId in _connections.GetConnections(namerecevier))
            {
                var Msg = new Model.Message
                {
                    Id = Guid.NewGuid(),
                    Time = DateTime.Now,
                    From = username,
                    To = namerecevier,
                    Text = $"From: {username} \nTO: {namerecevier} \nConnectionId: {connectionId}\nMessage: {message}",
                    IsReading = false
                };
                Messages.Add(Msg);
                var Object = JsonConvert.SerializeObject(Msg);
                Clients.Client(connectionId).SendAsync("ReceiveMessage", Object);
            }            
        }
        public void LoadMore(string username, int messageCount)
        {
            var SpecificMessageForUser = Messages
                                        .Where(item => item.From == username || item.To == username);
            var count = SpecificMessageForUser.Count();
            List<Model.Message> Tamp;
            if (count - messageCount > 10)
            {
                Tamp = SpecificMessageForUser.Skip(count - (10 + messageCount)).Take(10).ToList();
            }
            else
            {
                Tamp = SpecificMessageForUser.Skip(count - (10 + messageCount)).Take(count - messageCount).ToList();
            }
            foreach (var msg in Tamp.OrderByDescending(msg=>msg.Time))
            {
                foreach (var connectionId in _connections.GetConnections(username == msg.To ? msg.To : msg.From))
                {
                    var Object = JsonConvert.SerializeObject(msg);
                    Clients.Client(connectionId).SendAsync("LoadOldMessage", Object);
                }
            }
        }
        public void RecoverMessage(string username, int sizereturn)
        {
            //Select Message From To user
            var SpecificMessageForUser = Messages
                                        .Where(item => item.From == username || item.To == username);
            //Task Last 10 Message to Resturn it
            var count = SpecificMessageForUser.Count();
            SpecificMessageForUser = SpecificMessageForUser.Skip(Math.Max(0, count - sizereturn));

            foreach (var msg in SpecificMessageForUser)
            {
                foreach (var connectionId in _connections.GetConnections(username == msg.To ? msg.To : msg.From))
                {
                    var Object = JsonConvert.SerializeObject(msg);
                    Clients.Client(connectionId).SendAsync("ReceiveMessage", Object);
                }
            }
        }
        public void EditInfoMessageIsReading(Guid id)
        {
            Messages.FirstOrDefault(item => item.Id == id).IsReading = true;
        }
        public void Connect(string username)
        {
            string Id;
            if ( (Id = _connections.GetConnections(username).FirstOrDefault()) != null)
                _connections.Remove(username, Id);

            _connections.Add(username, Context.ConnectionId);

            //If There New Messages (Give it Notfiy)
            if (Messages.Any(msg => (msg.From == username || msg.To == username) && !msg.IsReading))
            {
                var Msg = new Model.Message
                {
                    Id = Guid.NewGuid(),
                    Time = DateTime.Now,
                    From = "SystemServer_Notify",
                    To = username,
                    Text = $"There new message for you. Please Check your inbox",
                    IsReading = true
                };
                var Object = JsonConvert.SerializeObject(Msg);
                Clients.Client(Context.ConnectionId).SendAsync("ReceiveMessage", Object);
            }

        }
    }
}
