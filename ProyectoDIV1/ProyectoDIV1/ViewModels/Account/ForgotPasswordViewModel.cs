using ProyectoDIV1.Helpers;
using ProyectoDIV1.Services.Interfaces;
using ProyectoDIV1.Validators;
using ProyectoDIV1.Validators.Rules;
using ProyectoDIV1.Views.Account;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ProyectoDIV1.ViewModels.Account
{
    public class ForgotPasswordViewModel : BaseViewModel
    {
        public ValidatableObject<string> _email = new ValidatableObject<string>();

        public ForgotPasswordViewModel()
        {
            ResetPasswordCommand = new Command(OnResetPassword);
            SignUpCommand = new Command(IrAregistro);
            AddValidationRules();
        }

        public ValidatableObject<string> Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        public Command ResetPasswordCommand { get; }
        public Command SignUpCommand { get; }

        public void AddValidationRules()
        {
            Email.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Email Requerido." });
            Email.Validations.Add(new IsValidEmailRule<string> { ValidationMessage = "Email invalido." });
        }
        bool ValidarFormulario()
        {
            bool isEmailValid = Email.Validate();
            return isEmailValid;
        }
        private async void IrAregistro(object obj)
        {
            await Shell.Current.GoToAsync($"//{nameof(InicioRegistroPage)}");
        }

        private async void OnResetPassword(object obj)
        {
            try
            {
                if (ValidarFormulario())
                {
                    var authService = DependencyService.Resolve<IAuthenticationService>();
                    await authService.ResetPassword(Email.Value);
                    Toasts.Success("Se ha enviado un email, verifique su bandeja o correo no deseado.", 3000);
                    await Task.Delay(2000);
                    await Shell.Current.GoToAsync("..");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Toasts.Error("No se ha podido enviar el mensaje, verifique el correo.", 3000);
            }
        }
    }
}
