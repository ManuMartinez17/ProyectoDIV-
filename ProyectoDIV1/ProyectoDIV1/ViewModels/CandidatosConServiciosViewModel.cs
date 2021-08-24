using ProyectoDIV1.DTOs;
using ProyectoDIV1.Entidades.Models;
using ProyectoDIV1.Services;
using ProyectoDIV1.Views;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ProyectoDIV1.ViewModels
{
    public class CandidatosConServiciosViewModel : BaseViewModel
    {
        private ObservableCollection<string> _profesiones;
        private ObservableCollection<CandidatoDTO> _candidatos;
        private CandidatoService _candidatoService;
        public CandidatosConServiciosViewModel()
        {

            _candidatoService = new CandidatoService();
            LoadProfesiones();
            RefreshCandidatos();
            MostrarListadoCandidatosCommand = new Command((param) => ExecuteListadoCandidatosPorServicio(param));
            LoadCandidatosCommand = new Command(async () => await LoadCandidatos());
        }

        private async void RefreshCandidatos()
        {
            await PopupNavigation.Instance.PushAsync(new PopupLoadingPage());
            try
            {
                if (Candidatos != null)
                {
                    Candidatos.Clear();
                }
                var candidatos = await _candidatoService.GetCandidatos();
                List<CandidatoDTO> candidatoDTOs = new List<CandidatoDTO>();
                candidatos.ForEach(x => candidatoDTOs.Add(new CandidatoDTO
                {
                    Candidato = x
                }));
                Candidatos = new ObservableCollection<CandidatoDTO>(candidatoDTOs);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                await PopupNavigation.Instance.PopAsync();
            }
           
        }

        private async Task LoadCandidatos()
        {
            IsBusy = true;
            try
            {
                Candidatos.Clear();
                var candidatos = await _candidatoService.GetCandidatos();
                List<CandidatoDTO> candidatoDTOs = new List<CandidatoDTO>();
                candidatos.ForEach(x => candidatoDTOs.Add(new CandidatoDTO { Candidato = x }));
                Candidatos = new ObservableCollection<CandidatoDTO>(candidatoDTOs);

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
        public void OnAppearing()
        {
            IsBusy = true;
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
                List<CandidatoDTO> candidatoDTOs = new List<CandidatoDTO>();
                candidatos.ForEach(x => candidatoDTOs.Add(new CandidatoDTO { Candidato = x }));
                Candidatos = new ObservableCollection<CandidatoDTO>(candidatoDTOs);
            }

        }
        public Command LoadCandidatosCommand { get; }
        public Command MostrarListadoCandidatosCommand { get; set; }

        public ObservableCollection<string> Profesiones
        {
            get { return _profesiones; }
            set { SetProperty(ref _profesiones, value); }
        }
        public ObservableCollection<CandidatoDTO> Candidatos
        {
            get { return _candidatos; }
            set { SetProperty(ref _candidatos, value); }
        }
    }
}
