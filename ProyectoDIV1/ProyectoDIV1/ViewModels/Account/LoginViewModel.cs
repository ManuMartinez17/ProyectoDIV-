using Acr.UserDialogs;
using ProyectoDIV1.Entidades.Models;
using ProyectoDIV1.Helpers;
using ProyectoDIV1.Services.FirebaseServices;
using ProyectoDIV1.Services.Helpers;
using ProyectoDIV1.Services.Interfaces;
using ProyectoDIV1.Validators;
using ProyectoDIV1.Validators.Rules;
using ProyectoDIV1.Views.Account;
using System;
using System.Diagnostics;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ProyectoDIV1.ViewModels.Account
{
    public class LoginViewModel : BaseViewModel
    {
        #region Atributos
        private string _email;
        private string _password;
        private static LoginViewModel _instance;
        private FirebaseHelper _firebaseHelper;
        public ValidatableObject<string> EmailValid { get; set; } = new ValidatableObject<string>();
        #endregion

        #region Commands
        public Command LoginCommand { get; }
        public Command InicioRegistroCommand { get; }
        public Command ForgotPasswordCommand { get; }

        #endregion

        #region Constructor
        public LoginViewModel()
        {
            _instance = this;
            _firebaseHelper = new FirebaseHelper();
            LoginCommand = new Command(OnLoginClicked, ValidateSave);
            InicioRegistroCommand = new Command(OnRegistroClicked);
            this.PropertyChanged +=
               (_, __) => LoginCommand.ChangeCanExecute();
            ForgotPasswordCommand = new Command(OnForgotPassword);
            AddValidationRules();
        }


        #endregion

        #region Properties
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

        #endregion

        #region validaciones
        public void AddValidationRules()
        {
            EmailValid.Validations.Add(new IsValidEmailRule<string> { ValidationMessage = "Email invalido" });
        }


        private bool ValidateSave()
        {
            return !String.IsNullOrWhiteSpace(Email)
                && !String.IsNullOrWhiteSpace(Password);
        }


        bool ValidarFormulario()
        {
            bool isEmailValid = EmailValid.Validate();
            return isEmailValid;
        }
        #endregion

        #region Methods
        private async void OnLoginClicked()
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await UserDialogs.Instance.AlertAsync("Verifique el internet del celular.");
                return;
            }
            EmailValid.Value = Email;
            if (ValidarFormulario())
            {
                try
                {
                    UserDialogs.Instance.ShowLoading("Iniciando Sesión...");
                    var authService = DependencyService.Resolve<IAuthenticationService>();
                    var token = await authService.SignIn(Email, Password);

                    bool candidato = await _firebaseHelper.GetUsuarioByEmailAsync<ECandidato>(Constantes.COLLECTION_CANDIDATO, Email);
                    bool empresa = await _firebaseHelper.GetUsuarioByEmailAsync<EEmpresa>(Constantes.COLLECTION_EMPRESA, Email);
                    if (candidato)
                    {
                        Settings.IsLogin = true;
                        Settings.TipoUsuario = Constantes.ROL_CANDIDATO;
                        Application.Current.MainPage = new MasterCandidatoPage();
                    }
                    else if (empresa)
                    {
                        Settings.IsLogin = true;
                        Settings.TipoUsuario = Constantes.ROL_EMPRESA;
                        Application.Current.MainPage = new MasterEmpresaPage();
                    }
                    UserDialogs.Instance.HideLoading();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    UserDialogs.Instance.HideLoading();
                    Toasts.Error("La contraseña y/o email es invalido.", 3000);
                }
            }
        }
        private async void OnForgotPassword()
        {
            await Shell.Current.GoToAsync(nameof(ForgotPasswordPage));
        }

        private async void OnRegistroClicked()
        {
            await Shell.Current.GoToAsync(nameof(InicioRegistroPage));
        }
        #endregion
    }
}
