using Acr.UserDialogs;
using Newtonsoft.Json;
using ProyectoDIV1.Entidades.Models;
using ProyectoDIV1.Helpers;
using ProyectoDIV1.Services;
using ProyectoDIV1.Views;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ProyectoDIV1.ViewModels
{
    public class EditarHojaDeVidaViewModel : BaseViewModel
    {
        private ObservableCollection<Lista> _habilidades;
        private ECandidato _candidato;
        private string _extension;
        private FileResult _curriculum;
        private Stream _archivoCurriculum;
        private FirebaseStorageHelper _firebaseStorage;
        private CandidatoService _candidatoService;
        private FirebaseHelper _firebase;
        public string ImagenButtonBuscador { get; set; }
        public string ImagenButtonUpload { get; set; }

        public EditarHojaDeVidaViewModel()
        {
            _firebaseStorage = new FirebaseStorageHelper();
            _firebase = new FirebaseHelper();
            _candidatoService = new CandidatoService();
            _candidato = JsonConvert.DeserializeObject<ECandidato>(Settings.Usuario);
            ImagenButtonBuscador = _candidato.Habilidades.Count == 0 ? "icon_plus.png" : "icon_edit.png";
            ImagenButtonUpload = string.IsNullOrEmpty(_candidato.Rutas.NombreArchivoRegistro) ? "icon_plus.png" : "icon_edit.png";
            UploadCurriculumCommand = new Command(UploadCurriculum);
            DownloadCurriculumCommand = new Command(DescargarCurriculum);
            MostrarBuscadorCommand = new Command(MostrarBuscador);
            BorrarHabilidadCommand = new Command((param) => ExecuteBorrarSkill(param));
        }


        private async void ExecuteBorrarSkill(object param)
        {
            var item = param as string;
            var query = await _candidatoService.GetCandidatoFirebaseObjectAsync(_candidato.UsuarioId);
            var candidato = await _candidatoService.GetCandidatoAsync(_candidato.UsuarioId);
            var lista = _candidatoService.BorrarHabilidadCandidato(candidato.Habilidades, item);
            candidato.Habilidades = lista;
            await _firebase.UpdateAsync<ECandidato>(candidato, Constantes.COLLECTION_CANDIDATO, query);
            Settings.Usuario = JsonConvert.SerializeObject(candidato);
            ImagenButtonBuscador = candidato.Habilidades.Count == 0 ? "icon_plus.png" : "icon_edit.png";
        }

        public Command UploadCurriculumCommand { get; set; }
        public Command MostrarBuscadorCommand { get; set; }
        public Command BorrarHabilidadCommand { get; set; }
        public Command DownloadCurriculumCommand { get; set; }
        private async void MostrarBuscador()
        {
            var candidato = await _candidatoService.GetCandidatoAsync(_candidato.UsuarioId);
            Settings.Candidato = JsonConvert.SerializeObject(candidato);
            await PopupNavigation.Instance.PushAsync(new BuscadorPage());
        }

        private async void DescargarCurriculum()
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(_candidato.Rutas.NombreArchivoRegistro) &&
                      !string.IsNullOrWhiteSpace(_candidato.Rutas.RutaArchivoRegistro))
                {
                    var url = new Uri(await _firebaseStorage.GetFile(_candidato.Rutas.NombreArchivoRegistro, Constantes.CARPETA_HOJASDEVIDA));
                    await Browser.OpenAsync(url, BrowserLaunchMode.SystemPreferred);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
        private async void UploadCurriculum()
        {
            var customFileType =
                new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                     { DevicePlatform.iOS, new[] { "com.adobe.pdf" , "com.microsoft.word.doc" } }, // or general UTType values
                     { DevicePlatform.Android, new[] { "application/pdf", "application/msword",
                         "application/vnd.openxmlformats-officedocument.wordprocessingml.document" } },
                     { DevicePlatform.UWP, new[] { ".pdf" ,".doc"} },
                     { DevicePlatform.Tizen, new[] { "*/*" } },
                     { DevicePlatform.macOS, new[] { "pdf","doc"} }, // or general UTType values
                });
            _curriculum = await FilePicker.PickAsync(new PickOptions
            {
                FileTypes = customFileType,
                PickerTitle = "Elegir el archivo pdf/word"
            });

            if (_curriculum != null)
            {
                _archivoCurriculum = await _curriculum.OpenReadAsync();
                await EditarCurriculum();
            }
        }

        private async Task EditarCurriculum()
        {
            try
            {
                if (_archivoCurriculum != null && _candidato != null)
                {
                    if (!string.IsNullOrWhiteSpace(_candidato.Rutas.NombreArchivoRegistro) &&
                        !string.IsNullOrWhiteSpace(_candidato.Rutas.RutaArchivoRegistro))
                    {
                        await _firebaseStorage.DeleteFile(_candidato.Rutas.NombreArchivoRegistro, Constantes.CARPETA_HOJASDEVIDA);
                    }
                    string nombreCurriculum = $"{_candidato.UsuarioId}{Path.GetExtension(_curriculum.FileName)}";
                    string RutaArchivo = await _firebaseStorage.UploadFile(_archivoCurriculum, nombreCurriculum, Constantes.CARPETA_HOJASDEVIDA);
                    _candidato.Rutas.NombreArchivoRegistro = nombreCurriculum;
                    _candidato.Rutas.RutaArchivoRegistro = RutaArchivo;
                    var query = await new CandidatoService().GetCandidatoFirebaseObjectAsync(_candidato.UsuarioId);
                    await _firebase.UpdateAsync<ECandidato>(_candidato, Constantes.COLLECTION_CANDIDATO, query);
                    UserDialogs.Instance.Toast("Se ha actualizado satisfactoriamente.");
                    await Task.Delay(1000);
                    Settings.Usuario = JsonConvert.SerializeObject(_candidato);
                    await Shell.Current.GoToAsync($"../../{nameof(SettingsCandidatoPage)}");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public ECandidato Candidato
        {
            get { return _candidato; }
            set
            {
                SetProperty(ref _candidato, value);
            }
        }
        public ObservableCollection<Lista> Habilidades
        {
            get { return _habilidades = _candidato != null ? new ObservableCollection<Lista>(_candidato.Habilidades) : null; }
            set { SetProperty(ref _habilidades, value); }
        }

        public string Extension
        {
            get
            {
                return _extension = !string.IsNullOrEmpty(_candidato.Rutas.NombreArchivoRegistro) ?
                  $"Hoja de vida{Path.GetExtension(_candidato.Rutas.NombreArchivoRegistro)}" : "No ha cargado ninguna hoja de vida.";
            }
            set { SetProperty(ref _extension, value); }
        }
    }

}
