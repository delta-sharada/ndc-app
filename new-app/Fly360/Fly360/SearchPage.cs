using System;

using Xamarin.Forms;

namespace Fly360
{
    public class SearchPage : ContentPage
    {
        public string Code { get; set; }
        public SearchPage()
        {
            Title = "Search Flight";
            NavigationPage.SetBackButtonTitle(this, string.Empty);
            ToolbarItems.Add(new ToolbarItem("Cancel", null, async () =>
            {
                await Navigation.PopToRootAsync();
            }));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            var image = new Image
            {
                HeightRequest = 275,
                Source = ImageSource.FromResource("office.jpg"),
                Aspect = Aspect.AspectFill
            };

            var label = new ShadowLabel
            {
                Text = $"Callister, Inc. ({Code})",
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

            var searchForm = new SearchForm(Code)
            {
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.Fill
            };
            Grid.SetRow(searchForm, 1);

            grid.Children.Add(image);
            grid.Children.Add(label);
            grid.Children.Add(searchForm);

            Content = grid;
        }

        public class SearchForm : Grid
        {
            public SearchForm(string toCode)
            {
                ColumnSpacing = 25;
                RowSpacing = 25;

                ColumnDefinitions.Add(new ColumnDefinition());
                ColumnDefinitions.Add(new ColumnDefinition());

                RowDefinitions.Add(new RowDefinition());
                RowDefinitions.Add(new RowDefinition());
                RowDefinitions.Add(new RowDefinition());
                RowDefinitions.Add(new RowDefinition());
                RowDefinitions.Add(new RowDefinition());

                Margin = 20;

                var fromEntry = new SearchEntry(ImageSource.FromResource("Fly360.images.deptIcon.png"))
                {
                    VerticalOptions = LayoutOptions.Start,
                    Text = "ATL"
                };

                var toEntry = new SearchEntry(ImageSource.FromResource("Fly360.images.arrvIcon.png"))
                {
                    VerticalOptions = LayoutOptions.Start,
                    Text = toCode
                };
                Grid.SetColumn(toEntry, 1);

                var startEntry = new SearchEntry(ImageSource.FromResource("Fly360.images.startIcon.png"), true)
                {
                    VerticalOptions = LayoutOptions.Start,
                    Text = DateTime.Today.AddDays(7).ToString("MM/dd/yyyy")
                };
                Grid.SetRow(startEntry, 1);
                Grid.SetColumn(startEntry, 0);

                var endEntry = new SearchEntry(ImageSource.FromResource("Fly360.images.endIcon.png"), true)
                {
                    VerticalOptions = LayoutOptions.Start,
                    Text = DateTime.Today.AddDays(10).ToString("MM/dd/yyyy")
                };
                Grid.SetRow(endEntry, 1);
                Grid.SetColumn(endEntry, 1);

                var countEntry = new ExtendedSearchEntry(ImageSource.FromResource("Fly360.images.passengerIcon.png"), "adult")
                {
                    VerticalOptions = LayoutOptions.Start,
                    Text = "1"
                };
                Grid.SetRow(countEntry, 2);
                Grid.SetColumn(countEntry, 0);

                var seatEntry = new SearchEntry(ImageSource.FromResource("Fly360.images.seatIcon.png"))
                {
                    VerticalOptions = LayoutOptions.Start,
                    Text = "Economy",

                };
                Grid.SetRow(seatEntry, 2);
                Grid.SetColumn(seatEntry, 1);

                var searchBtn = new Button
                {
                    Text = "Search",
                    BackgroundColor = Color.FromHex("#5579F7"),
                    //FontSize = 12,
                    TextColor = Color.FromHex("#F2F6F8"),
                    CornerRadius = 5,
                    HeightRequest = 40,
                    VerticalOptions = LayoutOptions.Start
                };
                Grid.SetRow(searchBtn, 3);
                Grid.SetColumnSpan(searchBtn, 2);
                searchBtn.Clicked += async (sender, e) => {
                    await Navigation.PushAsync(new ResultPage());
                };

                Children.Add(fromEntry);
                Children.Add(toEntry);
                Children.Add(startEntry);
                Children.Add(endEntry);
                Children.Add(countEntry);
                Children.Add(seatEntry);
                Children.Add(searchBtn);
            }
        }

        public class SearchEntry : Grid
        {
            public static readonly BindableProperty TextProperty = BindableProperty.Create(
                "Text", typeof(string), typeof(SearchEntry));

            public string Text
            {
                get { return (string)GetValue(TextProperty); }
                set { SetValue(TextProperty, value); }
            }


            protected Entry _entry;
            protected BoxView _border;
            public SearchEntry(ImageSource icon, bool addPadding = false)
            {
                HorizontalOptions = LayoutOptions.Fill;
                _border = new BoxView
                {
                    HeightRequest = 1,
                    Color = Color.FromHex("#CCCCCC"),
                    VerticalOptions = LayoutOptions.End,
                    HorizontalOptions = LayoutOptions.Fill
                };
                Grid.SetColumnSpan(_border, 3);

                var image = new Image
                {
                    Source = icon,
                    VerticalOptions = LayoutOptions.Start,
                    HorizontalOptions = LayoutOptions.Start,
                    HeightRequest = addPadding ? 21 : 24,
                    WidthRequest = addPadding ? 21 : 24,
                    Margin = addPadding ? 3 : 0
                };

                ColumnSpacing = 10;
                _entry = new CustomEntry
                {
                    Text = "ATL",
                    IsSpellCheckEnabled = false,
                    WidthRequest = 115,
                    VerticalOptions = LayoutOptions.Start,
                    HorizontalOptions = LayoutOptions.Fill,
                    HorizontalTextAlignment = TextAlignment.Start,

                    TextColor = Color.FromHex("#444444")
                };
                _entry.SetBinding(Entry.TextProperty, new Binding(path: "Text", source: this));

                Grid.SetColumn(_entry, 1);

                Children.Add(image);
                Children.Add(_border);
                Children.Add(_entry);

                ColumnDefinitions.Add(new ColumnDefinition { Width = 24 });
                ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });

                HeightRequest = 33;
            }
        }

        public class ExtendedSearchEntry : SearchEntry
        {
            public ExtendedSearchEntry(ImageSource icon, string suffix) : base(icon)
            {

                _entry.WidthRequest = 15;
                var lbl = new Label
                {
                    HeightRequest = 24,
                    Text = suffix,
                    VerticalOptions = LayoutOptions.Start,
                    HorizontalOptions = LayoutOptions.Fill,
                    HorizontalTextAlignment = TextAlignment.Start,
                    TextColor = Color.FromHex("#444444")
                };
                Grid.SetColumn(lbl, 2);
                Children.Add(lbl);
            }
        }
    }
}

