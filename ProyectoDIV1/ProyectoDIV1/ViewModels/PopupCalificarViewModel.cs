using Acr.UserDialogs;
using ProyectoDIV1.Helpers;
using ProyectoDIV1.Services.FirebaseServices;
using ProyectoDIV1.Services.Helpers;
using ProyectoDIV1.Views.Notificaciones;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Xamarin.Forms;

namespace ProyectoDIV1.ViewModels
{
    public class PopupCalificarViewModel : BaseViewModel
    {
        private CandidatoService _candidatoService;
        private EmpresaService _empresaService;
        private float _calificacion;
        private string _texto;
        public Guid IdReceptor { get; set; }
        public Guid IdNotificacion { get; set; }
        public Guid IdEmisor { get; set; }
        public float Calificacion
        {
            get { return _calificacion; }
            set { SetProperty(ref _calificacion, value); }
        }
        public string Texto
        {
            get { return _texto; }
            set { SetProperty(ref _texto, value); }
        }
        public PopupCalificarViewModel()
        {
            _candidatoService = new CandidatoService();
            _empresaService = new EmpresaService();
            EnviarCalificacionCommand = new Command(EnviarCalificacionClicked);
        }
        public Command EnviarCalificacionCommand { get; set; }
        private async void EnviarCalificacionClicked(object obj)
        {
            if (Calificacion == 0.0f)
            {
                Toasts.Error("Asigne una calificación.", 3000);
                return;
            }
            UserDialogs.Instance.ShowLoading("Enviando Calificación...");
            try
            {
                var candidato = await _candidatoService.GetCandidatoFirebaseObjectAsync(IdReceptor);
                var empresa = await _empresaService.GetEmpresaFirebaseObjectAsync(IdReceptor);
                if (candidato != null)
                {
                    var User = await _candidatoService.GetCandidatoAsync(IdReceptor);
                    if (User.Calificaciones == null)
                    {
                        User.Calificaciones = new List<float>();
                    }
                    User.Calificaciones.Add(Calificacion);
                    await _candidatoService.UpdateAsync(User, Constantes.COLLECTION_CANDIDATO, candidato);

                    EliminarNotificacionEmisor();
                    Toasts.Success("Enviado.", 2000);
                }
                else if (empresa != null)
                {
                    var User = await _empresaService.GetEmpresaAsync(IdReceptor);
                    if (User.Calificaciones == null)
                    {
                        User.Calificaciones = new List<float>();
                    }
                    User.Calificaciones.Add(Calificacion);
                    await _empresaService.UpdateAsync(User, Constantes.COLLECTION_EMPRESA, empresa);
                    EliminarNotificacionEmisor();
                    Toasts.Success("Enviado.", 2000);
                }
            }
            catch (Exception ex)
            {
                Toasts.Error("No se pudo enviar la calificación.", 2000);
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
                await PopupNavigation.Instance.PopAllAsync();
                await Shell.Current.GoToAsync($"..");

            }
        }

        private async void EliminarNotificacionEmisor()
        {
            var candidato = await _candidatoService.GetCandidatoFirebaseObjectAsync(IdEmisor);
            var empresa = await _empresaService.GetEmpresaFirebaseObjectAsync(IdEmisor);
            if (candidato != null)
            {
                var user = await _candidatoService.GetCandidatoAsync(IdEmisor);
                if (user.Notificaciones != null)
                {
                    var notificacion = user.Notificaciones.Where(x => x.Id == IdNotificacion).FirstOrDefault();
                    if (notificacion != null)
                    {
                        user.Notificaciones.Remove(notificacion);
                    }
                    await _candidatoService.UpdateAsync(user, Constantes.COLLECTION_CANDIDATO, candidato);
                }

            }
            else if (empresa != null)
            {
                var user = await _empresaService.GetEmpresaAsync(IdEmisor);
                if (user.Notificaciones != null)
                {
                    var notificacion = user.Notificaciones.Where(x => x.Id == IdNotificacion).FirstOrDefault();
                    if (notificacion != null)
                    {
                        user.Notificaciones.Remove(notificacion);
                    }
                    await _empresaService.UpdateAsync(user, Constantes.COLLECTION_EMPRESA, empresa);
                }
            }
        }


    }
}
