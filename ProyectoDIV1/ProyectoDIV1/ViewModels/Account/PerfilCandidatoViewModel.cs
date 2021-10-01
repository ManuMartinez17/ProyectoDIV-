using Newtonsoft.Json;
using Plugin.Media;
using Plugin.Media.Abstractions;
using ProyectoDIV1.DTOs;
using ProyectoDIV1.Entidades.Models;
using ProyectoDIV1.Helpers;
using ProyectoDIV1.Models;
using ProyectoDIV1.Services.FirebaseServices;
using ProyectoDIV1.Validators;
using ProyectoDIV1.Validators.Rules;
using ProyectoDIV1.Views;
using ProyectoDIV1.Views.Account;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ProyectoDIV1.ViewModels.Account
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
        private ArchivosDTO _archivos;
        private CandidatoModel _candidato;
        private ValidatableObject<string> _departamento;
        private ValidatableObject<string> _ciudad;
        private string _textoupload;
        private bool _visibleUpload;
        private string _textoButtonUpload;
        private ObservableCollection<string> _departamentosLista;
        private ObservableCollection<string> _ciudadesLista;
        #endregion

        #region Constructor
        public PerfilCandidatoViewModel()
        {
            _firebaseStorage = new FirebaseStorageHelper();
            _archivos = new ArchivosDTO();
            _candidato = new CandidatoModel();
            _departamento = new ValidatableObject<string>();
            _ciudad = new ValidatableObject<string>();
            LoadDepartamentos();
            AddValidationRules();
            Imagen = Application.Current.Resources["IconDefault"].ToString();
            CambiarImagenCommand = new Command(CambiarImagenAsync);
            InsertCommand = new Command(AgregarCandidatoOnclicked);
            UploadCurriculumCommand = new Command(UploadCurriculum);
            SignInCommand = new Command(OnSignIn);
            VisibleUpload = false;
            VerAyuda = new Command(VerHelpClicked);
            TextoButtonUpload = "hoja de vida pdf/docx";
            MaximumDate = DateTime.Today.AddYears(-18);
        }



        #endregion

        #region Commands
        public Command InsertCommand { get; }
        public Command VerAyuda { get; }
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
        public ValidatableObject<string> Ciudad
        {
            get
            {
                return _ciudad;
            }

            set
            {
                SetProperty(ref _ciudad, value);
            }
        }

        public ObservableCollection<string> CiudadesLista
        {
            get => _ciudadesLista;
            set { SetProperty(ref _ciudadesLista, value); }
        }


        public ValidatableObject<string> Departamento
        {
            get
            {
                if (!string.IsNullOrEmpty(_departamento.Value))
                {
                    CiudadesLista = new ObservableCollection<string>(LoadCiudades(_departamento.Value));
                }
                return _departamento;
            }
            set { SetProperty(ref _departamento, value); }
        }

        public ObservableCollection<string> DepartamentosLista
        {
            get { return _departamentosLista; }
            set { SetProperty(ref _departamentosLista, value); }
        }
        public CandidatoModel Candidato
        {
            get
            {
                return _candidato;
            }

            set
            {
                SetProperty(ref _candidato, value);
            }
        }


        #endregion

        #region Validaciones y carga de listas
        public void AddValidationRules()
        {
            Candidato.Nombre.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Nombre requerido." });
            Candidato.Apellido.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Apellido requerido." });
            Departamento.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Departamento requerido." });
            Ciudad.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Ciudad requerida." });
            Candidato.FechaDeNacimiento.Validations.Add(new HasValidAgeRule<DateTime> { ValidationMessage = "Debes tener 18 años o más." });
            Candidato.Celular.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Celular requerido." });
            Candidato.Celular.Validations.Add(new IsLenghtValidRule<string> { ValidationMessage = "El celular debe tener 10 digitos", MaximunLenght = 10, MinimunLenght = 10 });

            //Email Validation Rules
            Candidato.Email.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Email Requerido." });
            Candidato.Email.Validations.Add(new IsValidEmailRule<string> { ValidationMessage = "Email invalido." });
            //Password Validation Rules
            Candidato.Password.Item1.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Contraseña Requerida." });
            Candidato.Password.Item1.Validations.Add(new IsValidPasswordRule<string> { ValidationMessage = "Contraseña entre 8-20 caracteres; debe contener al menos una letra minúscula, una letra mayúscula, un dígito numérico y un carácter especial." });
            Candidato.Password.Item2.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Confirme la contraseña requerida." });
            Candidato.Password.Validations.Add(new MatchPairValidationRule<string> { ValidationMessage = "La contraseña y la contraseña de confirmación no coinciden." });

        }

        private bool ValidarFormulario()
        {
            string ciudad = Ciudad.Value;
            bool isDepartamentValid = Departamento.Validate();
            Ciudad.Value = ciudad;
            bool isCiudadValid = Ciudad.Validate();
            bool isFirstNameValid = Candidato.Nombre.Validate();
            bool isLastNameValid = Candidato.Apellido.Validate();
            bool isBirthDayValid = Candidato.FechaDeNacimiento.Validate();
            bool isPhoneNumberValid = Candidato.Celular.Validate();
          

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
        private List<string> LoadCiudades(string departamento)
        {
            List<string> ciudades = new List<string>();
            if (!string.IsNullOrEmpty(departamento))
            {
                JsonColombia ciudadescolombia = colombia.FirstOrDefault(x => x.Departamento.Equals(departamento));

                foreach (string item in ciudadescolombia.Ciudades)
                {
                    ciudades.Add(item);
                }
            }
            return ciudades;
        }

        #endregion

        #region Methods
        private async void VerHelpClicked()
        {
            await PopupNavigation.Instance.PushAsync(new PopupVerAyudaPage("Contraseña entre 8-20 caracteres; debe contener al menos una letra minúscula," +
                " una letra mayúscula, un dígito numérico y un carácter especial"));
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
                _archivos.Archivo = await StreamToByteArray(await _curriculum.OpenReadAsync());
                TextoUpload = "Se ha cargado la hoja de vida";
                TextoButtonUpload = $"{_curriculum.FileName}";
                VisibleUpload = true;
            }
        }
        public async Task<byte[]> StreamToByteArray(Stream input)
        {
            MemoryStream ms = new MemoryStream();
            await input.CopyToAsync(ms);
            return ms.ToArray();
        }
        private async void OnSignIn()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new LoginPage());
        }

        private async void CambiarImagenAsync()
        {
            await CrossMedia.Current.Initialize();
            try
            {
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
                    _archivos.ImagenPerfil = await StreamToByteArray(_ImagenArchivo.GetStream());
                }
            }
            catch (Exception ex)
            {
                Toasts.Error(ex.Message, 2000);
                return;
            }

        }


        private async void AgregarCandidatoOnclicked()
        {
            await PopupNavigation.Instance.PushAsync(new PopupLoadingPage());
            string RutaImagen = string.Empty;
            string ciudad = string.Empty;
            string RutaArchivo = string.Empty;
            string nombreCurriculum = string.Empty;
            string nombreImagen = string.Empty;
            try
            {
                Candidato.Nombre.Value = Candidato.Nombre.Value != null ? Candidato.Nombre.Value.Trim() : null;
                Candidato.Apellido.Value = Candidato.Apellido.Value != null ? Candidato.Apellido.Value.Trim() : null;
                Candidato.Email.Value = Candidato.Email.Value != null ? Candidato.Email.Value.Trim() : null;
                ciudad = Ciudad.Value;
                if (ValidarFormulario())
                {
                    bool existeEmail = await new CandidatoService().GetCandidatoByEmail(Candidato.Email.Value);
                    if (!existeEmail)
                    {
                        Candidato.UsuarioId = Guid.NewGuid();
                        if (_archivos.ImagenPerfil != null)
                        {
                            nombreImagen = $"{Candidato.UsuarioId}{Path.GetExtension(_ImagenArchivo.Path)}";
                        }
                        if (_archivos.Archivo != null)
                        {
                            nombreCurriculum = $"{Candidato.UsuarioId}{Path.GetExtension(_curriculum.FileName)}";
                        }

                        DateTime today = DateTime.Today;
                        int age = today.Year - Candidato.FechaDeNacimiento.Value.Year;

                        if (DateTime.Today.Month < Candidato.FechaDeNacimiento.Value.Month)
                        {
                            --age;
                        }
                        //sino preguntamos si estamos en el mismo mes, si es el mismo preguntamos si el dia de hoy es menor al de la fecha de nacimiento
                        else if (DateTime.Today.Month == Candidato.FechaDeNacimiento.Value.Month
                            && DateTime.Today.Day < Candidato.FechaDeNacimiento.Value.Day)
                        {
                            --age;
                        }

                        var entidad = new ECandidato
                        {
                            UsuarioId = Candidato.UsuarioId,
                            Departamento = Departamento.Value,
                            Nombre = Candidato.Nombre.Value,
                            Apellido = Candidato.Apellido.Value,
                            Email = Candidato.Email.Value,
                            Ciudad = Ciudad.Value,
                            Celular = Candidato.Celular.Value,
                            Edad = age,
                            Rutas =
                            {
                                NombreImagenRegistro = nombreImagen,
                                NombreArchivoRegistro = nombreCurriculum,
                            }
                        };
                        _archivos.Password = Candidato.Password.Item1.Value;
                        Settings.Archivos = JsonConvert.SerializeObject(_archivos);
                        Settings.Candidato = JsonConvert.SerializeObject(entidad);
                        await Shell.Current.GoToAsync(nameof(PerfilTrabajoPage));
                    }
                    else
                    {
                        Toasts.Error($"el correo {Candidato.Email.Value} ya esta en uso.", 3000);
                    }

                }
            }
            catch (Exception ex)
            {
                if (ex.Message.ToUpper().Contains("EMAIL"))
                {
                    Toasts.Error($"el correo {Candidato.Email.Value} ya esta en uso.", 3000);
                }
                else
                {
                    Toasts.Error($"Error {ex.Message}", 3000);
                }
            }
            finally
            {
                Ciudad.Value = ciudad;
                Ciudad.Value = Ciudad.Value != null ? CiudadesLista.FirstOrDefault(x => x.Equals(Ciudad.Value)) : null;
                await PopupNavigation.Instance.PopAsync();
            }
        }
        #endregion
    }
}