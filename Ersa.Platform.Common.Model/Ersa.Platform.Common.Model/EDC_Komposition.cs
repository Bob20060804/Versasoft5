using System;
using System.Collections.Generic;

namespace Ersa.Platform.Common.Model
{
	public class EDC_Komposition
	{
		private readonly IDictionary<object, EDC_AdressRelevanterTeil> m_dicValue;

		public IEnumerable<EDC_AdressRelevanterTeil> PRO_objValues => m_dicValue.Values;

		public EDC_AdressRelevanterTeil this[object i_objKey]
		{
			get
			{
				m_dicValue.TryGetValue(i_objKey, out EDC_AdressRelevanterTeil value);
				return value;
			}
		}

		public EDC_Komposition()
		{
			m_dicValue = new Dictionary<object, EDC_AdressRelevanterTeil>();
		}

		public void SUB_Hinzufuegen(EDC_AdressRelevanterTeil i_edcParent, object i_objAdressAnteil, EDC_AdressRelevanterTeil i_edcParameter)
		{
			i_edcParameter.PRO_objAdressAnteil = i_objAdressAnteil;
			i_edcParameter.PRO_edcParent = i_edcParent;
			m_dicValue.Add(i_objAdressAnteil, i_edcParameter);
		}

		public void SUB_Entfernen(object i_objAdressAnteil, Action<EDC_AdressRelevanterTeil> i_delVomParentEntfernen)
		{
			EDC_AdressRelevanterTeil eDC_AdressRelevanterTeil = m_dicValue[i_objAdressAnteil];
			i_delVomParentEntfernen(eDC_AdressRelevanterTeil.PRO_edcParent);
			eDC_AdressRelevanterTeil.PRO_edcParent = null;
			m_dicValue.Remove(i_objAdressAnteil);
		}
	}
}
