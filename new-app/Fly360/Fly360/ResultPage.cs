using System;
using System.Linq;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Extensions;
using SlideOverKit;
using Xamarin.Forms;

namespace Fly360
{
    public class TripData
    {
        public SegmentData[] Legs { get; set; }

        public string HotelImg1 { get; set; }
        public string HotelImg2 { get; set; }

        public string CarImg1 { get; set; }
        public string CarImg2 { get; set; }
    }

    public class SegmentData
    {
        public string DeptTime { get; set; }
        public string ArrvTime { get; set; }
        public string DeptCode { get; set; }
        public string ArrvCode { get; set; }

        public string Duration { get; set; }
        public string StopDesc { get; set; }
        public string FlightNbr { get; set; }
    }

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

        public static Ancillaries[] Ancillaries;

        public static TripData[] Trips = new TripData[] {
            new TripData {
                Legs = new SegmentData[] {
                    new SegmentData {
                        DeptCode = "EWR",
                        ArrvCode = "LHR",
                        DeptTime = "Jul 16, 2018 7pm",
                        ArrvTime = "Jul 17, 2018 7:10am",
                        Duration = "7h 10m",
                        StopDesc = "Non-stop",
                        FlightNbr = "0014"
                    },
                    new SegmentData {
                        DeptCode = "LHR",
                        ArrvCode = "EWR",
                        DeptTime = "Jul 29, 2018 7:55am",
                        ArrvTime = "Jul 29, 2018 10:55am",
                        Duration = "7h 22m",
                        StopDesc = "Non-stop",
                        FlightNbr = "0883"
                    }
                },

                HotelImg1 = "1",
                HotelImg2 = "2",
                CarImg1 = "1",
                CarImg2 = "2",

             },
            new TripData {
                Legs = new SegmentData[] {
                    new SegmentData {
                        DeptCode = "EWR",
                        ArrvCode = "LHR",
                        DeptTime = "Jul 16, 2018 9:00pm",
                        ArrvTime = "Jul 17, 2018 9:20am",
                        Duration = "7h 20m",
                        StopDesc = "Non-stop",
                        FlightNbr = "0016"
                    },
                    new SegmentData {
                        DeptCode = "LHR",
                        ArrvCode = "EWR",
                        DeptTime = "Jul 29, 2018 6:30am",
                        ArrvTime = "Jul 29, 2018 9:05am",
                        Duration = "7h 25m",
                        StopDesc = "Non-stop",
                        FlightNbr = "0883"
                    }
                },

                HotelImg1 = "3",
                HotelImg2 = "4",
                CarImg1 = "1",
                CarImg2 = "3"
            },
            //new TripData {
            //    Legs = new SegmentData[] {
            //        new SegmentData {
            //            DeptCode = "EWR",
            //            ArrvCode = "LHR",
            //            DeptTime = "Jul 16, 2018 9:00pm",
            //            ArrvTime = "Jul 17, 2018 9:20am",
            //            Duration = "7h 20m",
            //            StopDesc = "Non-stop",
            //            FlightNbr = "0016"
            //        },
            //        new SegmentData {
            //            DeptCode = "LHR",
            //            ArrvCode = "EWR",
            //            DeptTime = "Jul 29, 2018 6:30am",
            //            ArrvTime = "Jul 29, 2018 9:05am",
            //            Duration = "7h 25m",
            //            StopDesc = "Non-stop",
            //            FlightNbr = "0883"
            //        }
            //    },

            //    HotelImg1 = "5",
            //    HotelImg2 = "6",
            //    CarImg1 = "1",
            //    CarImg2 = "2"
            //},
            new TripData {
                Legs = new SegmentData[] {
                    new SegmentData {
                        DeptCode = "EWR",
                        ArrvCode = "LHR",
                        DeptTime = "Jul 16, 2018 11pm",
                        ArrvTime = "Jul 17, 2018 11:20am",
                        Duration = "7h 20m",
                        StopDesc = "Non-stop",
                        FlightNbr = "0020"
                    },
                    new SegmentData {
                        DeptCode = "LHR",
                        ArrvCode = "EWR",
                        DeptTime = "Jul 29, 2018 8:30am",
                        ArrvTime = "Jul 29, 2018 8:50am",
                        Duration = "7h 20m",
                        StopDesc = "Nos-stop",
                        FlightNbr = "0887"
                    }
                },

                HotelImg1 = "7",
                HotelImg2 = "8",
                CarImg1 = "1",
                CarImg2 = "3"
            }
        }; 
    }

    public class ResultPage : MenuContainerPage
    {
        protected async override void OnAppearing()
        {
            base.OnAppearing();

            var loadingPage = new LoadingPopupPage();
            await Navigation.PushPopupAsync(loadingPage);
            var ancillaries = await Loader.GetList();

            await Navigation.RemovePopupPageAsync(loadingPage);
            if (ancillaries == null)
                await Navigation.PopAsync();
            else
            {
                var filtered = ancillaries.Where(x => x.Type == "upa" && x.Attributes != null)
                                          .Where(y => y.Attributes.SmallIconUrl != null);

                H.Ancillaries = filtered.ToArray();
                carousalView.ItemsSource = H.Trips;
            }
        }


        CarouselView carousalView;
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

            carousalView = new CarouselView
            {
                ItemTemplate = new DataTemplate(typeof(BundleView)),
            };

            var btmBar = new BoxView { Color = Color.White };
            Grid.SetRow(btmBar, 1);

            Content = new Grid{
                RowDefinitions = {
                    new RowDefinition(),
                    new RowDefinition { Height = 30 }
                },
                Children = {
                    carousalView,
                    btmBar
                }
            };

            var upView = new SlideUpView()
            {
                HeightRequest = 440,
                DraggerButtonHeight = 40,
            };

            upView.SetBinding(BindingContextProperty, new Binding(path: "Item", source: carousalView));

            upView.OnTapped += (sender, e) => {
                if (upView.IsShown)
                    HideMenu();
                else
                    ShowMenu();
            };

            this.SlideMenu = upView;
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
        public string Mode = "1";
        public string SeatImage = "expressSeat.jpg";
        public string SeatUrl = "https://fly360.github.io/content/tn-economy/index.html";
        public string SeatIcon = "icon360.png";
        public string SkyImage = "skyClub.png";
        public string SkyUrl = "https://fly360.github.io/content/skyclubVR.html";
        public string SkyIcon = "iconVR.png";
    }

    public class ExtrasView : Grid
    {
        public Keys Keys = new Keys();
        View hotelToggle;
        View carToggle;

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

            Margin = RowSpacing; 
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (BindingContext is TripData trip)
            {
                Children.Clear();
                var ssr = new FlexLayout { 
                    BackgroundColor = Color.FromHex("#F7F7F7"),
                    Padding = 25, 

                    Wrap = FlexWrap.Wrap, 
                    JustifyContent = FlexJustify.Center | FlexJustify.SpaceAround
                };
                foreach (var a in H.Ancillaries)
                    ssr.Children.Add(new Image
                    {
                        HeightRequest = 45,
                        WidthRequest = 45,
                        Source = ImageSource.FromUri(new Uri(a.Attributes.SmallIconUrl)),
                        Margin = 5,
                        Aspect = Aspect.AspectFill
                    });

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

                var hotelView = (Keys.Mode == "1")
                    ? GetButton(2, trip.HotelImg1)
                    : GetButton(2, trip.HotelImg2);
                
                var carView = (Keys.Mode == "1")
                    ? GetButton(1, trip.CarImg1)
                    : GetButton(1, trip.CarImg2);

                SetColumnSpan(ssr, 3);
                Children.Add(ssr);

                SetRow(seatView, 1);
                SetColumnSpan(seatView, 2);
                Children.Add(seatView);

                SetRow(lounge, 2);
                SetColumnSpan(lounge, 2);
                Children.Add(lounge);

                SetRow(hotelView, 1);
                SetColumn(hotelView, 2);
                Children.Add(hotelView);

                SetRow(carView, 2);
                SetColumn(carView, 2);
                Children.Add(carView);

            }
        }

        View GetButton(int index, string id)
        {
            var btn = new Button
            {
                Text = string.Empty,

                BackgroundColor = Color.White,
                BorderColor = Color.FromHex("#7BABFC"),
                BorderWidth = 2,
                CornerRadius = 10,
            };

            var lbl = new Label
            {
                Text = index == 2 ? "Add hotel" : "Add car",
                TextColor = Color.FromHex("#444444"),
                FontSize = 14,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalOptions = LayoutOptions.End,
                Margin = 10
            };

            var iconType = index == 2 ? "hotelIcon" : "carIcon";
            var img = new Image
            {
                Source = ImageSource.FromResource(string.Format("Fly360.images.{0}.png", iconType)),
                WidthRequest = 48,
                HeightRequest = 58,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Start,
                Margin = 25
            };

            if(index == 2)
                hotelToggle =  new Image
                {
                    Source = ImageSource.FromResource(string.Format("Fly360.images.hotels.{0}.png", id)),
                    Aspect = Aspect.AspectFit,
                    HorizontalOptions = LayoutOptions.Fill,
                    VerticalOptions = LayoutOptions.Fill,
                };
            else
                carToggle = new Image
                {
                    Source = ImageSource.FromResource(string.Format("Fly360.images.cars.{0}.jpg", id)),
                    Aspect = Aspect.AspectFit,
                    HorizontalOptions = LayoutOptions.Fill,
                    VerticalOptions = LayoutOptions.Fill,
                };

            if(index == 2)
            {
                btn.Clicked += (sender, e) => OnHClicked();
                lbl.GestureRecognizers.Add(new TapGestureRecognizer((v, a) => OnHClicked()));
                img.GestureRecognizers.Add(new TapGestureRecognizer((v, a) => OnHClicked()));
                hotelToggle.GestureRecognizers.Add(new TapGestureRecognizer((v, a) => OnHClicked()));
            }
            else
            {
                btn.Clicked += (sender, e) => OnCClicked();
                lbl.GestureRecognizers.Add(new TapGestureRecognizer((v, a) => OnCClicked()));
                img.GestureRecognizers.Add(new TapGestureRecognizer((v, a) => OnCClicked()));
                carToggle.GestureRecognizers.Add(new TapGestureRecognizer((v, a) => OnCClicked()));
            }


            return new Grid {
                Children = {
                    btn, img, lbl, (index == 2) ? hotelToggle : carToggle
                }
            };
        }

        private void OnCClicked()
        {
            if (carToggle != null)
                carToggle.IsVisible = !carToggle.IsVisible;
        }

        private void OnHClicked()
        {
            if (hotelToggle != null)
                hotelToggle.IsVisible = !hotelToggle.IsVisible;
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

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if(BindingContext is TripData trip)
            {
                Children[0].BindingContext = trip.Legs[0];
                Children[1].BindingContext = trip.Legs[1];
            }
        }
    }

    public class SegmentView : Grid
    {
        public SegmentView()
        {
            RowSpacing = 3;
            HeightRequest = 95;

            ColumnDefinitions.Add(new ColumnDefinition { Width = 40 });
            ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(5, GridUnitType.Star) });
            ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) });
            ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(5, GridUnitType.Star) });

            RowDefinitions.Add(new RowDefinition());
            RowDefinitions.Add(new RowDefinition());
            RowDefinitions.Add(new RowDefinition());
            //RowDefinitions.Add(new RowDefinition());
            //RowDefinitions.Add(new RowDefinition());

        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if(BindingContext is SegmentData item)
            {
                var code = "ua";
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
                    Text = "United Airlines",
                    HorizontalOptions = LayoutOptions.Fill,
                    HorizontalTextAlignment = TextAlignment.Start,
                    VerticalOptions = LayoutOptions.Start
                };

                Grid.SetRow(airlineName, 2);

                var deptTime = new Label
                {
                    FontSize = 12,
                    TextColor = Color.FromHex("#444444"),
                    Text = item.DeptTime,
                    LineBreakMode = LineBreakMode.WordWrap,
                    VerticalOptions = LayoutOptions.Start
                };
                Grid.SetRow(deptTime, 1);
                Grid.SetColumn(deptTime, 1);

                var deptCode = new Label
                {
                    FontSize = 16,
                    TextColor = Color.FromHex("#444444"),
                    Text = item.DeptCode,
                    VerticalOptions = LayoutOptions.End
                };
                Grid.SetRow(deptCode, 0);
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
                    FontSize = 12,
                    TextColor = Color.FromHex("#444444"),
                    LineBreakMode = LineBreakMode.WordWrap,
                    Text = item.ArrvTime,
                    HorizontalTextAlignment = TextAlignment.End,
                    VerticalOptions = LayoutOptions.Start
                };
                Grid.SetRow(arrTime, 1);
                Grid.SetColumn(arrTime, 3);

                var arrvCode = new Label
                {
                    FontSize = 16,
                    TextColor = Color.FromHex("#444444"),
                    Text = item.ArrvCode,
                    HorizontalTextAlignment = TextAlignment.End,
                    VerticalOptions = LayoutOptions.End
                };
                Grid.SetRow(arrvCode, 0);
                Grid.SetColumn(arrvCode, 3);

                var duration = new Label
                {
                    Text = item.Duration,
                    FontSize = 12,
                    TextColor = Color.FromHex("#444444"),
                    HorizontalTextAlignment = TextAlignment.Center,
                    VerticalOptions = LayoutOptions.End
                };
                Grid.SetRow(duration, 1);
                Grid.SetColumn(duration, 2);

                var stops = new Label
                {
                    Text = item.StopDesc,
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
                    Text = item.FlightNbr,
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
}

