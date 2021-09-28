using ProyectoDIV1.Helpers;
using ProyectoDIV1.Services.Interfaces;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProyectoDIV1.Views.Account
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PopupSignOutPage : PopupPage
    {
        public PopupSignOutPage()
        {
            InitializeComponent();

        }

        private async void OnSignOut_Clicked(object sender, EventArgs e)
        {
            try
            {
                var authService = DependencyService.Resolve<IAuthenticationService>();
                authService.SignOut();
                Settings.IsLogin = false;
                Settings.TipoUsuario = null;
                Application.Current.MainPage = new MasterPage();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                await PopupNavigation.Instance.PopAllAsync();
            }  
        }

        private async void Cancelar_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAllAsync();
        }
    }
}