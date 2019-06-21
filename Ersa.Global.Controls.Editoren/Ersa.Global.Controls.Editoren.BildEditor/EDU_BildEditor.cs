using Ersa.Global.Common;
using Ersa.Global.Common.Data.Cad;
using Ersa.Global.Common.Helper;
using Ersa.Global.Controls.Editoren.EditorElemente;
using Ersa.Global.Controls.Editoren.Interfaces.Intern;
using Ersa.Global.Controls.Editoren.Werkzeuge;
using Ersa.Global.Controls.Extensions;
using Ersa.Global.Controls.Helpers;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;

namespace Ersa.Global.Controls.Editoren.BildEditor
{
	public class EDU_BildEditor : UserControl, INF_EditorKontext, INF_PunktVerschiebungsKontext, IComponentConnector, IStyleConnector
	{
		public static readonly DependencyProperty PRO_dblZoomSchrittweiteProperty = DependencyProperty.Register("PRO_dblZoomSchrittweite", typeof(double), typeof(EDU_BildEditor), new PropertyMetadata(0.02));

		public static readonly DependencyProperty PRO_blnBeschraenkeZoomRausProperty = DependencyProperty.Register("PRO_blnBeschraenkeZoomRaus", typeof(bool), typeof(EDU_BildEditor), new PropertyMetadata(false));

		public static readonly DependencyProperty PRO_dblMaximaleVergroesserungProperty = DependencyProperty.Register("PRO_dblMaximaleVergroesserung", typeof(double), typeof(EDU_BildEditor), new PropertyMetadata(0.0));

		public static readonly DependencyProperty PRO_dblCanvasTranslateXProperty = DependencyProperty.Register("PRO_dblCanvasTranslateX", typeof(double), typeof(EDU_BildEditor), new PropertyMetadata(0.0));

		public static readonly DependencyProperty PRO_dblCanvasTranslateYProperty = DependencyProperty.Register("PRO_dblCanvasTranslateY", typeof(double), typeof(EDU_BildEditor), new PropertyMetadata(0.0));

		public static readonly DependencyProperty PRO_fdcHintergrundProperty = DependencyProperty.Register("PRO_fdcHintergrund", typeof(FrameworkElement), typeof(EDU_BildEditor), new FrameworkPropertyMetadata(SUB_HintergrundGeaendert));

		public static readonly DependencyProperty PRO_enmBlickrichtungProperty = DependencyProperty.Register("PRO_enmBlickrichtung", typeof(ENUM_Blickrichtung), typeof(EDU_BildEditor), new PropertyMetadata(ENUM_Blickrichtung.BOTTOMVIEW));

		public static readonly DependencyProperty PRO_enuGrafikElementeProperty = DependencyProperty.Register("PRO_enuGrafikElemente", typeof(IEnumerable<EDC_EditorElement>), typeof(EDU_BildEditor), new PropertyMetadata(null, SUB_GrafikElementeGesetzt));

		private static DependencyPropertyKey PRO_enuTemporaereGrafikElementePropertyKey = DependencyProperty.RegisterReadOnly("PRO_enuTemporaereGrafikElemente", typeof(IEnumerable<EDC_EditorElement>), typeof(EDU_BildEditor), new PropertyMetadata(null, SUB_TemporaereGrafikElementeGesetzt));

		public static readonly DependencyProperty PRO_enuTemporaereGrafikElementeProperty = PRO_enuTemporaereGrafikElementePropertyKey.DependencyProperty;

		public static readonly DependencyProperty PRO_objTooltipProperty = DependencyProperty.Register("PRO_objTooltip", typeof(object), typeof(EDU_BildEditor), new PropertyMetadata((object)null));

		public static readonly DependencyProperty PRO_edcWerkzeugProperty = DependencyProperty.Register("PRO_edcWerkzeug", typeof(EDC_Tool), typeof(EDU_BildEditor), new PropertyMetadata(null, SUB_WerkzeugGeaendertAsync));

		public static readonly DependencyProperty PRO_dblNullpunktXProperty = DependencyProperty.Register("PRO_dblNullpunktX", typeof(double), typeof(EDU_BildEditor), new PropertyMetadata(0.0));

		public static readonly DependencyProperty PRO_dblNullpunktYProperty = DependencyProperty.Register("PRO_dblNullpunktY", typeof(double), typeof(EDU_BildEditor), new PropertyMetadata(0.0));

