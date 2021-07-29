using ProyectoDIV1.Models;
using System.Threading.Tasks;

namespace ProyectoDIV1.Interfaces
{
    public interface IJobAndSkillService
    {
        Task<Jobs> GetListJobsAsync(string rutaSolicitud, string token);
        Task<Skills> GetListJobsRelatedSkills(string rutaSolicitud, string token);
        string GenerarToken();
    }
}
