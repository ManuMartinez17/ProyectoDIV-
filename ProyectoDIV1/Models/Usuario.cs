using ProyectoDIV1.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoDIV1.Models
{
    public class Usuario: BaseViewModel
    {
        #region Attributes
        private string email;
        private string password;
        #endregion

        #region Properties
        public string Email
        {
            get { return this.email; }
            set { SetValue(ref this.email, value); }
        }

        public string Password
        {
            get { return this.password; }
            set { SetValue(ref this.password, value); }
        }
        #endregion
    }
}
