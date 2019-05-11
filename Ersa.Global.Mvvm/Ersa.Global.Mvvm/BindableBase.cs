using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Ersa.Global.Mvvm
{
	[Serializable]
	public abstract class BindableBase : INotifyPropertyChanged
	{
		private static class PropertySupport
		{
			public static string ExtractPropertyName<T>(Expression<Func<T>> propertyExpression)
			{
				if (propertyExpression == null)
				{
					throw new ArgumentNullException("propertyExpression");
				}
				return ExtractPropertyNameFromLambda(propertyExpression);
			}

			private static string ExtractPropertyNameFromLambda(LambdaExpression expression)
			{
				if (expression == null)
				{
					throw new ArgumentNullException("expression");
				}
				MemberExpression obj = expression.Body as MemberExpression;
				if (obj == null)
				{
					throw new ArgumentException("The expression is not a member access expression.", "expression");
				}
				PropertyInfo obj2 = obj.Member as PropertyInfo;
				if (obj2 == null)
				{
					throw new ArgumentException("The member access expression does not access a property.", "expression");
				}
				if (obj2.GetMethod.IsStatic)
				{
					throw new ArgumentException("The referenced property is a static property.", "expression");
				}
				return obj.Member.Name;
			}
		}

		[field: NonSerialized]
		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
		{
			if (object.Equals(storage, value))
			{
				return false;
			}
			storage = value;
			RaisePropertyChanged(propertyName);
			return true;
		}

		protected virtual bool SetProperty<T>(ref T storage, T value, Action onChanged, [CallerMemberName] string propertyName = null)
		{
			if (object.Equals(storage, value))
			{
				return false;
			}
			storage = value;
			onChanged?.Invoke();
			RaisePropertyChanged(propertyName);
			return true;
		}

		protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
		{
			OnPropertyChanged(propertyName);
		}

		[Obsolete("Please use the new RaisePropertyChanged method. This method will be removed to comply wth .NET coding standards. If you are overriding this method, you should overide the OnPropertyChanged(PropertyChangedEventArgs args) signature instead.")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
		}

		protected virtual void OnPropertyChanged(PropertyChangedEventArgs args)
		{
			this.PropertyChanged?.Invoke(this, args);
		}

		[Obsolete("Please use RaisePropertyChanged(nameof(PropertyName)) instead. Expressions are slower, and the new nameof feature eliminates the magic strings.")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected virtual void OnPropertyChanged<T>(Expression<Func<T>> propertyExpression)
		{
			string propertyName = PropertySupport.ExtractPropertyName(propertyExpression);
			OnPropertyChanged(propertyName);
		}
	}
}
