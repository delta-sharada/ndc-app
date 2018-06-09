using System;
using System.Threading.Tasks;
using Urho.Forms;
using Xamarin.Forms;
using Rg.Plugins.Popup.Extensions;

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

            urhoSurface = new UrhoSurface();
            urhoSurface.VerticalOptions = LayoutOptions.FillAndExpand;
            Grid.SetRowSpan(urhoSurface, 4);

            var profileView = GetProfileView();
            Grid.SetRow(profileView, 3);

            popupPage = new MenuPopupPage();
            popupPage.Appearing += (s, a) =>
            {
                entry.Opacity = 0.2f;
                urhoSurface.Opacity = 0.2f;
                profileView.Opacity = 0.2f;
            };
            popupPage.Disappearing += (s, a) =>
            {
                entry.Opacity = 1f;
                urhoSurface.Opacity = 1f;
                profileView.Opacity = 1f;
            };


            Content = new Grid
            {
                RowDefinitions = {
                    new RowDefinition { Height = 15 },
                    new RowDefinition { Height = 40 },
                    new RowDefinition { Height = GridLength.Star },
                    new RowDefinition { Height = 80 }
                },
                Children = {
                    urhoSurface,
                    entry,
                    profileView,
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

        void UrhoApp_CitySelected(object sender, string id)
        {
            urhoApp.CitySelected -= UrhoApp_CitySelected;
            Device.BeginInvokeOnMainThread(() =>
            {
                Navigation.PushAsync(new SearchPage {
                    Code = id.ToUpper()
                });
            });
        }



        View GetProfileView()
        {
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
                VerticalOptions = LayoutOptions.End
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
            Grid.SetColumnSpan(box, 3);

            var button = new Button
            {
                BackgroundColor = Color.FromHex("#F2F6F8"),
                TextColor = Color.FromHex("#5887F9"),
                Text = "Trips",
                HorizontalOptions = LayoutOptions.Center,
                Margin = new Thickness(10, 25),
                WidthRequest = 150,
                CornerRadius = 5,
            };
            Grid.SetRow(button, 0);
            Grid.SetRowSpan(button, 2);
            Grid.SetColumn(button, 2);

            button.Clicked += async (s, a) =>
            {
                await Navigation.PushPopupAsync(popupPage);
            };

            var grid = new Grid
            {
                ColumnDefinitions = {
                    new ColumnDefinition { Width = 75 },
                    new ColumnDefinition(),
                    new ColumnDefinition { Width = 100 },
                },
                RowDefinitions = {
                    new RowDefinition { Height = 40 },
                    new RowDefinition { Height = GridLength.Star }
                },
                Children = {
                    box,
                    profile,
                    name,
                    role,
                    button
                }
            };

            return grid;
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
                Source = ImageSource.FromResource("search_icon.png"),
                Margin = 10
            };

            var mic = new Image
            {
                Source = ImageSource.FromResource("mic_icon.png"),
                Margin = 10
            };
            Grid.SetColumn(mic, 2);

            Children.Add(frame);
            Children.Add(mic);
            Children.Add(search);
            Children.Add(entry);
        }
    }

    public class ShadowLabel : Label
    {
        public ShadowLabel()
        {
            Effects.Add(new ShadowEffect
            {
                Radius = 2,
                Color = Color.Black,
                DistanceX = 2,
                DistanceY = 2
            });
        }
    }
}

