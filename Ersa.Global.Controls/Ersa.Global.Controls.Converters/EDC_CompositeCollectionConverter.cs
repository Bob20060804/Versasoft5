using System;
using System.Collections;
using System.Globalization;
using System.Windows.Data;

namespace Ersa.Global.Controls.Converters
{
	public class EDC_CompositeCollectionConverter : IMultiValueConverter
	{
		public object Convert(object[] ia_objValues, Type i_fdcTargetType, object i_objParameter, CultureInfo i_fdcCulture)
		{
			CompositeCollection compositeCollection = new CompositeCollection();
			foreach (object obj in ia_objValues)
			{
				IEnumerable enumerable = obj as IEnumerable;
				compositeCollection.Add((enumerable != null) ? new CollectionContainer
				{
					Collection = enumerable
				} : obj);
			}
			return compositeCollection;
		}

		public object[] ConvertBack(object i_objValue, Type[] ia_fdcTargetTypes, object i_objParameter, CultureInfo i_fdcCulture)
		{
			throw new NotImplementedException();
		}
	}
}
