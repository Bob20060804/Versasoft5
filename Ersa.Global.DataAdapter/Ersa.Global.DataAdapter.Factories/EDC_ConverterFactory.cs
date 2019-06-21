using Ersa.Global.DataAdapter.Converter;
using Ersa.Global.DataAdapter.Converter.Impl;
using Ersa.Global.DataAdapter.Converter.Interfaces;
using System;

namespace Ersa.Global.DataAdapter.Factories
{
	public static class EDC_ConverterFactory
	{
		public static T FUN_fdcErstelleObjektInstanz<T>() where T : class
		{
			return Activator.CreateInstance(typeof(T)) as T;
		}

		public static INF_ObjektAusDataRow<T> FUN_edcErstelleObjektAusDataRowConverter<T>() where T : class
		{
			return new EDC_ObjektAusDataRow<T>();
		}

		public static INF_ObjektAusReader<T> FUN_edcErstelleObjektAusDataReaderConverter<T>()
		{
			return new EDC_ObjektAusReader<T>();
		}

		public static INF_ObjektZuDataRow<T> FUN_edcErstelleObjektZuDataRowConverter<T>()
		{
			return new EDC_ObjektZuDataRow<T>();
		}

		public static INF_SqlStatementErsteller<T> FUN_edcHoleSqlErsteller<T>(ENUM_SqlErstellerTyp i_enmSqlTyp)
		{
			switch (i_enmSqlTyp)
			{
			case ENUM_SqlErstellerTyp.Insert:
				return new EDC_ObjektInserter<T>();
			case ENUM_SqlErstellerTyp.Update:
				return new EDC_ObjektUpdater<T>();
			case ENUM_SqlErstellerTyp.Select:
				return new EDC_ObjektSelector<T>();
			case ENUM_SqlErstellerTyp.Delete:
				return new EDC_ObjektDeleter<T>();
			default:
				throw new ArgumentOutOfRangeException("i_enmSqlTyp", $"The statement creator type '{i_enmSqlTyp}' is invalid or not supported.");
			}
		}
	}
}
