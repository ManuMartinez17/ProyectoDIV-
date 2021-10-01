using ProyectoDIV1.ViewModels.Notificaciones;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProyectoDIV1.Views.Notificaciones
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NotificacionesPage : ContentPage
    {
        private NotificacionesViewModel _viewModel;
        public NotificacionesPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new NotificacionesViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}