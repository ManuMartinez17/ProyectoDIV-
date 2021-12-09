using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoDIV1.Entidades.Models
{
    public class ENotificacion
    {
        public Guid Id { get; set; }
        public Guid EmisorId { get; set; }
        public bool EstadoVisto { get; set; }
        public DateTime Fecha { get; set; }
        public string Mensaje { get; set; }
        public bool EstadoAceptado { get; set; }
        public bool EstadoRechazado { get; set; }
        public bool ContratoTerminado { get; set; }
    }
}
