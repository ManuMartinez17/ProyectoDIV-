using Acr.UserDialogs;
using Firebase.Auth;
using Newtonsoft.Json;
using ProyectoDIV1.Models;
using ProyectoDIV1.Validators;
using ProyectoDIV1.Validators.Rules;
using ProyectoDIV1.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ProyectoDIV1.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private string _email;
        private string _password;
        public ValidatableObject<string> EmailValid { get; set; } = new ValidatableObject<string>();
        #region Commands
        public Command LoginCommand { get; }
        public Command InicioRegistroCommand { get; }

        #endregion



        public LoginViewModel()
        {
            LoginCommand = new Command(OnLoginClicked, ValidateSave);
            InicioRegistroCommand = new Command(OnRegistroClicked);
            this.PropertyChanged +=
               (_, __) => LoginCommand.ChangeCanExecute();
            AddValidationRules();
        }

        public void AddValidationRules()
        {
            EmailValid.Validations.Add(new IsValidEmailRule<string> { ValidationMessage = "Email invalido" });
        }


        private bool ValidateSave()
        {
            return !String.IsNullOrWhiteSpace(Email)
                && !String.IsNullOrWhiteSpace(Password);
        }

    

        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }


        bool ValidarFormulario()
        {
            bool isEmailValid = EmailValid.Validate();
            return isEmailValid;
        }

        private async void OnLoginClicked()
        {
            EmailValid.Value = Email;
            if (ValidarFormulario())
            {
                string WebAPIkey = "AIzaSyBtaSAuQU_iOWSr-kRKVUmCKN7HbH3nKaI";

                var authProvider = new FirebaseAuthProvider(new FirebaseConfig(WebAPIkey));
                try
                {
                    UserDialogs.Instance.ShowLoading("Cargando...");
                    var auth = await authProvider.SignInWithEmailAndPasswordAsync(Email, Password);
                    UserDialogs.Instance.HideLoading();
                    var content = await auth.GetFreshAuthAsync();
                    var serializedcontnet = JsonConvert.SerializeObject(content);

                    Preferences.Set("MyFirebaseRefreshToken", serializedcontnet);
                }
                catch (Exception ex)
                {
                    UserDialogs.Instance.HideLoading();
                    await App.Current.MainPage.DisplayAlert("Alert", "La contraseña o email es invalido", "OK");
                }
            }
        }
        private async void OnRegistroClicked()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new InicioRegistroPage());
        }
    }
}
