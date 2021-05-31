using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoDIV1.ViewModels
{
    public class MainViewModel
    {
        #region ViewModels
        public InicioViewModel Login
        {
            get;
            set;
        }

        #endregion

        #region Constructors
        public MainViewModel()
        {
            instance = this;
            this.Login = new InicioViewModel();
        }
        #endregion
        #region Singleton
        private static MainViewModel instance;

        public static MainViewModel GetInstance()
        {
            if (instance == null)
            {
                return new MainViewModel();
            }

            return instance;
        }
        #endregion
    }
}
