using System;

using Xamarin.Forms;

namespace Fly360
{
    public class LoginPage : ContentPage
    {
        public LoginPage()
        {
            Title = "Fly 360";
            NavigationPage.SetBackButtonTitle(this, string.Empty);
            BackgroundColor = Color.Black;

            var loginBtn = new Button
            {
                Text = "Login with OpenID",
                BackgroundColor = Color.FromHex("#5579F7"),
                TextColor = Color.FromHex("#F2F6F8"),
                CornerRadius = 5,
                HeightRequest = 40,
                Margin = 75,
                VerticalOptions = LayoutOptions.End
            };
            loginBtn.Clicked += async (sender, e) => await Navigation.PushAsync(new ProfilePage());

            var imgEq = new Image
            {
                Source = ImageSource.FromResource("Fly360.images.iconEqBlack.png"),
                Aspect = Aspect.AspectFit,
                WidthRequest = 150,
                Margin = new Thickness(0, 115, 0, 0),
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Opacity = .75
            };

            var imgPlane = new Image
            {
                Source = ImageSource.FromResource("Fly360.images.flightIconYellow.png"),
                Aspect = Aspect.AspectFit,
                WidthRequest = 100,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Opacity = .75
            };

            var image = new Image
            {
                Source = ImageSource.FromResource("Fly360.images.splash.jpg"),
                Aspect = Aspect.AspectFill,
                Opacity = 0.75
            };

            Content = new Grid
            {
                Children = {
                    image,
                    imgPlane,
                    imgEq,
                    loginBtn
                }
            };
        }
    }
}

