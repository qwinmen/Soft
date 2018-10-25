namespace Monolit
{
	partial class Form1
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
			this.statusBottomBar = new System.Windows.Forms.StatusStrip();
			this.toolStripStatusLabel_OpenedFileName = new System.Windows.Forms.ToolStripStatusLabel();
			this.toolStripStatusLabel_Главы = new System.Windows.Forms.ToolStripStatusLabel();
			this.menuTopBar = new System.Windows.Forms.MenuStrip();
			this.файлToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.открытьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.richTextBox_Document = new System.Windows.Forms.RichTextBox();
			this.tamitaRunToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.statusBottomBar.SuspendLayout();
			this.menuTopBar.SuspendLayout();
			this.SuspendLayout();
			// 
			// statusBottomBar
			// 
			this.statusBottomBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel_OpenedFileName,
            this.toolStripStatusLabel_Главы});
			this.statusBottomBar.Location = new System.Drawing.Point(0, 366);
			this.statusBottomBar.Name = "statusBottomBar";
			this.statusBottomBar.Size = new System.Drawing.Size(535, 22);
			this.statusBottomBar.TabIndex = 0;
			this.statusBottomBar.Text = "statusStrip1";
			// 
			// toolStripStatusLabel_OpenedFileName
			// 
			this.toolStripStatusLabel_OpenedFileName.Name = "toolStripStatusLabel_OpenedFileName";
			this.toolStripStatusLabel_OpenedFileName.Size = new System.Drawing.Size(52, 17);
			this.toolStripStatusLabel_OpenedFileName.Text = "Файл: --";
			// 
			// toolStripStatusLabel_Главы
			// 
			this.toolStripStatusLabel_Главы.Name = "toolStripStatusLabel_Главы";
			this.toolStripStatusLabel_Главы.Size = new System.Drawing.Size(57, 17);
			this.toolStripStatusLabel_Главы.Text = "Главы: --";
			// 
			// menuTopBar
			// 
			this.menuTopBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.файлToolStripMenuItem});
			this.menuTopBar.Location = new System.Drawing.Point(0, 0);
			this.menuTopBar.Name = "menuTopBar";
			this.menuTopBar.Size = new System.Drawing.Size(535, 24);
			this.menuTopBar.TabIndex = 1;
			this.menuTopBar.Text = "menuStrip1";
			// 
			// файлToolStripMenuItem
			// 
			this.файлToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.открытьToolStripMenuItem,
            this.tamitaRunToolStripMenuItem});
			this.файлToolStripMenuItem.Name = "файлToolStripMenuItem";
			this.файлToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
			this.файлToolStripMenuItem.Text = "Файл";
			// 
			// открытьToolStripMenuItem
			// 
			this.открытьToolStripMenuItem.Name = "открытьToolStripMenuItem";
			this.открытьToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
			this.открытьToolStripMenuItem.Text = "Открыть";
			this.открытьToolStripMenuItem.ToolTipText = "Загрузить docx файл";
			this.открытьToolStripMenuItem.Click += new System.EventHandler(this.ОткрытьToolStripMenuItem_Click);
			// 
			// richTextBox_Document
			// 
			this.richTextBox_Document.Dock = System.Windows.Forms.DockStyle.Fill;
			this.richTextBox_Document.Location = new System.Drawing.Point(0, 24);
			this.richTextBox_Document.Name = "richTextBox_Document";
			this.richTextBox_Document.Size = new System.Drawing.Size(535, 342);
			this.richTextBox_Document.TabIndex = 2;
			this.richTextBox_Document.Text = "";
			// 
			// tamitaRunToolStripMenuItem
			// 
			this.tamitaRunToolStripMenuItem.Name = "tamitaRunToolStripMenuItem";
			this.tamitaRunToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
			this.tamitaRunToolStripMenuItem.Text = "Tamita run";
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(535, 388);
			this.Controls.Add(this.richTextBox_Document);
			this.Controls.Add(this.statusBottomBar);
			this.Controls.Add(this.menuTopBar);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MainMenuStrip = this.menuTopBar;
			this.Name = "Form1";
			this.Text = "Monolit Soft";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
			this.Load += new System.EventHandler(this.Form1_Load);
			this.statusBottomBar.ResumeLayout(false);
			this.statusBottomBar.PerformLayout();
			this.menuTopBar.ResumeLayout(false);
			this.menuTopBar.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.StatusStrip statusBottomBar;
		private System.Windows.Forms.MenuStrip menuTopBar;
		private System.Windows.Forms.ToolStripMenuItem файлToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem открытьToolStripMenuItem;
		private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel_OpenedFileName;
		private System.Windows.Forms.RichTextBox richTextBox_Document;
		private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel_Главы;
		private System.Windows.Forms.ToolStripMenuItem tamitaRunToolStripMenuItem;
	}
}

