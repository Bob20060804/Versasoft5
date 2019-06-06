using Ersa.Global.Controls.BildEditor.Commands;
using Ersa.Global.Controls.BildEditor.Converter;
using Ersa.Global.Controls.BildEditor.Eigenschaften;
using Ersa.Global.Controls.BildEditor.Exceptions;
using Ersa.Global.Controls.BildEditor.Grafik;
using Ersa.Global.Controls.BildEditor.Helfer;
using Ersa.Global.Controls.BildEditor.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml.Serialization;

namespace Ersa.Global.Controls.BildEditor
{
	public class EDC_BildEditorCanvas : Canvas
	{
		public static readonly DependencyProperty PRO_enuEditorToolProperty;

		public static readonly DependencyProperty PRO_dblSkalierungProperty;

		public static readonly DependencyProperty PRO_blnIstDirtyProperty;

		public static readonly DependencyProperty PRO_dblStrichStaerkeProperty;

		public static readonly DependencyProperty PRO_fdcGrafikFarbeProperty;

		public static readonly DependencyProperty PRO_strTextFontFamilyNameProperty;

		public static readonly DependencyProperty PRO_fdcTextFontStyleProperty;

		public static readonly DependencyProperty PRO_fdcTextFontWeightProperty;

		public static readonly DependencyProperty PRO_fdcTextFontStretchProperty;

		public static readonly DependencyProperty PRO_fdcTextFontSizeProperty;

		public static readonly DependencyProperty PRO_blnCanUndoProperty;

		public static readonly DependencyProperty PRO_blnCanRedoProperty;

		public static readonly DependencyProperty PRO_blnCanSelectAllProperty;

		public static readonly DependencyProperty PRO_blnCanUnselectAllProperty;

		public static readonly DependencyProperty PRO_blnCanDeleteProperty;

		public static readonly DependencyProperty PRO_blnCanDeleteAllProperty;

		public static readonly DependencyProperty PRO_blnCanMoveToFrontProperty;

		public static readonly DependencyProperty PRO_blnCanMoveToBackProperty;

		public static readonly DependencyProperty PRO_blnCanSetPropertiesProperty;

		private readonly EDC_AbstractTool[] ma_edcGrafikTools;

		private readonly EDC_TextTool m_edcTextTool;

		private readonly EDC_ComboxTool m_edcComboxTool;

		private readonly EDC_AuswahlTool m_edcAuswahlTool;

		private readonly EDC_HistoryController m_edcHistoryController;

		private ContextMenu m_fdcContextMenu;

		private VisualCollection m_lstGrafikListe;

		public ScrollViewer PRO_fdcScrollView
		{
			get;
			set;
		}

		public bool PRO_blnToolAutoReset
		{
			get;
			set;
		}

		public bool PRO_blnDisableContextMenu
		{
			get;
			set;
		}

		public bool PRO_blnSingleGrafikModus
		{
			get;
			set;
		}

		public bool PRO_blnRechteckZoomModus
		{
			get;
			set;
		}

		public Action<EDC_GrafikBasisObjekt> PRO_delNeueGrafikWurdeErstelltAction
		{
			get;
			set;
		}

		public Action<EDC_GrafikBasisObjekt> PRO_delGrafikWurdeSelektiertAction
		{
			get;
			set;
		}

		public Action<EDC_GrafikBasisObjekt> PRO_delGrafikWurdeVeraendertAction
		{
			get;
			set;
		}

		public Action<EDC_GrafikBasisObjekt> PRO_delRechteckZoomAction
		{
			get;
			set;
		}

		public Action<string> PRO_delNeuerEditorZustandAction
		{
			get;
			set;
		}

		public Action PRO_delBearbeitetHinweisAction
		{
			get;
			set;
		}

		public ENUM_EditorToolType PRO_enuEditorTool
		{
			get
			{
				return (ENUM_EditorToolType)GetValue(PRO_enuEditorToolProperty);
			}
			set
			{
				if (value >= ENUM_EditorToolType.Kein && value < ENUM_EditorToolType.Max)
				{
					SetValue(PRO_enuEditorToolProperty, value);
					ma_edcGrafikTools[(int)PRO_enuEditorTool].SUB_SetzeCursor(this);
				}
			}
		}

		public double PRO_dblSkalierung
		{
			get
			{
				return (double)GetValue(PRO_dblSkalierungProperty);
			}
			set
			{
				SetValue(PRO_dblSkalierungProperty, value);
			}
		}

		public bool PRO_blnIstDirty
		{
			get
			{
				return (bool)GetValue(PRO_blnIstDirtyProperty);
			}
			set
			{
				SetValue(PRO_blnIstDirtyProperty, value);
				if (PRO_delBearbeitetHinweisAction != null)
				{
					PRO_delBearbeitetHinweisAction();
				}
			}
		}

		public bool PRO_blnCanUndo
		{
			get
			{
				return (bool)GetValue(PRO_blnCanUndoProperty);
			}
			private set
			{
				SetValue(PRO_blnCanUndoProperty, value);
			}
		}

		public bool PRO_blnCanRedo
		{
			get
			{
				return (bool)GetValue(PRO_blnCanRedoProperty);
			}
			private set
			{
				SetValue(PRO_blnCanRedoProperty, value);
			}
		}

		public bool PRO_blnCanSelectAll
		{
			get
			{
				return (bool)GetValue(PRO_blnCanSelectAllProperty);
			}
			private set
			{
				SetValue(PRO_blnCanSelectAllProperty, value);
			}
		}

		public bool PRO_blnCanUnselectAll
		{
			get
			{
				return (bool)GetValue(PRO_blnCanUnselectAllProperty);
			}
			private set
			{
				SetValue(PRO_blnCanUnselectAllProperty, value);
			}
		}

		public bool PRO_blnCanDelete
		{
			get
			{
				return (bool)GetValue(PRO_blnCanDeleteProperty);
			}
			private set
			{
				SetValue(PRO_blnCanDeleteProperty, value);
			}
		}

		public bool PRO_blnCanDeleteAll
		{
			get
			{
				return (bool)GetValue(PRO_blnCanDeleteAllProperty);
			}
			private set
			{
				SetValue(PRO_blnCanDeleteAllProperty, value);
			}
		}

		public bool PRO_blnCanMoveToFront
		{
			get
			{
				return (bool)GetValue(PRO_blnCanMoveToFrontProperty);
			}
			private set
			{
				SetValue(PRO_blnCanMoveToFrontProperty, value);
			}
		}

		public bool PRO_blnCanMoveToBack
		{
			get
			{
				return (bool)GetValue(PRO_blnCanMoveToBackProperty);
			}
			private set
			{
				SetValue(PRO_blnCanMoveToBackProperty, value);
			}
		}

