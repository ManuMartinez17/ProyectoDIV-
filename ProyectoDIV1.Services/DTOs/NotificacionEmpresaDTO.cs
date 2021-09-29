using ProyectoDIV1.Entidades.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoDIV1.Services.DTOs
{
    public class NotificacionEmpresaDTO
    {
        public ENotificacion Notificacion { get; set; } = new ENotificacion();
        public EEmpresa EmpresaEmisor { get; set; } = new EEmpresa();
    }
}
