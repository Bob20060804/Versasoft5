using Ersa.Platform.Mes.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace Ersa.Platform.Mes
{
	[Export(typeof(INF_MesContainer))]
	public class EDC_MesContainer : INF_MesContainer
	{
		private readonly IDictionary<Type, object> m_dicObjects;

		public EDC_MesContainer()
		{
			m_dicObjects = new Dictionary<Type, object>();
		}

		public void SUB_AddObject<T>(object i_objObject)
		{
			SUB_RemoveObject<T>();
			m_dicObjects.Add(typeof(T), i_objObject);
		}

		public void SUB_RemoveObject<T>()
		{
			m_dicObjects.Remove(typeof(T));
		}

		public T FUN_edcGetObject<T>() where T : class
		{
			if (!m_dicObjects.TryGetValue(typeof(T), out object value))
			{
				return null;
			}
			return (T)value;
		}
	}
}
