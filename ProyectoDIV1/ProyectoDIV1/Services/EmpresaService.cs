using ProyectoDIV1.Entidades.Models;
using ProyectoDIV1.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoDIV1.Services
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
            Rutas = item.Object.Rutas,
            Calificaciones = item.Object.Calificaciones,
            Ciudad = item.Object.Ciudad,
            Departamento = item.Object.Departamento,
            Email = item.Object.Email,
            RazonSocial =item.Object.RazonSocial,
            Telefono = item.Object.Telefono,
            Nit = item.Object.Nit,
        }).Where(x => x.Email.Equals(email)).FirstOrDefault();
        }

    }
}
