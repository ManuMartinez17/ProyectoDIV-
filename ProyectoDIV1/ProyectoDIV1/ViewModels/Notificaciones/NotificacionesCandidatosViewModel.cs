using Dasync.Collections;
using Newtonsoft.Json;
using ProyectoDIV1.DTOs;
using ProyectoDIV1.Entidades.Models;
using ProyectoDIV1.Helpers;
using ProyectoDIV1.Services.FirebaseServices;
using ProyectoDIV1.Views;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

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
                       var candidatoEmisor =  await insertarCandidatoEmisor(item.EmisorId);

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
