using Newtonsoft.Json;
using ProyectoDIV1.DTOs;
using ProyectoDIV1.Helpers;
using ProyectoDIV1.Interfaces;
using ProyectoDIV1.Services;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ProyectoDIV1.ViewModels
{
    public class MasterCandidatoViewModel : BaseViewModel
    {
        private CandidatoDTO _candidato;
        private readonly FirebaseHelper _firebaseHelper;
        public MasterCandidatoViewModel()
        {
            CheckWhetherTheUserIsSignIn();
            _firebaseHelper = new FirebaseHelper();

            OnSignOut = new Command(OnSignOutClicked);
        }

        private async void CheckWhetherTheUserIsSignIn()
        {
            try
            {
                var authenticationService = DependencyService.Resolve<IAuthenticationService>();
                if (authenticationService.IsSignIn())
                {
                    string email = authenticationService.BuscarEmail();
                    var candidato = await BuscarIdCandidato(email);
                    if (candidato != null)
                    {
                        Settings.Usuario = JsonConvert.SerializeObject(candidato);
                    }
                    LoadCandidato();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        private async Task<string> BuscarIdCandidato(string email)
        {
            var candidato = await new FirebaseHelper().GetUsuario("Candidatos", email);
            return candidato.UsuarioId.ToString();
        }

        private void OnSignOutClicked()
        {
            var authService = DependencyService.Resolve<IAuthenticationService>();
            authService.SignOut();
            Shell.Current.GoToAsync("//LoginPage");
        }

        public Command OnSignOut { get; set; }

        private async void LoadCandidato()
        {
            Guid id = JsonConvert.DeserializeObject<Guid>(Settings.Usuario);
            await BuscarCandidato(id);
        }


        private async Task BuscarCandidato(Guid id)
        {
            var usuario = await _firebaseHelper.GetCandidatoId("Candidatos", id);
            if (usuario != null)
            {
                CandidatoDTO candidato = new CandidatoDTO()
                {
                    Candidato = usuario

                };
                Candidato = candidato;
            }
        }

        public CandidatoDTO Candidato
        {
            get => _candidato;
            set => SetProperty(ref _candidato, value);
        }
    }
}
