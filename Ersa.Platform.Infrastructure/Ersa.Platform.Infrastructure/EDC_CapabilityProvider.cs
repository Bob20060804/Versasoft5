using Ersa.Platform.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace Ersa.Platform.Infrastructure
{
	[Export(typeof(INF_CapabilityProvider))]
	public class EDC_CapabilityProvider : INF_CapabilityProvider
	{
		private readonly IDictionary<Type, Func<object>> m_dicRegistrierteCapabilities;

		private readonly IDictionary<Type, List<Func<object>>> m_dicRegistrierteMehrfachCapabilities;

		public EDC_CapabilityProvider()
		{
			m_dicRegistrierteCapabilities = new Dictionary<Type, Func<object>>();
			m_dicRegistrierteMehrfachCapabilities = new Dictionary<Type, List<Func<object>>>();
		}

		public void SUB_CapabilityRegistrieren<T>(Func<object> i_delCapabilityErmittler)
		{
			SUB_CapabilityDeregistrieren<T>();
			m_dicRegistrierteCapabilities.Add(typeof(T), i_delCapabilityErmittler);
		}

		public void SUB_CapabilityDeregistrieren<T>()
		{
			m_dicRegistrierteCapabilities.Remove(typeof(T));
		}

		public T FUN_edcCapabilityHolen<T>() where T : class
		{
			if (!m_dicRegistrierteCapabilities.TryGetValue(typeof(T), out Func<object> value))
			{
				return null;
			}
			return (T)value();
		}

		public void SUB_MehrfachCapabilityRegistrieren<T>(Func<object> i_delCapabilityErmittler)
		{
			if (!m_dicRegistrierteMehrfachCapabilities.ContainsKey(typeof(T)))
			{
				m_dicRegistrierteMehrfachCapabilities.Add(typeof(T), new List<Func<object>>());
			}
			if (m_dicRegistrierteMehrfachCapabilities.TryGetValue(typeof(T), out List<Func<object>> value))
			{
				value.Add(i_delCapabilityErmittler);
			}
		}

		public IEnumerable<T> FUN_edcMehrfachCapabilityListeHolen<T>() where T : class
		{
			List<T> list = new List<T>();
			if (m_dicRegistrierteMehrfachCapabilities.TryGetValue(typeof(T), out List<Func<object>> value))
			{
				foreach (Func<object> item in value)
				{
					T val = (T)item();
					if (val != null)
					{
						list.Add(val);
					}
				}
				return list;
			}
			return list;
		}
	}
}
