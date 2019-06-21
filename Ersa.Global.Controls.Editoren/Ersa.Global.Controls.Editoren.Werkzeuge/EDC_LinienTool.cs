using Ersa.Global.Common.Extensions;
using Ersa.Global.Controls.Editoren.EditorElemente;
using Ersa.Global.Controls.Editoren.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Ersa.Global.Controls.Editoren.Werkzeuge
{
	public class EDC_LinienTool : EDC_Tool
	{
		private readonly List<Point> m_lstPunkte = new List<Point>();

		private INF_LinieErstellenKontext m_edcLinieErstellenKontext;

		private EDC_LinienElement m_edcTemporaeresLinienElement;

		public bool PRO_blnBeschraenkePunktAnzahl
		{
			get;
			set;
		}

		public int PRO_i32MaximalePunktAnzahl
		{
			get;
			set;
		}

		public int PRO_i32MinimalePunktAnzahl
		{
			get;
			set;
		}

		public override bool PRO_blnErlaubeKontextmenue => false;

		[Obsolete("SUB_AlleVerfuegbarenKontexteInitialisieren verwenden")]
		public void SUB_LinieErstellenKontextInitialisieren(INF_LinieErstellenKontext i_edcLinieErstellenKontext)
		{
			m_edcLinieErstellenKontext = i_edcLinieErstellenKontext;
		}

		public override void SUB_AlleVerfuegbarenKontexteInitialisieren(params object[] ia_objKontexte)
		{
			base.SUB_AlleVerfuegbarenKontexteInitialisieren(ia_objKontexte);
			INF_LinieErstellenKontext iNF_LinieErstellenKontext = ia_objKontexte.OfType<INF_LinieErstellenKontext>().FirstOrDefault();
			if (iNF_LinieErstellenKontext != null)
			{
				m_edcLinieErstellenKontext = iNF_LinieErstellenKontext;
			}
		}

		public override bool FUN_blnMouseDown(Point i_sttPosition, MouseButtonState i_enmLeftButtonState, MouseButtonState i_enmRightButtonState)
		{
			base.FUN_blnMouseDown(i_sttPosition, i_enmLeftButtonState, i_enmRightButtonState);
			SUB_ErzeugeTemporaeresLinienElementFallsNichtVorhanden();
			bool result = false;
			if (i_enmLeftButtonState == MouseButtonState.Pressed)
			{
				result = FUN_blnPunktZuLinieHinzufuegen(i_sttPosition);
			}
			if (i_enmRightButtonState == MouseButtonState.Pressed)
			{
				result = FUN_blnLinieBeenden();
			}
			return result;
		}

		public override bool FUN_blnMouseMove(Point i_sttPosition, MouseButtonState i_enmLeftButtonState)
		{
			base.FUN_blnMouseMove(i_sttPosition, i_enmLeftButtonState);
			SUB_ErzeugeTemporaeresLinienElementFallsNichtVorhanden();
			m_edcTemporaeresLinienElement?.SUB_SetzePunkte(m_lstPunkte.FUN_enuUnion(i_sttPosition));
			return false;
		}

		public override void SUB_MouseLeave()
		{
			base.SUB_MouseLeave();
			m_edcTemporaeresLinienElement?.SUB_SetzePunkte(m_lstPunkte);
		}

		public override void SUB_WerkzeugDeaktiviert()
		{
			base.SUB_WerkzeugDeaktiviert();
			SUB_LinieAbbrechen();
		}

		private void SUB_ErzeugeTemporaeresLinienElementFallsNichtVorhanden()
		{
			if (m_edcTemporaeresLinienElement == null)
			{
				m_edcTemporaeresLinienElement = m_edcLinieErstellenKontext?.FUN_edcErzeugeLinie();
				if (m_edcTemporaeresLinienElement != null)
				{
					SUB_FuegeElementTemporaerHinzu(m_edcTemporaeresLinienElement);
				}
			}
		}

		private bool FUN_blnPunktZuLinieHinzufuegen(Point i_sttPosition)
		{
			if (m_edcLinieErstellenKontext?.FUN_blnValidiereLinienPunkt(m_lstPunkte, i_sttPosition) ?? true)
			{
				m_lstPunkte.Add(i_sttPosition);
				m_edcTemporaeresLinienElement.SUB_SetzePunkte(m_lstPunkte);
			}
			if (PRO_blnBeschraenkePunktAnzahl && m_lstPunkte.Count >= PRO_i32MaximalePunktAnzahl)
			{
				return FUN_blnLinieBeenden();
			}
			return true;
		}

		private void SUB_LinieAbbrechen()
		{
			if (m_edcTemporaeresLinienElement != null)
			{
				SUB_EntferneTemporaeresElement(m_edcTemporaeresLinienElement);
				m_edcTemporaeresLinienElement = null;
			}
			m_lstPunkte.Clear();
		}

		private bool FUN_blnLinieBeenden()
		{
			if (m_lstPunkte.Count < PRO_i32MinimalePunktAnzahl)
			{
				SUB_LinieAbbrechen();
			}
			if (m_lstPunkte.Count == 0)
			{
				SUB_WerkzeugDeaktivierenAnfordern();
				return true;
			}
			if (m_lstPunkte.Count > 0)
			{
				m_edcTemporaeresLinienElement.SUB_SetzePunkte(m_lstPunkte);
				m_edcLinieErstellenKontext?.SUB_UebernehmeLinie(m_edcTemporaeresLinienElement);
				SUB_EntferneTemporaeresElement(m_edcTemporaeresLinienElement);
				m_edcTemporaeresLinienElement = null;
				m_lstPunkte.Clear();
			}
			return true;
		}
	}
}