		public bool PRO_blnCanSetProperties
		{
			get
			{
				return (bool)GetValue(PRO_blnCanSetPropertiesProperty);
			}
			private set
			{
				SetValue(PRO_blnCanSetPropertiesProperty, value);
			}
		}

		public double PRO_dblStrichStaerke
		{
			get
			{
				return (double)GetValue(PRO_dblStrichStaerkeProperty);
			}
			set
			{
				SetValue(PRO_dblStrichStaerkeProperty, value);
			}
		}

		public Color PRO_fdcGrafikFarbe
		{
			get
			{
				return (Color)GetValue(PRO_fdcGrafikFarbeProperty);
			}
			set
			{
				SetValue(PRO_fdcGrafikFarbeProperty, value);
			}
		}

		public string PRO_strTextFontFamilyName
		{
			get
			{
				return (string)GetValue(PRO_strTextFontFamilyNameProperty);
			}
			set
			{
				SetValue(PRO_strTextFontFamilyNameProperty, value);
			}
		}

		public FontStyle PRO_fdcTextFontStyle
		{
			get
			{
				return (FontStyle)GetValue(PRO_fdcTextFontStyleProperty);
			}
			set
			{
				SetValue(PRO_fdcTextFontStyleProperty, value);
			}
		}

		public FontWeight PRO_fdcTextFontWeight
		{
			get
			{
				return (FontWeight)GetValue(PRO_fdcTextFontWeightProperty);
			}
			set
			{
				SetValue(PRO_fdcTextFontWeightProperty, value);
			}
		}

		public FontStretch PRO_fdcTextFontStretch
		{
			get
			{
				return (FontStretch)GetValue(PRO_fdcTextFontStretchProperty);
			}
			set
			{
				SetValue(PRO_fdcTextFontStretchProperty, value);
			}
		}

		public double PRO_fdcTextFontSize
		{
			get
			{
				return (double)GetValue(PRO_fdcTextFontSizeProperty);
			}
			set
			{
				SetValue(PRO_fdcTextFontSizeProperty, value);
			}
		}

		internal int PRO_i32GrafikAnzahl => m_lstGrafikListe.Count;

		internal VisualCollection PRO_lstGrafikliste => m_lstGrafikListe;

		internal IEnumerable<EDC_GrafikBasisObjekt> PRO_enuGrafikSelktionsliste => from EDC_GrafikBasisObjekt edcGrafik in m_lstGrafikListe
		where edcGrafik.PRO_blnIstSelektiert
		select edcGrafik;

		protected override int VisualChildrenCount
		{
			get
			{
				int num = m_lstGrafikListe.Count;
				if (m_edcTextTool.PRO_fdcTextBox != null || m_edcComboxTool.PRO_fdcComboBox != null)
				{
					num++;
				}
				return num;
			}
		}

		private int PRO_i32AnzahlSelektiert => m_lstGrafikListe.Cast<EDC_GrafikBasisObjekt>().Count((EDC_GrafikBasisObjekt i_edcGrafik) => i_edcGrafik.PRO_blnIstSelektiert);

		private Point PRO_fdcLetztePanningPosition
		{
			get;
			set;
		}

		private Point PRO_fdcLetzteZoomingPosition
		{
			get;
			set;
		}

		internal EDC_GrafikBasisObjekt this[int i_i32Index]
		{
			get
			{
				if (i_i32Index >= 0 && i_i32Index < PRO_i32GrafikAnzahl)
				{
					return (EDC_GrafikBasisObjekt)m_lstGrafikListe[i_i32Index];
				}
				return null;
			}
		}

		static EDC_BildEditorCanvas()
		{
			PropertyMetadata typeMetadata = new PropertyMetadata(ENUM_EditorToolType.Auswahl);
			PRO_enuEditorToolProperty = DependencyProperty.Register("PRO_enuEditorTool", typeof(ENUM_EditorToolType), typeof(EDC_BildEditorCanvas), typeMetadata);
			typeMetadata = new PropertyMetadata(1.0, SUB_SkalierungWurdeGeaendert);
			PRO_dblSkalierungProperty = DependencyProperty.Register("PRO_dblSkalierung", typeof(double), typeof(EDC_BildEditorCanvas), typeMetadata);
			typeMetadata = new PropertyMetadata(false);
			PRO_blnIstDirtyProperty = DependencyProperty.Register("PRO_blnIstDirty", typeof(bool), typeof(EDC_BildEditorCanvas), typeMetadata);
			typeMetadata = new PropertyMetadata(2.0, SUB_StrichStaerkeWurdeGeaendert);
			PRO_dblStrichStaerkeProperty = DependencyProperty.Register("PRO_dblStrichStaerke", typeof(double), typeof(EDC_BildEditorCanvas), typeMetadata);
			typeMetadata = new PropertyMetadata(EDC_BildEditorKonstanten.ms_fdcDefaultGrafikFarbe, SUB_FarbeWurdeGeaendert);
			PRO_fdcGrafikFarbeProperty = DependencyProperty.Register("PRO_fdcGrafikFarbe", typeof(Color), typeof(EDC_BildEditorCanvas), typeMetadata);
			typeMetadata = new PropertyMetadata("Arial", SUB_TextFontWurdeGeaendert);
			PRO_strTextFontFamilyNameProperty = DependencyProperty.Register("PRO_strFontFamilyName", typeof(string), typeof(EDC_BildEditorCanvas), typeMetadata);
			typeMetadata = new PropertyMetadata(EDC_BildEditorKonstanten.ms_fdcDefaultFontStyle, SUB_FontStyleWurdeGeaendert);
			PRO_fdcTextFontStyleProperty = DependencyProperty.Register("PRO_fdcFontStyle", typeof(FontStyle), typeof(EDC_BildEditorCanvas), typeMetadata);
			typeMetadata = new PropertyMetadata(EDC_BildEditorKonstanten.ms_fdcDefaultFontWeight, SUB_FontTextWeightWurdeGeaendert);
			PRO_fdcTextFontWeightProperty = DependencyProperty.Register("PRO_fdcFontWeight", typeof(FontWeight), typeof(EDC_BildEditorCanvas), typeMetadata);
			typeMetadata = new PropertyMetadata(EDC_BildEditorKonstanten.ms_fdcDefaultFontStretch, SUB_FontStretchWurdeGeaendert);
			PRO_fdcTextFontStretchProperty = DependencyProperty.Register("PRO_fdcFontStretch", typeof(FontStretch), typeof(EDC_BildEditorCanvas), typeMetadata);
			typeMetadata = new PropertyMetadata(20.0, SUB_FontSizeWurdeGeaendert);
			PRO_fdcTextFontSizeProperty = DependencyProperty.Register("PRO_dblFontSize", typeof(double), typeof(EDC_BildEditorCanvas), typeMetadata);
			typeMetadata = new PropertyMetadata(false);
			PRO_blnCanUndoProperty = DependencyProperty.Register("PRO_blnCanUndo", typeof(bool), typeof(EDC_BildEditorCanvas), typeMetadata);
			typeMetadata = new PropertyMetadata(false);
			PRO_blnCanRedoProperty = DependencyProperty.Register("PRO_blnCanRedo", typeof(bool), typeof(EDC_BildEditorCanvas), typeMetadata);
			typeMetadata = new PropertyMetadata(false);
			PRO_blnCanSelectAllProperty = DependencyProperty.Register("PRO_blnCanSelectAll", typeof(bool), typeof(EDC_BildEditorCanvas), typeMetadata);
			typeMetadata = new PropertyMetadata(false);
			PRO_blnCanUnselectAllProperty = DependencyProperty.Register("PRO_blnCanUnselectAll", typeof(bool), typeof(EDC_BildEditorCanvas), typeMetadata);
			typeMetadata = new PropertyMetadata(false);
			PRO_blnCanDeleteProperty = DependencyProperty.Register("PRO_blnCanDelete", typeof(bool), typeof(EDC_BildEditorCanvas), typeMetadata);
			typeMetadata = new PropertyMetadata(false);
			PRO_blnCanDeleteAllProperty = DependencyProperty.Register("PRO_blnCanDeleteAll", typeof(bool), typeof(EDC_BildEditorCanvas), typeMetadata);
			typeMetadata = new PropertyMetadata(false);
			PRO_blnCanMoveToFrontProperty = DependencyProperty.Register("PRO_blnCanMoveToFront", typeof(bool), typeof(EDC_BildEditorCanvas), typeMetadata);
			typeMetadata = new PropertyMetadata(false);
			PRO_blnCanMoveToBackProperty = DependencyProperty.Register("PRO_blnCanMoveToBack", typeof(bool), typeof(EDC_BildEditorCanvas), typeMetadata);
			typeMetadata = new PropertyMetadata(false);
			PRO_blnCanSetPropertiesProperty = DependencyProperty.Register("PRO_blnCanSetProperties", typeof(bool), typeof(EDC_BildEditorCanvas), typeMetadata);
		}

