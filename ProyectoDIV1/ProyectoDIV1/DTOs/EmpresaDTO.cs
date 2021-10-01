using ProyectoDIV1.Entidades.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProyectoDIV1.DTOs
{
    public class EmpresaDTO
    {
        public EEmpresa Empresa { get; set; } = new EEmpresa();
        public string FullName => $"{Empresa.RazonSocial} \n\n Nit: {Empresa.Nit}";
        public string ItPlace => $"{Empresa.Departamento} en la ciudad de: {Empresa.Ciudad}";
        public int? CantidadCalificaciones => Empresa.Calificaciones?.Count();
        public float Calificacion => Empresa.Calificaciones == null ? 0 : Empresa.Calificaciones.Average();
        public int? CantidadNotificaciones => Empresa.Notificaciones?.Count(x => x.EstadoVisto == false) > 0
           ? Empresa.Notificaciones?.Count(x => x.EstadoVisto == false) : null;
    }

}
