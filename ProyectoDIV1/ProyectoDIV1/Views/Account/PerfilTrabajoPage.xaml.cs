using ProyectoDIV1.Entidades.Models;
using ProyectoDIV1.ViewModels.Account;
using Syncfusion.XForms.Buttons;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProyectoDIV1.Views.Account
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PerfilTrabajoPage : ContentPage
    {
        PerfilTrabajoViewModel _viewModel;
        public PerfilTrabajoPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new PerfilTrabajoViewModel();
        }

        private void BorrarSkill_Clicked(object sender, EventArgs e)
        {
            var button = sender as SfButton;
            var skill = button?.BindingContext as Lista;
            _viewModel?.BorrarHabilidadCommand.Execute(skill);
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}