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
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ProyectoDIV1.ViewModels
{

    public class PerfilTrabajoViewModel : BaseViewModel
    {
        #region attributes
        private readonly FirebaseHelper _firebase;
        private Token _token;
        private ECandidato _candidatoReceived = new ECandidato();
        private ObservableCollection<Lista> _habilidades;
        JobAndSkillService serviceJobsandSkills = new JobAndSkillService();
        #endregion

        #region constructor
        public PerfilTrabajoViewModel()
        {
            _firebase = new FirebaseHelper();
            BuscarToken();
            GenerarToken();
            CandidatoReceived = JsonConvert.DeserializeObject<ECandidato>(Settings.Candidato);
            RegisterCommand = new Command(RegistrarClicked);
            SignInCommand = new Command(OnSignInClicked);
            SearchJobCommand = new Command(async (text) => await ExecuteBusquedaJob(text));
            SearchSkillsCommand = new Command(async (text) => await ExecuteBusquedaSkills(text));
        }

        private async Task ExecuteBusquedaJob(object text)
        {
            var texto = text as string;
            if (!string.IsNullOrWhiteSpace(texto))
            {
                UserDialogs.Instance.ShowLoading("Cargando...");
                await Task.Delay(1000);
                UserDialogs.Instance.HideLoading();
                await Shell.Current.GoToAsync($"{nameof(BusquedaJobPage)}?{nameof(BusquedaJobViewModel.Texto)}={texto}");
            }
        }
        private async Task ExecuteBusquedaSkills(object text)
        {
            var texto = text as string;
            if (!string.IsNullOrWhiteSpace(texto))
            {
                UserDialogs.Instance.ShowLoading("Cargando...");
                await Task.Delay(1000);
                UserDialogs.Instance.HideLoading();
                await Shell.Current.GoToAsync($"{nameof(BusquedaSkillsPage)}?{nameof(BusquedaSkillsViewModel.Texto)}={texto}");
            }
        }

        private void BuscarToken()
        {
            _token = JsonConvert.DeserializeObject<Token>(Settings.Token);
        }


        #endregion

        #region commands
        public Command RegisterCommand { get; set; }
        public Command SignInCommand { get; set; }
        public Command BorrarHabilidadCommand
        {
            get
            {
                return new Command<Lista>((skill) =>
               {

                   if (skill != null)
                   {
                       Habilidades.Remove(skill);
                       _candidatoReceived.Habilidades.Remove(skill);
                       Settings.Candidato = JsonConvert.SerializeObject(_candidatoReceived);
                   }
               });
            }
        }
        public Command SearchJobCommand { get; set; }
        public Command SearchSkillsCommand { get; set; }

        #endregion

        #region Properties
        public ECandidato CandidatoReceived
        {
            get { return _candidatoReceived; }
            set
            {
                Habilidades = value != null ? new ObservableCollection<Lista>(value.Habilidades) : null;
                SetProperty(ref _candidatoReceived, value);

            }
        }

        public ObservableCollection<Lista> Habilidades
        {
            get { return _habilidades; }
            set { SetProperty(ref _habilidades, value); }
        }
        #endregion

        #region methods
        private void GenerarToken()
        {
            if (_token == null)
            {
                var token = serviceJobsandSkills.GenerarToken();
                Settings.Token = JsonConvert.SerializeObject(token);
            }
            else
            {
                if (_token.Expiration < DateTime.Now)
                {
                    var token = serviceJobsandSkills.GenerarToken();
                    Settings.Token = JsonConvert.SerializeObject(token);
                }
            }
        }

        private async void OnSignInClicked()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new LoginPage());
        }

        private async void RegistrarClicked()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(CandidatoReceived.Expectativa))
                {
                    UserDialogs.Instance.Alert("Ingrese una expectativa.");
                    return;
                }
                if (string.IsNullOrWhiteSpace(CandidatoReceived.Profesion) || CandidatoReceived.Habilidades.Count == 0)
                {
                    UserDialogs.Instance.Alert("Tiene que tener una profesión, y una habilidad.");
                    return;
                }
                UserDialogs.Instance.ShowLoading("cargando...");
                CandidatoReceived.Habilidades = new List<Lista>();
                foreach (var item in Habilidades)
                {
                    CandidatoReceived.Habilidades.Add(item);
                }
                await _firebase.CrearAsync<ECandidato>(CandidatoReceived, Constantes.COLLECTION_CANDIDATO);
                UserDialogs.Instance.HideLoading();
                UserDialogs.Instance.Toast("se ha registrado satisfactoriamente", TimeSpan.FromSeconds(2));
                await Task.Delay(2000);
                Settings.Candidato = null;
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
