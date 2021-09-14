using Newtonsoft.Json;
using ProyectoDIV1.Entidades.Models;
using ProyectoDIV1.Helpers;
using ProyectoDIV1.Services.FirebaseServices;
using ProyectoDIV1.Services.Helpers;
using ProyectoDIV1.Validators;
using ProyectoDIV1.Validators.Rules;
using ProyectoDIV1.Views.Buscadores;
using Rg.Plugins.Popup.Services;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ProyectoDIV1.ViewModels.Candidato
{

    public class EditarDatosViewModel : BaseViewModel
    {
        private string _id;
        private CandidatoService _candidatoService;
        private ECandidato _candidato;
        private FirebaseHelper _firebase;
        private ValidatableObject<string> _nombre = new ValidatableObject<string>();
        private ValidatableObject<string> _apellido = new ValidatableObject<string>();

        public void OnAppearing()
        {
            LoadCandidato();
        }

        private void LoadCandidato()
        {
            Candidato = JsonConvert.DeserializeObject<ECandidato>(Settings.Usuario);
            Profesion.Value = Candidato.Profesion;
        }

        private ValidatableObject<string> _profesion = new ValidatableObject<string>();
        public EditarDatosViewModel()
        {
            _candidato = new ECandidato();
            _candidatoService = new CandidatoService();
            _firebase = new FirebaseHelper();
            EditarCommand = new Command(EditarClicked);
            MostrarBuscadorCommand = new Command(MostrarBuscador);
            AddValidationRules();
        }

        private void AddValidationRules()
        {
            Nombre.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Nombre requerido." });
            Apellido.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Apellido requerido." });
            Profesion.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Profesión requerida." });
        }

        public ValidatableObject<string> Nombre
        {
            get => _nombre;
            set => SetProperty(ref _nombre, value);
        }
        public ValidatableObject<string> Profesion
        {
            get => _profesion;
            set => SetProperty(ref _profesion, value);
        }
        public ValidatableObject<string> Apellido
        {
            get => _apellido;
            set => SetProperty(ref _apellido, value);
        }
        private bool ValidarFormulario()
        {

            bool isFirstNameValid = Nombre.Validate();
            bool isLastNameValid = Apellido.Validate();
            bool isJobValid = Profesion.Validate();
            return isFirstNameValid && isLastNameValid && isJobValid;
        }
        private async void EditarClicked()
        {
            try
            {
                if (ValidarFormulario())
                {
                    _candidato.Nombre = Nombre.Value.Trim();
                    _candidato.Apellido = Apellido.Value.Trim();
                    _candidato.Profesion = Profesion.Value.Trim();
                    var query = await _candidatoService.GetCandidatoFirebaseObjectAsync(_candidato.UsuarioId);
                    await _firebase.UpdateAsync(_candidato, Constantes.COLLECTION_CANDIDATO, query);
                    Toasts.Success("Se actualizo el perfil.", 2000);
                    await Task.Delay(2000);
                    Settings.Usuario = JsonConvert.SerializeObject(Candidato);
                    await Shell.Current.Navigation.PopModalAsync();
                }
            }
            catch (Exception ex)
            {

                Debug.WriteLine(ex.Message);
            }
        }
        private async void MostrarBuscador()
        {
            var candidato = await _candidatoService.GetCandidatoAsync(_candidato.UsuarioId);
            Settings.Candidato = JsonConvert.SerializeObject(candidato);
            await PopupNavigation.Instance.PushAsync(new BuscadorPage(Constantes.SEARCH_JOB));
        }
        public Command EditarCommand { get; }
        public Command MostrarBuscadorCommand { get; }
        public string Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;

                LoadCandidato(value);
            }
        }
        public ECandidato Candidato
        {
            get { return _candidato; }
            set
            {
                SetProperty(ref _candidato, value);
            }
        }
        private async void LoadCandidato(string value)
        {
            var id = new Guid(value);
            Candidato = await _candidatoService.GetCandidatoAsync(id);
            Nombre.Value = Candidato.Nombre;
            Apellido.Value = Candidato.Apellido;
            Profesion.Value = Candidato.Profesion;
        }
    }
}
