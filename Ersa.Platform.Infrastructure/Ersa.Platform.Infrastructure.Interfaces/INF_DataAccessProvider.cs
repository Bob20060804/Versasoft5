using System;

namespace Ersa.Platform.Infrastructure.Interfaces
{
	public interface INF_DataAccessProvider
	{
		void SUB_DataAccessInterfaceRegistrieren<T>(Func<object> i_delDataAccessInterface);

		void SUB_DataAccessInterfaceDeregistrieren<T>();

		T FUN_edcDataAccessInterfaceHolen<T>() where T : class;
	}
}
