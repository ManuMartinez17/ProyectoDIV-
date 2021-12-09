using ProyectoDIV1.Helpers;
using ProyectoDIV1.Services.Helpers;
using System;
using System.Diagnostics;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProyectoDIV1.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NoInternetConnectionPage : ContentPage
    {
        public NoInternetConnectionPage()
        {
            InitializeComponent();
        }

        private async void BT_Verificar_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                {
                    if (Shell.Current != null)
                    {
                        if (Shell.Current.Navigation.NavigationStack.Count > 0)
                        {
                            await Shell.Current.Navigation.PopAsync();
                        }
                        else if (Shell.Current.Navigation.ModalStack.Count > 0)
                        {
                            await Shell.Current.Navigation.PopAsync();
                        }
                    }
                    else
                    {
                        if (Settings.IsLogin)
                        {
                            string rol = Settings.TipoUsuario;
                            if (rol.Equals(Constantes.ROL_CANDIDATO))
                            {
                                Application.Current.MainPage = new MasterCandidatoPage();
                            }
                            else if (rol.Equals(Constantes.ROL_EMPRESA))
                            {
                                Application.Current.MainPage = new MasterEmpresaPage();
                            }
                        }
                        else
                        {
                            Application.Current.MainPage = new MasterPage();
                        }

                    }
                }
                else
                {
                    Toasts.Error("Todavia sigue sin internet", 3000);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}