using ProyectoDIV1.Services;
using ProyectoDIV1.Views;
using Xamarin.Forms;

namespace ProyectoDIV1
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();
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
