using System;

using Xamarin.Forms;
using static Fly360.SearchPage;

namespace Fly360
{
    public class ProfilePage : ContentPage
    {
        public ProfilePage()
        {
            Title = "Fly 360";
            NavigationPage.SetBackButtonTitle(this, string.Empty);

            var profile = new Image
            {
                Source = ImageSource.FromResource("Fly360.images.profile.png"),
                Aspect = Aspect.AspectFit,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Margin = 5
            };
            Grid.SetRowSpan(profile, 2);

            var name = new ShadowLabel
            {
                Text = "Jane Doe",
                FontSize = 24,
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.White,
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.Start
            };
            Grid.SetColumn(name, 1);

            var role = new Label
            {
                FontSize = 16,
                TextColor = Color.White,
                Text = "Consultant (Grade: B)",
                HorizontalTextAlignment = TextAlignment.Start
            };
            Grid.SetColumn(role, 1);
            Grid.SetRow(role, 1);

            var box = new BoxView
            {
                Color = Color.FromHex("#5579F7")
            };

            Grid.SetRowSpan(box, 2);
            Grid.SetColumnSpan(box, 2);

            var grid = new Grid
            {
                ColumnDefinitions = {
                    new ColumnDefinition { Width = 75 },
                    new ColumnDefinition(),
                },
                RowDefinitions = {
                    new RowDefinition { Height = 40 },
                    new RowDefinition { Height = GridLength.Star }
                },
                Children = {
                    box,
                    profile, 
                    name,  
                    role
                }
            };

            Content = grid;
        }
    }
}

