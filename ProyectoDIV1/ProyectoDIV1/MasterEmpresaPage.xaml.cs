using ProyectoDIV1.ViewModels.Empresa;
using ProyectoDIV1.Views.Buscadores;
using ProyectoDIV1.Views.Candidato;
using ProyectoDIV1.Views.Empresa;
using ProyectoDIV1.Views.Notificaciones;
using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProyectoDIV1
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MasterEmpresaPage : Shell
    {
        Dictionary<string, Type> routes = new Dictionary<string, Type>();

        public Dictionary<string, Type> Routes { get { return routes; } }

        public MasterEmpresaPage()
        {

            InitializeComponent();
            RegisterRoutes();
            BindingContext = new MasterEmpresaViewModel();
        }

        private void RegisterRoutes()
        {
            routes.Add(nameof(BusquedaJobPage), typeof(BusquedaJobPage));
            routes.Add(nameof(BusquedaSkillsPage), typeof(BusquedaSkillsPage));
            routes.Add(nameof(CandidatoPage), typeof(CandidatoPage));
            routes.Add(nameof(EmpresaPage), typeof(EmpresaPage));
            routes.Add(nameof(NotficacionesEmpresasPage), typeof(NotficacionesEmpresasPage));

            foreach (var item in routes)
            {
                Routing.RegisterRoute(item.Key, item.Value);
            }
        }
    }
}