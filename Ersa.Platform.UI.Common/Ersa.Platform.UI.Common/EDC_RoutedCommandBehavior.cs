using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace Ersa.Platform.UI.Common
{
	public class EDC_RoutedCommandBehavior : Behavior<FrameworkElement>, ICommandSource
	{
		public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register("CommandParameter", typeof(object), typeof(EDC_RoutedCommandBehavior));

		public static readonly DependencyProperty CommandProperty = DependencyProperty.Register("Command", typeof(ICommand), typeof(EDC_RoutedCommandBehavior));

		public static readonly DependencyProperty CommandTargetProperty = DependencyProperty.Register("CommandTarget", typeof(IInputElement), typeof(EDC_RoutedCommandBehavior));

		public static readonly DependencyProperty PRO_cmdRoutedCommandProperty = DependencyProperty.Register("PRO_cmdRoutedCommand", typeof(RoutedCommand), typeof(EDC_RoutedCommandBehavior));

		private CommandBinding commandBinding;

		public ICommand Command
		{
			get
			{
				return (ICommand)GetValue(CommandProperty);
			}
			set
			{
				SetValue(CommandProperty, value);
			}
		}

		public object CommandParameter
		{
			get
			{
				return GetValue(CommandParameterProperty);
			}
			set
			{
				SetValue(CommandParameterProperty, value);
			}
		}

		public IInputElement CommandTarget
		{
			get
			{
				return (IInputElement)GetValue(CommandTargetProperty);
			}
			set
			{
				SetValue(CommandTargetProperty, value);
			}
		}

		public RoutedCommand PRO_cmdRoutedCommand
		{
			get
			{
				return (RoutedCommand)GetValue(PRO_cmdRoutedCommandProperty);
			}
			set
			{
				SetValue(PRO_cmdRoutedCommandProperty, value);
			}
		}

		protected override void OnAttached()
		{
			if (base.AssociatedObject != null && !base.AssociatedObject.CommandBindings.Contains(commandBinding))
			{
				commandBinding = new CommandBinding(PRO_cmdRoutedCommand, SUB_HandleExecuted, SUB_HandleCanExecute);
				base.AssociatedObject.CommandBindings.Add(commandBinding);
			}
		}

		protected override void OnDetaching()
		{
			if (base.AssociatedObject != null && commandBinding != null && base.AssociatedObject.CommandBindings.Contains(commandBinding))
			{
				base.AssociatedObject.CommandBindings.Remove(commandBinding);
			}
			commandBinding = null;
		}

		private void SUB_HandleCanExecute(object i_objSender, CanExecuteRoutedEventArgs i_fdcArgs)
		{
			if (Command == null)
			{
				i_fdcArgs.CanExecute = false;
				return;
			}
			object parameter = CommandParameter ?? i_fdcArgs.Parameter;
			RoutedCommand routedCommand = Command as RoutedCommand;
			if (routedCommand != null)
			{
				i_fdcArgs.CanExecute = routedCommand.CanExecute(parameter, CommandTarget);
			}
			i_fdcArgs.CanExecute = Command.CanExecute(parameter);
		}

		private void SUB_HandleExecuted(object i_objSender, ExecutedRoutedEventArgs i_fdcArgs)
		{
			if (Command != null)
			{
				object parameter = CommandParameter ?? i_fdcArgs.Parameter;
				(Command as RoutedCommand)?.Execute(parameter, CommandTarget);
				Command.Execute(parameter);
				i_fdcArgs.Handled = true;
			}
		}
	}
}
