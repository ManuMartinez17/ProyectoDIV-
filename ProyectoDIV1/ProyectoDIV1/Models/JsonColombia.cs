using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace ProyectoDIV1.Models
{
    public class JsonColombia
    {
        private List<JsonColombia> colombia;
        const string path = "ArchivosLocales.Colombia.json";
        public int Id { get; set; }
        public string Departamento { get; set; }
        public string[] Ciudades { get; set; }
        public async Task<List<JsonColombia>> DeserializarJsonColombia()
        {

            try
            {

                var assembly = typeof(AppShell).GetTypeInfo().Assembly;

                var rutaCompleta = Path.Combine($"{assembly.GetName().Name}.{path}");
                Stream stream = assembly.GetManifestResourceStream(rutaCompleta);

                using (StreamReader reader = new StreamReader(stream))
                {
                    var json = await reader.ReadToEndAsync();
                    colombia = JsonConvert.DeserializeObject<List<JsonColombia>>(json);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return colombia;
        }

        public List<string> LoadDepartaments(List<JsonColombia> colombia)
        {
            List<string> lista = new List<string>();
            colombia.ForEach(x => lista.Add(x.Departamento));
            lista.Sort();
            return lista;
        }

    }
    public class Ciudades
    {
        public string Nombre { get; set; }
    }
}
