using Newtonsoft.Json;
using ProyectoDIV1.Entidades.Models;
using ProyectoDIV1.Helpers;
using ProyectoDIV1.Services;
using ProyectoDIV1.Services.ExternalServices;
using ProyectoDIV1.Services.Helpers;
using ProyectoDIV1.Services.Interfaces;
using ProyectoDIV1.Views;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Xamarin.Forms;

namespace ProyectoDIV1.ViewModels.Buscadores
{
    [QueryProperty(nameof(Texto), nameof(Texto))]
    public class BusquedaJobViewModel : BaseViewModel
    {
        private IEnumerable<object> _items;
        private string _texto;
        private ECandidato _candidato = new ECandidato();
        private TraductorService _traductor;
        private JobAndSkillService serviceJobsandSkills = new JobAndSkillService();
        private ObservableCollection<Job> _tiposDeJobs;
        public BusquedaJobViewModel()
        {
            _traductor = new TraductorService();
            _candidato = JsonConvert.DeserializeObject<ECandidato>(Settings.Candidato);
            InsertarCommand = new Command<object>(JobSelected, CanNavigate);
        }

        private async void JobSelected(object objeto)
        {
            await PopupNavigation.Instance.PushAsync(new PopupLoadingPage("Guardando..."));
            var lista = objeto as Syncfusion.ListView.XForms.ItemTappedEventArgs;
            var job = lista.ItemData as Job;
            try
            {

                if (job != null)
                {
                    if (_candidato == null)
                    {
                        _candidato = new ECandidato();
                    }
                    _candidato.Profesion = job.name;
                    var authenticationService = DependencyService.Resolve<IAuthenticationService>();
                    if (authenticationService.IsSignIn())
                    {
                        Settings.Usuario = JsonConvert.SerializeObject(_candidato);
                        await Shell.Current.GoToAsync("..");
                    }
                    else
                    {
                        Settings.Candidato = JsonConvert.SerializeObject(_candidato);
                        await Shell.Current.GoToAsync("..");
                    }

                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                await PopupNavigation.Instance.PopAllAsync();
            }
        }

        private bool CanNavigate(object argument)
        {
            return true;
        }


        public string Texto
        {
            get
            {
                return _texto;
            }
            set
            {
                _texto = value;
                LoadJobs(value);
            }
        }
        public IEnumerable<object> Items
        {
            get { return _items; }
            set { SetProperty(ref _items, value); }
        }
        public Command InsertarCommand { get; set; }
        public ObservableCollection<Job> TiposDeJobs
        {
            get { return _tiposDeJobs; }
            set { SetProperty(ref _tiposDeJobs, value); }
        }
        private async void LoadJobs(string value)
        {
            await PopupNavigation.Instance.PushAsync(new PopupLoadingPage());
            try
            {
                var token = JsonConvert.DeserializeObject<Token>(Settings.Token);
                if (token == null)
                {
                    var tokenNuevo = serviceJobsandSkills.GenerarToken();
                    Settings.Token = JsonConvert.SerializeObject(tokenNuevo);
                    token = tokenNuevo;
                }
                else if (token.Expiration < DateTime.Now)
                {
                    var tokenNuevo = serviceJobsandSkills.GenerarToken();
                    Settings.Token = JsonConvert.SerializeObject(tokenNuevo);
                    token = tokenNuevo;
                }
                string palabra = await _traductor.TraducirPalabra(value, Constantes.CodigoISOEnglish, Constantes.CodigoISOSpanish);
                string palabraTraducida = ParsearUrlConCodigoPorciento(palabra);
                var lista = await serviceJobsandSkills.GetListJobsAsync($"titles/versions/latest/titles?q=.{palabraTraducida}&limit=10", token.access_token);
                var listadotraducido = await _traductor.TraducirJobs(lista.data);
                TiposDeJobs = new ObservableCollection<Job>(listadotraducido);
            }
            catch (Exception ex)
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
