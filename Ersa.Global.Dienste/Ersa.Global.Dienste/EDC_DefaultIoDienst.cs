using Ersa.Global.Dienste.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;

namespace Ersa.Global.Dienste
{
	[Export(typeof(INF_IODienst))]
	public class EDC_DefaultIoDienst : INF_IODienst
	{
		public bool FUN_blnIstDateiSchreibgeschuetzt(string i_strPfad)
		{
			if (FUN_blnDateiExistiert(i_strPfad))
			{
				return (File.GetAttributes(i_strPfad) & FileAttributes.ReadOnly) == FileAttributes.ReadOnly;
			}
			return false;
		}

		public void SUB_DateiKopieren(string i_strQuellPfad, string i_strDestinationPfad, bool i_blnUeberschreiben)
		{
			File.Copy(i_strQuellPfad, i_strDestinationPfad, i_blnUeberschreiben);
		}

		public void SUB_DateiVerschieben(string i_strQuellPfad, string i_strDestinationPfad, bool i_blnUeberschreiben)
		{
			if (i_blnUeberschreiben && FUN_blnDateiExistiert(i_strDestinationPfad))
			{
				SUB_DateiLoeschen(i_strDestinationPfad);
			}
			File.Move(i_strQuellPfad, i_strDestinationPfad);
		}

		public void SUB_DateiLoeschen(string i_strPfad)
		{
			File.Delete(i_strPfad);
		}

		public void SUB_DateiSchreibschutzEntfernen(string i_strDateiPfad)
		{
			FileAttributes fileAttributes = File.GetAttributes(i_strDateiPfad) & ~FileAttributes.ReadOnly;
			File.SetAttributes(i_strDateiPfad, fileAttributes);
		}

		public void SUB_DateiInhaltSchreiben(string i_strDateiPfad, IEnumerable<string> i_enuDateiInhalt, Encoding i_fdcEncoding = null)
		{
			if (i_fdcEncoding != null)
			{
				File.WriteAllLines(i_strDateiPfad, i_enuDateiInhalt, i_fdcEncoding);
			}
			else
			{
				File.WriteAllLines(i_strDateiPfad, i_enuDateiInhalt);
			}
		}

		public void SUB_DateiInhaltSchreiben(string i_strDateiPfad, string i_strDateiInhalt, Encoding i_fdcEncoding = null)
		{
			if (i_fdcEncoding != null)
			{
				File.WriteAllText(i_strDateiPfad, i_strDateiInhalt, i_fdcEncoding);
			}
			else
			{
				File.WriteAllText(i_strDateiPfad, i_strDateiInhalt);
			}
		}

		public IList<string> FUN_lstDateiInhaltLesen(string i_strDateiPfad)
		{
			if (!FUN_blnDateiExistiert(i_strDateiPfad))
			{
				throw new FileNotFoundException($"File not found {i_strDateiPfad}", i_strDateiPfad);
			}
			return File.ReadAllLines(i_strDateiPfad);
		}

		public string FUN_strDateiInhaltLesen(string i_strDateiPfad)
		{
			if (!FUN_blnDateiExistiert(i_strDateiPfad))
			{
				throw new FileNotFoundException($"File not found {i_strDateiPfad}", i_strDateiPfad);
			}
			return File.ReadAllText(i_strDateiPfad);
		}

		public DateTime FUN_dtmDateiDatumErmitteln(string i_strDateipfad)
		{
			return File.GetLastWriteTime(i_strDateipfad);
		}

		public string FUN_strValidiereUndBereinigeDateiNamen(string i_strDateiName)
		{
			return new string(Path.GetInvalidFileNameChars()).Aggregate(i_strDateiName, (string i_strNeuerName, char i_chrZeichen) => i_strNeuerName.Replace(i_chrZeichen.ToString(), string.Empty));
		}

		public string FUN_strErmittleVerzeichnisName(string i_strVerzeichnisPfad)
		{
			return i_strVerzeichnisPfad.Split(new char[1]
			{
				Path.DirectorySeparatorChar
			}, StringSplitOptions.RemoveEmptyEntries).Last();
		}

		public bool FUN_blnVerzeichnisExistiert(string i_strPfad)
		{
			return Directory.Exists(i_strPfad);
		}

		public bool FUN_blnDateiExistiert(string i_strPfad)
		{
			return File.Exists(i_strPfad);
		}

		public IEnumerable<string> FUN_lstHoleUnterverzeichnisse(string i_strPfad, bool i_blnKurzeVerzeichnisNamenZurueckgeben)
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(i_strPfad);
			directoryInfo.Refresh();
			string[] array = (from i_fdcDirInfo in directoryInfo.GetDirectories()
			select i_fdcDirInfo.FullName).ToArray();
			if (!i_blnKurzeVerzeichnisNamenZurueckgeben)
			{
				return array;
			}
			return array.Select(FUN_strErmittleVerzeichnisName);
		}

		public void SUB_VerzeichnisVerschieben(string i_strPfad, string i_strNeuerPfad)
		{
			Directory.Move(i_strPfad, i_strNeuerPfad);
		}

