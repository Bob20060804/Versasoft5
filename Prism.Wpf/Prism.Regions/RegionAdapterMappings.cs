using Prism.Properties;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Prism.Regions
{
	public class RegionAdapterMappings
	{
		private readonly Dictionary<Type, IRegionAdapter> mappings = new Dictionary<Type, IRegionAdapter>();

		public void RegisterMapping(Type controlType, IRegionAdapter adapter)
		{
			if (controlType == null)
			{
				throw new ArgumentNullException("controlType");
			}
			if (adapter == null)
			{
				throw new ArgumentNullException("adapter");
			}
			if (mappings.ContainsKey(controlType))
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Prism.Properties.Resources.MappingExistsException, new object[1]
				{
					controlType.Name
				}));
			}
			mappings.Add(controlType, adapter);
		}

		public IRegionAdapter GetMapping(Type controlType)
		{
			Type type = controlType;
			while (type != null)
			{
				if (mappings.ContainsKey(type))
				{
					return mappings[type];
				}
				type = type.BaseType;
			}
			throw new KeyNotFoundException(string.Format(CultureInfo.CurrentCulture, Prism.Properties.Resources.NoRegionAdapterException, new object[1]
			{
				controlType
			}));
		}
	}
}
