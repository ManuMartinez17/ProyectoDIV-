using ProyectoDIV1.ViewModels;
using SQLite;
using System;

namespace ProyectoDIV1.Models
{
    public class Empresa : BaseViewModel
    {
        [PrimaryKey, AutoIncrement]
        public Guid UsuarioId { get; set; }
        private string nombre;
        private string nit;
        private string email;
        private string ciudad;
        private string celular;
        private string password;
        public string Nombre
        {
            get { return this.nombre; }
            set { SetValue(ref this.nombre, value); }
        }

        public string Password
        {
            get { return this.password; }
            set { SetValue(ref this.password, value); }
        }
        public string Nit
        {
            get { return this.nit; }
            set { SetValue(ref this.nit, value); }
        }

        public string Email
        {
            get { return this.email; }
            set { SetValue(ref this.email, value); }
        }

        public string Ciudad
        {
            get { return this.ciudad; }
            set { SetValue(ref this.ciudad, value); }
        }

        public string Celular
        {
            get { return this.celular; }
            set { SetValue(ref this.celular, value); }
        }

    }
}
