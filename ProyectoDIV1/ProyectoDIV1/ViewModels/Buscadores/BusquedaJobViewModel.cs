using Acr.UserDialogs;
using Newtonsoft.Json;
using ProyectoDIV1.Entidades.Models;
using ProyectoDIV1.Helpers;
using ProyectoDIV1.Services;
using ProyectoDIV1.Services.ExternalServices;
using ProyectoDIV1.Services.FirebaseServices;
using ProyectoDIV1.Services.Helpers;
using ProyectoDIV1.Services.Interfaces;
using ProyectoDIV1.Views.Candidato;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ProyectoDIV1.ViewModels.Buscadores
{
    [QueryProperty(nameof(Texto), nameof(Texto))]
    public class BusquedaJobViewModel : BaseViewModel
    {
        private string _texto;
        private ECandidato _candidato = new ECandidato();
        private TraductorService _traductor;
        private JobAndSkillService serviceJobsandSkills = new JobAndSkillService();
        private ObservableCollection<Job> _tiposDeJobs;
        public BusquedaJobViewModel()
        {
            _traductor = new TraductorService();
            _candidato = JsonConvert.DeserializeObject<ECandidato>(Settings.Candidato);
            BackCommand = new Command(BackClicked);
            InsertarCommand = new Command(async (param) => await ExecuteInsertarJob(param));
        }

        private async void BackClicked()
        {
            await Shell.Current.GoToAsync("..");

        }

        private async Task ExecuteInsertarJob(object param)
        {

            var job = param as Job;
            try
            {
                UserDialogs.Instance.ShowLoading("guardando...");
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
                        await Shell.Current.GoToAsync("..");
                        var query = await new CandidatoService().GetCandidatoFirebaseObjectAsync(_candidato.UsuarioId);
                        await new FirebaseHelper().UpdateAsync(_candidato, Constantes.COLLECTION_CANDIDATO, query);
                        Settings.Usuario = JsonConvert.SerializeObject(_candidato);
                        await Shell.Current.GoToAsync($"../../{nameof(EditarHojaDeVidaPage)}");
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
                UserDialogs.Instance.HideLoading();
            }
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
        public Command BackCommand { get; set; }
        public Command InsertarCommand { get; set; }
        public ObservableCollection<Job> TiposDeJobs
        {
            get { return _tiposDeJobs; }
            set { SetProperty(ref _tiposDeJobs, value); }
        }
        private async void LoadJobs(string value)
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
    }
}
