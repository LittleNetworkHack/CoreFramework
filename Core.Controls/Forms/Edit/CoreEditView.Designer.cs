namespace Core.Controls
{
	partial class CoreEditView
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
			this.coreSource = new System.Windows.Forms.BindingSource(this.components);
			this.coreEditManager = new Core.Controls.Components.CoreEditManager(this.components);
			((System.ComponentModel.ISupportInitialize)(this.coreSource)).BeginInit();
			this.SuspendLayout();
			this.coreEditManager.SetOnDelete(this.ctrlKeys, Core.Controls.Components.EditControlBehavior.Default);
			this.coreEditManager.SetOnInsert(this.ctrlKeys, Core.Controls.Components.EditControlBehavior.Default);
			this.coreEditManager.SetOnUpdate(this.ctrlKeys, Core.Controls.Components.EditControlBehavior.Default);
			this.ctrlKeys.Size = new System.Drawing.Size(434, 40);
			// 
			// CoreEditView
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(434, 361);
			this.Name = "CoreEditView";
			this.coreEditManager.SetOnDelete(this, Core.Controls.Components.EditControlBehavior.Default);
			this.coreEditManager.SetOnInsert(this, Core.Controls.Components.EditControlBehavior.Default);
			this.coreEditManager.SetOnUpdate(this, Core.Controls.Components.EditControlBehavior.Default);
			this.Text = "CoreEditView";
			((System.ComponentModel.ISupportInitialize)(this.coreSource)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		protected System.Windows.Forms.BindingSource coreSource;
		protected Components.CoreEditManager coreEditManager;
	}
}