		private static DependencyPropertyKey PRO_dblSkalierungPropertyKey = DependencyProperty.RegisterReadOnly("PRO_dblSkalierung", typeof(double), typeof(EDU_BildEditor), new PropertyMetadata(0.0, SUB_SkalierungGeaendert));

		public static readonly DependencyProperty PRO_dblSkalierungProperty = PRO_dblSkalierungPropertyKey.DependencyProperty;

		public static readonly DependencyProperty PRO_blnAchseXGespiegeltProperty = DependencyProperty.Register("PRO_blnAchseXGespiegelt", typeof(bool), typeof(EDU_BildEditor), new PropertyMetadata(false, SUB_AchseXGespiegeltGeaendert));

		public static readonly DependencyProperty PRO_blnAchseYGespiegeltProperty = DependencyProperty.Register("PRO_blnAchseYGespiegelt", typeof(bool), typeof(EDU_BildEditor), new PropertyMetadata(false, SUB_AchseYGespiegeltGeaendert));

		private static DependencyPropertyKey PRO_dblSpiegelungsFaktorXPropertyKey = DependencyProperty.RegisterReadOnly("PRO_dblSpiegelungsFaktorX", typeof(double), typeof(EDU_BildEditor), new PropertyMetadata(1.0));

		public static readonly DependencyProperty PRO_dblSpiegelungsFaktorXProperty = PRO_dblSpiegelungsFaktorXPropertyKey.DependencyProperty;

		private static DependencyPropertyKey PRO_dblSpiegelungsFaktorYPropertyKey = DependencyProperty.RegisterReadOnly("PRO_dblSpiegelungsFaktorY", typeof(double), typeof(EDU_BildEditor), new PropertyMetadata(-1.0));

		public static readonly DependencyProperty PRO_dblSpiegelungsFaktorYProperty = PRO_dblSpiegelungsFaktorYPropertyKey.DependencyProperty;

		private static DependencyPropertyKey PRO_dblSpiegelungsFaktorYFuerElementPropertyKey = DependencyProperty.RegisterReadOnly("PRO_dblSpiegelungsFaktorYFuerElement", typeof(double), typeof(EDU_BildEditor), new PropertyMetadata(1.0));

		public static readonly DependencyProperty PRO_dblSpiegelungsFaktorYFuerElementProperty = PRO_dblSpiegelungsFaktorYFuerElementPropertyKey.DependencyProperty;

		private const double mC_dblZoomSchrittweite = 0.02;

		private const double mC_intAdaptivesScrollenZeitraum = 100.0;

		private UIElement m_fdcReferenzControl;

		private Point m_sttLetzteMovePosition;

		private bool m_blnMausInBildEditor;

		private DateTime m_fdcLetztesScrollen;

		private FrameworkElement m_fdcAktuellerHintergrund;

		private Point m_fdcScrollStartPoint;

		private Point m_fdcScrollStartOffset;

		private bool m_blnGroesseInitialisiert;

		internal ScrollViewer scrollviewer;

		internal Grid grid;

		internal Canvas canvas;

		internal Popup popup;

		private bool _contentLoaded;

		public double PRO_dblZoomSchrittweite
		{
			get
			{
				return (double)GetValue(PRO_dblZoomSchrittweiteProperty);
			}
			set
			{
				SetValue(PRO_dblZoomSchrittweiteProperty, value);
			}
		}

		public bool PRO_blnBeschraenkeZoomRaus
		{
			get
			{
				return (bool)GetValue(PRO_blnBeschraenkeZoomRausProperty);
			}
			set
			{
				SetValue(PRO_blnBeschraenkeZoomRausProperty, value);
			}
		}

		public double PRO_dblMaximaleVergroesserung
		{
			get
			{
				return (double)GetValue(PRO_dblMaximaleVergroesserungProperty);
			}
			set
			{
				SetValue(PRO_dblMaximaleVergroesserungProperty, value);
			}
		}

		public double PRO_dblCanvasTranslateX
		{
			get
			{
				return (double)GetValue(PRO_dblCanvasTranslateXProperty);
			}
			set
			{
				SetValue(PRO_dblCanvasTranslateXProperty, value);
			}
		}

		public double PRO_dblCanvasTranslateY
		{
			get
			{
				return (double)GetValue(PRO_dblCanvasTranslateYProperty);
			}
			set
			{
				SetValue(PRO_dblCanvasTranslateYProperty, value);
			}
		}

		public FrameworkElement PRO_fdcHintergrund
		{
			get
			{
				return (FrameworkElement)GetValue(PRO_fdcHintergrundProperty);
			}
			set
			{
				SetValue(PRO_fdcHintergrundProperty, value);
			}
		}

