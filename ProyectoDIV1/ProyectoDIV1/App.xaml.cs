using Newtonsoft.Json;
using ProyectoDIV1.Helpers;
using ProyectoDIV1.Interfaces;
using ProyectoDIV1.Services;
using ProyectoDIV1.Views;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ProyectoDIV1
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();
            Settings.Usuario = null;
            MainPage = new PerfilTrabajoPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
