using System.Reflection;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace Prism.Interactivity
{
	public class InvokeCommandAction : TriggerAction<UIElement>
	{
		private class ExecutableCommandBehavior : CommandBehaviorBase<UIElement>
		{
			public ExecutableCommandBehavior(UIElement target)
				: base(target)
			{
			}

			public new void ExecuteCommand(object parameter)
			{
				base.ExecuteCommand(parameter);
			}
		}

		private ExecutableCommandBehavior _commandBehavior;

		public static readonly DependencyProperty AutoEnableProperty = DependencyProperty.Register("AutoEnable", typeof(bool), typeof(InvokeCommandAction), new PropertyMetadata(true, delegate(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((InvokeCommandAction)d).OnAllowDisableChanged((bool)e.NewValue);
		}));

		public static readonly DependencyProperty CommandProperty = DependencyProperty.Register("Command", typeof(ICommand), typeof(InvokeCommandAction), new PropertyMetadata(null, delegate(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((InvokeCommandAction)d).OnCommandChanged((ICommand)e.NewValue);
		}));

		public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register("CommandParameter", typeof(object), typeof(InvokeCommandAction), new PropertyMetadata(null, delegate(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((InvokeCommandAction)d).OnCommandParameterChanged(e.NewValue);
		}));

		public static readonly DependencyProperty TriggerParameterPathProperty = DependencyProperty.Register("TriggerParameterPath", typeof(string), typeof(InvokeCommandAction), new PropertyMetadata(null, delegate
		{
		}));

		public bool AutoEnable
		{
			get
			{
				return (bool)GetValue(AutoEnableProperty);
			}
			set
			{
				SetValue(AutoEnableProperty, value);
			}
		}

		public ICommand Command
		{
			get
			{
				return GetValue(CommandProperty) as ICommand;
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

		public string TriggerParameterPath
		{
			get
			{
				return GetValue(TriggerParameterPathProperty) as string;
			}
			set
			{
				SetValue(TriggerParameterPathProperty, value);
			}
		}

		private void OnAllowDisableChanged(bool newValue)
		{
			ExecutableCommandBehavior orCreateBehavior = GetOrCreateBehavior();
			if (orCreateBehavior != null)
			{
				orCreateBehavior.AutoEnable = newValue;
			}
		}

		private void OnCommandChanged(ICommand newValue)
		{
			ExecutableCommandBehavior orCreateBehavior = GetOrCreateBehavior();
			if (orCreateBehavior != null)
			{
				orCreateBehavior.Command = newValue;
			}
		}

		private void OnCommandParameterChanged(object newValue)
		{
			ExecutableCommandBehavior orCreateBehavior = GetOrCreateBehavior();
			if (orCreateBehavior != null)
			{
				orCreateBehavior.CommandParameter = newValue;
			}
		}

		public void InvokeAction(object parameter)
		{
			Invoke(parameter);
		}

		protected override void Invoke(object parameter)
		{
			if (!string.IsNullOrEmpty(TriggerParameterPath))
			{
				string[] array = TriggerParameterPath.Split('.');
				object obj = parameter;
				string[] array2 = array;
				foreach (string name in array2)
				{
					obj = obj.GetType().GetTypeInfo().GetProperty(name)
						.GetValue(obj);
				}
				parameter = obj;
			}
			GetOrCreateBehavior()?.ExecuteCommand(parameter);
		}

		protected override void OnDetaching()
		{
			base.OnDetaching();
			Command = null;
			CommandParameter = null;
			_commandBehavior = null;
		}

		protected override void OnAttached()
		{
			base.OnAttached();
			ExecutableCommandBehavior orCreateBehavior = GetOrCreateBehavior();
			orCreateBehavior.AutoEnable = AutoEnable;
			if (orCreateBehavior.Command != Command)
			{
				orCreateBehavior.Command = Command;
			}
			if (orCreateBehavior.CommandParameter != CommandParameter)
			{
				orCreateBehavior.CommandParameter = CommandParameter;
			}
		}

		private ExecutableCommandBehavior GetOrCreateBehavior()
		{
			if (_commandBehavior == null && base.AssociatedObject != null)
			{
				_commandBehavior = new ExecutableCommandBehavior(base.AssociatedObject);
			}
			return _commandBehavior;
		}
	}
}
