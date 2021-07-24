
using Firebase.Auth;
using ProyectoDIV1.Droid.Helpers;
using ProyectoDIV1.Interfaces;
using System.Threading.Tasks;

[assembly: Xamarin.Forms.Dependency(typeof(FirebaseAuthentication))]
namespace ProyectoDIV1.Droid.Helpers
{
    public class FirebaseAuthentication : IAuthenticationService
    {
  
        public bool IsSignIn()
            => FirebaseAuth.Instance.CurrentUser != null;

        public async Task<bool> Register(string username, string email, string password)
        {
            var authResult = await FirebaseAuth.Instance
                    .CreateUserWithEmailAndPasswordAsync(email, password);

            var userProfileChangeRequestBuilder = new UserProfileChangeRequest.Builder();
            userProfileChangeRequestBuilder.SetDisplayName(username);

            var userProfileChangeRequest = userProfileChangeRequestBuilder.Build();
            await authResult.User.UpdateProfileAsync(userProfileChangeRequest);

            return await Task.FromResult(true);
        }

        public async Task ResetPassword(string email)
            => await FirebaseAuth.Instance.SendPasswordResetEmailAsync(email);

        public async Task<string> SignIn(string email, string password)
        {
            var authResult = await FirebaseAuth.Instance.SignInWithEmailAndPasswordAsync(email, password);
            var token = await authResult.User.GetIdTokenAsync(false);
            return token.Token;
        }

        public void SignOut()
            => FirebaseAuth.Instance.SignOut();
    }
}