		public ENUM_Blickrichtung PRO_enmBlickrichtung
		{
			get
			{
				return (ENUM_Blickrichtung)GetValue(PRO_enmBlickrichtungProperty);
			}
			set
			{
				SetValue(PRO_enmBlickrichtungProperty, value);
			}
		}

		public IEnumerable<EDC_EditorElement> PRO_enuGrafikElemente
		{
			get
			{
				return (IEnumerable<EDC_EditorElement>)GetValue(PRO_enuGrafikElementeProperty);
			}
			set
			{
				SetValue(PRO_enuGrafikElementeProperty, value);
			}
		}

		public IEnumerable<EDC_EditorElement> PRO_enuTemporaereGrafikElemente
		{
			get
			{
				return (IEnumerable<EDC_EditorElement>)GetValue(PRO_enuTemporaereGrafikElementeProperty);
			}
			private set
			{
				SetValue(PRO_enuTemporaereGrafikElementePropertyKey, value);
			}
		}

		public object PRO_objTooltip
		{
			get
			{
				return GetValue(PRO_objTooltipProperty);
			}
			set
			{
				SetValue(PRO_objTooltipProperty, value);
			}
		}

		public EDC_Tool PRO_edcWerkzeug
		{
			get
			{
				return (EDC_Tool)GetValue(PRO_edcWerkzeugProperty);
			}
			set
			{
				SetValue(PRO_edcWerkzeugProperty, value);
			}
		}

		public double PRO_dblNullpunktX
		{
			get
			{
				return (double)GetValue(PRO_dblNullpunktXProperty);
			}
			set
			{
				SetValue(PRO_dblNullpunktXProperty, value);
			}
		}

		public double PRO_dblNullpunktY
		{
			get
			{
				return (double)GetValue(PRO_dblNullpunktYProperty);
			}
			set
			{
				SetValue(PRO_dblNullpunktYProperty, value);
			}
		}

		public double PRO_dblSkalierung
		{
			get
			{
				return (double)GetValue(PRO_dblSkalierungProperty);
			}
			private set
			{
				SetValue(PRO_dblSkalierungPropertyKey, value);
			}
		}

		public bool PRO_blnAchseXGespiegelt
		{
			get
			{
				return (bool)GetValue(PRO_blnAchseXGespiegeltProperty);
			}
			set
			{
				SetValue(PRO_blnAchseXGespiegeltProperty, value);
			}
		}

		public bool PRO_blnAchseYGespiegelt
		{
			get
			{
				return (bool)GetValue(PRO_blnAchseYGespiegeltProperty);
			}
			set
			{
				SetValue(PRO_blnAchseYGespiegeltProperty, value);
			}
		}

		public double PRO_dblSpiegelungsFaktorX
		{
			get
			{
				return (double)GetValue(PRO_dblSpiegelungsFaktorXProperty);
			}
			private set
			{
				SetValue(PRO_dblSpiegelungsFaktorXPropertyKey, value);
			}
		}

		public double PRO_dblSpiegelungsFaktorY
		{
			get
			{
				return (double)GetValue(PRO_dblSpiegelungsFaktorYProperty);
			}
			private set
			{
				SetValue(PRO_dblSpiegelungsFaktorYPropertyKey, value);
			}
		}

		public double PRO_dblSpiegelungsFaktorYFuerElement
		{
			get
			{
				return (double)GetValue(PRO_dblSpiegelungsFaktorYFuerElementProperty);
			}
			private set
			{
				SetValue(PRO_dblSpiegelungsFaktorYFuerElementPropertyKey, value);
			}
		}

		public double PRO_dblMinimaleSkalierung
		{
			get
			{
				if (PRO_fdcHintergrund == null)
				{
					return 1.0;
				}
				Size size = new Size(scrollviewer.ActualWidth, scrollviewer.ActualHeight);
				if (size == Size.Empty)
				{
					return 1.0;
				}
				double num = size.Width / PRO_fdcHintergrund.Width;
				double num2 = size.Height / PRO_fdcHintergrund.Height;
				if (!(num > num2))
				{
					return num;
				}
				return num2;
			}
		}

		public EDU_BildEditor()
		{
			InitializeComponent();
			PRO_enuTemporaereGrafikElemente = new ObservableCollection<EDC_EditorElement>();
		}

		public IEnumerable<EDC_EditorElement> FUN_enuHoleAlleElemente()
		{
			return PRO_enuGrafikElemente;
		}

