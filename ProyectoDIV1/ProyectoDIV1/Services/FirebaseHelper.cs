using Firebase.Database;
using Firebase.Database.Query;
using ProyectoDIV1.Entidades.Models;
using ProyectoDIV1.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoDIV1.Services
{
    public class FirebaseHelper
    {
        FirebaseClient firebase;
        public FirebaseHelper()
        {
            firebase = new FirebaseClient("https://proyectodiv-d53ed-default-rtdb.firebaseio.com/");
        }
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
                Password = item.Object.Password

            }).ToList();
        }

        public async Task<ECandidato> GetUsuario(string nombreCollection, string email)
        {
            return (await firebase
            .Child(nombreCollection)
            .OnceAsync<ECandidato>()).Select(item => new ECandidato
            {
                UsuarioId = item.Object.UsuarioId,
                Nombre = item.Object.Nombre,
                Apellido = item.Object.Apellido,
                Email = item.Object.Email,
                Ciudad = item.Object.Ciudad,
                Celular = item.Object.Celular,
                Edad = item.Object.Edad,
            }).Where(x => x.Email.Equals(email)).FirstOrDefault();
        }

        public async Task<List<Empresa>> GetEmpresa()
        {
            return (await firebase
            .Child("Empresa")
            .OnceAsync<Empresa>()).Select(item => new Empresa
            {
                UsuarioId = item.Object.UsuarioId,
                Nombre = item.Object.Nombre,
                Nit = item.Object.Nit,
                Email = item.Object.Email,
                Ciudad = item.Object.Ciudad,
                Celular = item.Object.Celular,
                Password = item.Object.Password

            }).ToList();
        }


        public async Task CrearAsync<T>(T modelo, string nombreCollection)
        {
            await firebase
            .Child(nombreCollection)
            .PostAsync(modelo);
        }

        public async Task UpdateAsync<T>(T modelo, string nombreCollection, FirebaseObject<T> modeloToUpdate)
        {
            await firebase
              .Child(nombreCollection)
              .Child(modeloToUpdate.Key)
              .PutAsync(modelo);
        }

        public async Task DeleteAsync<T>(string nombreCollection, FirebaseObject<T> modeloToDelete)
        {

            await firebase.Child(nombreCollection).Child(modeloToDelete.Key).DeleteAsync();

        }


    }
}