		public EDC_BildEditorCanvas()
		{
			m_lstGrafikListe = new VisualCollection(this);
			SUB_ErstelleDasContextMenu();
			ma_edcGrafikTools = new EDC_AbstractTool[9];
			m_edcAuswahlTool = new EDC_AuswahlTool();
			ma_edcGrafikTools[1] = m_edcAuswahlTool;
			ma_edcGrafikTools[2] = new EDC_RechteckTool();
			ma_edcGrafikTools[3] = new EDC_EllipseTool();
			ma_edcGrafikTools[4] = new EDC_LinienTool();
			ma_edcGrafikTools[5] = new EDC_MehrfachLinienTool();
			m_edcTextTool = new EDC_TextTool();
			ma_edcGrafikTools[8] = new EDC_PunktTool();
			ma_edcGrafikTools[6] = m_edcTextTool;
			m_edcComboxTool = new EDC_ComboxTool();
			ma_edcGrafikTools[7] = m_edcComboxTool;
			m_edcHistoryController = new EDC_HistoryController(this);
			m_edcHistoryController.StateChanged += SUB_HistoryControllerStateChanged;
			base.FocusVisualStyle = null;
			base.Loaded += SUB_BildEditorLoaded;
			base.MouseDown += SUB_BildEditorMouseDown;
			base.MouseMove += SUB_BildEditorMouseMove;
			base.MouseUp += SUB_BildEditorMouseUp;
			base.KeyDown += SUB_BildEditorKeyDown;
			base.LostMouseCapture += SUB_BildEditorLostMouseCapture;
		}

		public void SUB_SetzeCombobxTextListe(List<string> i_lstTexte)
		{
			if (m_edcComboxTool != null)
			{
				m_edcComboxTool.PRO_lstTexte = i_lstTexte;
			}
		}

		public EDC_GrafikEigenschaften[] FUNa_edcHoleGrafikEigenschaftenliste()
		{
			EDC_GrafikEigenschaften[] array = new EDC_GrafikEigenschaften[m_lstGrafikListe.Count];
			int num = 0;
			VisualCollection.Enumerator enumerator = m_lstGrafikListe.GetEnumerator();
			while (enumerator.MoveNext())
			{
				EDC_GrafikBasisObjekt eDC_GrafikBasisObjekt = (EDC_GrafikBasisObjekt)enumerator.Current;
				array[num++] = eDC_GrafikBasisObjekt.FUN_edcSerialisiereObjekt();
			}
			return array;
		}

		public void SUB_ZeichneGrafikenIndenContext(DrawingContext i_fdcDrawingContext, bool i_blnMitAuswahl = false)
		{
			EDC_GrafikConverter.SUB_ZeichneInDenContext(m_lstGrafikListe, i_fdcDrawingContext, i_blnMitAuswahl);
		}

		public void SUB_AllesZurueckstellen()
		{
			m_lstGrafikListe.Clear();
			SUB_LoescheHistory();
			SUB_AktualisiereStatus();
		}

		public IEnumerable<EDC_GrafikBasisObjekt> FUN_enuHoleGetroffeneObjekte(Point fdcPosition, Func<EDC_GrafikBasisObjekt, Point, bool> delTrefferBedingung)
		{
			if (delTrefferBedingung == null)
			{
				delTrefferBedingung = ((EDC_GrafikBasisObjekt i_edcGrafik, Point i_fdcPoint) => i_edcGrafik.FUN_i32MacheTrefferTest(i_fdcPoint) >= 0);
			}
			for (int i32Index = m_lstGrafikListe.Count - 1; i32Index >= 0; i32Index--)
			{
				EDC_GrafikBasisObjekt eDC_GrafikBasisObjekt = this[i32Index];
				if (delTrefferBedingung(eDC_GrafikBasisObjekt, fdcPosition))
				{
					yield return eDC_GrafikBasisObjekt;
				}
			}
		}

		public string FUN_strErstelleXmlAusAllenGrafiken()
		{
			return EDC_GrafikConverter.FUN_strErstelleXmlVonGrafiken(m_lstGrafikListe);
		}

		public void SUB_ErstelleGrafikenAusXml(string i_strXml, double i_dblBreite, double i_dblHoehe)
		{
			try
			{
				m_lstGrafikListe = EDC_GrafikConverter.FUN_lstErstelleGrafikListeVonXml(this, i_strXml);
			}
			catch (Exception)
			{
			}
			SUB_AuschnittAktualisieren(i_dblBreite, i_dblHoehe);
			SUB_LoescheHistory();
			SUB_AktualisiereStatus();
		}

