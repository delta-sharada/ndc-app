using System;

using Xamarin.Forms;

namespace Fly360
{
    public class SearchPage : ContentPage
    {
        public SearchPage()
        {
            Title = "Search Flight";
            NavigationPage.SetBackButtonTitle(this, string.Empty);
            ToolbarItems.Add(new ToolbarItem("Cancel", null, async () =>
            {
                await Navigation.PopAsync();
            }));

            var image = new BoxView
            {
                HeightRequest = 275,
                BackgroundColor = Color.Accent
            };

            var label = new ShadowLabel
            {
                Text = "Honolulu",
                Margin = 20,
                FontSize = 24,
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.White,
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.End
            };

            var grid = new Grid
            {
                RowDefinitions = {
                    new RowDefinition { Height = GridLength.Star },
                    new RowDefinition { Height = new GridLength(2, GridUnitType.Star)  }
                }
            };

            grid.Children.Add(image);
            grid.Children.Add(label);

            Content = grid;
        }

        public class ShadowLabel : Label
        {
            public ShadowLabel()
            {
                Effects.Add(new ShadowEffect
                {
                    Radius = 5,
                    Color = Color.Black,
                    DistanceX = 5,
                    DistanceY = 5
                });
            }
        }
    }
}

