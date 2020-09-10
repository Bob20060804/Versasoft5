using Ersa.Global.Common.Extensions;
using System.Collections.Generic;
using System.Linq;
using Ersa.Global.Mvvm;

namespace Ersa.Platform.Common.Model
{
	public class EDC_ArrayParameter : EDC_ArrayParameter<EDC_AdressRelevanterTeil>
	{
	}
	public class EDC_ArrayParameter<T> : EDC_AdressRelevanterTeil where T : EDC_AdressRelevanterTeil
	{
		public IList<T> PRO_lstValue
		{
			get;
		}

		public EDC_ArrayParameter()
		{
			PRO_lstValue = new List<T>();
		}

		public void SUB_ParameterHinzufuegen(T i_edcParameter, int? i_intAdressAnteil = default(int?))
		{
			i_edcParameter.PRO_objAdressAnteil = (i_intAdressAnteil ?? PRO_lstValue.Count);
			i_edcParameter.PRO_edcParent = this;
			PRO_lstValue.Add(i_edcParameter);
		}

		public void SUB_ParameterHinzufuegenMitIndex(T i_edcParameter, int i_i32Index)
		{
			i_edcParameter.PRO_objAdressAnteil = i_i32Index;
			i_edcParameter.PRO_edcParent = this;
			PRO_lstValue.Add(i_edcParameter);
		}

		public void SUB_ParameterEntfernen(int i_i32AdressAnteil)
		{
			T val = PRO_lstValue.FirstOrDefault((T i_edcParam) => (int)i_edcParam.PRO_objAdressAnteil == i_i32AdressAnteil);
			if (val != null)
			{
				val.PRO_edcParent = null;
				PRO_lstValue.Remove(val);
			}
		}

		public override IEnumerable<EDC_AdressRelevanterTeil> FUN_enuElementeHolen()
		{
			return this.FUN_enuUnion(PRO_lstValue.SelectMany((T i_edcElement) => i_edcElement.FUN_enuElementeHolen()));
		}
	}
}
