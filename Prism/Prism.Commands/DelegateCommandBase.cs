using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Threading;
using System.Windows.Input;

namespace Prism.Commands
{
	public abstract class DelegateCommandBase : ICommand, IActiveAware
	{
		private bool _isActive;

		private SynchronizationContext _synchronizationContext;

		private readonly HashSet<string> _propertiesToObserve = new HashSet<string>();

		private INotifyPropertyChanged _inpc;

		public bool IsActive
		{
			get
			{
				return _isActive;
			}
			set
			{
				if (_isActive != value)
				{
					_isActive = value;
					OnIsActiveChanged();
				}
			}
		}

		public virtual event EventHandler CanExecuteChanged;

		public virtual event EventHandler IsActiveChanged;

		protected DelegateCommandBase()
		{
			_synchronizationContext = SynchronizationContext.Current;
		}

		protected virtual void OnCanExecuteChanged()
		{
			EventHandler handler = this.CanExecuteChanged;
			if (handler != null)
			{
				if (_synchronizationContext != null && _synchronizationContext != SynchronizationContext.Current)
				{
					_synchronizationContext.Post(delegate
					{
						handler(this, EventArgs.Empty);
					}, null);
				}
				else
				{
					handler(this, EventArgs.Empty);
				}
			}
		}

		public void RaiseCanExecuteChanged()
		{
			OnCanExecuteChanged();
		}

		void ICommand.Execute(object parameter)
		{
			Execute(parameter);
		}

		bool ICommand.CanExecute(object parameter)
		{
			return CanExecute(parameter);
		}

		protected abstract void Execute(object parameter);

		protected abstract bool CanExecute(object parameter);

		protected internal void ObservesPropertyInternal<T>(Expression<Func<T>> propertyExpression)
		{
			AddPropertyToObserve(PropertySupport.ExtractPropertyName(propertyExpression));
			HookInpc(propertyExpression.Body as MemberExpression);
		}

		protected void HookInpc(MemberExpression expression)
		{
			if (expression == null || _inpc != null)
			{
				return;
			}
			ConstantExpression constantExpression = expression.Expression as ConstantExpression;
			if (constantExpression != null)
			{
				_inpc = (constantExpression.Value as INotifyPropertyChanged);
				if (_inpc != null)
				{
					_inpc.PropertyChanged += Inpc_PropertyChanged;
				}
			}
		}

		protected void AddPropertyToObserve(string property)
		{
			if (_propertiesToObserve.Contains(property))
			{
				throw new ArgumentException($"{property} is already being observed.");
			}
			_propertiesToObserve.Add(property);
		}

		private void Inpc_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (_propertiesToObserve.Contains(e.PropertyName) || (string.IsNullOrEmpty(e.PropertyName) && _propertiesToObserve.Count > 0))
			{
				RaiseCanExecuteChanged();
			}
		}

		protected virtual void OnIsActiveChanged()
		{
			this.IsActiveChanged?.Invoke(this, EventArgs.Empty);
		}
	}
}
