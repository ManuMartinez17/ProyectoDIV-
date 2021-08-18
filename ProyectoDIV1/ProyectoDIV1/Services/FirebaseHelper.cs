using Firebase.Database;
using Firebase.Database.Query;
using ProyectoDIV1.Entidades.Models;
using ProyectoDIV1.Helpers;
using ProyectoDIV1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoDIV1.Services
{
    public class FirebaseHelper
    {
        public FirebaseClient firebase;
        public FirebaseHelper()
        {
            firebase = new FirebaseClient("https://proyectodiv-d53ed-default-rtdb.firebaseio.com/");
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

        public async Task<bool> GetUsuarioByEmailAsync<T>(string nombreCollection, string email)
        {
            string nameOfProperty = "Email";
            bool existe = (await firebase.Child(nombreCollection).
               OnceAsync<T>()).Any(x => x.Object.GetType().GetProperty(nameOfProperty).GetValue(x.Object, null).Equals(email));
            return existe;

        }
    }
}

