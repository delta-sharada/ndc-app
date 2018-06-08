using System;

using Xamarin.Forms;

namespace Fly360
{
    public class ResultsPage : ContentPage
    {
        public ResultsPage()
        {
            Title = "Flight Results";
            NavigationPage.SetBackButtonTitle(this, string.Empty);
            ToolbarItems.Add(new ToolbarItem("Cancel", null, async () =>
            {
                await Navigation.PopToRootAsync();
            }));

            Content = new StackLayout
            {
                Children = {
                    new FlightResultView(),
                    new FlightResultView(),
                    new FlightResultView(),
                    new FlightResultView(),
                    new FlightResultView()
                },
                Margin = 15
            };
        }

        public class FlightResultView : Grid
        {
            public FlightResultView()
            {
                MinimumHeightRequest = 115;

                RowSpacing = 3;

                ColumnDefinitions.Add(new ColumnDefinition { Width = 50 });
                ColumnDefinitions.Add(new ColumnDefinition());
                ColumnDefinitions.Add(new ColumnDefinition());
                ColumnDefinitions.Add(new ColumnDefinition());

                RowDefinitions.Add(new RowDefinition());
                RowDefinitions.Add(new RowDefinition());
                RowDefinitions.Add(new RowDefinition());
                RowDefinitions.Add(new RowDefinition());
                RowDefinitions.Add(new RowDefinition());

                var airline = new Image
                {
                    Source = ImageSource.FromResource("dl.png"),
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
                    VerticalOptions = LayoutOptions.Center
                };

                Grid.SetRow(airlineName, 2);

                var deptTime = new Label
                {
                    FontSize = 14,
                    FontAttributes = FontAttributes.Bold,
                    TextColor = Color.FromHex("#444444"),
                    Text = "10:55 am",
                };
                Grid.SetColumn(deptTime, 1);

                var deptCode = new Label
                {
                    FontSize = 14,
                    TextColor = Color.FromHex("#444444"),
                    Text = "ATL",
                };
                Grid.SetRow(deptCode, 1);
                Grid.SetColumn(deptCode, 1);

                var flight = new Image
                {
                    VerticalOptions = LayoutOptions.Center,
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
                    HorizontalTextAlignment = TextAlignment.End
                };
                Grid.SetRow(arrTime, 0);
                Grid.SetColumn(arrTime, 3);

                var arrvCode = new Label
                {
                    FontSize = 14,
                    TextColor = Color.FromHex("#444444"),
                    Text = "HNL",
                    HorizontalTextAlignment = TextAlignment.End
                };
                Grid.SetRow(arrvCode, 1);
                Grid.SetColumn(arrvCode, 3);

                var duration = new Label
                {
                    Text = "9h 46m",
                    FontSize = 12,
                    TextColor = Color.FromHex("#444444"),
                    HorizontalTextAlignment = TextAlignment.Center
                };
                Grid.SetRow(duration, 1);
                Grid.SetColumn(duration, 2);

                var stops = new Label
                {
                    Text = "1 stop LAX 1h 4m",
                    FontSize = 10,
                    TextColor = Color.FromHex("#444444"),
                    HorizontalTextAlignment = TextAlignment.Center
                };
                Grid.SetRow(stops, 2);
                Grid.SetColumn(stops, 2);

                var seperator = new BoxView
                {
                    HeightRequest = 1,
                    Color = Color.FromHex("#CCCCCC"),
                    VerticalOptions = LayoutOptions.End,
                    HorizontalOptions = LayoutOptions.Fill
                }; 
                Grid.SetRow(seperator, 4);
                Grid.SetColumnSpan(seperator, 4);

                var price = new Label
                {
                    FontSize = 16,
                    TextColor = Color.FromHex("#444444"),
                    Text = "$345",
                };
                Grid.SetRow(price, 3);
                Grid.SetColumn(price, 1);

                var type = new Label
                {
                    Text = "Round Trip",
                    FontSize = 10,
                    TextColor = Color.FromHex("#444444"),
                    HorizontalTextAlignment = TextAlignment.Start
                };
                Grid.SetRow(type, 4);
                Grid.SetColumn(type, 1);

                var expander = new Image
                {
                    Source = ImageSource.FromResource("Fly360.images.moreInfo.png"),
                    HeightRequest = 35,
                    WidthRequest = 25,
                    HorizontalOptions = LayoutOptions.End,
                    VerticalOptions = LayoutOptions.Center
                };
                Grid.SetRowSpan(expander, 2);
                Grid.SetRow(expander, 3);
                Grid.SetColumn(expander, 3);

                Children.Add(airline);
                Children.Add(airlineName);
                Children.Add(deptTime);
                Children.Add(deptCode);
                Children.Add(flight);
                Children.Add(arrTime);
                Children.Add(arrvCode);
                Children.Add(duration);
                Children.Add(stops);
                Children.Add(price);
                Children.Add(type);
                Children.Add(expander);
                Children.Add(seperator);
            }
        }
    }
}

