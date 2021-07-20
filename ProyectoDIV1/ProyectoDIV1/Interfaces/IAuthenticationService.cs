using System.Threading.Tasks;

namespace ProyectoDIV1.Interfaces
{
    public interface IAuthenticationService
    {
        bool IsSignIn();
        Task<bool> Register(string username, string email, string password);
        void SignOut();
        Task<string> SignIn(string email, string password);
        Task ResetPassword(string email);
    }
}
