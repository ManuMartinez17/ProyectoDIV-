using GalaSoft.MvvmLight.Command;
using ProyectoDIV1.Models;
using ProyectoDIV1.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ProyectoDIV1.ViewModels
{
    class PersonViewModels : BaseViewModel
    {
        FirebaseHelper firebaseHelper = new FirebaseHelper();

        #region Attributes

        public string nombre;
        public string apellido;
        private string email;
        private string ciudad;
        private string celular;
        private int edad;
        private string foto;
        private string curriculum;
        public bool isRefreshing = false;
        public object listViewSource;
        #endregion


        #region Properties
        public string Nombre
        {
            get { return this.Nombre; }
            set { SetValue(ref this.nombre, value); }
        }

        public string Apellido
        {
            get { return this.apellido; }
            set { SetValue(ref this.apellido, value); }
        }

        public string Email
        {
            get { return this.email; }
            set { SetValue(ref this.email, value); }
        }

        public string Ciudad
        {
            get { return this.ciudad; }
            set { SetValue(ref this.ciudad, value); }
        }

        public string Celular
        {
            get { return this.celular; }
            set { SetValue(ref this.celular, value); }
        }

        public int Edad
        {
            get { return this.edad; }
            set { SetValue(ref this.edad, value); }
        }

        public string Foto
        {
            get { return this.foto; }
            set { SetValue(ref this.foto, value); }
        }

        public string Curriculum
        {
            get { return this.curriculum; }
            set { SetValue(ref this.curriculum, value); }
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

        #region Commands
        public ICommand InsertCommand
        {
            get
            {
                return new RelayCommand(InsertMethod);
            }
        }
        #endregion
        #region Methods
        private async void InsertMethod()
        {
            var candidatos = new Candidato
            {

                Nombre = nombre,
                Apellido = apellido,
                Email = email,
                Ciudad = ciudad,
                Celular = celular,
                Edad = edad,
                Foto = foto,
                Curriculum = curriculum
            };

            await firebaseHelper.AddPerson(candidatos);

            this.IsRefreshing = true;

            await Task.Delay(1000);

            LoadData();

            this.IsRefreshing = false;
        }

        public async Task LoadData()
        {
            this.ListViewSource = await firebaseHelper.GetCandidatos();
        }
        #endregion

        #region .
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
        #region Constructor
        public PersonViewModels()
        {
            LoadData();
            TestListViewBindingAsync();
        }
        #endregion
    }
}
    