		public EDC_EditorElement FUN_edcHoleElementAnPosition(Point i_sttPosition)
		{
			if (m_fdcReferenzControl == null)
			{
				return null;
			}
			return (EDC_HitTestHelfer.FUN_fdcHitTestNurSichtbare(m_fdcReferenzControl, i_sttPosition) as DependencyObject)?.FUN_objBenanntesElternElementErmitteln("editorElement")?.Tag as EDC_EditorElement;
		}

		public IDisposable FUN_fdcFuegeElementTemporaerHinzu(EDC_EditorElement i_edcElement)
		{
			IList<EDC_EditorElement> lstTempElemente = PRO_enuTemporaereGrafikElemente as IList<EDC_EditorElement>;
			if (lstTempElemente == null)
			{
				return EDC_Disposable.FUN_fdcEmpty();
			}
			lstTempElemente.Add(i_edcElement);
			return EDC_Disposable.FUN_fdcCreate(delegate
			{
				lstTempElemente.Remove(i_edcElement);
			});
		}

		public void SUB_AendereBildausschnitt(Rect i_sttBereich)
		{
			Rect rect = EDC_GeometrieHelfer.FUN_sttPasseRechteckKoordinatenAn(i_sttBereich, new Vector(PRO_dblNullpunktX, PRO_dblNullpunktY));
			double actualWidth = base.ActualWidth;
			double actualHeight = base.ActualHeight;
			if (rect.Width > 0.0 && rect.Height > 0.0)
			{
				double val = actualWidth / rect.Width;
				double val2 = actualHeight / rect.Height;
				double num = Math.Min(val, val2);
				if (PRO_blnBeschraenkeZoomRaus)
				{
					PRO_dblSkalierung = ((num > PRO_dblMinimaleSkalierung) ? num : PRO_dblMinimaleSkalierung);
				}
				else
				{
					PRO_dblSkalierung = num;
				}
				SUB_SetzeViewport(rect.Location);
			}
		}

		public EDC_PunktVerschiebungsDaten FUN_edcHolePunktVerschiebungsDaten(Point i_sttPosition)
		{
			if (m_fdcReferenzControl == null)
			{
				return null;
			}
			DependencyObject dependencyObject = EDC_HitTestHelfer.FUN_fdcHitTestNurSichtbare(m_fdcReferenzControl, i_sttPosition) as DependencyObject;
			if (dependencyObject == null)
			{
				return null;
			}
			FrameworkElement frameworkElement = FUN_fdcHoleUiElementMitNamen(dependencyObject, "punktAnfasserElement");
			if (frameworkElement == null)
			{
				return null;
			}
			EDC_EditorElementMitPunkten eDC_EditorElementMitPunkten = frameworkElement.FUN_objBenanntesElternElementErmitteln("editorElement")?.Tag as EDC_EditorElementMitPunkten;
			if (eDC_EditorElementMitPunkten == null)
			{
				return null;
			}
			object i_objPunktReferenz = eDC_EditorElementMitPunkten.FUN_objErmittlePunktReferenzAnPosition(i_sttPosition);
			return new EDC_PunktVerschiebungsDaten(eDC_EditorElementMitPunkten, i_objPunktReferenz, eDC_EditorElementMitPunkten.FUN_enuHolePunkte().ToList());
		}

		protected override void OnPreviewMouseDown(MouseButtonEventArgs i_fdcMouseEventArgs)
		{
			if (scrollviewer.IsMouseOver && i_fdcMouseEventArgs.MiddleButton == MouseButtonState.Pressed)
			{
				m_fdcScrollStartPoint = i_fdcMouseEventArgs.GetPosition(this);
				m_fdcScrollStartOffset.X = scrollviewer.HorizontalOffset;
				m_fdcScrollStartOffset.Y = scrollviewer.VerticalOffset;
				base.Cursor = ((scrollviewer.ExtentWidth > scrollviewer.ViewportWidth || scrollviewer.ExtentHeight > scrollviewer.ViewportHeight) ? Cursors.ScrollAll : Cursors.Arrow);
				CaptureMouse();
			}
			base.OnPreviewMouseDown(i_fdcMouseEventArgs);
		}

		protected override void OnPreviewMouseMove(MouseEventArgs i_fdcMouseEventArgs)
		{
			if (base.IsMouseCaptured)
			{
				Point position = i_fdcMouseEventArgs.GetPosition(this);
				Vector vector = m_fdcScrollStartPoint - position;
				m_fdcScrollStartPoint = position;
				vector /= PRO_dblSkalierung;
				SUB_BewegeViewport(vector);
			}
			base.OnPreviewMouseMove(i_fdcMouseEventArgs);
		}

