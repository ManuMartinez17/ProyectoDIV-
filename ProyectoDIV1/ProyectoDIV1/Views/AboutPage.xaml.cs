using ProyectoDIV1.Services.Interfaces;
using ProyectoDIV1.ViewModels;
using System;
using Xamarin.Forms;

namespace ProyectoDIV1.Views
{
    public partial class AboutPage : ContentPage
    {
        public AboutPage()
        {
            InitializeComponent();
            CheckWhetherTheUserIsSignIn();
            BindingContext = new AboutViewModel();
        }
        private async void CheckWhetherTheUserIsSignIn()
        {
            try
            {
                var authenticationService = DependencyService.Resolve<IAuthenticationService>();
                if (!authenticationService.IsSignIn())
                    await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}