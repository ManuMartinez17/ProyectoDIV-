using Newtonsoft.Json;
using ProyectoDIV1.DTOs;
using ProyectoDIV1.Helpers;
using ProyectoDIV1.Interfaces;
using Xamarin.Forms;

namespace ProyectoDIV1.ViewModels
{
    public class MasterCandidatoViewModel : BaseViewModel
    {
        private CandidatoDTO _candidato;

        public MasterCandidatoViewModel()
        {
            LoadCandidato();
            OnSignOut = new Command(OnSignOutClicked);
        }

        private void OnSignOutClicked()
        {
            var authService = DependencyService.Resolve<IAuthenticationService>();
            authService.SignOut();
            Shell.Current.GoToAsync("//LoginPage");
        }

        public Command OnSignOut { get; set; }

        private void LoadCandidato()
        {
            CandidatoDTO candidato = JsonConvert.DeserializeObject<CandidatoDTO>(Settings.Usuario);
            Candidato = candidato;
        }

        public CandidatoDTO Candidato
        {
            get => _candidato;
            set => SetProperty(ref _candidato, value);
        }
    }
}
