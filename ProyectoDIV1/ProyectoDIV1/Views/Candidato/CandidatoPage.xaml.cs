using ProyectoDIV1.ViewModels.Candidato;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProyectoDIV1.Views.Candidato
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CandidatoPage : ContentPage
    {
        public CandidatoPage()
        {
            InitializeComponent();
            BindingContext = new CandidatoViewModel();
        }
    }
}