		protected override void OnPreviewMouseUp(MouseButtonEventArgs i_fdcMouseEventArgs)
		{
			if (base.IsMouseCaptured)
			{
				base.Cursor = Cursors.Arrow;
				ReleaseMouseCapture();
			}
			base.OnPreviewMouseUp(i_fdcMouseEventArgs);
		}

		private static void SUB_GrafikElementeGesetzt(DependencyObject i_fdcDependencyObject, DependencyPropertyChangedEventArgs i_edcE)
		{
			EDU_BildEditor eDU_BildEditor = i_fdcDependencyObject as EDU_BildEditor;
			if (eDU_BildEditor != null)
			{
				ObservableCollection<EDC_EditorElement> observableCollection = i_edcE.OldValue as ObservableCollection<EDC_EditorElement>;
				if (observableCollection != null)
				{
					observableCollection.CollectionChanged -= eDU_BildEditor.SUB_GrafikElementeGeaendert;
				}
				ObservableCollection<EDC_EditorElement> observableCollection2 = i_edcE.NewValue as ObservableCollection<EDC_EditorElement>;
				if (observableCollection2 != null)
				{
					observableCollection2.CollectionChanged += eDU_BildEditor.SUB_GrafikElementeGeaendert;
					foreach (EDC_EditorElement item in observableCollection2)
					{
						item.SUB_SetzeSkalierung(eDU_BildEditor.PRO_dblSkalierung);
					}
				}
			}
		}

		private static void SUB_TemporaereGrafikElementeGesetzt(DependencyObject i_fdcDependencyObject, DependencyPropertyChangedEventArgs i_edcE)
		{
			EDU_BildEditor eDU_BildEditor = i_fdcDependencyObject as EDU_BildEditor;
			if (eDU_BildEditor != null)
			{
				ObservableCollection<EDC_EditorElement> observableCollection = i_edcE.OldValue as ObservableCollection<EDC_EditorElement>;
				if (observableCollection != null)
				{
					observableCollection.CollectionChanged -= eDU_BildEditor.SUB_TemporaereGrafikElementeGeaendert;
				}
				ObservableCollection<EDC_EditorElement> observableCollection2 = i_edcE.NewValue as ObservableCollection<EDC_EditorElement>;
				if (observableCollection2 != null)
				{
					observableCollection2.CollectionChanged += eDU_BildEditor.SUB_TemporaereGrafikElementeGeaendert;
					foreach (EDC_EditorElement item in observableCollection2)
					{
						item.SUB_SetzeSkalierung(eDU_BildEditor.PRO_dblSkalierung);
					}
				}
			}
		}

		private static void SUB_SkalierungGeaendert(DependencyObject i_fdcDependencyObject, DependencyPropertyChangedEventArgs i_edcE)
		{
			EDU_BildEditor eDU_BildEditor = i_fdcDependencyObject as EDU_BildEditor;
			if (eDU_BildEditor != null)
			{
				foreach (EDC_EditorElement item in eDU_BildEditor.PRO_enuGrafikElemente ?? Enumerable.Empty<EDC_EditorElement>())
				{
					item.SUB_SetzeSkalierung(eDU_BildEditor.PRO_dblSkalierung);
				}
				foreach (EDC_EditorElement item2 in eDU_BildEditor.PRO_enuTemporaereGrafikElemente ?? Enumerable.Empty<EDC_EditorElement>())
				{
					item2.SUB_SetzeSkalierung(eDU_BildEditor.PRO_dblSkalierung);
				}
			}
		}

		private static void SUB_AchseXGespiegeltGeaendert(DependencyObject i_fdcDependencyObject, DependencyPropertyChangedEventArgs i_edcE)
		{
			EDU_BildEditor eDU_BildEditor = i_fdcDependencyObject as EDU_BildEditor;
			if (eDU_BildEditor != null)
			{
				eDU_BildEditor.PRO_dblSpiegelungsFaktorX = ((!eDU_BildEditor.PRO_blnAchseXGespiegelt) ? 1 : (-1));
			}
		}

		private static void SUB_AchseYGespiegeltGeaendert(DependencyObject i_fdcDependencyObject, DependencyPropertyChangedEventArgs i_edcE)
		{
			EDU_BildEditor eDU_BildEditor = i_fdcDependencyObject as EDU_BildEditor;
			if (eDU_BildEditor != null)
			{
				eDU_BildEditor.PRO_dblSpiegelungsFaktorY = (eDU_BildEditor.PRO_blnAchseYGespiegelt ? 1 : (-1));
				eDU_BildEditor.PRO_dblSpiegelungsFaktorYFuerElement = ((!eDU_BildEditor.PRO_blnAchseYGespiegelt) ? 1 : (-1));
			}
		}

