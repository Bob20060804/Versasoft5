using Ersa.Global.Common.Helper;
using Ersa.Global.Controls.Editoren.EditorElemente;
using Ersa.Global.Controls.Editoren.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Ersa.Global.Controls.Editoren.Werkzeuge
{
	public class EDC_PasteTool : EDC_Tool
	{
		private const float mC_sngDrehungStep = -1.57079637f;

		private INF_CopyPasteKontext m_edcCopyPasteKontext;

		private bool m_blnElementeAngezeigt;

		private IEnumerable<EDC_EditorElementMitPunkten> m_enuCopyPasteElemente;

		private Point m_sttMittelPunkt;

		private Point m_ssttUrspruenglicherMittelPunkt;

		private float m_sngRotationRad;

		public override bool PRO_blnErlaubeKontextmenue => false;

		public void SUB_CopyPasteKontextInitialisieren(INF_CopyPasteKontext i_edcKontext)
		{
			m_edcCopyPasteKontext = i_edcKontext;
		}

		public void SUB_SetzePasteElemente(IEnumerable<EDC_EditorElementMitPunkten> i_enuTemporaereElemente)
		{
			m_sttMittelPunkt = FUN_sttMittelPunktErmitteln(m_enuCopyPasteElemente = i_enuTemporaereElemente.ToList());
			m_ssttUrspruenglicherMittelPunkt = m_sttMittelPunkt;
			m_sngRotationRad = 0f;
		}

		public override void SUB_MouseEnter()
		{
			base.SUB_MouseEnter();
			SUB_ElementeAnzeigen();
		}

		public override void SUB_MouseLeave()
		{
			base.SUB_MouseLeave();
			SUB_ElementeVerbergen();
		}

		public override bool FUN_blnMouseMove(Point i_sttPosition, MouseButtonState i_enmLeftButtonState)
		{
			base.FUN_blnMouseMove(i_sttPosition, i_enmLeftButtonState);
			SUB_ElementeAnzeigen();
			SUB_VerschiebeElementeAnPosition(i_sttPosition);
			return true;
		}

		public override bool FUN_blnMouseDown(Point i_sttPosition, MouseButtonState i_enmLeftButtonState, MouseButtonState i_enmRightButtonState)
		{
			base.FUN_blnMouseDown(i_sttPosition, i_enmLeftButtonState, i_enmRightButtonState);
			if (i_enmLeftButtonState == MouseButtonState.Pressed)
			{
				SUB_VerschiebeElementeAnPosition(i_sttPosition);
				if (m_edcCopyPasteKontext?.FUN_blnKannEinfuegen(FUN_delErstelleVerschiebungsFunktion()) ?? false)
				{
					m_edcCopyPasteKontext?.SUB_Einfuegen(FUN_delErstelleVerschiebungsFunktion());
				}
			}
			if (i_enmRightButtonState == MouseButtonState.Pressed)
			{
				SUB_WerkzeugDeaktivierenAnfordern();
			}
			return true;
		}

		public override void SUB_WerkzeugDeaktiviert()
		{
			base.SUB_WerkzeugDeaktiviert();
			SUB_VerschiebeElementeAnPosition(m_ssttUrspruenglicherMittelPunkt);
			SUB_DrehePunkte(0f - m_sngRotationRad);
			m_sngRotationRad = 0f;
			SUB_ElementeVerbergen();
		}

		public override void SUB_PreviewKeyDown(Key i_enmKey)
		{
			base.SUB_PreviewKeyDown(i_enmKey);
			if (i_enmKey == Key.R)
			{
				SUB_DrehePunkte(-1.57079637f);
				m_sngRotationRad += -1.57079637f;
				SUB_VerschiebeElementeAnPosition(m_sttMittelPunkt);
			}
		}

		public override void SUB_SetzeInitialeMausposition(Point i_sttPosition, bool i_blnMausInBildEditor)
		{
			base.SUB_SetzeInitialeMausposition(i_sttPosition, i_blnMausInBildEditor);
			if (i_blnMausInBildEditor)
			{
				FUN_blnMouseMove(i_sttPosition, MouseButtonState.Released);
			}
		}

		private Func<Point, Point> FUN_delErstelleVerschiebungsFunktion()
		{
			Vector sttVerschiebungAnUrsprung = new Vector(0.0 - m_ssttUrspruenglicherMittelPunkt.X, 0.0 - m_ssttUrspruenglicherMittelPunkt.Y);
			Vector sttVerschiebung = m_sttMittelPunkt - m_ssttUrspruenglicherMittelPunkt;
			float sngRotation = m_sngRotationRad;
			return (Point i_sttPoint) => EDC_GeometrieHelfer.FUN_sttPunktDrehenUndVerschieben(i_sttPoint + sttVerschiebungAnUrsprung, -sttVerschiebungAnUrsprung + sttVerschiebung, sngRotation);
		}

		private void SUB_ElementeAnzeigen()
		{
			if (!m_blnElementeAngezeigt)
			{
				foreach (EDC_EditorElementMitPunkten item in m_enuCopyPasteElemente)
				{
					SUB_FuegeElementTemporaerHinzu(item);
				}
				m_blnElementeAngezeigt = true;
			}
		}

		private void SUB_ElementeVerbergen()
		{
			if (m_blnElementeAngezeigt)
			{
				foreach (EDC_EditorElementMitPunkten item in m_enuCopyPasteElemente)
				{
					SUB_EntferneTemporaeresElement(item);
				}
				m_blnElementeAngezeigt = false;
			}
		}

		private void SUB_VerschiebeElementeAnPosition(Point i_sttPosition)
		{
			Vector i_sttVerschiebung = i_sttPosition - m_sttMittelPunkt;
			foreach (EDC_EditorElementMitPunkten item in m_enuCopyPasteElemente)
			{
				IEnumerable<Point> i_enuPunkte = item.FUN_enuHolePunkte();
				IEnumerable<Point> i_enuPunkte2 = FUN_enuVerschiebePunkteAnPosition(i_enuPunkte, i_sttVerschiebung);
				item.SUB_SetzePunkte(i_enuPunkte2);
			}
			m_sttMittelPunkt = i_sttPosition;
		}

		private IEnumerable<Point> FUN_enuVerschiebePunkteAnPosition(IEnumerable<Point> i_enuPunkte, Vector i_sttVerschiebung)
		{
			return from i_edcPoint in i_enuPunkte
			select i_edcPoint + i_sttVerschiebung;
		}

		private void SUB_DrehePunkte(float i_sngDrehung)
		{
			foreach (EDC_EditorElementMitPunkten item in m_enuCopyPasteElemente)
			{
				IEnumerable<Point> i_enuPunkte = item.FUN_enuHolePunkte();
				IEnumerable<Point> i_enuPunkte2 = FUN_enuDrehePunkte(i_enuPunkte, i_sngDrehung);
				item.SUB_SetzePunkte(i_enuPunkte2);
			}
		}

		private IEnumerable<Point> FUN_enuDrehePunkte(IEnumerable<Point> i_enuPunkte, float i_sngDrehung)
		{
			Vector sttVerschiebungAufUrsprung = new Vector(0.0 - m_sttMittelPunkt.X, 0.0 - m_sttMittelPunkt.Y);
			return from i_edcPoint in i_enuPunkte
			select i_edcPoint + sttVerschiebungAufUrsprung into i_sttPoint
			select EDC_GeometrieHelfer.FUN_sttPunktDrehenUndVerschieben(i_sttPoint, -sttVerschiebungAufUrsprung, i_sngDrehung);
		}

		private Point FUN_sttMittelPunktErmitteln(IEnumerable<EDC_EditorElementMitPunkten> i_enuElemente)
		{
			List<Point> source = i_enuElemente.SelectMany((EDC_EditorElementMitPunkten i_edcElement) => i_edcElement.FUN_enuHolePunkte()).ToList();
			return new Point(source.Average((Point i_sttPoint) => i_sttPoint.X), source.Average((Point i_sttPoint) => i_sttPoint.Y));
		}
	}
}
