using Acr.UserDialogs;
using Newtonsoft.Json;
using ProyectoDIV1.DTOs;
using ProyectoDIV1.Helpers;
using ProyectoDIV1.Interfaces;
using ProyectoDIV1.Services;
using ProyectoDIV1.Validators;
using ProyectoDIV1.Validators.Rules;
using ProyectoDIV1.Views;
using System;
using System.Threading.Tasks;
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
        public Command ForgotPasswordCommand { get; }

        #endregion

        #region Constructor
        public LoginViewModel()
        {
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
            EmailValid.Value = Email;
            if (ValidarFormulario())
            {
                try
                {
                    UserDialogs.Instance.ShowLoading("cargando...");
                    var authService = DependencyService.Resolve<IAuthenticationService>();
                    var token = await authService.SignIn(Email, Password);
                    App.Current.MainPage = new AppShell();

                    var candidato = await BuscarCandidato(Email);
                    if (candidato != null)
                    {
                        Settings.Usuario = JsonConvert.SerializeObject(candidato);
                    }
                    else
                    {
                        var empresa = await BuscarEmpresa(Email);
                        if (empresa != null)
                        {
                            Settings.Usuario = JsonConvert.SerializeObject(empresa);
                        }
                    }

                    UserDialogs.Instance.HideLoading();

                    await Shell.Current.GoToAsync("//AboutPage");
                }
                catch (Exception ex)
                {
                    UserDialogs.Instance.HideLoading();
                    await App.Current.MainPage.DisplayAlert("Alert", "La contraseña o email es invalido", "OK");
                }
            }
        }

        private async Task<CandidatoDTO> BuscarCandidato(string email)
        {
            var usuario = await new FirebaseHelper().GetUsuario("Candidatos", email);
            CandidatoDTO candidato = new CandidatoDTO()
            {
                Candidato = usuario,

            };
            string path = await new FirebaseStorageHelper().GetFile($"{candidato.Candidato.UsuarioId}.jpg", "imagenesdeperfil");
            candidato.PathImagen = path = !string.IsNullOrEmpty(path) ? path : "https://i.postimg.cc/BQmWRFDZ/iconuser.jpg";

            return candidato;
        }

        private async Task<CandidatoDTO> BuscarEmpresa(string email)
        {
            var usuario = await new FirebaseHelper().GetUsuario("Empresas", email);
            CandidatoDTO candidato = new CandidatoDTO()
            {
                Candidato = usuario
            };
            return candidato;
        }

        private async void OnForgotPassword()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new ForgotPasswordPage());
        }

        private async void OnRegistroClicked()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new InicioRegistroPage());
        }
        #endregion
    }
}
