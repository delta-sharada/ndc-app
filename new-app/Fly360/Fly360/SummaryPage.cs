using System;

using Xamarin.Forms;

namespace Fly360
{
    public class SummaryPage : ContentPage
    {
        public SummaryPage()
        {
            Content = new StackLayout
            {
                Children = {
                    new Image
                    {
                        Source = ImageSource.FromResource("bookingConfirmed.png"),
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.Center
                    }
                }
            };
        }
    }
}

