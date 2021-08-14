using ProyectoDIV1.Entidades.Models;
using ProyectoDIV1.ViewModels;
using Syncfusion.XForms.Buttons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProyectoDIV1.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PerfilTrabajoPage : ContentPage
    {
        public PerfilTrabajoPage()
        {
            InitializeComponent();
            BindingContext = new PerfilTrabajoViewModel();
        }

        private void BorrarSkill_Clicked(object sender, EventArgs e)
        {
            var button = sender as SfButton;
            var skill = button?.BindingContext as Lista;
            var vm = BindingContext as PerfilTrabajoViewModel;
            vm?.BorrarHabilidadCommand.Execute(skill);
        }
    }
}