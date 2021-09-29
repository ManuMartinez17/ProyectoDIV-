using ProyectoDIV1.ViewModels.Notificaciones;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProyectoDIV1.Views.Notificaciones
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InfoNotificacionPage : ContentPage
    {
        private InfoNotificacionViewModel _viewModel;
        public InfoNotificacionPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new InfoNotificacionViewModel();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnApperaing();
        }
    }
}