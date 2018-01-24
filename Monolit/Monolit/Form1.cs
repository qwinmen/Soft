using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
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

		private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var openFileDialog = new OpenFileDialog
			{
				Filter = @"Файлы word|*.docx;*.doc"
			};

			if (openFileDialog.ShowDialog() != DialogResult.OK)
				return;
			word.Application app = new word.Application();
			try
			{
				_document = app.Documents.Open(openFileDialog.FileName);
			}
			catch (Exception ex)
			{
				MessageBox.Show(@"Ошибка при открытии документа. " + ex.Message);
				toolStripStatusLabel_OpenedFileName.Text = @"Файл: -error-";
				return;
			}
			toolStripStatusLabel_OpenedFileName.Text = @"Файл: " + _document.Name;
			_document.Close();
			app.Quit();
		}
	}
}
