using Acr.UserDialogs;
using Newtonsoft.Json;
using ProyectoDIV1.Entidades.Models;
using ProyectoDIV1.Helpers;
using ProyectoDIV1.Models;
using ProyectoDIV1.Services;
using ProyectoDIV1.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Xamarin.Forms;

namespace ProyectoDIV1.ViewModels
{
    public class PerfilTrabajoViewModel : BaseViewModel
    {
        #region attributes
        private ObservableCollection<Job> _tiposDeJobs;
        private readonly FirebaseHelper _firebase;
        private ECandidato _candidatoReceived = new ECandidato();
        private List<string> _habilidades = new List<string>();
        private ObservableCollection<Lista> _listaHabilidades;
        private ObservableCollection<Skill> _tiposDeKills;

        #endregion

        #region constructor
        public PerfilTrabajoViewModel()
        {
            _firebase = new FirebaseHelper();
            CandidatoReceived = JsonConvert.DeserializeObject<ECandidato>(Settings.Candidato);
            LoadJobs();
            _listaHabilidades = new ObservableCollection<Lista>{
               new Lista()
               {
                   Nombre = "diego"
               },
                new Lista()
               {
                   Nombre = "Laura"
               },
                    new Lista()
               {
                   Nombre = "Erika"
               },
            };
            RegisterCommand = new Command(RegistrarClicked);
            SignInCommand = new Command(OnSignInClicked);
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

            InsertarCommand =
                new Command((param) =>
                {
                    var habilidad = param as Lista;
                    if (habilidad != null)
                    {
                        _habilidades.Add(habilidad.Nombre);
                    }
                });
        }
        #endregion

        #region commands
        public Command SearchCommand { get; set; }
        public Command RegisterCommand { get; set; }
        public Command SignInCommand { get; set; }
        public Command InsertarCommand { get; set; }
        #endregion

        #region Properties
        public ECandidato CandidatoReceived
        {
            get { return _candidatoReceived; }
            set { SetProperty(ref _candidatoReceived, value); }
        }
        public ObservableCollection<Job> TiposDeJobs
        {
            get { return _tiposDeJobs; }
            set { SetProperty(ref _tiposDeJobs, value); }
        }

        public ObservableCollection<Lista> ListaHabilidades
        {
            get { return _listaHabilidades; }
            set { SetProperty(ref _listaHabilidades, value); }
        }

        public ObservableCollection<Skill> TiposDeKills
        {
            get { return _tiposDeKills; }
            set { SetProperty(ref _tiposDeKills, value); }
        }

        public List<string> ListaHabi
        {
            get { return _habilidades; }
            set { SetProperty(ref _habilidades, value); }
        }
        #endregion

        #region methods
        private async void LoadJobs()
        {
            var serviceJobs = new ServiceJobs();
            var lista = await serviceJobs.GetListJobsAsync("jobs/autocomplete?begins_with=software");
            TiposDeJobs = new ObservableCollection<Job>(lista);
        }



        private async void OnSignInClicked()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new LoginPage());
        }

        private async void RegistrarClicked()
        {
            try
            {
                UserDialogs.Instance.ShowLoading("cargando...");

                CandidatoReceived.Habilidades = ListaHabi;
                await _firebase.CrearAsync<ECandidato>(CandidatoReceived, Constantes.COLLECTION_CANDIDATO);
                UserDialogs.Instance.HideLoading();
                UserDialogs.Instance.Toast("se ha registrado satisfactoriamente", TimeSpan.FromSeconds(2));
                App.Current.MainPage = new AppShell();
                await Shell.Current.GoToAsync($"//{nameof(AboutPage)}");
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                Debug.WriteLine(ex.Message);
            }

        }
        #endregion
    }
}
