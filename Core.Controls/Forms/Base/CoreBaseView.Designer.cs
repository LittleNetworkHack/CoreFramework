namespace Core.Controls
{
	partial class CoreBaseView
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
			this.ctrlKeys = new Core.Controls.CoreFnPanel();
			this.SuspendLayout();
			// 
			// ctrlKeys
			// 
			this.ctrlKeys.Dock = System.Windows.Forms.DockStyle.Top;
			this.ctrlKeys.Location = new System.Drawing.Point(0, 0);
			this.ctrlKeys.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
			this.ctrlKeys.Name = "ctrlKeys";
			this.ctrlKeys.Size = new System.Drawing.Size(800, 100);
			this.ctrlKeys.TabIndex = 0;
			// 
			// CoreBaseView
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(800, 450);
			this.Controls.Add(this.ctrlKeys);
			this.Name = "CoreBaseView";
			this.Text = "CoreBaseView";
			this.ResumeLayout(false);

		}

		#endregion

		protected CoreFnPanel ctrlKeys;
	}
}