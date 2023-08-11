using JsonReader.App.Commands;
using JsonReader.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;

namespace JsonReader.App.ViewModels
{
    public abstract class ViewModelBase : DisposableBase, INotifyPropertyChanged
    {
        protected ViewModelBase()
        {
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler? handler = PropertyChanged;

            if (handler is null)
            {
                return;
            }

            handler(this, new PropertyChangedEventArgs(propertyName));
        }

        protected ICommand CreateAsyncCommand(Action executeFunc, Func<bool>? canExecute = null)
        {
            return new AsyncCommand(async () => await Task.Run(() => executeFunc()), canExecute);
        }

        protected bool SetProperty<T>(ref T location,
                                      T value,
                                      [CallerMemberName] string? propertyName = null,
                                      Action<T, T>? onChanged = null,
                                      bool alwaysSet = false)
        {
            if (!alwaysSet && EqualityComparer<T>.Default.Equals(location, value))
            {
                return false;
            }

            T? oldValue = location;
            location = value;

            onChanged?.Invoke(oldValue, location);

            OnPropertyChanged(propertyName);

            return true;
        }
    }
}