using ProyectoDIV1.ViewModels.Notificaciones;
using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProyectoDIV1.Views.Notificaciones
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PopupEnviarNotificacionPage : PopupPage
    {
        private PopupEnviarNotificacionViewModel _model;
        public PopupEnviarNotificacionPage(string id)
        {
            InitializeComponent();
            BindingContext = _model = new PopupEnviarNotificacionViewModel();
            _model.IdReceptor = id;
        }
    }
}