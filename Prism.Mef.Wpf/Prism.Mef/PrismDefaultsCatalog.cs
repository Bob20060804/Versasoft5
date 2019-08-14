using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Primitives;
using System.Linq;

namespace Prism.Mef
{
	internal class PrismDefaultsCatalog : ComposablePartCatalog
	{
		private readonly IEnumerable<ComposablePartDefinition> parts;

		public override IQueryable<ComposablePartDefinition> Parts => parts.AsQueryable();

		public PrismDefaultsCatalog(IEnumerable<ComposablePartDefinition> parts)
		{
			if (parts == null)
			{
				throw new ArgumentNullException("parts");
			}
			this.parts = parts;
		}
	}
}
