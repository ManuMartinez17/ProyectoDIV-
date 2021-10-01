using Dasync.Collections;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ProyectoDIV1.DTOs;
using ProyectoDIV1.Entidades.Models;
using ProyectoDIV1.Helpers;
using ProyectoDIV1.Services.FirebaseServices;
using ProyectoDIV1.Services.Helpers;
using ProyectoDIV1.Views;
using ProyectoDIV1.Views.Notificaciones;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ProyectoDIV1.ViewModels.Notificaciones
{
    public class NotificacionesViewModel : BaseViewModel
    {
        private NotificacionesService _notificacionesService;
        private ObservableCollection<NotificacionDTO> _notificaciones;
        private ECandidato _candidato;
        private EEmpresa _empresa;
        private CandidatoService candidatoService;
        private string _mensaje;
        private string _noLeidas;
        private EmpresaService _empresaService;

        public NotificacionesViewModel()
        {

            candidatoService = new CandidatoService();
            _empresaService = new EmpresaService();
            _notificacionesService = new NotificacionesService();
            MoreInformationCommand = new Command<object>(NotificacionSelected, CanNavigate);
            RefreshCommand = new Command(async () => await RefreshNotificaciones());
        }

        private async void Deserializarusuario()
        {
            JObject Jobject = JObject.Parse(Settings.Usuario);
            try
            {
                var objetoCandidato = Jobject.ToObject(typeof(ECandidato));
                var objetoEmpresa = Jobject.ToObject(typeof(EEmpresa));
                if (objetoCandidato is ECandidato)
                {
                    _candidato = objetoCandidato as ECandidato;
                    var query = await candidatoService.GetCandidatoAsync(_candidato.UsuarioId);
                    if (query != null)
                    {
                        _candidato = query;
                    }
                    else
                    {
                        _candidato = null;
                    }
                }
                if (objetoEmpresa is EEmpresa)
                {
                    _empresa = objetoEmpresa as EEmpresa;
                    var query = await _empresaService.GetEmpresaAsync(_empresa.UsuarioId);
                    if (query != null)
                    {
                        _empresa = query;
                    }
                    else
                    {
                        _empresa = null;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public string Mensaje
        {
            get { return _mensaje; }
            set { SetProperty(ref _mensaje, value); }
        }
        public string NoLeidas
        {
            get { return _noLeidas; }
            set { SetProperty(ref _noLeidas, value); }
        }
        private async Task RefreshNotificaciones()
        {
            IsBusy = true;
            try
            {
                if (_candidato != null)
                {
                    await InsertarListadoNotificaciones(_candidato.UsuarioId, Constantes.COLLECTION_CANDIDATO);
                }
                else if (_empresa != null)
                {
                    await InsertarListadoNotificaciones(_empresa.UsuarioId, Constantes.COLLECTION_EMPRESA);
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task InsertarListadoNotificaciones(Guid usuarioId, string collection)
        {
            List<NotificacionDTO> lista = new List<NotificacionDTO>();
            List<ENotificacion> notificaciones = new List<ENotificacion>();
            if (collection.Equals(Constantes.COLLECTION_CANDIDATO))
            {
                notificaciones = await _notificacionesService.GetNotificacionesCandidatos(usuarioId);

            }
            else if (collection.Equals(Constantes.COLLECTION_EMPRESA))
            {
                notificaciones = await _notificacionesService.GetNotificacionesEmpresas(usuarioId);
            }
            if (notificaciones.Count > 0)
            {
                await notificaciones.ParallelForEachAsync(async item =>
                {
                    var candidatoEmisor = await insertarCandidatoEmisor(item.EmisorId);
                    if (candidatoEmisor == null)
                    {
                        var empresaEmisor = await insertarEmpresaEmisor(item.EmisorId);
                        if (empresaEmisor != null)
                        {
                            lista.Add(new NotificacionDTO()
                            {
                                IdEmisor = empresaEmisor.UsuarioId,
                                EmpresaEmisor = empresaEmisor,
                                Notificacion = item,
                                FullName = $"De: {empresaEmisor.RazonSocial}: {empresaEmisor.Nit}"
                            });
                        }
                    }
                    else
                    {
                        lista.Add(new NotificacionDTO()
                        {
                            IdEmisor = candidatoEmisor.UsuarioId,
                            CandidatoEmisor = candidatoEmisor,
                            Notificacion = item,
                            FullName = $"De: {candidatoEmisor.Nombre} {candidatoEmisor.Apellido}"
                        });
                    }

                }, maxDegreeOfParallelism: 10);
                Notificaciones = new ObservableCollection<NotificacionDTO>(lista);
                NoLeidas = notificaciones.Count(x => x.EstadoVisto == false).ToString();
                Mensaje = $"Tiene {NoLeidas} sin leer.";

            }
            else
            {
                Mensaje = "No tiene notificaciones por el momento.";
            }
        }

        public Command MoreInformationCommand { get; }
        public Command RefreshCommand { get; }
        private bool CanNavigate(object argument)
        {
            return true;
        }
        private async void NotificacionSelected(object objeto)
        {
            var lista = objeto as Syncfusion.ListView.XForms.ItemTappedEventArgs;
            var notificacion = lista.ItemData as NotificacionDTO;
            HelpDTO help = new HelpDTO();
            if (notificacion == null)
                return;

            if (_candidato != null)
            {
                help.UsuarioId = _candidato.UsuarioId.ToString();

                if (notificacion.IdEmisor.Equals(notificacion.CandidatoEmisor.UsuarioId))
                {
                    help.UsuarioEmisorId = notificacion.CandidatoEmisor.UsuarioId.ToString();
                    help.Collection = Constantes.COLLECTION_CANDIDATO;
                }
                else if (notificacion.IdEmisor.Equals(notificacion.EmpresaEmisor.UsuarioId))
                {
                    help.UsuarioEmisorId = notificacion.EmpresaEmisor.UsuarioId.ToString();
                    help.Collection = Constantes.COLLECTION_EMPRESA;
                }

            }
            else if (_empresa != null)
            {
                help.UsuarioId = _empresa.UsuarioId.ToString();
                if (notificacion.IdEmisor.Equals(notificacion.CandidatoEmisor.UsuarioId))
                {
                    help.UsuarioEmisorId = notificacion.CandidatoEmisor.UsuarioId.ToString();
                    help.Collection = Constantes.COLLECTION_CANDIDATO;
                }
                else if (notificacion.IdEmisor.Equals(notificacion.EmpresaEmisor.UsuarioId))
                {
                    help.UsuarioEmisorId = notificacion.EmpresaEmisor.UsuarioId.ToString();
                    help.Collection = Constantes.COLLECTION_EMPRESA;
                }
            }
            help.IdNotificacion = notificacion.Notificacion.Id.ToString();
            Settings.Token = JsonConvert.SerializeObject(help);
            await Shell.Current.GoToAsync($"{nameof(InfoNotificacionPage)}");
        }

        public void OnAppearing()
        {
            Deserializarusuario();
            LoadNotificaciones();
        }

        private async void LoadNotificaciones()
        {
            await PopupNavigation.Instance.PushAsync(new PopupLoadingPage());
            try
            {
                if (_candidato != null)
                {
                    await InsertarListadoNotificaciones(_candidato.UsuarioId, Constantes.COLLECTION_CANDIDATO);
                }
                else if (_empresa != null)
                {
                    await InsertarListadoNotificaciones(_empresa.UsuarioId, Constantes.COLLECTION_EMPRESA);
                }
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

        private async Task<EEmpresa> insertarEmpresaEmisor(Guid emisorId)
        {
            return await _empresaService.GetEmpresaAsync(emisorId);
        }

        private async Task<ECandidato> insertarCandidatoEmisor(Guid emisorId)
        {
            return await candidatoService.GetCandidatoAsync(emisorId);

        }

        public ObservableCollection<NotificacionDTO> Notificaciones
        {
            get { return _notificaciones; }
            set { SetProperty(ref _notificaciones, value); }
        }
    }
}
