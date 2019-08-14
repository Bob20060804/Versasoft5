using Prism.Properties;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Prism.Mvvm
{
	public static class PropertySupport
	{
		public static string ExtractPropertyName<T>(Expression<Func<T>> propertyExpression)
		{
			if (propertyExpression == null)
			{
				throw new ArgumentNullException("propertyExpression");
			}
			return ExtractPropertyNameFromLambda(propertyExpression);
		}

		internal static string ExtractPropertyNameFromLambda(LambdaExpression expression)
		{
			if (expression == null)
			{
				throw new ArgumentNullException("expression");
			}
			MemberExpression obj = expression.Body as MemberExpression;
			if (obj == null)
			{
				throw new ArgumentException(Resources.PropertySupport_NotMemberAccessExpression_Exception, "expression");
			}
			PropertyInfo obj2 = obj.Member as PropertyInfo;
			if ((object)obj2 == null)
			{
				throw new ArgumentException(Resources.PropertySupport_ExpressionNotProperty_Exception, "expression");
			}
			if (obj2.GetMethod.IsStatic)
			{
				throw new ArgumentException(Resources.PropertySupport_StaticExpression_Exception, "expression");
			}
			return obj.Member.Name;
		}
	}
}