		public void SUB_SpeichereGrafikenInDatei(string i_strDateiname)
		{
			try
			{
				EDC_GrafikSerialisierer o = new EDC_GrafikSerialisierer(m_lstGrafikListe);
				XmlSerializer xmlSerializer = new XmlSerializer(typeof(EDC_GrafikSerialisierer));
				using (Stream stream = new FileStream(i_strDateiname, FileMode.Create, FileAccess.Write, FileShare.None))
				{
					xmlSerializer.Serialize(stream, o);
					SUB_LoescheHistory();
					SUB_AktualisiereStatus();
				}
			}
			catch (IOException ex)
			{
				throw new EDC_BildEditorCanvasException(ex.Message, ex);
			}
			catch (InvalidOperationException ex2)
			{
				throw new EDC_BildEditorCanvasException(ex2.Message, ex2);
			}
		}

		public void SUB_LadeGrafikenAusDatei(string i_strDateiName)
		{
			try
			{
				XmlSerializer xmlSerializer = new XmlSerializer(typeof(EDC_GrafikSerialisierer));
				EDC_GrafikSerialisierer eDC_GrafikSerialisierer;
				using (Stream stream = new FileStream(i_strDateiName, FileMode.Open, FileAccess.Read, FileShare.Read))
				{
					eDC_GrafikSerialisierer = (EDC_GrafikSerialisierer)xmlSerializer.Deserialize(stream);
				}
				if (eDC_GrafikSerialisierer.SUB_GrafikObjekteArray == null)
				{
					throw new EDC_BildEditorCanvasException("Die XML Datei hat nicht die gewünschten Informationen");
				}
				m_lstGrafikListe.Clear();
				EDC_GrafikEigenschaften[] sUB_GrafikObjekteArray = eDC_GrafikSerialisierer.SUB_GrafikObjekteArray;
				foreach (EDC_GrafikEigenschaften eDC_GrafikEigenschaften in sUB_GrafikObjekteArray)
				{
					m_lstGrafikListe.Add(eDC_GrafikEigenschaften.FUN_edcErstelleGrafikObjekt());
				}
				SUB_AuschnittAktualisieren();
				SUB_LoescheHistory();
				SUB_AktualisiereStatus();
			}
			catch (IOException ex)
			{
				throw new EDC_BildEditorCanvasException(ex.Message, ex);
			}
			catch (InvalidOperationException ex2)
			{
				throw new EDC_BildEditorCanvasException(ex2.Message, ex2);
			}
			catch (ArgumentNullException ex3)
			{
				throw new EDC_BildEditorCanvasException(ex3.Message, ex3);
			}
		}

		public void SUB_SelektiereAlleGrafiken()
		{
			this.SUB_SelektiereAlleGrafikObjekte();
			SUB_AktualisiereStatus();
		}

		public void SUB_SelektionAufheben()
		{
			this.SUB_UnselektiereAlleGrafikObjekte();
			SUB_AktualisiereStatus();
		}

		public void SUB_LoescheGrafikAuswahl()
		{
			this.SUB_LoescheAuswahl();
			SUB_AktualisiereStatus();
		}

		public void SUB_LoescheGrafik(EDC_GrafikBasisObjekt i_edcGrafik)
		{
			this.SUB_LoescheDieGrafik(i_edcGrafik);
			SUB_AktualisiereStatus();
		}

		public void SUB_LoescheAlleGrafikobjekte()
		{
			this.SUB_LoescheAlleGrafikObjekte();
			SUB_AktualisiereStatus();
		}

		public void SUB_BringeGrafikauswahlInDenVordergrund()
		{
			this.SUB_BringeAuswahlInVordergrund();
			SUB_AktualisiereStatus();
		}

		public void SUB_BringeGrafikauswahlInDenHintergrund()
		{
			this.SUB_BringeAuswahlInHintergrund();
			SUB_AktualisiereStatus();
		}

		public void SUB_SetzeNeueEigenschaftenInAuswahl()
		{
			this.SUB_SetzeEigenschaften();
			SUB_AktualisiereStatus();
		}

		public void SUB_AuschnittAktualisieren()
		{
			VisualCollection.Enumerator enumerator = m_lstGrafikListe.GetEnumerator();
			while (enumerator.MoveNext())
			{
				EDC_GrafikBasisObjekt obj = (EDC_GrafikBasisObjekt)enumerator.Current;
				obj.Clip = new RectangleGeometry(new Rect(0.0, 0.0, base.ActualWidth, base.ActualHeight));
				obj.PRO_dblSkalierung = PRO_dblSkalierung;
			}
		}

		public void SUB_AuschnittAktualisieren(double i_dblBreite, double i_dblHoehe)
		{
			VisualCollection.Enumerator enumerator = m_lstGrafikListe.GetEnumerator();
			while (enumerator.MoveNext())
			{
				EDC_GrafikBasisObjekt obj = (EDC_GrafikBasisObjekt)enumerator.Current;
				obj.Clip = new RectangleGeometry(new Rect(0.0, 0.0, i_dblBreite, i_dblHoehe));
				obj.PRO_dblSkalierung = PRO_dblSkalierung;
			}
		}

		public void SUB_AuschnittAufheben()
		{
			VisualCollection.Enumerator enumerator = m_lstGrafikListe.GetEnumerator();
			while (enumerator.MoveNext())
			{
				((EDC_GrafikBasisObjekt)enumerator.Current).Clip = null;
			}
		}

		public void SUB_Undo()
		{
			m_edcHistoryController.Undo();
			SUB_AktualisiereStatus();
		}

		public void SUB_Redo()
		{
			m_edcHistoryController.Redo();
			SUB_AktualisiereStatus();
		}

		public void SUB_FuegeNeuesGrafikObjektHinzu(EDC_GrafikBasisObjekt i_edcGrafikObjekt)
		{
			if (PRO_blnSingleGrafikModus)
			{
				SUB_LoescheAlleGrafikobjekte();
			}
			PRO_lstGrafikliste.Add(i_edcGrafikObjekt);
			CaptureMouse();
			if (PRO_delNeueGrafikWurdeErstelltAction != null)
			{
				PRO_delNeueGrafikWurdeErstelltAction(i_edcGrafikObjekt);
			}
		}

		public void SUB_FuegeCommandZuHistoryHinzu(EDC_GrafikBasisObjekt i_edcGrafik)
		{
			m_edcHistoryController.AddCommandToHistory(new EDC_BildEditorCommandAdd(i_edcGrafik));
		}

		public void SUB_BildEditorKeyDown(object i_fdcSender, KeyEventArgs i_fdcArgs)
		{
			if (i_fdcArgs.Key == Key.Escape && base.IsMouseCaptured)
			{
				SUB_BrecheAktuelleOperationAb();
				SUB_AktualisiereStatus();
			}
			if (i_fdcArgs.Key == Key.Delete)
			{
				SUB_LoescheGrafikAuswahl();
			}
		}

