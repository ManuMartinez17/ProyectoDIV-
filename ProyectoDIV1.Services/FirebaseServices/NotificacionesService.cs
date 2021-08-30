using Firebase.Database;
using ProyectoDIV1.Entidades.Models;
using ProyectoDIV1.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoDIV1.Services.FirebaseServices
{
    public class NotificacionesService
    {
        public FirebaseClient firebase;
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
    }
}
