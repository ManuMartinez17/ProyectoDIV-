using ProyectoDIV1.Interfaces;
using ProyectoDIV1.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace ProyectoDIV1
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        Dictionary<string, Type> routes = new Dictionary<string, Type>();
        public Dictionary<string, Type> Routes { get { return routes; } }
        public AppShell()
        {
            InitializeComponent();
            RegisterRoutes();
        }
        private void RegisterRoutes()
        {
            routes.Add(nameof(PerfilTrabajoPage), typeof(PerfilTrabajoPage));
            foreach (var item in routes)
            {
                Routing.RegisterRoute(item.Key, item.Value);
            }
        }
        private void OnSignOut_Clicked(object sender, EventArgs e)
        {
            var authService = DependencyService.Resolve<IAuthenticationService>();
            authService.SignOut();
            Shell.Current.GoToAsync("//LoginPage");
        }
    }
}
