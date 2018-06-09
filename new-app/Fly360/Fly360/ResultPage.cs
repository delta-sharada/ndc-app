using System;
using SlideOverKit;
using Xamarin.Forms;

namespace Fly360
{
    public static class H
    {
        public static void Launch(string url)
        {
            // We rely on the built-in service lcoator in this example, but you could just
            // as easily locate this service using DI and launch from your ViewModel
            var service = DependencyService.Get<INativeBrowserService>();
            {
                if (service == null) return;

                service.LaunchNativeEmbeddedBrowser(url);
            }
        }
    }
    public class ResultPage : MenuContainerPage
    {
        public ResultPage()
        {
            Title = "Select";
            NavigationPage.SetBackButtonTitle(this, string.Empty);
            ToolbarItems.Add(new ToolbarItem("Cancel", null, async () =>
            {
                await Navigation.PopToRootAsync();
            }));

            ToolbarItems.Add(new ToolbarItem("Book", null, async () =>
            {
                await Navigation.PushAsync(new SummaryPage());
            }));

            var carousalView = new CarouselView
            {
                ItemTemplate = new DataTemplate(typeof(BundleView)),
                ItemsSource = new string[] {
                    "1", "2", "3", "4" 
                },
            };

            Content = new Grid{
                RowDefinitions = {
                    new RowDefinition(),
                    new RowDefinition { Height = 40 }
                },
                Children = {
                    carousalView,
                }
            };

            SlideMenu = new SlideUpView()
            {
                HeightRequest = 440,
                DraggerButtonHeight = 40,
                BindingContext = "test"
            };
        }
    }

    public class BundleView : ContentView
    {
        public BundleView()
        {
            var tripView = new TripView();
            Grid.SetRowSpan(tripView, 2);

            var extras = new ExtrasView();
            Grid.SetRow(extras, 1);
            Grid.SetRowSpan(extras, 2);

            Content = new Grid
            {
                RowDefinitions = {
                    new RowDefinition { Height = 200 },
                    new RowDefinition(),
                    new RowDefinition { Height = 360 },
                },
                Children = {
                    tripView,
                    extras
                }
            };
        }
    }

    public enum LinkType { Image360, ImageVR };

    public class Keys
    {
        public string SeatImage = "expressSeat.jpg";
        public string SeatUrl = "https://fly360.github.io/content/sq-economy/index.html";
        public string SeatIcon = "icon360.png";
        public string SkyImage = "skyClub.png";
        public string SkyUrl = "https://fly360.github.io/content/skyclubVR.html";
        public string SkyIcon = "iconVR.png";
    }

    public class ExtrasView : Grid
    {
        public Keys Keys = new Keys();

        public ExtrasView()
        {
            RowDefinitions.Add(new RowDefinition());
            RowDefinitions.Add(new RowDefinition());
            RowDefinitions.Add(new RowDefinition());

            ColumnDefinitions.Add(new ColumnDefinition 
                { Width = new GridLength(4, GridUnitType.Star) });
            ColumnDefinitions.Add(new ColumnDefinition 
                { Width = new GridLength(3, GridUnitType.Star) });
            ColumnDefinitions.Add(new ColumnDefinition 
                { Width = new GridLength(3, GridUnitType.Star) });
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            var ssr = new BoxView { Color = Color.FromHex("#F7F7F7") };
            var seatView = new Grid()
            {
                Children = {
                    new Image
                    {
                        HeightRequest = 275,
                        Source = ImageSource.FromResource(Keys.SeatImage),
                        Aspect = Aspect.AspectFill
                    },
                    new Image
                    {
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.Center,
                        WidthRequest = 75, 
                        Source = ImageSource.FromResource(Keys.SeatIcon)
                    }
                },
                GestureRecognizers = {
                    new TapGestureRecognizer(async (obj) =>
                    {
                        H.Launch(Keys.SeatUrl);
                    })
                }
            };

            var lounge = new Grid()
            {
                Children = {
                    new Image
                    {
                        HeightRequest = 275,
                        Source = ImageSource.FromResource(Keys.SkyImage),
                        Aspect = Aspect.AspectFill
                    },
                    new Image
                    {
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.Center,
                        WidthRequest = 75, 
                        Source = ImageSource.FromResource(Keys.SkyIcon)
                    }
                },
                GestureRecognizers = {
                    new TapGestureRecognizer(async (obj) =>
                    {
                        H.Launch(Keys.SkyUrl);
                    })
                }
            };
            var hotelView = new BoxView { Color = Color.Red };
            var carView = new BoxView { Color = Color.Beige };

            SetColumnSpan(ssr, 3);
            Children.Add(ssr);

            SetRow(seatView, 1);
            Children.Add(seatView);

            SetRow(lounge, 2);
            Children.Add(lounge);

            SetRow(hotelView, 1);
            SetRowSpan(hotelView, 2);
            SetColumn(hotelView, 1);
            Children.Add(hotelView);

            SetRow(carView, 1);
            SetRowSpan(carView, 2);
            SetColumn(carView, 2);
            Children.Add(carView);


        }
    }

    public class TripView : StackLayout
    {
        public TripView()
        {
            Spacing = 3;
            Padding = 10;

            Children.Add(new SegmentView());
            Children.Add(new SegmentView());
        }
    }

