using ProyectoDIV1.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProyectoDIV1.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EmpresasConServiciosPage : ContentPage
    {
        public EmpresasConServiciosPage()
        {
            InitializeComponent();
            BindingContext = new EmpresasConServiciosViewModel();
        }
    }
}