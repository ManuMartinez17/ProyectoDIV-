using ProyectoDIV1.ViewModels.Candidato;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProyectoDIV1.Views.Candidato
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VerHojaDeVidaPage : ContentPage
    {
        public VerHojaDeVidaPage()
        {
            InitializeComponent();
            BindingContext = new VerHojaDeVidaViewModel();
        }
    }
}