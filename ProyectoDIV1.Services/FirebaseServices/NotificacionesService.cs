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

        public async Task<NotificacionCandidatoDTO> GetNotificacionCandidatoEmisorById(Guid idNotificacion, Guid idCandidatoEmisor, Guid idCandidatoReceptor)
        {
            try
            {
                NotificacionCandidatoDTO notificacionCandidatoDTO = new NotificacionCandidatoDTO();
                var candidatoEmisor = (await firebase.Child(Constantes.COLLECTION_CANDIDATO)
                    .OnceAsync<ECandidato>()).FirstOrDefault(x => x.Object.UsuarioId == idCandidatoEmisor).Object;
                if (candidatoEmisor != null)
                {
                    var userReceptor = (await firebase.Child(Constantes.COLLECTION_CANDIDATO)
                   .OnceAsync<ECandidato>()).FirstOrDefault(x => x.Object.UsuarioId == idCandidatoReceptor).Object;
                    var notificacion = userReceptor.Notificaciones.FirstOrDefault(x => x.Id.Equals(idNotificacion));
                    notificacionCandidatoDTO.CandidatoEmisor = candidatoEmisor;
                    notificacionCandidatoDTO.Notificacion = notificacion;
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
            var empresa = (await firebase.Child(Constantes.COLLECTION_EMPRESA)
                .OnceAsync<EEmpresa>()).FirstOrDefault(x => x.Object.UsuarioId == idEmpresaEmisor).Object;
            if (empresa != null)
            {
                var userReceptor = (await firebase.Child(Constantes.COLLECTION_CANDIDATO)
                 .OnceAsync<ECandidato>()).FirstOrDefault(x => x.Object.UsuarioId == idReceptor).Object;
                var notificacion = userReceptor.Notificaciones.FirstOrDefault(x => x.Id.Equals(idNotificacion));
                notificacionCandidatoDTO.EmpresaEmisor = empresa;
                notificacionCandidatoDTO.Notificacion = notificacion;
            }
            return notificacionCandidatoDTO;
        }
    }
}
