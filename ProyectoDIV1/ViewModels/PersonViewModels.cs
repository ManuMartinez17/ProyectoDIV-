using ProyectoDIV1.Models;
using ProyectoDIV1.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ProyectoDIV1.ViewModels
{
    class PersonViewModels : BaseViewModel
    {
        FirebaseHelper firebaseHelper = new FirebaseHelper();

        #region Attributes
        private List<JsonColombia> colombia;
        private Ciudades _ciudad;
        private Candidato _candidato;
        public bool isRefreshing = false;
        public object listViewSource;
        private ObservableCollection<string> _tiposDeCategoria;
        private ObservableCollection<string> _departamentosLista;
        private ObservableCollection<Ciudades> _ciudadesLista;
        #endregion


        #region Constructor
        public PersonViewModels()
        {
            LoadDepartamentos();
            LoadTiposCategoria();
            _candidato = new Candidato();
            _ciudad = new Ciudades();
            InsertCommand = new Command(InsertMethod);
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
        #endregion

        #region Commands
        public Command InsertCommand { get; }

        #endregion

        #region Properties

        public ObservableCollection<string> TiposDeCategoria
        {
            get { return _tiposDeCategoria; }
            set { SetValue(ref _tiposDeCategoria, value); }
        }


        public ObservableCollection<Ciudades> CiudadesLista
        {
            get { return _ciudadesLista; }
            set { SetValue(ref _ciudadesLista, value); }
        }


        public ObservableCollection<string> DepartamentosLista
        {
            get { return _departamentosLista; }
            set { SetValue(ref _departamentosLista, value); }
        }
        public Candidato Candidato
        {
            get
            {
                if (!string.IsNullOrEmpty(_candidato.Departamento))
                {
                    CiudadesLista = new ObservableCollection<Ciudades>(LoadCiudades(_candidato.Departamento));
                }
                return _candidato;
            }

            set
            {
                SetValue(ref _candidato, value);
            }
        }

        public Ciudades Ciudad
        {
            get { return _ciudad; }
            set { SetValue(ref _ciudad, value); }
        }

        public bool IsRefreshing
        {
            get { return isRefreshing; }
            set { SetValue(ref this.isRefreshing, value); }
        }

        public object ListViewSource
        {

            get { return this.listViewSource; }
            set
            {
                SetValue(ref this.listViewSource, value);
            }
        }
        #endregion

        #region Methods
        private async void InsertMethod()
        {
            var candidato = Candidato;
            candidato.Ciudad = Ciudad.Nombre;
            await firebaseHelper.AddPerson(Candidato);

            this.IsRefreshing = true;

            await Task.Delay(1000);

            await LoadData();

            this.IsRefreshing = false;
        }

        public async Task LoadData()
        {
            this.ListViewSource = await firebaseHelper.GetCandidatos();
        }
        #endregion

        #region
        public ObservableCollection<Candidato> IngredientsCollection = new ObservableCollection<Candidato>();

        private async Task TestListViewBindingAsync()
        {
            var Ingredients = new List<Candidato>();

            {
                Ingredients = await firebaseHelper.GetCandidatos();
            }
            foreach (var Ingredient in Ingredients)
            {
                IngredientsCollection.Add(Ingredient);
            }

        }
        #endregion


    }
}


