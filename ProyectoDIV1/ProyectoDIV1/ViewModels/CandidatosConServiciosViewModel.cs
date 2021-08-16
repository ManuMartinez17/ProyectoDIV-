using ProyectoDIV1.Entidades.Models;
using ProyectoDIV1.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;

namespace ProyectoDIV1.ViewModels
{
    public class CandidatosConServiciosViewModel : BaseViewModel
    {
        private ObservableCollection<string> _profesiones;
        private ObservableCollection<ECandidato> _candidatos;
        private CandidatoService _candidatoService;
        public CandidatosConServiciosViewModel()
        {
          
            _candidatoService = new CandidatoService();
            LoadProfesiones();
            MostrarListadoCandidatosCommand = new Command((param) => ExecuteListadoCandidatosPorServicio(param));
        }

        private async void LoadProfesiones()
        {
            var profesiones = await _candidatoService.GetProfesiones();
            Profesiones = new ObservableCollection<string>(profesiones);
        }

        private async void ExecuteListadoCandidatosPorServicio(object param)
        {
            string profesion = param as string;
            if (!string.IsNullOrEmpty(profesion))
            {
                var candidatos = await _candidatoService.GetCandidatosPorServicio(profesion);
                Candidatos = new ObservableCollection<ECandidato>(candidatos);
            }

        }

        public Command MostrarListadoCandidatosCommand { get; set; }

        public ObservableCollection<string> Profesiones
        {
            get { return _profesiones; }
            set { SetProperty(ref _profesiones, value); }
        }
        public ObservableCollection<ECandidato> Candidatos
        {
            get { return _candidatos; }
            set { SetProperty(ref _candidatos, value); }
        }
    }
}
