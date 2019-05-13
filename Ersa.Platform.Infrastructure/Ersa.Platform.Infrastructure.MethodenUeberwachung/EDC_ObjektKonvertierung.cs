using System;
using System.Collections.Generic;
using System.Linq;

namespace Ersa.Platform.Infrastructure.MethodenUeberwachung
{
	public class EDC_ObjektKonvertierung
	{
		private static EDC_ObjektKonvertierung ms_edcInstanz;

		private readonly IDictionary<Type, Func<object, string>> m_dicTypKonvertierer;

		public static EDC_ObjektKonvertierung PRO_edcInstanz => ms_edcInstanz ?? (ms_edcInstanz = new EDC_ObjektKonvertierung());

		private EDC_ObjektKonvertierung()
		{
			m_dicTypKonvertierer = new Dictionary<Type, Func<object, string>>
			{
				{
					typeof(string[]),
					delegate(object l_objObjekt)
					{
						string[] value = ((string[])l_objObjekt).Select(FUN_strObjektKonvertieren).ToArray();
						return string.Format("[{0}]", string.Join(", ", value));
					}
				},
				{
					typeof(DateTime),
					(object l_objObjekt) => string.Format("{{{0:HH}:{0:mm}:{0:ss}, {0:dd}.{0:MM}.{0:yyyy} ({0:dddd})}}", (DateTime)l_objObjekt)
				}
			};
		}

		public string FUN_strObjektKonvertieren(object i_objObjekt)
		{
			if (i_objObjekt == null)
			{
				return "null";
			}
			m_dicTypKonvertierer.TryGetValue(i_objObjekt.GetType(), out Func<object, string> value);
			return (value != null) ? value(i_objObjekt) : $"{{{i_objObjekt}}}";
		}
	}
}
