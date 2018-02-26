using System;
using System.Windows.Forms;
using Monolit.Interfaces.Contracts;
using Monolit.OfficeWord;
using Monolit.Text;
using word = Microsoft.Office.Interop.Word;

namespace Monolit
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		private word.Document _document;
		private word.Application _application;

		private void ОткрытьToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var openFileDialog = new OpenFileDialog
			{
				Filter = @"Файлы word|*.docx;*.doc"
			};

			if (openFileDialog.ShowDialog() != DialogResult.OK)
				return;

			Form1_FormClosing(sender, null);
			_application = new word.Application();
			try
			{
				_document = WordHelpers.IsWordFileOpened() 
					? _application.Documents.OpenOld(openFileDialog.FileName, ReadOnly: false)
					: _application.Documents.Open(openFileDialog.FileName, ReadOnly: false, Visible: false);
				toolStripStatusLabel_OpenedFileName.Text = @"Файл: " + _document.Name;
				OpenDocument(_application);
			}
			catch (Exception ex)
			{
				MessageBox.Show(@"Ошибка при открытии документа. " + ex.Message);
				toolStripStatusLabel_OpenedFileName.Text = @"Файл: -error-";
			}
		}

		private void OpenDocument(word.Application application)
		{
			word.Selection selection = application.Selection;
			if (selection == null)
			{
				MessageBox.Show(@"Selection is null");
				return;
			}

			switch (selection.Type)
			{
				case word.WdSelectionType.wdSelectionNormal:
				case word.WdSelectionType.wdSelectionIP:
				{
					var range = _document.Range();
					range.Select();
						richTextBox_Document.Text = range.Text;
					TextParseManager.FullWordText = range.Text;
					toolStripStatusLabel_Главы.Text = @"Главы: " + WordHelpers.FindChapters(_document);
					var array = WordHelpers.SplitByChapters(TextParseManager.FullWordText);
						break;
				}
				
				default:
					MessageBox.Show(@"Неподдерживаемый тип Selection");
					break;
			}

			_document.RemoveDocumentInformation(word.WdRemoveDocInfoType.wdRDIAll);
			application.Documents.Save(NoPrompt: true, OriginalFormat: true);
		}

		private void Form1_FormClosing(object sender, FormClosingEventArgs e)
		{
			if(_application == null || _document == null)
				return;

			WordHelpers.DisposeWordInstance(_application, _document);
			try
			{
				_document.Close(SaveChanges: false, OriginalFormat: false, RouteDocument: false);
				_application.Quit(SaveChanges: false, OriginalFormat: false, RouteDocument: false);
			}
			catch (Exception)
			{}
			System.Runtime.InteropServices.Marshal.ReleaseComObject(_application);
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			Form1_FormClosing(sender, null);
		}
	}
}
