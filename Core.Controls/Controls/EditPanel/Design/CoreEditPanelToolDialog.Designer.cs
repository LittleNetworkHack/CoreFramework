namespace Core.Controls.Controls.EditPanel.Design
{
	partial class CoreEditPanelToolDialog
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
			this.label1 = new System.Windows.Forms.Label();
			this.textEditBox1 = new Core.Controls.TextEditBox();
			this.button1 = new System.Windows.Forms.Button();
			this.comboEditBox1 = new Core.Controls.ComboEditBox();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 15);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(35, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "label1";
			// 
			// textEditBox1
			// 
			this.textEditBox1.Location = new System.Drawing.Point(75, 79);
			this.textEditBox1.Name = "textEditBox1";
			this.textEditBox1.Size = new System.Drawing.Size(100, 20);
			this.textEditBox1.TabIndex = 1;
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(158, 96);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(75, 23);
			this.button1.TabIndex = 2;
			this.button1.Text = "button1";
			this.button1.UseVisualStyleBackColor = true;
			// 
			// comboEditBox1
			// 
			this.comboEditBox1.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.comboEditBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboEditBox1.FormattingEnabled = true;
			this.comboEditBox1.Location = new System.Drawing.Point(112, 12);
			this.comboEditBox1.Name = "comboEditBox1";
			this.comboEditBox1.Size = new System.Drawing.Size(121, 21);
			this.comboEditBox1.TabIndex = 3;
			// 
			// CoreEditPanelToolDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(353, 172);
			this.Controls.Add(this.comboEditBox1);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.textEditBox1);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "CoreEditPanelToolDialog";
			this.Text = "CoreEditPanelToolDialog";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private TextEditBox textEditBox1;
		private System.Windows.Forms.Button button1;
		private ComboEditBox comboEditBox1;
	}
}