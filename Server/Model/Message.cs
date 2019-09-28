using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Model
{
    public class Message
    {
        public Guid Id { get; set; }
        public DateTime Time { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Text { get; set; }
        public bool IsReading { get; set; }
    }
}
