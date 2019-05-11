#define TRACE
using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace BR.AN.Logging
{
	public class Dbg
	{
		private static DbgProblemCollection problems = new DbgProblemCollection();

		public static DbgProblemCollection Problems => problems;

		public static bool AutoFlush
		{
			get
			{
				return Debug.AutoFlush;
			}
			set
			{
				Debug.AutoFlush = value;
			}
		}

		public static int IndentLevel
		{
			get
			{
				return Debug.IndentLevel;
			}
			set
			{
				Debug.IndentLevel = value;
			}
		}

		public static int IndentSize
		{
			get
			{
				return Debug.IndentSize;
			}
			set
			{
				Debug.IndentSize = value;
			}
		}

		public static TraceListenerCollection Listeners => Trace.Listeners;

		[Conditional("TRACE")]
		public static void InitializeUnhandledExceptionHandler()
		{
			AppDomain.CurrentDomain.UnhandledException += DbgExceptionHandler;
		}

		public static void DbgExceptionHandler(object sender, UnhandledExceptionEventArgs args)
		{
			Exception ex = (Exception)args.ExceptionObject;
			Trace.WriteLine("Exception: " + ex.Message);
			MessageBox.Show("A fatal problem has occurred.\n" + ex.Message, "Program Stopped", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
			Trace.Close();
			Process.GetCurrentProcess().Kill();
		}

		[Conditional("TRACE")]
		public static void Warn(bool b, DbgKey key)
		{
			Trace.WriteLine("Warning: " + key.Name);
			if (problems.Contains(key))
			{
				string explanation = GetExplanation(key);
				MessageBox.Show(explanation, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
			}
			else
			{
				MessageBox.Show("A problem has occurred that should be corrected.\n\nReference: " + key.Name, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
			}
		}

		[Conditional("TRACE")]
		public static void Assert(bool b, DbgKey key)
		{
			Trace.WriteLine("Assert: " + key.Name);
			if (problems.Contains(key))
			{
				string explanation = GetExplanation(key);
				MessageBox.Show(explanation, "Program Stopped", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
			}
			else
			{
				MessageBox.Show("A fatal problem has occurred.\n\nReference: " + key.Name, "Program Stopped", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
			}
			Trace.Close();
			Process.GetCurrentProcess().Kill();
		}

		[Conditional("DEBUG")]
		public static void LogOutput(string fn)
		{
			Debug.Listeners.Add(new TextWriterTraceListener(fn));
			DateTime now = DateTime.Now;
		}

		[Conditional("DEBUG")]
		public static void Break()
		{
			Debugger.Break();
		}

		private static string GetExplanation(DbgKey key)
		{
			ProblemReason problemReason = problems[key];
			string text = problemReason.GetProblem() + "\n\nPossible reasons:\n\n";
			int num = 1;
			string[] reasons = problemReason.GetReasons();
			foreach (string text2 in reasons)
			{
				string text3 = text;
				text = text3 + "  " + num.ToString() + ". " + text2 + "\n";
				num++;
			}
			return text;
		}

		[Conditional("TRACE")]
		public static void Assert(bool b)
		{
			Trace.Assert(b);
		}

		[Conditional("TRACE")]
		public static void Assert(bool b, string s)
		{
			Trace.Assert(b, s);
		}

		[Conditional("TRACE")]
		public static void Assert(bool b, string s1, string s2)
		{
			Trace.Assert(b, s1, s2);
		}

		[Conditional("TRACE")]
		public static void Fail(string s)
		{
			Trace.Fail(s);
		}

		[Conditional("TRACE")]
		public static void Fail(string s1, string s2)
		{
			Trace.Fail(s1, s2);
		}

		[Conditional("DEBUG")]
		public static void Close()
		{
			Trace.Close();
		}

		[Conditional("DEBUG")]
		public static void Flush()
		{
		}

		[Conditional("DEBUG")]
		public static void Indent()
		{
		}

		[Conditional("DEBUG")]
		public static void Unindent()
		{
		}

		[Conditional("DEBUG")]
		public static void Write(object obj)
		{
		}

		[Conditional("DEBUG")]
		public static void Write(string s)
		{
		}

		[Conditional("DEBUG")]
		public static void Write(object obj, string s)
		{
		}

		[Conditional("DEBUG")]
		public static void Write(string s1, string s2)
		{
		}

		[Conditional("DEBUG")]
		public static void WriteIf(bool b, object obj)
		{
		}

		[Conditional("DEBUG")]
		public static void WriteIf(bool b, string s)
		{
		}

		[Conditional("DEBUG")]
		public static void WriteIf(bool b, object obj, string s)
		{
		}

		[Conditional("DEBUG")]
		public static void WriteIf(bool b, string s1, string s2)
		{
		}

		[Conditional("DEBUG")]
		public static void WriteLine(object obj)
		{
		}

		[Conditional("DEBUG")]
		public static void WriteLine(string s)
		{
		}

		[Conditional("DEBUG")]
		public static void WriteLine(object obj, string s)
		{
		}

		[Conditional("DEBUG")]
		public static void WriteLine(string s1, string s2)
		{
		}

		[Conditional("DEBUG")]
		public static void WriteLineIf(bool b, object obj)
		{
		}

		[Conditional("DEBUG")]
		public static void WriteLineIf(bool b, string s)
		{
		}

		[Conditional("DEBUG")]
		public static void WriteLineIf(bool b, object obj, string s)
		{
		}

		[Conditional("DEBUG")]
		public static void WriteLineIf(bool b, string s1, string s2)
		{
		}
	}
}
