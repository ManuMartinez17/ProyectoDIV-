
using ProyectoDIV1.ViewModels.Account;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProyectoDIV1.Views.Account
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PerfilCandidatoPage : ContentPage
    {
        public PerfilCandidatoPage()
        {
            InitializeComponent();
            BindingContext = new PerfilCandidatoViewModel();
        }
    }
}