		private static void SUB_WerkzeugGeaendertAsync(DependencyObject i_edcDependencyObject, DependencyPropertyChangedEventArgs i_edcDependencyPropertyChangedEventArgs)
		{
			(i_edcDependencyObject as EDU_BildEditor)?.SUB_WerkzeugInitialisieren(i_edcDependencyPropertyChangedEventArgs.OldValue as EDC_Tool);
		}

		private static void SUB_HintergrundGeaendert(DependencyObject i_edcD, DependencyPropertyChangedEventArgs i_edcE)
		{
			(i_edcD as EDU_BildEditor)?.SUB_SetzeHintergrund(i_edcE.NewValue as FrameworkElement);
		}

		private static FrameworkElement FUN_fdcHoleUiElementMitNamen(DependencyObject i_fdcUiObjekt, string i_strName)
		{
			FrameworkElement frameworkElement = i_fdcUiObjekt as FrameworkElement;
			if (frameworkElement == null || !frameworkElement.Name.Equals(i_strName))
			{
				frameworkElement = i_fdcUiObjekt.FUN_objBenanntesElternElementErmitteln(i_strName);
			}
			return frameworkElement;
		}

		private void SUB_GrafikElementeGeaendert(object i_objSender, NotifyCollectionChangedEventArgs i_fdcArgs)
		{
			switch (i_fdcArgs.Action)
			{
			case NotifyCollectionChangedAction.Remove:
			case NotifyCollectionChangedAction.Move:
				break;
			case NotifyCollectionChangedAction.Reset:
				foreach (EDC_EditorElement item in PRO_enuGrafikElemente)
				{
					item.SUB_SetzeSkalierung(PRO_dblSkalierung);
				}
				break;
			case NotifyCollectionChangedAction.Add:
			case NotifyCollectionChangedAction.Replace:
				foreach (object newItem in i_fdcArgs.NewItems)
				{
					(newItem as EDC_EditorElement)?.SUB_SetzeSkalierung(PRO_dblSkalierung);
				}
				break;
			}
		}

		private void SUB_TemporaereGrafikElementeGeaendert(object i_objSender, NotifyCollectionChangedEventArgs i_fdcArgs)
		{
			switch (i_fdcArgs.Action)
			{
			case NotifyCollectionChangedAction.Remove:
			case NotifyCollectionChangedAction.Move:
				break;
			case NotifyCollectionChangedAction.Reset:
				foreach (EDC_EditorElement item in PRO_enuTemporaereGrafikElemente)
				{
					item.SUB_SetzeSkalierung(PRO_dblSkalierung);
				}
				break;
			case NotifyCollectionChangedAction.Add:
			case NotifyCollectionChangedAction.Replace:
				foreach (object newItem in i_fdcArgs.NewItems)
				{
					(newItem as EDC_EditorElement)?.SUB_SetzeSkalierung(PRO_dblSkalierung);
				}
				break;
			}
		}

		private void SUB_WerkzeugInitialisieren(EDC_Tool i_edcAltesWerkzeug)
		{
			i_edcAltesWerkzeug?.SUB_WerkzeugDeaktiviert();
			PRO_edcWerkzeug?.SUB_EditorKontextInitialisieren(this);
			(PRO_edcWerkzeug as EDC_AuswahlTool)?.SUB_PunktVerschiebungsKontextInitialisieren(this);
			base.Cursor = (PRO_edcWerkzeug?.FUN_fdcHoleWerkzeugCursor() ?? Cursors.Arrow);
			PRO_edcWerkzeug?.SUB_SetzeInitialeMausposition(m_sttLetzteMovePosition, m_blnMausInBildEditor);
		}

		private void SUB_SetzeHintergrund(FrameworkElement i_edcFrameworkElement)
		{
			if (m_fdcAktuellerHintergrund != null)
			{
				canvas.Children.Remove(m_fdcAktuellerHintergrund);
			}
			m_fdcAktuellerHintergrund = i_edcFrameworkElement;
			if (m_fdcAktuellerHintergrund != null)
			{
				Panel.SetZIndex(m_fdcAktuellerHintergrund, -50);
				if (i_edcFrameworkElement.Parent != null)
				{
					(i_edcFrameworkElement.Parent as Canvas)?.Children.Remove(i_edcFrameworkElement);
				}
				if (m_fdcAktuellerHintergrund != null)
				{
					canvas.Children.Add(m_fdcAktuellerHintergrund);
				}
				SUB_ZoomeZuVollbild();
			}
		}

