using ProyectoDIV1.ViewModels.Notificaciones;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProyectoDIV1.Views.Notificaciones
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NotificacionesCandidatosPage : ContentPage
    {
        private NotificacionesCandidatosViewModel _viewModel;
        public NotificacionesCandidatosPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new NotificacionesCandidatosViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}