using ProyectoDIV1.ViewModels;
using SQLite;
using System;

namespace ProyectoDIV1.Models
{
    class Candidato : BaseViewModel
    {
        [PrimaryKey, AutoIncrement]
        public Guid UsuarioId { get; set; }
        private string nombre;
        private string apellido;
        private string email;
        private string ciudad;
        private string celular;
        private int edad;
        private string foto;
        private string curriculum;
        public string Nombre
        {
            get { return this.nombre; }
            set { SetValue(ref this.nombre, value); }
        }

        public string Apellido
        {
            get { return this.apellido; }
            set { SetValue(ref this.apellido, value); }
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

        public int Edad
        {
            get { return this.edad; }
            set { SetValue(ref this.edad, value); }
        }

        public string Foto
        {
            get { return this.foto; }
            set { SetValue(ref this.foto, value); }
        }

        public string Curriculum
        {
            get { return this.curriculum; }
            set { SetValue(ref this.curriculum, value); }
        }

    }
}
