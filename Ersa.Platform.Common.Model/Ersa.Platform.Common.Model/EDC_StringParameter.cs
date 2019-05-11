using Ersa.Platform.Infrastructure;

namespace Ersa.Platform.Common.Model
{
	public class EDC_StringParameter : EDC_PrimitivParameter
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
					RaisePropertyChanged("PRO_strWert");
					RaisePropertyChanged("PRO_strAnzeigeWert");
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
				RaisePropertyChanged("PRO_strAnzeigeWert");
			}
		}

		public override bool PRO_blnHatAenderung => PRO_strWert != PRO_strAnzeigeWert;

		public string PRO_strWert
		{
			get
			{
				return (string)PRO_objValue;
			}
			set
			{
				if (PRO_strWert != value)
				{
					PRO_objValue = value;
				}
			}
		}

		public string PRO_strAnzeigeWert
		{
			get
			{
				return (string)PRO_objAnzeigeWert;
			}
			set
			{
				if (PRO_strAnzeigeWert != value)
				{
					PRO_objAnzeigeWert = value;
				}
			}
		}
	}
}
