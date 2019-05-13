using System;
using System.Collections.Generic;

namespace Ersa.Platform.Infrastructure.Interfaces
{
	public interface INF_CapabilityProvider
	{
		void SUB_CapabilityRegistrieren<T>(Func<object> i_delCapabilityErmittler);

		void SUB_CapabilityDeregistrieren<T>();

		T FUN_edcCapabilityHolen<T>() where T : class;

		void SUB_MehrfachCapabilityRegistrieren<T>(Func<object> i_delCapabilityErmittler);

		IEnumerable<T> FUN_edcMehrfachCapabilityListeHolen<T>() where T : class;
	}
}
