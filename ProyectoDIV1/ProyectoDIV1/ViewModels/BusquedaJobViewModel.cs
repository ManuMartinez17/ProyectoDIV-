using Acr.UserDialogs;
using Newtonsoft.Json;
using ProyectoDIV1.Entidades.Models;
using ProyectoDIV1.Helpers;
using ProyectoDIV1.Models;
using ProyectoDIV1.Services;
using ProyectoDIV1.Views;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ProyectoDIV1.ViewModels
{
    [QueryProperty(nameof(Texto), nameof(Texto))]
    public class BusquedaJobViewModel : BaseViewModel
    {
        private string _texto;
        private ECandidato _candidato = new ECandidato();
        private JobAndSkillService serviceJobsandSkills = new JobAndSkillService();
        private ObservableCollection<Job> _tiposDeJobs;
        public BusquedaJobViewModel()
        {
            _candidato = JsonConvert.DeserializeObject<ECandidato>(Settings.Candidato);
            InsertarCommand = new Command(async (param) => await ExecuteInsertarJob(param));
        }

        private async Task ExecuteInsertarJob(object param)
        {
            var job = param as Job;
            if (job != null)
            {
                if (_candidato == null)
                {
                    _candidato = new ECandidato();
                }
                _candidato.Profesion = job.name;
                Settings.Candidato = JsonConvert.SerializeObject(_candidato);
                Application.Current.MainPage = new NavigationPage();
                await Application.Current.MainPage.Navigation.PushAsync(new PerfilTrabajoPage());
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

        public Command InsertarCommand { get; set; }
        public ObservableCollection<Job> TiposDeJobs
        {
            get { return _tiposDeJobs; }
            set { SetProperty(ref _tiposDeJobs, value); }
        }
        private async void LoadJobs(string value)
        {
            var token = JsonConvert.DeserializeObject<Token>(Settings.Token);
            if (token != null && !string.IsNullOrWhiteSpace(value))
            {
                var lista = await serviceJobsandSkills.GetListJobsAsync($"titles/versions/latest/titles?q=.{value}", token.access_token);
                TiposDeJobs = new ObservableCollection<Job>(lista.data);
            }

        }
    }
}