		public void SUB_ZentriereDenViewport(double i_dblVorzeichen)
		{
			ScrollBar scrollBar = PRO_fdcScrollView.Template.FindName("PART_VerticalScrollBar", PRO_fdcScrollView) as ScrollBar;
			ScrollBar scrollBar2 = PRO_fdcScrollView.Template.FindName("PART_HorizontalScrollBar", PRO_fdcScrollView) as ScrollBar;
			double num = PRO_fdcScrollView.ContentHorizontalOffset + scrollBar.ActualWidth * i_dblVorzeichen;
			double num2 = PRO_fdcScrollView.ContentVerticalOffset + scrollBar2.ActualHeight * i_dblVorzeichen;
			if (num < 0.0)
			{
				num = 0.0;
			}
			if (num > PRO_fdcScrollView.ExtentWidth - PRO_fdcScrollView.ViewportWidth)
			{
				num = PRO_fdcScrollView.ExtentWidth - PRO_fdcScrollView.ViewportWidth;
			}
			if (num2 < 0.0)
			{
				num2 = 0.0;
			}
			if (num2 > PRO_fdcScrollView.ExtentHeight - PRO_fdcScrollView.ViewportHeight)
			{
				num2 = PRO_fdcScrollView.ExtentHeight - PRO_fdcScrollView.ViewportHeight;
			}
			PRO_fdcScrollView.ScrollToVerticalOffset(num2);
			PRO_fdcScrollView.ScrollToHorizontalOffset(num);
		}

		public void SUB_VerschiebeViewPort(Rect i_fdcView)
		{
			PRO_fdcScrollView.ScrollToHorizontalOffset(i_fdcView.Left);
			PRO_fdcScrollView.ScrollToVerticalOffset(i_fdcView.Top);
		}

		public void SUB_VerschiebeViewPort(Point i_fdcPunkt)
		{
			PRO_fdcScrollView.ScrollToHorizontalOffset(i_fdcPunkt.X);
			PRO_fdcScrollView.ScrollToVerticalOffset(i_fdcPunkt.Y);
		}

		internal void SUB_FuegeCommandZuHistoryHinzu(EDC_BildEditorCommandBase i_blnBildEditorCommand)
		{
			m_edcHistoryController.AddCommandToHistory(i_blnBildEditorCommand);
		}

		internal void SUB_BlendeTextboxAus(EDC_TextGrafik i_blnTextGrafik)
		{
			if (m_edcTextTool.PRO_fdcTextBox == null)
			{
				return;
			}
			i_blnTextGrafik.PRO_blnIstSelektiert = true;
			if (m_edcTextTool.PRO_fdcTextBox.Text.Trim().Length == 0)
			{
				if (!string.IsNullOrEmpty(m_edcTextTool.PRO_strAlterText))
				{
					m_edcHistoryController.AddCommandToHistory(new EDC_BildEditorCommandDelete(this));
				}
				m_lstGrafikListe.Remove(i_blnTextGrafik);
			}
			else if (!string.IsNullOrEmpty(m_edcTextTool.PRO_strAlterText))
			{
				if (m_edcTextTool.PRO_fdcTextBox.Text.Trim() != m_edcTextTool.PRO_strAlterText)
				{
					EDC_BildEditorCommandChangeState eDC_BildEditorCommandChangeState = new EDC_BildEditorCommandChangeState(this);
					i_blnTextGrafik.PRO_strText = m_edcTextTool.PRO_fdcTextBox.Text.Trim();
					i_blnTextGrafik.SUB_AktualisiereDasRechteck();
					eDC_BildEditorCommandChangeState.SUB_NeuerZustand(this);
					m_edcHistoryController.AddCommandToHistory(eDC_BildEditorCommandChangeState);
				}
			}
			else
			{
				i_blnTextGrafik.PRO_strText = m_edcTextTool.PRO_fdcTextBox.Text.Trim();
				i_blnTextGrafik.SUB_AktualisiereDasRechteck();
				m_edcHistoryController.AddCommandToHistory(new EDC_BildEditorCommandAdd(i_blnTextGrafik));
			}
			base.Children.Remove(m_edcTextTool.PRO_fdcTextBox);
			m_edcTextTool.PRO_fdcTextBox.PreviewKeyDown -= m_edcTextTool.SUB_TextBoxPreviewKeyDown;
			m_edcTextTool.PRO_fdcTextBox.Loaded -= m_edcTextTool.SUB_TextboxWurdeGeladen;
			m_edcTextTool.PRO_fdcTextBox = null;
			Focus();
		}

		internal void SUB_BlendeComboboxAus(EDC_TextGrafik i_blnTextGrafik)
		{
			if (m_edcComboxTool.PRO_fdcComboBox != null)
			{
				i_blnTextGrafik.PRO_blnIstSelektiert = true;
				if (i_blnTextGrafik.PRO_strText.Trim().Length == 0)
				{
					m_edcHistoryController.AddCommandToHistory(new EDC_BildEditorCommandDelete(this));
					m_lstGrafikListe.Remove(i_blnTextGrafik);
				}
				else
				{
					i_blnTextGrafik.SUB_AktualisiereDasRechteck();
					m_edcHistoryController.AddCommandToHistory(new EDC_BildEditorCommandAdd(i_blnTextGrafik));
				}
				base.Children.Remove(m_edcComboxTool.PRO_fdcComboBox);
				m_edcComboxTool.PRO_fdcComboBox.LostFocus -= m_edcComboxTool.SUB_ComboBoxLostFocus;
				m_edcComboxTool.PRO_fdcComboBox.PreviewKeyDown -= m_edcComboxTool.SUB_ComboPreviewKeyDown;
				m_edcComboxTool.PRO_fdcComboBox.SelectionChanged -= m_edcComboxTool.SUB_TextAuswahlGeaendert;
				m_edcComboxTool.PRO_fdcComboBox = null;
				Focus();
			}
		}

		protected override Visual GetVisualChild(int i_i32Index)
		{
			if (i_i32Index < 0 || i_i32Index >= m_lstGrafikListe.Count)
			{
				if (m_edcTextTool.PRO_fdcTextBox != null && i_i32Index == m_lstGrafikListe.Count)
				{
					return m_edcTextTool.PRO_fdcTextBox;
				}
				if (m_edcComboxTool.PRO_fdcComboBox != null && i_i32Index == m_lstGrafikListe.Count)
				{
					return m_edcComboxTool.PRO_fdcComboBox;
				}
				throw new ArgumentOutOfRangeException("i_i32Index");
			}
			return m_lstGrafikListe[i_i32Index];
		}

		private static void SUB_SkalierungWurdeGeaendert(DependencyObject i_objDependencyProperty, DependencyPropertyChangedEventArgs i_objArgs)
		{
			EDC_BildEditorCanvas eDC_BildEditorCanvas = i_objDependencyProperty as EDC_BildEditorCanvas;
			eDC_BildEditorCanvas?.FUN_blnSetzeNeueSkalierung(eDC_BildEditorCanvas.PRO_dblSkalierung);
		}

