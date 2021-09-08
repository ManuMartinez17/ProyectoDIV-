using ProyectoDIV1.ViewModels;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms.Xaml;

namespace ProyectoDIV1.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PopupVerAyudaPage : PopupPage
    {
        public string _texto { get; set; }
        PopupVerAyudaViewModel _viewModel;
        public PopupVerAyudaPage(string texto)
        {
            _texto = texto;
            InitializeComponent();
            BindingContext = _viewModel = new PopupVerAyudaViewModel();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.Texto = _texto;
        }
    }
}