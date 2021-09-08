using ProyectoDIV1.Views.Account;
using ProyectoDIV1.Views.Buscadores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            foreach (var item in routes)
            {
                Routing.RegisterRoute(item.Key, item.Value);
            }
        }
    }
}