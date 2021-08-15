using ProyectoDIV1.Entidades.Models;
using ProyectoDIV1.ViewModels;
using Syncfusion.XForms.Buttons;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProyectoDIV1.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditarHojaDeVidaPage : ContentPage
    {
        public EditarHojaDeVidaPage()
        {
            InitializeComponent();
            BindingContext = new EditarHojaDeVidaViewModel();
        }

        private void chipsHabilidades_ItemRemoved(object sender, Syncfusion.Buttons.XForms.SfChip.ItemRemovedEventArgs e)
        {
            var vm = BindingContext as EditarHojaDeVidaViewModel;
            var item = e.RemovedItem as SfButton;
            if (!string.IsNullOrEmpty(item.Text))
            {
                vm?.BorrarHabilidadCommand.Execute(item.Text);
            }

        }
    }
}