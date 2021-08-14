using Firebase.Database;
using ProyectoDIV1.Entidades.Models;
using ProyectoDIV1.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoDIV1.Services
{
    public class CandidatoService
    {
        public FirebaseClient firebase;
        public CandidatoService()
        {
            firebase = new FirebaseClient("https://proyectodiv-d53ed-default-rtdb.firebaseio.com/");
        }
        public async Task<ECandidato> GetIdXEmail(string email)
        {
            return (await firebase
           .Child(Constantes.COLLECTION_CANDIDATO)
           .OnceAsync<ECandidato>()).Select(item => new ECandidato
           {
               Nombre = item.Object.Nombre,
               Calificaciones = item.Object.Calificaciones,
               Apellido = item.Object.Apellido,
               Email = item.Object.Email,
               Ciudad = item.Object.Ciudad,
               Celular = item.Object.Celular,
               Edad = item.Object.Edad,
               Departamento = item.Object.Departamento,
               Habilidades = item.Object.Habilidades,
               Profesion = item.Object.Profesion,
               Rutas = item.Object.Rutas,
               UsuarioId = item.Object.UsuarioId
           }).FirstOrDefault(x => x.Email.Equals(email));
        }

        public async Task<List<ECandidato>> GetCandidatos()
        {
            return (await firebase
            .Child(Constantes.COLLECTION_CANDIDATO)
            .OnceAsync<ECandidato>()).Select(item => new ECandidato
            {
                UsuarioId = item.Object.UsuarioId,
                Nombre = item.Object.Nombre,
                Apellido = item.Object.Apellido,
                Calificaciones = item.Object.Calificaciones,
                Email = item.Object.Email,
                Ciudad = item.Object.Ciudad,
                Celular = item.Object.Celular,
                Edad = item.Object.Edad,
                Departamento = item.Object.Departamento,
                Habilidades = item.Object.Habilidades,
                Profesion = item.Object.Profesion,
                Rutas = item.Object.Rutas
            }).ToList();
        }

        public async Task<List<Lista>> GetHablidadesCandidatoAsync(Guid usuarioId)
        {
            var query = (await firebase
          .Child(Constantes.COLLECTION_CANDIDATO)
          .OnceAsync<ECandidato>()).Select(item => new ECandidato
          {
              Habilidades = item.Object.Habilidades
          }).FirstOrDefault();

            return query.Habilidades;
        }
        public List<Lista> BorrarHabilidadCandidato(List<Lista> lista, string item)
        {
            var query = lista.Where(x => x.Nombre.Equals(item)).FirstOrDefault();
            if (query != null)
            {
                lista.Remove(query);
            }
            return lista;
        }

        public async Task<FirebaseObject<ECandidato>> GetCandidatoFirebaseObjectAsync(Guid id)
        {
            return (await firebase
           .Child(Constantes.COLLECTION_CANDIDATO)
           .OnceAsync<ECandidato>().ConfigureAwait(false)).FirstOrDefault(x => x.Object.UsuarioId == id);
        }
        public async Task<ECandidato> GetCandidatoAsync(Guid id)
        {
            return (await firebase
        .Child(Constantes.COLLECTION_CANDIDATO)
        .OnceAsync<ECandidato>()).Select(item => new ECandidato
        {
            Nombre = item.Object.Nombre,
            Calificaciones = item.Object.Calificaciones,
            Apellido = item.Object.Apellido,
            Email = item.Object.Email,
            Ciudad = item.Object.Ciudad,
            Celular = item.Object.Celular,
            Edad = item.Object.Edad,
            Departamento = item.Object.Departamento,
            Habilidades = item.Object.Habilidades,
            Profesion = item.Object.Profesion,
            Rutas = item.Object.Rutas,
            UsuarioId = item.Object.UsuarioId
        }).FirstOrDefault(x => x.UsuarioId.Equals(id));
        }
    }
}
