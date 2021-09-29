using Dasync.Collections;
using Newtonsoft.Json;
using ProyectoDIV1.DTOs;
using ProyectoDIV1.Entidades.Models;
using ProyectoDIV1.Helpers;
using ProyectoDIV1.Services.FirebaseServices;
using ProyectoDIV1.Services.Helpers;
using ProyectoDIV1.Views;
using ProyectoDIV1.Views.Notificaciones;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ProyectoDIV1.ViewModels.Notificaciones
{
    public class NotificacionesCandidatosViewModel : BaseViewModel
    {
        private NotificacionesService _notificacionesService;
        private ObservableCollection<NotificacionDTO> _notificaciones;
        private ECandidato _candidato;
        private CandidatoService candidatoService;
        public NotificacionesCandidatosViewModel()
        {
            candidatoService = new CandidatoService();
            _candidato = JsonConvert.DeserializeObject<ECandidato>(Settings.Usuario);
            _notificacionesService = new NotificacionesService();
            MoreInformationCommand = new Command<object>(NotificacionSelected, CanNavigate);
        }
        public Command MoreInformationCommand { get; }
        private bool CanNavigate(object argument)
        {
            return true;
        }
        private async void NotificacionSelected(object objeto)
        {
            var lista = objeto as Syncfusion.ListView.XForms.ItemTappedEventArgs;
            var notificacion = lista.ItemData as NotificacionDTO;
            if (notificacion == null)
                return;

            HelpDTO help = new HelpDTO
            {
                UsuarioId = _candidato.UsuarioId.ToString(),
                UsuarioEmisorId = notificacion.CandidatoEmisor.UsuarioId.ToString(),
                Collection = Constantes.COLLECTION_CANDIDATO,
                IdNotificacion = notificacion.Notificacion.Id.ToString()
            };
            Settings.Token = JsonConvert.SerializeObject(help);
            await Shell.Current.GoToAsync($"{nameof(InfoNotificacionPage)}");
        }

        public void OnAppearing()
        {
            LoadNotificaciones();
        }

        private async void LoadNotificaciones()
        {
            List<NotificacionDTO> lista = new List<NotificacionDTO>();
            await PopupNavigation.Instance.PushAsync(new PopupLoadingPage());
            try
            {
                if (_candidato != null)
                {
                    var notificaciones = await _notificacionesService.GetNotificacionesCandidatos(_candidato.UsuarioId);

                    await notificaciones.ParallelForEachAsync(async item =>
                    {
                        var candidatoEmisor = await insertarCandidatoEmisor(item.EmisorId);

                        lista.Add(new NotificacionDTO()
                        {
                            CandidatoEmisor = candidatoEmisor,
                            Notificacion = item
                        });
                    }, maxDegreeOfParallelism: 10);

                    Notificaciones = new ObservableCollection<NotificacionDTO>(lista);
                }
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

        private async Task<ECandidato> insertarCandidatoEmisor(Guid emisorId)
        {
            return await candidatoService.GetCandidatoAsync(emisorId);

        }

        public ObservableCollection<NotificacionDTO> Notificaciones
        {
            get { return _notificaciones; }
            set { SetProperty(ref _notificaciones, value); }
        }
    }
}
