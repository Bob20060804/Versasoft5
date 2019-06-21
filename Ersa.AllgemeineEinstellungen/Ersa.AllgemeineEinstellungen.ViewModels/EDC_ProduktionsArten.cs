using Ersa.Platform.Common.Produktionssteuerung;
using System.Collections.Generic;
using System.Linq;

namespace Ersa.AllgemeineEinstellungen.ViewModels
{
	public class EDC_ProduktionsArten : EDC_ProduktionsArtElement
	{
		private bool m_blnAktiv;

		public bool PRO_blnAktiv
		{
			get
			{
				return m_blnAktiv;
			}
			set
			{
				SetProperty(ref m_blnAktiv, value, "PRO_blnAktiv");
			}
		}

		public ENUM_Produktionsart? PRO_enmProduktionsart
		{
			get;
		}

		public IEnumerable<EDC_ProduktionsUnterart> PRO_enuUnterarten
		{
			get;
		}

		public EDC_ProduktionsUnterart PRO_edcAktiveUnterart
		{
			get;
			set;
		}

		public EDC_ProduktionsArten(string i_strLocKey, ENUM_Produktionsart i_enmProduktionsart)
			: base(i_strLocKey)
		{
			PRO_enmProduktionsart = i_enmProduktionsart;
			PRO_enuUnterarten = Enumerable.Empty<EDC_ProduktionsUnterart>();
		}

		public EDC_ProduktionsArten(string i_strLocKey, params EDC_ProduktionsUnterart[] ia_edcUnterarten)
			: base(i_strLocKey)
		{
			PRO_enuUnterarten = ia_edcUnterarten;
		}
	}
}
