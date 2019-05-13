using Ersa.Platform.UI.Common.Helfer;
using System;
using System.Windows.Markup;

namespace Ersa.Platform.UI.Common.MarkupExtensions
{
	public class EDC_EnumExtension : MarkupExtension
	{
		private readonly Type m_fdcEnumType;

		public EDC_EnumExtension(Type i_fdcEnumType)
		{
			if (i_fdcEnumType == null)
			{
				throw new ArgumentNullException("i_fdcEnumType");
			}
			if (!(Nullable.GetUnderlyingType(i_fdcEnumType) ?? i_fdcEnumType).IsEnum)
			{
				throw new ArgumentException("Type must be an enum type");
			}
			m_fdcEnumType = i_fdcEnumType;
		}

		public override object ProvideValue(IServiceProvider i_fdcServiceProvider)
		{
			return EDC_EnumHelfer.FUN_enmBeschreibungenErmitteln(m_fdcEnumType);
		}
	}
}
