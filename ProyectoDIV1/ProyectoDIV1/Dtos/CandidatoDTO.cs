using ProyectoDIV1.Entidades.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoDIV1.DTOs
{
    public class CandidatoDTO
    {
        public ECandidato Candidato { get; set; } = new ECandidato();
        public string FullName => $"{Candidato.Nombre} {Candidato.Apellido}";
    }
}
