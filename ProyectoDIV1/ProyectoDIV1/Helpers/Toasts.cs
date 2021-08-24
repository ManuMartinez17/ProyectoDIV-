using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ProyectoDIV1.Helpers
{
    public class Toasts
    {

        public static void Success(string mensaje, int duration)
        {
            ToastConfig toasconfig = new ToastConfig(mensaje);
            toasconfig.SetDuration(duration);   
            toasconfig.SetMessageTextColor(Color.White);
            toasconfig.SetIcon("icon_check.png");
          
            toasconfig.SetBackgroundColor(Color.FromArgb(72, 65,124));
            UserDialogs.Instance.Toast(toasconfig);
        }
        public static void Error(string mensaje, int duration)
        {
            ToastConfig toasconfig = new ToastConfig(mensaje);
            toasconfig.SetDuration(duration);
            toasconfig.SetMessageTextColor(Color.White);

            toasconfig.SetIcon("icon_error.png");
            toasconfig.SetBackgroundColor(Color.FromArgb(199, 0, 36));
            UserDialogs.Instance.Toast(toasconfig);
        }

        public static void Warning(string mensaje, int duration)
        {
            ToastConfig toasconfig = new ToastConfig(mensaje);
            toasconfig.SetDuration(duration);
            toasconfig.SetMessageTextColor(Color.White);
            toasconfig.SetBackgroundColor(Color.FromArgb(72, 65, 124));
            UserDialogs.Instance.Toast(toasconfig);
        }

    }
}