    public class SegmentView : Grid
    {
        public SegmentView()
        {
            RowSpacing = 3;
            HeightRequest = 95;

            ColumnDefinitions.Add(new ColumnDefinition { Width = 50 });
            ColumnDefinitions.Add(new ColumnDefinition());
            ColumnDefinitions.Add(new ColumnDefinition());
            ColumnDefinitions.Add(new ColumnDefinition());

            RowDefinitions.Add(new RowDefinition());
            RowDefinitions.Add(new RowDefinition());
            RowDefinitions.Add(new RowDefinition());
            //RowDefinitions.Add(new RowDefinition());
            //RowDefinitions.Add(new RowDefinition());

        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();


            var id = BindingContext as string;

            var code = (id == "1") ? "aa" : (id == "2") ? "dl" : (id == "3") ? "ua" : "multi";
            var airline = new Image
            {
                Source = ImageSource.FromResource(code + ".png"),
                HeightRequest = 35,
                WidthRequest = 35,
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.Center
            };
            Grid.SetRowSpan(airline, 2);

            var airlineName = new Label
            {
                FontSize = 14,
                TextColor = Color.FromHex("#444444"),
                Text = "Delta",
                HorizontalOptions = LayoutOptions.Fill,
                HorizontalTextAlignment = TextAlignment.Start,
                VerticalOptions = LayoutOptions.Start
            };

            Grid.SetRow(airlineName, 2);

            var deptTime = new Label
            {
                FontSize = 14,
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.FromHex("#444444"),
                Text = "10:55 am",
                VerticalOptions = LayoutOptions.End
            };
            Grid.SetColumn(deptTime, 1);

            var deptCode = new Label
            {
                FontSize = 14,
                TextColor = Color.FromHex("#444444"),
                Text = "ATL",
                VerticalOptions = LayoutOptions.Start
            };
            Grid.SetRow(deptCode, 1);
            Grid.SetColumn(deptCode, 1);

            var flight = new Image
            {
                VerticalOptions = LayoutOptions.End,
                HorizontalOptions = LayoutOptions.Center,
                Source = ImageSource.FromResource("Fly360.images.flightIcon.png"),
                WidthRequest = 20,
                HeightRequest = 20
            };
            Grid.SetRow(flight, 0);
            Grid.SetColumn(flight, 2);

            var arrTime = new Label
            {
                FontSize = 14,
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.FromHex("#444444"),
                Text = "02:41 pm",
                HorizontalTextAlignment = TextAlignment.End,
                VerticalOptions = LayoutOptions.End
            };
            Grid.SetRow(arrTime, 0);
            Grid.SetColumn(arrTime, 3);

            var arrvCode = new Label
            {
                FontSize = 14,
                TextColor = Color.FromHex("#444444"),
                Text = "HNL",
                HorizontalTextAlignment = TextAlignment.End,
                VerticalOptions = LayoutOptions.Start
            };
            Grid.SetRow(arrvCode, 1);
            Grid.SetColumn(arrvCode, 3);

            var duration = new Label
            {
                Text = "9h 46m",
                FontSize = 12,
                TextColor = Color.FromHex("#444444"),
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalOptions = LayoutOptions.End                                       
            };
            Grid.SetRow(duration, 1);
            Grid.SetColumn(duration, 2);

            var stops = new Label
            {
                Text = "1 stop LAX 1h 4m",
                FontSize = 10,
                TextColor = Color.FromHex("#444444"),
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalOptions = LayoutOptions.Start
            };
            Grid.SetRow(stops, 2);
            Grid.SetColumn(stops, 2);

            var price = new Label
            {
                FontSize = 20,
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.FromHex("#444444"),
                HorizontalTextAlignment = TextAlignment.Start,
                VerticalOptions = LayoutOptions.End,
                Margin = new Thickness(15, 10),
                Text = "$345",
            };
            Grid.SetRow(price, 1);
            Grid.SetRowSpan(price, 2);
            Grid.SetColumnSpan(price, 2);
            Grid.SetColumn(price, 0);

            //var type = new Label
            //{
            //    Text = "Round Trip",
            //    FontSize = 10,
            //    TextColor = Color.FromHex("#444444"),
            //    HorizontalTextAlignment = TextAlignment.Start
            //};
            //Grid.SetRow(type, 4);
            //Grid.SetColumn(type, 1);

            //var expander = new Image
            //{
            //    Source = ImageSource.FromResource("Fly360.images.moreInfo.png"),
            //    HeightRequest = 35,
            //    WidthRequest = 25,
            //    HorizontalOptions = LayoutOptions.End,
            //    VerticalOptions = LayoutOptions.Center
            //};
            //Grid.SetRowSpan(expander, 2);
            //Grid.SetRow(expander, 3);
            //Grid.SetColumn(expander, 3);

            var seperator = new BoxView
            {
                HeightRequest = 1,
                Color = Color.FromHex("#CCCCCC"),
                VerticalOptions = LayoutOptions.End,
                HorizontalOptions = LayoutOptions.Fill
            };
            Grid.SetRow(seperator, 2);
            Grid.SetColumnSpan(seperator, 4);

            Children.Clear();
            Children.Add(airline);
            //Children.Add(airlineName);
            Children.Add(deptTime);
            Children.Add(deptCode);
            Children.Add(flight);
            Children.Add(arrTime);
            Children.Add(arrvCode);
            Children.Add(duration);
            Children.Add(stops);
            Children.Add(price);
            //Children.Add(type);
            //Children.Add(expander);
            Children.Add(seperator);
        }
    }
}

