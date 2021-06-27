using ProyectoDIV1.ViewModels;
using SQLite;
using System;

namespace ProyectoDIV1.Models
{
    public class Empresa : BaseViewModel
    {
        [PrimaryKey, AutoIncrement]
        public Guid UsuarioId { get; set; }
        public string Nombre { get; set; }
        public string Nit { get; set; }
        public string Email { get; set; }
        public string Ciudad { get; set; }
        public string Departamento { get; set; }
        public string Celular { get; set; }
    }
}
