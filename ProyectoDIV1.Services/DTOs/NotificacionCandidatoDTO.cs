using ProyectoDIV1.Entidades.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoDIV1.Services.DTOs
{
    public class NotificacionCandidatoDTO
    {
        public ENotificacion Notificacion { get; set; } = new ENotificacion();
        public ECandidato CandidatoEmisor { get; set; } = new ECandidato();
    }
}
