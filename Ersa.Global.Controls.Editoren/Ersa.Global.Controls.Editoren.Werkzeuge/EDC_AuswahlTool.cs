using Ersa.Global.Controls.Editoren.EditorElemente;
using Ersa.Global.Controls.Editoren.Interfaces;
using Ersa.Global.Controls.Editoren.Interfaces.Intern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Ersa.Global.Controls.Editoren.Werkzeuge
{
	public class EDC_AuswahlTool : EDC_Tool
	{
		private Point? m_sttDragStartPoint;

		private Point? m_sttLetzteBehandelteDragPosition;

		private bool m_blnSelektionStarten;

		private ENUM_Bewegungsaktion m_enmBewegungsaktion;

		private EDC_RechteckElement m_edcAuswahlRahmen;

		private INF_AuswahlKontext m_edcAuswahlKontext;

		private INF_PositionsKontext m_edcPositionsKontext;

		private INF_PunktBearbeitungsKontext m_edcPunktBearbeitungsKontext;

		private INF_PunktVerschiebungsKontext m_edcPunktVerschiebungsKontext;

		private EDC_PunktVerschiebungsDaten m_edcVerschiebungsDaten;

		private Dictionary<Key, ENUM_BewegungsRichtung> m_dicRichtungFuerTasten = new Dictionary<Key, ENUM_BewegungsRichtung>
		{
			{
				Key.Left,
				ENUM_BewegungsRichtung.Links
			},
			{
				Key.Up,
				ENUM_BewegungsRichtung.Oben
			},
			{
				Key.Right,
				ENUM_BewegungsRichtung.Rechts
			},
			{
				Key.Down,
				ENUM_BewegungsRichtung.Unten
			}
		};

		public override bool PRO_blnErlaubeKontextmenue => true;

		[Obsolete("SUB_AlleVerfuegbarenKontexteInitialisieren verwenden")]
		public void SUB_AuswahlKontextInitialisieren(INF_AuswahlKontext i_edcAuswahlKontext)
		{
			m_edcAuswahlKontext = i_edcAuswahlKontext;
		}

		[Obsolete("SUB_AlleVerfuegbarenKontexteInitialisieren verwenden")]
		public void SUB_PositionsKontextInitialisieren(INF_PositionsKontext i_edcPositionsKontext)
		{
			m_edcPositionsKontext = i_edcPositionsKontext;
		}

		[Obsolete("SUB_AlleVerfuegbarenKontexteInitialisieren verwenden")]
		public void SUB_PunktBearbeitungsKontextInitialisieren(INF_PunktBearbeitungsKontext i_edcPunktBearbeitungsKontext)
		{
			m_edcPunktBearbeitungsKontext = i_edcPunktBearbeitungsKontext;
		}

		public override void SUB_AlleVerfuegbarenKontexteInitialisieren(params object[] ia_objKontexte)
		{
			base.SUB_AlleVerfuegbarenKontexteInitialisieren(ia_objKontexte);
			INF_AuswahlKontext iNF_AuswahlKontext = ia_objKontexte.OfType<INF_AuswahlKontext>().FirstOrDefault();
			if (iNF_AuswahlKontext != null)
			{
				m_edcAuswahlKontext = iNF_AuswahlKontext;
			}
			INF_PositionsKontext iNF_PositionsKontext = ia_objKontexte.OfType<INF_PositionsKontext>().FirstOrDefault();
			if (iNF_PositionsKontext != null)
			{
				m_edcPositionsKontext = iNF_PositionsKontext;
			}
			INF_PunktBearbeitungsKontext iNF_PunktBearbeitungsKontext = ia_objKontexte.OfType<INF_PunktBearbeitungsKontext>().FirstOrDefault();
			if (iNF_PunktBearbeitungsKontext != null)
			{
				m_edcPunktBearbeitungsKontext = iNF_PunktBearbeitungsKontext;
			}
		}

		public override bool FUN_blnMouseDown(Point i_sttPosition, MouseButtonState i_enmLeftButtonState, MouseButtonState i_enmRightButtonState)
		{
			base.FUN_blnMouseDown(i_sttPosition, i_enmLeftButtonState, i_enmRightButtonState);
			if (i_enmLeftButtonState == MouseButtonState.Pressed)
			{
				if (FUN_blnInitialisiereVerschiebung(i_sttPosition))
				{
					m_enmBewegungsaktion = ENUM_Bewegungsaktion.PunktVerschieben;
					m_sttDragStartPoint = i_sttPosition;
					m_sttLetzteBehandelteDragPosition = m_sttDragStartPoint;
				}
				else
				{
					m_blnSelektionStarten = true;
				}
			}
			return false;
		}

		public override bool FUN_blnMouseMove(Point i_sttPosition, MouseButtonState i_enmLeftButtonState)
		{
			base.FUN_blnMouseMove(i_sttPosition, i_enmLeftButtonState);
			if (i_enmLeftButtonState == MouseButtonState.Released && m_sttDragStartPoint.HasValue)
			{
				FUN_blnMouseUp(i_sttPosition, i_enmLeftButtonState);
				return false;
			}
			if (i_enmLeftButtonState != MouseButtonState.Pressed)
			{
				return false;
			}
			bool result = false;
			if (m_sttDragStartPoint.HasValue)
			{
				Point value = i_sttPosition;
				Point? sttLetzteBehandelteDragPosition = m_sttLetzteBehandelteDragPosition;
				Vector? vector = value - sttLetzteBehandelteDragPosition;
				switch (m_enmBewegungsaktion)
				{
				case ENUM_Bewegungsaktion.RahmenZiehen:
				{
					if (m_edcAuswahlRahmen == null)
					{
						m_edcAuswahlRahmen = new EDC_RechteckElement
						{
							PRO_fdcInnenFarbe = Brushes.Transparent,
							PRO_sttPosition = i_sttPosition,
							PRO_blnAuswaehlbar = false,
							PRO_i32ZIndex = 45
						};
						SUB_FuegeElementTemporaerHinzu(m_edcAuswahlRahmen);
					}
					double x = Math.Min(i_sttPosition.X, m_sttDragStartPoint.GetValueOrDefault().X);
					double y = Math.Min(i_sttPosition.Y, m_sttDragStartPoint.GetValueOrDefault().Y);
					Vector vector2 = i_sttPosition - m_sttDragStartPoint.GetValueOrDefault();
					m_edcAuswahlRahmen.PRO_sttPosition = new Point(x, y);
					m_edcAuswahlRahmen.SUB_AendereGroesse(new Size(Math.Abs(vector2.X), Math.Abs(vector2.Y)));
					break;
				}
				case ENUM_Bewegungsaktion.ObjekteVerschieben:
					m_sttLetzteBehandelteDragPosition = i_sttPosition;
					foreach (EDC_EditorElement item in FUN_enuHoleAlleAusgewaehltenElemente())
					{
						item.SUB_SetzeTemporaerePosition(item.PRO_sttPosition + vector.GetValueOrDefault());
					}
					break;
				case ENUM_Bewegungsaktion.PunktVerschieben:
					m_sttLetzteBehandelteDragPosition = i_sttPosition;
					SUB_VerschiebePunkt(vector.GetValueOrDefault());
					break;
				}
				result = true;
			}
			else
			{
				m_sttDragStartPoint = i_sttPosition;
				m_sttLetzteBehandelteDragPosition = m_sttDragStartPoint;
				EDC_EditorElement eDC_EditorElement = FUN_edcHoleElementAnPosition(i_sttPosition);
				if (eDC_EditorElement != null && eDC_EditorElement.PRO_blnAusgewaehlt)
				{
					m_enmBewegungsaktion = ENUM_Bewegungsaktion.ObjekteVerschieben;
				}
				else
				{
					m_enmBewegungsaktion = ENUM_Bewegungsaktion.RahmenZiehen;
				}
			}
			m_blnSelektionStarten = false;
			return result;
		}

		public override bool FUN_blnMouseUp(Point i_sttPosition, MouseButtonState i_enmLeftButtonState)
		{
			base.FUN_blnMouseUp(i_sttPosition, i_enmLeftButtonState);
			if (i_enmLeftButtonState != 0)
			{
				return false;
			}
			if (m_sttDragStartPoint.HasValue)
			{
				switch (m_enmBewegungsaktion)
				{
				case ENUM_Bewegungsaktion.RahmenZiehen:
					SUB_SchliesseRahmenZiehenAb();
					break;
				case ENUM_Bewegungsaktion.ObjekteVerschieben:
					SUB_SchliesseObjektVerschiebungAb();
					break;
				case ENUM_Bewegungsaktion.PunktVerschieben:
					SUB_SchliessePunktVerschiebungAb();
					break;
				}
			}
			if (m_blnSelektionStarten)
			{
				SUB_SchliesseEinzelAuswahlAb(i_sttPosition);
			}
			m_blnSelektionStarten = false;
			m_sttDragStartPoint = null;
			m_sttLetzteBehandelteDragPosition = null;
			m_enmBewegungsaktion = ENUM_Bewegungsaktion.Keine;
			return false;
		}

		public override void SUB_MouseLeave()
		{
			base.SUB_MouseLeave();
			FUN_blnMouseUp(m_sttLetzteBehandelteDragPosition.GetValueOrDefault(), MouseButtonState.Released);
		}

		public override void SUB_PreviewKeyDown(Key i_enmKey)
		{
			base.SUB_PreviewKeyDown(i_enmKey);
			if (i_enmKey == Key.Delete)
			{
				List<EDC_EditorElement> i_enuElemente = FUN_enuHoleAlleAusgewaehltenElemente().ToList();
				SUB_AuswahlAufheben();
				m_edcAuswahlKontext?.SUB_EntferneElemente(i_enuElemente);
			}
			else if (m_dicRichtungFuerTasten.ContainsKey(i_enmKey))
			{
				IEnumerable<EDC_EditorElement> elements = FUN_enuHoleAlleAusgewaehltenElemente();
				ENUM_BewegungsRichtung direction = m_dicRichtungFuerTasten[i_enmKey];
				m_edcAuswahlKontext?.SUB_BewegeElemente(elements, direction);
			}
		}

		public override void SUB_WerkzeugDeaktiviert()
		{
			base.SUB_WerkzeugDeaktiviert();
			SUB_AuswahlAufheben();
		}

		internal virtual void SUB_PunktVerschiebungsKontextInitialisieren(INF_PunktVerschiebungsKontext i_edcPunktVerschiebungsKontext)
		{
			m_edcPunktVerschiebungsKontext = i_edcPunktVerschiebungsKontext;
		}

		private void SUB_SchliesseEinzelAuswahlAb(Point i_sttPosition)
		{
			if (!Keyboard.IsKeyDown(Key.LeftCtrl) && !Keyboard.IsKeyDown(Key.RightCtrl))
			{
				SUB_AuswahlAufheben();
			}
			EDC_EditorElement eDC_EditorElement = FUN_edcHoleElementAnPosition(i_sttPosition);
			if (eDC_EditorElement != null && eDC_EditorElement.PRO_blnAuswaehlbar)
			{
				eDC_EditorElement.PRO_blnAusgewaehlt = true;
				m_edcAuswahlKontext?.SUB_AuswahlGeaendert(new EDC_EditorElement[1]
				{
					eDC_EditorElement
				});
			}
			else if (eDC_EditorElement == null)
			{
				m_edcPositionsKontext?.SUB_BehandleKlickAnPositionOhneElement(i_sttPosition);
			}
		}

		private void SUB_SchliesseRahmenZiehenAb()
		{
			if (m_edcAuswahlRahmen != null)
			{
				if (!Keyboard.IsKeyDown(Key.LeftCtrl) && !Keyboard.IsKeyDown(Key.RightCtrl))
				{
					SUB_AuswahlAufheben();
				}
				Rect i_sttBereich = m_edcAuswahlRahmen.FUN_sttErmittleBereich();
				List<EDC_EditorElement> list = (from i_edcElement in FUN_enuHoleElementeInBereich(i_sttBereich)
				where i_edcElement.PRO_blnAuswaehlbar
				select i_edcElement).ToList();
				foreach (EDC_EditorElement item in list)
				{
					item.PRO_blnAusgewaehlt = true;
				}
				m_edcAuswahlKontext?.SUB_AuswahlGeaendert(list);
				SUB_EntferneTemporaeresElement(m_edcAuswahlRahmen);
				m_edcAuswahlRahmen = null;
			}
		}

		private void SUB_SchliesseObjektVerschiebungAb()
		{
			EDC_EditorElement[] array = FUN_enuHoleAlleAusgewaehltenElemente().ToArray();
			if (m_edcPositionsKontext?.FUN_blnDarfElementeVerschieben(array) ?? true)
			{
				m_edcPositionsKontext?.SUB_EditorPositionenGeaendert(array);
				EDC_EditorElement[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					array2[i].SUB_UebernehmeTemporaerePosition();
				}
			}
			else
			{
				EDC_EditorElement[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					array2[i].SUB_VerwerfeTemporaerePosition();
				}
			}
		}

		private bool FUN_blnInitialisiereVerschiebung(Point i_sttPosition)
		{
			if (m_edcPunktVerschiebungsKontext == null)
			{
				return false;
			}
			m_edcVerschiebungsDaten = m_edcPunktVerschiebungsKontext.FUN_edcHolePunktVerschiebungsDaten(i_sttPosition);
			return m_edcVerschiebungsDaten != null;
		}

		private void SUB_VerschiebePunkt(Vector i_sttDelta)
		{
			if (m_edcVerschiebungsDaten?.PRO_edcElement != null)
			{
				m_edcVerschiebungsDaten.PRO_objPunktReferenz = m_edcVerschiebungsDaten.PRO_edcElement.FUN_objPunktVerschieben(m_edcVerschiebungsDaten.PRO_objPunktReferenz, i_sttDelta);
			}
		}

		private void SUB_SchliessePunktVerschiebungAb()
		{
			if (m_edcVerschiebungsDaten != null && m_edcPunktBearbeitungsKontext != null)
			{
				if (m_edcPunktBearbeitungsKontext.FUN_blnVeraenderungValidieren(m_edcVerschiebungsDaten))
				{
					m_edcPunktBearbeitungsKontext.SUB_EditorPunkteGeaendert(m_edcVerschiebungsDaten);
				}
				else
				{
					SUB_VeraenderungRueckgaengigMachen(m_edcVerschiebungsDaten);
				}
				m_edcVerschiebungsDaten = null;
			}
		}

		private void SUB_VeraenderungRueckgaengigMachen(EDC_PunktVerschiebungsDaten i_edcVerschiebungsDaten)
		{
			i_edcVerschiebungsDaten.PRO_edcElement.SUB_SetzePunkte(i_edcVerschiebungsDaten.PRO_lstOriginalPunkte);
		}

		private void SUB_AuswahlAufheben()
		{
			List<EDC_EditorElement> list = FUN_enuHoleAlleAusgewaehltenElemente().ToList();
			foreach (EDC_EditorElement item in list)
			{
				item.PRO_blnAusgewaehlt = false;
			}
			m_edcAuswahlKontext?.SUB_AuswahlGeaendert(list);
		}
	}
}
