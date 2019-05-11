using System.Threading.Tasks;
using System.Windows.Input;

namespace Ersa.Global.Mvvm.Commands
{
	public interface IAsyncCommand : ICommand
	{
		Task ExecuteAsync(object parameter);
	}
}
