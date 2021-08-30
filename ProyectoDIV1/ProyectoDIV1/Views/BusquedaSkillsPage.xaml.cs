using ProyectoDIV1.Entidades.Models;
using ProyectoDIV1.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProyectoDIV1.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BusquedaSkillsPage : ContentPage
    {
        public BusquedaSkillsPage()
        {
            InitializeComponent();
            BindingContext = new BusquedaSkillsViewModel();
        }

        private void SfAutoComplete_SelectionChanged(object sender, Syncfusion.SfAutoComplete.XForms.SelectionChangedEventArgs e)
        {
            var vm = BindingContext as BusquedaSkillsViewModel;
            var addedSkill = e.AddedItems as Skill;
            string addedItems = addedSkill != null ? addedSkill.name : string.Empty;
            var removedSkill = e.RemovedItems as Skill;
            string removedItems = removedSkill != null ? removedSkill.name : string.Empty;

            if (!string.IsNullOrEmpty(addedItems))
            {
                vm?.InsertarCommand.Execute(addedItems);
            }
            else if (!string.IsNullOrEmpty(removedItems))
            {
                vm?.BorrarCommand.Execute(removedItems);
            }
        }
    }
}