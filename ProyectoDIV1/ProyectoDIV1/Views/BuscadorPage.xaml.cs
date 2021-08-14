using Acr.UserDialogs;
using ProyectoDIV1.ViewModels;
using Rg.Plugins.Popup.Services;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProyectoDIV1.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BuscadorPage
    {
        public BuscadorPage()
        {
            InitializeComponent();
        }

     

        private async void Buscar_SearchButtonPressed(object sender, System.EventArgs e)
        {
            SearchBar searchBar = (SearchBar)sender;
            string texto = searchBar.Text;
            await PopupNavigation.Instance.PopAsync(true);
            UserDialogs.Instance.ShowLoading("cargando..");
            await Task.Delay(1000);
            await Shell.Current.GoToAsync($"{nameof(BusquedaSkillsPage)}?{nameof(BusquedaSkillsViewModel.Texto)}={texto}");
            UserDialogs.Instance.HideLoading();

        }
    }
}