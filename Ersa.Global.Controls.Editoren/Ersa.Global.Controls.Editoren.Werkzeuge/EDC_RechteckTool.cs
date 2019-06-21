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
	public class EDC_RechteckTool : EDC_Tool
	{
		private readonly List<Point> m_lstPunkte = new List<Point>();

		private INF_RechteckErstellenKontext m_edcRechteckErstellenKontext;

		private EDC_RechteckElement m_edcTemporaeresRechteck;

		public override bool PRO_blnErlaubeKontextmenue => false;

		[Obsolete("SUB_AlleVerfuegbarenKontexteInitialisieren verwenden")]
		public void SUB_RechteckErstellenKontextInitialisieren(INF_RechteckErstellenKontext i_edcRechteckErstellenKontext)
		{
			m_edcRechteckErstellenKontext = i_edcRechteckErstellenKontext;
		}

		public override void SUB_AlleVerfuegbarenKontexteInitialisieren(params object[] ia_objKontexte)
		{
			base.SUB_AlleVerfuegbarenKontexteInitialisieren(ia_objKontexte);
			INF_RechteckErstellenKontext iNF_RechteckErstellenKontext = ia_objKontexte.OfType<INF_RechteckErstellenKontext>().FirstOrDefault();
			if (iNF_RechteckErstellenKontext != null)
			{
				m_edcRechteckErstellenKontext = iNF_RechteckErstellenKontext;
			}
		}

		public override bool FUN_blnMouseDown(Point i_sttPosition, MouseButtonState i_enmLeftButtonState, MouseButtonState i_enmRightButtonState)
		{
			base.FUN_blnMouseDown(i_sttPosition, i_enmLeftButtonState, i_enmRightButtonState);
			bool result = false;
			if (i_enmLeftButtonState == MouseButtonState.Pressed)
			{
				result = FUN_blnPunktHinzufuegen(i_sttPosition);
			}
			if (i_enmRightButtonState == MouseButtonState.Pressed)
			{
				result = SUB_TemporaeresRechteckEntfernen();
			}
			return result;
		}

		public override bool FUN_blnMouseMove(Point i_sttPosition, MouseButtonState i_enmLeftButtonState)
		{
			base.FUN_blnMouseMove(i_sttPosition, i_enmLeftButtonState);
			if (!m_lstPunkte.Any())
			{
				return false;
			}
			SUB_ErstelleTempraeresRechteckElementFallsNichtVorhanden();
			if (m_edcTemporaeresRechteck != null)
			{
				SUB_SetzePunkte(m_lstPunkte.FUN_enuUnion(i_sttPosition).ToList());
			}
			return false;
		}

		public override void SUB_WerkzeugDeaktiviert()
		{
			base.SUB_WerkzeugDeaktiviert();
			SUB_TemporaeresRechteckEntfernen();
		}

		private void SUB_ErstelleTempraeresRechteckElementFallsNichtVorhanden()
		{
			if (m_edcTemporaeresRechteck == null)
			{
				m_edcTemporaeresRechteck = m_edcRechteckErstellenKontext?.FUN_edcErzeugeRechteck();
				if (m_edcTemporaeresRechteck != null)
				{
					SUB_FuegeElementTemporaerHinzu(m_edcTemporaeresRechteck);
				}
			}
		}

		private bool FUN_blnPunktHinzufuegen(Point i_sttPosition)
		{
			if (m_edcRechteckErstellenKontext?.FUN_blnValidiereRechteckPunkt(m_lstPunkte, i_sttPosition) ?? true)
			{
				m_lstPunkte.Add(i_sttPosition);
				SUB_SetzePunkte(m_lstPunkte);
			}
			if (m_lstPunkte.Count >= 2)
			{
				return FUN_blnRechteckBeenden();
			}
			return true;
		}

		private void SUB_SetzePunkte(IList<Point> i_lstPunkte)
		{
			SUB_ErstelleTempraeresRechteckElementFallsNichtVorhanden();
			Rect i_sttRechteck = (i_lstPunkte.Count >= 2) ? new Rect(i_lstPunkte[0], i_lstPunkte[1]) : new Rect(i_lstPunkte[0], i_lstPunkte[0]);
			m_edcTemporaeresRechteck.SUB_SetzePunkte(i_sttRechteck);
		}

		private bool FUN_blnRechteckBeenden()
		{
			m_edcRechteckErstellenKontext?.SUB_UebernehmeRechteck(m_edcTemporaeresRechteck);
			return SUB_TemporaeresRechteckEntfernen();
		}

		private bool SUB_TemporaeresRechteckEntfernen()
		{
			if (m_lstPunkte.Count == 0)
			{
				SUB_WerkzeugDeaktivierenAnfordern();
			}
			else
			{
				SUB_EntferneTemporaeresElement(m_edcTemporaeresRechteck);
			}
			m_edcTemporaeresRechteck = null;
			m_lstPunkte.Clear();
			return true;
		}
	}
}
