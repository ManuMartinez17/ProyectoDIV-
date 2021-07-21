using Newtonsoft.Json;
using ProyectoDIV1.Entidades.Models;
using ProyectoDIV1.Models;
using ProyectoDIV1.Services;
using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace ProyectoDIV1.ViewModels
{
    [QueryProperty("Candidato", "candidato")]
    public class PerfilTrabajoViewModel : BaseViewModel
    {
        private ObservableCollection<Job> _tiposDeJobs;

        private ECandidato _candidatoReceived = new ECandidato();

        private ObservableCollection<Skill> _tiposDeKills;


        public PerfilTrabajoViewModel()
        {
            LoadJobs();
            SearchCommand =
                new Command(async (param) =>
                {
                    var job = param as Job;
                    if (job != null)
                    {
                        var serviceJobs = new ServiceJobs();
                        var lista = await serviceJobs.GetListJobsRelatedSkills("jobs/", $"{job.uuid}/related_skills");
                        if (lista != null || lista.skills.Count > 0)
                        {
                            TiposDeKills = new ObservableCollection<Skill>(lista.skills);
                        }                    
                    }
                });
        }

        private async void LoadJobs()
        {
            var serviceJobs = new ServiceJobs();
            var lista = await serviceJobs.GetListJobsAsync("jobs/autocomplete?begins_with=software");
            TiposDeJobs = new ObservableCollection<Job>(lista);
        }

        public string Candidato
        {
            set => CandidatoReceived = JsonConvert.DeserializeObject<ECandidato>(Uri.UnescapeDataString(value));
        }

        public ECandidato CandidatoReceived
        {
            get { return _candidatoReceived; }
            set { SetProperty(ref _candidatoReceived, value); }
        }
        public Command SearchCommand { get; set; }
        public ObservableCollection<Job> TiposDeJobs
        {
            get { return _tiposDeJobs; }
            set { SetProperty(ref _tiposDeJobs, value); }
        }

        public ObservableCollection<Skill> TiposDeKills
        {
            get { return _tiposDeKills; }
            set { SetProperty(ref _tiposDeKills, value); }
        }

    }
}
