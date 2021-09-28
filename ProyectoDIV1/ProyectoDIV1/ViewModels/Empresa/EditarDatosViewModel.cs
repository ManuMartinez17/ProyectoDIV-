using Newtonsoft.Json;
using ProyectoDIV1.Entidades.Models;
using ProyectoDIV1.Helpers;
using ProyectoDIV1.Services.FirebaseServices;
using ProyectoDIV1.Services.Helpers;
using ProyectoDIV1.Validators;
using ProyectoDIV1.Validators.Rules;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ProyectoDIV1.ViewModels.Empresa
{
    public class EditarDatosViewModel : BaseViewModel
    {
        private string _id;
        private EmpresaService _empresaService;
        private EEmpresa _empresa;
        private FirebaseHelper _firebase;
        private ValidatableObject<string> _telefono = new ValidatableObject<string>();
        private ValidatableObject<string> _RazonSocial = new ValidatableObject<string>();
        public EditarDatosViewModel()
        {
            _empresaService = new EmpresaService();
            _empresa = new EEmpresa();
            _firebase = new FirebaseHelper();
            GuardarCommand = new Command(GuardarClicked);
        }


        public Command GuardarCommand { get; set; }

        public void AddValidationRules()
        {
            _telefono.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Celular requerido." });
            _telefono.Validations.Add(new IsLenghtValidRule<string>
            {
                MaximunLenght = 10,
                MinimunLenght = 7,
                ValidationMessage = "El telefono y/o celular debe tener minimo 7 y maximo 10 digitos."

            });

        }
        bool ValidarFormulario()
        {
            bool isRazonSocialValid = RazonSocial.Validate();
            bool isPhoneNumberValid = Telefono.Validate();
            return isPhoneNumberValid && isRazonSocialValid;
        }
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
        public ValidatableObject<string> Telefono
        {
            get => _telefono;
            set => SetProperty(ref _telefono, value);
        }
        public ValidatableObject<string> RazonSocial
        {
            get => _RazonSocial;
            set => SetProperty(ref _RazonSocial, value);
        }
        public EEmpresa Empresa
        {
            get => _empresa;
            set => SetProperty(ref _empresa, value);
        }

        private async void LoadEmpresa(string value)
        {
            var id = new Guid(value);
            Empresa = await _empresaService.GetEmpresaAsync(id);
            RazonSocial.Value = Empresa.RazonSocial;
            Telefono.Value = Empresa.Telefono;
        }

        private async void GuardarClicked()
        {
            try
            {
                if (ValidarFormulario())
                {
                    Empresa.RazonSocial = RazonSocial.Value.Trim();
                    Empresa.Telefono = Telefono.Value.Trim();
                    var query = await _empresaService.GetEmpresaFirebaseObjectAsync(Empresa.UsuarioId);
                    await _firebase.UpdateAsync(Empresa, Constantes.COLLECTION_EMPRESA, query);
                    Toasts.Success("Se actualizo el perfil.", 2000);
                    await Task.Delay(2000);
                    Settings.Usuario = JsonConvert.SerializeObject(Empresa);
                    await Shell.Current.Navigation.PopModalAsync();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

        }
    }
}
