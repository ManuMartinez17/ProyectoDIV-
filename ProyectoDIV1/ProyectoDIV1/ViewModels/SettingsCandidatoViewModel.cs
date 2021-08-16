using Acr.UserDialogs;
using Newtonsoft.Json;
using Plugin.Media;
using Plugin.Media.Abstractions;
using ProyectoDIV1.DTOs;
using ProyectoDIV1.Entidades.Models;
using ProyectoDIV1.Helpers;
using ProyectoDIV1.Services;
using ProyectoDIV1.Views;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ProyectoDIV1.ViewModels
{
    public class SettingsCandidatoViewModel : BaseViewModel
    {
        private CandidatoDTO _candidato;
        private CandidatoService _candidatoService;
        private ImageSource _imagen;
        private FirebaseStorageHelper _firebaseStorage;
        private MediaFile _ImagenArchivo;
        public SettingsCandidatoViewModel()
        {
            _candidatoService = new CandidatoService();
            _firebaseStorage = new FirebaseStorageHelper();
            LoadCandidato();
            CambiarImagenCommand = new Command(CambiarImagenAsync);
            MoreInformationCommand = new Command(MoreInformation);
        }

        private async void MoreInformation()
        {
            try
            {
                await Shell.Current.Navigation.PushAsync(new EditarHojaDeVidaPage());
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

        }

        public Command CambiarImagenCommand { get; set; }
        public Command MoreInformationCommand { get; set; }
        public CandidatoDTO Candidato
        {
            get => _candidato;
            set => SetProperty(ref _candidato, value);
        }
        public ImageSource Imagen
        {
            get => _imagen;
            set => SetProperty(ref _imagen, value);
        }
        private void LoadCandidato()
        {
            ECandidato candidato = JsonConvert.DeserializeObject<ECandidato>(Settings.Usuario);
            candidato.Rutas.RutaImagenRegistro = string.IsNullOrEmpty(candidato.Rutas.RutaImagenRegistro) ?
              "https://i.postimg.cc/BQmWRFDZ/iconuser.jpg" : candidato.Rutas.RutaImagenRegistro;
            CandidatoDTO candidatoDTO = new CandidatoDTO()
            {
                Candidato = candidato

            };
            Candidato = candidatoDTO;
            Imagen = candidato.Rutas.RutaImagenRegistro;
        }

        private async void CambiarImagenAsync()
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
                        await App.Current.MainPage.DisplayAlert("error", "No soporta la Cámara.", "Aceptar");
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
                        await App.Current.MainPage.DisplayAlert("error", "No hay galeria.", "Aceptar");
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
                    await EditarImagenPerfil();
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("error", ex.Message, "Aceptar");
                return;
            }

        }
        private async Task EditarImagenPerfil()
        {
            try
            {
                if (_candidato != null)
                {
                    if (!string.IsNullOrWhiteSpace(_candidato.Candidato.Rutas.NombreArchivoRegistro) &&
                        !string.IsNullOrWhiteSpace(_candidato.Candidato.Rutas.NombreArchivoRegistro))
                    {
                        await _firebaseStorage.DeleteFile(_candidato.Candidato.Rutas.NombreImagenRegistro, Constantes.CARPETA_IMAGENES_PERFIL);
                    }
                    string nombreImagen = $"{_candidato.Candidato.UsuarioId}{Path.GetExtension(_ImagenArchivo.Path)}";
                    string RutaImagen = await _firebaseStorage.UploadFile(_ImagenArchivo.GetStream(), nombreImagen, Constantes.CARPETA_IMAGENES_PERFIL);
                    _candidato.Candidato.Rutas.NombreImagenRegistro = nombreImagen;
                    _candidato.Candidato.Rutas.RutaImagenRegistro = RutaImagen;
                    var query = await new CandidatoService().GetCandidatoFirebaseObjectAsync(_candidato.Candidato.UsuarioId);
                    await new FirebaseHelper().UpdateAsync<ECandidato>(_candidato.Candidato, Constantes.COLLECTION_CANDIDATO, query);
                    UserDialogs.Instance.Toast("Se ha actualizado satisfactoriamente.");
                    await Task.Delay(1000);
                    Settings.Usuario = JsonConvert.SerializeObject(_candidato.Candidato);
                    await Shell.Current.GoToAsync($"../../{nameof(SettingsCandidatoPage)}");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

    }
}
