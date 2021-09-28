using Newtonsoft.Json;
using ProyectoDIV1.DTOs;
using ProyectoDIV1.Helpers;
using ProyectoDIV1.Services.FirebaseServices;
using ProyectoDIV1.ViewModels.Chat;
using ProyectoDIV1.Views;
using ProyectoDIV1.Views.Candidato;
using ProyectoDIV1.Views.Chat;
using ProyectoDIV1.Views.Notificaciones;
using Rg.Plugins.Popup.Services;
using System;
using System.Diagnostics;
using System.IO;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ProyectoDIV1.ViewModels.Candidato
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
            VerHojaDeVidaCommand = new Command(VerHojaDeVida);
            ContactarCommand = new Command(MostrarPopupCreateNotification);
        }

        private async void MostrarPopupCreateNotification()
        {
            await PopupNavigation.Instance.PushAsync(new PopupEnviarNotificacionPage(_candidato.Candidato.UsuarioId.ToString()));
        }

        private async void MostrarChat(object obj)
        {
            await PopupNavigation.Instance.PushAsync(new PopupLoadingPage());
            try
            {
                await Shell.Current.GoToAsync($"{nameof(ChatCandidatoPage)}?{nameof(ChatCandidatoViewModel.IdReceptor)}={_candidato.Candidato.UsuarioId}");

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                await PopupNavigation.Instance.PopAllAsync();
            }
        }

        private async void VerHojaDeVida(object obj)
        {

            try
            {
                if (string.IsNullOrWhiteSpace(_candidato.Candidato.Rutas.RutaArchivoRegistro))
                {
                    Toasts.Warning("El candidato no tiene hoja de vida por el momento.", 3000);
                    return;
                }
                var extension = Path.GetExtension(_candidato.Candidato.Rutas.NombreArchivoRegistro);
                if (extension.Contains("doc"))
                {
                    var url = new Uri(_candidato.Candidato.Rutas.RutaArchivoRegistro);
                    await Browser.OpenAsync(url, BrowserLaunchMode.SystemPreferred);
                }
                else if (extension.Contains("pdf"))
                {
                    await PopupNavigation.Instance.PushAsync(new PopupLoadingPage());
                    Settings.Url = JsonConvert.SerializeObject(_candidato.Candidato.Rutas.RutaArchivoRegistro);
                    await Shell.Current.GoToAsync(nameof(VerHojaDeVidaPage));
                    await PopupNavigation.Instance.PopAllAsync();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public Command VerHojaDeVidaCommand { get; }
        public Command ContactarCommand { get; }
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