		private static void SUB_StrichStaerkeWurdeGeaendert(DependencyObject i_objDependencyProperty, DependencyPropertyChangedEventArgs i_objArgs)
		{
			EDC_BildEditorCanvas eDC_BildEditorCanvas = i_objDependencyProperty as EDC_BildEditorCanvas;
			eDC_BildEditorCanvas?.FUN_blnSetzeNeueStrichStaerke(eDC_BildEditorCanvas.PRO_dblStrichStaerke, i_blnZurHistoryHinzufuegen: true);
		}

		private static void SUB_FarbeWurdeGeaendert(DependencyObject i_objDependencyProperty, DependencyPropertyChangedEventArgs i_objArgs)
		{
			EDC_BildEditorCanvas eDC_BildEditorCanvas = i_objDependencyProperty as EDC_BildEditorCanvas;
			eDC_BildEditorCanvas?.FUN_blnSetzeNeueFarbe(eDC_BildEditorCanvas.PRO_fdcGrafikFarbe, i_blnZurHistoryHinzufuegen: true);
		}

		private static void SUB_TextFontWurdeGeaendert(DependencyObject i_objDependencyProperty, DependencyPropertyChangedEventArgs i_objArgs)
		{
			EDC_BildEditorCanvas eDC_BildEditorCanvas = i_objDependencyProperty as EDC_BildEditorCanvas;
			eDC_BildEditorCanvas?.FUN_blnSetzeNeueFontFamily(eDC_BildEditorCanvas.PRO_strTextFontFamilyName, i_blnZurHistoryHinzufuegen: true);
		}

		private static void SUB_FontStyleWurdeGeaendert(DependencyObject i_objDependencyProperty, DependencyPropertyChangedEventArgs i_objArgs)
		{
			EDC_BildEditorCanvas eDC_BildEditorCanvas = i_objDependencyProperty as EDC_BildEditorCanvas;
			eDC_BildEditorCanvas?.FUN_blnSetzeNeuenFontStyle(eDC_BildEditorCanvas.PRO_fdcTextFontStyle, i_blnZurHistoryHinzufuegen: true);
		}

		private static void SUB_FontTextWeightWurdeGeaendert(DependencyObject i_objDependencyProperty, DependencyPropertyChangedEventArgs i_objArgs)
		{
			EDC_BildEditorCanvas eDC_BildEditorCanvas = i_objDependencyProperty as EDC_BildEditorCanvas;
			eDC_BildEditorCanvas?.FUN_blnSetzeNeuenFontWeight(eDC_BildEditorCanvas.PRO_fdcTextFontWeight, i_blnZurHistoryHinzufuegen: true);
		}

		private static void SUB_FontStretchWurdeGeaendert(DependencyObject i_objDependencyProperty, DependencyPropertyChangedEventArgs i_objArgs)
		{
			EDC_BildEditorCanvas eDC_BildEditorCanvas = i_objDependencyProperty as EDC_BildEditorCanvas;
			eDC_BildEditorCanvas?.FUN_blnSetzeNeuenFontStretch(eDC_BildEditorCanvas.PRO_fdcTextFontStretch, i_blnZurHistoryHinzufuegen: true);
		}

		private static void SUB_FontSizeWurdeGeaendert(DependencyObject i_objDependencyProperty, DependencyPropertyChangedEventArgs i_objArgs)
		{
			EDC_BildEditorCanvas eDC_BildEditorCanvas = i_objDependencyProperty as EDC_BildEditorCanvas;
			eDC_BildEditorCanvas?.FUN_blnSetzeNeuenFontSize(eDC_BildEditorCanvas.PRO_fdcTextFontSize, i_blnZurHistoryHinzufuegen: true);
		}

		private void SUB_BildEditorMouseDown(object i_fdcSender, MouseButtonEventArgs i_fdcArgs)
		{
			if (ma_edcGrafikTools[(int)PRO_enuEditorTool] == null)
			{
				return;
			}
			Focus();
			if (i_fdcArgs.ChangedButton == MouseButton.Left)
			{
				if (i_fdcArgs.ClickCount == 2)
				{
					SUB_BehandleDoppelKlick(i_fdcArgs);
				}
				else
				{
					ma_edcGrafikTools[(int)PRO_enuEditorTool].SUB_OnMouseDown(this, i_fdcArgs);
				}
				SUB_AktualisiereStatus();
			}
			else if (i_fdcArgs.ChangedButton == MouseButton.Right && !PRO_blnDisableContextMenu)
			{
				SUB_ZeigeContextMenu(i_fdcArgs);
			}
		}

		private void SUB_BildEditorMouseMove(object i_fdcSender, MouseEventArgs i_fdcArgs)
		{
			if (ma_edcGrafikTools[(int)PRO_enuEditorTool] == null)
			{
				return;
			}
			if (i_fdcArgs.MiddleButton == MouseButtonState.Pressed)
			{
				EDC_AuswahlTool eDC_AuswahlTool = ma_edcGrafikTools[(int)PRO_enuEditorTool] as EDC_AuswahlTool;
				if (eDC_AuswahlTool != null && eDC_AuswahlTool.PRO_blnIstPanningMoeglich())
				{
					Point position = i_fdcArgs.GetPosition(PRO_fdcScrollView);
					double num = PRO_fdcScrollView.HorizontalOffset - (position.X - PRO_fdcLetztePanningPosition.X);
					double num2 = PRO_fdcScrollView.VerticalOffset - (position.Y - PRO_fdcLetztePanningPosition.Y);
					if (num < 0.0)
					{
						num = 0.0;
					}
					if (num > PRO_fdcScrollView.ExtentWidth - PRO_fdcScrollView.ViewportWidth)
					{
						num = PRO_fdcScrollView.ExtentWidth - PRO_fdcScrollView.ViewportWidth;
					}
					if (num2 < 0.0)
					{
						num2 = 0.0;
					}
					if (num2 > PRO_fdcScrollView.ExtentHeight - PRO_fdcScrollView.ViewportHeight)
					{
						num2 = PRO_fdcScrollView.ExtentHeight - PRO_fdcScrollView.ViewportHeight;
					}
					PRO_fdcScrollView.ScrollToVerticalOffset(num2);
					PRO_fdcScrollView.ScrollToHorizontalOffset(num);
					PRO_fdcLetztePanningPosition = position;
				}
			}
			else
			{
				PRO_fdcLetztePanningPosition = i_fdcArgs.GetPosition(PRO_fdcScrollView);
			}
			PRO_fdcLetzteZoomingPosition = i_fdcArgs.GetPosition(PRO_fdcScrollView);
			if (i_fdcArgs.MiddleButton == MouseButtonState.Released && i_fdcArgs.RightButton == MouseButtonState.Released)
			{
				ma_edcGrafikTools[(int)PRO_enuEditorTool].SUB_OnMouseMove(this, i_fdcArgs);
				SUB_AktualisiereStatus();
			}
			else
			{
				base.Cursor = EDC_BildEditorExtensions.PRO_fdcDefaultCursor;
			}
		}

