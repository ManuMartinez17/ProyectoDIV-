using System;

namespace ProyectoDIV1.Entidades.Models
{
    public class EEmpresa
    {
        public Guid UsuarioId { get; set; }
        public string Nombre { get; set; }
        public string Nit { get; set; }
        public string Email { get; set; }
        public string Ciudad { get; set; }
        public string Departamento { get; set; }
        public string Celular { get; set; }
        public string Password { get; set; }
        public RutasArchivos Rutas { get; set; } = new RutasArchivos();
    }
}
