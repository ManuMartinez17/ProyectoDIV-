using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoDIV1.Entidades.Models
{
    public class ENotificacion
    {
        public Guid EmisorId { get; set; }
        public bool Estado { get; set; }
        public DateTime Fecha { get; set; }
        public string Mensaje { get; set; }
    }
}
