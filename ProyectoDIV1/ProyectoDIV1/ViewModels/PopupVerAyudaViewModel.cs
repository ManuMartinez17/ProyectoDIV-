using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ProyectoDIV1.ViewModels
{
    class PopupVerAyudaViewModel: BaseViewModel
    {
        private string _texto;
        public string Texto
        {
            get { return _texto; }
            set { SetProperty(ref _texto, value); }
        }
    }
}
