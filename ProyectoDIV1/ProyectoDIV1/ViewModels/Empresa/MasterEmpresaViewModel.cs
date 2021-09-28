using Acr.UserDialogs;
using Newtonsoft.Json;
using ProyectoDIV1.DTOs;
using ProyectoDIV1.Entidades.Models;
using ProyectoDIV1.Helpers;
using ProyectoDIV1.Services.FirebaseServices;
using ProyectoDIV1.Services.Interfaces;
using ProyectoDIV1.Views.Account;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ProyectoDIV1.ViewModels.Empresa
{
    public class MasterEmpresaViewModel : BaseViewModel
    {
        private EmpresaDTO _empresa;
        private EmpresaService _empresaService;

        public MasterEmpresaViewModel()
        {
            _empresaService = new EmpresaService();
            CheckWhetherTheUserIsSignInAsync();
        }

        public EmpresaDTO Empresa
        {
            get => _empresa;
            set => SetProperty(ref _empresa, value);
        }
        private async void CheckWhetherTheUserIsSignInAsync()
        {
            try
            {
                var authenticationService = DependencyService.Resolve<IAuthenticationService>();
                if (authenticationService.IsSignIn())
                {
                    string email = authenticationService.BuscarEmail();
                    var empresa = await BuscarIdEmpresa(email);
                    if (empresa != null)
                    {
                        Settings.Usuario = JsonConvert.SerializeObject(empresa);
                        LoadEmpresa(empresa);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        private async Task<EEmpresa> BuscarIdEmpresa(string email)
        {
            try
            {
                var candidato = await _empresaService.GetIdXEmail(email);
                return candidato;
            }
            catch (Exception ex)
            {

                Debug.WriteLine(ex.Message);
            }
            return default;
        }

        private void LoadEmpresa(EEmpresa empresa)
        {
            empresa.Rutas.RutaImagenRegistro = string.IsNullOrEmpty(empresa.Rutas.RutaImagenRegistro) ?
               "https://i.postimg.cc/zDkX2Zh7/logo.png" : empresa.Rutas.RutaImagenRegistro;
            EmpresaDTO empresaDTO = new EmpresaDTO()
            {
                Empresa = empresa

            };
            Empresa = empresaDTO;
        }
    }
}
