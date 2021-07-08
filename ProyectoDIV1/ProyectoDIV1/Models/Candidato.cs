using ProyectoDIV1.Validators;
using SQLite;
using System;

namespace ProyectoDIV1.Models
{
    public class Candidato
    {
        [PrimaryKey, AutoIncrement]
        public Guid UsuarioId { get; set; }
        public ValidatableObject<string> Nombre { get; set; } = new ValidatableObject<string>();
        public ValidatableObject<string> Apellido { get; set; } = new ValidatableObject<string>();
        public ValidatableObject<string> Email { get; set; } = new ValidatableObject<string>();
        public ValidatableObject<string> Celular { get; set; } = new ValidatableObject<string>();
        public ValidatableObject<string> Departamento { get; set; } = new ValidatableObject<string>();
        public ValidatableObject<string> Ciudad { get; set; } = new ValidatableObject<string>();
        public ValidatableObject<DateTime> FechaDeNacimiento { get; set; } = new ValidatableObject<DateTime>() { Value = DateTime.Now };

        public int Edad { get; set; }
        private string foto;
        private string curriculum;
        public ValidatablePair<string> Password { get; set; } = new ValidatablePair<string>();
        public ValidatableObject<string> Categoria { get; set; } = new ValidatableObject<string>();
    }
}