		private double FUN_intBerechneAdaptiveScrollweite(TimeSpan i_fdcScrollAbstand)
		{
			if (i_fdcScrollAbstand.TotalMilliseconds > 100.0 || i_fdcScrollAbstand.TotalMilliseconds < 0.0)
			{
				return 1.0;
			}
			return Math.Pow(100.0 - i_fdcScrollAbstand.TotalMilliseconds, 0.33333333333333331);
		}

		private void SUB_MouseWheel(object i_objSender, MouseWheelEventArgs i_fdcArgs)
		{
			if (PRO_dblSkalierung == 0.0)
			{
				SUB_SkaliereVollbild();
			}
			i_fdcArgs.Handled = true;
			TimeSpan i_fdcScrollAbstand = DateTime.Now - m_fdcLetztesScrollen;
			m_fdcLetztesScrollen = DateTime.Now;
			double num = FUN_intBerechneAdaptiveScrollweite(i_fdcScrollAbstand);
			int num2 = 1;
			if (i_fdcArgs.Delta < 0)
			{
				num2 = -1;
			}
			double num3 = 1.0 + PRO_dblZoomSchrittweite * (double)num2 * num;
			double num4 = PRO_dblSkalierung * num3;
			if (num3 < 1.0)
			{
				if (PRO_blnBeschraenkeZoomRaus && num4 < PRO_dblMinimaleSkalierung)
				{
					PRO_dblSkalierung = PRO_dblMinimaleSkalierung;
					return;
				}
				if (num4 < 0.02)
				{
					PRO_dblSkalierung = 0.02;
					return;
				}
			}
			int num5 = 1;
			if (!(num3 > 0.0) || !(PRO_dblMaximaleVergroesserung > 0.0) || !(num4 > PRO_dblMaximaleVergroesserung * (double)num5))
			{
				Point position = i_fdcArgs.MouseDevice.GetPosition(this);
				Point point = new Point(position.X / PRO_dblSkalierung, position.Y / PRO_dblSkalierung);
				Point point2 = new Point(position.X / num4, position.Y / num4);
				Vector i_fdcDelta = point - point2;
				PRO_dblSkalierung = num4;
				SUB_BewegeViewport(i_fdcDelta);
			}
		}

		private void SUB_MouseDown(object i_objSender, MouseButtonEventArgs i_fdcArgs)
		{
			Point position = i_fdcArgs.GetPosition(m_fdcReferenzControl);
			i_fdcArgs.Handled = (PRO_edcWerkzeug?.FUN_blnMouseDown(position, i_fdcArgs.LeftButton, i_fdcArgs.RightButton) ?? false);
		}

		private void SUB_MouseMove(object i_objSender, MouseEventArgs i_fdcArgs)
		{
			Point position = i_fdcArgs.GetPosition(m_fdcReferenzControl);
			if (position == m_sttLetzteMovePosition)
			{
				return;
			}
			m_sttLetzteMovePosition = position;
			EDC_RoutedCommands.ms_cmdBildEditorMausPositionGeaendert.SUB_Execute(position, this);
			i_fdcArgs.Handled = (PRO_edcWerkzeug?.FUN_blnMouseMove(position, i_fdcArgs.LeftButton) ?? false);
			Point position2 = i_fdcArgs.GetPosition(grid);
			if (PRO_objTooltip != null)
			{
				if (!popup.IsOpen)
				{
					popup.IsOpen = true;
				}
				popup.HorizontalOffset = position2.X + 16.0;
				popup.VerticalOffset = position2.Y + 35.0;
			}
			else if (popup.IsOpen)
			{
				popup.IsOpen = false;
			}
		}

		private void SUB_PreviewKeyDown(object i_objSender, KeyEventArgs i_fdcArgs)
		{
			if (i_fdcArgs.IsDown)
			{
				Key i_enmKey = (i_fdcArgs.Key == Key.System) ? i_fdcArgs.SystemKey : i_fdcArgs.Key;
				PRO_edcWerkzeug?.SUB_PreviewKeyDown(i_enmKey);
			}
		}

