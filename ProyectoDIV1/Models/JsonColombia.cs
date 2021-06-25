using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace ProyectoDIV1.Models
{
    public class JsonColombia
    {
        const string path = "Colombia.json";
        public int Id { get; set; }
        public string Departamento { get; set; }
        public string[] Ciudades { get; set; }
        public async Task<List<JsonColombia>> DeserializarJsonColombia()
        {
            var colombia = new List<JsonColombia>();
            try
            {
           
                var assembly = typeof(MainPage).GetTypeInfo().Assembly;
                Stream stream = assembly.GetManifestResourceStream($"{assembly.GetName().Name}.{path}");

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
    }   
}
