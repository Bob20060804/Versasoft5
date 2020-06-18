using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace Ersa.Platform.Plc.KommunikationsDienst
{
    /// <summary>
    /// 参数类型行为
    /// Actions for parameter types
    /// </summary>
	[Export]
	public class EDC_AktionenFuerParameterTypen
	{
		private readonly INF_SpsProvider m_edcSpsProvider;

		private INF_Sps m_edcSpsService;
        private INF_Sps PRO_edcSpsService => m_edcSpsService ?? (m_edcSpsService = m_edcSpsProvider.FUN_edcAktiveSps());

        public IDictionary<ENUM_SpsTyp, Func<string, object>> PRO_dicLeseAktionen
		{
			get;
			set;
		}

		public IDictionary<ENUM_SpsTyp, Action<string, object>> PRO_dicSchreibeAktionen
		{
			get;
			set;
		}

		[ImportingConstructor]
		public EDC_AktionenFuerParameterTypen(INF_SpsProvider i_delSpsProvider)
		{
			m_edcSpsProvider = i_delSpsProvider;
			SUB_InitializiereLeseAktionen();
			SUB_InitializiereSchreibeAktionen();
		}

        /// <summary>
        /// Initialize Read actions
        /// </summary>
		private void SUB_InitializiereLeseAktionen()
		{
			PRO_dicLeseAktionen = new Dictionary<ENUM_SpsTyp, Func<string, object>>
			{
				{
					ENUM_SpsTyp.enmUInt32,
					(string i_strAdresse) => PRO_edcSpsService.FUN_u32WertLesen(i_strAdresse)
				},
				{
					ENUM_SpsTyp.enmInt32,
					(string i_strAdresse) => PRO_edcSpsService.FUN_i32WertLesen(i_strAdresse)
				},
				{
					ENUM_SpsTyp.enmUInt16,
					(string i_strAdresse) => PRO_edcSpsService.FUN_u16WertLesen(i_strAdresse)
				},
				{
					ENUM_SpsTyp.enmInt16,
					(string i_strAdresse) => PRO_edcSpsService.FUN_i16WertLesen(i_strAdresse)
				},
				{
					ENUM_SpsTyp.enmByte,
					(string i_strAdresse) => PRO_edcSpsService.FUN_bytWertLesen(i_strAdresse)
				},
				{
					ENUM_SpsTyp.enmBool,
					(string i_strAdresse) => PRO_edcSpsService.FUN_blnWertLesen(i_strAdresse)
				},
				{
					ENUM_SpsTyp.enmString,
					(string i_strAdresse) => PRO_edcSpsService.FUN_strWertLesen(i_strAdresse)
				},
				{
					ENUM_SpsTyp.enmSingle,
					(string i_strAdresse) => PRO_edcSpsService.FUN_sngWertLesen(i_strAdresse)
				}
			};
		}

		private void SUB_InitializiereSchreibeAktionen()
		{
			Array values = Enum.GetValues(typeof(ENUM_SpsTyp));
			PRO_dicSchreibeAktionen = new Dictionary<ENUM_SpsTyp, Action<string, object>>();
			foreach (ENUM_SpsTyp item in values)
			{
				PRO_dicSchreibeAktionen.Add(item, delegate(string i_strAdresse, object i_objValue)
				{
					PRO_edcSpsService.SUB_WertSchreiben(i_strAdresse, i_objValue.ToString());
				});
			}
		}
	}
}
