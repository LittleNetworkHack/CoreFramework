namespace CoreFramework
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
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.textBox2 = new System.Windows.Forms.TextBox();
			this.baseTextBox1 = new Core.Controls.BaseTextBox();
			this.SuspendLayout();
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(121, 119);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(75, 20);
			this.textBox1.TabIndex = 1;
			this.textBox1.Text = "baseTextBox1";
			// 
			// textBox2
			// 
			this.textBox2.Location = new System.Drawing.Point(202, 93);
			this.textBox2.Name = "textBox2";
			this.textBox2.Size = new System.Drawing.Size(75, 20);
			this.textBox2.TabIndex = 2;
			this.textBox2.Text = "baseTextBox1";
			// 
			// baseTextBox1
			// 
			this.baseTextBox1.Location = new System.Drawing.Point(121, 93);
			this.baseTextBox1.Margin = new System.Windows.Forms.Padding(2);
			this.baseTextBox1.Name = "baseTextBox1";
			this.baseTextBox1.Padding = new System.Windows.Forms.Padding(2);
			this.baseTextBox1.Size = new System.Drawing.Size(75, 20);
			this.baseTextBox1.TabIndex = 0;
			this.baseTextBox1.Text = "baseTextBox1";
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(800, 450);
			this.Controls.Add(this.textBox2);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.baseTextBox1);
			this.Name = "Form1";
			this.Text = "Form1";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private Core.Controls.BaseTextBox baseTextBox1;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.TextBox textBox2;
	}
}

