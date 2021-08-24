using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoDIV1.Entidades.Models
{
    public class ECandidato
    {
        public Guid UsuarioId { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public string Celular { get; set; }
        public string Departamento { get; set; }
        public string Ciudad { get; set; }
        public int Edad { get; set; }
        public List<float> Calificaciones { get; set; }
        public string Profesion { get; set; }
        public List<Lista> Habilidades { get; set; } = new List<Lista>();
        public Archivos Rutas { get; set; } = new Archivos();
        public string Expectativa { get; set; }
        public List<ENotificacion> Notificaciones { get; set; }
    }
    public class Lista
    {
        public string Nombre { get; set; }
    }
}
