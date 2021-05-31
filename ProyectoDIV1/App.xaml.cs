using ProyectoDIV1.Views;
using Syncfusion.Licensing;
using Xamarin.Forms;

namespace ProyectoDIV1
{
    public partial class App : Application
    {
        public App()
        {
            SyncfusionLicenseProvider.RegisterLicense("MzA4MzU5QDMxMzgyZTMyMmUzMEZqYjJ2Tms5TkxMaUl3QWF5UHEwUldRQnRQYVNGWXAvM1JhandUKzYydUk9");
            InitializeComponent();
            MainPage = new NavigationPage(new inicioPortada());
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
