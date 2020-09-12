namespace Core.Controls
{
	partial class CoreGridView
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
			this.components = new System.ComponentModel.Container();
			this.ctrlGrid = new System.Windows.Forms.DataGridView();
			this.ctrlPages = new Core.Controls.CoreTabControl();
			this.ctrlFilters = new System.Windows.Forms.TabPage();
			this.coreSource = new System.Windows.Forms.BindingSource(this.components);
			((System.ComponentModel.ISupportInitialize)(this.ctrlGrid)).BeginInit();
			this.ctrlPages.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.coreSource)).BeginInit();
			this.SuspendLayout();
			// 
			// ctrlGrid
			// 
			this.ctrlGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.ctrlGrid.Location = new System.Drawing.Point(12, 158);
			this.ctrlGrid.Name = "ctrlGrid";
			this.ctrlGrid.Size = new System.Drawing.Size(776, 280);
			this.ctrlGrid.TabIndex = 2;
			// 
			// ctrlPages
			// 
			this.ctrlPages.Alignment = System.Windows.Forms.TabAlignment.Bottom;
			this.ctrlPages.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.ctrlPages.Controls.Add(this.ctrlFilters);
			this.ctrlPages.ItemSize = new System.Drawing.Size(0, 1);
			this.ctrlPages.Location = new System.Drawing.Point(12, 43);
			this.ctrlPages.Multiline = true;
			this.ctrlPages.Name = "ctrlPages";
			this.ctrlPages.SelectedIndex = 0;
			this.ctrlPages.Size = new System.Drawing.Size(776, 109);
			this.ctrlPages.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
			this.ctrlPages.TabIndex = 3;
			// 
			// ctrlFilters
			// 
			this.ctrlFilters.Location = new System.Drawing.Point(4, 4);
			this.ctrlFilters.Name = "ctrlFilters";
			this.ctrlFilters.Padding = new System.Windows.Forms.Padding(3);
			this.ctrlFilters.Size = new System.Drawing.Size(768, 100);
			this.ctrlFilters.TabIndex = 0;
			this.ctrlFilters.Text = "Filteri";
			this.ctrlFilters.UseVisualStyleBackColor = true;
			// 
			// CoreGridView
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(800, 450);
			this.Controls.Add(this.ctrlGrid);
			this.Controls.Add(this.ctrlPages);
			this.Name = "CoreGridView";
			this.Text = "CoreGridView";
			this.Controls.SetChildIndex(this.ctrlKeys, 0);
			this.Controls.SetChildIndex(this.ctrlPages, 0);
			this.Controls.SetChildIndex(this.ctrlGrid, 0);
			((System.ComponentModel.ISupportInitialize)(this.ctrlGrid)).EndInit();
			this.ctrlPages.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.coreSource)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		protected System.Windows.Forms.DataGridView ctrlGrid;
		protected CoreTabControl ctrlPages;
		protected System.Windows.Forms.TabPage ctrlFilters;
		protected System.Windows.Forms.BindingSource coreSource;
	}
}