		private void SUB_BildEditorMouseUp(object i_fdcSender, MouseButtonEventArgs i_fdcArgs)
		{
			if (ma_edcGrafikTools[(int)PRO_enuEditorTool] == null)
			{
				return;
			}
			if (i_fdcArgs.ChangedButton == MouseButton.Left)
			{
				if (PRO_blnRechteckZoomModus && PRO_enuEditorTool == ENUM_EditorToolType.Auswahl)
				{
					VisualCollection.Enumerator enumerator = m_lstGrafikListe.GetEnumerator();
					while (enumerator.MoveNext())
					{
						EDC_AuswahlGrafik eDC_AuswahlGrafik = enumerator.Current as EDC_AuswahlGrafik;
						if (eDC_AuswahlGrafik != null && PRO_delRechteckZoomAction != null)
						{
							PRO_delRechteckZoomAction(eDC_AuswahlGrafik);
						}
					}
					ma_edcGrafikTools[(int)PRO_enuEditorTool].SUB_OnMouseUp(this, i_fdcArgs);
				}
				else
				{
					ma_edcGrafikTools[(int)PRO_enuEditorTool].SUB_OnMouseUp(this, i_fdcArgs);
					if (m_lstGrafikListe.Cast<EDC_GrafikBasisObjekt>().Count((EDC_GrafikBasisObjekt i_edcGrafik) => i_edcGrafik.PRO_blnIstSelektiert) == 1)
					{
						VisualCollection.Enumerator enumerator = m_lstGrafikListe.GetEnumerator();
						while (enumerator.MoveNext())
						{
							EDC_GrafikBasisObjekt obj = (EDC_GrafikBasisObjekt)enumerator.Current;
							if (PRO_delGrafikWurdeSelektiertAction != null)
							{
								PRO_delGrafikWurdeSelektiertAction(obj);
							}
						}
					}
				}
				SUB_AktualisiereStatus();
			}
			if (i_fdcArgs.ChangedButton == MouseButton.Middle && i_fdcArgs.ButtonState == MouseButtonState.Released)
			{
				ma_edcGrafikTools[(int)PRO_enuEditorTool].SUB_OnMouseUp(this, i_fdcArgs);
			}
			if (PRO_blnToolAutoReset)
			{
				PRO_enuEditorTool = ENUM_EditorToolType.Auswahl;
				base.Cursor = EDC_BildEditorExtensions.PRO_fdcDefaultCursor;
			}
		}

		private void SUB_BildEditorLoaded(object i_fdcSender, RoutedEventArgs i_fdcArgs)
		{
			base.Focusable = true;
		}

		private void SUB_ContextMenuItemClick(object i_fdcSender, RoutedEventArgs i_fdcArgs)
		{
			MenuItem menuItem = i_fdcSender as MenuItem;
			if (menuItem != null)
			{
				switch ((ENUM_ContextMenuCommand)menuItem.Tag)
				{
				case ENUM_ContextMenuCommand.AlleAuswaehlen:
					SUB_SelektiereAlleGrafiken();
					break;
				case ENUM_ContextMenuCommand.AuswahlAufheben:
					SUB_SelektionAufheben();
					break;
				case ENUM_ContextMenuCommand.Loeschen:
					SUB_LoescheGrafikAuswahl();
					break;
				case ENUM_ContextMenuCommand.AlleLoeschen:
					SUB_LoescheAlleGrafikobjekte();
					break;
				case ENUM_ContextMenuCommand.InDenVordergrund:
					SUB_BringeGrafikauswahlInDenVordergrund();
					break;
				case ENUM_ContextMenuCommand.InDenHintergrund:
					SUB_BringeGrafikauswahlInDenHintergrund();
					break;
				case ENUM_ContextMenuCommand.Undo:
					SUB_Undo();
					break;
				case ENUM_ContextMenuCommand.Redo:
					SUB_Redo();
					break;
				case ENUM_ContextMenuCommand.Eigenschaften:
					SUB_SetzeNeueEigenschaftenInAuswahl();
					break;
				}
			}
		}

		private void SUB_BildEditorLostMouseCapture(object i_fdcSender, MouseEventArgs i_fdcArgs)
		{
			if (base.IsMouseCaptured)
			{
				SUB_BrecheAktuelleOperationAb();
				SUB_AktualisiereStatus();
			}
		}

		private void SUB_HistoryControllerStateChanged(object i_fdcSender, EventArgs i_fdcArgs)
		{
			PRO_blnCanUndo = m_edcHistoryController.CanUndo;
			PRO_blnCanRedo = m_edcHistoryController.CanRedo;
			if (m_edcHistoryController.IsDirty != PRO_blnIstDirty)
			{
				PRO_blnIstDirty = m_edcHistoryController.IsDirty;
			}
			if (PRO_delGrafikWurdeVeraendertAction != null)
			{
				foreach (EDC_GrafikBasisObjekt item in PRO_enuGrafikSelktionsliste)
				{
					PRO_delGrafikWurdeVeraendertAction(item);
				}
			}
			if (PRO_delNeuerEditorZustandAction != null)
			{
				string text = FUN_strErstelleXmlAusAllenGrafiken();
				if (string.IsNullOrEmpty(text))
				{
					text = string.Empty;
				}
				PRO_delNeuerEditorZustandAction(text);
			}
		}

