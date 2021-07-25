using Acr.UserDialogs;
using Newtonsoft.Json;
using Plugin.Media;
using Plugin.Media.Abstractions;
using ProyectoDIV1.Entidades.Models;
using ProyectoDIV1.Interfaces;
using ProyectoDIV1.Models;
using ProyectoDIV1.Services;
using ProyectoDIV1.Validators.Rules;
using ProyectoDIV1.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ProyectoDIV1.ViewModels
{
    public class PerfilCandidatoViewModel : BaseViewModel
    {
        #region Attributes
        private FirebaseStorageHelper _firebaseStorage;
        private List<JsonColombia> colombia;
        private ImageSource _imagen;
        private MediaFile _ImagenArchivo;
        private DateTime _maximumDate;
        private FileResult _curriculum;
        private Ciudades _ciudad;
        private Candidato _candidato;
        private string _textoupload;
        private bool _visibleUpload;
        private string _textoButtonUpload;
        private Stream _archivoCurriculum;
        private ObservableCollection<string> _departamentosLista;
        private ObservableCollection<Ciudades> _ciudadesLista;
        #endregion

        #region Constructor
        public PerfilCandidatoViewModel()
        {
            _firebaseStorage = new FirebaseStorageHelper();
            LoadDepartamentos();
            _candidato = new Candidato();
            _ciudad = new Ciudades();
            AddValidationRules();
            Imagen = App.Current.Resources["IconDefault"].ToString();
            CambiarImagenCommand = new Command(CambiarImagenAsync);
            InsertCommand = new Command(AgregarCandidatoOnclicked);
            UploadCurriculumCommand = new Command(UploadCurriculum);
            SignInCommand = new Command(OnSignIn);
            VisibleUpload = false;
            TextoButtonUpload = "Subir hoja de vida pdf/docx";
            MaximumDate = DateTime.Today.AddYears(-18);
        }


        #endregion

        #region Commands
        public Command InsertCommand { get; }
        public Command CambiarImagenCommand { get; set; }
        public Command UploadCurriculumCommand { get; set; }
        public Command SignInCommand { get; }

        #endregion

        #region Properties

        public ImageSource Imagen
        {
            get => _imagen;
            set => SetProperty(ref _imagen, value);
        }

        public string TextoUpload
        {
            get => _textoupload;
            set => SetProperty(ref _textoupload, value);
        }
        public DateTime MaximumDate
        {
            get => _maximumDate;
            set => SetProperty(ref _maximumDate, value);
        }
        public string TextoButtonUpload
        {
            get => _textoButtonUpload;
            set => SetProperty(ref _textoButtonUpload, value);

        }
        public bool VisibleUpload
        {
            get => _visibleUpload;
            set => SetProperty(ref _visibleUpload, value);
        }

        public ObservableCollection<Ciudades> CiudadesLista
        {
            get { return _ciudadesLista; }
            set { SetProperty(ref _ciudadesLista, value); }
        }


        public ObservableCollection<string> DepartamentosLista
        {
            get { return _departamentosLista; }
            set { SetProperty(ref _departamentosLista, value); }
        }
        public Candidato Candidato
        {
            get
            {
                if (!string.IsNullOrEmpty(_candidato.Departamento.Value))
                {
                    CiudadesLista = new ObservableCollection<Ciudades>(LoadCiudades(_candidato.Departamento.Value));
                }
                return _candidato;
            }

            set
            {
                SetProperty(ref _candidato, value);
            }
        }

        public Ciudades Ciudad
        {
            get { return _ciudad; }
            set { SetProperty(ref _ciudad, value); }
        }
        #endregion

        #region Validaciones y carga de listas
        public void AddValidationRules()
        {
            Candidato.Nombre.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Nombre requerido." });
            Candidato.Apellido.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Apellido requerido." });
            Candidato.Departamento.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Departamento requerido." });
            Candidato.Ciudad.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Ciudad requerida." });
            Candidato.FechaDeNacimiento.Validations.Add(new HasValidAgeRule<DateTime> { ValidationMessage = "Debes tener 18 años o más." });
            Candidato.Celular.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Celular requerido." });
            Candidato.Celular.Validations.Add(new IsLenghtValidRule<string> { ValidationMessage = "El celular debe tener 10 digitos", MaximunLenght = 10, MinimunLenght = 10 });

            //Email Validation Rules
            Candidato.Email.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Email Requerido." });
            Candidato.Email.Validations.Add(new IsValidEmailRule<string> { ValidationMessage = "Email invalido." });
            //Password Validation Rules
            Candidato.Password.Item1.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Contraseña Requerida." });
            Candidato.Password.Item1.Validations.Add(new IsValidPasswordRule<string> { ValidationMessage = "Contraseña entre 8-20 caracteres; debe contener al menos una letra minúscula, una letra mayúscula, un dígito numérico y un carácter especial " });
            Candidato.Password.Item2.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Confirme la contraseña requerida." });
            Candidato.Password.Validations.Add(new MatchPairValidationRule<string> { ValidationMessage = "La contraseña y la contraseña de confirmación no coinciden." });

        }

        bool validarFormulario()
        {
            bool isFirstNameValid = Candidato.Nombre.Validate();
            bool isLastNameValid = Candidato.Apellido.Validate();
            bool isBirthDayValid = Candidato.FechaDeNacimiento.Validate();
            bool isPhoneNumberValid = Candidato.Celular.Validate();
            bool isDepartamentValid = Candidato.Departamento.Validate();
            bool isCiudadValid = Candidato.Ciudad.Validate();
            bool isEmailValid = Candidato.Email.Validate();
            bool isPasswordValid = Candidato.Password.Validate();
            return isFirstNameValid && isLastNameValid && isBirthDayValid && isDepartamentValid && isCiudadValid
                   && isPhoneNumberValid && isEmailValid && isPasswordValid;
        }
        private async void LoadDepartamentos()
        {
            colombia = await new JsonColombia().DeserializarJsonColombia();
            DepartamentosLista = new ObservableCollection<string>(new JsonColombia().LoadDepartaments(colombia));

        }
        private List<Ciudades> LoadCiudades(string departamento)
        {
            List<Ciudades> ciudades = new List<Ciudades>();
            if (!string.IsNullOrEmpty(departamento))
            {
                JsonColombia ciudadescolombia = colombia.FirstOrDefault(x => x.Departamento.Equals(departamento));

                foreach (var item in ciudadescolombia.Ciudades)
                {
                    var ciudad = new Ciudades
                    {
                        Nombre = item
                    };
                    ciudades.Add(ciudad);
                }
            }
            return ciudades;
        }

        #endregion

        #region Methods

        private async void UploadCurriculum()
        {
            var customFileType =
                new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                     { DevicePlatform.iOS, new[] { "com.adobe.pdf" , "com.microsoft.word.doc" } }, // or general UTType values
                     { DevicePlatform.Android, new[] { "application/pdf", "application/msword", "application/vnd.openxmlformats-officedocument.wordprocessingml.document" } },
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
                TextoUpload = "Se ha cargado la hoja de vida";
                TextoButtonUpload = $"{_curriculum.FileName}";
                VisibleUpload = true;
            }
        }

        private async void OnSignIn()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new LoginPage());
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


        private async void AgregarCandidatoOnclicked()
        {
            string RutaImagen = string.Empty;
            string RutaArchivo = string.Empty;
            try
            {
                if (Ciudad != null)
                {
                    if (!string.IsNullOrWhiteSpace(Ciudad.Nombre))
                    {
                        _candidato.Ciudad.Value = Ciudad.Nombre;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            if (validarFormulario())
            {
                Candidato.Nombre.Value = Candidato.Nombre.Value.Trim();
                Candidato.Apellido.Value = Candidato.Apellido.Value.Trim();
                Candidato.Email.Value = Candidato.Email.Value.Trim();
                UserDialogs.Instance.ShowLoading("Cargando...");
                try
                {
                    var authService = DependencyService.Resolve<IAuthenticationService>();
                    if (await authService.Register($"{Candidato.Nombre.Value} {Candidato.Apellido.Value}",
                        Candidato.Email.Value, Candidato.Password.Item1.Value))
                    {
                        Candidato.UsuarioId = Guid.NewGuid();
                        if (_ImagenArchivo != null)
                        {
                            string nombreImagen = $"{Candidato.UsuarioId}{Path.GetExtension(_ImagenArchivo.Path)}";
                            RutaImagen = await _firebaseStorage.UploadFile(_ImagenArchivo.GetStream(), nombreImagen, "imagenesdeperfil");
                        }
                        if (_archivoCurriculum != null)
                        {
                            string nombreCurriculum = $"{Candidato.UsuarioId}{Path.GetExtension(_curriculum.FileName)}";
                            RutaArchivo = await _firebaseStorage.UploadFile(_archivoCurriculum, nombreCurriculum, "hojasdevida");
                        }
                        DateTime today = DateTime.Today;
                        int age = today.Year - Candidato.FechaDeNacimiento.Value.Year;
                        var entidad = new ECandidato
                        {
                            UsuarioId = Candidato.UsuarioId,
                            Departamento = Candidato.Departamento.Value,
                            Nombre = Candidato.Nombre.Value,
                            Apellido = Candidato.Apellido.Value,
                            Email = Candidato.Email.Value,
                            Ciudad = Candidato.Ciudad.Value,
                            Celular = Candidato.Celular.Value,
                            Edad = age,
                            Rutas =
                            {
                                 RutaImagenRegistro= RutaImagen,
                                RutaArchivoRegistro = RutaArchivo
                            }
                        };
                        UserDialogs.Instance.HideLoading();
                        var jsonContact = JsonConvert.SerializeObject(entidad);
                        await Shell.Current.GoToAsync($"PerfilTrabajoPage?candidato={jsonContact}");
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert("Alerta", $"el email {Candidato.Email.Value} ya existe", "OK");
                    }
                }
                catch (Exception ex)
                {
                    await App.Current.MainPage.DisplayAlert("Alerta", $"{ex.Message}", "OK");
                    return;
                }
            }
        }

        #endregion
    }
}