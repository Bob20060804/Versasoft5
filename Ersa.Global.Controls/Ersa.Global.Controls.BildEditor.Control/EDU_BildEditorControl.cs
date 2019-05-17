using Ersa.Global.Controls.BildEditor.Grafik;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Ersa.Global.Controls.BildEditor.Control
{
	public class EDU_BildEditorControl : UserControl, IComponentConnector
	{
		internal ScrollViewer scrollviewer;

		internal EDC_BildEditorCanvas canvas;

		private bool _contentLoaded;

		public EDC_BildEditorCanvas PRO_edcEditorCanvas => canvas;

		public bool PRO_blnToolAutoReset
		{
			get
			{
				return canvas.PRO_blnToolAutoReset;
			}
			set
			{
				canvas.PRO_blnToolAutoReset = value;
			}
		}

		public bool PRO_blnSingleGrafikModus
		{
			get
			{
				return canvas.PRO_blnSingleGrafikModus;
			}
			set
			{
				canvas.PRO_blnSingleGrafikModus = value;
			}
		}

		public bool PRO_blnDisableContextMenu
		{
			get
			{
				return canvas.PRO_blnDisableContextMenu;
			}
			set
			{
				canvas.PRO_blnDisableContextMenu = value;
			}
		}

		public int PRO_i32StrichBreite
		{
			get
			{
				return (int)canvas.PRO_dblStrichStaerke;
			}
			set
			{
				canvas.PRO_dblStrichStaerke = value;
			}
		}

		public double PRO_dblSkalierung
		{
			get
			{
				return PRO_edcViewModel.PRO_dblSkalierung;
			}
			set
			{
				PRO_edcViewModel.PRO_dblSkalierung = value;
			}
		}

		public bool PRO_blnRechteckZoomModus
		{
			get
			{
				return canvas.PRO_blnRechteckZoomModus;
			}
			set
			{
				canvas.PRO_blnRechteckZoomModus = value;
			}
		}

		public Color PRO_fdcGrafikFarbe
		{
			get
			{
				return canvas.PRO_fdcGrafikFarbe;
			}
			set
			{
				canvas.PRO_fdcGrafikFarbe = value;
			}
		}

		public Action<EDC_GrafikBasisObjekt> PRO_delNeueGrafikWurdeErstelltAction
		{
			set
			{
				canvas.PRO_delNeueGrafikWurdeErstelltAction = value;
			}
		}

		public Action<EDC_GrafikBasisObjekt> PRO_delGrafikWurdeSelektiertAction
		{
			set
			{
				canvas.PRO_delGrafikWurdeSelektiertAction = value;
			}
		}

		public Action<EDC_GrafikBasisObjekt> PRO_delGrafikWurdeVeraendertAction
		{
			set
			{
				canvas.PRO_delGrafikWurdeVeraendertAction = value;
			}
		}

		public Action<string> PRO_delNeuerEditorZustandAction
		{
			set
			{
				canvas.PRO_delNeuerEditorZustandAction = value;
			}
		}

		public Action PRO_delBearbeitetHinweisAction
		{
			set
			{
				canvas.PRO_delBearbeitetHinweisAction = value;
			}
		}

		public string PRO_strTextFontFamilyName
		{
			get
			{
				return canvas.PRO_strTextFontFamilyName;
			}
			set
			{
				canvas.PRO_strTextFontFamilyName = value;
			}
		}

		public FontStyle PRO_fdcTextFontStyle
		{
			get
			{
				return canvas.PRO_fdcTextFontStyle;
			}
			set
			{
				canvas.PRO_fdcTextFontStyle = value;
			}
		}

		public FontWeight PRO_fdcTextFontWeight
		{
			get
			{
				return canvas.PRO_fdcTextFontWeight;
			}
			set
			{
				canvas.PRO_fdcTextFontWeight = value;
			}
		}

		public FontStretch PRO_fdcTextFontStretch
		{
			get
			{
				return canvas.PRO_fdcTextFontStretch;
			}
			set
			{
				canvas.PRO_fdcTextFontStretch = value;
			}
		}

		public double PRO_fdcTextFontSize
		{
			get
			{
				return canvas.PRO_fdcTextFontSize;
			}
			set
			{
				canvas.PRO_fdcTextFontSize = value;
			}
		}

		public double PRO_dblMaximaleVergroeserung
		{
			get
			{
				return PRO_edcViewModel.PRO_dblMaximaleVergroeserung;
			}
			set
			{
				PRO_edcViewModel.PRO_dblMaximaleVergroeserung = value;
			}
		}

		public double PRO_dblZoomSchrittweite
		{
			get
			{
				return PRO_edcViewModel.PRO_dblMaximaleVergroeserung;
			}
			set
			{
				PRO_edcViewModel.PRO_dblMaximaleVergroeserung = value;
			}
		}

		private EDC_BildEditorControlViewModel PRO_edcViewModel
		{
			get
			{
				return base.DataContext as EDC_BildEditorControlViewModel;
			}
			set
			{
				base.DataContext = value;
			}
		}

		public EDU_BildEditorControl()
		{
			InitializeComponent();
			PRO_edcViewModel = new EDC_BildEditorControlViewModel();
			canvas.PRO_fdcScrollView = scrollviewer;
			canvas.MouseWheel += PRO_edcViewModel.SUB_MouseWheel;
			PRO_edcViewModel.PRO_edcEditorCanvas = canvas;
			base.MouseMove += PRO_edcViewModel.SUB_MouseMoveHandler;
			scrollviewer.KeyDown += canvas.SUB_BildEditorKeyDown;
		}

		public void SUB_ZommeZuVollbild()
		{
			PRO_edcViewModel.SUB_SkaliereVollbild();
		}

		public Point FUN_fdcTransformiereNachPixel(Point i_fdcPosition)
		{
			return PRO_edcViewModel.FUN_fdcTransformiereNachPixel(i_fdcPosition);
		}

		public void SUB_LoescheAlleGrafiken()
		{
			canvas.SUB_LoescheAlleGrafikobjekte();
		}

		public void SUB_LoescheDieGrafik(EDC_GrafikBasisObjekt i_edcGrafik)
		{
			canvas.SUB_LoescheGrafik(i_edcGrafik);
		}

		public void SUB_LoescheAusgewählteGrafiken()
		{
			canvas.SUB_LoescheGrafikAuswahl();
		}

		public void SUB_SetzeNeuesBild(WriteableBitmap i_fdcBild)
		{
			PRO_edcViewModel.PRO_fdcBild = i_fdcBild;
		}

		public void SUB_AktiviereAuswahlTool()
		{
			canvas.PRO_enuEditorTool = ENUM_EditorToolType.Auswahl;
		}

		public void SUB_AktiviereRechteckTool()
		{
			canvas.SUB_SelektionAufheben();
			canvas.PRO_enuEditorTool = ENUM_EditorToolType.Rechteck;
		}

		public void SUB_AktiviereTextTool()
		{
			canvas.SUB_SelektionAufheben();
			canvas.PRO_enuEditorTool = ENUM_EditorToolType.Text;
		}

		public void SUB_AktiviereEllipsenTool()
		{
			canvas.SUB_SelektionAufheben();
			canvas.PRO_enuEditorTool = ENUM_EditorToolType.Ellipse;
		}

		public void SUB_AktivierePunktTool()
		{
			canvas.SUB_SelektionAufheben();
			canvas.PRO_enuEditorTool = ENUM_EditorToolType.Punkt;
		}

		public void SUB_AktiviereLinienTool()
		{
			canvas.SUB_SelektionAufheben();
			canvas.PRO_enuEditorTool = ENUM_EditorToolType.Linie;
		}

		public void SUB_AktiviereMehrfachLinienTool()
		{
			canvas.SUB_SelektionAufheben();
			canvas.PRO_enuEditorTool = ENUM_EditorToolType.MehrfachLinie;
		}

		public void SUB_AktiviereComboboxTool(List<string> i_lstTexte)
		{
			canvas.SUB_SelektionAufheben();
			canvas.SUB_SetzeCombobxTextListe(i_lstTexte);
			canvas.PRO_enuEditorTool = ENUM_EditorToolType.ComboBox;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			if (!_contentLoaded)
			{
				_contentLoaded = true;
				Uri resourceLocator = new Uri("/Ersa.Global.Controls;component/bildeditor/control/edu_bildeditorcontrol.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		internal Delegate _CreateDelegate(Type delegateType, string handler)
		{
			return Delegate.CreateDelegate(delegateType, this, handler);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			switch (connectionId)
			{
			case 1:
				scrollviewer = (ScrollViewer)target;
				break;
			case 2:
				canvas = (EDC_BildEditorCanvas)target;
				break;
			default:
				_contentLoaded = true;
				break;
			}
		}
	}
}
