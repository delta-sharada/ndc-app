using System;
using System.Collections.Generic;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace Fly360
{
    public class MenuPopupPage : PopupPage
    {
        public MenuPopupPage()
        {
            //<Button Text="Close Popup" TextColor="Red" Clicked="OnClose" VerticalOptions="CenterAndExpand" HorizontalOptions="CenterAndExpand"></Button>
            HasSystemPadding = true;

            var label = new Label
            {
                Text = "Suggested Destination Types",
                TextColor = Color.White,
                FontSize = 18,
                HorizontalTextAlignment = TextAlignment.Center
            };

            var closeBtn = new Button
            {
                Text = "Close",
                BackgroundColor = Color.FromHex("#F2F6F8"),
                TextColor = Color.FromHex("#5887F9"),
                CornerRadius = 5,
                WidthRequest = 175,
                HeightRequest = 40,
                HorizontalOptions = LayoutOptions.Center,
                Margin = new Thickness(40, 0)
            };

            closeBtn.Clicked += async(sender, e) => {
                await PopupNavigation.PopAsync();
            };

            var lables = new string[] {
                "City Break",
                "Ski",
                "Beach",
                "Luxury",
                "Family",
                "Safari"
            };
            var images = new string[] {
                "cityIcon",
                "skiingIcon",
                "beachIcon",
                "luxuryIcon",
                "familyIcon",
                "safariIcon"
            };

            var items = new List<View>();
            for (int i = 0; i < 6; i++)
            {
                var btn = new Button
                {
                    Text = string.Empty,

                    BackgroundColor = i == 2 ? Color.White : Color.FromHex("#805F5F5F"),
                    //Opacity = i == 2 ? 1 : 0.5,
                    BorderColor = i == 2 ? Color.FromHex("#7BABFC") : Color.FromHex("#9B9B9B"),
                    BorderWidth = 2,
                    CornerRadius = 10,
                    WidthRequest = 110,
                    HeightRequest = 125,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                };

                var lbl = new Label
                {
                    Text = lables[i],
                    TextColor = i == 2 ? Color.FromHex("#444444") : Color.White,
                    FontSize = 14,
                    HorizontalTextAlignment = TextAlignment.Center,
                    VerticalOptions = LayoutOptions.End,
                    Margin = 10
                };

                var img = new Image
                {
                    Source = ImageSource.FromResource(string.Format("Fly360.images.{0}.png", images[i])),
                    WidthRequest = 48,
                    HeightRequest = 58,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Start,
                    Margin = 25
                };

                Grid.SetRow(btn, i / 2);
                Grid.SetColumn(btn, i % 2);
                items.Add(btn);

                Grid.SetRow(lbl, i / 2);
                Grid.SetColumn(lbl, i % 2);
                items.Add(lbl);

                Grid.SetRow(img, i / 2);
                Grid.SetColumn(img, i % 2);
                items.Add(img);


                if(i == 2)
                {
                    btn.Clicked += (sender, e) => OnClicked();
                    lbl.GestureRecognizers.Add(new TapGestureRecognizer((v, a) => OnClicked()));
                    img.GestureRecognizers.Add(new TapGestureRecognizer((v, a) => OnClicked()));
                }
            }

            var grid = new Grid
            {
                RowDefinitions = {
                    new RowDefinition(),
                    new RowDefinition(),
                    new RowDefinition()
                },
                ColumnDefinitions = {
                    new ColumnDefinition(),
                    new ColumnDefinition(),
                },
                ColumnSpacing = 55,
                RowSpacing = 25,
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Center,
            };

            foreach (var b in items)
                grid.Children.Add(b);

            Content = new Grid
            {
                Children = {
                    new Image { 
                        Source = ImageSource.FromResource("Fly360.images.blur.jpg"),
                        Opacity = 0.33, 
                        Aspect = Aspect.Fill },
                    new StackLayout {
                        Margin = new Thickness(40, 65, 40, 40),
                        Spacing = 25,
                        Children = {
                            label,
                            grid,
                            closeBtn
                        },
                        HorizontalOptions = LayoutOptions.Fill,
                        VerticalOptions = LayoutOptions.FillAndExpand,
                    }
                },
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.FillAndExpand,
            };
        }

        async void OnClicked()
        {
            await PopupNavigation.PopAsync();
            await Navigation.PushAsync(new BeachOptionsPage());
        }

    }
}

