using System;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Extensions;
using Urho.Forms;
using Xamarin.Forms;

namespace Fly360
{
    public class MainPage : ContentPage
    {
        UrhoSurface urhoSurface;
        EarthGlobeView urhoApp;
        MenuPopupPage popupPage;

        public MainPage()
        {
            Title = "Fly 360";
            NavigationPage.SetBackButtonTitle(this, string.Empty);

            BackgroundColor = Color.Black;

            var entry = new IconEntry
            {
                Margin = new Thickness(25, 0),
            };

            Grid.SetRow(entry, 1);
            var button = new Button
            {
                BackgroundColor = Color.FromHex("#F2F6F8"),
                TextColor = Color.FromHex("#5887F9"),
                Text = "Suggested Destinations",
                HorizontalOptions = LayoutOptions.Center,
                Margin = new Thickness(40, 0),
                WidthRequest = 250,
                CornerRadius = 5
            };
            Grid.SetRow(button, 3);
            button.Clicked += async (s, a) =>
            {
                await Navigation.PushPopupAsync(popupPage);
            };

            urhoSurface = new UrhoSurface();
            urhoSurface.VerticalOptions = LayoutOptions.FillAndExpand;
            Grid.SetRowSpan(urhoSurface, 5);

            popupPage = new MenuPopupPage();
            popupPage.Appearing += (s, a) =>
            {
                entry.Opacity = 0.25f;
                button.IsVisible = false;
            };
            popupPage.Disappearing += (s, a) =>
            {
                entry.Opacity = 1f;
                button.IsVisible = true;
            };

            Content = new Grid
            {
                RowDefinitions = {
                    new RowDefinition { Height = 15 },
                    new RowDefinition { Height = 40 },
                    new RowDefinition { Height = GridLength.Star },
                    new RowDefinition { Height = 40 },
                    new RowDefinition { Height = 40 }
                },
                Children = {
                    urhoSurface,
                    entry,
                    button,
                }
            };
        }

        protected override void OnDisappearing()
        {
            UrhoSurface.OnDestroy();
            base.OnDisappearing();
        }

        protected override async void OnAppearing()
        {
            await StartUrhoApp();
            urhoApp.CitySelected += UrhoApp_CitySelected;
        }

        async Task StartUrhoApp()
        {
            urhoApp = await urhoSurface.Show<EarthGlobeView>(
                new Urho.ApplicationOptions(assetsFolder: "Data")
                {
                    Orientation = Urho.ApplicationOptions.OrientationType.LandscapeAndPortrait
                });
        }

        void UrhoApp_CitySelected(object sender, EventArgs e)
        {
            urhoApp.CitySelected -= UrhoApp_CitySelected;
            Device.BeginInvokeOnMainThread(() =>
            {
                Navigation.PushAsync(new HawaiiDetailsPage());
            });
        }

    }

    public class CustomEntry : Entry
    {
        public CustomEntry()
        {
            Effects.Add(new EntryEffect());
        }
    }

    public class IconEntry : Grid
    {
        public IconEntry()
        {

            ColumnDefinitions.Add(new ColumnDefinition { Width = 36 });
            ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
            ColumnDefinitions.Add(new ColumnDefinition { Width = 36 });

            var entry = new CustomEntry
            {
                BackgroundColor = Color.Transparent,
                HorizontalOptions = LayoutOptions.Fill,
                TextColor = Color.White,
                Placeholder = "Search",
                WidthRequest = 300,
                PlaceholderColor = Color.FromHex("#8E8E93"),
            };
            Grid.SetColumn(entry, 1);

            var frame = new Frame
            {
                BackgroundColor = Color.FromHex("#E8E9EA"),
                BorderColor = Color.FromHex("#E8E9EA"),
                CornerRadius = 15,
                Opacity = 0.33
            };
            Grid.SetColumnSpan(frame, 3);

            var search = new Image
            {
                Source = ImageSource.FromResource("Fly360.images.search_icon.png"),
                Margin = 10
            };

            var mic = new Image
            {
                Source = ImageSource.FromResource("Fly360.images.mic_icon.png"),
                Margin = 10
            };
            Grid.SetColumn(mic, 2);

            Children.Add(frame);
            Children.Add(mic);
            Children.Add(search);
            Children.Add(entry);
        }
    }

}

