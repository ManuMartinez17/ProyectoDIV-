
using ProyectoDIV1.Entidades.Models;
using ProyectoDIV1.ViewModels.Buscadores;
using Rg.Plugins.Popup.Services;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProyectoDIV1.Views.Buscadores
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BusquedaJobPage : ContentPage
    {
        BusquedaJobViewModel _viewModel;
        public BusquedaJobPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new BusquedaJobViewModel();
        }

        private async void autocomplete_jobs_FilterCollectionChanged(object sender, Syncfusion.SfAutoComplete.XForms.FilterCollectionChangedEventArgs e)
        {
            await PopupNavigation.Instance.PushAsync(new PopupLoadingPage());
            try
            {
                IEnumerable lista = (IEnumerable)e.Value;
                List<Job> asList = lista.Cast<Job>().ToList();
                if (asList.Count > 0)
                {
                    ListaJobs.ItemsSource = asList;
                }
                else
                {
                    ListaJobs.ItemsSource = _viewModel.TiposDeJobs;
                }
            }
            catch (System.Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                await PopupNavigation.Instance.PopAllAsync();
            }
        }
    }
}