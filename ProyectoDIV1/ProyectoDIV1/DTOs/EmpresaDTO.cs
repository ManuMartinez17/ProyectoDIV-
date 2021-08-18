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
        public string FullName => $"{Empresa.RazonSocial} \n Nit: {Empresa.Nit}";
        public string ItPlace => $" {Empresa.Departamento} en la ciudad de: {Empresa.Ciudad}";
        public float Calificacion => Empresa.Calificaciones == null ? 0 : Empresa.Calificaciones.Average();

    }

}
