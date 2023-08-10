using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace JsonReader.App.Commands
{
    public class AsyncCommand : IAsyncCommand
    {
        private static readonly Func<bool> DefaultCanExecute = () => true;

        private readonly Func<Task> _execute;

        private readonly Func<bool> _canExecute;

        private volatile bool _isExecuting;

        public AsyncCommand(Func<Task> execute, Func<bool> canExecute = null)
        {
            if (execute == null)
                throw new ArgumentNullException(nameof(execute));

            _execute = execute;
            _canExecute = canExecute ?? DefaultCanExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public bool CanExecute() => !_isExecuting && _canExecute.Invoke();

        public async Task ExecuteAsync()
        {
            if (CanExecute())
            {
                try
                {
                    _isExecuting = true;
                    await _execute();
                }
                finally
                {
                    _isExecuting = false;
                }
            }

            RaiseCanExecuteChanged();
        }

        public void RaiseCanExecuteChanged()
        {
            // NOTE: Raises all command refreshing
            CommandManager.InvalidateRequerySuggested();
        }

        #region Explicit implementations

        bool ICommand.CanExecute(object parameter) => CanExecute();

        async void ICommand.Execute(object parameter)
        {
            await ExecuteAsync();
        }

        #endregion Explicit implementations
    }
}
