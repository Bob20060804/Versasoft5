using Ersa.Global.Controls.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Ersa.Global.Controls
{
	public class EDU_NavigationsListe : ItemsControl
	{
		public static readonly DependencyProperty PRO_i32AnzahlSichtbarerElementeProperty;

		public static readonly DependencyProperty PRO_i32AnzahlNichtSichtbarerElementeVornProperty;

		public static readonly DependencyProperty PRO_i32AnzahlNichtSichtbarerElementeHintenProperty;

		public static readonly DependencyProperty PRO_blnInhaltAktivertProperty;

		private static readonly DependencyPropertyKey PRO_i32AnzahlElementePropertyKey;

		public static readonly DependencyProperty PRO_i32AnzahlElementeProperty;

		private readonly Func<FrameworkElement, bool> m_delElementHatGroessePruefung = (FrameworkElement i_fdcElement) => i_fdcElement.ActualWidth > 0.0 == i_fdcElement.ActualHeight > 0.0;

		public int PRO_i32AnzahlSichtbarerElemente
		{
			get
			{
				return (int)GetValue(PRO_i32AnzahlSichtbarerElementeProperty);
			}
			set
			{
				SetValue(PRO_i32AnzahlSichtbarerElementeProperty, value);
			}
		}

		public int PRO_i32AnzahlNichtSichtbarerElementeVorn
		{
			get
			{
				return (int)GetValue(PRO_i32AnzahlNichtSichtbarerElementeVornProperty);
			}
			set
			{
				SetValue(PRO_i32AnzahlNichtSichtbarerElementeVornProperty, value);
			}
		}

		public int PRO_i32AnzahlNichtSichtbarerElementeHinten
		{
			get
			{
				return (int)GetValue(PRO_i32AnzahlNichtSichtbarerElementeHintenProperty);
			}
			set
			{
				SetValue(PRO_i32AnzahlNichtSichtbarerElementeHintenProperty, value);
			}
		}

		public bool PRO_blnInhaltAktivert
		{
			get
			{
				return (bool)GetValue(PRO_blnInhaltAktivertProperty);
			}
			set
			{
				SetValue(PRO_blnInhaltAktivertProperty, value);
			}
		}

		public int PRO_i32AnzahlElemente
		{
			get
			{
				return (int)GetValue(PRO_i32AnzahlElementeProperty);
			}
			private set
			{
				SetValue(PRO_i32AnzahlElementePropertyKey, value);
			}
		}

		static EDU_NavigationsListe()
		{
			PRO_i32AnzahlSichtbarerElementeProperty = DependencyProperty.Register("PRO_i32AnzahlSichtbarerElemente", typeof(int), typeof(EDU_NavigationsListe));
			PRO_i32AnzahlNichtSichtbarerElementeVornProperty = DependencyProperty.Register("PRO_i32AnzahlNichtSichtbarerElementeVorn", typeof(int), typeof(EDU_NavigationsListe));
			PRO_i32AnzahlNichtSichtbarerElementeHintenProperty = DependencyProperty.Register("PRO_i32AnzahlNichtSichtbarerElementeHinten", typeof(int), typeof(EDU_NavigationsListe));
			PRO_blnInhaltAktivertProperty = DependencyProperty.Register("PRO_blnInhaltAktivert", typeof(bool), typeof(EDU_NavigationsListe), new PropertyMetadata(true));
			PRO_i32AnzahlElementePropertyKey = DependencyProperty.RegisterReadOnly("PRO_i32AnzahlElemente", typeof(int), typeof(EDU_NavigationsListe), new PropertyMetadata(0));
			PRO_i32AnzahlElementeProperty = PRO_i32AnzahlElementePropertyKey.DependencyProperty;
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(EDU_NavigationsListe), new FrameworkPropertyMetadata(typeof(EDU_NavigationsListe)));
		}

		public EDU_NavigationsListe()
		{
			base.GotFocus += delegate
			{
				SUB_BindingsAktualisieren(PRO_i32AnzahlSichtbarerElementeProperty);
				SUB_BindingsAktualisieren(PRO_i32AnzahlNichtSichtbarerElementeVornProperty);
				SUB_BindingsAktualisieren(PRO_i32AnzahlNichtSichtbarerElementeHintenProperty);
			};
		}

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			ScrollViewer fdcScrollViewer = this.FUN_lstKindElementeSuchen<ScrollViewer>().FirstOrDefault();
			if (fdcScrollViewer != null)
			{
				fdcScrollViewer.ScrollChanged += SUB_ScrollBereichGeaendert;
			}
			RepeatButton repeatButton = this.FUN_objBenanntesKindElementSuchen<RepeatButton>("LinksButton").FirstOrDefault();
			if (repeatButton != null)
			{
				repeatButton.Click += delegate
				{
					((IEnumerable<FrameworkElement>)fdcScrollViewer.FUN_lstInhaltErmitteln()).Where((Func<FrameworkElement, bool>)fdcScrollViewer.FUN_blnIstElementInSichtbereich).FirstOrDefault()?.BringIntoView();
				};
			}
			RepeatButton repeatButton2 = this.FUN_objBenanntesKindElementSuchen<RepeatButton>("RechtsButton").FirstOrDefault();
			if (repeatButton2 != null)
			{
				repeatButton2.Click += delegate
				{
					((IEnumerable<FrameworkElement>)fdcScrollViewer.FUN_lstInhaltErmitteln()).Where((Func<FrameworkElement, bool>)fdcScrollViewer.FUN_blnIstElementInSichtbereich).LastOrDefault()?.BringIntoView();
				};
			}
		}

		protected override void OnItemsSourceChanged(IEnumerable i_lstAlterWert, IEnumerable i_lstNeuerWert)
		{
			base.OnItemsSourceChanged(i_lstAlterWert, i_lstNeuerWert);
			SUB_AnzahlElementeAktualisieren();
		}

		protected override void OnItemsChanged(NotifyCollectionChangedEventArgs i_fdcArgs)
		{
			base.OnItemsChanged(i_fdcArgs);
			SUB_AnzahlElementeAktualisieren();
		}

		private void SUB_AnzahlElementeAktualisieren()
		{
			ApplyTemplate();
			ScrollViewer scrollViewer = this.FUN_lstKindElementeSuchen<ScrollViewer>().FirstOrDefault();
			if (scrollViewer != null)
			{
				scrollViewer.ApplyTemplate();
				PRO_i32AnzahlElemente = scrollViewer.FUN_lstInhaltErmitteln(m_delElementHatGroessePruefung).Count;
			}
		}

		private void SUB_ScrollBereichGeaendert(object sender, ScrollChangedEventArgs e)
		{
			ScrollViewer scrollViewer = sender as ScrollViewer;
			if (scrollViewer != null && base.Items != null)
			{
				IList<FrameworkElement> list = scrollViewer.FUN_lstInhaltErmitteln(m_delElementHatGroessePruefung);
				IList<FrameworkElement> list2 = ((IEnumerable<FrameworkElement>)list).Where((Func<FrameworkElement, bool>)scrollViewer.FUN_blnIstElementInSichtbereich).ToList();
				int num = 0;
				int num2 = 0;
				int num3 = 0;
				foreach (FrameworkElement item in list)
				{
					if (list2.Contains(item))
					{
						num3 = 1;
					}
					else if (num3 == 1)
					{
						num3 = 2;
					}
					switch (num3)
					{
					case 0:
						num++;
						break;
					case 2:
						num2++;
						break;
					}
				}
				PRO_i32AnzahlSichtbarerElemente = list2.Count;
				PRO_i32AnzahlNichtSichtbarerElementeVorn = num;
				PRO_i32AnzahlNichtSichtbarerElementeHinten = num2;
			}
			else
			{
				PRO_i32AnzahlSichtbarerElemente = 0;
				PRO_i32AnzahlNichtSichtbarerElementeVorn = 0;
				PRO_i32AnzahlNichtSichtbarerElementeHinten = 0;
			}
		}

		private void SUB_BindingsAktualisieren(DependencyProperty i_fdcDependencyProperty)
		{
			GetBindingExpression(i_fdcDependencyProperty)?.UpdateSource();
		}
	}
}
