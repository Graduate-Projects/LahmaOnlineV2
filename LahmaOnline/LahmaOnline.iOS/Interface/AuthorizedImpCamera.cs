using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AVFoundation;
using Foundation;
using LahmaOnline.Interface;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(LahmaOnline.iOS.Interface.AuthorizedImpCamera))]
namespace LahmaOnline.iOS.Interface
{
    public class AuthorizedImpCamera : IAuthorizeCamera
    {
        public void Authorized()
        {
            bool permission = false;
            var authorizationStatus = AVCaptureDevice.GetAuthorizationStatus(AVMediaType.Video);
            if (authorizationStatus != AVAuthorizationStatus.Authorized)
            {
                Task.Run(async () => permission = await AVCaptureDevice.RequestAccessForMediaTypeAsync(AVMediaType.Video)).Wait();
            }
            else
                permission = true;
            MessagingCenter.Send<string>(permission.ToString(), "ResultAuthorization");
        }

    }
}