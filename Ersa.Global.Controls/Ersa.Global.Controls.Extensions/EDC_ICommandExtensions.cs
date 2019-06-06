using System.Windows;
using System.Windows.Input;

namespace Ersa.Global.Controls.Extensions
{
	public static class EDC_ICommandExtensions
	{
		public static void SUB_Execute(this ICommand i_cmdCommand, object i_objCommandParameter, IInputElement i_fdcCommandTarget = null)
		{
			if (i_cmdCommand != null)
			{
				RoutedCommand routedCommand = i_cmdCommand as RoutedCommand;
				if (routedCommand != null)
				{
					SUB_RoutedCommandExecute(routedCommand, i_objCommandParameter, i_fdcCommandTarget);
				}
				else
				{
					SUB_CommandExecute(i_cmdCommand, i_objCommandParameter);
				}
			}
		}

		private static void SUB_RoutedCommandExecute(RoutedCommand i_cmdCommand, object i_objCommandParameter, IInputElement i_fdcCommandTarget)
		{
			if (i_cmdCommand.CanExecute(i_objCommandParameter, i_fdcCommandTarget))
			{
				i_cmdCommand.Execute(i_objCommandParameter, i_fdcCommandTarget);
			}
		}

		private static void SUB_CommandExecute(ICommand i_cmdCommand, object i_objCommandParameter)
		{
			if (i_cmdCommand.CanExecute(i_objCommandParameter))
			{
				i_cmdCommand.Execute(i_objCommandParameter);
			}
		}
	}
}
