using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;

namespace ProyectoDIV1.Droid
{
    [Activity(Label = "DIV1", Theme = "@style/MyTheme.Splash", MainLauncher = true, NoHistory = true)]
    public class SplashActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            StartActivity(new Intent(Application.Context, typeof(MainActivity)));
            // Create your application here
        }
    }
}
