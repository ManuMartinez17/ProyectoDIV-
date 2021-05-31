using ProyectoDIV1.Models;
using ProyectoDIV1.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ProyectoDIV1.ViewModels
{
    public class RegistroViewModel : Usuario
    {
        public Command RegistroCandidatoCommand { get; }
        public Command RegistroEmpresaCommand { get; }

        public RegistroViewModel () {
            RegistroCandidatoCommand = new Command(OnRegistroCandidatoClicked);
            RegistroEmpresaCommand = new Command(OnRegistroEmpresaClicked);
        }

        private async void OnRegistroCandidatoClicked(object obj)
        {
            await Application.Current.MainPage.Navigation.PushAsync(new PerfilCandidato());
        }

        private async void OnRegistroEmpresaClicked(object obj)
        {
            await Application.Current.MainPage.Navigation.PushAsync(new PerfilCandidato());
        }
    }

    
}
