using Ersa.Platform.Infrastructure.Prism;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Markup;

namespace Ersa.Platform.UI.VideoAnzeige
{
	public class EDU_VideoAnzeige : System.Windows.Controls.UserControl, IComponentConnector
	{
		public static readonly DependencyProperty ms_edcVideoDeviceVerwaltung = DependencyProperty.Register("PRO_edcVideoDeviceVerwaltung", typeof(INF_VideoDeviceVerwaltung), typeof(EDU_VideoAnzeige), new PropertyMetadata(SUB_VideoDeviceVerwaltungGeaendert));

		public static readonly DependencyProperty ms_i32VideoDeviceIndexProperty = DependencyProperty.Register("PRO_i32VideoDeviceIndex", typeof(int), typeof(EDU_VideoAnzeige), new PropertyMetadata(-1, SUB_VideoDeviceGeaendert));

		public static readonly DependencyProperty ms_blnFehlerZustandProperty = DependencyProperty.Register("PRO_blnFehlerZustand", typeof(bool), typeof(EDU_VideoAnzeige));

		public static readonly DependencyProperty ms_blnGestopptZustandProperty = DependencyProperty.Register("PRO_blnGestopptZustand", typeof(bool), typeof(EDU_VideoAnzeige));

		private const int mC_i32DefaultDeviceIndex = -1;

		internal Border brdRand;

		internal System.Windows.Forms.Panel panelVideo;

		private bool _contentLoaded;

		public INF_VideoDeviceVerwaltung PRO_edcVideoDeviceVerwaltung
		{
			get
			{
				return ((INF_VideoDeviceVerwaltung)GetValue(ms_edcVideoDeviceVerwaltung)) ?? EDC_ServiceLocator.PRO_edcInstanz.FUN_objObjektSicherAusContainerHolen<INF_VideoDeviceVerwaltung>();
			}
			set
			{
				SetValue(ms_edcVideoDeviceVerwaltung, value);
			}
		}

		public int PRO_i32VideoDeviceIndex
		{
			get
			{
				return (int)GetValue(ms_i32VideoDeviceIndexProperty);
			}
			set
			{
				SetValue(ms_i32VideoDeviceIndexProperty, value);
			}
		}

		public bool PRO_blnFehlerZustand
		{
			get
			{
				return (bool)GetValue(ms_blnFehlerZustandProperty);
			}
			set
			{
				SetValue(ms_blnFehlerZustandProperty, value);
			}
		}

		public bool PRO_blnGestopptZustand
		{
			get
			{
				return (bool)GetValue(ms_blnGestopptZustandProperty);
			}
			set
			{
				SetValue(ms_blnGestopptZustandProperty, value);
			}
		}

		public double PRO_dblBreiteUndHoehe
		{
			get
			{
				return panelVideo.Width;
			}
			set
			{
				ApplyTemplate();
				SUB_BreiteSetzen(value);
				SUB_HoeheAnhandBreiteSetzen(value);
			}
		}

		public EDU_VideoAnzeige()
		{
			InitializeComponent();
			base.IsVisibleChanged += SUB_OnIsVisibleChanged;
		}

		private static void SUB_VideoDeviceVerwaltungGeaendert(object i_objSender, DependencyPropertyChangedEventArgs i_fdcArgs)
		{
			EDU_VideoAnzeige eDU_VideoAnzeige = i_objSender as EDU_VideoAnzeige;
			if (eDU_VideoAnzeige != null && i_fdcArgs.OldValue == null)
			{
				eDU_VideoAnzeige.PRO_edcVideoDeviceVerwaltung.m_evtDeviceListeGeaendert += eDU_VideoAnzeige.SUB_DeviceListeAenderungBehandeln;
			}
		}

