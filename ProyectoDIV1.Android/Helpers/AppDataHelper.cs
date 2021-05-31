using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Firebase;
using Firebase.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProyectoDIV1.Droid.Helpers
{
    public static class AppDataHelper
    {
        public static FirebaseDatabase GetDatabase()
        {
            var app = FirebaseApp.InitializeApp(Application.Context);
            FirebaseDatabase database;

            if(app == null)
            {
                var option = new Firebase.FirebaseOptions.Builder()
                    .SetApplicationId("proyectodiv-d53ed")
                    .SetApiKey("AIzaSyBtaSAuQU_iOWSr-kRKVUmCKN7HbH3nKaI")
                    .SetDatabaseUrl("https://proyectodiv-d53ed-default-rtdb.firebaseio.com/")
                    .SetStorageBucket("proyectodiv-d53ed.appspot.com")
                    .Build();

                app = FirebaseApp.InitializeApp(Application.Context, option);
                database = FirebaseDatabase.GetInstance(app);


            }
            else
            {
                database = FirebaseDatabase.GetInstance(app);
            }

            return database;
        }
    }
}