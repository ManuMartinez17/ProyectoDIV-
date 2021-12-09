using ProyectoDIV1.ViewModels;
using ProyectoDIV1.ViewModels.Empresa;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProyectoDIV1.Views.Empresa
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EmpresasConServiciosPage : ContentPage
    {
        private EmpresasConServiciosViewModel _viewModel;
        public EmpresasConServiciosPage()
        {
            InitializeComponent();
            BindingContext = _viewModel =  new EmpresasConServiciosViewModel();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}