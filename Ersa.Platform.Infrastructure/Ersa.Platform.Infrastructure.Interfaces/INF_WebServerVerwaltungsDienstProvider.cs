using System;

namespace Ersa.Platform.Infrastructure.Interfaces
{
	public interface INF_WebServerVerwaltungsDienstProvider : IDisposable
	{
		void SUB_WebServerVerwaltungsDienstRegistrieren(object i_objWebserver, Func<object> i_delDelegat);

		void SUB_WebServerVerwaltungsDienstDeregistrieren(object i_objWebserver);

		T FUN_edcWebServerVerwaltungsDienstHolen<T>(object i_objWebserver) where T : class;
	}
}
