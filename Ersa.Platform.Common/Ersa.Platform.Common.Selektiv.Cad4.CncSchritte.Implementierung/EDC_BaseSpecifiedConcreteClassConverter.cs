using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;

namespace Ersa.Platform.Common.Selektiv.Cad4.CncSchritte.Implementierung
{
	public class EDC_BaseSpecifiedConcreteClassConverter : DefaultContractResolver
	{
		protected override JsonConverter ResolveContractConverter(Type i_fdcObjectType)
		{
			if (typeof(EDC_AbstractCncSchritt).IsAssignableFrom(i_fdcObjectType) && !i_fdcObjectType.IsAbstract)
			{
				return null;
			}
			return base.ResolveContractConverter(i_fdcObjectType);
		}
	}
}
