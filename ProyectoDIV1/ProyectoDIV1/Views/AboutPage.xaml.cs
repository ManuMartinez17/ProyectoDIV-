using ProyectoDIV1.ViewModels;
using Xamarin.Forms;

namespace ProyectoDIV1.Views
{
    public partial class AboutPage : ContentPage
    {

        public AboutPage()
        {
            InitializeComponent();

            BindingContext = new AboutViewModel();
        }
    }
}