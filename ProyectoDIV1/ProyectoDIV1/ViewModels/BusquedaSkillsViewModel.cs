using Acr.UserDialogs;
using Newtonsoft.Json;
using ProyectoDIV1.Entidades.Models;
using ProyectoDIV1.Helpers;
using ProyectoDIV1.Interfaces;
using ProyectoDIV1.Models;
using ProyectoDIV1.Services;
using ProyectoDIV1.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ProyectoDIV1.ViewModels
{
    [QueryProperty(nameof(Texto), nameof(Texto))]
    public class BusquedaSkillsViewModel : BaseViewModel
    {
        private string _texto;
        private ECandidato _candidato = new ECandidato();
        private JobAndSkillService serviceJobsandSkills = new JobAndSkillService();
        private ObservableCollection<Skill> _tiposDeKills;

        public BusquedaSkillsViewModel()
        {

            _candidato = JsonConvert.DeserializeObject<ECandidato>(Settings.Candidato);
            InsertarCommand = new Command((param) => ExecuteInsertarSkills(param));
            BorrarCommand = new Command((param) => ExecuteBorrarSkills(param));
            GuardarCommand = new Command(OnGuardarClicked);
        }

        private void ExecuteBorrarSkills(object param)
        {
            var habilidad = param as string;
            if (habilidad != null && _candidato != null)
            {
                var query = _candidato.Habilidades.Where(x => x.Nombre.Equals(habilidad)).FirstOrDefault();
                if (query != null)
                {
                    _candidato.Habilidades.Remove(query);
                }
                Settings.Candidato = JsonConvert.SerializeObject(_candidato);
            }
        }
        private void ExecuteInsertarSkills(object param)
        {
            var habilidad = param as string;
            if (habilidad != null)
            {
                Lista item = new Lista()
                {
                    Nombre = habilidad
                };
                if (_candidato == null)
                {
                    _candidato = new ECandidato();
                }
                _candidato.Habilidades.Add(item);
                Settings.Candidato = JsonConvert.SerializeObject(_candidato);
            }

        }


        public Command InsertarCommand { get; set; }
        public Command BorrarCommand { get; set; }
        public Command GuardarCommand { get; set; }
        public ObservableCollection<Skill> TiposDeKills
        {
            get { return _tiposDeKills; }
            set { SetProperty(ref _tiposDeKills, value); }
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
                LoadSkills(value);
            }
        }

        private async void LoadSkills(string value)
        {
            var token = JsonConvert.DeserializeObject<Token>(Settings.Token);
            if (token != null && !string.IsNullOrWhiteSpace(value))
            {
                var lista = await serviceJobsandSkills.GetListJobsRelatedSkills($"skills/versions/latest/skills?q=.{value}", token.access_token);
                TiposDeKills = new ObservableCollection<Skill>(lista.data);
            }
        }

        private async void OnGuardarClicked()
        {
            if (_candidato != null)
            {
                if (_candidato.Habilidades.Count == 0)
                {
                    UserDialogs.Instance.Alert("Guarde por lo menos una habilidad.");
                    return;
                }
                var authenticationService = DependencyService.Resolve<IAuthenticationService>();

                if (authenticationService.IsSignIn())
                {
                    UserDialogs.Instance.ShowLoading("guardando...");
                    var query = await new CandidatoService().GetCandidatoFirebaseObjectAsync(_candidato.UsuarioId);
                    await new FirebaseHelper().UpdateAsync<ECandidato>(_candidato, Constantes.COLLECTION_CANDIDATO, query);
                    Settings.Usuario = JsonConvert.SerializeObject(_candidato);
                    UserDialogs.Instance.HideLoading();
                    await Shell.Current.GoToAsync($"../../{nameof(EditarHojaDeVidaPage)}");
                }
                else
                {
                    App.Current.MainPage = new AppShell();
                    await Shell.Current.GoToAsync($"//{nameof(PerfilTrabajoPage)}");
                }
            }

            else
            {
                UserDialogs.Instance.Alert("Guarde por lo menos una habilidad.");
                return;
            }
        }


    }
}
