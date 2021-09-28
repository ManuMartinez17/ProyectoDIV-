using ProyectoDIV1.ViewModels.Candidato;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProyectoDIV1.Views.Candidato
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditarDatosPage : ContentPage
    {
        EditarDatosViewModel _viewModel;
        public EditarDatosPage(string usuarioId)
        {
            InitializeComponent();
            BindingContext = _viewModel = new EditarDatosViewModel();
            _viewModel.Id = usuarioId;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}