using ProyectoDIV1.ViewModels;
using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProyectoDIV1.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PopupCalificarPage : PopupPage
    {
        PopupCalificarViewModel _viewModel;
        private Guid _idReceptor;
        private Guid _idNotificacion;
        private Guid _idEmisor;
        private string _fullName;
        public PopupCalificarPage(Guid IdReceptor, Guid IdNotificacion, Guid IdEmisor, string fullName)
        {
            InitializeComponent();
            _idReceptor = IdReceptor;
            _idNotificacion = IdNotificacion;
            _idEmisor = IdEmisor;
            _fullName = fullName;
            BindingContext = _viewModel = new PopupCalificarViewModel();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.IdReceptor = _idReceptor;
            _viewModel.IdNotificacion = _idNotificacion;
            _viewModel.IdEmisor = _idEmisor;
            _viewModel.Texto = _fullName;
        }
    }
}