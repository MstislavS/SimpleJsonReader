using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace JsonReader.App.Commands
{
    public class AsyncCommand : IAsyncCommand
    {
        private static readonly Func<bool> s_defaultCanExecute = () => true;

        private readonly Func<bool> _canExecute;

        private readonly Func<Task> _execute;

        private volatile bool _isExecuting;

        public AsyncCommand(Func<Task> execute, Func<bool>? canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute ?? s_defaultCanExecute;
        }

        public event EventHandler? CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public bool CanExecute()
        {
            return !_isExecuting && _canExecute.Invoke();
        }

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
            // Raises all command refreshing
            CommandManager.InvalidateRequerySuggested();
        }

        #region Explicit implementations

        bool ICommand.CanExecute(object parameter)
        {
            return CanExecute();
        }

        async void ICommand.Execute(object parameter)
        {
            await ExecuteAsync();
        }

        #endregion Explicit implementations
    }
}