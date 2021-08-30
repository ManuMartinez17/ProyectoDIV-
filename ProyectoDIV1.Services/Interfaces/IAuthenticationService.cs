using System.Threading.Tasks;

namespace ProyectoDIV1.Services.Interfaces
{
    public interface IAuthenticationService
    {
        bool IsSignIn();
        string BuscarEmail();
        Task<bool> Register(string username, string email, string password);
        void SignOut();
        Task<string> SignIn(string email, string password);
        Task ResetPassword(string email);
    }
}
