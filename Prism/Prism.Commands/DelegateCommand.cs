using Prism.Properties;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Prism.Commands
{
	public class DelegateCommand : DelegateCommandBase
	{
		private Action _executeMethod;

		private Func<bool> _canExecuteMethod;

		public DelegateCommand(Action executeMethod)
			: this(executeMethod, () => true)
		{
		}

		public DelegateCommand(Action executeMethod, Func<bool> canExecuteMethod)
		{
			if (executeMethod == null || canExecuteMethod == null)
			{
				throw new ArgumentNullException("executeMethod", Resources.DelegateCommandDelegatesCannotBeNull);
			}
			_executeMethod = executeMethod;
			_canExecuteMethod = canExecuteMethod;
		}

		public void Execute()
		{
			_executeMethod();
		}

		public bool CanExecute()
		{
			return _canExecuteMethod();
		}

		protected override void Execute(object parameter)
		{
			Execute();
		}

		protected override bool CanExecute(object parameter)
		{
			return CanExecute();
		}

		public DelegateCommand ObservesProperty<T>(Expression<Func<T>> propertyExpression)
		{
			ObservesPropertyInternal(propertyExpression);
			return this;
		}

		public DelegateCommand ObservesCanExecute(Expression<Func<bool>> canExecuteExpression)
		{
			_canExecuteMethod = canExecuteExpression.Compile();
			ObservesPropertyInternal(canExecuteExpression);
			return this;
		}
	}
	public class DelegateCommand<T> : DelegateCommandBase
	{
		private readonly Action<T> _executeMethod;

		private Func<T, bool> _canExecuteMethod;

		public DelegateCommand(Action<T> executeMethod)
			: this(executeMethod, (Func<T, bool>)((T o) => true))
		{
		}

		public DelegateCommand(Action<T> executeMethod, Func<T, bool> canExecuteMethod)
		{
			if (executeMethod == null || canExecuteMethod == null)
			{
				throw new ArgumentNullException("executeMethod", Resources.DelegateCommandDelegatesCannotBeNull);
			}
			TypeInfo typeInfo = typeof(T).GetTypeInfo();
			if (typeInfo.get_IsValueType() && (!typeInfo.get_IsGenericType() || !typeof(Nullable<>).GetTypeInfo().IsAssignableFrom(typeInfo.GetGenericTypeDefinition().GetTypeInfo())))
			{
				throw new InvalidCastException(Resources.DelegateCommandInvalidGenericPayloadType);
			}
			_executeMethod = executeMethod;
			_canExecuteMethod = canExecuteMethod;
		}

		public void Execute(T parameter)
		{
			_executeMethod(parameter);
		}

		public bool CanExecute(T parameter)
		{
			return _canExecuteMethod(parameter);
		}

		protected override void Execute(object parameter)
		{
			Execute((T)parameter);
		}

		protected override bool CanExecute(object parameter)
		{
			return CanExecute((T)parameter);
		}

		public DelegateCommand<T> ObservesProperty<TType>(Expression<Func<TType>> propertyExpression)
		{
			ObservesPropertyInternal(propertyExpression);
			return this;
		}

		public DelegateCommand<T> ObservesCanExecute(Expression<Func<bool>> canExecuteExpression)
		{
			Expression<Func<T, bool>> expression = Expression.Lambda<Func<T, bool>>(canExecuteExpression.Body, new ParameterExpression[1]
			{
				Expression.Parameter(typeof(T), "o")
			});
			_canExecuteMethod = expression.Compile();
			ObservesPropertyInternal(canExecuteExpression);
			return this;
		}
	}
}
