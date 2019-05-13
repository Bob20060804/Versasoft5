using Ersa.Global.Common;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace Ersa.Platform.Infrastructure.Validierung
{
	public abstract class EDC_NotificationObjectMitValidierung : EDC_NotificationObjectMitSprachUmschaltung
	{
		private readonly object m_objSyncObject = new object();

		private IReadOnlyDictionary<string, IEnumerable<EDC_PropertyValidierungsRegel>> m_dicValidierungsRegeln;

		public bool PRO_blnIstValide => !PRO_lstValidierungsFehler.Any();

		public EDC_SmartObservableCollection<EDC_PropertyValidierungsFehler> PRO_lstValidierungsFehler
		{
			get;
			private set;
		}

		protected EDC_NotificationObjectMitValidierung()
		{
			PRO_lstValidierungsFehler = new EDC_SmartObservableCollection<EDC_PropertyValidierungsFehler>();
			CollectionChangedEventManager.AddHandler(PRO_lstValidierungsFehler, delegate
			{
				RaisePropertyChanged("PRO_blnIstValide");
			});
		}

		protected virtual IEnumerable<EDC_PropertyValidierungsRegeln> FUN_enuValidierungsRegelnErmitteln()
		{
			return Enumerable.Empty<EDC_PropertyValidierungsRegeln>();
		}

		protected void SUB_ValidierungAktualisieren(string i_strPropertyName = null)
		{
			if (!(i_strPropertyName == "PRO_blnIstValide"))
			{
				lock (m_objSyncObject)
				{
					if (string.IsNullOrEmpty(i_strPropertyName))
					{
						PRO_lstValidierungsFehler.SUB_Reset(FUN_enuValidierungsFehlerErmitteln());
					}
					else
					{
						List<EDC_PropertyValidierungsFehler> list = (from i_edcFehler in PRO_lstValidierungsFehler
						where i_edcFehler.PRO_strPropertyName == i_strPropertyName
						select i_edcFehler).ToList();
						if (list.Any())
						{
							PRO_lstValidierungsFehler.SUB_RemoveRange(list);
						}
						List<EDC_PropertyValidierungsFehler> list2 = FUN_enuValidierungsFehlerErmitteln(i_strPropertyName).ToList();
						if (list2.Any())
						{
							PRO_lstValidierungsFehler.SUB_AddRange(list2);
						}
					}
				}
			}
		}

		protected IEnumerable<EDC_PropertyValidierungsFehler> FUN_enuValidierungsFehlerErmitteln(string i_strPropertyName = null)
		{
			SUB_ValidierungsRegelnInitialisieren();
			foreach (EDC_PropertyValidierungsRegel item in FUN_enuRegelnErmitteln(i_strPropertyName))
			{
				EDC_PropertyValidierungsFehler eDC_PropertyValidierungsFehler = item.FUN_edcValidieren();
				if (eDC_PropertyValidierungsFehler != null)
				{
					yield return eDC_PropertyValidierungsFehler;
				}
			}
		}

		protected override void OnPropertyChanged(PropertyChangedEventArgs i_fdcArgs)
		{
			base.OnPropertyChanged(i_fdcArgs);
			SUB_ValidierungAktualisieren(i_fdcArgs?.PropertyName);
		}

		private void SUB_ValidierungsRegelnInitialisieren()
		{
			if (m_dicValidierungsRegeln == null)
			{
				m_dicValidierungsRegeln = (from i_edcRegeln in FUN_enuValidierungsRegelnErmitteln()
				group i_edcRegeln by i_edcRegeln.PRO_strPropertyName).ToDictionary((IGrouping<string, EDC_PropertyValidierungsRegeln> i_enuGruppe) => i_enuGruppe.Key, (IGrouping<string, EDC_PropertyValidierungsRegeln> i_enuGruppe) => i_enuGruppe.SelectMany((EDC_PropertyValidierungsRegeln i_edcRegeln) => i_edcRegeln.PRO_enuRegeln));
			}
		}

		private IEnumerable<EDC_PropertyValidierungsRegel> FUN_enuRegelnErmitteln(string i_strPropertyName)
		{
			if (!string.IsNullOrEmpty(i_strPropertyName))
			{
				if (!m_dicValidierungsRegeln.ContainsKey(i_strPropertyName))
				{
					return Enumerable.Empty<EDC_PropertyValidierungsRegel>();
				}
				return m_dicValidierungsRegeln[i_strPropertyName];
			}
			return m_dicValidierungsRegeln.Values.SelectMany((IEnumerable<EDC_PropertyValidierungsRegel> i_enuRegeln) => i_enuRegeln);
		}
	}
}
