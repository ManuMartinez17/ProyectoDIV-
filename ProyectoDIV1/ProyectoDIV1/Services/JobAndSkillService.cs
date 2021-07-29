using Newtonsoft.Json;
using ProyectoDIV1.Helpers;
using ProyectoDIV1.Interfaces;
using ProyectoDIV1.Models;
using RestSharp;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ProyectoDIV1.Services
{
    public class JobAndSkillService : IJobAndSkillService
    {
        private Jobs jobs = new Jobs();
        private Skills skills = new Skills();
        public string GenerarToken()
        {
           
            try
            {
                var client = new RestClient("https://auth.emsicloud.com/connect/token");
                var request = new RestRequest(Method.POST);
                request.AddHeader("content-type", "application/x-www-form-urlencoded");
                request.AddParameter("application/x-www-form-urlencoded", Constantes.CREDENCIALES, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                if (response.IsSuccessful)
                {
                    var json = response.Content;
                    var token = JsonConvert.DeserializeObject<Token>(json);
                    return token.access_token;
                }
            }
            catch (Exception ex)
            {
              Debug.WriteLine(ex.Message);
            }
            
           
            return string.Empty;
        }

        public async Task<Jobs> GetListJobsAsync(string rutaSolicitud, string token)
        {
            try
            {
                var ruta = new Uri($"{ApiJobAndSkill.apiClient.BaseUrl}{rutaSolicitud}");
                ApiJobAndSkill.apiClient.BaseUrl = ruta;
                var request = new RestRequest(Method.GET);
                request.AddHeader("Authorization", $"Bearer {token}");
                IRestResponse response = ApiJobAndSkill.apiClient.Execute(request);

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    var mensaje = JsonConvert.DeserializeObject<Mensaje>(response.Content);
                    if (mensaje.message.Contains("Token expired"))
                    {
                        token = GenerarToken();
                        return await GetListJobsAsync(rutaSolicitud, token);
                    }
                }
                else if (response.IsSuccessful)
                {
                    jobs = JsonConvert.DeserializeObject<Jobs>(response.Content);
                }
                return jobs;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<Skills> GetListJobsRelatedSkills(string rutaSolicitud, string token)
        {
            try
            {
                RestClient apiClient = new RestClient($"https://emsiservices.com/{rutaSolicitud}");
                var request = new RestRequest(Method.GET);
                request.AddHeader("Authorization", $"Bearer {token}");
                IRestResponse response = apiClient.Execute(request);
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    var mensaje = JsonConvert.DeserializeObject<Mensaje>(response.Content);
                    if (mensaje.message.Contains("Token expired"))
                    {
                        token =  GenerarToken();
                        return await GetListJobsRelatedSkills(rutaSolicitud, token);
                    }
                }
                else if (response.IsSuccessful)
                {
                    skills = JsonConvert.DeserializeObject<Skills>(response.Content);
                }
                return skills;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
