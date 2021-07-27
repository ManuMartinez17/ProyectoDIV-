using Firebase.Database;
using ProyectoDIV1.Entidades.Models;
using ProyectoDIV1.Helpers;
using System;
using System.Collections.Generic;
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
                Calificacion = item.Object.Calificacion,
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
    }
}
