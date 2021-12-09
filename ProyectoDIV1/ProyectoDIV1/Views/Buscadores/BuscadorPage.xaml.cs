using Acr.UserDialogs;
using ProyectoDIV1.Helpers;
using ProyectoDIV1.Services.Helpers;
using ProyectoDIV1.ViewModels.Buscadores;
using Rg.Plugins.Popup.Services;
using System.Diagnostics;
using System.Web;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProyectoDIV1.Views.Buscadores
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BuscadorPage
    {
        private string _buscador;
        public BuscadorPage(string buscador)
        {
            _buscador = buscador;
            InitializeComponent();
            Buscar.Placeholder = _buscador.Equals(Constantes.SEARCH_JOB) ? "Buscar profesión" : "Buscar Habilidades";
        }

        private async void Buscar_SearchButtonPressed(object sender, System.EventArgs e)
        {
            try
            {
                await PopupNavigation.Instance.PopAllAsync(true);
                UserDialogs.Instance.ShowLoading("cargando..");
                SearchBar searchBar = (SearchBar)sender;
                string texto = searchBar.Text;
                if (texto.Length < 2)
                {
                    Toasts.Error("Ingrese un minimo de dos caracteres.", 4000);
                    return;
                }
                texto = HttpUtility.UrlEncode(texto);
                if (_buscador.Equals(Constantes.SEARCH_JOB))
                {
                    await Shell.Current.GoToAsync($"{nameof(BusquedaJobPage)}?{nameof(BusquedaJobViewModel.Texto)}={texto}");
                }
                else if (_buscador.Equals(Constantes.SEARCH_SKILL))
                {
                    await Shell.Current.GoToAsync($"{nameof(BusquedaSkillsPage)}?{nameof(BusquedaSkillsViewModel.Texto)}={texto}");
                }
            }
            catch (System.Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
        }
    }
}