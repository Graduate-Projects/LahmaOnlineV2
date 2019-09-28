using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using LahmaOnline.iOS.CustomRenderer;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(Picker), typeof(CustomPickerRender))]
namespace LahmaOnline.iOS.CustomRenderer
{
    class CustomPickerRender : PickerRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            base.OnElementChanged(e);
            var view = e.NewElement as Picker;
            this.Control.BorderStyle = UITextBorderStyle.None;
        }
    }
}
