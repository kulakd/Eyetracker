using Android.App;
using GuiAndroid.Model;
using GuiAndroid.ViewModel;
using MVVMKit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MVVMKit
{
    public abstract class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(params string[] propertyName)
        {
            if (PropertyChanged != null)
                foreach (string property in propertyName)
                    PropertyChanged(this, new PropertyChangedEventArgs(property));
        }
    }
}