using Newtonsoft.Json;
using ProyectoDIV1.DTOs;
using ProyectoDIV1.Helpers;
using ProyectoDIV1.Services.Helpers;
using ProyectoDIV1.ViewModels.Candidato;
using ProyectoDIV1.ViewModels.Empresa;
using ProyectoDIV1.ViewModels.Notificaciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProyectoDIV1.Views.Notificaciones
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InfoNotificacionPage : ContentPage
    {
        private InfoNotificacionViewModel _viewModel;
        public InfoNotificacionPage()
        {
            InitializeComponent();
            BindingContext = _viewModel =  new InfoNotificacionViewModel();
        }
        protected override void OnAppearing()
        {   
            base.OnAppearing();
            _viewModel.OnApperaing();
        }
    }
}