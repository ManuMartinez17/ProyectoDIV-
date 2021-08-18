using System;
using System.Collections.Generic;

namespace ProyectoDIV1.Entidades.Models
{
    public class EEmpresa
    {
        public Guid UsuarioId { get; set; }
        public string Nit { get; set; }
        public string RazonSocial { get; set; }
        public string Email { get; set; }
        public string Ciudad { get; set; }
        public string Departamento { get; set; }
        public List<float> Calificaciones { get; set; }
        public string Telefono { get; set; }
        public Archivos Rutas { get; set; } = new Archivos();
    }
}