		private void SUB_MouseUp(object i_objSender, MouseButtonEventArgs i_fdcArgs)
		{
			Point position = i_fdcArgs.GetPosition(m_fdcReferenzControl);
			i_fdcArgs.Handled = (PRO_edcWerkzeug?.FUN_blnMouseUp(position, i_fdcArgs.LeftButton) ?? false);
		}

		private void SUB_MouseEnter(object i_objSender, MouseEventArgs i_fdcArgs)
		{
			PRO_edcWerkzeug?.SUB_MouseEnter();
			m_blnMausInBildEditor = true;
		}

		private void SUB_MouseLeave(object i_objSender, MouseEventArgs i_fdcArgs)
		{
			PRO_edcWerkzeug?.SUB_MouseLeave();
			popup.IsOpen = false;
			m_blnMausInBildEditor = false;
		}

		private void SUB_GroesseGeaendert(object i_edcSender, SizeChangedEventArgs i_edcE)
		{
			if (!m_blnGroesseInitialisiert)
			{
				m_blnGroesseInitialisiert = true;
				SUB_ZoomeZuVollbild();
			}
		}

		private void SUB_ZoomCanExecute(object i_edcSender, CanExecuteRoutedEventArgs i_edcE)
		{
			i_edcE.CanExecute = true;
		}

		private void SUB_ZoomExecuted(object i_edcSender, ExecutedRoutedEventArgs i_edcE)
		{
			SUB_ZoomeZuVollbild();
		}

		private void SUB_ZoomeZuVollbild()
		{
			SUB_SkaliereVollbild();
		}

		private void SUB_SkaliereVollbild()
		{
			PRO_dblSkalierung = PRO_dblMinimaleSkalierung;
			double? num = PRO_fdcHintergrund?.RenderTransform.Value.M11;
			double? num2 = PRO_fdcHintergrund?.RenderTransform.Value.M22;
			if (num2.HasValue && Math.Abs(num2.Value - -1.0) < 0.001)
			{
				PRO_dblCanvasTranslateY = PRO_fdcHintergrund.Height;
			}
			else
			{
				PRO_dblCanvasTranslateY = 0.0;
			}
			if (num.HasValue && Math.Abs(num.Value - -1.0) < 0.001)
			{
				PRO_dblCanvasTranslateX = PRO_fdcHintergrund.Width;
			}
			else
			{
				PRO_dblCanvasTranslateX = 0.0;
			}
		}

		private void SUB_BewegeViewport(Vector i_fdcDelta)
		{
			PRO_dblCanvasTranslateX -= i_fdcDelta.X;
			PRO_dblCanvasTranslateY -= i_fdcDelta.Y;
		}

		private void SUB_SetzeViewport(Point i_fdcViewportPunkt)
		{
			PRO_dblCanvasTranslateX = 0.0 - i_fdcViewportPunkt.X;
			PRO_dblCanvasTranslateY = 0.0 - i_fdcViewportPunkt.Y;
		}

		private void SUB_ReferenzControlOnLoaded(object i_objSender, RoutedEventArgs i_fdcArgs)
		{
			m_fdcReferenzControl = (i_objSender as UIElement);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			if (!_contentLoaded)
			{
				_contentLoaded = true;
				Uri resourceLocator = new Uri("/Ersa.Global.Controls.Editoren;component/bildeditor/edu_bildeditor.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
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
				((EDU_BildEditor)target).SizeChanged += SUB_GroesseGeaendert;
				break;
			case 2:
				((CommandBinding)target).CanExecute += SUB_ZoomCanExecute;
				((CommandBinding)target).Executed += SUB_ZoomExecuted;
				break;
			case 3:
				scrollviewer = (ScrollViewer)target;
				scrollviewer.PreviewKeyDown += SUB_PreviewKeyDown;
				break;
			case 4:
				grid = (Grid)target;
				break;
			case 5:
				((Grid)target).MouseWheel += SUB_MouseWheel;
				((Grid)target).MouseDown += SUB_MouseDown;
				((Grid)target).MouseMove += SUB_MouseMove;
				((Grid)target).MouseUp += SUB_MouseUp;
				((Grid)target).MouseEnter += SUB_MouseEnter;
				((Grid)target).MouseLeave += SUB_MouseLeave;
				break;
			case 6:
				canvas = (Canvas)target;
				break;
			case 8:
				popup = (Popup)target;
				break;
			default:
				_contentLoaded = true;
				break;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IStyleConnector.Connect(int connectionId, object target)
		{
			if (connectionId == 7)
			{
				((Canvas)target).Loaded += SUB_ReferenzControlOnLoaded;
			}
		}
	}
}
