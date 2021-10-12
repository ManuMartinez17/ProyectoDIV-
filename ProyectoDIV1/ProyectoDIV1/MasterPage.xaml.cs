using ProyectoDIV1.Views;
using ProyectoDIV1.Views.Account;
using ProyectoDIV1.Views.Buscadores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProyectoDIV1
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MasterPage : Shell
    {
        Dictionary<string, Type> routes = new Dictionary<string, Type>();
        public MasterPage()
        {
            InitializeComponent();
            RegisterRoutes();
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
            routes.Add(nameof(ForgotPasswordPage), typeof(ForgotPasswordPage));
            routes.Add(nameof(PerfilCandidatoPage), typeof(PerfilCandidatoPage));
            routes.Add(nameof(InicioRegistroPage), typeof(InicioRegistroPage));
            routes.Add(nameof(PerfilEmpresaPage), typeof(PerfilEmpresaPage));
            routes.Add(nameof(PerfilTrabajoPage), typeof(PerfilTrabajoPage));
            routes.Add(nameof(BusquedaJobPage), typeof(BusquedaJobPage));
            routes.Add(nameof(BusquedaSkillsPage), typeof(BusquedaSkillsPage));
            routes.Add(nameof(NoInternetConnectionPage), typeof(NoInternetConnectionPage));
            foreach (var item in routes)
            {
                Routing.RegisterRoute(item.Key, item.Value);
            }
        }
    }
}