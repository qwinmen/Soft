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
			this.menuTopBar = new System.Windows.Forms.MenuStrip();
			this.файлToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.открытьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripStatusLabel_OpenedFileName = new System.Windows.Forms.ToolStripStatusLabel();
			this.statusBottomBar.SuspendLayout();
			this.menuTopBar.SuspendLayout();
			this.SuspendLayout();
			// 
			// statusBottomBar
			// 
			this.statusBottomBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel_OpenedFileName});
			this.statusBottomBar.Location = new System.Drawing.Point(0, 366);
			this.statusBottomBar.Name = "statusBottomBar";
			this.statusBottomBar.Size = new System.Drawing.Size(535, 22);
			this.statusBottomBar.TabIndex = 0;
			this.statusBottomBar.Text = "statusStrip1";
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
            this.открытьToolStripMenuItem});
			this.файлToolStripMenuItem.Name = "файлToolStripMenuItem";
			this.файлToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
			this.файлToolStripMenuItem.Text = "Файл";
			// 
			// открытьToolStripMenuItem
			// 
			this.открытьToolStripMenuItem.Name = "открытьToolStripMenuItem";
			this.открытьToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.открытьToolStripMenuItem.Text = "Открыть";
			this.открытьToolStripMenuItem.ToolTipText = "Загрузить docx файл";
			this.открытьToolStripMenuItem.Click += new System.EventHandler(this.открытьToolStripMenuItem_Click);
			// 
			// toolStripStatusLabel_OpenedFileName
			// 
			this.toolStripStatusLabel_OpenedFileName.Name = "toolStripStatusLabel_OpenedFileName";
			this.toolStripStatusLabel_OpenedFileName.Size = new System.Drawing.Size(52, 17);
			this.toolStripStatusLabel_OpenedFileName.Text = "Файл: --";
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(535, 388);
			this.Controls.Add(this.statusBottomBar);
			this.Controls.Add(this.menuTopBar);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MainMenuStrip = this.menuTopBar;
			this.Name = "Form1";
			this.Text = "Monolit Soft";
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
	}
}

