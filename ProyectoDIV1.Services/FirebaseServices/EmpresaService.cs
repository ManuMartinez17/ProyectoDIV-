using ProyectoDIV1.Entidades.Models;
using ProyectoDIV1.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoDIV1.Services.FirebaseServices
{
    public class EmpresaService : FirebaseHelper
    {
        public async Task<EEmpresa> GetIdXEmail(string email)
        {
            return (await firebase
        .Child(Constantes.COLLECTION_EMPRESA)
        .OnceAsync<EEmpresa>()).Select(item => new EEmpresa
        {
            UsuarioId = item.Object.UsuarioId,
            Rutas = string.IsNullOrWhiteSpace(item.Object.Rutas.RutaImagenRegistro) ? new Archivos()
            {
                RutaImagenRegistro = "https://i.postimg.cc/zDkX2Zh7/logo.png",
                NombreArchivoRegistro = item.Object.Rutas.NombreArchivoRegistro,
                NombreImagenRegistro = item.Object.Rutas.NombreImagenRegistro,
                RutaArchivoRegistro = item.Object.Rutas.RutaArchivoRegistro
            } : item.Object.Rutas,
            Calificaciones = item.Object.Calificaciones,
            Ciudad = item.Object.Ciudad,
            Departamento = item.Object.Departamento,
            Email = item.Object.Email,
            RazonSocial = item.Object.RazonSocial,
            Telefono = item.Object.Telefono,
            Nit = item.Object.Nit,
            Notificaciones = item.Object.Notificaciones
        }).Where(x => x.Email.Equals(email)).FirstOrDefault();
        }

        public async Task<EEmpresa> GetEmpresaAsync(Guid id)
        {
            return (await firebase.Child(Constantes.COLLECTION_EMPRESA)
            .OnceAsync<EEmpresa>()).FirstOrDefault(x => x.Object.UsuarioId == id).Object;
        }

        public async Task<List<EEmpresa>> GetEmpresas()
        {
            return (await firebase
                .Child(Constantes.COLLECTION_EMPRESA)
                .OnceAsync<EEmpresa>()).Select(item => new EEmpresa
                {
                    UsuarioId = item.Object.UsuarioId,
                    Nit = item.Object.Nit,
                    RazonSocial = item.Object.RazonSocial,
                    Calificaciones = item.Object.Calificaciones,
                    Email = item.Object.Email,
                    Ciudad = item.Object.Ciudad,
                    Telefono = item.Object.Telefono,
                    Departamento = item.Object.Departamento,
                    Rutas = string.IsNullOrWhiteSpace(item.Object.Rutas.RutaImagenRegistro) ? new Archivos()
                    {
                        RutaImagenRegistro = "https://i.postimg.cc/zDkX2Zh7/logo.png",
                        NombreArchivoRegistro = item.Object.Rutas.NombreArchivoRegistro,
                        NombreImagenRegistro = item.Object.Rutas.NombreImagenRegistro,
                        RutaArchivoRegistro = item.Object.Rutas.RutaArchivoRegistro
                    } : item.Object.Rutas,
                    Notificaciones = item.Object.Notificaciones
                }).ToList();
        }

        public async Task<List<EEmpresa>> GetEmpresaBySearch(string busqueda)
        {
            return (await firebase
               .Child(Constantes.COLLECTION_EMPRESA)
               .OnceAsync<EEmpresa>()).Select(item => new EEmpresa
               {
                   UsuarioId = item.Object.UsuarioId,
                   Nit = item.Object.Nit,
                   RazonSocial = item.Object.RazonSocial,
                   Calificaciones = item.Object.Calificaciones,
                   Email = item.Object.Email,
                   Ciudad = item.Object.Ciudad,
                   Telefono = item.Object.Telefono,
                   Departamento = item.Object.Departamento,
                   Rutas = string.IsNullOrWhiteSpace(item.Object.Rutas.RutaImagenRegistro) ? new Archivos()
                   {
                       RutaImagenRegistro = "https://i.postimg.cc/zDkX2Zh7/logo.png",
                       NombreArchivoRegistro = item.Object.Rutas.NombreArchivoRegistro,
                       NombreImagenRegistro = item.Object.Rutas.NombreImagenRegistro,
                       RutaArchivoRegistro = item.Object.Rutas.RutaArchivoRegistro
                   } : item.Object.Rutas,
                   Notificaciones = item.Object.Notificaciones
               }).Where(x => x.RazonSocial.Equals(busqueda)).ToList();
        }
    }
}
