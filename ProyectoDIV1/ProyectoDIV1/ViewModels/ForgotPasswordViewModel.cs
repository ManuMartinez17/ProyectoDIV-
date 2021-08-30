using ProyectoDIV1.Services.Interfaces;
using ProyectoDIV1.Views;
using System;
using Xamarin.Forms;

namespace ProyectoDIV1.ViewModels
{
    public class ForgotPasswordViewModel : BaseViewModel
    {
        private string email;

        public ForgotPasswordViewModel()
        {
            ResetPasswordCommand = new Command(OnResetPassword);
            SignUpCommand = new Command(IrAregistro);
        }

        public string Email
        {
            get => email;
            set => SetProperty(ref email, value);
        }

        public Command ResetPasswordCommand { get; }
        public Command SignUpCommand { get; }


        private async void IrAregistro(object obj)
        {
            await Shell.Current.GoToAsync(nameof(InicioRegistroPage));
        }

        private async void OnResetPassword(object obj)
        {
            try
            {

                var authService = DependencyService.Resolve<IAuthenticationService>();
                await authService.ResetPassword(Email);

                await Application.Current.MainPage.DisplayAlert("Alerta", "Se ha enviado un email verifique su bandeja.", "OK");

                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                await Application.Current.MainPage.DisplayAlert("Alerta", "No se ha podido enviar el correo.", "OK");
            }
        }
    }
}
