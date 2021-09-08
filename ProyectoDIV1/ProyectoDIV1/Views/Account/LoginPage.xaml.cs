using ProyectoDIV1.ViewModels.Account;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProyectoDIV1.Views.Account
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        private LoginViewModel _viewModel;
        public LoginPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new LoginViewModel();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.Email = string.Empty;
            _viewModel.EmailValid = new Validators.ValidatableObject<string>();
            _viewModel.AddValidationRules();
            _viewModel.Password = string.Empty;
        }
    }
}