using System;

namespace Ersa.Platform.Common.Loetprotokoll
{
	[AttributeUsage(AttributeTargets.Property)]
	public class EDC_ProtokollParameterAttribute : Attribute
	{
		public ENUM_ProtokollParameterTyp PRO_enmParameterTyp
		{
			get;
			set;
		}

		public EDC_ProtokollParameterAttribute()
		{
			PRO_enmParameterTyp = ENUM_ProtokollParameterTyp.enmUndefiniert;
		}
	}
}
