using ProyectoDIV1.Validators;
using System;

namespace ProyectoDIV1.Models
{
    public class Empresa
    {
        public Guid UsuarioId { get; set; }
        public ValidatableObject<string> Nombre { get; set; } = new ValidatableObject<string>();
        public ValidatableObject<string> Nit { get; set; } = new ValidatableObject<string>();
        public ValidatableObject<string> Email { get; set; } = new ValidatableObject<string>();
        public ValidatableObject<string> Celular { get; set; } = new ValidatableObject<string>();
        public ValidatableObject<string> Departamento { get; set; } = new ValidatableObject<string>();
        public ValidatableObject<string> Ciudad { get; set; } = new ValidatableObject<string>();
        public ValidatablePair<string> Password { get; set; } = new ValidatablePair<string>();
    }
}
