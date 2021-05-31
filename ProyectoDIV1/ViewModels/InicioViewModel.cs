using ProyectoDIV1.Models;
using ProyectoDIV1.Views;
using System.Windows.Input;
using Xamarin.Forms;

namespace ProyectoDIV1.ViewModels
{
    public class InicioViewModel : Usuario
    {
        #region Commands
        public Command LoginCommand { get; }

        #endregion


        public InicioViewModel()
        {
            LoginCommand = new Command(OnLoginClicked);
        }
        // alertas del LoginPage
        private async void OnLoginClicked(object obj)
        {
            if (string.IsNullOrEmpty(this.Apodo) || string.IsNullOrEmpty(this.Password))
            {   
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "Debe ingresar un correo electrónico.",
                    "Aceptar");
                return;
            }
       
            await Application.Current.MainPage.Navigation.PushAsync(new inicioDiseno());
        }
       
    }
}
