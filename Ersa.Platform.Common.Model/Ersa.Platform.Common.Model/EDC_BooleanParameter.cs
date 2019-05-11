using Ersa.Platform.Infrastructure;

namespace Ersa.Platform.Common.Model
{
	public class EDC_BooleanParameter : EDC_PrimitivParameter
	{
		public override object PRO_objValue
		{
			get
			{
				return base.PRO_objValue;
			}
			set
			{
				base.PRO_objValue = value;
				EDC_Dispatch.SUB_AktionStarten(delegate
				{
					RaisePropertyChanged("PRO_blnWert");
					RaisePropertyChanged("PRO_blnAnzeigeWert");
				});
			}
		}

		public override object PRO_objAnzeigeWert
		{
			get
			{
				return base.PRO_objAnzeigeWert;
			}
			set
			{
				base.PRO_objAnzeigeWert = value;
				RaisePropertyChanged("PRO_blnAnzeigeWert");
			}
		}

		public override bool PRO_blnHatAenderung => PRO_blnWert != PRO_blnAnzeigeWert;

		public bool PRO_blnWert
		{
			get
			{
				return EDC_WertKonvertierung.FUN_blnWertUmwandeln(PRO_objValue) == true;
			}
			set
			{
				if (PRO_blnWert != value || PRO_objValue == null)
				{
					PRO_objValue = value;
				}
			}
		}

		public bool PRO_blnAnzeigeWert
		{
			get
			{
				return EDC_WertKonvertierung.FUN_blnWertUmwandeln(PRO_objAnzeigeWert) == true;
			}
			set
			{
				if (PRO_blnAnzeigeWert != value)
				{
					PRO_objAnzeigeWert = value;
				}
			}
		}
	}
}
