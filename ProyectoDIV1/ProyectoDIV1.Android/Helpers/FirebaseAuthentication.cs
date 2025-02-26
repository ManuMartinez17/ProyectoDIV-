﻿
using Android.Util;
using Firebase.Auth;
using ProyectoDIV1.Droid.Helpers;
using ProyectoDIV1.Services.Interfaces;
using System.Threading.Tasks;

[assembly: Xamarin.Forms.Dependency(typeof(FirebaseAuthentication))]
namespace ProyectoDIV1.Droid.Helpers
{
    public class FirebaseAuthentication : IAuthenticationService
    {
        private GetTokenResult token;
        public string BuscarEmail()
        {
            string email = string.Empty;
            if (FirebaseAuth.Instance.CurrentUser != null)
            {
                email = FirebaseAuth.Instance.CurrentUser.Email;
            }
            return email;
        }

        public async Task<string> BuscarToken()
        {
            
            if (FirebaseAuth.Instance.CurrentUser != null)
            {
               token = await FirebaseAuth.Instance.CurrentUser.GetIdTokenAsync(false);
            }
            return token.Token;
        }

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
            Log.Debug("Token", token.Token);
            return token.Token;
        }

        public void SignOut()
            => FirebaseAuth.Instance.SignOut();
    }
}