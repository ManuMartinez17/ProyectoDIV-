using ProyectoDIV1.DTOs;
using ProyectoDIV1.Services;
using ProyectoDIV1.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProyectoDIV1.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EmpresasConServiciosPage : ContentPage
    {
        private ObservableCollection<EmpresaDTO> _candidatos;
        private EmpresaService _empresaService;
        public EmpresasConServiciosPage()
        {
            InitializeComponent();
            BindingContext = new EmpresasConServiciosViewModel();
        }
    }
}