using Acr.UserDialogs;
using Firebase.Storage;
using Plugin.Media;
using Plugin.Media.Abstractions;
using ProyectoDIV1.Entidades;
using ProyectoDIV1.Helpers.Funciones;
using ProyectoDIV1.Models;
using ProyectoDIV1.Services;
using ProyectoDIV1.Validators.Rules;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace ProyectoDIV1.ViewModels
{
    public class PerfilCandidatoViewModel : BaseViewModel
    {
        #region Attributes
        private FirebaseHelper _firebaseHelper;
        private FirebaseStorageHelper _firebaseStorage;
        private List<JsonColombia> colombia;
        private ImageSource _imagen;
        private MediaFile _ImagenArchivo;
        private Ciudades _ciudad;
        private Candidato _candidato;
        public object listViewSource;
        private ObservableCollection<string> _tiposDeCategoria;
        private ObservableCollection<string> _departamentosLista;
        private ObservableCollection<Ciudades> _ciudadesLista;
        #endregion

        #region Constructor
        public PerfilCandidatoViewModel()
        {
            _firebaseHelper = new FirebaseHelper();
            _firebaseStorage = new FirebaseStorageHelper();
            LoadDepartamentos();
            LoadTiposCategoria();
            _candidato = new Candidato();
            _ciudad = new Ciudades();
            AddValidationRules();
            Imagen = App.Current.Resources["IconDefault"].ToString();
            this.cambiarImagenCommand = new Command(this.cambiarImagenAsync);
            InsertCommand = new Command(AgregarCandidatoOnclicked);
        }
        #endregion

        #region Commands
        public Command InsertCommand { get; }
        public Command cambiarImagenCommand { get; set; }
        #endregion


        #region Properties

        public ImageSource Imagen
        {
            get => _imagen;
            set => SetProperty(ref _imagen, value);
        }

        public ObservableCollection<string> TiposDeCategoria
        {
            get { return _tiposDeCategoria; }
            set { SetProperty(ref _tiposDeCategoria, value); }
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
        public object ListViewSource
        {

            get { return this.listViewSource; }
            set
            {
                SetProperty(ref this.listViewSource, value);
            }
        }
        #endregion

        #region Methods
        private async void cambiarImagenAsync()
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
        private void LoadTiposCategoria()
        {
            List<string> lista = new List<string>
            {

                "Ingeniero de sistemas",
                "administrador de empresas",
                "Contador"
            };
            _tiposDeCategoria = new ObservableCollection<string>(lista);
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
                JsonColombia ciudadescolombia = colombia.Where(x => x.Departamento.Equals(departamento)).FirstOrDefault();

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

        private async void AgregarCandidatoOnclicked()
        {
            if (!string.IsNullOrWhiteSpace(_ciudad.Nombre))
            {
                _candidato.Ciudad.Value = _ciudad.Nombre;
            }
            if (validarFormulario())
            {
                UserDialogs.Instance.ShowLoading("Cargando...");
                if (_ImagenArchivo != null)
                {
                    Candidato.UsuarioId = Guid.NewGuid();
                    string nombreImagen = $"{Candidato.UsuarioId}.{Path.GetExtension(_ImagenArchivo.Path)}";
                    await _firebaseStorage.UploadFile(_ImagenArchivo.GetStream(), nombreImagen, "imagenesdeperfil");
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
                    Password = Candidato.Password.Item1.Value

                };
                await _firebaseHelper.CrearAsync<ECandidato>(entidad, "Candidatos");
                UserDialogs.Instance.HideLoading();
            }
        }

        #endregion
    }
}