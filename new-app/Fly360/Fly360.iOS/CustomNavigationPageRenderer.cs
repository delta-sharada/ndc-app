using System;
using CoreAnimation;
using CoreGraphics;
using Fly360;
using Fly360.Droid;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomNavigationPage), typeof(CustomNavigationPageRenderer))]
namespace Fly360.Droid
{
    public class CustomNavigationPageRenderer : NavigationRenderer
    {
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();


            var gradientLayer = new CAGradientLayer();
            gradientLayer.NeedsDisplayOnBoundsChange = true;

            var b = NavigationBar.Bounds;
            gradientLayer.Bounds = new CGRect(b.X, b.Y, b.Width, b.Height + 20);

            gradientLayer.Colors = new CGColor[] { Color.FromHex("#60C3FF").ToCGColor(), 
                Color.FromHex("#5574F7").ToCGColor() };

            float x1 = 0f, x2 = 1f, y1 = 0f, y2 = 1f;
            x1 = x2 = 0.5f;
            gradientLayer.StartPoint = new CGPoint(x1, y1);
            gradientLayer.EndPoint = new CGPoint(x2, y2);
           
            UIGraphics.BeginImageContext(gradientLayer.Bounds.Size);
            gradientLayer.RenderInContext(UIGraphics.GetCurrentContext());
            UIImage image = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();

            NavigationBar.SetBackgroundImage(image, UIBarMetrics.Default);
        }
    }
}

