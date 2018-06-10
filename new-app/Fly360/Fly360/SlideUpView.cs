using System;
using System.Runtime.CompilerServices;
using SlideOverKit;
using Xamarin.Forms;

namespace Fly360
{
    public class SlideUpView : SlideMenuView
    {
        public event EventHandler OnTapped;

        public SlideUpView()
        {
            IsFullScreen = true;
            MenuOrientations = MenuOrientation.BottomToTop;
            BackgroundViewColor = Color.Transparent;
            BackgroundColor = Color.Transparent;
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            var extras = new ExtrasView
            {
                Keys = new Keys
                {
                    Mode = "2",
                    SeatImage = "upgradeSeats.png",
                    SeatUrl = "https://fly360.github.io/content/luxuryVR.html",
                    SeatIcon = "iconVR.png",
                    SkyImage = "skyClub.png",
                    SkyUrl = "https://fly360.github.io/content/skyclubVR.html",
                    SkyIcon = "iconVR.png",
                },
                HeightRequest = this.HeightRequest - DraggerButtonHeight,
                BackgroundColor = Color.White,
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill
            };

            var upgrade = new Image
            {
                Source = ImageSource.FromResource("upgradeIcon.png"),
                HeightRequest = 20,
                WidthRequest = 40,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };

            var price = new ShadowLabel
            {
                FontSize = 18,
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.White,
                HorizontalTextAlignment = TextAlignment.Start,
                VerticalOptions = LayoutOptions.Center,
                Margin = new Thickness(15, 10),
                Text = "+ $50",
            };

            var mainLayout = new StackLayout
            {
                Spacing = 0,
                Children = {
                    new StackLayout {
                        HeightRequest = 40,
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.Center,
                        BackgroundColor = Color.FromHex("#5887F9"),
                        Children = {
                            upgrade, 
                            price
                        },
                        Orientation = StackOrientation.Horizontal,
                        GestureRecognizers = {
                            new TapGestureRecognizer(async (obj) =>
                            {
                                this.OnTapped?.Invoke(this, EventArgs.Empty);
                            })
                        }
                    },
                    extras
                }
            };

            Content = mainLayout;
        }
    }
}

