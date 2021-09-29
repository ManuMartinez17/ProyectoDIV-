using ProyectoDIV1.Entidades.Models;
using System.Linq;

namespace ProyectoDIV1.DTOs
{
    public class CandidatoDTO
    {
        public ECandidato Candidato { get; set; } = new ECandidato();
        public string FullName => $"{Candidato.Nombre} {Candidato.Apellido}";
        public string ItLives => $"Vivo en {Candidato.Departamento} en la ciudad de: {Candidato.Ciudad}";
        public int? CantidadCalificaciones => Candidato.Calificaciones?.Count();
        public float Calificacion => Candidato.Calificaciones == null ? 0 : Candidato.Calificaciones.Average();
        public int? CantidadNotificaciones => Candidato.Notificaciones?.Count(x => x.Estado == false) > 0 ? Candidato.Notificaciones?.Count(x => x.Estado == false) : null;
    }
}
