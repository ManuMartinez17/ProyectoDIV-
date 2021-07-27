using Newtonsoft.Json;
using Plugin.Media;
using Plugin.Media.Abstractions;
using ProyectoDIV1.DTOs;
using ProyectoDIV1.Entidades.Models;
using ProyectoDIV1.Helpers;
using ProyectoDIV1.Services;
using System.IO;
using Xamarin.Forms;

namespace ProyectoDIV1.ViewModels
{
    public class SettingsCandidatoViewModel : BaseViewModel
    {
        private CandidatoDTO _candidato;
        private CandidatoService _candidatoService;
        private ImageSource _imagen;
        private MediaFile _ImagenArchivo;
        public SettingsCandidatoViewModel()
        {
            _candidatoService = new CandidatoService();

            LoadCandidato();
            CambiarImagenCommand = new Command(CambiarImagenAsync);
        }
        public Command CambiarImagenCommand { get; set; }
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
            CandidatoDTO candidatoDTO = new CandidatoDTO()
            {
                Candidato = candidato

            };
            Candidato = candidatoDTO;
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
            }
        }

    }
}
