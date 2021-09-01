using Newtonsoft.Json;
using ProyectoDIV1.Entidades.Models;
using ProyectoDIV1.Helpers;
using ProyectoDIV1.Services.FirebaseServices;
using ProyectoDIV1.Views;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace ProyectoDIV1.ViewModels.Notificaciones
{
    public class NotificacionesCandidatosViewModel : BaseViewModel
    {
        private NotificacionesService _notificacionesService;
        private ObservableCollection<ENotificacion> _notificaciones;
        private ECandidato _candidato;
        public NotificacionesCandidatosViewModel()
        {
            _candidato = JsonConvert.DeserializeObject<ECandidato>(Settings.Usuario);
            _notificacionesService = new NotificacionesService();
        }
        public void OnAppearing()
        {
            LoadNotificaciones();
        }

        private async void LoadNotificaciones()
        {
            await PopupNavigation.Instance.PushAsync(new PopupLoadingPage());
            try
            {
                if (_candidato != null)
                {
                    var notificaciones = await _notificacionesService.GetNotificacionesCandidatos(_candidato.UsuarioId);
                    Notificaciones = new ObservableCollection<ENotificacion>(notificaciones);
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

        public ObservableCollection<ENotificacion> Notificaciones
        {
            get { return _notificaciones; }
            set { SetProperty(ref _notificaciones, value); }
        }
    }
}
