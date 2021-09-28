using ProyectoDIV1.DTOs;
using ProyectoDIV1.Services.FirebaseServices;
using ProyectoDIV1.Views.Notificaciones;
using Rg.Plugins.Popup.Services;
using System;
using Xamarin.Forms;

namespace ProyectoDIV1.ViewModels.Empresa
{
    [QueryProperty(nameof(Id), nameof(Id))]
    public class EmpresaViewModel : BaseViewModel
    {
        private string _id;
        private EmpresaDTO _empresa;
        private EmpresaService _empresaService;
        public EmpresaViewModel()
        {
            _empresaService = new EmpresaService();
            ContactarCommand = new Command(MostrarPopupCreateNotification);
        }
        public Command ContactarCommand { get; }

        public string Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
                LoadEmpresa(value);
            }
        }
        public EmpresaDTO Empresa
        {
            get { return _empresa; }
            set
            {
                SetProperty(ref _empresa, value);
            }
        }
        private async void MostrarPopupCreateNotification()
        {
            await PopupNavigation.Instance.PushAsync(new PopupEnviarNotificacionPage(_empresa.Empresa.UsuarioId.ToString()));
        }
        private async void LoadEmpresa(string value)
        {
            var id = new Guid(value);
            var empresa = await _empresaService.GetEmpresaAsync(id);
            if (empresa == null)
            {
                return;
            }
            Empresa = new EmpresaDTO()
            {
                Empresa = empresa
            };
        }
    }
}
