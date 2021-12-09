using ProyectoDIV1.ViewModels.Empresa;
using ProyectoDIV1.Views;
using ProyectoDIV1.Views.Buscadores;
using ProyectoDIV1.Views.Candidato;
using ProyectoDIV1.Views.Empresa;
using ProyectoDIV1.Views.Notificaciones;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProyectoDIV1
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MasterEmpresaPage : Shell
    {
        private MasterEmpresaViewModel _viewModel;
        Dictionary<string, Type> routes = new Dictionary<string, Type>();

        public Dictionary<string, Type> Routes { get { return routes; } }

        public MasterEmpresaPage()
        {

            InitializeComponent();
            RegisterRoutes();
            BindingContext = _viewModel = new MasterEmpresaViewModel();
            Connectivity.ConnectivityChanged += ConnectivityChangedHandler;
        }
        private void ConnectivityChangedHandler(object sender, ConnectivityChangedEventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
           
        }
        private void RegisterRoutes()
        {
            routes.Add(nameof(BusquedaJobPage), typeof(BusquedaJobPage));
            routes.Add(nameof(BusquedaSkillsPage), typeof(BusquedaSkillsPage));
            routes.Add(nameof(CandidatoPage), typeof(CandidatoPage));
            routes.Add(nameof(EmpresaPage), typeof(EmpresaPage));
            routes.Add(nameof(NotificacionesPage), typeof(NotificacionesPage));
            routes.Add(nameof(InfoNotificacionPage), typeof(InfoNotificacionPage));
            routes.Add(nameof(NoInternetConnectionPage), typeof(NoInternetConnectionPage));

            foreach (var item in routes)
            {
                Routing.RegisterRoute(item.Key, item.Value);
            }
        }
        protected override void OnNavigating(ShellNavigatingEventArgs args)
        {
            base.OnNavigating(args);
            try
            {
                if (_viewModel != null)
                {
                    _viewModel.RefreshEmpresa(_viewModel.Empresa.Empresa.UsuarioId);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}