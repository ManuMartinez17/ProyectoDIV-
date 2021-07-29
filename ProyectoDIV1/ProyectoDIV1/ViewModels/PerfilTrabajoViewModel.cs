using Acr.UserDialogs;
using Newtonsoft.Json;
using ProyectoDIV1.Entidades.Models;
using ProyectoDIV1.Helpers;
using ProyectoDIV1.Models;
using ProyectoDIV1.Services;
using ProyectoDIV1.Views;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ProyectoDIV1.ViewModels
{
    public class PerfilTrabajoViewModel : BaseViewModel
    {
        #region attributes
        private ObservableCollection<Job> _tiposDeJobs;
        private readonly FirebaseHelper _firebase;
        private ECandidato _candidatoReceived = new ECandidato();
        private ObservableCollection<Skill> _tiposDeKills;
        JobAndSkillService serviceJobsandSkills = new JobAndSkillService();
        private string _token;
        #endregion

        #region constructor
        public PerfilTrabajoViewModel()
        {
            _firebase = new FirebaseHelper();
            GenerarToken();
            CandidatoReceived = JsonConvert.DeserializeObject<ECandidato>(Settings.Candidato);
            RegisterCommand = new Command(RegistrarClicked);
            SignInCommand = new Command(OnSignInClicked);
            LoadJobs();
            loadSkills();
            InsertarCommand =
                new Command((param) =>
                {
                    var habilidad = param as Skill;
                    if (habilidad != null)
                    {
                        _candidatoReceived.Habilidades.Add(habilidad.name);
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

        public ObservableCollection<Skill> TiposDeKills
        {
            get { return _tiposDeKills; }
            set { SetProperty(ref _tiposDeKills, value); }
        }


        #endregion

        #region methods
        private void GenerarToken()
        {
            _token = serviceJobsandSkills.GenerarToken();
        }

        private async void LoadJobs(string param = "psy")
        {
            var lista = await serviceJobsandSkills.GetListJobsAsync($"titles/versions/latest/titles?q=.{param}", _token);
            TiposDeJobs = new ObservableCollection<Job>(lista.data);
        }

        private async void loadSkills(string param = "psy")
        {
            var lista = await serviceJobsandSkills.GetListJobsRelatedSkills($"skills/versions/latest/skills?q=.{param}", _token);
            TiposDeKills = new ObservableCollection<Skill>(lista.data);
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
                await _firebase.CrearAsync<ECandidato>(CandidatoReceived, Constantes.COLLECTION_CANDIDATO);
                UserDialogs.Instance.HideLoading();
                UserDialogs.Instance.Toast("se ha registrado satisfactoriamente", TimeSpan.FromSeconds(2));
                await Task.Delay(2000);
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
