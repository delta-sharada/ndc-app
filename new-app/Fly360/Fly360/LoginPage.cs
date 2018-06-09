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
            loginBtn.Clicked += (sender, e) => 
                Application.Current.MainPage 
                    = new CustomNavigationPage(new MainPage());

            Content = new Grid
            {
                Children = {
                    new Image { 
                        Source = ImageSource.FromResource("splash.png"),
                        Aspect = Aspect.AspectFill,
                        Opacity = 0.9
                    },
                    loginBtn
                }
            };
        }
    }
}

