using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LaunchContentDemo
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        void Handle_Clicked(object sender, System.EventArgs e)
        {
            // We rely on the built-in service lcoator in this example, but you could just
            // as easily locate this service using DI and launch from your ViewModel
            var service = DependencyService.Get<INativeBrowserService>();
            {
                if (service == null) return;

                service.LaunchNativeEmbeddedBrowser("https://delta-sharada.github.io/ndc-content/");
            }
        }
    }
}
