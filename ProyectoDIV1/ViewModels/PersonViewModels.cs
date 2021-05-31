using ProyectoDIV1.Models;
using ProyectoDIV1.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        #endregion


        #region Constructor
        public PersonViewModels()
        {
            LoadTiposCategoria();
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


