using ProyectoDIV1.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoDIV1.Models
{
    public class Usuario: BaseViewModel
    {
        #region Attributes
        private string apodo;
        private string password;
        #endregion

        #region Properties
        public string Apodo
        {
            get { return this.apodo; }
            set { SetValue(ref this.apodo, value); }
        }

        public string Password
        {
            get { return this.password; }
            set { SetValue(ref this.password, value); }
        }
        #endregion
    }
}
