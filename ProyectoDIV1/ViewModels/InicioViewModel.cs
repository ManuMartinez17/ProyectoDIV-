using Firebase.Auth;
using Newtonsoft.Json;
using ProyectoDIV1.Models;
using ProyectoDIV1.Views;
using System;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ProyectoDIV1.ViewModels
{
    public class InicioViewModel : Usuario
    {
        #region Commands
        public Command LoginCommand { get; }
        public Command RegistroCommand { get; }

        #endregion


        public InicioViewModel()
        {
            LoginCommand = new Command(OnLoginClicked);
            RegistroCommand = new Command(OnRegistroClicked);
        }
        // alertas del LoginPage
        private async void OnLoginClicked(object obj)
        {
            if (string.IsNullOrEmpty(this.Email) || string.IsNullOrEmpty(this.Password))
            {   
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "Debe ingresar un correo electrónico.",
                    "Aceptar");
                return;
            }
       
            await Application.Current.MainPage.Navigation.PushAsync(new inicioDiseno());

            string WebAPIkey = "AIzaSyBtaSAuQU_iOWSr-kRKVUmCKN7HbH3nKaI";

            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(WebAPIkey));
            try
            {
                var auth = await authProvider.SignInWithEmailAndPasswordAsync(this.Email.ToString(),this.Password.ToString());
                var content = await auth.GetFreshAuthAsync();
                var serializedcontnet = JsonConvert.SerializeObject(content);

                Preferences.Set("MyFirebaseRefreshToken", serializedcontnet);
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Alert", "La contraseña o email es invalido", "OK");
            }
        }
        private async void OnRegistroClicked(object obj)
        {
            await Application.Current.MainPage.Navigation.PushAsync(new inicioRegistro());
        }

    }
}
