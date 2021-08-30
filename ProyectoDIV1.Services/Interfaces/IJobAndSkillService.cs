using ProyectoDIV1.Entidades.Models;
using System.Threading.Tasks;

namespace ProyectoDIV1.Services.Interfaces
{
    public interface IJobAndSkillService
    {
        Task<Jobs> GetListJobsAsync(string rutaSolicitud, string token);
        Task<Skills> GetListJobsRelatedSkills(string rutaSolicitud, string token);
        Token GenerarToken();
    }
}
