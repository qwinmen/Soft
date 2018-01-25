using System;
using System.Collections.Generic;
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
			catch (Exception e)
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

		public static string[] SplitByChapters(word.Document document, string findText = "Глава ")
		{
			var chapters = new List<string>();
			Dictionary<string, string> dictionaryChapters = new Dictionary<string, string>();
			{
				var allBookText = document.Content.Text;
				var predlojenie = document.Sentences[10];   //массив предложений
				bool findFlag = false;
				int chapterKey = 0;

				for (var i = 1; i < document.Sentences.Count; i++)
				{
					var abzac = document.Sentences[i];
					var rng = abzac;
					rng.Find.Text = findText;
					rng.Find.Forward = true;
					if (rng.Find.Execute(MatchCase: false))
					{
						findFlag = true;
						chapterKey++;
						dictionaryChapters.Add($"Глава {chapterKey}", rng.Text);
						if (chapters.Count != 0)
							chapters.Clear();
					}

					if(findFlag)
						chapters.Add(rng.Text);
				}

				
				
			}

			return chapters.ToArray();
		}
	}
}
