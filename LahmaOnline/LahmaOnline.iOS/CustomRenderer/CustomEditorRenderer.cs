﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoreGraphics;
using Foundation;
using LahmaOnline.CustomRenderer;
using LahmaOnline.iOS.CustomRenderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomEditor), typeof(CustomEditorRenderer))]
namespace LahmaOnline.iOS.CustomRenderers
{
    class CustomEditorRenderer : EditorRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement != null)
            {
                var view = (CustomEditor)Element;

                // Control.LeftView = new UIView(new CGRect(0f, 0f, 9f, 20f));
                // Control.LeftViewMode = UITextFieldViewMode.Always;

                Control.KeyboardAppearance = UIKeyboardAppearance.Dark;
                Control.ReturnKeyType = UIReturnKeyType.Done;
                // Control.BorderStyle = UITextBorderStyle.None;
                // Control.Layer.CornerRadius = Convert.ToSingle(view.CornerRadius);
                // Control.Layer.BorderColor = view.BorderColor.ToCGColor();
                // Control.Layer.BorderWidth = view.BorderWidth;
                Control.ClipsToBounds = true;
                //  UIMenuController.SharedMenuController.SetMenuVisible(false, false);
            }
        }
    }

}