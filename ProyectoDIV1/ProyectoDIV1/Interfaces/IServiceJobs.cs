using ProyectoDIV1.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoDIV1.Interfaces
{
    public interface IServiceJobs
    {
        Task<List<Job>> GetListJobsAsync(string rutaSolicitud);
        Task<List<Skills>> GetListJobsRelatedSkills(string rutaSolicitud, string Id);
    }
}
