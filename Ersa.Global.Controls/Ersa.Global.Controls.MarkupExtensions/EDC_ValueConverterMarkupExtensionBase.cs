using System;
using System.Windows.Markup;

namespace Ersa.Global.Controls.MarkupExtensions
{
	public abstract class EDC_ValueConverterMarkupExtensionBase : MarkupExtension
	{
		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			return this;
		}
	}
}
