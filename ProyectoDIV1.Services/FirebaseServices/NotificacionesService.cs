using Firebase.Database;
using ProyectoDIV1.Entidades.Models;
using ProyectoDIV1.Services.DTOs;
using ProyectoDIV1.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoDIV1.Services.FirebaseServices
{
    public class NotificacionesService : FirebaseHelper
    {
        private CandidatoService candidatoService = new CandidatoService();
        private EmpresaService empresaService = new EmpresaService();
        public NotificacionesService()
        {
            firebase = new FirebaseClient("https://proyectodiv-d53ed-default-rtdb.firebaseio.com/");
        }
        public async Task<List<ENotificacion>> GetNotificacionesCandidatos(Guid id)
        {
            return (await firebase.Child(Constantes.COLLECTION_CANDIDATO)
                .OnceAsync<EEmpresa>()).FirstOrDefault(x => x.Object.UsuarioId == id).Object.Notificaciones;
        }
        public async Task<List<ENotificacion>> GetNotificacionesEmpresas(Guid id)
        {
            return (await firebase.Child(Constantes.COLLECTION_EMPRESA)
                .OnceAsync<EEmpresa>()).FirstOrDefault(x => x.Object.UsuarioId == id).Object.Notificaciones;
        }

        public async Task<NotificacionCandidatoDTO> GetNotificacionCandidatoEmisorById(Guid idNotificacion, Guid idCandidatoEmisor, Guid idReceptor)
        {
            try
            {
                NotificacionCandidatoDTO notificacionCandidatoDTO = new NotificacionCandidatoDTO();
                var candidatoEmisor = await candidatoService.GetCandidatoAsync(idCandidatoEmisor);
                if (candidatoEmisor != null)
                {
                    var userCandidatoReceptor = await candidatoService.GetCandidatoAsync(idReceptor);

                    if (userCandidatoReceptor != null)
                    {
                        var notificacion = userCandidatoReceptor.Notificaciones.FirstOrDefault(x => x.Id.Equals(idNotificacion));
                        notificacionCandidatoDTO.CandidatoEmisor = candidatoEmisor;
                        notificacionCandidatoDTO.Notificacion = notificacion;
                    }
                    else
                    {
                        var userEmpresaReceptor = await empresaService.GetEmpresaAsync(idReceptor);
                        if (userEmpresaReceptor != null)
                        {
                            var notificacion = userEmpresaReceptor.Notificaciones.FirstOrDefault(x => x.Id.Equals(idNotificacion));
                            notificacionCandidatoDTO.CandidatoEmisor = candidatoEmisor;
                            notificacionCandidatoDTO.Notificacion = notificacion;
                        }
                    }
                }
                return notificacionCandidatoDTO;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return null;
        }
        public async Task<NotificacionEmpresaDTO> GetNotificacionEmpresaEmisorById(Guid idNotificacion, Guid idEmpresaEmisor, Guid idReceptor)
        {
            NotificacionEmpresaDTO notificacionCandidatoDTO = new NotificacionEmpresaDTO();

            var empresa = await empresaService.GetEmpresaAsync(idEmpresaEmisor);
            if (empresa != null)
            {
                var userCandidatoReceptor = await candidatoService.GetCandidatoAsync(idReceptor);

                if (userCandidatoReceptor != null)
                {
                    var notificacion = userCandidatoReceptor.Notificaciones.FirstOrDefault(x => x.Id.Equals(idNotificacion));
                    notificacionCandidatoDTO.EmpresaEmisor = empresa;
                    notificacionCandidatoDTO.Notificacion = notificacion;
                }
                else
                {
                    var userEmpresaReceptor = await empresaService.GetEmpresaAsync(idReceptor);
                    if (userEmpresaReceptor != null)
                    {
                        var notificacion = userEmpresaReceptor.Notificaciones.FirstOrDefault(x => x.Id.Equals(idNotificacion));
                        notificacionCandidatoDTO.EmpresaEmisor = empresa;
                        notificacionCandidatoDTO.Notificacion = notificacion;
                    }
                }
            }
            return notificacionCandidatoDTO;
        }
    }
}
