using ProyectoDIV1.DTOs;
using ProyectoDIV1.Services.FirebaseServices;
using System;
using Xamarin.Forms;

namespace ProyectoDIV1.ViewModels
{
    [QueryProperty(nameof(Id), nameof(Id))]
    public class CandidatoViewModel : BaseViewModel
    {
        private string _id;
        private CandidatoService _candidatoService;
        private CandidatoDTO _candidato;

        public CandidatoViewModel()
        {
            _candidatoService = new CandidatoService();
        }
        public string Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;

                LoadCandidato(value);
            }
        }

        public CandidatoDTO Candidato
        {
            get { return _candidato; }
            set
            {
                SetProperty(ref _candidato, value);
            }
        }

        private async void LoadCandidato(string value)
        {
            var id = new Guid(value);
            var candidato = await _candidatoService.GetCandidatoAsync(id);
            if (candidato == null)
            {
                return;
            }
            Candidato = new CandidatoDTO()
            {
                Candidato = candidato
            };
        }
    }
}
