using Newtonsoft.Json;
using ProyectoDIV1.DTOs;
using ProyectoDIV1.Helpers;
using ProyectoDIV1.Services.FirebaseServices;
using ProyectoDIV1.Services.Helpers;
using System;
using System.Diagnostics;
using Xamarin.Forms;

namespace ProyectoDIV1.ViewModels.Notificaciones
{
    public class InfoNotificacionViewModel : BaseViewModel
    {
        private NotificacionDTO _notificacion;
        private NotificacionSalidaDTO _notificacionDetalle;
        private NotificacionesService notificacionService;
        private CandidatoService candidatoService;
        private EmpresaService empresaService;
        private HelpDTO help;

        public InfoNotificacionViewModel()
        {
            help = JsonConvert.DeserializeObject<HelpDTO>(Settings.Token);
            _notificacion = new NotificacionDTO();
            _notificacionDetalle = new NotificacionSalidaDTO();
            notificacionService = new NotificacionesService();
            candidatoService = new CandidatoService();
            empresaService = new EmpresaService();
            AceptarContratoCommand = new Command(AceptarContratoClicked);
            RechazarContratoCommand = new Command(RechazarContratoClicked);
            LoadReceptor();
        }

        public void OnApperaing()
        {
            ActualizarEstadoNotificacion();
            LoadNotificacion();
        }

        private async void ActualizarEstadoNotificacion()
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
                                item.Estado = true;
                                break;
                            }
                        }
                        await notificacionService.UpdateAsync(_notificacion.CandidatoReceptor, help.Collection, query);
                    }
                    else if (help.Collection.Equals(Constantes.COLLECTION_EMPRESA))
                    {
                        var query = await empresaService.GetEmpresaFirebaseObjectAsync(id);
                        _notificacion.EmpresaReceptor.Notificaciones.ForEach(x =>
                        {
                            if (x.Id == idNotificacion)
                            {
                                x.Estado = true;
                                return;
                            }
                        });
                        await notificacionService.UpdateAsync(_notificacion.EmpresaReceptor, help.Collection, query);
                    }
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

        }

        private void AceptarContratoClicked(object obj)
        {
            throw new NotImplementedException();
        }

        private void RechazarContratoClicked(object obj)
        {
            throw new NotImplementedException();
        }

        public Command AceptarContratoCommand { get; }
        public Command RechazarContratoCommand { get; }


        public NotificacionSalidaDTO Detalle
        {
            get { return _notificacionDetalle; }
            set { SetProperty(ref _notificacionDetalle, value); }
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
                    Detalle.FullName = $"De: {_notificacion.CandidatoEmisor.Nombre} {_notificacion.CandidatoEmisor.Apellido}";
                    Detalle.notificacion = _notificacion.Notificacion;
                }
                else if (help.Collection.Equals(Constantes.COLLECTION_EMPRESA))
                {
                    var notificacionCandidato = await notificacionService.GetNotificacionEmpresaEmisorById(id, idEmisor, idReceptor);

                    _notificacion.EmpresaEmisor = notificacionCandidato.EmpresaEmisor;
                    _notificacion.Notificacion = notificacionCandidato.Notificacion;
                    Detalle.FullName = $"De: {_notificacion.EmpresaEmisor.RazonSocial} \n Nit: {_notificacion.EmpresaEmisor.Nit}";
                    Detalle.notificacion = _notificacion.Notificacion;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }


        }
    }
}
