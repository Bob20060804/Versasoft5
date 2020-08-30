using Ersa.Global.Common;
using Ersa.Platform.Common.Data.Meldungen;
using Ersa.Platform.Common.Meldungen;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ersa.Platform.Dienste.Meldungen.Interfaces
{
	/// <summary>
	/// ��Ϣ���������
	/// message administration service
	/// </summary>
	public interface INF_MeldungVerwaltungsDienst
	{
		/// <summary>
		/// �۲�ļ���
		/// </summary>
		EDC_SmartObservableCollection<INF_Meldung> PRO_lstNichtQuittierteMeldungen
		{
			get;
			set;
		}

		/// <summary>
		/// ת����Ϊ�Ǽ�
		/// </summary>
		/// <param name="i_objSender"></param>
		/// <param name="i_delAction"></param>
		/// <returns></returns>
		IDisposable FUN_fdcWeitergabeAktionRegistrieren(object i_objSender, Func<IEnumerable<INF_Meldung>, ENUM_MeldungAktionen, bool, Task> i_delAction);

		/// <summary>
		/// ��ʼ����Ϣ�첽
		/// </summary>
		/// <returns></returns>
		Task FUN_fdcMeldungenInitialisierenAsync();

		/// <summary>
		/// ������Ϣ�첽
		/// handle messages async
		/// </summary>
		/// <param name="i_enuMeldungen"></param>
		/// <param name="i_enmAktion"></param>
		/// <returns></returns>
		Task FUN_fdcMeldungenBehandelnAsync(IEnumerable<INF_Meldung> i_enuMeldungen, ENUM_MeldungAktionen i_enmAktion);

		/// <summary>
		/// ������ȷ�ϵ���Ϣ
		/// </summary>
		/// <param name="i_enmMeldungProduzent"></param>
		/// <param name="i_enmMeldungsTyp"></param>
		/// <param name="i_sttVon"></param>
		/// <param name="i_sttBis"></param>
		/// <returns></returns>
		Task<IEnumerable<INF_Meldung>> FUN_fdcQuittierteMeldungenImZeitraumLadenAsync(ENUM_MeldungProduzent i_enmMeldungProduzent, ENUM_MeldungsTypen i_enmMeldungsTyp, DateTime i_sttVon, DateTime i_sttBis);

		/// <summary>
		/// ����δȷ�ϵ���Ϣ
		/// </summary>
		/// <param name="i_enmMeldungProduzent"></param>
		/// <returns></returns>
		Task<IEnumerable<INF_Meldung>> FUN_fdcNichtQuittierteMeldungenLadenAsync(ENUM_MeldungProduzent i_enmMeldungProduzent);

		/// <summary>
		/// ˢ��δȷ�ϵ���Ϣ
		/// </summary>
		/// <param name="i_enmMeldungProduzent"></param>
		/// <returns></returns>
		Task FUN_fdcNichtQuittierteMeldungenRefreshenAsync(ENUM_MeldungProduzent i_enmMeldungProduzent);

		/// <summary>
		/// ��ǰδȷ�ϵ���Ϣ�б�
		/// </summary>
		/// <param name="i_enmMeldungProduzent"></param>
		/// <returns></returns>
		IEnumerable<INF_Meldung> FUN_lstAktuelleNichtQuittierteMeldungenListe(ENUM_MeldungProduzent i_enmMeldungProduzent);

		/// <summary>
		/// ȷ�����е���Ϣ������
		/// </summary>
		/// <returns></returns>
		IEnumerable<ENUM_MeldungProduzent> FUN_enuVorhandeneMeldungsProduzentenErmitteln();

		/// <summary>
		/// �Դ���Ϣ����
		/// </summary>
		/// <param name="i_enuMeldungen"></param>
		/// <param name="i_enmAktion"></param>
		/// <returns></returns>
		Task FUN_fdcMeldungBehandelnAnfordernAsync(IEnumerable<INF_Meldung> i_enuMeldungen, ENUM_MeldungAktionen i_enmAktion);

		/// <summary>
		/// ��Ϣ�Ѵ���
		/// </summary>
		/// <param name="i_enmMeldungProduzent"></param>
		/// <param name="i_i32ProduzentenCode"></param>
		/// <param name="i_i32MeldungsNummer"></param>
		/// <param name="i_i32MeldungsOrt1"></param>
		/// <param name="i_i32MeldungsOrt2"></param>
		/// <param name="i_i32MeldungsOrt3"></param>
		/// <returns></returns>
		bool FUN_blnIstMeldungBereitsVorhanden(ENUM_MeldungProduzent i_enmMeldungProduzent, int i_i32ProduzentenCode, int i_i32MeldungsNummer, int i_i32MeldungsOrt1, int i_i32MeldungsOrt2, int i_i32MeldungsOrt3);
	}
}
