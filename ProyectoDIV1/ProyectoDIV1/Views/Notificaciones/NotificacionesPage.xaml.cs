using ProyectoDIV1.DTOs;
using ProyectoDIV1.ViewModels.Notificaciones;
using Syncfusion.XForms.Buttons;
using System;
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

        private void BtnTerminar_Clicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var notificacion = button?.BindingContext as NotificacionDTO;
            _viewModel?.TerminarContratoCommand.Execute(notificacion);
        }

        private void BtnCalificar_Clicked(object sender, EventArgs e)
        {
            var button = sender as SfButton;
            var notificacion = button?.BindingContext as NotificacionDTO;
            _viewModel?.CalificarCommand.Execute(notificacion);
        }
    }
}