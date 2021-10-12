using ProyectoDIV1.ViewModels.Candidato;
using ProyectoDIV1.ViewModels.Notificaciones;
using ProyectoDIV1.Views;
using ProyectoDIV1.Views.Account;
using ProyectoDIV1.Views.Buscadores;
using ProyectoDIV1.Views.Candidato;
using ProyectoDIV1.Views.Chat;
using ProyectoDIV1.Views.Empresa;
using ProyectoDIV1.Views.Notificaciones;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProyectoDIV1
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MasterCandidatoPage : Shell
    {
        private MasterCandidatoViewModel _viewModel;
        Dictionary<string, Type> routes = new Dictionary<string, Type>();

        public Dictionary<string, Type> Routes { get { return routes; } }
        public MasterCandidatoPage()
        {
            InitializeComponent();
            RegisterRoutes();
            BindingContext = _viewModel = new MasterCandidatoViewModel();
            Connectivity.ConnectivityChanged += ConnectivityChangedHandler;

        }
        private void ConnectivityChangedHandler(object sender, ConnectivityChangedEventArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                {
                    if (Current.Navigation.NavigationStack.Count > 0)
                    {
                        Current.Navigation.PopAsync();
                    }
                    else if (Current.Navigation.ModalStack.Count > 0)
                    {
                        Current.Navigation.PopAsync();
                    }
                    else if (Application.Current.MainPage.Navigation.NavigationStack.Count > 0)
                    {
                        Application.Current.MainPage.Navigation.PopAsync();
                    }
                }
                else
                {
                    Current.Navigation.PushAsync(new NoInternetConnectionPage());
                }
            });
        }
        private void RegisterRoutes()
        {
            routes.Add(nameof(BusquedaJobPage), typeof(BusquedaJobPage));
            routes.Add(nameof(ChatCandidatoPage), typeof(ChatCandidatoPage));
            routes.Add(nameof(ForgotPasswordPage), typeof(ForgotPasswordPage));
            routes.Add(nameof(InicioRegistroPage), typeof(InicioRegistroPage));
            routes.Add(nameof(PerfilCandidatoPage), typeof(PerfilCandidatoPage));
            routes.Add(nameof(PerfilEmpresaPage), typeof(PerfilEmpresaPage));
            routes.Add(nameof(BusquedaSkillsPage), typeof(BusquedaSkillsPage));
            routes.Add(nameof(CandidatoPage), typeof(CandidatoPage));
            routes.Add(nameof(EditarDatosPage), typeof(EditarDatosPage));
            routes.Add(nameof(NotificacionesPage), typeof(NotificacionesPage));
            routes.Add(nameof(VerHojaDeVidaPage), typeof(VerHojaDeVidaPage));
            routes.Add(nameof(EmpresaPage), typeof(EmpresaPage));
            routes.Add(nameof(EditarHojaDeVidaPage), typeof(EditarHojaDeVidaPage));
            routes.Add(nameof(InfoNotificacionPage), typeof(InfoNotificacionPage));
            routes.Add(nameof(NoInternetConnectionPage), typeof(NoInternetConnectionPage));
            foreach (var item in routes)
            {
                Routing.RegisterRoute(item.Key, item.Value);
            }
        }


        private void Shell_Navigating(object sender, ShellNavigatingEventArgs args)
        {
            try
            {
                if (_viewModel != null)
                {
                    _viewModel.RefreshCandidato(_viewModel.Candidato.Candidato.UsuarioId);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
           
        }

        protected override void OnNavigating(ShellNavigatingEventArgs args)
        {
            try
            {
                if (_viewModel != null)
                {
                    _viewModel.RefreshCandidato(_viewModel.Candidato.Candidato.UsuarioId);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}