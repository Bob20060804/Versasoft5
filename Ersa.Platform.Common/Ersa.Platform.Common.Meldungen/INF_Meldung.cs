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
		/// ����ʱ��
		/// </summary>
		DateTime? PRO_sttAufgetreten
		{
			get;
		}

		/// <summary>
		/// ȷ��ʱ��
		/// </summary>
		DateTime? PRO_sttQuittiert
		{
			get;
			set;
		}

		/// <summary>
		/// �Ƴ�ʱ��
		/// </summary>
		DateTime? PRO_sttZurueckgestellt
		{
			get;
			set;
		}

		/// <summary>
		/// ��Ϣ����
		/// </summary>
		ENUM_MeldungsTypen PRO_enmMeldungsTyp
		{
			get;
		}

		/// <summary>
		/// ����ģʽ
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
		/// ��Ϣ�ص�1
		/// </summary>
		int PRO_i32MeldungsOrt1
		{
			get;
		}

		/// <summary>
		/// ��Ϣ�ص�2
		/// </summary>
		int PRO_i32MeldungsOrt2
		{
			get;
		}

		/// <summary>
		/// ��Ϣ�ص�3
		/// </summary>
		int PRO_i32MeldungsOrt3
		{
			get;
		}

		/// <summary>
		/// ϸ��
		/// </summary>
		string PRO_strDetails
		{
			get;
			set;
		}

		/// <summary>
		/// ����
		/// </summary>
		string PRO_strContext
		{
			get;
			set;
		}

		/// <summary>
		/// �źŵ����̶���
		/// </summary>
		IEnumerable<ENUM_ProzessAktionen> PRO_enuProzessAktionen
		{
			get;
			set;
		}

		/// <summary>
		/// �ɲ�ȡ���ж�(����|ȷ��...)
		/// </summary>
		IEnumerable<ENUM_MeldungAktionen> PRO_enuMoeglicheAktionen
		{
			get;
			set;
		}

		/// <summary>
		/// ��Ϣ�����׼
		/// </summary>
		string PRO_strMeldungSortierKriterium
		{
			get;
		}

		/// <summary>
		/// ����λ��1
		/// </summary>
		string PRO_strMeldeort1
		{
			get;
			set;
		}

		/// <summary>
		/// ����λ��2
		/// </summary>
		string PRO_strMeldeort2
		{
			get;
			set;
		}

		/// <summary>
		/// ����λ��3
		/// </summary>
		string PRO_strMeldeort3
		{
			get;
			set;
		}

		/// <summary>
		/// �����ı�
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
