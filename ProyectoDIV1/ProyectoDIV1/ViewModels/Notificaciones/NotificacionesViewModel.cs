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
        private ObservableCollection<NotificacionDTO> _notificaciones = new ObservableCollection<NotificacionDTO>();
        private ECandidato _candidato;
        private EEmpresa _empresa;
        private CandidatoService candidatoService;
        private string _noLeidas;
        private EmpresaService _empresaService;
        private bool _isEnabled;
        public NotificacionesViewModel()
        {

            candidatoService = new CandidatoService();
            _empresaService = new EmpresaService();
            _notificacionesService = new NotificacionesService();
            MoreInformationCommand = new Command<object>(NotificacionSelected, CanNavigate);
            RefreshCommand = new Command(async () => await RefreshNotificaciones());
        }

        public Command TerminarContratoCommand
        {
            get
            {
                return new Command<NotificacionDTO>((param) => TerminarContratoClicked(param));
            }
        }

        public Command CalificarCommand
        {
            get
            {
                return new Command<NotificacionDTO>((param) => CalificarClicked(param));
            }
        }

        private void CalificarClicked(NotificacionDTO notificacion)
        {

            try
            {
                if (notificacion != null)
                {
                    if (_candidato != null)
                    {
                        CalificarCandidato(notificacion);
                      
                    }
                    else if (_empresa != null)
                    {
                        CalificarEmpresa(notificacion);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private async void CalificarEmpresa(NotificacionDTO notificacion)
        {
            await PopupNavigation.Instance.PushAsync(new PopupCalificarPage(notificacion.Notificacion.EmisorId,
                            notificacion.Notificacion.Id, _empresa.UsuarioId, notificacion.FullName));
        }

        private async void CalificarCandidato(NotificacionDTO notificacion)
        {
            await PopupNavigation.Instance.PushAsync(new PopupCalificarPage(notificacion.Notificacion.EmisorId,
                            notificacion.Notificacion.Id, _candidato.UsuarioId, notificacion.FullName));
        }

        private async void TerminarContratoClicked(NotificacionDTO notificacion)
        {
            string fullName = string.Empty;
            string isUser = string.Empty;
            await PopupNavigation.Instance.PushAsync(new PopupLoadingPage("Enviando..."));
            try
            {
                if (notificacion != null)
                {
                    if (_candidato != null)
                    {
                        var candidatoActual = await candidatoService.GetCandidatoAsync(_candidato.UsuarioId);
                        foreach (var item in candidatoActual.Notificaciones)
                        {
                            if (item.Id == notificacion.Notificacion.Id)
                            {
                                if (item.EstadoAceptado)
                                {
                                    item.ContratoTerminado = true;
                                    break;
                                }
                            }
                        }
                        var query = await candidatoService.GetCandidatoFirebaseObjectAsync(_candidato.UsuarioId);
                        await candidatoService.UpdateAsync(candidatoActual, Constantes.COLLECTION_CANDIDATO, query);
                    }
                    else if (_empresa != null)
                    {
                        var empresaActual = await _empresaService.GetEmpresaAsync(_empresa.UsuarioId);
                        foreach (var item in empresaActual.Notificaciones)
                        {
                            if (item.Id == notificacion.Notificacion.Id)
                            {
                                if (item.EstadoAceptado)
                                {
                                    item.ContratoTerminado = true;
                                    break;
                                }
                            }
                        }
                        var query = await _empresaService.GetEmpresaFirebaseObjectAsync(_empresa.UsuarioId);
                        await _empresaService.UpdateAsync(empresaActual, Constantes.COLLECTION_EMPRESA, query);
                    }

                    var candidatoReceptor = await candidatoService.GetCandidatoFirebaseObjectAsync(notificacion.Notificacion.EmisorId);
                    var EmpresaReceptor = await _empresaService.GetEmpresaFirebaseObjectAsync(notificacion.Notificacion.EmisorId);
                    if (candidatoReceptor != null)
                    {
                        ENotificacion notificacionTerminada = new ENotificacion();

                        var candidato = await candidatoService.GetCandidatoAsync(notificacion.Notificacion.EmisorId);
                        notificacionTerminada.Id = Guid.NewGuid();
                        if (_candidato != null)
                        {
                            notificacionTerminada.EmisorId = _candidato.UsuarioId;
                            fullName = $"{_candidato.Nombre} {_candidato.Apellido}";
                            isUser = "el usuario";
                        }
                        else if (_empresa != null)
                        {

                            notificacionTerminada.EmisorId = _empresa.UsuarioId;
                            fullName = $"{_empresa.RazonSocial} NIT: {_empresa.Nit}";
                            isUser = "la empresa";
                        }
                        notificacionTerminada.Fecha = DateTime.Now;
                        notificacionTerminada.EstadoVisto = false;
                        notificacionTerminada.ContratoTerminado = true;
                        notificacionTerminada.Mensaje = "Califiqueme por favor.";
                        notificacionTerminada.EstadoAceptado = true;
                        if (candidato.Notificaciones == null)
                        {
                            candidato.Notificaciones = new List<ENotificacion>();
                        }
                        candidato.Notificaciones.Add(notificacionTerminada);
                        await candidatoService.UpdateAsync(candidato, Constantes.COLLECTION_CANDIDATO, candidatoReceptor);

                        new CorreoHelper().enviarCorreo(candidato.Email, notificacion.FullName, "Ya puede calificar.", $"el trabajo por {isUser} " +
                            $"{fullName} ha terminado califique por favor.");
                    }
                    else if (EmpresaReceptor != null)
                    {
                        var empresa = await _empresaService.GetEmpresaAsync(notificacion.Notificacion.EmisorId);
                        ENotificacion notificacionTerminada = new ENotificacion();
                        notificacionTerminada.Id = Guid.NewGuid();
                        if (_candidato != null)
                        {
                            notificacionTerminada.EmisorId = _candidato.UsuarioId;
                            fullName = $"{_candidato.Nombre} {_candidato.Apellido}";
                            isUser = "el usuario";
                        }
                        else if (_empresa != null)
                        {

                            notificacionTerminada.EmisorId = _empresa.UsuarioId;
                            fullName = $"{_empresa.RazonSocial} NIT: {_empresa.Nit}";
                            isUser = "la empresa";
                        }
                        notificacionTerminada.Fecha = DateTime.Now;
                        notificacionTerminada.EstadoVisto = false;
                        notificacionTerminada.ContratoTerminado = true;
                        notificacionTerminada.Mensaje = "Califiqueme por favor.";
                        notificacionTerminada.EstadoAceptado = true;
                        if (empresa.Notificaciones == null)
                        {
                            empresa.Notificaciones = new List<ENotificacion>();
                        }
                        empresa.Notificaciones.Add(notificacionTerminada);
                        await _empresaService.UpdateAsync(empresa, Constantes.COLLECTION_EMPRESA, EmpresaReceptor);
                        new CorreoHelper().enviarCorreo(empresa.Email, notificacion.FullName, "Ya puede calificar.", $"el trabajo por {isUser} " +
                            $"{fullName} ha terminado califique por favor.");
                    }
                    await Task.Delay(3000);
                    LoadNotificaciones();
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

        public string Mensaje { get; set; }
        public string NoLeidas
        {
            get { return _noLeidas; }
            set { SetProperty(ref _noLeidas, value); }
        }

        public bool IsEnabled
        {
            get { return _isEnabled; }
            set { SetProperty(ref _isEnabled, value); }
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
            if (notificaciones == null)
            {
                Mensaje = "No tiene notificaciones por el momento.";
            }
            else
            {
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
                                    FullName = $"{empresaEmisor.RazonSocial}: Nit: {empresaEmisor.Nit}"
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
                                FullName = $"{candidatoEmisor.Nombre} {candidatoEmisor.Apellido}"
                            });
                        }

                    }, maxDegreeOfParallelism: 10);
                    Notificaciones.Clear();
                    Notificaciones = new ObservableCollection<NotificacionDTO>(lista.OrderBy(x => x.Notificacion.Fecha));
                    NoLeidas = notificaciones.Count(x => x.EstadoVisto == false).ToString();
                    Mensaje = $"Tiene {NoLeidas} sin leer.";
                }
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
            if (Notificaciones.Count == 0)
            {
                Mensaje = "No tiene notificaciones por el momento.";
            }
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
