using Firebase.Database;
using Firebase.Database.Query;
using ProyectoDIV1.Entidades;
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
                Password = item.Object.Password

            }).ToList();
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


        public async Task AddPerson(Candidato _candidatos)
        {
            DateTime today = DateTime.Today;
            int age = today.Year - _candidatos.FechaDeNacimiento.Value.Year;
            await firebase
            .Child("Candidatos")
            .PostAsync(new ECandidato()
            {
                UsuarioId = _candidatos.UsuarioId,
                Departamento = _candidatos.Departamento.Value,
                Nombre = _candidatos.Nombre.Value,
                Apellido = _candidatos.Apellido.Value,
                Email = _candidatos.Email.Value,
                Ciudad = _candidatos.Ciudad.Value,
                Celular = _candidatos.Celular.Value,
                Edad = age,
                Password = _candidatos.Password.Item1.Value
            });
        }

        public async Task AddPerson(Empresa _empresa)
        {
            await firebase
            .Child("Empresas")
            .PostAsync(new Empresa()
            {
                UsuarioId = Guid.NewGuid(),
                Nombre = _empresa.Nombre,
                Nit = _empresa.Nit,
                Email = _empresa.Email,
                Ciudad = _empresa.Ciudad,
                Celular = _empresa.Celular,
                Password = _empresa.Password
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
                  Password = _candidatos.Password
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
    
