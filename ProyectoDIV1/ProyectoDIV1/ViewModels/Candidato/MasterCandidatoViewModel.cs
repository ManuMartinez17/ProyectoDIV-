using Newtonsoft.Json;
using ProyectoDIV1.DTOs;
using ProyectoDIV1.Entidades.Models;
using ProyectoDIV1.Helpers;
using ProyectoDIV1.Services.FirebaseServices;
using ProyectoDIV1.Services.Interfaces;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ProyectoDIV1.ViewModels.Candidato
{
    public class MasterCandidatoViewModel : BaseViewModel
    {
        private CandidatoDTO _candidato;
        private CandidatoService _candidatoService;
        public MasterCandidatoViewModel()
        {
            _candidatoService = new CandidatoService();
            CheckWhetherTheUserIsSignIn();
        }

        public async void RefreshCandidato(Guid id)
        {
            var candidato = await _candidatoService.GetCandidatoAsync(id);
            if (candidato == null)
            {
                return;
            }
            CandidatoDTO candidatoDTO = new CandidatoDTO()
            {
                Candidato = candidato
            };
            Candidato = candidatoDTO;
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
                        LoadCandidato(candidato);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }
        private async Task<ECandidato> BuscarIdCandidato(string email)
        {

            try
            {
                var candidato = await _candidatoService.GetIdXEmail(email);
                return candidato;
            }
            catch (Exception ex)
            {

                Debug.WriteLine(ex.Message);
            }
            return default;
        }
        private void LoadCandidato(ECandidato candidato)
        {
            CandidatoDTO candidatoDTO = new CandidatoDTO()
            {
                Candidato = candidato
            };

            Candidato = candidatoDTO;
        }

        public CandidatoDTO Candidato
        {
            get => _candidato;
            set => SetProperty(ref _candidato, value);
        }
    }
}
