using System.Threading.Tasks;
using System.Windows.Input;

namespace JsonReader.App.Commands
{
    public interface IAsyncCommand : ICommand
    {
        Task ExecuteAsync();

        bool CanExecute();
    }
}
