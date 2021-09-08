using ProyectoDIV1.Entidades.Models;
using ProyectoDIV1.Helpers;
using ProyectoDIV1.Services.FirebaseServices;
using ProyectoDIV1.Services.Helpers;
using ProyectoDIV1.Services.Interfaces;
using ProyectoDIV1.Views;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ProyectoDIV1
{
    public partial class App : Application
    {
        private FirebaseHelper _firebaseHelper = new FirebaseHelper();
        public App()
        {
            InitializeComponent();
            Settings.Usuario = null;
            if (VersionTracking.IsFirstLaunchEver)
            {
                MainPage = new NavigationPage();
                MainPage.Navigation.PushModalAsync(new OnboardingPage());
            }
            else
            {
                MainPage = new MasterPage();
            }
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
