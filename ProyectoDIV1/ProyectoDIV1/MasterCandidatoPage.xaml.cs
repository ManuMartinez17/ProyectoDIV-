using ProyectoDIV1.ViewModels.Candidato;
using ProyectoDIV1.Views.Account;
using ProyectoDIV1.Views.Buscadores;
using ProyectoDIV1.Views.Candidato;
using ProyectoDIV1.Views.Empresa;
using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProyectoDIV1
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MasterCandidatoPage : Shell
    {
        Dictionary<string, Type> routes = new Dictionary<string, Type>();

        public Dictionary<string, Type> Routes { get { return routes; } }
        public MasterCandidatoPage()
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
            routes.Add(nameof(CandidatoPage), typeof(CandidatoPage));
            routes.Add(nameof(EmpresaPage), typeof(EmpresaPage));
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