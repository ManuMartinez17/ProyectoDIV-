using ProyectoDIV1.Interfaces;
using System;
using Xamarin.Forms;

namespace ProyectoDIV1
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
        }

        private void OnSignOut_Clicked(object sender, EventArgs e)
        {
            var authService = DependencyService.Resolve<IAuthenticationService>();
            authService.SignOut();
            Shell.Current.GoToAsync("//LoginPage");
        }
    }
}
