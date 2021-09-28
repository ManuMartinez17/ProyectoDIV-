using ProyectoDIV1.ViewModels.Empresa;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProyectoDIV1.Views.Empresa
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditarDatosEmpresaPage : ContentPage
    {
        EditarDatosViewModel _viewModel;
        public EditarDatosEmpresaPage(string usuarioId)
        {
            InitializeComponent();
            BindingContext = _viewModel = new EditarDatosViewModel();
            _viewModel.Id = usuarioId;
        }
    }
}