using ProyectoDIV1.Entidades.Models;
using System.Linq;

namespace ProyectoDIV1.DTOs
{
    public class CandidatoDTO
    {
        public ECandidato Candidato { get; set; } = new ECandidato();
        public string FullName => $"{Candidato.Nombre} {Candidato.Apellido}";
        public string ItLives => $"Vivo en {Candidato.Departamento} en la ciudad de: {Candidato.Ciudad}";
        public float Calificacion => Candidato.Calificaciones == null ? 0 : Candidato.Calificaciones.Average();
    }
}
