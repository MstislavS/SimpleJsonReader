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
    /// <summary>
    /// Represents a view model abstract class
    /// </summary>
    public abstract class ViewModelBase : DisposableBase, INotifyPropertyChanged
    {


        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModelBase"/> class.
        /// </summary>
        protected ViewModelBase()
        {
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;

            if (handler is null)
                return;

            handler(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Creates an asynchronous command.
        /// </summary>
        /// <param name="executeFunc">The execute action function.</param>
        /// <param name="canExecute">Can execute function.</param>
        /// <returns>The command.</returns>
        protected ICommand CreateAsyncCommand(Action executeFunc, Func<bool> canExecute = null)
        {
            return new AsyncCommand(async () => await Task.Run(() => executeFunc), canExecute);
        }

        protected bool SetProperty<T>(ref T location,
                                      T value,
                                      [CallerMemberName] string propertyName = null,
                                      Action<T, T> onChanged = null,
                                      bool alwaysSet = false)
        {
            if (!alwaysSet && EqualityComparer<T>.Default.Equals(location, value))
                return false;

            var oldValue = location;
            location = value;

            onChanged?.Invoke(oldValue, location);

            OnPropertyChanged(propertyName);

            return true;
        }

    }
}
