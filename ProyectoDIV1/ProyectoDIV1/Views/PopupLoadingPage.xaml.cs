using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProyectoDIV1.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PopupLoadingPage : PopupPage
    {

        public PopupLoadingPage(string texto = "Cargando...")
        {
            InitializeComponent();
            LB_texto.Text = texto;
        }
    }
}