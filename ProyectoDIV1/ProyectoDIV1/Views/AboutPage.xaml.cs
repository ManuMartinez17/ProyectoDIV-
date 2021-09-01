using ProyectoDIV1.Services.Interfaces;
using ProyectoDIV1.ViewModels;
using ProyectoDIV1.Views.Account;
using System;
using System.ComponentModel;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ProyectoDIV1.Views
{
    [DesignTimeVisible(false)]
    public partial class AboutPage : ContentPage
    {
        public AboutPage()
        {
            InitializeComponent();
           
            BindingContext = new AboutViewModel();
        }
    }
}