using Newtonsoft.Json;
using ProyectoDIV1.DTOs;
using ProyectoDIV1.Entidades.Models;
using ProyectoDIV1.Helpers;
using ProyectoDIV1.Services.FirebaseServices;
using ProyectoDIV1.Views;
using ProyectoDIV1.Views.Empresa;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ProyectoDIV1.ViewModels.Empresa
{
    public class EmpresasConServiciosViewModel : BaseViewModel
    {

        #region Attributes
        private ObservableCollection<EmpresaDTO> _empresas;
        private ObservableCollection<EEmpresa> _empresasBusqueda;
        private EmpresaService _empresaService;
        #endregion

        #region Constructor
        public EmpresasConServiciosViewModel()
        {
            _empresaService = new EmpresaService();
            LoadEmpresasBusqueda();
            LoadEmpresas();
            MoreInformationCommand = new Command<object>(EmpresaSelected, CanNavigate);
            MostrarListadoEmpresasCommand = new Command((param) => ExecuteListadoEmpresas(param));
            LoadEmpresasCommand = new Command(async () => await RefreshEmpresas());
        }

        private async void EmpresaSelected(object objeto)
        {
            var lista = objeto as Syncfusion.ListView.XForms.ItemTappedEventArgs;
            var empresa = lista.ItemData as EmpresaDTO;
            if (empresa == null)
                return;
            await Shell.Current.GoToAsync($"{nameof(EmpresaPage)}?{nameof(EmpresaViewModel.Id)}={empresa.Empresa.UsuarioId}");
        }
        #endregion
        private bool CanNavigate(object argument)
        {
            return true;
        }
        #region Commands
        public Command MostrarListadoEmpresasCommand { get; set; }
        public Command LoadEmpresasCommand { get; }
        public Command MoreInformationCommand { get; }
        #endregion

        #region Properties
        public ObservableCollection<EmpresaDTO> Empresas
        {
            get { return _empresas; }
            set { SetProperty(ref _empresas, value); }
        }

        public ObservableCollection<EEmpresa> EmpresasBusqueda
        {
            get { return _empresasBusqueda; }
            set { SetProperty(ref _empresasBusqueda, value); }
        }
        #endregion

        #region Methods
        private async void LoadEmpresas()
        {
            await PopupNavigation.Instance.PushAsync(new PopupLoadingPage());
            try
            {
                if (Empresas != null)
                {
                    Empresas.Clear();
                }
                var empresas = await _empresaService.GetEmpresas();
                var user = JsonConvert.DeserializeObject<EEmpresa>(Settings.Usuario);
                var empresaIam = empresas.Find(x => x.UsuarioId == user.UsuarioId);
                if (empresaIam != null)
                {
                    empresas.Remove(empresaIam);
                }
                List<EmpresaDTO> empresasDTOs = new List<EmpresaDTO>();
                empresas.ForEach(x => empresasDTOs.Add(new EmpresaDTO
                {
                    Empresa = x
                }));
                Empresas = new ObservableCollection<EmpresaDTO>(empresasDTOs);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                await PopupNavigation.Instance.PopAsync();
            }
        }
        private Task RefreshEmpresas()
        {
            throw new NotImplementedException();
        }

        private async void ExecuteListadoEmpresas(object param)
        {
            try
            {
                var busqueda = param as EEmpresa;
                if (busqueda != null)
                {
                    if (Empresas != null)
                    {
                        Empresas.Clear();
                    }
                    await PopupNavigation.Instance.PushAsync(new PopupLoadingPage());
                    var empresas = await _empresaService.GetEmpresaBySearch(busqueda.RazonSocial);
                    List<EmpresaDTO> empresasDTOs = new List<EmpresaDTO>();
                    Parallel.ForEach(empresas,
                        item =>
                        {
                            empresasDTOs.Add(new EmpresaDTO
                            {
                                Empresa = item
                            });
                        });
                    Empresas = new ObservableCollection<EmpresaDTO>(empresasDTOs);
                    await PopupNavigation.Instance.PopAllAsync();
                }
                else
                {
                    LoadEmpresas();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private async void LoadEmpresasBusqueda()
        {
            try
            {
                if (Empresas != null)
                {
                    Empresas.Clear();
                }
                var empresas = await _empresaService.GetEmpresas();
                EmpresasBusqueda = new ObservableCollection<EEmpresa>(empresas);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
        #endregion
    }
}
