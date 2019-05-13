using System;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Linq;
using System.Linq.Expressions;

namespace Ersa.Platform.Infrastructure.Prism
{
	public class EDC_FilteredCatalog : ComposablePartCatalog, INotifyComposablePartCatalogChanged
	{
		private readonly ComposablePartCatalog m_fdcInnerCatalog;

		private readonly INotifyComposablePartCatalogChanged m_fdcInnerNotifyChange;

		private readonly IQueryable<ComposablePartDefinition> m_fdcPartsQuery;

		public override IQueryable<ComposablePartDefinition> Parts => m_fdcPartsQuery;

		public event EventHandler<ComposablePartCatalogChangeEventArgs> Changed
		{
			add
			{
				if (m_fdcInnerNotifyChange != null)
				{
					m_fdcInnerNotifyChange.Changed += value;
				}
			}
			remove
			{
				if (m_fdcInnerNotifyChange != null)
				{
					m_fdcInnerNotifyChange.Changed -= value;
				}
			}
		}

		public event EventHandler<ComposablePartCatalogChangeEventArgs> Changing
		{
			add
			{
				if (m_fdcInnerNotifyChange != null)
				{
					m_fdcInnerNotifyChange.Changing += value;
				}
			}
			remove
			{
				if (m_fdcInnerNotifyChange != null)
				{
					m_fdcInnerNotifyChange.Changing -= value;
				}
			}
		}

		public EDC_FilteredCatalog(ComposablePartCatalog i_fdcInnerCatalog, Expression<Func<ComposablePartDefinition, bool>> i_fdcExpression)
		{
			m_fdcInnerCatalog = i_fdcInnerCatalog;
			m_fdcInnerNotifyChange = (m_fdcInnerCatalog as INotifyComposablePartCatalogChanged);
			m_fdcPartsQuery = m_fdcInnerCatalog.Parts.Where(i_fdcExpression);
		}
	}
}
