using Newtonsoft.Json;
using Plugin.Media;
using Plugin.Media.Abstractions;
using ProyectoDIV1.DTOs;
using ProyectoDIV1.Entidades.Models;
using ProyectoDIV1.Helpers;
using ProyectoDIV1.Services.FirebaseServices;
using ProyectoDIV1.Services.Helpers;
using ProyectoDIV1.Views.Empresa;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ProyectoDIV1.ViewModels.Empresa
{
    public class SettingsEmpresaViewModel : BaseViewModel
    {
        private FirebaseStorageHelper _firebaseStorage;
        private FirebaseHelper _firebaseHelper;
        private EmpresaService _empresaService;
        private ImageSource _imagen;
        private MediaFile _ImagenArchivo;
        private EmpresaDTO _empresa;
        private Stream _archivoCamaraDeComercio;
        private FileResult _camaraDeComercio;
        private string _extension;
        public SettingsEmpresaViewModel()
        {
            _firebaseStorage = new FirebaseStorageHelper();
            _firebaseHelper = new FirebaseHelper();
            _empresaService = new EmpresaService();
            _empresa = new EmpresaDTO();
            loadEmpresa();
            ImagenButtonUpload = string.IsNullOrEmpty(_empresa.Empresa.Rutas.NombreArchivoRegistro) ? "icon_plus.png" : "icon_edit.png";
            NavigateEditCommand = new Command(NavigateEditClicked);
            CambiarImagenCommand = new Command(CambiarImagenAsync);
            DownloadCamaraDeComercioCommand = new Command(DescargarArchivo);
            UploadcamaraDeComercioCommand = new Command(EditarArchivoClicked);
        }
        public Command CambiarImagenCommand { get; }
        public Command DownloadCamaraDeComercioCommand { get; }
        public Command NavigateEditCommand { get; }
        public Command UploadcamaraDeComercioCommand { get; }
        private void loadEmpresa()
        {
            var empresa = JsonConvert.DeserializeObject<EEmpresa>(Settings.Usuario);
            Empresa = new EmpresaDTO()
            {
                Empresa = empresa
            };
            Imagen = Empresa.Empresa.Rutas.RutaImagenRegistro;
        }

        private async void DescargarArchivo()
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(_empresa.Empresa.Rutas.NombreArchivoRegistro) &&
                      !string.IsNullOrWhiteSpace(_empresa.Empresa.Rutas.RutaArchivoRegistro))
                {
                    var url = new Uri(await _firebaseStorage.GetFile(_empresa.Empresa.Rutas.NombreArchivoRegistro, Constantes.CARPETA_CAMARADECOMERCIO));
                    await Browser.OpenAsync(url, BrowserLaunchMode.SystemPreferred);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private async void CambiarImagenAsync(object obj)
        {
            await CrossMedia.Current.Initialize();

            string source = await Application.Current.MainPage.DisplayActionSheet(
                "¿Donde quieres tomar tu foto?",
                "Cancelar",
                null,
                "Galería",
               "Cámara");

            if (source == "Cancelar")
            {
                _ImagenArchivo = null;
                return;
            }
            try
            {
                if (source == "Cámara")
                {
                    if (!CrossMedia.Current.IsCameraAvailable)
                    {
                        await Application.Current.MainPage.DisplayAlert("error", "No soporta la Cámara.", "Aceptar");
                        return;
                    }

                    _ImagenArchivo = await CrossMedia.Current.TakePhotoAsync(
                        new StoreCameraMediaOptions
                        {
                            Directory = "Sample",
                            Name = "test.jpg",
                            PhotoSize = PhotoSize.Small,
                        }
                    );
                }
                else
                {
                    if (!CrossMedia.Current.IsPickPhotoSupported)
                    {
                        await Application.Current.MainPage.DisplayAlert("error", "No hay galeria.", "Aceptar");
                        return;
                    }

                    _ImagenArchivo = await CrossMedia.Current.PickPhotoAsync();
                }

                if (_ImagenArchivo != null)
                {
                    Imagen = ImageSource.FromStream(() =>
                    {
                        Stream stream = _ImagenArchivo.GetStream();
                        return stream;
                    });
                    await EditarLogo();
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("error", ex.Message, "Aceptar");
                return;
            }
        }
        private async void EditarArchivoClicked()
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
            _camaraDeComercio = await FilePicker.PickAsync(new PickOptions
            {
                FileTypes = customFileType,
                PickerTitle = "Elegir el archivo pdf/word"
            });

            if (_camaraDeComercio != null)
            {
                _archivoCamaraDeComercio = await _camaraDeComercio.OpenReadAsync();
                await EditarArchivo();
            }
        }
        public EmpresaDTO Empresa
        {
            get => _empresa;
            set => SetProperty(ref _empresa, value);
        }
        public ImageSource Imagen
        {
            get => _imagen;
            set => SetProperty(ref _imagen, value);
        }
        public string ImagenButtonUpload { get; set; }
        public string Extension
        {
            get
            {
                return _extension = !string.IsNullOrEmpty(_empresa.Empresa.Rutas.NombreArchivoRegistro) ?
                  $"cámara de comercio{Path.GetExtension(_empresa.Empresa.Rutas.NombreArchivoRegistro)}" : "No ha cargado ningún archivo";
            }
            set { SetProperty(ref _extension, value); }
        }
        private async Task EditarArchivo()
        {
            try
            {
                if (_archivoCamaraDeComercio != null && _empresa != null)
                {
                    if (!string.IsNullOrWhiteSpace(_empresa.Empresa.Rutas.NombreArchivoRegistro) &&
                        !string.IsNullOrWhiteSpace(_empresa.Empresa.Rutas.RutaArchivoRegistro))
                    {
                        await _firebaseStorage.DeleteFile(_empresa.Empresa.Rutas.NombreArchivoRegistro, Constantes.CARPETA_CAMARADECOMERCIO);
                    }
                    string nombreCurriculum = $"{_empresa.Empresa.UsuarioId}{Path.GetExtension(_camaraDeComercio.FileName)}";
                    string RutaArchivo = await _firebaseStorage.UploadFile(_archivoCamaraDeComercio, nombreCurriculum, Constantes.CARPETA_CAMARADECOMERCIO);
                    _empresa.Empresa.Rutas.NombreArchivoRegistro = nombreCurriculum;
                    _empresa.Empresa.Rutas.RutaArchivoRegistro = RutaArchivo;
                    var query = await _empresaService.GetEmpresaFirebaseObjectAsync(_empresa.Empresa.UsuarioId);
                    await _firebaseHelper.UpdateAsync(_empresa.Empresa, Constantes.COLLECTION_EMPRESA, query);
                    Toasts.Success("Se actualizaron los datos.", 2000);
                    await Task.Delay(2000);
                    Settings.Usuario = JsonConvert.SerializeObject(_empresa);
                    Extension = $"Camara de comercio{Path.GetExtension(_empresa.Empresa.Rutas.NombreArchivoRegistro)}";
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private async Task EditarLogo()
        {
            try
            {
                if (_empresa != null)
                {
                    if (!string.IsNullOrWhiteSpace(_empresa.Empresa.Rutas.NombreArchivoRegistro) &&
                        !string.IsNullOrWhiteSpace(_empresa.Empresa.Rutas.NombreArchivoRegistro))
                    {
                        await _firebaseStorage.DeleteFile(_empresa.Empresa.Rutas.NombreImagenRegistro, Constantes.CARPETA_LOGOS_EMPRESAS);
                    }
                    string nombreImagen = $"{_empresa.Empresa.UsuarioId}{Path.GetExtension(_ImagenArchivo.Path)}";
                    string RutaImagen = await _firebaseStorage.UploadFile(_ImagenArchivo.GetStream(), nombreImagen, Constantes.CARPETA_LOGOS_EMPRESAS);
                    _empresa.Empresa.Rutas.NombreImagenRegistro = nombreImagen;
                    _empresa.Empresa.Rutas.RutaImagenRegistro = RutaImagen;
                    var query = await new EmpresaService().GetEmpresaFirebaseObjectAsync(_empresa.Empresa.UsuarioId);
                    await new FirebaseHelper().UpdateAsync(_empresa.Empresa, Constantes.COLLECTION_EMPRESA, query);
                    Toasts.Success("Se actualizo.", 2000);
                    await Task.Delay(1000);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private async void NavigateEditClicked()
        {
            await Shell.Current.Navigation.PushModalAsync(new EditarDatosEmpresaPage(_empresa.Empresa.UsuarioId.ToString()));
        }


       
    }
}
