using Prism.Modularity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ersa.Platform.Module.Extensions
{
	public static class EDC_IModuleExtensions
	{
		public static IEnumerable<Type> FUN_enuAlleModulTypen(this IModule i_edcModul)
		{
			Type type = i_edcModul.GetType();
			return type.Assembly.GetTypes().Except(new List<Type>
			{
				type
			});
		}
	}
}
