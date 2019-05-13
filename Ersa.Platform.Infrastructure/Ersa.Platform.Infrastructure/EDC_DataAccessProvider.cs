using Ersa.Platform.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace Ersa.Platform.Infrastructure
{
	[Export(typeof(INF_DataAccessProvider))]
	public class EDC_DataAccessProvider : INF_DataAccessProvider
	{
		private readonly IDictionary<Type, Func<object>> m_dicRegistrierteInterfaces = new Dictionary<Type, Func<object>>();

		public void SUB_DataAccessInterfaceRegistrieren<T>(Func<object> i_delDataAccessInterface)
		{
			SUB_DataAccessInterfaceDeregistrieren<T>();
			m_dicRegistrierteInterfaces.Add(typeof(T), i_delDataAccessInterface);
		}

		public void SUB_DataAccessInterfaceDeregistrieren<T>()
		{
			m_dicRegistrierteInterfaces.Remove(typeof(T));
		}

		public T FUN_edcDataAccessInterfaceHolen<T>() where T : class
		{
			if (!m_dicRegistrierteInterfaces.TryGetValue(typeof(T), out Func<object> value))
			{
				return null;
			}
			return (T)value();
		}
	}
}
