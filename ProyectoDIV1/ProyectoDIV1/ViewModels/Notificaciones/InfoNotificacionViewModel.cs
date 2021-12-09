using Newtonsoft.Json;
using ProyectoDIV1.DTOs;
using ProyectoDIV1.Entidades.Models;
using ProyectoDIV1.Helpers;
using ProyectoDIV1.Services.FirebaseServices;
using ProyectoDIV1.Services.Helpers;
using ProyectoDIV1.Views;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ProyectoDIV1.ViewModels.Notificaciones
{
    public class InfoNotificacionViewModel : BaseViewModel
    {
        private NotificacionDTO _notificacion;
        private NotificacionesService notificacionService;
        private CandidatoService candidatoService;
        private EmpresaService empresaService;
        private HelpDTO help;
        private string _mensaje;
        private DateTime _fecha;
        private string _fullName;
        private string _isAccepted;
        private string _iconAccepted;
        private bool _isvisible = false;


        public InfoNotificacionViewModel()
        {
            help = JsonConvert.DeserializeObject<HelpDTO>(Settings.Token);
            _notificacion = new NotificacionDTO();
            notificacionService = new NotificacionesService();
            candidatoService = new CandidatoService();
            empresaService = new EmpresaService();
            AceptarContratoCommand = new Command(AceptarContratoClicked);
            RechazarContratoCommand = new Command(RechazarContratoClicked);
            LoadReceptor();
        }

        public void OnApperaing()
        {
            LoadNotificacion();
            ActualizarEstadoNotificacion();
        }
        public Command AceptarContratoCommand { get; }
        public Command RechazarContratoCommand { get; }


        public string Mensaje
        {
            get { return _mensaje; }
            set { SetProperty(ref _mensaje, value); }
        }

        public DateTime Fecha
        {
            get { return _fecha; }
            set { SetProperty(ref _fecha, value); }
        }
        public string FullName
        {
            get { return _fullName; }
            set { SetProperty(ref _fullName, value); }
        }

        public string IsAccepted
        {
            get { return _isAccepted; }
            set { SetProperty(ref _isAccepted, value); }
        }
        public string IconIsAccepted
        {
            get { return _iconAccepted; }
            set { SetProperty(ref _iconAccepted, value); }
        }

        public bool IsVisible
        {
            get { return _isvisible; }
            set { SetProperty(ref _isvisible, value); }
        }
        private async void ActualizarEstadoNotificacion(bool acepto = false, bool rechazo = false)
        {
            try
            {
                if (help != null)
                {
                    Guid id = new Guid(help.UsuarioId);
                    Guid idNotificacion = new Guid(help.IdNotificacion);
                    var query = await candidatoService.GetCandidatoFirebaseObjectAsync(id);
                    var queryEmpresa = await empresaService.GetEmpresaFirebaseObjectAsync(id);
                    if (query != null)
                    {
                        var candidato = await candidatoService.GetCandidatoAsync(id);
                        foreach (var item in candidato.Notificaciones)
                        {
                            if (item.Id == idNotificacion)
                            {
                                if (acepto)
                                {
                                    item.EstadoAceptado = true;
                                }
                                else if (rechazo)
                                {
                                    item.EstadoRechazado = true;
                                }
                                item.EstadoVisto = true;
                                break;
                            }
                        }
                        await notificacionService.UpdateAsync(candidato, Constantes.COLLECTION_CANDIDATO, query);
                    }
                    else if (queryEmpresa != null)
                    {
                        var empresa = await empresaService.GetEmpresaAsync(id);
                        empresa.Notificaciones.ForEach(x =>
                        {
                            if (x.Id == idNotificacion)
                            {
                                if (acepto)
                                {
                                    x.EstadoAceptado = true;
                                }
                                else if (rechazo)
                                {
                                    x.EstadoRechazado = true;
                                }
                                x.EstadoVisto = true;
                                return;
                            }
                        });
                        await notificacionService.UpdateAsync(empresa, Constantes.COLLECTION_EMPRESA, queryEmpresa);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

        }

        private async void AceptarContratoClicked(object obj)
        {
            await PopupNavigation.Instance.PushAsync(new PopupLoadingPage("enviando notificación..."));
            try
            {
                await EnviarNotificacion(true);
                ActualizarEstadoNotificacion(true);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                await PopupNavigation.Instance.PopAsync();
                Toasts.Success("Aceptado.", 2000);
                await Shell.Current.GoToAsync("..");
            }

        }

        private async Task EnviarNotificacion(bool SiAcepto)
        {
            ENotificacion notificacion = new ENotificacion();

            if (_notificacion.CandidatoEmisor != null)
            {
                if (_notificacion.CandidatoEmisor.Notificaciones == null)
                {
                    _notificacion.CandidatoEmisor.Notificaciones = new List<ENotificacion>();
                }
                notificacion.Id = Guid.NewGuid();
               
                notificacion.EmisorId = new Guid(help.UsuarioId);
                notificacion.Fecha = DateTime.Now;
                notificacion.EstadoVisto = false;
                if (SiAcepto)
                {
                    notificacion.EstadoAceptado = true;
                    notificacion.Mensaje = "Solicitud Aceptada.";
                }
                else
                {
                    notificacion.EstadoRechazado = true;
                    notificacion.Mensaje = "Solicitud Rechazada.";
                }


                _notificacion.CandidatoEmisor.Notificaciones.Add(notificacion);
                var query = await candidatoService.GetCandidatoFirebaseObjectAsync(_notificacion.CandidatoEmisor.UsuarioId);
                await candidatoService.UpdateAsync(_notificacion.CandidatoEmisor, Constantes.COLLECTION_CANDIDATO, query);
            }
            else if (_notificacion.EmpresaEmisor != null)
            {
                if (_notificacion.EmpresaEmisor.Notificaciones == null)
                {
                    _notificacion.EmpresaEmisor.Notificaciones = new List<ENotificacion>();
                }
                notificacion.Id = Guid.NewGuid();
                notificacion.EmisorId = new Guid(help.UsuarioId);
                notificacion.Fecha = DateTime.Now;
                notificacion.EstadoVisto = false;
                if (SiAcepto)
                {
                    notificacion.EstadoAceptado = true;
                    notificacion.Mensaje = "Solicitud Aceptada.";
                }
                else
                {
                    notificacion.EstadoRechazado = true;
                    notificacion.Mensaje = "Solicitud Rechazada.";
                }
                _notificacion.EmpresaEmisor.Notificaciones.Add(notificacion);
                var query = await empresaService.GetEmpresaFirebaseObjectAsync(_notificacion.EmpresaEmisor.UsuarioId);
                await empresaService.UpdateAsync(_notificacion.EmpresaEmisor, Constantes.COLLECTION_EMPRESA, query);
            }
        }

        private async void RechazarContratoClicked(object obj)
        {
            await PopupNavigation.Instance.PushAsync(new PopupLoadingPage("enviando notificación..."));
            try
            {
                await EnviarNotificacion(false);
                ActualizarEstadoNotificacion(false, true);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                await PopupNavigation.Instance.PopAsync();
                Toasts.Error("Rechazado.", 2000);
                await Shell.Current.GoToAsync("..");
            }
        }



        private async void LoadReceptor()
        {
            try
            {
                if (help != null)
                {
                    Guid id = new Guid(help.UsuarioId);
                    if (help.Collection.Equals(Constantes.COLLECTION_CANDIDATO))
                    {
                        _notificacion.CandidatoReceptor = await candidatoService.GetCandidatoAsync(id);

                    }
                    else if (help.Collection.Equals(Constantes.COLLECTION_EMPRESA))
                    {
                        _notificacion.EmpresaReceptor = await empresaService.GetEmpresaAsync(id);

                    }
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

        }

        private async void LoadNotificacion()
        {
            try
            {
                Guid idEmisor = new Guid(help.UsuarioEmisorId);
                Guid idReceptor = new Guid(help.UsuarioId);
                Guid id = new Guid(help.IdNotificacion);
                if (help.Collection.Equals(Constantes.COLLECTION_CANDIDATO))
                {
                    var notificacionCandidato = await notificacionService.GetNotificacionCandidatoEmisorById(id, idEmisor, idReceptor);

                    _notificacion.CandidatoEmisor = notificacionCandidato.CandidatoEmisor;
                    _notificacion.Notificacion = notificacionCandidato.Notificacion;
                    FullName = $"{_notificacion.CandidatoEmisor.Nombre} {_notificacion.CandidatoEmisor.Apellido}";
                    Fecha = _notificacion.Notificacion.Fecha;
                    Mensaje = _notificacion.Notificacion.Mensaje;
                    IsAccepted = _notificacion.Notificacion.EstadoAceptado == false && _notificacion.Notificacion.EstadoRechazado == false
                       ? "Pendiente" : _notificacion.Notificacion.EstadoRechazado
                        == true ? "rechazado" : _notificacion.Notificacion.ContratoTerminado == false ? "Aceptado." : "Contrato finalizado.";
                    IconIsAccepted = _notificacion.Notificacion.EstadoAceptado == false && _notificacion.Notificacion.EstadoRechazado 
                        == false ? "icon_pending.png" : _notificacion.Notificacion.EstadoRechazado
                        == true ? "icon_rechazing.png" : _notificacion.Notificacion.ContratoTerminado == false ? "icon_checked.png" :
                        "icon_finished.png";
                    IsVisible = _notificacion.Notificacion.EstadoAceptado == false &&
                       _notificacion.Notificacion.EstadoRechazado == false ? true : _notificacion.Notificacion.EstadoRechazado == true
                       || _notificacion.Notificacion.EstadoAceptado == true ? false : true;
                    _notificacion.EmpresaEmisor = null;
                }
                else if (help.Collection.Equals(Constantes.COLLECTION_EMPRESA))
                {
                    var notificacionCandidato = await notificacionService.GetNotificacionEmpresaEmisorById(id, idEmisor, idReceptor);

                    _notificacion.EmpresaEmisor = notificacionCandidato.EmpresaEmisor;
                    _notificacion.Notificacion = notificacionCandidato.Notificacion;
                    FullName = $"{_notificacion.EmpresaEmisor.RazonSocial} Nit: {_notificacion.EmpresaEmisor.Nit}";
                    Fecha = _notificacion.Notificacion.Fecha;
                    Mensaje = _notificacion.Notificacion.Mensaje;
                    IsAccepted = _notificacion.Notificacion.EstadoAceptado == false && _notificacion.Notificacion.EstadoRechazado == false
                        ? "Pendiente" : _notificacion.Notificacion.EstadoRechazado
                         == true ? "rechazado" : _notificacion.Notificacion.ContratoTerminado == false ? "Aceptado." : "Contrato finalizado.";
                    IconIsAccepted = _notificacion.Notificacion.EstadoAceptado == false && 
                        _notificacion.Notificacion.EstadoRechazado == false ? "icon_pending.png" : _notificacion.Notificacion.EstadoRechazado
                        == true ? "icon_rechazing.png" : _notificacion.Notificacion.ContratoTerminado == false ? "icon_checked.png" :
                        "icon_finished.png";
                    IsVisible = _notificacion.Notificacion.EstadoAceptado == false &&
                         _notificacion.Notificacion.EstadoRechazado == false ? true : _notificacion.Notificacion.EstadoRechazado == true
                         || _notificacion.Notificacion.EstadoAceptado == true ? false : true;
                    _notificacion.CandidatoEmisor = null;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }


        }
    }
}
