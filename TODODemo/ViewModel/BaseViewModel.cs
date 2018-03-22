using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TODODemo.ViewModel
{
    public class BaseViewModel:INotifyPropertyChanged
    {
        public BaseViewModel()
        {
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string name = null)
        {
            var changed = PropertyChanged;

            changed?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
