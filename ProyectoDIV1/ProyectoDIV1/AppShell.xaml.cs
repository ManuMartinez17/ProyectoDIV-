using ProyectoDIV1.ViewModels;
using ProyectoDIV1.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace ProyectoDIV1
{
    public partial class AppShell : Shell
    {
        Dictionary<string, Type> routes = new Dictionary<string, Type>();

        public Dictionary<string, Type> Routes { get { return routes; } }

        public AppShell()
        {

            InitializeComponent();
            RegisterRoutes();
            BindingContext = new MasterCandidatoViewModel();
        }

        private void RegisterRoutes()
        {
            routes.Add(nameof(BusquedaJobPage), typeof(BusquedaJobPage));
            routes.Add(nameof(ForgotPasswordPage), typeof(ForgotPasswordPage));
            routes.Add(nameof(InicioRegistroPage), typeof(InicioRegistroPage));
            routes.Add(nameof(PerfilCandidatoPage), typeof(PerfilCandidatoPage));
            routes.Add(nameof(PerfilEmpresaPage), typeof(PerfilEmpresaPage));
            routes.Add(nameof(BusquedaSkillsPage), typeof(BusquedaSkillsPage));
            routes.Add(nameof(EditarHojaDeVidaPage), typeof(EditarHojaDeVidaPage));
            foreach (var item in routes)
            {
                Routing.RegisterRoute(item.Key, item.Value);
            }
        }

        private void Shell_Navigating(object sender, ShellNavigatingEventArgs e)
        {

        }
    }
}
