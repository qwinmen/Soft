using System;
using System.Collections.Generic;
using System.Linq;
using word = Microsoft.Office.Interop.Word;

namespace Monolit.OfficeWord
{
	public class WordHelpers
	{
		public static bool IsWordFileOpened()
		{
			System.Diagnostics.Process[] processes = System.Diagnostics.Process.GetProcessesByName("WINWORD");
			if (processes != null)
			{
				if (processes.Length > 0)
					return true;
				else
					return false;
			}
			else
				return false;
		}

		public static void DisposeWordInstance(word.Application application, word.Document document)
		{
			try
			{
				document.Close(null, null, null);
				application.Quit();
			}
			catch (Exception)
			{
			}
			System.Runtime.InteropServices.Marshal.ReleaseComObject(document);
			System.Runtime.InteropServices.Marshal.ReleaseComObject(application);
			GC.Collect(); // force final cleanup!
		}

		public static int FindChapters(word.Document document, string findText = "Глава ")
		{
			word.Range rng = document.Range();
			rng.Find.ClearFormatting();//очистит предыдущие условия поиска
			rng.Find.Forward = true;
			rng.Find.Text = findText;

			rng.Find.Execute();
			var countChapters = 0;
			while (rng.Find.Found)
			{
				countChapters++;
				rng.Find.Execute(MatchCase: false);
				
				if (countChapters > 1000)
					break;
			}
			return countChapters;
		}

		public static string[] SplitByChapters(string document, string findText = "Глава ")
		{
			Dictionary<string, string> dictionaryChapters = new Dictionary<string, string>();
			{
				//глава третья исходный свет в тонелях материка глава четвертая корпорация наносит визит
				string[] splitterArray = document.Split(new[] { findText }, StringSplitOptions.RemoveEmptyEntries);
				for (var i = 0; i < splitterArray.Length; i++)
				{
					dictionaryChapters.Add($"Глава {i}", splitterArray[i]);
				}

			}

			return dictionaryChapters.Keys.ToArray();
		}
	}
}
