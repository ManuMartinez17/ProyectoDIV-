using ProyectoDIV1.Models;
using ProyectoDIV1.Services;
using ProyectoDIV1.Validators.Rules;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace ProyectoDIV1.ViewModels
{
    public class PerfilCandidatoViewModel : BaseViewModel
    {
        #region Attributes
        private FirebaseHelper _firebaseHelper;

        private List<JsonColombia> colombia;
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
            LoadDepartamentos();
            LoadTiposCategoria();
            _candidato = new Candidato();
            _ciudad = new Ciudades();
            AddValidationRules();
            InsertCommand = new Command(AgregarCandidatoOnclicked);
        }
        #endregion

        #region Commands
        public Command InsertCommand { get; }

        #endregion


        #region Properties

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

        public void AddValidationRules()
        {
            Candidato.Nombre.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "First Name Required" });
            Candidato.Apellido.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Last Name Required" });
            Candidato.Departamento.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Departamento requerido" });
            Candidato.Ciudad.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Ciudad requerida" });
            Candidato.FechaDeNacimiento.Validations.Add(new HasValidAgeRule<DateTime> { ValidationMessage = "You must be 18 years of age or older" });
            Candidato.Celular.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Phone Number Required" });
            Candidato.Celular.Validations.Add(new IsLenghtValidRule<string> { ValidationMessage = "El celular debe tener 10 digitos", MaximunLenght = 10, MinimunLenght = 10 });

            //Email Validation Rules
            Candidato.Email.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Email Required" });

            //Password Validation Rules
            Candidato.Password.Item1.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Password Required" });
            Candidato.Password.Item1.Validations.Add(new IsValidPasswordRule<string> { ValidationMessage = "Password between 8-20 characters; must contain at least one lowercase letter, one uppercase letter, one numeric digit, and one special character" });
            Candidato.Password.Item2.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Confirm password required" });
            Candidato.Password.Validations.Add(new MatchPairValidationRule<string> { ValidationMessage = "Password and confirm password don't match" });

        }

        bool AreFieldsValid()
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
           
           _candidato.Ciudad.Value = _ciudad.Nombre;
            if (AreFieldsValid())
            {
                await _firebaseHelper.AddPerson(Candidato);
            }
        }

        #endregion
    }
}