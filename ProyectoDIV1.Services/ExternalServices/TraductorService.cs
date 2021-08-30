using Dasync.Collections;
using Newtonsoft.Json.Linq;
using ProyectoDIV1.Entidades.Models;
using ProyectoDIV1.Services.Helpers;
using RestSharp;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoDIV1.Services.ExternalServices
{
    public class TraductorService
    {
        public async Task<List<Job>> TraducirJobs(List<Job> data)
        {
            List<Job> jobs = new List<Job>();
            await data.ParallelForEachAsync(async item =>
            {

                //var result = await translator.TranslateAsync(item.name, LanguajeSpanish, LanguajeEnglish);
                string palabra = await TraducirPalabra(item.name, Constantes.CodigoISOSpanish, Constantes.CodigoISOEnglish);
                Job job = new Job()
                {
                    name = palabra
                };
                jobs.Add(job);
            }, maxDegreeOfParallelism: 10);
            return jobs;
        }
        public async Task<List<Skill>> TraducirSkills(List<Skill> data)
        {
            List<Skill> skills = new List<Skill>();
            await data.ParallelForEachAsync(async item =>
            {

                //var result = await translator.TranslateAsync(item.name, LanguajeSpanish, LanguajeEnglish);
                string palabra = await TraducirPalabra(item.name, Constantes.CodigoISOSpanish, Constantes.CodigoISOEnglish);
                Skill skill = new Skill()
                {
                    name = palabra
                };
                skills.Add(skill);
            }, maxDegreeOfParallelism: 10);
            return skills;
        }

        public async Task<string> TraducirPalabra(string palabra, string idiomaTO, string idiomaFROM)
        {
            var client = new RestClient($"https://microsoft-translator-text.p.rapidapi.com/translate?api-version=3.0&to={idiomaTO}&profanityAction=NoAction&from={idiomaFROM}");
            var request = new RestRequest(Method.POST);
            request.AddHeader("content-type", "application/json");
            request.AddHeader("x-rapidapi-host", "microsoft-translator-text.p.rapidapi.com");
            request.AddHeader("x-rapidapi-key", "8a6707e93cmsh34b79df1bbc29f9p1e0abdjsnf9007e4ab1ce");
            request.AddParameter("application/json", "[\r{\r\"Text\": \"" + palabra + "\"\r }\r]", ParameterType.RequestBody);
            IRestResponse response = await client.ExecuteAsync(request);
            string json = response.Content;
            json = json.TrimStart(new char[] { '[' }).TrimEnd(new char[] { ']' });
            JObject Jobject = JObject.Parse(json);
            return Jobject.GetValue("translations").Values<string>("text").FirstOrDefault();
        }
    }
}
