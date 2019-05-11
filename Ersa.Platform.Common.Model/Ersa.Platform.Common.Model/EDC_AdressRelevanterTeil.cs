using Ersa.Platform.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ersa.Platform.Common.Model
{
	public abstract class EDC_AdressRelevanterTeil : EDC_NotificationObjectMitSprachUmschaltung
	{
		private string m_strAdresse;

		private object[] ma_objAdresse;

		public string PRO_strAdresse
		{
			get
			{
				if (m_strAdresse == null)
				{
					SUB_ErzeugeAdresse();
				}
				return m_strAdresse;
			}
		}

		[Obsolete("PROa_objAdresse verwenden")]
		public object[] PRO_aobjAdresse
		{
			get
			{
				return PROa_objAdresse;
			}
		}

		public object[] PROa_objAdresse
		{
			get
			{
				if (ma_objAdresse == null)
				{
					SUB_ErzeugeAdresse();
				}
				return ma_objAdresse;
			}
		}

		public string PRO_strNameKey
		{
			get;
			set;
		}

		public string PRO_strNameSuffix
		{
			get;
			set;
		}

		public Uri PRO_uriHilfe
		{
			get;
			set;
		}

		public EDC_AdressRelevanterTeil PRO_edcParent
		{
			get;
			set;
		}

		public object PRO_objAdressAnteil
		{
			get;
			set;
		}

		public abstract IEnumerable<EDC_AdressRelevanterTeil> FUN_enuElementeHolen();

		private void SUB_ErzeugeAdresse()
		{
			EDC_AdressRelevanterTeil eDC_AdressRelevanterTeil = this;
			IList<object> list = new List<object>();
			while (eDC_AdressRelevanterTeil != null)
			{
				if (eDC_AdressRelevanterTeil.PRO_objAdressAnteil != null)
				{
					list.Add(eDC_AdressRelevanterTeil.PRO_objAdressAnteil);
				}
				eDC_AdressRelevanterTeil = eDC_AdressRelevanterTeil.PRO_edcParent;
			}
			ma_objAdresse = list.Reverse().ToArray();
			m_strAdresse = string.Join("|", ma_objAdresse);
		}
	}
}
