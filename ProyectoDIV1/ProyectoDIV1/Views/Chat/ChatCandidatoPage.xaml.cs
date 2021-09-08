using ProyectoDIV1.ViewModels.Chat;
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
    }
}