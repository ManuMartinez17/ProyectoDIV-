using Acr.UserDialogs;
using Newtonsoft.Json;
using ProyectoDIV1.DTOs;
using ProyectoDIV1.Entidades.Models;
using ProyectoDIV1.Helpers;
using ProyectoDIV1.Services.FirebaseServices;
using ProyectoDIV1.Views;
using ProyectoDIV1.Views.Candidato;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ProyectoDIV1.ViewModels.Candidato
{
    public class CandidatosConServiciosViewModel : BaseViewModel
    {
        private ObservableCollection<string> _profesiones;
        private ObservableCollection<CandidatoDTO> _candidatos;
        private CandidatoService _candidatoService;
        public CandidatosConServiciosViewModel()
        {
            _candidatoService = new CandidatoService();
            MostrarListadoCandidatosCommand = new Command((param) => ExecuteListadoCandidatosPorServicio(param));
            MoreInformationCommand = new Command<object>(CandidatoSelected, CanNavigate);
            LoadCandidatosCommand = new Command(async () => await RefreshCandidatos());
        }
        private bool CanNavigate(object argument)
        {
            return true;
        }
        private async void CandidatoSelected(object objeto)
        {
            var lista = objeto as Syncfusion.ListView.XForms.ItemTappedEventArgs;
            var candidato = lista.ItemData as CandidatoDTO;
            if (candidato == null)
                return;
            await Shell.Current.GoToAsync($"{nameof(CandidatoPage)}?{nameof(CandidatoViewModel.Id)}={candidato.Candidato.UsuarioId}");
        }

        public Command LoadCandidatosCommand { get; }
        public Command<Object> MoreInformationCommand { get; set; }
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
        private async Task RefreshCandidatos()
        {
            IsBusy = true;
            try
            {
                await MostrarCandidatos();

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

        private async void LoadCandidatos()
        {
            UserDialogs.Instance.ShowLoading("Cargando...");
            try
            {
                await MostrarCandidatos();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
        }

        private async Task MostrarCandidatos()
        {
            if (Candidatos != null)
            {
                Candidatos.Clear();
            }
            var candidatos = await _candidatoService.GetCandidatos();

            var user = JsonConvert.DeserializeObject<ECandidato>(Settings.Usuario);
            var candidatoIam = candidatos.Find(x => x.UsuarioId == user.UsuarioId);
            if (candidatoIam != null)
            {
                candidatos.Remove(candidatoIam);
            }
            List<CandidatoDTO> candidatoDTOs = new List<CandidatoDTO>();
            candidatos.ForEach(x => candidatoDTOs.Add(new CandidatoDTO
            {
                Candidato = x
            }));
           
            Candidatos = new ObservableCollection<CandidatoDTO>(candidatoDTOs);
        }

        public void OnAppearing()
        {
            LoadProfesiones();
            LoadCandidatos();
        }
        private async void LoadProfesiones()
        {
            var profesiones = await _candidatoService.GetProfesiones();
            Profesiones = new ObservableCollection<string>(profesiones);
        }

        private async void ExecuteListadoCandidatosPorServicio(object param)
        {
            string profesion = param as string;
            try
            {
                if (!string.IsNullOrEmpty(profesion))
                {
                    await PopupNavigation.Instance.PushAsync(new PopupLoadingPage());
                    var candidatos = await _candidatoService.GetCandidatosPorServicio(profesion);
                    List<CandidatoDTO> candidatoDTOs = new List<CandidatoDTO>();
                    candidatos.ForEach(x => candidatoDTOs.Add(new CandidatoDTO
                    {
                        Candidato = x
                    }));
                    Candidatos = new ObservableCollection<CandidatoDTO>(candidatoDTOs);
                    await PopupNavigation.Instance.PopAsync();
                }
                else
                {
                    LoadCandidatos();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

    }
}
