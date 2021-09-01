using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProyectoDIV1.Views.Account
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InicioRegistroPage : ContentPage
    {
        public InicioRegistroPage()
        {
            InitializeComponent();
        }

        private async void Candidato_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(PerfilCandidatoPage));
        }

        private async void Empresa_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(PerfilEmpresaPage));
        }
    }
}