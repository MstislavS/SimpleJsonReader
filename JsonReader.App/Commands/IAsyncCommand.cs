using System.Threading.Tasks;
using System.Windows.Input;

namespace JsonReader.App.Commands
{
    public interface IAsyncCommand : ICommand
    {
        bool CanExecute();

        Task ExecuteAsync();
    }
}