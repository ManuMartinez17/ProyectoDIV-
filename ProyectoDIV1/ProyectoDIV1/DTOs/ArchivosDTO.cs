using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ProyectoDIV1.DTOs
{
    public class ArchivosDTO
    {
        public byte[] ImagenPerfil { get; set; }
        public byte[] Archivo { get; set; }
        public string Password { get; set; }
    }
}