		private void SUB_ErstelleDasContextMenu()
		{
			m_fdcContextMenu = new ContextMenu();
			MenuItem menuItem = new MenuItem
			{
				Header = "Alle auswählen",
				Tag = ENUM_ContextMenuCommand.AlleAuswaehlen
			};
			menuItem.Click += SUB_ContextMenuItemClick;
			m_fdcContextMenu.Items.Add(menuItem);
			menuItem = new MenuItem
			{
				Header = "Auswahl aufheben",
				Tag = ENUM_ContextMenuCommand.AuswahlAufheben
			};
			menuItem.Click += SUB_ContextMenuItemClick;
			m_fdcContextMenu.Items.Add(menuItem);
			menuItem = new MenuItem
			{
				Header = "Löschen",
				Tag = ENUM_ContextMenuCommand.Loeschen
			};
			menuItem.Click += SUB_ContextMenuItemClick;
			m_fdcContextMenu.Items.Add(menuItem);
			menuItem = new MenuItem
			{
				Header = "Alle löschen",
				Tag = ENUM_ContextMenuCommand.AlleLoeschen
			};
			menuItem.Click += SUB_ContextMenuItemClick;
			m_fdcContextMenu.Items.Add(menuItem);
			m_fdcContextMenu.Items.Add(new Separator());
			menuItem = new MenuItem
			{
				Header = "In den Vordergund bringen",
				Tag = ENUM_ContextMenuCommand.InDenVordergrund
			};
			menuItem.Click += SUB_ContextMenuItemClick;
			m_fdcContextMenu.Items.Add(menuItem);
			menuItem = new MenuItem
			{
				Header = "In den Hintergrund bringen",
				Tag = ENUM_ContextMenuCommand.InDenHintergrund
			};
			menuItem.Click += SUB_ContextMenuItemClick;
			m_fdcContextMenu.Items.Add(menuItem);
			m_fdcContextMenu.Items.Add(new Separator());
			menuItem = new MenuItem
			{
				Header = "Rückgängig machen",
				Tag = ENUM_ContextMenuCommand.Undo
			};
			menuItem.Click += SUB_ContextMenuItemClick;
			m_fdcContextMenu.Items.Add(menuItem);
			menuItem = new MenuItem
			{
				Header = "Wiederherstellen",
				Tag = ENUM_ContextMenuCommand.Redo
			};
			menuItem.Click += SUB_ContextMenuItemClick;
			m_fdcContextMenu.Items.Add(menuItem);
			menuItem = new MenuItem
			{
				Header = "Eigenschaften",
				Tag = ENUM_ContextMenuCommand.Eigenschaften
			};
			menuItem.Click += SUB_ContextMenuItemClick;
			m_fdcContextMenu.Items.Add(menuItem);
		}

		private void SUB_ZeigeContextMenu(MouseButtonEventArgs i_fdcArgs)
		{
			Point position = i_fdcArgs.GetPosition(this);
			EDC_GrafikBasisObjekt eDC_GrafikBasisObjekt = null;
			for (int num = m_lstGrafikListe.Count - 1; num >= 0; num--)
			{
				if (((EDC_GrafikBasisObjekt)m_lstGrafikListe[num]).FUN_i32MacheTrefferTest(position) == 0)
				{
					eDC_GrafikBasisObjekt = (EDC_GrafikBasisObjekt)m_lstGrafikListe[num];
					break;
				}
			}
			if (eDC_GrafikBasisObjekt != null)
			{
				if (!eDC_GrafikBasisObjekt.PRO_blnIstSelektiert)
				{
					SUB_SelektionAufheben();
				}
				eDC_GrafikBasisObjekt.PRO_blnIstSelektiert = true;
			}
			SUB_AktualisiereStatus();
			foreach (object item in (IEnumerable)m_fdcContextMenu.Items)
			{
				MenuItem menuItem = item as MenuItem;
				if (menuItem != null)
				{
					switch ((ENUM_ContextMenuCommand)menuItem.Tag)
					{
					case ENUM_ContextMenuCommand.AlleAuswaehlen:
						menuItem.IsEnabled = PRO_blnCanSelectAll;
						break;
					case ENUM_ContextMenuCommand.AuswahlAufheben:
						menuItem.IsEnabled = PRO_blnCanUnselectAll;
						break;
					case ENUM_ContextMenuCommand.Loeschen:
						menuItem.IsEnabled = PRO_blnCanDelete;
						break;
					case ENUM_ContextMenuCommand.AlleLoeschen:
						menuItem.IsEnabled = PRO_blnCanDeleteAll;
						break;
					case ENUM_ContextMenuCommand.InDenVordergrund:
						menuItem.IsEnabled = PRO_blnCanMoveToFront;
						break;
					case ENUM_ContextMenuCommand.InDenHintergrund:
						menuItem.IsEnabled = PRO_blnCanMoveToBack;
						break;
					case ENUM_ContextMenuCommand.Undo:
						menuItem.IsEnabled = PRO_blnCanUndo;
						break;
					case ENUM_ContextMenuCommand.Redo:
						menuItem.IsEnabled = PRO_blnCanRedo;
						break;
					case ENUM_ContextMenuCommand.Eigenschaften:
						menuItem.IsEnabled = PRO_blnCanSetProperties;
						break;
					}
				}
			}
			m_fdcContextMenu.IsOpen = true;
		}

		private void SUB_BrecheAktuelleOperationAb()
		{
			if (PRO_enuEditorTool == ENUM_EditorToolType.Auswahl)
			{
				if (m_lstGrafikListe.Count > 0)
				{
					if (m_lstGrafikListe[m_lstGrafikListe.Count - 1] is EDC_AuswahlGrafik)
					{
						m_lstGrafikListe.RemoveAt(m_lstGrafikListe.Count - 1);
					}
					else
					{
						m_edcAuswahlTool.SUB_FuegeAenderungInHistoryHinzu(this);
					}
				}
			}
			else if (PRO_enuEditorTool > ENUM_EditorToolType.Auswahl && PRO_enuEditorTool < ENUM_EditorToolType.Max && m_lstGrafikListe.Count > 0)
			{
				m_lstGrafikListe.RemoveAt(m_lstGrafikListe.Count - 1);
			}
			PRO_enuEditorTool = ENUM_EditorToolType.Auswahl;
			ReleaseMouseCapture();
			base.Cursor = EDC_BildEditorExtensions.PRO_fdcDefaultCursor;
		}

		private void SUB_BehandleDoppelKlick(MouseButtonEventArgs i_fdcArgs)
		{
			Point position = i_fdcArgs.GetPosition(this);
			int num = m_lstGrafikListe.Count - 1;
			EDC_TextGrafik eDC_TextGrafik;
			while (true)
			{
				if (num >= 0)
				{
					eDC_TextGrafik = (m_lstGrafikListe[num] as EDC_TextGrafik);
					if (eDC_TextGrafik != null && eDC_TextGrafik.FUN_blnEnthaeltPunkt(position))
					{
						break;
					}
					num--;
					continue;
				}
				return;
			}
			m_edcTextTool.SUB_ErstelleDieTextbox(eDC_TextGrafik, this);
		}

		private void SUB_LoescheHistory()
		{
			m_edcHistoryController.ClearHistory();
		}

		private void SUB_AktualisiereStatus()
		{
			bool flag = PRO_i32GrafikAnzahl > 0;
			bool flag2 = PRO_i32AnzahlSelektiert > 0;
			PRO_blnCanSelectAll = flag;
			PRO_blnCanUnselectAll = flag;
			PRO_blnCanDelete = flag2;
			PRO_blnCanDeleteAll = flag;
			PRO_blnCanMoveToFront = flag2;
			PRO_blnCanMoveToBack = flag2;
			PRO_blnCanSetProperties = this.FUN_blnKannEigenschaftGesetztWerden();
		}
	}
}
