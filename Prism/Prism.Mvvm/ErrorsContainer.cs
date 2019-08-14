using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Prism.Mvvm
{
	public class ErrorsContainer<T>
	{
		private static readonly T[] noErrors = new T[0];

		protected readonly Action<string> raiseErrorsChanged;

		protected readonly Dictionary<string, List<T>> validationResults;

		public bool HasErrors => validationResults.Count != 0;

		public ErrorsContainer(Action<string> raiseErrorsChanged)
		{
			if (raiseErrorsChanged == null)
			{
				throw new ArgumentNullException("raiseErrorsChanged");
			}
			this.raiseErrorsChanged = raiseErrorsChanged;
			validationResults = new Dictionary<string, List<T>>();
		}

		public IEnumerable<T> GetErrors(string propertyName)
		{
			string key = propertyName ?? string.Empty;
			List<T> value = null;
			if (validationResults.TryGetValue(key, out value))
			{
				return value;
			}
			return noErrors;
		}

		public void ClearErrors<TProperty>(Expression<Func<TProperty>> propertyExpression)
		{
			string propertyName = PropertySupport.ExtractPropertyName(propertyExpression);
			ClearErrors(propertyName);
		}

		public void ClearErrors(string propertyName)
		{
			SetErrors(propertyName, new List<T>());
		}

		public void SetErrors<TProperty>(Expression<Func<TProperty>> propertyExpression, IEnumerable<T> propertyErrors)
		{
			string propertyName = PropertySupport.ExtractPropertyName(propertyExpression);
			SetErrors(propertyName, propertyErrors);
		}

		public void SetErrors(string propertyName, IEnumerable<T> newValidationResults)
		{
			string text = propertyName ?? string.Empty;
			bool num = validationResults.ContainsKey(text);
			bool flag = newValidationResults != null && newValidationResults.Count() > 0;
			if (num | flag)
			{
				if (flag)
				{
					validationResults[text] = new List<T>(newValidationResults);
					raiseErrorsChanged(text);
				}
				else
				{
					validationResults.Remove(text);
					raiseErrorsChanged(text);
				}
			}
		}
	}
}
