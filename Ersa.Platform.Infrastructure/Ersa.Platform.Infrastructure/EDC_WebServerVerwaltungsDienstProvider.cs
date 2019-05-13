using Ersa.Global.Common;
using Ersa.Platform.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace Ersa.Platform.Infrastructure
{
	[Export(typeof(INF_WebServerVerwaltungsDienstProvider))]
	public class EDC_WebServerVerwaltungsDienstProvider : EDC_DisposableObject, INF_WebServerVerwaltungsDienstProvider, IDisposable
	{
		private IDictionary<object, Func<object>> m_dicRegistrierteInterfaces = new Dictionary<object, Func<object>>();

		public void SUB_WebServerVerwaltungsDienstRegistrieren(object i_objWebserver, Func<object> i_delDelegat)
		{
			SUB_WebServerVerwaltungsDienstDeregistrieren(i_objWebserver);
			m_dicRegistrierteInterfaces.Add(i_objWebserver, i_delDelegat);
		}

		public void SUB_WebServerVerwaltungsDienstDeregistrieren(object i_objWebserver)
		{
			m_dicRegistrierteInterfaces.Remove(i_objWebserver);
		}

		public T FUN_edcWebServerVerwaltungsDienstHolen<T>(object i_objWebserver) where T : class
		{
			if (!m_dicRegistrierteInterfaces.TryGetValue(i_objWebserver, out Func<object> value))
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
