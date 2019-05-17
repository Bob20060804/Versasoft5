using System;
using System.Runtime.Serialization;

namespace Ersa.Global.Controls.BildEditor.Exceptions
{
	[Serializable]
	public class EDC_BildEditorCanvasException : Exception
	{
		public EDC_BildEditorCanvasException()
			: base("Bildeditor Fehler")
		{
		}

		public EDC_BildEditorCanvasException(string i_strMeldung)
			: base(i_strMeldung)
		{
		}

		public EDC_BildEditorCanvasException(string i_strMeldung, Exception i_fdcInnerException)
			: base(i_strMeldung, i_fdcInnerException)
		{
		}

		protected EDC_BildEditorCanvasException(SerializationInfo i_fdcInfo, StreamingContext i_fdcContext)
			: base(i_fdcInfo, i_fdcContext)
		{
		}
	}
}
