namespace Ersa.Platform.Common.Model
{
    /// <summary>
    /// 参数描述
    /// </summary>
	public class EDC_ParameterBeschreibung
	{
        /// <summary>
        /// 参数行为
        /// </summary>
		public ENUM_ParameterVerhalten PRO_enmParameterVerhalten
		{
			get;
			set;
		}

        /// <summary>
        /// 参数持久性
        /// </summary>
		public ENUM_ParameterPersistenz PRO_enmParameterPersistenz
		{
			get;
			set;
		}

        /// <summary>
        /// 参数描述
        /// </summary>
		public ENUM_Richtung PRO_enmRichtung
		{
			get;
			set;
		}

		public bool Equals(EDC_ParameterBeschreibung i_edcAndereParameterBeschreibung)
		{
			if (i_edcAndereParameterBeschreibung == null)
			{
				return false;
			}
			if (PRO_enmParameterPersistenz.Equals(i_edcAndereParameterBeschreibung.PRO_enmParameterPersistenz) && PRO_enmParameterVerhalten.Equals(i_edcAndereParameterBeschreibung.PRO_enmParameterVerhalten))
			{
				return PRO_enmRichtung.Equals(i_edcAndereParameterBeschreibung.PRO_enmRichtung);
			}
			return false;
		}

		public override int GetHashCode()
		{
			return PRO_enmParameterVerhalten.GetHashCode() ^ PRO_enmParameterPersistenz.GetHashCode() ^ PRO_enmRichtung.GetHashCode();
		}

		public override bool Equals(object i_objObjekt)
		{
			EDC_ParameterBeschreibung eDC_ParameterBeschreibung = i_objObjekt as EDC_ParameterBeschreibung;
			if (eDC_ParameterBeschreibung != null)
			{
				return Equals(eDC_ParameterBeschreibung);
			}
			return false;
		}
	}
}
