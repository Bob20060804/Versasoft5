using Ersa.Global.Controls.Editoren.EditorElemente;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Ersa.Global.Controls.Editoren.Helfer
{
	public static class EDC_LinienAbschnittHelfer
	{
		private static readonly Vector ms_sttYVector = new Vector(0.0, 1.0);

		public static IEnumerable<EDC_RichtungspfeilDefinition> FUN_enuHoleRichtungspfeile(IEnumerable<Point> i_enuPunkte)
		{
			return FUN_enuLinienAbschnitteZuRichtungsPfeilen(FUN_lstHoleLinienAbschnitte(i_enuPunkte));
		}

		public static IReadOnlyList<Tuple<Point, Point>> FUN_lstHoleLinienAbschnitte(IEnumerable<Point> i_enuPunkte)
		{
			List<Tuple<Point, Point>> list = new List<Tuple<Point, Point>>();
			if (i_enuPunkte == null)
			{
				return list;
			}
			Point? point = null;
			foreach (Point item in i_enuPunkte)
			{
				if (!point.HasValue)
				{
					point = item;
				}
				else
				{
					list.Add(new Tuple<Point, Point>(point.Value, item));
					point = item;
				}
			}
			if (list.Count == 0 && point.HasValue)
			{
				list.Add(new Tuple<Point, Point>(point.Value, point.Value));
			}
			return list;
		}

		public static IEnumerable<Vector> FUN_enuLinienAbschnitteZuVektoren(IEnumerable<Tuple<Point, Point>> i_enuAbschnitte)
		{
			return i_enuAbschnitte.Select(FUN_sttAbschnittZuVector);
		}

		public static IEnumerable<EDC_RichtungspfeilDefinition> FUN_enuLinienAbschnitteZuRichtungsPfeilen(IEnumerable<Tuple<Point, Point>> i_enuAbschnitte)
		{
			return i_enuAbschnitte.Select(FUN_fdcAbschnittZuMittelpunktRichtungTupel);
		}

		public static double FUN_dblHoleWinkelFuerVektorZuYAchse(Vector i_sttVektor)
		{
			return Vector.AngleBetween(ms_sttYVector, i_sttVektor);
		}

		private static EDC_RichtungspfeilDefinition FUN_fdcAbschnittZuMittelpunktRichtungTupel(Tuple<Point, Point> i_fdcAbschnitt)
		{
			return new EDC_RichtungspfeilDefinition(FUN_sttHoleMittelpunkt(i_fdcAbschnitt), FUN_dblHoleWinkelFuerVektorZuYAchse(FUN_sttAbschnittZuVector(i_fdcAbschnitt)));
		}

		private static Point FUN_sttHoleMittelpunkt(Tuple<Point, Point> i_fdcAbschnitt)
		{
			return new Point((i_fdcAbschnitt.Item1.X + i_fdcAbschnitt.Item2.X) / 2.0, (i_fdcAbschnitt.Item1.Y + i_fdcAbschnitt.Item2.Y) / 2.0);
		}

		private static Vector FUN_sttAbschnittZuVector(Tuple<Point, Point> i_fdcAbschnitt)
		{
			return new Vector(i_fdcAbschnitt.Item2.X - i_fdcAbschnitt.Item1.X, i_fdcAbschnitt.Item2.Y - i_fdcAbschnitt.Item1.Y);
		}
	}
}
