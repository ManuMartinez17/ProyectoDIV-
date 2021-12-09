using Firebase.Database;
using ProyectoDIV1.Entidades.Models;
using ProyectoDIV1.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoDIV1.Services.FirebaseServices
{
    public class CandidatoService : FirebaseHelper
    {
        private static string UrlDefault = "icon_user.png";
        public CandidatoService()
        {
            firebase = new FirebaseClient("https://proyectodiv-d53ed-default-rtdb.firebaseio.com/");
        }
        public async Task<ECandidato> GetIdXEmail(string email)
        {
            if (!ValidarInternet())
            {
                return default;
            }
            try
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
              Notificaciones = item.Object.Notificaciones,
              Profesion = item.Object.Profesion,
              Rutas = string.IsNullOrWhiteSpace(item.Object.Rutas.RutaImagenRegistro) ? new Archivos()
              {
                  RutaImagenRegistro = UrlDefault,
                  NombreArchivoRegistro = item.Object.Rutas.NombreArchivoRegistro,
                  NombreImagenRegistro = item.Object.Rutas.NombreImagenRegistro,
                  RutaArchivoRegistro = item.Object.Rutas.RutaArchivoRegistro
              } : item.Object.Rutas,
              UsuarioId = item.Object.UsuarioId,
              Expectativa = item.Object.Expectativa
          }).FirstOrDefault(x => x.Email.Equals(email));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return default;
            }

        }

        public async Task<List<string>> GetProfesiones()
        {
            if (!ValidarInternet())
            {
                return default;
            }
            List<ECandidato> candidatos = await GetCandidatos();

            List<string> profesiones = new List<string>();
            Parallel.ForEach(candidatos,
                item =>
                {
                    profesiones.Add(item.Profesion);
                });
            IEnumerable<string> profesionesSinRepetidos = profesiones.Distinct();
            return profesionesSinRepetidos.ToList();
        }

        public async Task<List<ECandidato>> GetCandidatosPorServicio(string profesion)
        {
            if (!ValidarInternet())
            {
                return default;
            }
            try
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
              Rutas = string.IsNullOrWhiteSpace(item.Object.Rutas.RutaImagenRegistro) ? new Archivos()
              {
                  RutaImagenRegistro = UrlDefault,
                  NombreArchivoRegistro = item.Object.Rutas.NombreArchivoRegistro,
                  NombreImagenRegistro = item.Object.Rutas.NombreImagenRegistro,
                  RutaArchivoRegistro = item.Object.Rutas.RutaArchivoRegistro
              } : item.Object.Rutas,
              Expectativa = item.Object.Expectativa,
          }).Where(x => x.Profesion.Equals(profesion)).ToList();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return default;
            }
          
        }

        public async Task<List<ECandidato>> GetCandidatos()
        {
            if (!ValidarInternet())
            {
                return default;
            }
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
                Rutas = string.IsNullOrWhiteSpace(item.Object.Rutas.RutaImagenRegistro) ? new Archivos()
                {
                    RutaImagenRegistro = UrlDefault,
                    NombreArchivoRegistro = item.Object.Rutas.NombreArchivoRegistro,
                    NombreImagenRegistro = item.Object.Rutas.NombreImagenRegistro,
                    RutaArchivoRegistro = item.Object.Rutas.RutaArchivoRegistro
                } : item.Object.Rutas,
                Expectativa = item.Object.Expectativa,
                Notificaciones = item.Object.Notificaciones
            }).ToList();
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
            if (!ValidarInternet())
            {
                return default;
            }
            try
            {
                var query = (await firebase
                    .Child(Constantes.COLLECTION_CANDIDATO)
                    .OnceAsync<ECandidato>().ConfigureAwait(false)).FirstOrDefault(x => x.Object.UsuarioId == id);
                return query;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }


        }
        public async Task<ECandidato> GetCandidatoAsync(Guid id)
        {
            if (!ValidarInternet())
            {
                return default;
            }
            try
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
                        Rutas = string.IsNullOrWhiteSpace(item.Object.Rutas.RutaImagenRegistro) ? new Archivos()
                        {
                            RutaImagenRegistro = UrlDefault,
                            NombreArchivoRegistro = item.Object.Rutas.NombreArchivoRegistro,
                            NombreImagenRegistro = item.Object.Rutas.NombreImagenRegistro,
                            RutaArchivoRegistro = item.Object.Rutas.RutaArchivoRegistro
                        } : item.Object.Rutas,
                        UsuarioId = item.Object.UsuarioId,
                        Expectativa = item.Object.Expectativa,
                        Notificaciones = item.Object.Notificaciones
                    }).FirstOrDefault(x => x.UsuarioId.Equals(id));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<bool> GetCandidatoByEmail(string value)
        {
            if (!ValidarInternet())
            {
                return default;
            }
            bool existe = (await firebase.Child(Constantes.COLLECTION_CANDIDATO).
                OnceAsync<ECandidato>()).Any(x => x.Object.Email.Equals(value));
            return existe;
        }
    }
}
