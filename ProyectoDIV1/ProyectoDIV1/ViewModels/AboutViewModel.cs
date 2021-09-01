using ProyectoDIV1.Entidades.Models;
using ProyectoDIV1.Services.FirebaseServices;
using ProyectoDIV1.Services.Helpers;
using ProyectoDIV1.Services.Interfaces;
using ProyectoDIV1.Views;
using ProyectoDIV1.Views.Account;
using System;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ProyectoDIV1.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        private FirebaseHelper _firebaseHelper;
        public AboutViewModel()
        {
            Title = "About";
            _firebaseHelper = new FirebaseHelper();
            CheckWhetherTheUserIsSignIn();
            OpenWebCommand = new Command(async () => await Browser.OpenAsync("https://aka.ms/xamarin-quickstart"));
        }

        private async void CheckWhetherTheUserIsSignIn()
        {
            try
            {
                var authenticationService = DependencyService.Resolve<IAuthenticationService>();
                if (!authenticationService.IsSignIn())
                    if (VersionTracking.IsFirstLaunchEver)
                    {
                        Application.Current.MainPage = new NavigationPage();
                        await Application.Current.MainPage.Navigation.PushModalAsync(new OnboardingPage());
                    }
                    else
                    {
                        Application.Current.MainPage = new MasterPage();
                        await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
                    }     
                else
                {
                    string Email = authenticationService.BuscarEmail();
                    bool candidato = await _firebaseHelper.GetUsuarioByEmailAsync<ECandidato>(Constantes.COLLECTION_CANDIDATO, Email);
                    bool empresa = await _firebaseHelper.GetUsuarioByEmailAsync<EEmpresa>(Constantes.COLLECTION_EMPRESA, Email);
                    if (candidato)
                    {
                        Application.Current.MainPage = new MasterCandidatoPage();
                    }
                    else if (empresa)
                    {
                        Application.Current.MainPage = new MasterEmpresaPage();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public ICommand OpenWebCommand { get; }
    }
}