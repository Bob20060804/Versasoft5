using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Ersa.Global.Controls.Extensions
{
	public static class EDC_DependencyObjectExtensions
	{
		public static IEnumerable<T> FUN_lstKindElementeSuchen<T>(this DependencyObject i_objStammElement) where T : DependencyObject
		{
			if (i_objStammElement == null)
			{
				yield break;
			}
			ContentPresenter contentPresenter = i_objStammElement as ContentPresenter;
			if (contentPresenter != null)
			{
				DependencyObject objContent2 = contentPresenter.Content as DependencyObject;
				if (objContent2 is T)
				{
					yield return (T)objContent2;
				}
				foreach (T item in objContent2.FUN_lstKindElementeSuchen<T>())
				{
					yield return item;
				}
				yield break;
			}
			int i32AnzahlChildren = VisualTreeHelper.GetChildrenCount(i_objStammElement);
			for (int i32Index = 0; i32Index < i32AnzahlChildren; i32Index++)
			{
				DependencyObject objContent2 = VisualTreeHelper.GetChild(i_objStammElement, i32Index);
				if (objContent2 is T)
				{
					yield return (T)objContent2;
				}
				foreach (T item2 in objContent2.FUN_lstKindElementeSuchen<T>())
				{
					yield return item2;
				}
			}
		}

		public static IEnumerable<T> FUN_objBenanntesKindElementSuchen<T>(this DependencyObject i_objStammElement, string i_strName) where T : FrameworkElement
		{
			if (i_objStammElement == null)
			{
				yield break;
			}
			for (int i32Index = 0; i32Index < VisualTreeHelper.GetChildrenCount(i_objStammElement); i32Index++)
			{
				FrameworkElement objKind = VisualTreeHelper.GetChild(i_objStammElement, i32Index) as FrameworkElement;
				if (objKind is T && objKind.Name.Equals(i_strName))
				{
					yield return (T)objKind;
				}
				foreach (T item in objKind.FUN_objBenanntesKindElementSuchen<T>(i_strName))
				{
					yield return item;
				}
			}
		}

		public static IEnumerable<FrameworkElement> FUN_objBenanntesKindElementSuchen(this DependencyObject i_objStammElement, string i_strName)
		{
			return i_objStammElement.FUN_objBenanntesKindElementSuchen<FrameworkElement>(i_strName);
		}

		public static DependencyObject FUN_objStammElementErmitteln(this DependencyObject i_objDependencyObject)
		{
			while (true)
			{
				DependencyObject parent = VisualTreeHelper.GetParent(i_objDependencyObject);
				if (parent == null)
				{
					break;
				}
				i_objDependencyObject = parent;
			}
			return i_objDependencyObject;
		}

		public static IEnumerable<DependencyObject> FUN_lstKindElementeErmitteln(this DependencyObject i_objStammElement)
		{
			int i32AnzahlKinder = VisualTreeHelper.GetChildrenCount(i_objStammElement);
			for (int i = 0; i < i32AnzahlKinder; i++)
			{
				yield return VisualTreeHelper.GetChild(i_objStammElement, i);
			}
		}

		public static T FUN_objElternElementErmitteln<T>(this DependencyObject i_objAusgangsObjekt) where T : FrameworkElement
		{
			T val = null;
			while (val == null && i_objAusgangsObjekt != null && (i_objAusgangsObjekt.GetType().FUN_blnIstTypAbgeleitet(typeof(Visual)) || i_objAusgangsObjekt.GetType().FUN_blnIstTypAbgeleitet(typeof(Visual3D))))
			{
				i_objAusgangsObjekt = VisualTreeHelper.GetParent(i_objAusgangsObjekt);
				if (i_objAusgangsObjekt is T)
				{
					val = (T)i_objAusgangsObjekt;
				}
			}
			return val;
		}

		public static T FUN_objFindeBestimmtenParent<T>(DependencyObject i_objChild) where T : DependencyObject
		{
			DependencyObject parent = VisualTreeHelper.GetParent(i_objChild);
			if (parent == null)
			{
				return null;
			}
			T val = parent as T;
			if (val != null)
			{
				return val;
			}
			return FUN_objFindeBestimmtenParent<T>(parent);
		}

		public static T FUN_objBenanntesElternElementErmitteln<T>(this DependencyObject i_objAusgangsObjekt, string i_strName) where T : FrameworkElement
		{
			T val = null;
			while (val == null && i_objAusgangsObjekt != null && (i_objAusgangsObjekt.GetType().FUN_blnIstTypAbgeleitet(typeof(Visual)) || i_objAusgangsObjekt.GetType().FUN_blnIstTypAbgeleitet(typeof(Visual3D))))
			{
				FrameworkElement frameworkElement = VisualTreeHelper.GetParent(i_objAusgangsObjekt) as FrameworkElement;
				i_objAusgangsObjekt = frameworkElement;
				if (frameworkElement is T && frameworkElement.Name.Equals(i_strName))
				{
					val = (T)frameworkElement;
				}
			}
			return val;
		}

		public static FrameworkElement FUN_objBenanntesElternElementErmitteln(this DependencyObject i_objAusgangsObjekt, string i_strName)
		{
			FrameworkElement frameworkElement = null;
			while (frameworkElement == null && i_objAusgangsObjekt != null && (i_objAusgangsObjekt.GetType().FUN_blnIstTypAbgeleitet(typeof(Visual)) || i_objAusgangsObjekt.GetType().FUN_blnIstTypAbgeleitet(typeof(Visual3D))))
			{
				FrameworkElement frameworkElement2 = VisualTreeHelper.GetParent(i_objAusgangsObjekt) as FrameworkElement;
				i_objAusgangsObjekt = frameworkElement2;
				if (frameworkElement2 != null && frameworkElement2.Name.Equals(i_strName))
				{
					frameworkElement = frameworkElement2;
				}
			}
			return frameworkElement;
		}
	}
}
