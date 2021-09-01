using ProyectoDIV1.ViewModels;
using ProyectoDIV1.ViewModels.Empresa;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProyectoDIV1.Views.Empresa
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