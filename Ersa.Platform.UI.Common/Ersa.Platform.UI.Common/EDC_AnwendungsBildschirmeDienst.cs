using Ersa.Platform.Infrastructure;
using Ersa.Platform.UI.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace Ersa.Platform.UI.Common
{
	[Export(typeof(INF_AnwendungsBildschirmeDienst))]
	public class EDC_AnwendungsBildschirmeDienst : INF_AnwendungsBildschirmeDienst
	{
		private const string mC_strAnwendungsBildschirme = "AnwendungsBildschirme";

		private EDC_GlobalSetting m_edcBildschirmSetting;

		[Export]
		public IList<EDC_GlobalSetting> PRO_edcBildschirmSetting
		{
			get
			{
				List<EDC_GlobalSetting> list = new List<EDC_GlobalSetting>();
				object eDC_GlobalSetting = m_edcBildschirmSetting;
				if (eDC_GlobalSetting == null)
				{
					EDC_GlobalSetting obj = new EDC_GlobalSetting("AnwendungsBildschirme")
					{
						PRO_strDefaultWert = string.Empty,
						PRO_blnIstAusgeblendet = true,
						PRO_strLokalisierungsKey = "11_935",
						PRO_enmBereich = ENUM_GlobalSettingBereich.enmAnwender
					};
					EDC_GlobalSetting eDC_GlobalSetting2 = obj;
					m_edcBildschirmSetting = obj;
					eDC_GlobalSetting = eDC_GlobalSetting2;
				}
				list.Add((EDC_GlobalSetting)eDC_GlobalSetting);
				return list;
			}
		}

		public IEnumerable<int> FUN_enuAnwendungsBildschirmeErmitteln()
		{
			string[] array = ((!string.IsNullOrEmpty(m_edcBildschirmSetting.PRO_strWert)) ? m_edcBildschirmSetting.PRO_strWert : m_edcBildschirmSetting.PRO_strDefaultWert).Split(new string[1]
			{
				","
			}, StringSplitOptions.RemoveEmptyEntries);
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				if (int.TryParse(array2[i], out int result))
				{
					yield return result;
				}
			}
		}

		public void SUB_AnwendungsBildchirmeFestlegen(IEnumerable<int> i_enuBildschirme)
		{
			m_edcBildschirmSetting.PRO_strWert = string.Join(",", i_enuBildschirme);
		}
	}
}
