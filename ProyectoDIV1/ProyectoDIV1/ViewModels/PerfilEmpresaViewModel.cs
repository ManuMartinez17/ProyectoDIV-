using Acr.UserDialogs;
using Newtonsoft.Json;
using Plugin.Media;
using Plugin.Media.Abstractions;
using ProyectoDIV1.Entidades.Models;
using ProyectoDIV1.Helpers;
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
    public class PerfilEmpresaViewModel : BaseViewModel
    {
        #region Attributes
        private FirebaseStorageHelper _firebaseStorage;
        private FirebaseHelper _firebaseHelper;
        private List<JsonColombia> colombia;
        private ImageSource _imagen;
        private MediaFile _ImagenArchivo;
        private FileResult _camaraDeComercio;
        private Ciudades _ciudad;
        private Empresa _empresa;
        private string _textoupload;
        private bool _visibleUpload;
        private string _textoButtonUpload;
        private Stream _archivocamaraDeComercio;
        private ObservableCollection<string> _departamentosLista;
        private ObservableCollection<Ciudades> _ciudadesLista;
        #endregion

        #region Constructor
        public PerfilEmpresaViewModel()
        {
            _firebaseStorage = new FirebaseStorageHelper();
            _firebaseHelper = new FirebaseHelper();
            LoadDepartamentos();
            _empresa = new Empresa();
            _ciudad = new Ciudades();
            AddValidationRules();
            Imagen = Application.Current.Resources["LogoDefault"].ToString();
            CambiarImagenCommand = new Command(CambiarImagenAsync);
            InsertCommand = new Command(AgregarEmpresaOnclicked);
            UploadcamaraDeComercioCommand = new Command(UploadcamaraDeComercio);
            SignInCommand = new Command(OnSignIn);
            VisibleUpload = false;
            TextoButtonUpload = "cámara de comercio pdf/docx";
        }


        #endregion

        #region Commands
        public Command InsertCommand { get; }
        public Command CambiarImagenCommand { get; set; }
        public Command UploadcamaraDeComercioCommand { get; set; }
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
        public Empresa Empresa
        {
            get
            {
                if (!string.IsNullOrEmpty(_empresa.Departamento.Value))
                {
                    CiudadesLista = new ObservableCollection<Ciudades>(LoadCiudades(_empresa.Departamento.Value));
                }
                return _empresa;
            }

            set
            {
                SetProperty(ref _empresa, value);
            }
        }

        public Ciudades Ciudad
        {
            get { return _ciudad; }
            set
            {

                SetProperty(ref _ciudad, value);
            }
        }
        #endregion

        #region Validaciones y carga de listas
        public void AddValidationRules()
        {
            Empresa.Departamento.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Departamento requerido." });
            Empresa.Ciudad.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Ciudad requerida." });
            Empresa.Celular.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Celular requerido." });
            Empresa.Celular.Validations.Add(new IsLenghtValidRule<string> { ValidationMessage = "El celular debe tener 10 digitos", MaximunLenght = 10, MinimunLenght = 10 });
            Empresa.Nit.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Celular requerido." });
            //Email Validation Rules
            Empresa.Email.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Email Requerido." });
            Empresa.Email.Validations.Add(new IsValidEmailRule<string> { ValidationMessage = "Email invalido." });
            //Password Validation Rules
            Empresa.Password.Item1.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Contraseña Requerida." });
            Empresa.Password.Item1.Validations.Add(new IsValidPasswordRule<string> { ValidationMessage = "Contraseña entre 8-20 caracteres; debe contener al menos una letra minúscula, una letra mayúscula, un dígito numérico y un carácter especial " });
            Empresa.Password.Item2.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Confirme la contraseña requerida." });
            Empresa.Password.Validations.Add(new MatchPairValidationRule<string> { ValidationMessage = "La contraseña y la contraseña de confirmación no coinciden." });

        }

        bool ValidarFormulario()
        {
            bool isNitValid = Empresa.Nit.Validate();
            bool isPhoneNumberValid = Empresa.Celular.Validate();
            bool isDepartamentValid = Empresa.Departamento.Validate();
            bool isCiudadValid = Empresa.Ciudad.Validate();
            bool isEmailValid = Empresa.Email.Validate();
            bool isPasswordValid = Empresa.Password.Validate();
            return isNitValid && isDepartamentValid && isCiudadValid
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

        private async void UploadcamaraDeComercio()
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
            _camaraDeComercio = await FilePicker.PickAsync(new PickOptions
            {
                FileTypes = customFileType,
                PickerTitle = "Elegir el archivo pdf/word"
            });

            if (_camaraDeComercio != null)
            {
                _archivocamaraDeComercio = await _camaraDeComercio.OpenReadAsync();
                TextoUpload = "Se ha cargado satisfactoriamente.";
                TextoButtonUpload = $"{_camaraDeComercio.FileName}";
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


        private async void AgregarEmpresaOnclicked()
        {
            string RutaImagen = string.Empty;
            string RutaArchivo = string.Empty;
            string nombrecamaraDeComercio = string.Empty;
            string nombreImagen = string.Empty;
            try
            {
                if (Ciudad != null)
                {
                    if (!string.IsNullOrWhiteSpace(Ciudad.Nombre))
                    {
                        _empresa.Ciudad.Value = Ciudad.Nombre;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            Empresa.Nit.Value = Empresa.Nit.Value.Trim();
            Empresa.Email.Value = Empresa.Email.Value.Trim();
            Empresa.Nombre.Value = Empresa.Nombre.Value.Trim();
            if (ValidarFormulario())
            {
                UserDialogs.Instance.ShowLoading("Cargando...");
                try
                {
                    var authService = DependencyService.Resolve<IAuthenticationService>();
                    if (await authService.Register($"{Empresa.Nit.Value}",
                        Empresa.Email.Value, Empresa.Password.Item1.Value))
                    {
                        Empresa.UsuarioId = Guid.NewGuid();
                        if (_ImagenArchivo != null)
                        {
                            nombreImagen = $"{Empresa.UsuarioId}{Path.GetExtension(_ImagenArchivo.Path)}";
                            RutaImagen = await _firebaseStorage.UploadFile(_ImagenArchivo.GetStream(), nombreImagen, Constantes.CARPETA_LOGOS_EMPRESAS);
                        }
                        if (_archivocamaraDeComercio != null)
                        {
                            nombrecamaraDeComercio = $"{Empresa.UsuarioId}{Path.GetExtension(_camaraDeComercio.FileName)}";
                            RutaArchivo = await _firebaseStorage.UploadFile(_archivocamaraDeComercio, nombrecamaraDeComercio, Constantes.CARPETA_CAMARADECOMERCIO);
                        }
                        var entidad = new EEmpresa
                        {
                            UsuarioId = Empresa.UsuarioId,
                            RazonSocial = Empresa.Nombre.Value,
                            Nit = Empresa.Nit.Value,
                            Departamento = Empresa.Departamento.Value,
                            Email = Empresa.Email.Value,
                            Ciudad = Empresa.Ciudad.Value,
                            Telefono = Empresa.Celular.Value,
                            Rutas =
                            {
                                RutaImagenRegistro= RutaImagen,
                                NombreImagenRegistro = nombreImagen,
                                NombreArchivoRegistro = nombrecamaraDeComercio,
                                RutaArchivoRegistro = RutaArchivo
                            }
                        };

                        Settings.Empresa = JsonConvert.SerializeObject(entidad);
                        await _firebaseHelper.CrearAsync(entidad, Constantes.COLLECTION_EMPRESA);
                        UserDialogs.Instance.HideLoading();
                        UserDialogs.Instance.Toast("Se ha registrado satisfactoriamente", TimeSpan.FromSeconds(2));
                        Application.Current.MainPage = new MasterEmpresaPage();
                        await Shell.Current.GoToAsync($"//{nameof(AboutPage)}");
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Alerta", $"el email {Empresa.Email.Value} ya existe", "OK");
                    }
                }
                catch (Exception ex)
                {
                    UserDialogs.Instance.HideLoading();
                    if (ex.Message.ToUpper().Contains("EMAIL"))
                    {
                        await Application.Current.MainPage.DisplayAlert("Alerta", "El correo ya esta en uso.", "OK");
                        return;
                    }
                    await Application.Current.MainPage.DisplayAlert("Alerta", $"{ex.Message}", "OK");
                    return;
                }
            }
        }

        #endregion
    }
}
