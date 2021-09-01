
using ProyectoDIV1.ViewModels.Buscadores;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProyectoDIV1.Views.Buscadores
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BusquedaJobPage : ContentPage
    {
        public BusquedaJobPage()
        {
            InitializeComponent();
            BindingContext = new BusquedaJobViewModel();
        }
    }
}