		public void SUB_VerzeichnisRekursivLoeschen(string i_strPfad)
		{
			SUB_SchreibschutzEntfernen(i_strPfad, i_blnRekursiv: true);
			Directory.Delete(i_strPfad, recursive: true);
		}

		public void SUB_VerzeichnisLoeschen(string i_strPfad)
		{
			SUB_SchreibschutzEntfernen(i_strPfad, i_blnRekursiv: true);
			new DirectoryInfo(i_strPfad).Delete(recursive: true);
		}

		public void SUB_VerzeichnisErstellen(string i_strPfad)
		{
			Directory.CreateDirectory(i_strPfad);
		}

		public void SUB_VerzeichnisKopieren(string i_strPfad, string i_strNeuerPfad, bool i_blnUeberschreiben = false, string i_strDateiAusnahmenFilter = null)
		{
			if (!FUN_blnVerzeichnisExistiert(i_strPfad))
			{
				throw new DirectoryNotFoundException($"Source directory does not exist or could not be found: {i_strPfad}");
			}
			if (!FUN_blnVerzeichnisExistiert(i_strNeuerPfad))
			{
				SUB_VerzeichnisErstellen(i_strNeuerPfad);
			}
			IEnumerable<string> second = string.IsNullOrEmpty(i_strDateiAusnahmenFilter) ? Enumerable.Empty<string>() : FUN_enuHoleDateiListeAusVerzeichnis(i_strPfad, i_strDateiAusnahmenFilter);
			foreach (string item in FUN_enuDateienImVerzeichnisErmitteln(i_strPfad).Except(second))
			{
				string fileName = Path.GetFileName(item);
				if (!string.IsNullOrEmpty(fileName))
				{
					SUB_DateiKopieren(item, Path.Combine(i_strNeuerPfad, fileName), i_blnUeberschreiben);
				}
			}
			string[] directories = Directory.GetDirectories(i_strPfad);
			foreach (string text in directories)
			{
				SUB_VerzeichnisKopieren(text, Path.Combine(i_strNeuerPfad, FUN_strErmittleVerzeichnisName(text)), i_blnUeberschreiben, i_strDateiAusnahmenFilter);
			}
		}

		public IEnumerable<string> FUN_enuDateienImVerzeichnisErmitteln(string i_strPfad)
		{
			return Directory.GetFiles(i_strPfad);
		}

		public IEnumerable<string> FUN_enuHoleDateiListeAusVerzeichnis(string i_strPfad, IEnumerable<string> i_lstDateiExtensions)
		{
			return from i_strFile in Directory.GetFiles(i_strPfad, "*.*", SearchOption.AllDirectories)
			where i_lstDateiExtensions.Any(i_strFile.EndsWith)
			select i_strFile;
		}

		public IEnumerable<string> FUN_enuHoleDateiListeAusVerzeichnis(string i_strPfad, string i_strSearchPattern)
		{
			return Directory.GetFiles(i_strPfad, i_strSearchPattern, SearchOption.TopDirectoryOnly);
		}

		public bool FUN_blnIstValiderOrdnerName(string i_strName)
		{
			if (string.IsNullOrWhiteSpace(i_strName))
			{
				return false;
			}
			char[] lstUngueltigeZeichen = Path.GetInvalidFileNameChars();
			return i_strName.All((char i_objZeichen) => !lstUngueltigeZeichen.Contains(i_objZeichen));
		}

		public IEnumerable<int> FUNa_i32SerielleSchnittstellenNummernErmitteln()
		{
			IEnumerable<string> enumerable = FUNa_strSerielleSchnittstellenNamenErmitteln();
			foreach (string item in enumerable)
			{
				if (item.StartsWith("COM") && item.Length > 3 && int.TryParse(item.Substring(3), out int result))
				{
					yield return result;
				}
			}
		}

		public Stream FUN_fdcDateiOeffnen(string i_strDatei, FileMode i_fdcFileMode)
		{
			return File.Open(i_strDatei, i_fdcFileMode);
		}

		public Stream FUN_fdcDateiLesendOeffnen(string i_strDatei)
		{
			return File.Open(i_strDatei, FileMode.Open, FileAccess.Read, FileShare.Read);
		}

		private IEnumerable<string> FUNa_strSerielleSchnittstellenNamenErmitteln()
		{
			try
			{
				return SerialPort.GetPortNames();
			}
			catch (Win32Exception)
			{
				return new string[0];
			}
		}

		private void SUB_SchreibschutzEntfernen(string i_strVerzeichnisPfad, bool i_blnRekursiv)
		{
			string[] files = Directory.GetFiles(i_strVerzeichnisPfad);
			foreach (string i_strDateiPfad in files)
			{
				SUB_DateiSchreibschutzEntfernen(i_strDateiPfad);
			}
			if (i_blnRekursiv)
			{
				foreach (string item in FUN_lstHoleUnterverzeichnisse(i_strVerzeichnisPfad, i_blnKurzeVerzeichnisNamenZurueckgeben: false))
				{
					SUB_SchreibschutzEntfernen(item, i_blnRekursiv: true);
				}
			}
		}
	}
}
