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

        private Candidato _candidato;
        public bool isRefreshing = false;
        public object listViewSource;
        private ObservableCollection<string> _tiposDeCategoria;
        private ObservableCollection<string> _departamentosLista;
        private ObservableCollection<string> _ciudadesLista;
        #endregion


        #region Constructor
        public PersonViewModels()
        {
            LoadTiposCategoria();
            LoadDepartamentos();
            _candidato = new Candidato();
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
           
            List<string> lista = new List<string>();
            var colombia = await new JsonColombia().DeserializarJsonColombia();
            colombia.ForEach(x => lista.Add(x.Departamento));
            _departamentosLista = new ObservableCollection<string>(lista);
            LoadCiudades(colombia);
        }



        private void LoadCiudades(List<JsonColombia> colombia)
        {
            if (Candidato != null)
            {
                JsonColombia ciudadescolombia = colombia.Where(x => x.Departamento.Equals(Candidato.Departamento)).FirstOrDefault();
                List<string> ciudades = new List<string>();

                foreach (var item in ciudadescolombia.Ciudades)
                {
                    ciudades.Add(item);
                }
                _ciudadesLista = new ObservableCollection<string>(ciudades);
            }     
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


        public ObservableCollection<string> CiudadesLista
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
            get { return _candidato; }
            set { SetValue(ref _candidato, value); }
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


