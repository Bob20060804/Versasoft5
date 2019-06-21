using Ersa.Global.DataAdapter.Adapter.Interface;
using Ersa.Global.DataAdapter.DatabaseModel.Model;
using Ersa.Global.DataAdapter.Exeptions;

namespace Ersa.Global.DataAdapter.DatabaseModel.Ersteller
{
	public class EDC_DatenbankErsteller : EDC_DatenbankErstellungsBasis
	{
		public EDC_DatenbankErsteller(INF_DatenbankModel i_edcDatenbankModel, INF_DatenbankAdapter i_edcDatenbankAdapter)
		{
			base.PRO_edcDatenbankDatenbankModel = i_edcDatenbankModel;
			base.PRO_edcDatenbankAdapter = i_edcDatenbankAdapter;
		}

		public void SUB_ErstelleServerbasierendeDatenbank()
		{
			SUB_ErstelleServerDatenbank();
			SUB_WarteInMillicekunden(2000);
			SUB_ErstelleTablespaces(base.PRO_edcDatenbankDatenbankModel);
			SUB_WarteInMillicekunden(5000);
		}

		public void SUB_ErstelleDatenbankKomponenten()
		{
			int num = 0;
			bool flag;
			do
			{
				SUB_WarteInMillicekunden(1000);
				flag = base.PRO_edcDatenbankAdapter.FUN_blnPruefeVerbindung(1000);
				num++;
			}
			while (!flag && num < 25);
			if (!flag)
			{
				throw new EDC_DatenbankErstellungsExeption("Unable to connect to database");
			}
			SUB_ErstelleTabellen();
			SUB_ErstelleSequences();
			SUB_ErstelleFunktionen();
			SUB_ErstelleTrigger();
			SUB_WarteInMillicekunden(2000);
			SUB_ErstelleConstraints();
			SUB_WarteInMillicekunden(2000);
		}

		public void SUB_ErstelleEineZusatztabelle(object i_objModell)
		{
			string i_strSkript = base.PRO_edcDatenbankDatenbankModel.FUN_strHoleStandardErstellungsStringFuerModell(i_objModell);
			SUB_FuehreSkriptAus(i_strSkript);
		}

		private void SUB_ErstelleServerDatenbank()
		{
			string pRO_strDatenbankErstellungsString = base.PRO_edcDatenbankDatenbankModel.PRO_strDatenbankErstellungsString;
			SUB_FuehreSkriptAus(pRO_strDatenbankErstellungsString);
		}

		private void SUB_ErstelleSequences()
		{
			string pRO_strSequencesErstellungsString = base.PRO_edcDatenbankDatenbankModel.PRO_strSequencesErstellungsString;
			SUB_FuehreSkriptAus(pRO_strSequencesErstellungsString);
		}

		private void SUB_ErstelleFunktionen()
		{
			string pRO_strFunctionErstellungsString = base.PRO_edcDatenbankDatenbankModel.PRO_strFunctionErstellungsString;
			SUB_FuehreSkriptAus(pRO_strFunctionErstellungsString);
		}

		private void SUB_ErstelleTrigger()
		{
			string pRO_strTriggerErstellungsString = base.PRO_edcDatenbankDatenbankModel.PRO_strTriggerErstellungsString;
			SUB_FuehreSkriptAus(pRO_strTriggerErstellungsString);
		}

		private void SUB_ErstelleConstraints()
		{
			string pRO_strConstraintsErstellungsString = base.PRO_edcDatenbankDatenbankModel.PRO_strConstraintsErstellungsString;
			SUB_FuehreSkriptAus(pRO_strConstraintsErstellungsString);
		}

		private void SUB_ErstelleTabellen()
		{
			foreach (string item in base.PRO_edcDatenbankDatenbankModel.PRO_lstTabellenErstellungsliste)
			{
				SUB_FuehreSkriptAus(item);
			}
		}
	}
}
