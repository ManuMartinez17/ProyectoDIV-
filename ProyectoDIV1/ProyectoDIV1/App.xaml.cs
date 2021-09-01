using ProyectoDIV1.Helpers;
using ProyectoDIV1.Views;
using Xamarin.Forms;

namespace ProyectoDIV1
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();
            Settings.Usuario = null;
            MainPage = new NavigationPage(new AboutPage());
        }
        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
