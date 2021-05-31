using Firebase.Database;
using Firebase.Database.Query;
using ProyectoDIV1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoDIV1.Services
{
    class FirebaseHelper
    {
        public async Task<List<Candidato>> GetCandidatos()
        {
            return (await firebase
            .Child("Candidatos")
            .OnceAsync<Candidato>()).Select(item => new Candidato
            {
                UsuarioId = item.Object.UsuarioId,
                Nombre = item.Object.Nombre,
                Apellido = item.Object.Apellido,
                Email = item.Object.Email,
                Ciudad = item.Object.Ciudad,
                Celular = item.Object.Celular,
                Edad = item.Object.Edad,
                Foto = item.Object.Curriculum

            }).ToList();
        }


        public async Task AddPerson(Candidato _candidatos)
        {
            await firebase
            .Child("Candidatos")
            .PostAsync(new Candidato()
            {
                UsuarioId = Guid.NewGuid(),
                Nombre = _candidatos.Nombre,
                Apellido = _candidatos.Apellido,
                Email = _candidatos.Email,
                Ciudad = _candidatos.Ciudad,
                Celular = _candidatos.Celular,
                Edad = _candidatos.Edad,
                Foto = _candidatos.Foto,
                Curriculum = _candidatos.Curriculum
            });
        }


        public async Task UpdatePerson(Candidato _candidatos)
        {
            var toUpdatePerson = (await firebase
              .Child("Candidatos")
              .OnceAsync<Candidato>()).Where(a => a.Object.UsuarioId == _candidatos.UsuarioId).FirstOrDefault();

            await firebase
              .Child("Candidatos")
              .Child(toUpdatePerson.Key)
              .PutAsync(new Candidato() { 
                  UsuarioId = _candidatos.UsuarioId,
                  Nombre = _candidatos.Nombre,
                  Apellido = _candidatos.Apellido,
                  Email = _candidatos.Email,
                  Ciudad = _candidatos.Ciudad,
                  Celular = _candidatos.Celular,
                  Edad = _candidatos.Edad,
                  Foto = _candidatos.Foto,
                  Curriculum = _candidatos.Curriculum
              });
        }

        public async Task DeletePerson(Guid usuarioId)
        {
            var toDeletePerson = (await firebase
              .Child("Candidatos")
              .OnceAsync<Candidato>()).Where(a => a.Object.UsuarioId == usuarioId).FirstOrDefault();

            await firebase.Child("Candidatos").Child(toDeletePerson.Key).DeleteAsync();

        }

        FirebaseClient firebase;
        public FirebaseHelper()
        {
            firebase = new FirebaseClient("https://proyectodiv-d53ed-default-rtdb.firebaseio.com/");
        }
    }
}
    
