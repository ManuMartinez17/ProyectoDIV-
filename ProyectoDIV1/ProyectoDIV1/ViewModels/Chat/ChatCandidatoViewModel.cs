using Newtonsoft.Json;
using ProyectoDIV1.Entidades.Models;
using ProyectoDIV1.Helpers;
using ProyectoDIV1.Services.FirebaseServices;
using Syncfusion.XForms.Chat;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;

namespace ProyectoDIV1.ViewModels.Chat
{
    [QueryProperty(nameof(IdReceptor), nameof(IdReceptor))]
    public class ChatCandidatoViewModel : BaseViewModel
    {
        private string _idReceptor;
        private ObservableCollection<object> _messages;
        private Author currentUserEmisor;
        private ECandidato _candidatoReceptor;
        private CandidatoService _candidatoService;
        private ECandidato _candidatoEmisor;
        public Command<object> tappedCommand;
        public ChatCandidatoViewModel()
        {
            _candidatoService = new CandidatoService();
            _messages = new ObservableCollection<object>();
            _candidatoReceptor = new ECandidato();
            _candidatoEmisor = JsonConvert.DeserializeObject<ECandidato>(Settings.Usuario);
            TappedCommand = new Command<object>(MessageTapped);
            currentUserEmisor = new Author() { Name = $"{_candidatoEmisor.Nombre} {_candidatoEmisor.Apellido}", Avatar = _candidatoEmisor.Rutas.RutaImagenRegistro };
            GenerateMessages();
        }

        public ObservableCollection<object> Messages
        {
            get { return _messages; }
            set { SetProperty(ref _messages, value); }
        }
        public Author CurrentUserEmisor
        {
            get { return currentUserEmisor; }
            set { SetProperty(ref currentUserEmisor, value); }
        }

        public Command<object> TappedCommand
        {
            get { return tappedCommand; }
            set { SetProperty(ref tappedCommand, value); }
        }


        public ECandidato CandidatoEmisor
        {
            get { return _candidatoEmisor; }
            set { SetProperty(ref _candidatoEmisor, value); }
        }
        public ECandidato CandidatoReceptor
        {
            get { return _candidatoReceptor; }
            set { SetProperty(ref _candidatoReceptor, value); }
        }
        public string IdReceptor
        {
            get
            {
                return _idReceptor;
            }
            set
            {
                _idReceptor = value;

                LoadCandidatoReceptor(value);
            }
        }

        private async void LoadCandidatoReceptor(string value)
        {
            var id = new Guid(value);
            var candidato = await _candidatoService.GetCandidatoAsync(id);
            if (candidato == null)
            {
                return;
            }
            CandidatoReceptor = candidato;
        }

        private void GenerateMessages()
        {
            Messages.Add(new TextMessage()
            {
                Author = CurrentUserEmisor,
                Text = "Buen día para solicitar de sus servicios.",
            });
        }
        private void MessageTapped(object args)
        {
            var MessageTappedArgs = args as MessageTappedEventArgs;
            App.Current.MainPage.DisplayAlert("Message", "Tapped on Message :" + MessageTappedArgs.Message.Author.Name, "Ok");
        }
    }
}
