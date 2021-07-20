using Newtonsoft.Json;
using ProyectoDIV1.Interfaces;
using ProyectoDIV1.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ProyectoDIV1.Services
{

    public class ServiceJobs : IServiceJobs
    {
        public async Task<List<Job>> GetListJobsAsync(string rutaSolicitud)
        {
            try
            {
                var response = await ApiJobs.apiClient.GetAsync(rutaSolicitud);
                response.EnsureSuccessStatusCode();
                var jsonResult = await response.Content.ReadAsStringAsync();

                var result = JsonConvert.DeserializeObject<List<Job>>(jsonResult);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<Skills>> GetListJobsRelatedSkills(string rutaSolicitud, string Id)
        {
            try
            {
                var response = await ApiJobs.apiClient.GetAsync($"{rutaSolicitud}{Id}");
                response.EnsureSuccessStatusCode();
                var jsonResult = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<List<Skills>>(jsonResult);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
    public class ApiJobs
    {

        public static HttpClient apiClient = new HttpClient();
        static ApiJobs()
        {
            var SERVIDOR = new Uri("http://api.dataatwork.org/v1/");
            apiClient.BaseAddress = SERVIDOR;
            apiClient.DefaultRequestHeaders.Clear();
            apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}
