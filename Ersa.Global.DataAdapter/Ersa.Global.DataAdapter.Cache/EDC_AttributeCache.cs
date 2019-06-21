using Ersa.Global.Common.Data.Attributes;
using Ersa.Global.DataAdapter.Helper;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;

namespace Ersa.Global.DataAdapter.Cache
{
	public class EDC_AttributeCache
	{
		private static readonly SemaphoreSlim m_fdcSplatenSemaphore = new SemaphoreSlim(1);

		private static readonly SemaphoreSlim m_fdcTabellenSemaphore = new SemaphoreSlim(1);

		private static readonly SemaphoreSlim m_fdcWhereSemaphore = new SemaphoreSlim(1);

		private static readonly SemaphoreSlim m_fdcFilterSemaphore = new SemaphoreSlim(1);

		private static readonly Lazy<EDC_AttributeCache> ms_edcInstance = new Lazy<EDC_AttributeCache>(() => new EDC_AttributeCache());

		private readonly Dictionary<Type, Dictionary<PropertyInfo, EDC_SpaltenInformation>> m_fdcSpaltenInformationenCache = new Dictionary<Type, Dictionary<PropertyInfo, EDC_SpaltenInformation>>();

		private readonly Dictionary<Type, EDC_TabellenInformation> m_fdcTabellenNamenCache = new Dictionary<Type, EDC_TabellenInformation>();

		private readonly Dictionary<Type, PropertyInfo> m_fdcWherecache = new Dictionary<Type, PropertyInfo>();

		private readonly Dictionary<Type, Dictionary<PropertyInfo, EDC_FilterInformationen>> m_fdcFilterInformationenCache = new Dictionary<Type, Dictionary<PropertyInfo, EDC_FilterInformationen>>();

		public static EDC_AttributeCache PRO_edcInstance => ms_edcInstance.Value;

		public Dictionary<PropertyInfo, EDC_SpaltenInformation> FUN_fdcHoleSpaltenInformationen<T>(T i_edcModel)
		{
			Type type = i_edcModel as Type;
			return FUN_fdcHoleSpaltenInformationen(type ?? i_edcModel.GetType());
		}

		public Dictionary<PropertyInfo, EDC_SpaltenInformation> FUN_fdcHoleSpaltenInformationen(Type i_edcType)
		{
			Dictionary<PropertyInfo, EDC_SpaltenInformation> value = null;
			try
			{
				m_fdcSplatenSemaphore.Wait();
				if (m_fdcSpaltenInformationenCache.TryGetValue(i_edcType, out value))
				{
					return value;
				}
				value = EDC_AttributHelfer.FUN_fdcHoleSpaltenInformationen(i_edcType);
				m_fdcSpaltenInformationenCache.Add(i_edcType, value);
				return value;
			}
			finally
			{
				m_fdcSplatenSemaphore.Release(1);
			}
		}

		public Dictionary<PropertyInfo, EDC_FilterInformationen> FUN_fdcHoleFilterInformationen<T>(T i_edcModel)
		{
			Type type = i_edcModel as Type;
			return FUN_fdcHoleFilterIformationen(type ?? i_edcModel.GetType());
		}

		public Dictionary<PropertyInfo, EDC_FilterInformationen> FUN_fdcHoleFilterIformationen(Type i_edcType)
		{
			Dictionary<PropertyInfo, EDC_FilterInformationen> value = null;
			try
			{
				m_fdcFilterSemaphore.Wait();
				if (m_fdcFilterInformationenCache.TryGetValue(i_edcType, out value))
				{
					return value;
				}
				value = EDC_AttributHelfer.FUN_fdcHoleFilterInformationen(i_edcType);
				m_fdcFilterInformationenCache.Add(i_edcType, value);
				return value;
			}
			finally
			{
				m_fdcFilterSemaphore.Release(1);
			}
		}

		public EDC_TabellenInformation FUN_strHoleTabellenInformation<T>(T i_edcModel)
		{
			Type type = i_edcModel as Type;
			return FUN_strHoleTabellenInformation(type ?? i_edcModel.GetType());
		}

		public EDC_TabellenInformation FUN_strHoleTabellenInformation(Type i_edcType)
		{
			EDC_TabellenInformation value = null;
			try
			{
				m_fdcTabellenSemaphore.Wait();
				if (m_fdcTabellenNamenCache.TryGetValue(i_edcType, out value))
				{
					return value;
				}
				value = EDC_AttributHelfer.FUN_strHoleTabellenInformationen(i_edcType);
				m_fdcTabellenNamenCache.Add(i_edcType, value);
				return value;
			}
			finally
			{
				m_fdcTabellenSemaphore.Release(1);
			}
		}

		public string FUN_strHoleWhereStatement<T>(T i_edcModel)
		{
			Type type = i_edcModel.GetType();
			PropertyInfo value;
			try
			{
				m_fdcWhereSemaphore.Wait();
				if (!m_fdcWherecache.TryGetValue(type, out value))
				{
					value = i_edcModel.FUN_fdcHoleWhereStatementProperty();
					m_fdcWherecache.Add(type, value);
				}
			}
			finally
			{
				m_fdcWhereSemaphore.Release(1);
			}
			if (value != null)
			{
				return (string)value.GetValue(i_edcModel);
			}
			return string.Empty;
		}
	}
}
