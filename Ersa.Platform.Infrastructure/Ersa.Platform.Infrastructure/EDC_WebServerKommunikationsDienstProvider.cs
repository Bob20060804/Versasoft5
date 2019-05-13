using Ersa.Global.Common;
using Ersa.Platform.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace Ersa.Platform.Infrastructure
{
	[Export(typeof(INF_WebServerKommunikationsDienstProvider))]
	public class EDC_WebServerKommunikationsDienstProvider : EDC_DisposableObject, INF_WebServerKommunikationsDienstProvider, IDisposable
	{
		private IDictionary<Type, Func<object>> m_dicRegistrierteInterfaces = new Dictionary<Type, Func<object>>();

		public void SUB_WebServerKommunikationsDienstRegistrieren<T>(Func<object> i_delKommunikationsDienstInterface)
		{
			SUB_WebServerKommunikationsDienstDeregistrieren<T>();
			m_dicRegistrierteInterfaces.Add(typeof(T), i_delKommunikationsDienstInterface);
		}

		public void SUB_WebServerKommunikationsDienstDeregistrieren<T>()
		{
			m_dicRegistrierteInterfaces.Remove(typeof(T));
		}

		public T FUN_edcWebServerKommunikationsDienstHolen<T>() where T : class
		{
			if (!m_dicRegistrierteInterfaces.TryGetValue(typeof(T), out Func<object> value))
			{
				return null;
			}
			return (T)value();
		}

		protected override void SUB_InternalDispose()
		{
			m_dicRegistrierteInterfaces = null;
		}
	}
}
