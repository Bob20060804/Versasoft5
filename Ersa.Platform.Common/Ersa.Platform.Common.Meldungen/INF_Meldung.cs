using Ersa.Platform.Common.Data.Meldungen;
using System;
using System.Collections.Generic;

namespace Ersa.Platform.Common.Meldungen
{
    /// <summary>
    /// Message
    /// </summary>
	public interface INF_Meldung : IEquatable<INF_Meldung>
	{
		/// <summary>
		/// Message guid
		/// </summary>
		string PRO_strMeldungGuid
		{
			get;
		}

		/// <summary>
		/// Message producer(Machine|MES|Versascan)
		/// </summary>
		ENUM_MeldungProduzent PRO_enmMeldungProduzent
		{
			get;
		}

		/// <summary>
		/// Producer Code
		/// </summary>
		int PRO_i32ProduzentenCode
		{
			get;
		}

		/// <summary>
		/// 发生时间
		/// </summary>
		DateTime? PRO_sttAufgetreten
		{
			get;
		}

		/// <summary>
		/// 确认时间
		/// </summary>
		DateTime? PRO_sttQuittiert
		{
			get;
			set;
		}

		/// <summary>
		/// 推迟时间
		/// </summary>
		DateTime? PRO_sttZurueckgestellt
		{
			get;
			set;
		}

		/// <summary>
		/// 消息类型
		/// </summary>
		ENUM_MeldungsTypen PRO_enmMeldungsTyp
		{
			get;
		}

		/// <summary>
		/// 操作模式
		/// </summary>
		int PRO_i32Betriebsart
		{
			get;
		}

		/// <summary>
		/// Message Number
		/// </summary>
		int PRO_i32MeldungsNummer
		{
			get;
		}

		/// <summary>
		/// 消息地点1
		/// </summary>
		int PRO_i32MeldungsOrt1
		{
			get;
		}

		/// <summary>
		/// 消息地点2
		/// </summary>
		int PRO_i32MeldungsOrt2
		{
			get;
		}

		/// <summary>
		/// 消息地点3
		/// </summary>
		int PRO_i32MeldungsOrt3
		{
			get;
		}

		/// <summary>
		/// 细节
		/// </summary>
		string PRO_strDetails
		{
			get;
			set;
		}

		/// <summary>
		/// 内容
		/// </summary>
		string PRO_strContext
		{
			get;
			set;
		}

		/// <summary>
		/// 信号的流程动作
		/// </summary>
		IEnumerable<ENUM_ProzessAktionen> PRO_enuProzessAktionen
		{
			get;
			set;
		}

		/// <summary>
		/// 可采取的行动(创造|确认...)
		/// </summary>
		IEnumerable<ENUM_MeldungAktionen> PRO_enuMoeglicheAktionen
		{
			get;
			set;
		}

		/// <summary>
		/// 信息分类标准
		/// </summary>
		string PRO_strMeldungSortierKriterium
		{
			get;
		}

		/// <summary>
		/// 报告位置1
		/// </summary>
		string PRO_strMeldeort1
		{
			get;
			set;
		}

		/// <summary>
		/// 报告位置2
		/// </summary>
		string PRO_strMeldeort2
		{
			get;
			set;
		}

		/// <summary>
		/// 报告位置3
		/// </summary>
		string PRO_strMeldeort3
		{
			get;
			set;
		}

		/// <summary>
		/// 报告文本
		/// </summary>
		string PRO_strMeldetext
		{
			get;
			set;
		}

		/// <summary>
		/// User
		/// </summary>
		string PRO_strBenutzerName
		{
			get;
			set;
		}

		/// <summary>
		/// Can be reset
		/// </summary>
		bool PRO_blnKannZurueckgestelltWerden
		{
			get;
		}

		/// <summary>
		/// can be Acknowledged
		/// </summary>
		bool PRO_blnKannQuittiertWerden
		{
			get;
		}
	}
}
