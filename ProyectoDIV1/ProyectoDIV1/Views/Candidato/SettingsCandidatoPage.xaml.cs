
using ProyectoDIV1.ViewModels.Candidato;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProyectoDIV1.Views.Candidato
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsCandidatoPage : ContentPage
    {
        private SettingsCandidatoViewModel _viewModel;
        public SettingsCandidatoPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new SettingsCandidatoViewModel();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}