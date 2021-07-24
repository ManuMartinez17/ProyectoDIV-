using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ProyectoDIV1.Helpers.Funciones
{
    public static class FuncionesEstaticas
    {
        public static byte[] ConvertirABytes(Stream input)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                input.CopyTo(ms);
                return ms.ToArray();
            }
        }
    }
}
