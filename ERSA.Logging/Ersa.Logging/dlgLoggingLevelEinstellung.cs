using Ersa.Logging.My.Resources;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace Ersa.Logging
{
	[DesignerGenerated]
	public class dlgLoggingLevelEinstellung : Form
	{
		private IContainer components;

		[AccessedThroughProperty("grpBasic")]
		private GroupBox _grpBasic;

		[AccessedThroughProperty("grpModule")]
		private GroupBox _grpModule;

		[AccessedThroughProperty("tcbInfo")]
		private uclCheckBox _tcbInfo;

		[AccessedThroughProperty("tcbWarnung")]
		private uclCheckBox _tcbWarnung;

		[AccessedThroughProperty("tcbFehler")]
		private uclCheckBox _tcbFehler;

		[AccessedThroughProperty("tcbAlleBasis")]
		private uclCheckBox _tcbAlleBasis;

		[AccessedThroughProperty("tcbVorheizModul")]
		private uclCheckBox _tcbVorheizModul;

		[AccessedThroughProperty("tcbEinlauf")]
		private uclCheckBox _tcbEinlauf;

		[AccessedThroughProperty("tcbFluxerModul")]
		private uclCheckBox _tcbFluxerModul;

		[AccessedThroughProperty("tcbEinlaufModul")]
		private uclCheckBox _tcbEinlaufModul;

		[AccessedThroughProperty("tcbTraceability")]
		private uclCheckBox _tcbTraceability;

		[AccessedThroughProperty("tcbAlleErweitert")]
		private uclCheckBox _tcbAlleErweitert;

		[AccessedThroughProperty("grpGlobal")]
		private GroupBox _grpGlobal;

		[AccessedThroughProperty("tcbAlle")]
		private uclCheckBox _tcbAlle;

		[AccessedThroughProperty("tcbKein")]
		private uclCheckBox _tcbKein;

		[AccessedThroughProperty("pnlForm")]
		private Panel _pnlForm;

		[AccessedThroughProperty("lblNameDateiMitPfadLogging")]
		private Label _lblNameDateiMitPfadLogging;

		[AccessedThroughProperty("btnLoeschen")]
		private Button _btnLoeschen;

		[AccessedThroughProperty("lblLevelHex")]
		private Label _lblLevelHex;

		[AccessedThroughProperty("lblLevelDecimal")]
		private Label _lblLevelDecimal;

		[AccessedThroughProperty("btnUebernehmen")]
		private Button _btnUebernehmen;

		[AccessedThroughProperty("btnSchliessen")]
		private Button _btnSchliessen;

		[AccessedThroughProperty("tcbLoetModul")]
		private uclCheckBox _tcbLoetModul;

		[AccessedThroughProperty("tcbAuslaufModul")]
		private uclCheckBox _tcbAuslaufModul;

		[AccessedThroughProperty("tcbAuslauf")]
		private uclCheckBox _tcbAuslauf;

		[AccessedThroughProperty("ToolTipDialog")]
		private ToolTip _ToolTipDialog;

		[AccessedThroughProperty("tcbLoetProgrammEditor")]
		private uclCheckBox _tcbLoetProgrammEditor;

		[AccessedThroughProperty("tcbKonfiguration")]
		private uclCheckBox _tcbKonfiguration;

		[AccessedThroughProperty("tcbCodebetrieb")]
		private uclCheckBox _tcbCodebetrieb;

		[AccessedThroughProperty("tcbProtokoll")]
		private uclCheckBox _tcbProtokoll;

		[AccessedThroughProperty("tcbPCBDurchlauf")]
		private uclCheckBox _tcbPCBDurchlauf;

		[AccessedThroughProperty("tcbLoetProgramm")]
		private uclCheckBox _tcbLoetProgramm;

		[AccessedThroughProperty("tcbProzessSchreiber")]
		private uclCheckBox _tcbProzessSchreiber;

		[AccessedThroughProperty("tcbDatensicherung")]
		private uclCheckBox _tcbDatensicherung;

		[AccessedThroughProperty("tcbLeiterkarte")]
		private uclCheckBox _tcbLeiterkarte;

		[AccessedThroughProperty("tcbHeizung")]
		private uclCheckBox _tcbHeizung;

		[AccessedThroughProperty("tcbBasisklasse")]
		private uclCheckBox _tcbBasisklasse;

		[AccessedThroughProperty("tcbUebersicht")]
		private uclCheckBox _tcbUebersicht;

		[AccessedThroughProperty("tcbRueckTransportModul")]
		private uclCheckBox _tcbRueckTransportModul;

		[AccessedThroughProperty("tcbFluxer")]
		private uclCheckBox _tcbFluxer;

		[AccessedThroughProperty("tcbCNC")]
		private uclCheckBox _tcbCNC;

		[AccessedThroughProperty("tcbLoetEinheit")]
		private uclCheckBox _tcbLoetEinheit;

		private bool m_blnEinstellungenUebernehmen;

		public bool m_blnLoggingErweitert;

		private ENUM_LoggingLevels m_enmLoggingLevelTemp;

		public ENUM_LoggingLevels m_enmLoggingLevelAktuell;

		internal EDC_ERSALogging m_edcLogging;

		internal virtual GroupBox grpBasic
		{
			get
			{
				return _grpBasic;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				_grpBasic = value;
			}
		}

		internal virtual GroupBox grpModule
		{
			get
			{
				return _grpModule;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				_grpModule = value;
			}
		}

		internal virtual uclCheckBox tcbInfo
		{
			get
			{
				return _tcbInfo;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler value2 = tcbLogging_Erweitert_EVT_CheckStateChanged;
				if (_tcbInfo != null)
				{
					_tcbInfo.Click -= value2;
				}
				_tcbInfo = value;
				if (_tcbInfo != null)
				{
					_tcbInfo.Click += value2;
				}
			}
		}

		internal virtual uclCheckBox tcbWarnung
		{
			get
			{
				return _tcbWarnung;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler value2 = tcbLogging_Erweitert_EVT_CheckStateChanged;
				if (_tcbWarnung != null)
				{
					_tcbWarnung.Click -= value2;
				}
				_tcbWarnung = value;
				if (_tcbWarnung != null)
				{
					_tcbWarnung.Click += value2;
				}
			}
		}

		internal virtual uclCheckBox tcbFehler
		{
			get
			{
				return _tcbFehler;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				_tcbFehler = value;
			}
		}

		internal virtual uclCheckBox tcbAlleBasis
		{
			get
			{
				return _tcbAlleBasis;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler value2 = tcbLogging_Basis_EVT_CheckStateChanged;
				if (_tcbAlleBasis != null)
				{
					_tcbAlleBasis.Click -= value2;
				}
				_tcbAlleBasis = value;
				if (_tcbAlleBasis != null)
				{
					_tcbAlleBasis.Click += value2;
				}
			}
		}

		internal virtual uclCheckBox tcbVorheizModul
		{
			get
			{
				return _tcbVorheizModul;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler value2 = tcbLogging_Erweitert_EVT_CheckStateChanged;
				if (_tcbVorheizModul != null)
				{
					_tcbVorheizModul.Click -= value2;
				}
				_tcbVorheizModul = value;
				if (_tcbVorheizModul != null)
				{
					_tcbVorheizModul.Click += value2;
				}
			}
		}

		internal virtual uclCheckBox tcbEinlauf
		{
			get
			{
				return _tcbEinlauf;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler value2 = tcbLogging_Erweitert_EVT_CheckStateChanged;
				if (_tcbEinlauf != null)
				{
					_tcbEinlauf.Click -= value2;
				}
				_tcbEinlauf = value;
				if (_tcbEinlauf != null)
				{
					_tcbEinlauf.Click += value2;
				}
			}
		}

		internal virtual uclCheckBox tcbFluxerModul
		{
			get
			{
				return _tcbFluxerModul;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler value2 = tcbLogging_Erweitert_EVT_CheckStateChanged;
				if (_tcbFluxerModul != null)
				{
					_tcbFluxerModul.Click -= value2;
				}
				_tcbFluxerModul = value;
				if (_tcbFluxerModul != null)
				{
					_tcbFluxerModul.Click += value2;
				}
			}
		}

		internal virtual uclCheckBox tcbEinlaufModul
		{
			get
			{
				return _tcbEinlaufModul;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler value2 = tcbLogging_Erweitert_EVT_CheckStateChanged;
				if (_tcbEinlaufModul != null)
				{
					_tcbEinlaufModul.Click -= value2;
				}
				_tcbEinlaufModul = value;
				if (_tcbEinlaufModul != null)
				{
					_tcbEinlaufModul.Click += value2;
				}
			}
		}

		internal virtual uclCheckBox tcbTraceability
		{
			get
			{
				return _tcbTraceability;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler value2 = tcbLogging_Erweitert_EVT_CheckStateChanged;
				if (_tcbTraceability != null)
				{
					_tcbTraceability.Click -= value2;
				}
				_tcbTraceability = value;
				if (_tcbTraceability != null)
				{
					_tcbTraceability.Click += value2;
				}
			}
		}

		internal virtual uclCheckBox tcbAlleErweitert
		{
			get
			{
				return _tcbAlleErweitert;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler value2 = tcbLogging_Basis_EVT_CheckStateChanged;
				if (_tcbAlleErweitert != null)
				{
					_tcbAlleErweitert.Click -= value2;
				}
				_tcbAlleErweitert = value;
				if (_tcbAlleErweitert != null)
				{
					_tcbAlleErweitert.Click += value2;
				}
			}
		}

		internal virtual GroupBox grpGlobal
		{
			get
			{
				return _grpGlobal;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				_grpGlobal = value;
			}
		}

		internal virtual uclCheckBox tcbAlle
		{
			get
			{
				return _tcbAlle;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler value2 = tcbLogging_Basis_EVT_CheckStateChanged;
				if (_tcbAlle != null)
				{
					_tcbAlle.Click -= value2;
				}
				_tcbAlle = value;
				if (_tcbAlle != null)
				{
					_tcbAlle.Click += value2;
				}
			}
		}

		internal virtual uclCheckBox tcbKein
		{
			get
			{
				return _tcbKein;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler value2 = tcbLogging_Basis_EVT_CheckStateChanged;
				if (_tcbKein != null)
				{
					_tcbKein.Click -= value2;
				}
				_tcbKein = value;
				if (_tcbKein != null)
				{
					_tcbKein.Click += value2;
				}
			}
		}

		internal virtual Panel pnlForm
		{
			get
			{
				return _pnlForm;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				_pnlForm = value;
			}
		}

		internal virtual Label lblNameDateiMitPfadLogging
		{
			get
			{
				return _lblNameDateiMitPfadLogging;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				_lblNameDateiMitPfadLogging = value;
			}
		}

		public virtual Button btnLoeschen
		{
			get
			{
				return _btnLoeschen;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				_btnLoeschen = value;
			}
		}

		internal virtual Label lblLevelHex
		{
			get
			{
				return _lblLevelHex;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				_lblLevelHex = value;
			}
		}

		internal virtual Label lblLevelDecimal
		{
			get
			{
				return _lblLevelDecimal;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				_lblLevelDecimal = value;
			}
		}

		public virtual Button btnUebernehmen
		{
			get
			{
				return _btnUebernehmen;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler value2 = btnUebernehmen_Click;
				if (_btnUebernehmen != null)
				{
					_btnUebernehmen.Click -= value2;
				}
				_btnUebernehmen = value;
				if (_btnUebernehmen != null)
				{
					_btnUebernehmen.Click += value2;
				}
			}
		}

		public virtual Button btnSchliessen
		{
			get
			{
				return _btnSchliessen;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler value2 = btnSchliessen_Click;
				if (_btnSchliessen != null)
				{
					_btnSchliessen.Click -= value2;
				}
				_btnSchliessen = value;
				if (_btnSchliessen != null)
				{
					_btnSchliessen.Click += value2;
				}
			}
		}

		internal virtual uclCheckBox tcbLoetModul
		{
			get
			{
				return _tcbLoetModul;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler value2 = tcbLogging_Erweitert_EVT_CheckStateChanged;
				if (_tcbLoetModul != null)
				{
					_tcbLoetModul.Click -= value2;
				}
				_tcbLoetModul = value;
				if (_tcbLoetModul != null)
				{
					_tcbLoetModul.Click += value2;
				}
			}
		}

		internal virtual uclCheckBox tcbAuslaufModul
		{
			get
			{
				return _tcbAuslaufModul;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler value2 = tcbLogging_Erweitert_EVT_CheckStateChanged;
				if (_tcbAuslaufModul != null)
				{
					_tcbAuslaufModul.Click -= value2;
				}
				_tcbAuslaufModul = value;
				if (_tcbAuslaufModul != null)
				{
					_tcbAuslaufModul.Click += value2;
				}
			}
		}

		internal virtual uclCheckBox tcbAuslauf
		{
			get
			{
				return _tcbAuslauf;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler value2 = tcbLogging_Erweitert_EVT_CheckStateChanged;
				if (_tcbAuslauf != null)
				{
					_tcbAuslauf.Click -= value2;
				}
				_tcbAuslauf = value;
				if (_tcbAuslauf != null)
				{
					_tcbAuslauf.Click += value2;
				}
			}
		}

		internal virtual ToolTip ToolTipDialog
		{
			get
			{
				return _ToolTipDialog;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				_ToolTipDialog = value;
			}
		}

		internal virtual uclCheckBox tcbLoetProgrammEditor
		{
			get
			{
				return _tcbLoetProgrammEditor;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler value2 = tcbLogging_Erweitert_EVT_CheckStateChanged;
				if (_tcbLoetProgrammEditor != null)
				{
					_tcbLoetProgrammEditor.Click -= value2;
				}
				_tcbLoetProgrammEditor = value;
				if (_tcbLoetProgrammEditor != null)
				{
					_tcbLoetProgrammEditor.Click += value2;
				}
			}
		}

		internal virtual uclCheckBox tcbKonfiguration
		{
			get
			{
				return _tcbKonfiguration;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler value2 = tcbLogging_Erweitert_EVT_CheckStateChanged;
				if (_tcbKonfiguration != null)
				{
					_tcbKonfiguration.Click -= value2;
				}
				_tcbKonfiguration = value;
				if (_tcbKonfiguration != null)
				{
					_tcbKonfiguration.Click += value2;
				}
			}
		}

		internal virtual uclCheckBox tcbCodebetrieb
		{
			get
			{
				return _tcbCodebetrieb;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler value2 = tcbLogging_Erweitert_EVT_CheckStateChanged;
				if (_tcbCodebetrieb != null)
				{
					_tcbCodebetrieb.Click -= value2;
				}
				_tcbCodebetrieb = value;
				if (_tcbCodebetrieb != null)
				{
					_tcbCodebetrieb.Click += value2;
				}
			}
		}

		internal virtual uclCheckBox tcbProtokoll
		{
			get
			{
				return _tcbProtokoll;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler value2 = tcbLogging_Erweitert_EVT_CheckStateChanged;
				if (_tcbProtokoll != null)
				{
					_tcbProtokoll.Click -= value2;
				}
				_tcbProtokoll = value;
				if (_tcbProtokoll != null)
				{
					_tcbProtokoll.Click += value2;
				}
			}
		}

		internal virtual uclCheckBox tcbPCBDurchlauf
		{
			get
			{
				return _tcbPCBDurchlauf;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler value2 = tcbLogging_Erweitert_EVT_CheckStateChanged;
				if (_tcbPCBDurchlauf != null)
				{
					_tcbPCBDurchlauf.Click -= value2;
				}
				_tcbPCBDurchlauf = value;
				if (_tcbPCBDurchlauf != null)
				{
					_tcbPCBDurchlauf.Click += value2;
				}
			}
		}

		internal virtual uclCheckBox tcbLoetProgramm
		{
			get
			{
				return _tcbLoetProgramm;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler value2 = tcbLogging_Erweitert_EVT_CheckStateChanged;
				if (_tcbLoetProgramm != null)
				{
					_tcbLoetProgramm.Click -= value2;
				}
				_tcbLoetProgramm = value;
				if (_tcbLoetProgramm != null)
				{
					_tcbLoetProgramm.Click += value2;
				}
			}
		}

		internal virtual uclCheckBox tcbProzessSchreiber
		{
			get
			{
				return _tcbProzessSchreiber;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler value2 = tcbLogging_Erweitert_EVT_CheckStateChanged;
				if (_tcbProzessSchreiber != null)
				{
					_tcbProzessSchreiber.Click -= value2;
				}
				_tcbProzessSchreiber = value;
				if (_tcbProzessSchreiber != null)
				{
					_tcbProzessSchreiber.Click += value2;
				}
			}
		}

		internal virtual uclCheckBox tcbDatensicherung
		{
			get
			{
				return _tcbDatensicherung;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler value2 = tcbLogging_Erweitert_EVT_CheckStateChanged;
				if (_tcbDatensicherung != null)
				{
					_tcbDatensicherung.Click -= value2;
				}
				_tcbDatensicherung = value;
				if (_tcbDatensicherung != null)
				{
					_tcbDatensicherung.Click += value2;
				}
			}
		}

		internal virtual uclCheckBox tcbLeiterkarte
		{
			get
			{
				return _tcbLeiterkarte;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler value2 = tcbLogging_Erweitert_EVT_CheckStateChanged;
				if (_tcbLeiterkarte != null)
				{
					_tcbLeiterkarte.Click -= value2;
				}
				_tcbLeiterkarte = value;
				if (_tcbLeiterkarte != null)
				{
					_tcbLeiterkarte.Click += value2;
				}
			}
		}

		internal virtual uclCheckBox tcbHeizung
		{
			get
			{
				return _tcbHeizung;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler value2 = tcbLogging_Erweitert_EVT_CheckStateChanged;
				if (_tcbHeizung != null)
				{
					_tcbHeizung.Click -= value2;
				}
				_tcbHeizung = value;
				if (_tcbHeizung != null)
				{
					_tcbHeizung.Click += value2;
				}
			}
		}

		internal virtual uclCheckBox tcbBasisklasse
		{
			get
			{
				return _tcbBasisklasse;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler value2 = tcbLogging_Erweitert_EVT_CheckStateChanged;
				if (_tcbBasisklasse != null)
				{
					_tcbBasisklasse.Click -= value2;
				}
				_tcbBasisklasse = value;
				if (_tcbBasisklasse != null)
				{
					_tcbBasisklasse.Click += value2;
				}
			}
		}

		internal virtual uclCheckBox tcbUebersicht
		{
			get
			{
				return _tcbUebersicht;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler value2 = tcbLogging_Erweitert_EVT_CheckStateChanged;
				if (_tcbUebersicht != null)
				{
					_tcbUebersicht.Click -= value2;
				}
				_tcbUebersicht = value;
				if (_tcbUebersicht != null)
				{
					_tcbUebersicht.Click += value2;
				}
			}
		}

		internal virtual uclCheckBox tcbRueckTransportModul
		{
			get
			{
				return _tcbRueckTransportModul;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler value2 = tcbLogging_Erweitert_EVT_CheckStateChanged;
				if (_tcbRueckTransportModul != null)
				{
					_tcbRueckTransportModul.Click -= value2;
				}
				_tcbRueckTransportModul = value;
				if (_tcbRueckTransportModul != null)
				{
					_tcbRueckTransportModul.Click += value2;
				}
			}
		}

		internal virtual uclCheckBox tcbFluxer
		{
			get
			{
				return _tcbFluxer;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler value2 = tcbLogging_Erweitert_EVT_CheckStateChanged;
				if (_tcbFluxer != null)
				{
					_tcbFluxer.Click -= value2;
				}
				_tcbFluxer = value;
				if (_tcbFluxer != null)
				{
					_tcbFluxer.Click += value2;
				}
			}
		}

		internal virtual uclCheckBox tcbCNC
		{
			get
			{
				return _tcbCNC;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler value2 = tcbLogging_Erweitert_EVT_CheckStateChanged;
				if (_tcbCNC != null)
				{
					_tcbCNC.Click -= value2;
				}
				_tcbCNC = value;
				if (_tcbCNC != null)
				{
					_tcbCNC.Click += value2;
				}
			}
		}

		internal virtual uclCheckBox tcbLoetEinheit
		{
			get
			{
				return _tcbLoetEinheit;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler value2 = tcbLogging_Erweitert_EVT_CheckStateChanged;
				if (_tcbLoetEinheit != null)
				{
					_tcbLoetEinheit.Click -= value2;
				}
				_tcbLoetEinheit = value;
				if (_tcbLoetEinheit != null)
				{
					_tcbLoetEinheit.Click += value2;
				}
			}
		}

		public dlgLoggingLevelEinstellung()
		{
			base.Load += dlgLoggingLevelEinstellung_Load;
			m_blnEinstellungenUebernehmen = false;
			m_blnLoggingErweitert = false;
			m_enmLoggingLevelTemp = ENUM_LoggingLevels.Kein;
			m_enmLoggingLevelAktuell = ENUM_LoggingLevels.Kein;
			InitializeComponent();
		}

		[DebuggerNonUserCode]
		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		[System.Diagnostics.DebuggerStepThrough]
		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager componentResourceManager = new System.ComponentModel.ComponentResourceManager(typeof(Ersa.Logging.dlgLoggingLevelEinstellung));
			grpBasic = new System.Windows.Forms.GroupBox();
			tcbInfo = new Ersa.Logging.uclCheckBox();
			tcbWarnung = new Ersa.Logging.uclCheckBox();
			tcbFehler = new Ersa.Logging.uclCheckBox();
			grpModule = new System.Windows.Forms.GroupBox();
			tcbCNC = new Ersa.Logging.uclCheckBox();
			tcbLoetEinheit = new Ersa.Logging.uclCheckBox();
			tcbFluxer = new Ersa.Logging.uclCheckBox();
			tcbRueckTransportModul = new Ersa.Logging.uclCheckBox();
			tcbLeiterkarte = new Ersa.Logging.uclCheckBox();
			tcbHeizung = new Ersa.Logging.uclCheckBox();
			tcbBasisklasse = new Ersa.Logging.uclCheckBox();
			tcbUebersicht = new Ersa.Logging.uclCheckBox();
			tcbPCBDurchlauf = new Ersa.Logging.uclCheckBox();
			tcbLoetProgramm = new Ersa.Logging.uclCheckBox();
			tcbProzessSchreiber = new Ersa.Logging.uclCheckBox();
			tcbDatensicherung = new Ersa.Logging.uclCheckBox();
			tcbLoetProgrammEditor = new Ersa.Logging.uclCheckBox();
			tcbKonfiguration = new Ersa.Logging.uclCheckBox();
			tcbCodebetrieb = new Ersa.Logging.uclCheckBox();
			tcbProtokoll = new Ersa.Logging.uclCheckBox();
			tcbAuslauf = new Ersa.Logging.uclCheckBox();
			tcbAuslaufModul = new Ersa.Logging.uclCheckBox();
			tcbLoetModul = new Ersa.Logging.uclCheckBox();
			tcbVorheizModul = new Ersa.Logging.uclCheckBox();
			tcbEinlauf = new Ersa.Logging.uclCheckBox();
			tcbFluxerModul = new Ersa.Logging.uclCheckBox();
			tcbEinlaufModul = new Ersa.Logging.uclCheckBox();
			tcbTraceability = new Ersa.Logging.uclCheckBox();
			grpGlobal = new System.Windows.Forms.GroupBox();
			tcbAlle = new Ersa.Logging.uclCheckBox();
			tcbKein = new Ersa.Logging.uclCheckBox();
			tcbAlleBasis = new Ersa.Logging.uclCheckBox();
			tcbAlleErweitert = new Ersa.Logging.uclCheckBox();
			pnlForm = new System.Windows.Forms.Panel();
			lblNameDateiMitPfadLogging = new System.Windows.Forms.Label();
			btnLoeschen = new System.Windows.Forms.Button();
			lblLevelHex = new System.Windows.Forms.Label();
			lblLevelDecimal = new System.Windows.Forms.Label();
			btnUebernehmen = new System.Windows.Forms.Button();
			btnSchliessen = new System.Windows.Forms.Button();
			ToolTipDialog = new System.Windows.Forms.ToolTip(components);
			grpBasic.SuspendLayout();
			grpModule.SuspendLayout();
			grpGlobal.SuspendLayout();
			pnlForm.SuspendLayout();
			SuspendLayout();
			grpBasic.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right);
			grpBasic.Controls.Add(tcbInfo);
			grpBasic.Controls.Add(tcbWarnung);
			grpBasic.Controls.Add(tcbFehler);
			grpBasic.Enabled = false;
			grpBasic.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			System.Drawing.Point point2 = grpBasic.Location = new System.Drawing.Point(8, 96);
			grpBasic.Name = "grpBasic";
			System.Drawing.Size size2 = grpBasic.Size = new System.Drawing.Size(776, 80);
			grpBasic.TabIndex = 0;
			grpBasic.TabStop = false;
			grpBasic.Text = "Basic Logging";
			tcbInfo.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			tcbInfo.BackColor = System.Drawing.SystemColors.Control;
			tcbInfo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			tcbInfo.ForeColor = System.Drawing.SystemColors.ControlText;
			point2 = (tcbInfo.Location = new System.Drawing.Point(392, 24));
			System.Windows.Forms.Padding padding2 = tcbInfo.Margin = new System.Windows.Forms.Padding(8);
			tcbInfo.Name = "tcbInfo";
			tcbInfo.PRO_blnEnabled = true;
			tcbInfo.PRO_intCharSet = 1;
			tcbInfo.PRO_intCheckState = System.Windows.Forms.CheckState.Unchecked;
			tcbInfo.PRO_sdcBackColor = System.Drawing.SystemColors.Control;
			tcbInfo.PRO_strBeschreibung = "Info";
			tcbInfo.PRO_strToolTip = "";
			size2 = (tcbInfo.Size = new System.Drawing.Size(184, 48));
			tcbInfo.TabIndex = 3;
			tcbWarnung.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			tcbWarnung.BackColor = System.Drawing.SystemColors.Control;
			tcbWarnung.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			tcbWarnung.ForeColor = System.Drawing.SystemColors.ControlText;
			point2 = (tcbWarnung.Location = new System.Drawing.Point(200, 24));
			padding2 = (tcbWarnung.Margin = new System.Windows.Forms.Padding(8));
			tcbWarnung.Name = "tcbWarnung";
			tcbWarnung.PRO_blnEnabled = true;
			tcbWarnung.PRO_intCharSet = 1;
			tcbWarnung.PRO_intCheckState = System.Windows.Forms.CheckState.Unchecked;
			tcbWarnung.PRO_sdcBackColor = System.Drawing.SystemColors.Control;
			tcbWarnung.PRO_strBeschreibung = "Warning";
			tcbWarnung.PRO_strToolTip = "";
			size2 = (tcbWarnung.Size = new System.Drawing.Size(184, 48));
			tcbWarnung.TabIndex = 2;
			tcbFehler.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			tcbFehler.BackColor = System.Drawing.SystemColors.Control;
			tcbFehler.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			tcbFehler.ForeColor = System.Drawing.SystemColors.ControlText;
			point2 = (tcbFehler.Location = new System.Drawing.Point(8, 24));
			padding2 = (tcbFehler.Margin = new System.Windows.Forms.Padding(8));
			tcbFehler.Name = "tcbFehler";
			tcbFehler.PRO_blnEnabled = false;
			tcbFehler.PRO_intCharSet = 1;
			tcbFehler.PRO_intCheckState = System.Windows.Forms.CheckState.Unchecked;
			tcbFehler.PRO_sdcBackColor = System.Drawing.SystemColors.Control;
			tcbFehler.PRO_strBeschreibung = "Error";
			tcbFehler.PRO_strToolTip = "";
			size2 = (tcbFehler.Size = new System.Drawing.Size(184, 48));
			tcbFehler.TabIndex = 1;
			grpModule.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right);
			grpModule.BackColor = System.Drawing.Color.Transparent;
			grpModule.Controls.Add(tcbCNC);
			grpModule.Controls.Add(tcbLoetEinheit);
			grpModule.Controls.Add(tcbFluxer);
			grpModule.Controls.Add(tcbRueckTransportModul);
			grpModule.Controls.Add(tcbLeiterkarte);
			grpModule.Controls.Add(tcbHeizung);
			grpModule.Controls.Add(tcbBasisklasse);
			grpModule.Controls.Add(tcbUebersicht);
			grpModule.Controls.Add(tcbPCBDurchlauf);
			grpModule.Controls.Add(tcbLoetProgramm);
			grpModule.Controls.Add(tcbProzessSchreiber);
			grpModule.Controls.Add(tcbDatensicherung);
			grpModule.Controls.Add(tcbLoetProgrammEditor);
			grpModule.Controls.Add(tcbKonfiguration);
			grpModule.Controls.Add(tcbCodebetrieb);
			grpModule.Controls.Add(tcbProtokoll);
			grpModule.Controls.Add(tcbAuslauf);
			grpModule.Controls.Add(tcbAuslaufModul);
			grpModule.Controls.Add(tcbLoetModul);
			grpModule.Controls.Add(tcbVorheizModul);
			grpModule.Controls.Add(tcbEinlauf);
			grpModule.Controls.Add(tcbFluxerModul);
			grpModule.Controls.Add(tcbEinlaufModul);
			grpModule.Controls.Add(tcbTraceability);
			grpModule.Enabled = false;
			grpModule.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			point2 = (grpModule.Location = new System.Drawing.Point(8, 184));
			grpModule.Name = "grpModule";
			size2 = (grpModule.Size = new System.Drawing.Size(776, 360));
			grpModule.TabIndex = 1;
			grpModule.TabStop = false;
			grpModule.Text = "Extended Logging";
			tcbCNC.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			tcbCNC.BackColor = System.Drawing.SystemColors.Control;
			tcbCNC.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			tcbCNC.ForeColor = System.Drawing.SystemColors.ControlText;
			point2 = (tcbCNC.Location = new System.Drawing.Point(584, 304));
			padding2 = (tcbCNC.Margin = new System.Windows.Forms.Padding(4));
			tcbCNC.Name = "tcbCNC";
			tcbCNC.PRO_blnEnabled = true;
			tcbCNC.PRO_intCharSet = 1;
			tcbCNC.PRO_intCheckState = System.Windows.Forms.CheckState.Unchecked;
			tcbCNC.PRO_sdcBackColor = System.Drawing.Color.Transparent;
			tcbCNC.PRO_strBeschreibung = "CNC";
			tcbCNC.PRO_strToolTip = "";
			size2 = (tcbCNC.Size = new System.Drawing.Size(184, 48));
			tcbCNC.TabIndex = 29;
			tcbLoetEinheit.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			tcbLoetEinheit.BackColor = System.Drawing.SystemColors.Control;
			tcbLoetEinheit.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			tcbLoetEinheit.ForeColor = System.Drawing.SystemColors.ControlText;
			point2 = (tcbLoetEinheit.Location = new System.Drawing.Point(392, 304));
			padding2 = (tcbLoetEinheit.Margin = new System.Windows.Forms.Padding(4));
			tcbLoetEinheit.Name = "tcbLoetEinheit";
			tcbLoetEinheit.PRO_blnEnabled = true;
			tcbLoetEinheit.PRO_intCharSet = 1;
			tcbLoetEinheit.PRO_intCheckState = System.Windows.Forms.CheckState.Unchecked;
			tcbLoetEinheit.PRO_sdcBackColor = System.Drawing.Color.Transparent;
			tcbLoetEinheit.PRO_strBeschreibung = "Soldering";
			tcbLoetEinheit.PRO_strToolTip = "";
			size2 = (tcbLoetEinheit.Size = new System.Drawing.Size(184, 48));
			tcbLoetEinheit.TabIndex = 28;
			tcbFluxer.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			tcbFluxer.BackColor = System.Drawing.SystemColors.Control;
			tcbFluxer.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			tcbFluxer.ForeColor = System.Drawing.SystemColors.ControlText;
			point2 = (tcbFluxer.Location = new System.Drawing.Point(200, 304));
			padding2 = (tcbFluxer.Margin = new System.Windows.Forms.Padding(4));
			tcbFluxer.Name = "tcbFluxer";
			tcbFluxer.PRO_blnEnabled = true;
			tcbFluxer.PRO_intCharSet = 1;
			tcbFluxer.PRO_intCheckState = System.Windows.Forms.CheckState.Unchecked;
			tcbFluxer.PRO_sdcBackColor = System.Drawing.Color.Transparent;
			tcbFluxer.PRO_strBeschreibung = "Fluxing";
			tcbFluxer.PRO_strToolTip = "";
			size2 = (tcbFluxer.Size = new System.Drawing.Size(184, 48));
			tcbFluxer.TabIndex = 27;
			tcbRueckTransportModul.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			tcbRueckTransportModul.BackColor = System.Drawing.SystemColors.Control;
			tcbRueckTransportModul.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			tcbRueckTransportModul.ForeColor = System.Drawing.SystemColors.ControlText;
			point2 = (tcbRueckTransportModul.Location = new System.Drawing.Point(584, 248));
			padding2 = (tcbRueckTransportModul.Margin = new System.Windows.Forms.Padding(4));
			tcbRueckTransportModul.Name = "tcbRueckTransportModul";
			tcbRueckTransportModul.PRO_blnEnabled = true;
			tcbRueckTransportModul.PRO_intCharSet = 1;
			tcbRueckTransportModul.PRO_intCheckState = System.Windows.Forms.CheckState.Unchecked;
			tcbRueckTransportModul.PRO_sdcBackColor = System.Drawing.Color.Transparent;
			tcbRueckTransportModul.PRO_strBeschreibung = "Return transport unit";
			tcbRueckTransportModul.PRO_strToolTip = "";
			size2 = (tcbRueckTransportModul.Size = new System.Drawing.Size(184, 48));
			tcbRueckTransportModul.TabIndex = 26;
			tcbLeiterkarte.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			tcbLeiterkarte.BackColor = System.Drawing.SystemColors.Control;
			tcbLeiterkarte.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			tcbLeiterkarte.ForeColor = System.Drawing.SystemColors.ControlText;
			point2 = (tcbLeiterkarte.Location = new System.Drawing.Point(392, 248));
			padding2 = (tcbLeiterkarte.Margin = new System.Windows.Forms.Padding(4));
			tcbLeiterkarte.Name = "tcbLeiterkarte";
			tcbLeiterkarte.PRO_blnEnabled = true;
			tcbLeiterkarte.PRO_intCharSet = 1;
			tcbLeiterkarte.PRO_intCheckState = System.Windows.Forms.CheckState.Unchecked;
			tcbLeiterkarte.PRO_sdcBackColor = System.Drawing.Color.Transparent;
			tcbLeiterkarte.PRO_strBeschreibung = "PCB";
			tcbLeiterkarte.PRO_strToolTip = "Printed circuit board";
			size2 = (tcbLeiterkarte.Size = new System.Drawing.Size(184, 48));
			tcbLeiterkarte.TabIndex = 25;
			tcbHeizung.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			tcbHeizung.BackColor = System.Drawing.SystemColors.Control;
			tcbHeizung.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			tcbHeizung.ForeColor = System.Drawing.SystemColors.ControlText;
			point2 = (tcbHeizung.Location = new System.Drawing.Point(8, 304));
			padding2 = (tcbHeizung.Margin = new System.Windows.Forms.Padding(4));
			tcbHeizung.Name = "tcbHeizung";
			tcbHeizung.PRO_blnEnabled = true;
			tcbHeizung.PRO_intCharSet = 1;
			tcbHeizung.PRO_intCheckState = System.Windows.Forms.CheckState.Unchecked;
			tcbHeizung.PRO_sdcBackColor = System.Drawing.Color.Transparent;
			tcbHeizung.PRO_strBeschreibung = "Heating";
			tcbHeizung.PRO_strToolTip = "";
			size2 = (tcbHeizung.Size = new System.Drawing.Size(184, 48));
			tcbHeizung.TabIndex = 24;
			tcbBasisklasse.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			tcbBasisklasse.BackColor = System.Drawing.SystemColors.Control;
			tcbBasisklasse.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			tcbBasisklasse.ForeColor = System.Drawing.SystemColors.ControlText;
			point2 = (tcbBasisklasse.Location = new System.Drawing.Point(200, 248));
			padding2 = (tcbBasisklasse.Margin = new System.Windows.Forms.Padding(4));
			tcbBasisklasse.Name = "tcbBasisklasse";
			tcbBasisklasse.PRO_blnEnabled = true;
			tcbBasisklasse.PRO_intCharSet = 1;
			tcbBasisklasse.PRO_intCheckState = System.Windows.Forms.CheckState.Unchecked;
			tcbBasisklasse.PRO_sdcBackColor = System.Drawing.Color.Transparent;
			tcbBasisklasse.PRO_strBeschreibung = "Base class";
			tcbBasisklasse.PRO_strToolTip = "";
			size2 = (tcbBasisklasse.Size = new System.Drawing.Size(184, 48));
			tcbBasisklasse.TabIndex = 23;
			tcbUebersicht.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			tcbUebersicht.BackColor = System.Drawing.SystemColors.Control;
			tcbUebersicht.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			tcbUebersicht.ForeColor = System.Drawing.SystemColors.ControlText;
			point2 = (tcbUebersicht.Location = new System.Drawing.Point(8, 248));
			padding2 = (tcbUebersicht.Margin = new System.Windows.Forms.Padding(4));
			tcbUebersicht.Name = "tcbUebersicht";
			tcbUebersicht.PRO_blnEnabled = true;
			tcbUebersicht.PRO_intCharSet = 1;
			tcbUebersicht.PRO_intCheckState = System.Windows.Forms.CheckState.Unchecked;
			tcbUebersicht.PRO_sdcBackColor = System.Drawing.Color.Transparent;
			tcbUebersicht.PRO_strBeschreibung = "General view";
			tcbUebersicht.PRO_strToolTip = "";
			size2 = (tcbUebersicht.Size = new System.Drawing.Size(184, 48));
			tcbUebersicht.TabIndex = 22;
			tcbPCBDurchlauf.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			tcbPCBDurchlauf.BackColor = System.Drawing.SystemColors.Control;
			tcbPCBDurchlauf.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			tcbPCBDurchlauf.ForeColor = System.Drawing.SystemColors.ControlText;
			point2 = (tcbPCBDurchlauf.Location = new System.Drawing.Point(392, 192));
			padding2 = (tcbPCBDurchlauf.Margin = new System.Windows.Forms.Padding(4));
			tcbPCBDurchlauf.Name = "tcbPCBDurchlauf";
			tcbPCBDurchlauf.PRO_blnEnabled = true;
			tcbPCBDurchlauf.PRO_intCharSet = 1;
			tcbPCBDurchlauf.PRO_intCheckState = System.Windows.Forms.CheckState.Unchecked;
			tcbPCBDurchlauf.PRO_sdcBackColor = System.Drawing.Color.Transparent;
			tcbPCBDurchlauf.PRO_strBeschreibung = "PCB run-through";
			tcbPCBDurchlauf.PRO_strToolTip = "";
			size2 = (tcbPCBDurchlauf.Size = new System.Drawing.Size(184, 48));
			tcbPCBDurchlauf.TabIndex = 21;
			tcbLoetProgramm.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			tcbLoetProgramm.BackColor = System.Drawing.SystemColors.Control;
			tcbLoetProgramm.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			tcbLoetProgramm.ForeColor = System.Drawing.SystemColors.ControlText;
			point2 = (tcbLoetProgramm.Location = new System.Drawing.Point(584, 192));
			padding2 = (tcbLoetProgramm.Margin = new System.Windows.Forms.Padding(4));
			tcbLoetProgramm.Name = "tcbLoetProgramm";
			tcbLoetProgramm.PRO_blnEnabled = true;
			tcbLoetProgramm.PRO_intCharSet = 1;
			tcbLoetProgramm.PRO_intCheckState = System.Windows.Forms.CheckState.Unchecked;
			tcbLoetProgramm.PRO_sdcBackColor = System.Drawing.Color.Transparent;
			tcbLoetProgramm.PRO_strBeschreibung = "SP management";
			tcbLoetProgramm.PRO_strToolTip = "Soldering program management";
			size2 = (tcbLoetProgramm.Size = new System.Drawing.Size(184, 48));
			tcbLoetProgramm.TabIndex = 20;
			tcbProzessSchreiber.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			tcbProzessSchreiber.BackColor = System.Drawing.SystemColors.Control;
			tcbProzessSchreiber.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			tcbProzessSchreiber.ForeColor = System.Drawing.SystemColors.ControlText;
			point2 = (tcbProzessSchreiber.Location = new System.Drawing.Point(200, 192));
			padding2 = (tcbProzessSchreiber.Margin = new System.Windows.Forms.Padding(4));
			tcbProzessSchreiber.Name = "tcbProzessSchreiber";
			tcbProzessSchreiber.PRO_blnEnabled = true;
			tcbProzessSchreiber.PRO_intCharSet = 1;
			tcbProzessSchreiber.PRO_intCheckState = System.Windows.Forms.CheckState.Unchecked;
			tcbProzessSchreiber.PRO_sdcBackColor = System.Drawing.Color.Transparent;
			tcbProzessSchreiber.PRO_strBeschreibung = "Process monitoring";
			tcbProzessSchreiber.PRO_strToolTip = "";
			size2 = (tcbProzessSchreiber.Size = new System.Drawing.Size(184, 48));
			tcbProzessSchreiber.TabIndex = 19;
			tcbDatensicherung.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			tcbDatensicherung.BackColor = System.Drawing.SystemColors.Control;
			tcbDatensicherung.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			tcbDatensicherung.ForeColor = System.Drawing.SystemColors.ControlText;
			point2 = (tcbDatensicherung.Location = new System.Drawing.Point(8, 192));
			padding2 = (tcbDatensicherung.Margin = new System.Windows.Forms.Padding(4));
			tcbDatensicherung.Name = "tcbDatensicherung";
			tcbDatensicherung.PRO_blnEnabled = true;
			tcbDatensicherung.PRO_intCharSet = 1;
			tcbDatensicherung.PRO_intCheckState = System.Windows.Forms.CheckState.Unchecked;
			tcbDatensicherung.PRO_sdcBackColor = System.Drawing.Color.Transparent;
			tcbDatensicherung.PRO_strBeschreibung = "Data Storing";
			tcbDatensicherung.PRO_strToolTip = "";
			size2 = (tcbDatensicherung.Size = new System.Drawing.Size(184, 48));
			tcbDatensicherung.TabIndex = 18;
			tcbLoetProgrammEditor.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			tcbLoetProgrammEditor.BackColor = System.Drawing.SystemColors.Control;
			tcbLoetProgrammEditor.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			tcbLoetProgrammEditor.ForeColor = System.Drawing.SystemColors.ControlText;
			point2 = (tcbLoetProgrammEditor.Location = new System.Drawing.Point(584, 136));
			padding2 = (tcbLoetProgrammEditor.Margin = new System.Windows.Forms.Padding(4));
			tcbLoetProgrammEditor.Name = "tcbLoetProgrammEditor";
			tcbLoetProgrammEditor.PRO_blnEnabled = true;
			tcbLoetProgrammEditor.PRO_intCharSet = 1;
			tcbLoetProgrammEditor.PRO_intCheckState = System.Windows.Forms.CheckState.Unchecked;
			tcbLoetProgrammEditor.PRO_sdcBackColor = System.Drawing.Color.Transparent;
			tcbLoetProgrammEditor.PRO_strBeschreibung = "SP Editor";
			tcbLoetProgrammEditor.PRO_strToolTip = "Soldering program editor";
			size2 = (tcbLoetProgrammEditor.Size = new System.Drawing.Size(184, 48));
			tcbLoetProgrammEditor.TabIndex = 17;
			tcbKonfiguration.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			tcbKonfiguration.BackColor = System.Drawing.SystemColors.Control;
			tcbKonfiguration.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			tcbKonfiguration.ForeColor = System.Drawing.SystemColors.ControlText;
			point2 = (tcbKonfiguration.Location = new System.Drawing.Point(392, 136));
			padding2 = (tcbKonfiguration.Margin = new System.Windows.Forms.Padding(4));
			tcbKonfiguration.Name = "tcbKonfiguration";
			tcbKonfiguration.PRO_blnEnabled = true;
			tcbKonfiguration.PRO_intCharSet = 1;
			tcbKonfiguration.PRO_intCheckState = System.Windows.Forms.CheckState.Unchecked;
			tcbKonfiguration.PRO_sdcBackColor = System.Drawing.Color.Transparent;
			tcbKonfiguration.PRO_strBeschreibung = "Configuration";
			tcbKonfiguration.PRO_strToolTip = "";
			size2 = (tcbKonfiguration.Size = new System.Drawing.Size(184, 48));
			tcbKonfiguration.TabIndex = 16;
			tcbCodebetrieb.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			tcbCodebetrieb.BackColor = System.Drawing.SystemColors.Control;
			tcbCodebetrieb.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			tcbCodebetrieb.ForeColor = System.Drawing.SystemColors.ControlText;
			point2 = (tcbCodebetrieb.Location = new System.Drawing.Point(200, 136));
			padding2 = (tcbCodebetrieb.Margin = new System.Windows.Forms.Padding(4));
			tcbCodebetrieb.Name = "tcbCodebetrieb";
			tcbCodebetrieb.PRO_blnEnabled = true;
			tcbCodebetrieb.PRO_intCharSet = 1;
			tcbCodebetrieb.PRO_intCheckState = System.Windows.Forms.CheckState.Unchecked;
			tcbCodebetrieb.PRO_sdcBackColor = System.Drawing.Color.Transparent;
			tcbCodebetrieb.PRO_strBeschreibung = "Code mode";
			tcbCodebetrieb.PRO_strToolTip = "";
			size2 = (tcbCodebetrieb.Size = new System.Drawing.Size(184, 48));
			tcbCodebetrieb.TabIndex = 15;
			tcbProtokoll.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			tcbProtokoll.BackColor = System.Drawing.SystemColors.Control;
			tcbProtokoll.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			tcbProtokoll.ForeColor = System.Drawing.SystemColors.ControlText;
			point2 = (tcbProtokoll.Location = new System.Drawing.Point(8, 136));
			padding2 = (tcbProtokoll.Margin = new System.Windows.Forms.Padding(4));
			tcbProtokoll.Name = "tcbProtokoll";
			tcbProtokoll.PRO_blnEnabled = true;
			tcbProtokoll.PRO_intCharSet = 1;
			tcbProtokoll.PRO_intCheckState = System.Windows.Forms.CheckState.Unchecked;
			tcbProtokoll.PRO_sdcBackColor = System.Drawing.Color.Transparent;
			tcbProtokoll.PRO_strBeschreibung = "Protocol";
			tcbProtokoll.PRO_strToolTip = "";
			size2 = (tcbProtokoll.Size = new System.Drawing.Size(184, 48));
			tcbProtokoll.TabIndex = 14;
			tcbAuslauf.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			tcbAuslauf.BackColor = System.Drawing.SystemColors.Control;
			tcbAuslauf.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			tcbAuslauf.ForeColor = System.Drawing.SystemColors.ControlText;
			point2 = (tcbAuslauf.Location = new System.Drawing.Point(584, 80));
			padding2 = (tcbAuslauf.Margin = new System.Windows.Forms.Padding(4));
			tcbAuslauf.Name = "tcbAuslauf";
			tcbAuslauf.PRO_blnEnabled = true;
			tcbAuslauf.PRO_intCharSet = 1;
			tcbAuslauf.PRO_intCheckState = System.Windows.Forms.CheckState.Unchecked;
			tcbAuslauf.PRO_sdcBackColor = System.Drawing.Color.Transparent;
			tcbAuslauf.PRO_strBeschreibung = "Exit";
			tcbAuslauf.PRO_strToolTip = "";
			size2 = (tcbAuslauf.Size = new System.Drawing.Size(184, 48));
			tcbAuslauf.TabIndex = 13;
			tcbAuslaufModul.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			tcbAuslaufModul.BackColor = System.Drawing.SystemColors.Control;
			tcbAuslaufModul.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			tcbAuslaufModul.ForeColor = System.Drawing.SystemColors.ControlText;
			point2 = (tcbAuslaufModul.Location = new System.Drawing.Point(392, 80));
			padding2 = (tcbAuslaufModul.Margin = new System.Windows.Forms.Padding(4));
			tcbAuslaufModul.Name = "tcbAuslaufModul";
			tcbAuslaufModul.PRO_blnEnabled = true;
			tcbAuslaufModul.PRO_intCharSet = 1;
			tcbAuslaufModul.PRO_intCheckState = System.Windows.Forms.CheckState.Unchecked;
			tcbAuslaufModul.PRO_sdcBackColor = System.Drawing.Color.Transparent;
			tcbAuslaufModul.PRO_strBeschreibung = "Exit unit";
			tcbAuslaufModul.PRO_strToolTip = "";
			size2 = (tcbAuslaufModul.Size = new System.Drawing.Size(184, 48));
			tcbAuslaufModul.TabIndex = 12;
			tcbLoetModul.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			tcbLoetModul.BackColor = System.Drawing.SystemColors.Control;
			tcbLoetModul.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			tcbLoetModul.ForeColor = System.Drawing.SystemColors.ControlText;
			point2 = (tcbLoetModul.Location = new System.Drawing.Point(200, 80));
			padding2 = (tcbLoetModul.Margin = new System.Windows.Forms.Padding(4));
			tcbLoetModul.Name = "tcbLoetModul";
			tcbLoetModul.PRO_blnEnabled = true;
			tcbLoetModul.PRO_intCharSet = 1;
			tcbLoetModul.PRO_intCheckState = System.Windows.Forms.CheckState.Unchecked;
			tcbLoetModul.PRO_sdcBackColor = System.Drawing.Color.Transparent;
			tcbLoetModul.PRO_strBeschreibung = "Soldering unit";
			tcbLoetModul.PRO_strToolTip = "";
			size2 = (tcbLoetModul.Size = new System.Drawing.Size(184, 48));
			tcbLoetModul.TabIndex = 11;
			tcbVorheizModul.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			tcbVorheizModul.BackColor = System.Drawing.SystemColors.Control;
			tcbVorheizModul.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			tcbVorheizModul.ForeColor = System.Drawing.SystemColors.ControlText;
			point2 = (tcbVorheizModul.Location = new System.Drawing.Point(8, 80));
			padding2 = (tcbVorheizModul.Margin = new System.Windows.Forms.Padding(4));
			tcbVorheizModul.Name = "tcbVorheizModul";
			tcbVorheizModul.PRO_blnEnabled = true;
			tcbVorheizModul.PRO_intCharSet = 1;
			tcbVorheizModul.PRO_intCheckState = System.Windows.Forms.CheckState.Unchecked;
			tcbVorheizModul.PRO_sdcBackColor = System.Drawing.Color.Transparent;
			tcbVorheizModul.PRO_strBeschreibung = "Preheat unit";
			tcbVorheizModul.PRO_strToolTip = "";
			size2 = (tcbVorheizModul.Size = new System.Drawing.Size(184, 48));
			tcbVorheizModul.TabIndex = 10;
			tcbEinlauf.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			tcbEinlauf.BackColor = System.Drawing.SystemColors.Control;
			tcbEinlauf.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			tcbEinlauf.ForeColor = System.Drawing.SystemColors.ControlText;
			point2 = (tcbEinlauf.Location = new System.Drawing.Point(200, 24));
			padding2 = (tcbEinlauf.Margin = new System.Windows.Forms.Padding(4));
			tcbEinlauf.Name = "tcbEinlauf";
			tcbEinlauf.PRO_blnEnabled = true;
			tcbEinlauf.PRO_intCharSet = 1;
			tcbEinlauf.PRO_intCheckState = System.Windows.Forms.CheckState.Unchecked;
			tcbEinlauf.PRO_sdcBackColor = System.Drawing.Color.Transparent;
			tcbEinlauf.PRO_strBeschreibung = "Infeed";
			tcbEinlauf.PRO_strToolTip = "";
			size2 = (tcbEinlauf.Size = new System.Drawing.Size(184, 48));
			tcbEinlauf.TabIndex = 8;
			tcbFluxerModul.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			tcbFluxerModul.BackColor = System.Drawing.SystemColors.Control;
			tcbFluxerModul.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			tcbFluxerModul.ForeColor = System.Drawing.SystemColors.ControlText;
			point2 = (tcbFluxerModul.Location = new System.Drawing.Point(584, 24));
			padding2 = (tcbFluxerModul.Margin = new System.Windows.Forms.Padding(4));
			tcbFluxerModul.Name = "tcbFluxerModul";
			tcbFluxerModul.PRO_blnEnabled = true;
			tcbFluxerModul.PRO_intCharSet = 1;
			tcbFluxerModul.PRO_intCheckState = System.Windows.Forms.CheckState.Unchecked;
			tcbFluxerModul.PRO_sdcBackColor = System.Drawing.Color.Transparent;
			tcbFluxerModul.PRO_strBeschreibung = "Flux unit";
			tcbFluxerModul.PRO_strToolTip = "";
			size2 = (tcbFluxerModul.Size = new System.Drawing.Size(184, 48));
			tcbFluxerModul.TabIndex = 7;
			tcbEinlaufModul.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			tcbEinlaufModul.BackColor = System.Drawing.SystemColors.Control;
			tcbEinlaufModul.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			tcbEinlaufModul.ForeColor = System.Drawing.SystemColors.ControlText;
			point2 = (tcbEinlaufModul.Location = new System.Drawing.Point(392, 24));
			padding2 = (tcbEinlaufModul.Margin = new System.Windows.Forms.Padding(4));
			tcbEinlaufModul.Name = "tcbEinlaufModul";
			tcbEinlaufModul.PRO_blnEnabled = true;
			tcbEinlaufModul.PRO_intCharSet = 1;
			tcbEinlaufModul.PRO_intCheckState = System.Windows.Forms.CheckState.Unchecked;
			tcbEinlaufModul.PRO_sdcBackColor = System.Drawing.Color.Transparent;
			tcbEinlaufModul.PRO_strBeschreibung = "Infeed unit";
			tcbEinlaufModul.PRO_strToolTip = "";
			size2 = (tcbEinlaufModul.Size = new System.Drawing.Size(184, 48));
			tcbEinlaufModul.TabIndex = 6;
			tcbTraceability.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			tcbTraceability.BackColor = System.Drawing.SystemColors.Control;
			tcbTraceability.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			tcbTraceability.ForeColor = System.Drawing.SystemColors.ControlText;
			point2 = (tcbTraceability.Location = new System.Drawing.Point(8, 24));
			padding2 = (tcbTraceability.Margin = new System.Windows.Forms.Padding(4));
			tcbTraceability.Name = "tcbTraceability";
			tcbTraceability.PRO_blnEnabled = true;
			tcbTraceability.PRO_intCharSet = 1;
			tcbTraceability.PRO_intCheckState = System.Windows.Forms.CheckState.Unchecked;
			tcbTraceability.PRO_sdcBackColor = System.Drawing.Color.Transparent;
			tcbTraceability.PRO_strBeschreibung = "Traceability";
			tcbTraceability.PRO_strToolTip = "";
			size2 = (tcbTraceability.Size = new System.Drawing.Size(184, 48));
			tcbTraceability.TabIndex = 5;
			grpGlobal.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right);
			grpGlobal.Controls.Add(tcbAlle);
			grpGlobal.Controls.Add(tcbKein);
			grpGlobal.Controls.Add(tcbAlleBasis);
			grpGlobal.Controls.Add(tcbAlleErweitert);
			grpGlobal.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			point2 = (grpGlobal.Location = new System.Drawing.Point(8, 8));
			grpGlobal.Name = "grpGlobal";
			size2 = (grpGlobal.Size = new System.Drawing.Size(776, 80));
			grpGlobal.TabIndex = 16;
			grpGlobal.TabStop = false;
			grpGlobal.Text = "Global Logging";
			tcbAlle.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			tcbAlle.BackColor = System.Drawing.SystemColors.Control;
			tcbAlle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			tcbAlle.Enabled = false;
			tcbAlle.ForeColor = System.Drawing.SystemColors.ControlText;
			point2 = (tcbAlle.Location = new System.Drawing.Point(584, 24));
			padding2 = (tcbAlle.Margin = new System.Windows.Forms.Padding(8));
			tcbAlle.Name = "tcbAlle";
			tcbAlle.PRO_blnEnabled = true;
			tcbAlle.PRO_intCharSet = 1;
			tcbAlle.PRO_intCheckState = System.Windows.Forms.CheckState.Unchecked;
			tcbAlle.PRO_sdcBackColor = System.Drawing.SystemColors.Control;
			tcbAlle.PRO_strBeschreibung = "All";
			tcbAlle.PRO_strToolTip = "";
			size2 = (tcbAlle.Size = new System.Drawing.Size(184, 48));
			tcbAlle.TabIndex = 17;
			tcbKein.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			tcbKein.BackColor = System.Drawing.SystemColors.Control;
			tcbKein.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			tcbKein.ForeColor = System.Drawing.SystemColors.ControlText;
			point2 = (tcbKein.Location = new System.Drawing.Point(8, 24));
			padding2 = (tcbKein.Margin = new System.Windows.Forms.Padding(8));
			tcbKein.Name = "tcbKein";
			tcbKein.PRO_blnEnabled = true;
			tcbKein.PRO_intCharSet = 1;
			tcbKein.PRO_intCheckState = System.Windows.Forms.CheckState.Checked;
			tcbKein.PRO_sdcBackColor = System.Drawing.SystemColors.Control;
			tcbKein.PRO_strBeschreibung = "None";
			tcbKein.PRO_strToolTip = "";
			size2 = (tcbKein.Size = new System.Drawing.Size(184, 48));
			tcbKein.TabIndex = 16;
			tcbAlleBasis.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			tcbAlleBasis.BackColor = System.Drawing.SystemColors.Control;
			tcbAlleBasis.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			tcbAlleBasis.ForeColor = System.Drawing.SystemColors.ControlText;
			point2 = (tcbAlleBasis.Location = new System.Drawing.Point(200, 24));
			padding2 = (tcbAlleBasis.Margin = new System.Windows.Forms.Padding(8));
			tcbAlleBasis.Name = "tcbAlleBasis";
			tcbAlleBasis.PRO_blnEnabled = true;
			tcbAlleBasis.PRO_intCharSet = 1;
			tcbAlleBasis.PRO_intCheckState = System.Windows.Forms.CheckState.Unchecked;
			tcbAlleBasis.PRO_sdcBackColor = System.Drawing.SystemColors.Control;
			tcbAlleBasis.PRO_strBeschreibung = "Basic All";
			tcbAlleBasis.PRO_strToolTip = "";
			size2 = (tcbAlleBasis.Size = new System.Drawing.Size(184, 48));
			tcbAlleBasis.TabIndex = 0;
			tcbAlleErweitert.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			tcbAlleErweitert.BackColor = System.Drawing.SystemColors.Control;
			tcbAlleErweitert.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			tcbAlleErweitert.ForeColor = System.Drawing.SystemColors.ControlText;
			point2 = (tcbAlleErweitert.Location = new System.Drawing.Point(392, 24));
			padding2 = (tcbAlleErweitert.Margin = new System.Windows.Forms.Padding(8));
			tcbAlleErweitert.Name = "tcbAlleErweitert";
			tcbAlleErweitert.PRO_blnEnabled = true;
			tcbAlleErweitert.PRO_intCharSet = 1;
			tcbAlleErweitert.PRO_intCheckState = System.Windows.Forms.CheckState.Unchecked;
			tcbAlleErweitert.PRO_sdcBackColor = System.Drawing.SystemColors.Control;
			tcbAlleErweitert.PRO_strBeschreibung = "Module All";
			tcbAlleErweitert.PRO_strToolTip = "";
			size2 = (tcbAlleErweitert.Size = new System.Drawing.Size(184, 48));
			tcbAlleErweitert.TabIndex = 4;
			pnlForm.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right);
			pnlForm.Controls.Add(lblNameDateiMitPfadLogging);
			pnlForm.Controls.Add(btnLoeschen);
			pnlForm.Controls.Add(lblLevelHex);
			pnlForm.Controls.Add(lblLevelDecimal);
			pnlForm.Controls.Add(btnUebernehmen);
			pnlForm.Controls.Add(btnSchliessen);
			point2 = (pnlForm.Location = new System.Drawing.Point(0, 576));
			pnlForm.Name = "pnlForm";
			size2 = (pnlForm.Size = new System.Drawing.Size(792, 140));
			pnlForm.TabIndex = 21;
			lblNameDateiMitPfadLogging.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right);
			lblNameDateiMitPfadLogging.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			lblNameDateiMitPfadLogging.Font = new System.Drawing.Font("Arial", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			point2 = (lblNameDateiMitPfadLogging.Location = new System.Drawing.Point(8, 0));
			lblNameDateiMitPfadLogging.Name = "lblNameDateiMitPfadLogging";
			size2 = (lblNameDateiMitPfadLogging.Size = new System.Drawing.Size(776, 48));
			lblNameDateiMitPfadLogging.TabIndex = 30;
			lblNameDateiMitPfadLogging.Text = "NameDateiMitPfadLogging";
			lblNameDateiMitPfadLogging.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			btnLoeschen.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
			btnLoeschen.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			btnLoeschen.BackColor = System.Drawing.SystemColors.Control;
			btnLoeschen.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
			btnLoeschen.Cursor = System.Windows.Forms.Cursors.Default;
			btnLoeschen.ForeColor = System.Drawing.SystemColors.ControlText;
			btnLoeschen.Image = Ersa.Logging.My.Resources.Resources.pngTrash;
			point2 = (btnLoeschen.Location = new System.Drawing.Point(212, 72));
			padding2 = (btnLoeschen.Margin = new System.Windows.Forms.Padding(4));
			btnLoeschen.Name = "btnLoeschen";
			btnLoeschen.RightToLeft = System.Windows.Forms.RightToLeft.No;
			size2 = (btnLoeschen.Size = new System.Drawing.Size(60, 60));
			btnLoeschen.TabIndex = 29;
			btnLoeschen.UseVisualStyleBackColor = false;
			btnLoeschen.Visible = false;
			lblLevelHex.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			lblLevelHex.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			lblLevelHex.Font = new System.Drawing.Font("Courier New", 14.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			point2 = (lblLevelHex.Location = new System.Drawing.Point(572, 86));
			lblLevelHex.Name = "lblLevelHex";
			size2 = (lblLevelHex.Size = new System.Drawing.Size(212, 32));
			lblLevelHex.TabIndex = 28;
			lblLevelHex.Text = "0";
			lblLevelHex.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			lblLevelDecimal.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			lblLevelDecimal.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			lblLevelDecimal.Font = new System.Drawing.Font("Courier New", 14.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			point2 = (lblLevelDecimal.Location = new System.Drawing.Point(446, 86));
			lblLevelDecimal.Name = "lblLevelDecimal";
			size2 = (lblLevelDecimal.Size = new System.Drawing.Size(120, 32));
			lblLevelDecimal.TabIndex = 27;
			lblLevelDecimal.Text = "000000000";
			lblLevelDecimal.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			btnUebernehmen.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
			btnUebernehmen.BackColor = System.Drawing.SystemColors.Control;
			btnUebernehmen.Cursor = System.Windows.Forms.Cursors.Default;
			btnUebernehmen.ForeColor = System.Drawing.SystemColors.ControlText;
			btnUebernehmen.Image = Ersa.Logging.My.Resources.Resources.png3071;
			point2 = (btnUebernehmen.Location = new System.Drawing.Point(76, 72));
			padding2 = (btnUebernehmen.Margin = new System.Windows.Forms.Padding(4));
			btnUebernehmen.Name = "btnUebernehmen";
			btnUebernehmen.RightToLeft = System.Windows.Forms.RightToLeft.No;
			size2 = (btnUebernehmen.Size = new System.Drawing.Size(60, 60));
			btnUebernehmen.TabIndex = 26;
			btnUebernehmen.UseVisualStyleBackColor = false;
			btnSchliessen.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
			btnSchliessen.BackColor = System.Drawing.SystemColors.Control;
			btnSchliessen.Cursor = System.Windows.Forms.Cursors.Default;
			btnSchliessen.ForeColor = System.Drawing.SystemColors.ControlText;
			btnSchliessen.Image = Ersa.Logging.My.Resources.Resources.png3070;
			point2 = (btnSchliessen.Location = new System.Drawing.Point(8, 72));
			padding2 = (btnSchliessen.Margin = new System.Windows.Forms.Padding(4));
			btnSchliessen.Name = "btnSchliessen";
			btnSchliessen.RightToLeft = System.Windows.Forms.RightToLeft.No;
			size2 = (btnSchliessen.Size = new System.Drawing.Size(60, 60));
			btnSchliessen.TabIndex = 25;
			btnSchliessen.UseVisualStyleBackColor = false;
			System.Drawing.SizeF sizeF2 = AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			size2 = (ClientSize = new System.Drawing.Size(792, 716));
			Controls.Add(grpModule);
			Controls.Add(grpBasic);
			Controls.Add(grpGlobal);
			Controls.Add(pnlForm);
			Icon = (System.Drawing.Icon)componentResourceManager.GetObject("$this.Icon");
			MaximizeBox = false;
			Name = "dlgLoggingLevelEinstellung";
			StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			Text = "Logging Level Settings";
			grpBasic.ResumeLayout(performLayout: false);
			grpModule.ResumeLayout(performLayout: false);
			grpGlobal.ResumeLayout(performLayout: false);
			pnlForm.ResumeLayout(performLayout: false);
			ResumeLayout(performLayout: false);
		}

		private void dlgLoggingLevelEinstellung_Load(object sender, EventArgs e)
		{
			ToolTipDialog.SetToolTip(btnSchliessen, "Close");
			ToolTipDialog.SetToolTip(btnUebernehmen, "Apply");
			ToolTipDialog.SetToolTip(btnLoeschen, "Delete");
			ToolTipDialog.SetToolTip(lblNameDateiMitPfadLogging, "Name and location of the logging file");
			m_enmLoggingLevelTemp = m_enmLoggingLevelAktuell;
			SUB_LoggingLevelAuswerten();
		}

		private void tcbLogging_Basis_EVT_CheckStateChanged(object sender, EventArgs e)
		{
			ENUM_LoggingLevels eNUM_LoggingLevels = ENUM_LoggingLevels.Kein;
			try
			{
				bool flag = false;
				string empty = string.Empty;
				uclCheckBox uclCheckBox = (uclCheckBox)sender;
				string name = uclCheckBox.Name;
				if (Operators.CompareString(name, tcbKein.Name, TextCompare: false) == 0)
				{
					eNUM_LoggingLevels = (ENUM_LoggingLevels)((uclCheckBox.PRO_intCheckState != CheckState.Checked) ? 1 : 0);
				}
				else if (Operators.CompareString(name, tcbAlleBasis.Name, TextCompare: false) == 0)
				{
					eNUM_LoggingLevels = ENUM_LoggingLevels.AlleBasis;
				}
				else if (Operators.CompareString(name, tcbAlleErweitert.Name, TextCompare: false) == 0)
				{
					eNUM_LoggingLevels = ENUM_LoggingLevels.AlleErweitert;
				}
				else if (Operators.CompareString(name, tcbAlle.Name, TextCompare: false) == 0)
				{
					eNUM_LoggingLevels = ENUM_LoggingLevels.AlleBasis;
					if (m_blnLoggingErweitert)
					{
						eNUM_LoggingLevels = ENUM_LoggingLevels.Alle;
					}
				}
				if (uclCheckBox.PRO_intCheckState == CheckState.Checked)
				{
					if (Operators.CompareString(uclCheckBox.Name, tcbKein.Name, TextCompare: false) == 0)
					{
						m_enmLoggingLevelTemp = eNUM_LoggingLevels;
					}
					else
					{
						m_enmLoggingLevelTemp |= eNUM_LoggingLevels;
					}
				}
				else if (uclCheckBox.PRO_intCheckState == CheckState.Unchecked)
				{
					if (Operators.CompareString(uclCheckBox.Name, tcbKein.Name, TextCompare: false) == 0)
					{
						m_enmLoggingLevelTemp = eNUM_LoggingLevels;
					}
					else
					{
						m_enmLoggingLevelTemp = ((m_enmLoggingLevelTemp & ~eNUM_LoggingLevels) | ENUM_LoggingLevels.Fehler);
					}
				}
				SUB_LoggingLevelAuswerten();
				flag = true;
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Interaction.MsgBox(MethodBase.GetCurrentMethod().DeclaringType.Namespace + ": " + MethodBase.GetCurrentMethod().DeclaringType.Name + " - " + MethodBase.GetCurrentMethod().Name + ": \r\n\r\n" + ex2.ToString(), MsgBoxStyle.Critical, MethodBase.GetCurrentMethod().DeclaringType.Namespace);
				ProjectData.ClearProjectError();
			}
		}

		private void tcbLogging_Erweitert_EVT_CheckStateChanged(object sender, EventArgs e)
		{
			ENUM_LoggingLevels eNUM_LoggingLevels = ENUM_LoggingLevels.Kein;
			try
			{
				bool flag = false;
				string empty = string.Empty;
				uclCheckBox uclCheckBox = (uclCheckBox)sender;
				string name = uclCheckBox.Name;
				if (Operators.CompareString(name, tcbWarnung.Name, TextCompare: false) == 0)
				{
					eNUM_LoggingLevels = ENUM_LoggingLevels.Warnung;
				}
				else if (Operators.CompareString(name, tcbInfo.Name, TextCompare: false) == 0)
				{
					eNUM_LoggingLevels = ENUM_LoggingLevels.Info;
				}
				else if (Operators.CompareString(name, tcbTraceability.Name, TextCompare: false) == 0)
				{
					eNUM_LoggingLevels = ENUM_LoggingLevels.Traceability;
				}
				else if (Operators.CompareString(name, tcbEinlauf.Name, TextCompare: false) == 0)
				{
					eNUM_LoggingLevels = ENUM_LoggingLevels.Einlauf;
				}
				else if (Operators.CompareString(name, tcbEinlaufModul.Name, TextCompare: false) == 0)
				{
					eNUM_LoggingLevels = ENUM_LoggingLevels.Einlaufmodul;
				}
				else if (Operators.CompareString(name, tcbFluxerModul.Name, TextCompare: false) == 0)
				{
					eNUM_LoggingLevels = ENUM_LoggingLevels.Fluxermodul;
				}
				else if (Operators.CompareString(name, tcbVorheizModul.Name, TextCompare: false) == 0)
				{
					eNUM_LoggingLevels = ENUM_LoggingLevels.Vorheizmodul;
				}
				else if (Operators.CompareString(name, tcbLoetModul.Name, TextCompare: false) == 0)
				{
					eNUM_LoggingLevels = ENUM_LoggingLevels.Loetmodul;
				}
				else if (Operators.CompareString(name, tcbAuslaufModul.Name, TextCompare: false) == 0)
				{
					eNUM_LoggingLevels = ENUM_LoggingLevels.Auslaufmodul;
				}
				else if (Operators.CompareString(name, tcbAuslauf.Name, TextCompare: false) == 0)
				{
					eNUM_LoggingLevels = ENUM_LoggingLevels.Auslauf;
				}
				else if (Operators.CompareString(name, tcbProtokoll.Name, TextCompare: false) == 0)
				{
					eNUM_LoggingLevels = ENUM_LoggingLevels.Protokoll;
				}
				else if (Operators.CompareString(name, tcbCodebetrieb.Name, TextCompare: false) == 0)
				{
					eNUM_LoggingLevels = ENUM_LoggingLevels.Codebetrieb;
				}
				else if (Operators.CompareString(name, tcbKonfiguration.Name, TextCompare: false) == 0)
				{
					eNUM_LoggingLevels = ENUM_LoggingLevels.Konfiguration;
				}
				else if (Operators.CompareString(name, tcbLoetProgrammEditor.Name, TextCompare: false) == 0)
				{
					eNUM_LoggingLevels = ENUM_LoggingLevels.LoetProgrammEditor;
				}
				else if (Operators.CompareString(name, tcbDatensicherung.Name, TextCompare: false) == 0)
				{
					eNUM_LoggingLevels = ENUM_LoggingLevels.Datensicherung;
				}
				else if (Operators.CompareString(name, tcbProzessSchreiber.Name, TextCompare: false) == 0)
				{
					eNUM_LoggingLevels = ENUM_LoggingLevels.ProzessSchreiber;
				}
				else if (Operators.CompareString(name, tcbPCBDurchlauf.Name, TextCompare: false) == 0)
				{
					eNUM_LoggingLevels = ENUM_LoggingLevels.PCBDurchlauf;
				}
				else if (Operators.CompareString(name, tcbLoetProgramm.Name, TextCompare: false) == 0)
				{
					eNUM_LoggingLevels = ENUM_LoggingLevels.LoetProgramm;
				}
				else if (Operators.CompareString(name, tcbUebersicht.Name, TextCompare: false) == 0)
				{
					eNUM_LoggingLevels = ENUM_LoggingLevels.Uebersicht;
				}
				else if (Operators.CompareString(name, tcbBasisklasse.Name, TextCompare: false) == 0)
				{
					eNUM_LoggingLevels = ENUM_LoggingLevels.Basisklasse;
				}
				else if (Operators.CompareString(name, tcbLeiterkarte.Name, TextCompare: false) == 0)
				{
					eNUM_LoggingLevels = ENUM_LoggingLevels.Leiterkarte;
				}
				else if (Operators.CompareString(name, tcbRueckTransportModul.Name, TextCompare: false) == 0)
				{
					eNUM_LoggingLevels = ENUM_LoggingLevels.Ruecktransportmodul;
				}
				else if (Operators.CompareString(name, tcbHeizung.Name, TextCompare: false) == 0)
				{
					eNUM_LoggingLevels = ENUM_LoggingLevels.Heizung;
				}
				else if (Operators.CompareString(name, tcbFluxer.Name, TextCompare: false) == 0)
				{
					eNUM_LoggingLevels = ENUM_LoggingLevels.Fluxer;
				}
				else if (Operators.CompareString(name, tcbLoetEinheit.Name, TextCompare: false) == 0)
				{
					eNUM_LoggingLevels = ENUM_LoggingLevels.Loeteinheit;
				}
				else if (Operators.CompareString(name, tcbCNC.Name, TextCompare: false) == 0)
				{
					eNUM_LoggingLevels = ENUM_LoggingLevels.CNC;
				}
				if (uclCheckBox.PRO_intCheckState == CheckState.Checked)
				{
					m_enmLoggingLevelTemp |= eNUM_LoggingLevels;
				}
				else if (uclCheckBox.PRO_intCheckState == CheckState.Unchecked)
				{
					m_enmLoggingLevelTemp &= ~eNUM_LoggingLevels;
				}
				SUB_LoggingLevelAuswerten();
				flag = true;
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Interaction.MsgBox(MethodBase.GetCurrentMethod().DeclaringType.Namespace + ": " + MethodBase.GetCurrentMethod().DeclaringType.Name + " - " + MethodBase.GetCurrentMethod().Name + ": \r\n\r\n" + ex2.ToString(), MsgBoxStyle.Critical, MethodBase.GetCurrentMethod().DeclaringType.Namespace);
				ProjectData.ClearProjectError();
			}
		}

		private void btnSchliessen_Click(object sender, EventArgs e)
		{
			if (m_blnEinstellungenUebernehmen)
			{
				DialogResult = DialogResult.OK;
			}
			else
			{
				DialogResult = DialogResult.Abort;
			}
		}

		private void btnUebernehmen_Click(object sender, EventArgs e)
		{
			m_blnEinstellungenUebernehmen = true;
			m_enmLoggingLevelAktuell = m_enmLoggingLevelTemp;
		}

		private void SUB_LoggingLevelSetzenGruppeAlleBasis(CheckState i_intCheckState)
		{
			tcbAlleBasis.PRO_intCheckState = i_intCheckState;
			tcbFehler.PRO_intCheckState = (CheckState)Conversions.ToInteger(Interaction.IIf(tcbKein.PRO_intCheckState == CheckState.Checked, CheckState.Unchecked, CheckState.Checked));
			tcbWarnung.PRO_intCheckState = i_intCheckState;
			tcbInfo.PRO_intCheckState = i_intCheckState;
		}

		private void SUB_LoggingLevelSetzenGruppeAlleErweitert(CheckState i_intCheckState)
		{
			tcbAlleErweitert.PRO_intCheckState = i_intCheckState;
			tcbTraceability.PRO_intCheckState = i_intCheckState;
			tcbEinlauf.PRO_intCheckState = i_intCheckState;
			tcbEinlaufModul.PRO_intCheckState = i_intCheckState;
			tcbFluxerModul.PRO_intCheckState = i_intCheckState;
			tcbVorheizModul.PRO_intCheckState = i_intCheckState;
			tcbLoetModul.PRO_intCheckState = i_intCheckState;
			tcbAuslaufModul.PRO_intCheckState = i_intCheckState;
			tcbAuslauf.PRO_intCheckState = i_intCheckState;
			tcbProtokoll.PRO_intCheckState = i_intCheckState;
			tcbCodebetrieb.PRO_intCheckState = i_intCheckState;
			tcbKonfiguration.PRO_intCheckState = i_intCheckState;
			tcbLoetProgrammEditor.PRO_intCheckState = i_intCheckState;
			tcbDatensicherung.PRO_intCheckState = i_intCheckState;
			tcbProzessSchreiber.PRO_intCheckState = i_intCheckState;
			tcbPCBDurchlauf.PRO_intCheckState = i_intCheckState;
			tcbLoetProgramm.PRO_intCheckState = i_intCheckState;
			tcbUebersicht.PRO_intCheckState = i_intCheckState;
			tcbBasisklasse.PRO_intCheckState = i_intCheckState;
			tcbLeiterkarte.PRO_intCheckState = i_intCheckState;
			tcbRueckTransportModul.PRO_intCheckState = i_intCheckState;
			tcbHeizung.PRO_intCheckState = i_intCheckState;
			tcbFluxer.PRO_intCheckState = i_intCheckState;
			tcbLoetEinheit.PRO_intCheckState = i_intCheckState;
			tcbCNC.PRO_intCheckState = i_intCheckState;
		}

		private void SUB_ControlsKonfigurieren()
		{
			if (tcbKein.PRO_intCheckState == CheckState.Checked)
			{
				grpBasic.Enabled = false;
				grpModule.Enabled = false;
				tcbAlleBasis.Enabled = false;
				tcbAlleErweitert.Enabled = false;
				tcbAlle.Enabled = false;
			}
			else
			{
				grpBasic.Enabled = true;
				grpModule.Enabled = true;
				tcbAlleBasis.Enabled = true;
				tcbAlleErweitert.Enabled = true;
				tcbAlle.Enabled = true;
			}
		}

		private void SUB_LoggingLevelAuswerten()
		{
			CheckState r_intCheckStateErgebnis = CheckState.Unchecked;
			EDC_ERSALogging.FUN_blnLoggingLevelUeberpruefen(ENUM_LoggingLevels.Kein, m_enmLoggingLevelTemp, ref r_intCheckStateErgebnis);
			tcbKein.PRO_intCheckState = r_intCheckStateErgebnis;
			if (r_intCheckStateErgebnis == CheckState.Checked)
			{
				tcbAlle.PRO_intCheckState = CheckState.Unchecked;
				SUB_LoggingLevelSetzenGruppeAlleBasis(CheckState.Unchecked);
				SUB_LoggingLevelSetzenGruppeAlleErweitert(CheckState.Unchecked);
			}
			else
			{
				tcbFehler.PRO_intCheckState = CheckState.Checked;
			}
			SUB_ControlsKonfigurieren();
			SUB_LoggingLevelWertAktualisieren();
			if (tcbKein.PRO_intCheckState != CheckState.Checked)
			{
				EDC_ERSALogging.FUN_blnLoggingLevelUeberpruefen(ENUM_LoggingLevels.Warnung, m_enmLoggingLevelTemp, ref r_intCheckStateErgebnis);
				tcbWarnung.PRO_intCheckState = r_intCheckStateErgebnis;
				EDC_ERSALogging.FUN_blnLoggingLevelUeberpruefen(ENUM_LoggingLevels.Info, m_enmLoggingLevelTemp, ref r_intCheckStateErgebnis);
				tcbInfo.PRO_intCheckState = r_intCheckStateErgebnis;
				EDC_ERSALogging.FUN_blnLoggingLevelUeberpruefen(ENUM_LoggingLevels.AlleBasis, m_enmLoggingLevelTemp, ref r_intCheckStateErgebnis);
				tcbAlleBasis.PRO_intCheckState = r_intCheckStateErgebnis;
				if (m_blnLoggingErweitert)
				{
					EDC_ERSALogging.FUN_blnLoggingLevelUeberpruefen(ENUM_LoggingLevels.Traceability, m_enmLoggingLevelTemp, ref r_intCheckStateErgebnis);
					tcbTraceability.PRO_intCheckState = r_intCheckStateErgebnis;
					EDC_ERSALogging.FUN_blnLoggingLevelUeberpruefen(ENUM_LoggingLevels.Einlauf, m_enmLoggingLevelTemp, ref r_intCheckStateErgebnis);
					tcbEinlauf.PRO_intCheckState = r_intCheckStateErgebnis;
					EDC_ERSALogging.FUN_blnLoggingLevelUeberpruefen(ENUM_LoggingLevels.Einlaufmodul, m_enmLoggingLevelTemp, ref r_intCheckStateErgebnis);
					tcbEinlaufModul.PRO_intCheckState = r_intCheckStateErgebnis;
					EDC_ERSALogging.FUN_blnLoggingLevelUeberpruefen(ENUM_LoggingLevels.Fluxermodul, m_enmLoggingLevelTemp, ref r_intCheckStateErgebnis);
					tcbFluxerModul.PRO_intCheckState = r_intCheckStateErgebnis;
					EDC_ERSALogging.FUN_blnLoggingLevelUeberpruefen(ENUM_LoggingLevels.Vorheizmodul, m_enmLoggingLevelTemp, ref r_intCheckStateErgebnis);
					tcbVorheizModul.PRO_intCheckState = r_intCheckStateErgebnis;
					EDC_ERSALogging.FUN_blnLoggingLevelUeberpruefen(ENUM_LoggingLevels.Loetmodul, m_enmLoggingLevelTemp, ref r_intCheckStateErgebnis);
					tcbLoetModul.PRO_intCheckState = r_intCheckStateErgebnis;
					EDC_ERSALogging.FUN_blnLoggingLevelUeberpruefen(ENUM_LoggingLevels.Auslaufmodul, m_enmLoggingLevelTemp, ref r_intCheckStateErgebnis);
					tcbAuslaufModul.PRO_intCheckState = r_intCheckStateErgebnis;
					EDC_ERSALogging.FUN_blnLoggingLevelUeberpruefen(ENUM_LoggingLevels.Auslauf, m_enmLoggingLevelTemp, ref r_intCheckStateErgebnis);
					tcbAuslauf.PRO_intCheckState = r_intCheckStateErgebnis;
					EDC_ERSALogging.FUN_blnLoggingLevelUeberpruefen(ENUM_LoggingLevels.Protokoll, m_enmLoggingLevelTemp, ref r_intCheckStateErgebnis);
					tcbProtokoll.PRO_intCheckState = r_intCheckStateErgebnis;
					EDC_ERSALogging.FUN_blnLoggingLevelUeberpruefen(ENUM_LoggingLevels.Codebetrieb, m_enmLoggingLevelTemp, ref r_intCheckStateErgebnis);
					tcbCodebetrieb.PRO_intCheckState = r_intCheckStateErgebnis;
					EDC_ERSALogging.FUN_blnLoggingLevelUeberpruefen(ENUM_LoggingLevels.Konfiguration, m_enmLoggingLevelTemp, ref r_intCheckStateErgebnis);
					tcbKonfiguration.PRO_intCheckState = r_intCheckStateErgebnis;
					EDC_ERSALogging.FUN_blnLoggingLevelUeberpruefen(ENUM_LoggingLevels.LoetProgrammEditor, m_enmLoggingLevelTemp, ref r_intCheckStateErgebnis);
					tcbLoetProgrammEditor.PRO_intCheckState = r_intCheckStateErgebnis;
					EDC_ERSALogging.FUN_blnLoggingLevelUeberpruefen(ENUM_LoggingLevels.Datensicherung, m_enmLoggingLevelTemp, ref r_intCheckStateErgebnis);
					tcbDatensicherung.PRO_intCheckState = r_intCheckStateErgebnis;
					EDC_ERSALogging.FUN_blnLoggingLevelUeberpruefen(ENUM_LoggingLevels.ProzessSchreiber, m_enmLoggingLevelTemp, ref r_intCheckStateErgebnis);
					tcbProzessSchreiber.PRO_intCheckState = r_intCheckStateErgebnis;
					EDC_ERSALogging.FUN_blnLoggingLevelUeberpruefen(ENUM_LoggingLevels.PCBDurchlauf, m_enmLoggingLevelTemp, ref r_intCheckStateErgebnis);
					tcbPCBDurchlauf.PRO_intCheckState = r_intCheckStateErgebnis;
					EDC_ERSALogging.FUN_blnLoggingLevelUeberpruefen(ENUM_LoggingLevels.LoetProgramm, m_enmLoggingLevelTemp, ref r_intCheckStateErgebnis);
					tcbLoetProgramm.PRO_intCheckState = r_intCheckStateErgebnis;
					EDC_ERSALogging.FUN_blnLoggingLevelUeberpruefen(ENUM_LoggingLevels.Uebersicht, m_enmLoggingLevelTemp, ref r_intCheckStateErgebnis);
					tcbUebersicht.PRO_intCheckState = r_intCheckStateErgebnis;
					EDC_ERSALogging.FUN_blnLoggingLevelUeberpruefen(ENUM_LoggingLevels.Basisklasse, m_enmLoggingLevelTemp, ref r_intCheckStateErgebnis);
					tcbBasisklasse.PRO_intCheckState = r_intCheckStateErgebnis;
					EDC_ERSALogging.FUN_blnLoggingLevelUeberpruefen(ENUM_LoggingLevels.Leiterkarte, m_enmLoggingLevelTemp, ref r_intCheckStateErgebnis);
					tcbLeiterkarte.PRO_intCheckState = r_intCheckStateErgebnis;
					EDC_ERSALogging.FUN_blnLoggingLevelUeberpruefen(ENUM_LoggingLevels.Ruecktransportmodul, m_enmLoggingLevelTemp, ref r_intCheckStateErgebnis);
					tcbRueckTransportModul.PRO_intCheckState = r_intCheckStateErgebnis;
					EDC_ERSALogging.FUN_blnLoggingLevelUeberpruefen(ENUM_LoggingLevels.Heizung, m_enmLoggingLevelTemp, ref r_intCheckStateErgebnis);
					tcbHeizung.PRO_intCheckState = r_intCheckStateErgebnis;
					EDC_ERSALogging.FUN_blnLoggingLevelUeberpruefen(ENUM_LoggingLevels.Fluxer, m_enmLoggingLevelTemp, ref r_intCheckStateErgebnis);
					tcbFluxer.PRO_intCheckState = r_intCheckStateErgebnis;
					EDC_ERSALogging.FUN_blnLoggingLevelUeberpruefen(ENUM_LoggingLevels.Loeteinheit, m_enmLoggingLevelTemp, ref r_intCheckStateErgebnis);
					tcbLoetEinheit.PRO_intCheckState = r_intCheckStateErgebnis;
					EDC_ERSALogging.FUN_blnLoggingLevelUeberpruefen(ENUM_LoggingLevels.CNC, m_enmLoggingLevelTemp, ref r_intCheckStateErgebnis);
					tcbCNC.PRO_intCheckState = r_intCheckStateErgebnis;
					EDC_ERSALogging.FUN_blnLoggingLevelUeberpruefen(ENUM_LoggingLevels.AlleErweitert, m_enmLoggingLevelTemp, ref r_intCheckStateErgebnis);
					tcbAlleErweitert.PRO_intCheckState = r_intCheckStateErgebnis;
					EDC_ERSALogging.FUN_blnLoggingLevelUeberpruefen(ENUM_LoggingLevels.Alle, m_enmLoggingLevelTemp, ref r_intCheckStateErgebnis);
					tcbAlle.PRO_intCheckState = r_intCheckStateErgebnis;
				}
				else
				{
					EDC_ERSALogging.FUN_blnLoggingLevelUeberpruefen(ENUM_LoggingLevels.AlleBasis, m_enmLoggingLevelTemp, ref r_intCheckStateErgebnis);
					tcbAlle.PRO_intCheckState = r_intCheckStateErgebnis;
				}
				SUB_LoggingLevelWertAktualisieren();
			}
		}

		private void SUB_LoggingLevelWertAktualisieren()
		{
			lblLevelDecimal.Text = m_enmLoggingLevelTemp.ToString("d");
			lblLevelHex.Text = m_enmLoggingLevelTemp.ToString("X");
		}
	}
}
