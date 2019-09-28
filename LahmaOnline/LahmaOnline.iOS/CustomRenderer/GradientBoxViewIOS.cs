using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CoreAnimation;
using CoreGraphics;
using LahmaOnline.CustomRenderer;
using LahmaOnline.iOS.CustomRenderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(GradientBoxView), typeof(GradientBoxViewRenderer))]
namespace LahmaOnline.iOS.CustomRenderers
{
    using CoreAnimation;
    using CoreGraphics;
    using Xamarin.Forms.Platform.iOS;

    public class GradientBoxViewRenderer : VisualElementRenderer<GradientBoxView>
    {
        public override void Draw(CGRect rect)
        {
            base.Draw(rect);

            var topColor = Element.StartColor.ToCGColor();
            var bottomColor = Element.EndColor.ToCGColor();
            var gradientLayer = new CAGradientLayer
            {
                StartPoint = new CGPoint(0, 0.5),
                EndPoint = new CGPoint(1, 0.5),
                Frame = rect,
                Colors = new CGColor[] { topColor, bottomColor },
                
            };
            NativeView.Layer.InsertSublayer(gradientLayer, 0);
        }
    }
}