		private static void SUB_VideoDeviceGeaendert(object i_objSender, DependencyPropertyChangedEventArgs i_fdcArgs)
		{
			EDU_VideoAnzeige eDU_VideoAnzeige = i_objSender as EDU_VideoAnzeige;
			if (eDU_VideoAnzeige == null)
			{
				return;
			}
			int num = (int)i_fdcArgs.OldValue;
			if (FUN_blnVideoDeviceGeaendert(num, (int)i_fdcArgs.NewValue) && !eDU_VideoAnzeige.FUN_blnIstImFehlerZustand(num))
			{
				EDC_LiveDeviceSource eDC_LiveDeviceSource = eDU_VideoAnzeige.FUN_edcVideoDeviceErmitteln(num);
				if (eDC_LiveDeviceSource != null)
				{
					eDU_VideoAnzeige.PRO_edcVideoDeviceVerwaltung.SUB_VideoOutputFensterZuruecksetzen(eDC_LiveDeviceSource);
				}
			}
			eDU_VideoAnzeige.SUB_VideoOutputFensterSetzen();
		}

		private static bool FUN_blnVideoDeviceGeaendert(int i_i32AlterDeviceIndex, int i_i32NeuerDeviceIndex)
		{
			if (i_i32AlterDeviceIndex != -1 && i_i32NeuerDeviceIndex != -1)
			{
				return i_i32AlterDeviceIndex != i_i32NeuerDeviceIndex;
			}
			return false;
		}

		private void SUB_VideoOutputFensterSetzen()
		{
			if (base.IsVisible)
			{
				PRO_blnFehlerZustand = false;
				PRO_blnGestopptZustand = false;
				try
				{
					PRO_blnFehlerZustand = FUN_blnIstImFehlerZustand(PRO_i32VideoDeviceIndex);
					if (!PRO_blnFehlerZustand)
					{
						EDC_LiveDeviceSource eDC_LiveDeviceSource = FUN_edcVideoDeviceErmitteln(PRO_i32VideoDeviceIndex);
						if (eDC_LiveDeviceSource == null)
						{
							PRO_blnGestopptZustand = true;
						}
						else
						{
							PRO_edcVideoDeviceVerwaltung.SUB_SetzeVideoOutputFenster(eDC_LiveDeviceSource, new HandleRef(panelVideo, panelVideo.Handle));
						}
					}
				}
				catch (Exception)
				{
					PRO_blnFehlerZustand = true;
				}
			}
		}

		private bool FUN_blnIstImFehlerZustand(int i_i32DeviceIndex)
		{
			if (PRO_edcVideoDeviceVerwaltung != null)
			{
				return PRO_edcVideoDeviceVerwaltung.FUN_blnVideoDeviceImFehlerZustand(i_i32DeviceIndex);
			}
			return true;
		}

		private EDC_LiveDeviceSource FUN_edcVideoDeviceErmitteln(int i_i32DeviceIndex)
		{
			return PRO_edcVideoDeviceVerwaltung.FUN_fdcAktivesDeviceErmitteln(i_i32DeviceIndex) ?? PRO_edcVideoDeviceVerwaltung.FUN_fdcDeviceAktivieren(i_i32DeviceIndex);
		}

		private void SUB_OnIsVisibleChanged(object i_objSender, DependencyPropertyChangedEventArgs i_fdcArgs)
		{
			SUB_VideoOutputFensterSetzen();
		}

		private void SUB_DeviceListeAenderungBehandeln(object i_objSender, EDC_DeviceListeGeaendertArgs i_fdcArgs)
		{
			if (i_fdcArgs.PRO_enmDeviceAenderung == ENUM_VideoDeviceAenderung.enmHinzufuegen)
			{
				SUB_VideoOutputFensterSetzen();
			}
		}

		private void SUB_BreiteSetzen(double i_dblBreite)
		{
			int num = (int)Math.Round(i_dblBreite);
			panelVideo.Width = num;
			base.Width = num;
		}

		private void SUB_HoeheAnhandBreiteSetzen(double i_dblBreite)
		{
			int num = (int)Math.Round(i_dblBreite * 3.0 / 4.0);
			panelVideo.Height = num;
			base.Height = num;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			if (!_contentLoaded)
			{
				_contentLoaded = true;
				Uri resourceLocator = new Uri("/Ersa.Platform.UI;component/videoanzeige/edu_videoanzeige.xaml", UriKind.Relative);
				System.Windows.Application.LoadComponent(this, resourceLocator);
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			switch (connectionId)
			{
			case 1:
				brdRand = (Border)target;
				break;
			case 2:
				panelVideo = (System.Windows.Forms.Panel)target;
				break;
			default:
				_contentLoaded = true;
				break;
			}
		}
	}
}
