using Acr.UserDialogs;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ProyectoDIV1.Entidades.Models;
using ProyectoDIV1.Helpers;
using ProyectoDIV1.Services.FirebaseServices;
using ProyectoDIV1.Services.Helpers;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Xamarin.Forms;

namespace ProyectoDIV1.ViewModels.Notificaciones
{
    public class PopupEnviarNotificacionViewModel : BaseViewModel
    {
        private string _mensaje;
        private string _id;
        private ECandidato _candidatoReceptor;
        private ECandidato _candidatoEmisor;
        private EEmpresa _empresaReceptor;
        private EEmpresa _empresaEmisor;
        private CandidatoService _candidatoService;
        private EmpresaService _empresaService;
        public PopupEnviarNotificacionViewModel()
        {
            _mensaje = "Buen día para solicitar de sus servicios.";
            _candidatoEmisor = new ECandidato();
            _empresaEmisor = new EEmpresa();
            _candidatoReceptor = new ECandidato();
            _empresaReceptor = new EEmpresa();
            _candidatoService = new CandidatoService();
            _empresaService = new EmpresaService();
            CargarEmisor();
            EnviarMensajeCommand = new Command(EnviarClicked);
        }

        private async void EnviarClicked()
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(Mensaje))
                {
                    UserDialogs.Instance.ShowLoading("Enviando...");
                    ENotificacion notificacion = new ENotificacion();
                    
                    if (_candidatoReceptor != null)
                    {
                        _candidatoReceptor.Notificaciones = new List<ENotificacion>();
                        notificacion.Id = Guid.NewGuid();
                        notificacion.EmisorId = _candidatoEmisor.UsuarioId;
                        notificacion.Fecha = DateTime.Now;
                        notificacion.Estado = false;
                        notificacion.Mensaje = Mensaje;
                        _candidatoReceptor.Notificaciones.Add(notificacion);
                        var query = await _candidatoService.GetCandidatoFirebaseObjectAsync(_candidatoReceptor.UsuarioId);
                        await _candidatoService.UpdateAsync(_candidatoReceptor, Constantes.COLLECTION_CANDIDATO, query);
                    }
                    else if (_empresaReceptor != null)
                    {
                        _empresaEmisor.Notificaciones = new List<ENotificacion>();
                        notificacion.Id = Guid.NewGuid();
                        notificacion.EmisorId = _empresaEmisor.UsuarioId;
                        notificacion.Fecha = DateTime.Now;
                        notificacion.Estado = false;
                        notificacion.Mensaje = Mensaje;
                        _empresaReceptor.Notificaciones.Add(notificacion);
                        var query = await _empresaService.GetEmpresaFirebaseObjectAsync(_empresaReceptor.UsuarioId);
                        await _empresaService.UpdateAsync(_empresaReceptor, Constantes.COLLECTION_EMPRESA, query);
                    }
                    UserDialogs.Instance.HideLoading();
                    Toasts. Success("Enviado.", 2000);
                    await PopupNavigation.Instance.PopAllAsync();

                }
                else
                {
                    Toasts.Error("Tiene que enviar un mensaje", 2000);
                }
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                Debug.WriteLine(ex.Message);
            }

        }

        public Command EnviarMensajeCommand { get; set; }

        public string IdReceptor
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;

                CargarReceptor(value);
            }
        }
        public string Mensaje
        {
            get { return _mensaje; }
            set { SetProperty(ref _mensaje, value); }
        }
        private async void CargarReceptor(string value)
        {
            try
            {
                Guid id = new Guid(value);
                var objetoEmpresa = await _empresaService.GetEmpresaAsync(id);
                if (objetoEmpresa != null)
                {
                    _candidatoReceptor = null;
                    _empresaReceptor = objetoEmpresa;

                }
                else
                {
                    var objetoCandidato = await _candidatoService.GetCandidatoAsync(id);
                    if (objetoCandidato != null)
                    {
                        _empresaReceptor = null;
                        _candidatoReceptor = objetoCandidato;
                    }
                }
            }
            catch (Exception ex)
            {

                Debug.WriteLine(ex.Message);
            }
        }

        private void CargarEmisor()
        {
            JObject Jobject = JObject.Parse(Settings.Usuario);
            try
            {
                var objetoCandidato = Jobject.ToObject(typeof(ECandidato));
                var objetoEmpresa = Jobject.ToObject(typeof(EEmpresa));
                if (objetoCandidato is ECandidato)
                {
                    _candidatoEmisor = objetoCandidato as ECandidato;
                }
                else if (objetoEmpresa is EEmpresa)
                {
                    _empresaEmisor = objetoEmpresa as EEmpresa;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}
