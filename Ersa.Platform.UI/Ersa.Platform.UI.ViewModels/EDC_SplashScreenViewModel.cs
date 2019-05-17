using Ersa.Global.Mvvm;
using Ersa.Platform.Infrastructure;
using Ersa.Platform.UI.Common.Interfaces;
using Ersa.Platform.UI.Interfaces;
using Ersa.Platform.UI.Splash;
using Prism.Commands;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace Ersa.Platform.UI.ViewModels
{
	[Export]
	[Export(typeof(INF_SplashScreenViewModel))]
	[Export(typeof(INF_ModulStatusViewModel))]
	public class EDC_SplashScreenViewModel : BindableBase, INF_SplashScreenViewModel, INF_ModulStatusViewModel, INotifyPropertyChanged, IDisposable
	{
		private Dispatcher m_fdcDispatcher;

		private bool m_blnIsDisposed;

		private ObservableCollection<EDC_ModulStatus> m_lstModule = new ObservableCollection<EDC_ModulStatus>();

		private Visibility m_fdcLeftButtonVisiblity = Visibility.Collapsed;

		private Visibility m_fdcRightButtonVisiblity = Visibility.Collapsed;

		private Visibility m_fdcMeldungControlVisibility = Visibility.Collapsed;

		private string m_strMeldungsTextKey;

		private string m_strSoftwareVersion;

		private string m_strHerstellerInfo;

		private string m_strMaschinenTyp;

		private string m_strMaschinenIp;

		private string m_strVerbindungsModusKey;

		private string m_strMeldungKopf;

		private string m_strMeldungIndex;

		private string m_strMeldung;

		private string m_strButtonLeft;

		private string m_strButtonRight;

		private int m_i32SSplashDauerZaehler;

		private bool m_blnIstFehlerMeldung;

		private bool m_blnIstInMeldungModus;

		private TaskCompletionSource<bool> m_fdcWarteBisEingabeEnde = new TaskCompletionSource<bool>();

		public ICommand PRO_cmdLeftButton
		{
			get;
			set;
		}

		public ICommand PRO_cmdRightButton
		{
			get;
			set;
		}

		public Action PRO_delLeftButtonAction
		{
			get;
			set;
		}

		public Action PRO_delRightButtonAction
		{
			get;
			set;
		}

		public bool PRO_blnIstInMeldungModus
		{
			get
			{
				return m_blnIstInMeldungModus;
			}
			set
			{
				m_blnIstInMeldungModus = value;
				RaisePropertyChanged("PRO_blnIstInMeldungModus");
				PRO_fdcMeldungControlVisibility = ((!m_blnIstInMeldungModus) ? Visibility.Collapsed : Visibility.Visible);
			}
		}

		public Visibility PRO_fdcMeldungControlVisibility
		{
			get
			{
				return m_fdcMeldungControlVisibility;
			}
			set
			{
				m_fdcMeldungControlVisibility = value;
				RaisePropertyChanged("PRO_fdcMeldungControlVisibility");
			}
		}

		public Visibility PRO_fdcLeftButtonVisibility
		{
			get
			{
				return m_fdcLeftButtonVisiblity;
			}
			set
			{
				m_fdcLeftButtonVisiblity = value;
				RaisePropertyChanged("PRO_fdcLeftButtonVisibility");
			}
		}

		public Visibility PRO_fdcRightButtonVisibility
		{
			get
			{
				return m_fdcRightButtonVisiblity;
			}
			set
			{
				m_fdcRightButtonVisiblity = value;
				RaisePropertyChanged("PRO_fdcRightButtonVisibility");
			}
		}

		public ObservableCollection<EDC_ModulStatus> PRO_lstModule
		{
			get
			{
				return m_lstModule;
			}
			set
			{
				m_lstModule = value;
				RaisePropertyChanged("PRO_lstModule");
			}
		}

		public int PRO_i32SplashDauerZaehler
		{
			get
			{
				return m_i32SSplashDauerZaehler;
			}
			private set
			{
				m_i32SSplashDauerZaehler = value;
				RaisePropertyChanged("PRO_i32SplashDauerZaehler");
			}
		}

		public string PRO_strMeldungsTextKey
		{
			get
			{
				return m_strMeldungsTextKey;
			}
			set
			{
				m_fdcDispatcher?.BeginInvoke((Action)delegate
				{
					if (!(m_strMeldungsTextKey == value))
					{
						m_strMeldungsTextKey = value;
						RaisePropertyChanged("PRO_strMeldungsTextKey");
					}
				});
			}
		}

		public string PRO_strSoftwareVersion
		{
			get
			{
				return m_strSoftwareVersion;
			}
			set
			{
				m_fdcDispatcher?.BeginInvoke((Action)delegate
				{
					if (!(m_strSoftwareVersion == value))
					{
						m_strSoftwareVersion = value;
						RaisePropertyChanged("PRO_strSoftwareVersion");
					}
				});
			}
		}

		public string PRO_strHerstellerInfo
		{
			get
			{
				return m_strHerstellerInfo;
			}
			set
			{
				m_fdcDispatcher?.BeginInvoke((Action)delegate
				{
					if (!(m_strHerstellerInfo == value))
					{
						m_strHerstellerInfo = value;
						RaisePropertyChanged("PRO_strHerstellerInfo");
					}
				});
			}
		}

		public string PRO_strMaschinenTyp
		{
			get
			{
				return m_strMaschinenTyp;
			}
			set
			{
				m_fdcDispatcher?.BeginInvoke((Action)delegate
				{
					if (!(m_strMaschinenTyp == value))
					{
						m_strMaschinenTyp = value;
						RaisePropertyChanged("PRO_strMaschinenTyp");
					}
				});
			}
		}

		public string PRO_strMaschinenIp
		{
			get
			{
				return m_strMaschinenIp;
			}
			set
			{
				m_fdcDispatcher?.BeginInvoke((Action)delegate
				{
					if (!(m_strMaschinenIp == value))
					{
						m_strMaschinenIp = value;
						RaisePropertyChanged("PRO_strMaschinenIp");
					}
				});
			}
		}

		public string PRO_strVerbindungsModusKey
		{
			get
			{
				return m_strVerbindungsModusKey;
			}
			set
			{
				m_fdcDispatcher?.BeginInvoke((Action)delegate
				{
					m_strVerbindungsModusKey = value;
					RaisePropertyChanged("PRO_strVerbindungsModusKey");
				});
			}
		}

		public string PRO_strMeldungKopf
		{
			get
			{
				return m_strMeldungKopf;
			}
			set
			{
				m_strMeldungKopf = value;
				RaisePropertyChanged("PRO_strMeldungKopf");
			}
		}

		public string PRO_strMeldungIndex
		{
			get
			{
				return m_strMeldungIndex;
			}
			set
			{
				m_strMeldungIndex = value;
				RaisePropertyChanged("PRO_strMeldungIndex");
			}
		}

		public string PRO_strMeldung
		{
			get
			{
				return m_strMeldung;
			}
			set
			{
				m_strMeldung = value;
				RaisePropertyChanged("PRO_strMeldung");
			}
		}

		public string PRO_strButtonLeft
		{
			get
			{
				return m_strButtonLeft;
			}
			set
			{
				m_strButtonLeft = value;
				RaisePropertyChanged("PRO_strButtonLeft");
			}
		}

		public string PRO_strButtonRight
		{
			get
			{
				return m_strButtonRight;
			}
			set
			{
				m_strButtonRight = value;
				RaisePropertyChanged("PRO_strButtonRight");
			}
		}

		public bool PRO_blnIstFehlerMeldung
		{
			get
			{
				return m_blnIstFehlerMeldung;
			}
			set
			{
				m_blnIstFehlerMeldung = value;
				RaisePropertyChanged("PRO_blnIstFehlerMeldung");
			}
		}

		private DispatcherTimer PRO_fdcDispatcherTimer
		{
			get;
			set;
		}

		public EDC_SplashScreenViewModel()
		{
			PRO_cmdLeftButton = new DelegateCommand(SUB_ExecuteLeftButtonAction, () => true);
			PRO_cmdRightButton = new DelegateCommand(SUB_ExecuteRightButtonAction, () => true);
		}

		~EDC_SplashScreenViewModel()
		{
			SUB_InternalDispose(i_blnDisposing: false);
		}

		public void Dispose()
		{
			SUB_InternalDispose(i_blnDisposing: true);
			GC.SuppressFinalize(this);
		}

		public void SUB_InitialisiereDasModel(Dispatcher i_fdcDispatcher)
		{
			m_fdcDispatcher = i_fdcDispatcher;
			m_fdcDispatcher.BeginInvoke((Action)delegate
			{
				PRO_fdcDispatcherTimer = new DispatcherTimer(DispatcherPriority.Send);
				PRO_fdcDispatcherTimer.Tick += FUN_delDispatchTimerHandler;
				PRO_fdcDispatcherTimer.Interval = new TimeSpan(0, 0, 1);
				PRO_fdcDispatcherTimer.Start();
			});
		}

		public void SUB_AktualisiereDasModul(string i_strModulNameKey, ENUM_ModulStatus i_enmModulStatus, string i_strMeldungKey)
		{
			m_fdcDispatcher?.BeginInvoke((Action)delegate
			{
				EDC_ModulStatus eDC_ModulStatus = FUN_fdcFindeDasModulBeimNamen(i_strModulNameKey);
				eDC_ModulStatus.PRO_enmModulStatus = i_enmModulStatus;
				eDC_ModulStatus.PRO_strMeldungKey = i_strMeldungKey;
				RaisePropertyChanged("PRO_lstModule");
			});
		}

		public void SUB_ZeigeMeldung(EDC_SplashMeldung i_edcSplahMeldung)
		{
			if (m_fdcDispatcher != null && !PRO_blnIstInMeldungModus)
			{
				m_fdcWarteBisEingabeEnde = new TaskCompletionSource<bool>();
				m_fdcDispatcher.BeginInvoke((Action)delegate
				{
					PRO_strMeldungKopf = i_edcSplahMeldung.PRO_strMeldungKopfIndex;
					PRO_strMeldung = i_edcSplahMeldung.PRO_strMeldung;
					PRO_strMeldungIndex = i_edcSplahMeldung.PRO_strMeldungIndex;
					PRO_strButtonLeft = i_edcSplahMeldung.PRO_strLeftButtonIndex;
					PRO_strButtonRight = i_edcSplahMeldung.PRO_strRightButtonIndex;
					PRO_delLeftButtonAction = i_edcSplahMeldung.PRO_delLeftButtonAction;
					PRO_fdcLeftButtonVisibility = ((PRO_delLeftButtonAction == null) ? Visibility.Collapsed : Visibility.Visible);
					PRO_delRightButtonAction = i_edcSplahMeldung.PRO_delRightButtonAction;
					PRO_fdcRightButtonVisibility = ((PRO_delRightButtonAction == null) ? Visibility.Collapsed : Visibility.Visible);
					PRO_blnIstInMeldungModus = true;
					PRO_blnIstFehlerMeldung = i_edcSplahMeldung.PRO_blnIstFehlerMeldung;
				});
			}
		}

		public async Task<bool> FUN_fdcWarteAufAntwortAsync(EDC_SplashMeldung i_edcSplahMeldung)
		{
			if (m_fdcDispatcher == null)
			{
				return false;
			}
			if (PRO_blnIstInMeldungModus)
			{
				return false;
			}
			m_fdcWarteBisEingabeEnde = new TaskCompletionSource<bool>();
			await m_fdcDispatcher.BeginInvoke((Action)delegate
			{
				PRO_strMeldungKopf = i_edcSplahMeldung.PRO_strMeldungKopfIndex;
				PRO_strMeldung = i_edcSplahMeldung.PRO_strMeldung;
				PRO_strMeldungIndex = i_edcSplahMeldung.PRO_strMeldungIndex;
				PRO_strButtonLeft = i_edcSplahMeldung.PRO_strLeftButtonIndex;
				PRO_strButtonRight = i_edcSplahMeldung.PRO_strRightButtonIndex;
				PRO_delLeftButtonAction = i_edcSplahMeldung.PRO_delLeftButtonAction;
				PRO_fdcLeftButtonVisibility = ((PRO_delLeftButtonAction == null) ? Visibility.Collapsed : Visibility.Visible);
				PRO_delRightButtonAction = i_edcSplahMeldung.PRO_delRightButtonAction;
				PRO_fdcRightButtonVisibility = ((PRO_delRightButtonAction == null) ? Visibility.Collapsed : Visibility.Visible);
				PRO_blnIstInMeldungModus = true;
				PRO_blnIstFehlerMeldung = i_edcSplahMeldung.PRO_blnIstFehlerMeldung;
			});
			return await m_fdcWarteBisEingabeEnde.Task;
		}

		public async Task SUB_EntferneVorhandeneMeldungAsync()
		{
			if (m_fdcDispatcher != null && PRO_blnIstInMeldungModus)
			{
				await m_fdcDispatcher.BeginInvoke((Action)delegate
				{
					PRO_strMeldungKopf = string.Empty;
					PRO_strMeldung = string.Empty;
					PRO_strMeldungIndex = string.Empty;
					PRO_strButtonLeft = string.Empty;
					PRO_strButtonRight = string.Empty;
					PRO_delLeftButtonAction = null;
					PRO_fdcLeftButtonVisibility = Visibility.Collapsed;
					PRO_delRightButtonAction = null;
					PRO_fdcRightButtonVisibility = Visibility.Collapsed;
					PRO_blnIstInMeldungModus = false;
				});
			}
		}

		protected virtual void SUB_InternalDispose(bool i_blnDisposing)
		{
			if (!m_blnIsDisposed && i_blnDisposing)
			{
				if (PRO_fdcDispatcherTimer == null)
				{
					return;
				}
				PRO_fdcDispatcherTimer.Stop();
				PRO_fdcDispatcherTimer.IsEnabled = false;
			}
			m_blnIsDisposed = true;
		}

		private void SUB_ExecuteRightButtonAction()
		{
			PRO_delRightButtonAction?.Invoke();
			m_fdcWarteBisEingabeEnde.SetResult(result: true);
		}

		private void SUB_ExecuteLeftButtonAction()
		{
			PRO_delLeftButtonAction?.Invoke();
			m_fdcWarteBisEingabeEnde.SetResult(result: false);
		}

		private EDC_ModulStatus FUN_fdcFindeDasModulBeimNamen(string i_strModulName)
		{
			foreach (EDC_ModulStatus item in PRO_lstModule)
			{
				if (item.PRO_strModulNameKey.Equals(i_strModulName))
				{
					return item;
				}
			}
			EDC_ModulStatus eDC_ModulStatus = new EDC_ModulStatus
			{
				PRO_strModulNameKey = i_strModulName
			};
			PRO_lstModule.Add(eDC_ModulStatus);
			return eDC_ModulStatus;
		}

		private void FUN_delDispatchTimerHandler(object i_objSender, EventArgs i_fdcArgs)
		{
			int num = ++PRO_i32SplashDauerZaehler;
			foreach (EDC_ModulStatus item in PRO_lstModule)
			{
				if (!item.PRO_enmModulStatus.Equals(ENUM_ModulStatus.enmCompleted))
				{
					num = ++item.PRO_i32Zaehler;
				}
			}
		}
	}
}
