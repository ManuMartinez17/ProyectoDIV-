using ProyectoDIV1.Entidades.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoDIV1.DTOs
{
    public class NotificacionDTO
    {
        public ENotificacion Notificacion { get; set; } = new ENotificacion();
        public ECandidato CandidatoEmisor { get; set; } = new ECandidato();
        public ECandidato CandidatoReceptor { get; set; } = new ECandidato();
        public EEmpresa EmpresaEmisor { get; set; } = new EEmpresa();
        public EEmpresa EmpresaReceptor { get; set; } = new EEmpresa();
        public string IsVisto => Notificacion.EstadoVisto == false ? "icon_noView.png" : "icon_view.png";
        public string IsAccepted => Notificacion.EstadoAceptado == false && Notificacion.EstadoRechazado == false ?
            "Pendiente" : Notificacion.EstadoRechazado == true ? "Rechazado" : Notificacion.ContratoTerminado == false ? "Aceptado" : "Contrato terminado.";
        public string FullName { get; set; }
        public bool TerminarContrato => Notificacion.Mensaje.Equals("Solicitud Aceptada.") || Notificacion.Mensaje.Equals("Solicitud Rechazada.")
            ? false : Notificacion.EstadoAceptado == true && Notificacion.ContratoTerminado == false ? true : false;
        public bool IsCalificated => Notificacion.Mensaje.Equals("Califiqueme por favor.") ? true : false;
        public string IconIsAccepted => Notificacion.EstadoAceptado == false && Notificacion.EstadoRechazado == false ?
            "icon_pending.png" : Notificacion.EstadoRechazado == true ? "icon_rechazing.png" :
            Notificacion.ContratoTerminado == false ? "icon_checked.png" : "icon_finished.png";
        public Guid IdEmisor { get; set; }
    }
}
