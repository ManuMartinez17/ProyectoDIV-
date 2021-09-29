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
            ActualizarEstadoNotificacion();
        }

        public void OnApperaing()
        {
            LoadNotificacion();
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
        private async void ActualizarEstadoNotificacion(bool acepto = false)
        {
            try
            {
                if (help != null)
                {
                    Guid id = new Guid(help.UsuarioId);
                    Guid idNotificacion = new Guid(help.IdNotificacion);
                    if (help.Collection.Equals(Constantes.COLLECTION_CANDIDATO))
                    {
                        var query = await candidatoService.GetCandidatoFirebaseObjectAsync(id);
                        foreach (var item in _notificacion.CandidatoReceptor.Notificaciones)
                        {
                            if (item.Id == idNotificacion)
                            {
                                if (acepto)
                                {
                                    item.EstadoAceptado = true;
                                }
                                item.EstadoVisto = true;
                                break;
                            }
                        }
                        await notificacionService.UpdateAsync(_notificacion.CandidatoReceptor, help.Collection, query);
                        if (acepto)
                        {
                            await Shell.Current.GoToAsync("..");
                        }
                    }
                    else if (help.Collection.Equals(Constantes.COLLECTION_EMPRESA))
                    {
                        var query = await empresaService.GetEmpresaFirebaseObjectAsync(id);
                        _notificacion.EmpresaReceptor.Notificaciones.ForEach(x =>
                        {
                            if (x.Id == idNotificacion)
                            {
                                if (acepto)
                                {
                                    x.EstadoAceptado = true;
                                }
                                x.EstadoVisto = true;
                                return;
                            }
                        });
                        await notificacionService.UpdateAsync(_notificacion.EmpresaReceptor, help.Collection, query);
                        if (acepto)
                        {
                            await Shell.Current.GoToAsync("..");
                        }
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
            await PopupNavigation.Instance.PushAsync(new PopupLoadingPage("enviando notficación..."));
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
                notificacion.EmisorId = _notificacion.CandidatoReceptor.UsuarioId;
                notificacion.Fecha = DateTime.Now;
                notificacion.EstadoVisto = true;
                if (SiAcepto)
                {
                    notificacion.EstadoAceptado = true;
                    notificacion.Mensaje = "Solicitud Aceptada.";
                }
                else
                {
                    notificacion.Mensaje = "Solicitud Rechazada.";
                }
                _notificacion.CandidatoEmisor.Notificaciones.Add(notificacion);
                var query = await candidatoService.GetCandidatoFirebaseObjectAsync(_notificacion.CandidatoEmisor.UsuarioId);
                await candidatoService.UpdateAsync(_notificacion.CandidatoEmisor, Constantes.COLLECTION_CANDIDATO, query);
            }
            else if (_notificacion.EmpresaReceptor != null)
            {
                if (_notificacion.EmpresaReceptor.Notificaciones == null)
                {
                    _notificacion.EmpresaReceptor.Notificaciones = new List<ENotificacion>();
                }
                notificacion.Id = Guid.NewGuid();
                notificacion.EmisorId = _notificacion.EmpresaEmisor.UsuarioId;
                notificacion.Fecha = DateTime.Now;
                notificacion.EstadoVisto = true;
                if (SiAcepto)
                {
                    notificacion.Mensaje = "Solicitud Aceptada.";
                }
                else
                {
                    notificacion.Mensaje = "Solicitud Rechazada.";
                }
                _notificacion.EmpresaReceptor.Notificaciones.Add(notificacion);
                var query = await empresaService.GetEmpresaFirebaseObjectAsync(_notificacion.EmpresaReceptor.UsuarioId);
                await empresaService.UpdateAsync(_notificacion.EmpresaReceptor, Constantes.COLLECTION_EMPRESA, query);
            }
        }

        private async void RechazarContratoClicked(object obj)
        {
            await PopupNavigation.Instance.PushAsync(new PopupLoadingPage("enviando notficación..."));
            try
            {
                await EnviarNotificacion(false);
                ActualizarEstadoNotificacion();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                await PopupNavigation.Instance.PopAsync();
                Toasts.Error("Rechazado.", 2000);
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
                    IsAccepted = _notificacion.Notificacion.EstadoAceptado == false ? "Pendiente" : "Aceptado";
                    IconIsAccepted = _notificacion.Notificacion.EstadoAceptado == false ? "icon_pending.png" : "icon_checked.png";
                    IsVisible = _notificacion.Notificacion.EstadoAceptado == false ? true : false;
                }
                else if (help.Collection.Equals(Constantes.COLLECTION_EMPRESA))
                {
                    var notificacionCandidato = await notificacionService.GetNotificacionEmpresaEmisorById(id, idEmisor, idReceptor);

                    _notificacion.EmpresaEmisor = notificacionCandidato.EmpresaEmisor;
                    _notificacion.Notificacion = notificacionCandidato.Notificacion;
                    FullName = $"{_notificacion.EmpresaEmisor.RazonSocial} Nit: {_notificacion.EmpresaEmisor.Nit}";
                    Fecha = _notificacion.Notificacion.Fecha;
                    Mensaje = _notificacion.Notificacion.Mensaje;
                    IsAccepted = _notificacion.Notificacion.EstadoAceptado == false ? "Pendiente" : "Aceptado";
                    IconIsAccepted = _notificacion.Notificacion.EstadoAceptado == false ? "icon_pending.png" : "icon_checked.png";
                    IsVisible = _notificacion.Notificacion.EstadoAceptado == false ? true : false;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }


        }
    }
}
