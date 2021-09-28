using ProyectoDIV1.Helpers;
using ProyectoDIV1.Services.Interfaces;
using ProyectoDIV1.Views.Account;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Xamarin.Forms;

namespace ProyectoDIV1.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {

        string title = string.Empty;
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }
        bool isBusy = false;
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }
        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName] string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        public Command OnSignOutCommand
        {
            get
            {
                return new Command(async () =>
                {
                    await PopupNavigation.Instance.PushAsync(new PopupSignOutPage());

                });
            }
        }

        public Command BackCommand
        {
            get
            {
                return new Command(async () =>
                {
                    await Shell.Current.GoToAsync("..");

                });
            }
        }

        public string ParsearUrlConCodigoPorciento(string palabra)
        {
            string specialChar = @"\|!#$%&/()=?»«@£§€{}.-;'<>_,+";
            StringBuilder palabraTraducida = new StringBuilder();
            foreach (var item in palabra)
            {
                if (specialChar.Contains(item))
                {
                    var item2 = HttpUtility.UrlEncode(item.ToString());
                    palabraTraducida.Append(item2);
                    palabraTraducida = palabraTraducida.Replace(" ", string.Empty);
                }
                else
                {
                    palabraTraducida.Append(item);
                }

            }
            return palabraTraducida.ToString();
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
