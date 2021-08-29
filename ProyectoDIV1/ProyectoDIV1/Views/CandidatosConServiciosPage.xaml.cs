using ProyectoDIV1.ViewModels;
using Syncfusion.SfAutoComplete.XForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProyectoDIV1.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CandidatosConServiciosPage : ContentPage
    {
        private CandidatosConServiciosViewModel _viewModel;
        public CandidatosConServiciosPage()
        {
            InitializeComponent();
            BindingContext = _viewModel =  new CandidatosConServiciosViewModel();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}