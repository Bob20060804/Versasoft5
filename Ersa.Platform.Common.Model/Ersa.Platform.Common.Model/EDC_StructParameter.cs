using Ersa.Global.Common.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace Ersa.Platform.Common.Model
{
	public abstract class EDC_StructParameter : EDC_AdressRelevanterTeil
	{
		protected EDC_Komposition PRO_edcKomposition
		{
			get;
		}

		protected EDC_StructParameter()
		{
			PRO_edcKomposition = new EDC_Komposition();
		}

		public virtual void SUB_AenderungSignalisieren()
		{
		}

		public override IEnumerable<EDC_AdressRelevanterTeil> FUN_enuElementeHolen()
		{
			return this.FUN_enuUnion(PRO_edcKomposition.PRO_objValues.SelectMany((EDC_AdressRelevanterTeil i_edcValue) => i_edcValue.FUN_enuElementeHolen()));
		}

		protected virtual void SUB_KompositionsPropertySetter(object i_objAdressAnTeil, EDC_AdressRelevanterTeil i_edcValue)
		{
			EDC_AdressRelevanterTeil eDC_AdressRelevanterTeil = PRO_edcKomposition[i_objAdressAnTeil];
			if (i_edcValue != eDC_AdressRelevanterTeil)
			{
				if (eDC_AdressRelevanterTeil != null)
				{
					PRO_edcKomposition.SUB_Entfernen(eDC_AdressRelevanterTeil.PRO_objAdressAnteil, delegate
					{
					});
				}
				if (i_edcValue != null)
				{
					PRO_edcKomposition.SUB_Hinzufuegen(this, i_objAdressAnTeil, i_edcValue);
				}
			}
		}
	}
}
