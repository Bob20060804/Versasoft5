using System;

namespace Ersa.Platform.Infrastructure.Interfaces
{
	public interface INF_WebServerKommunikationsDienstProvider : IDisposable
	{
		void SUB_WebServerKommunikationsDienstRegistrieren<T>(Func<object> i_delKommunikationsDienstInterface);

		void SUB_WebServerKommunikationsDienstDeregistrieren<T>();

		T FUN_edcWebServerKommunikationsDienstHolen<T>() where T : class;
	}
}
