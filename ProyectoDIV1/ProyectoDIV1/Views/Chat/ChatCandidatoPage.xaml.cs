using ProyectoDIV1.ViewModels.Chat;
using Syncfusion.XForms.Chat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProyectoDIV1.Views.Chat
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChatCandidatoPage : ContentPage
    {
        private ChatCandidatoViewModel _viewModel;
        public ChatCandidatoPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new ChatCandidatoViewModel();
        }

        private void sfChat_AttachmentButtonClicked(object sender, EventArgs e)
        {

        }

        private void sfChat_SendMessage(object sender, Syncfusion.XForms.Chat.SendMessageEventArgs e)
        {
            if (e.Message.Text == "hola mundo")
            {
                // Handling the message from editor, won&apos;t add it to the chat control.
                e.Handled = true;

                //Message created from the editor is added to the chat control. 
                sfChat.Messages.Add(e.Message);

                //Creating and adding response message to the chat control.
                var message = new TextMessage();
                message.Text = e.Message.Text + e.Message.Author.Name;
                sfChat.Messages.Add(message);
            }
        }
    }
}