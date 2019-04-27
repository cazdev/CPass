namespace CMessage
{
    partial class frmCMessage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCMessage));
            this.mainProgress = new Bunifu.Framework.UI.BunifuProgressBar();
            this.lblTitle = new Bunifu.Framework.UI.BunifuCustomLabel();
            this.tmrMainProgress = new System.Windows.Forms.Timer(this.components);
            this.lblVersion = new Bunifu.Framework.UI.BunifuCustomLabel();
            this.SuspendLayout();
            // 
            // mainProgress
            // 
            this.mainProgress.BackColor = System.Drawing.Color.Silver;
            this.mainProgress.BorderRadius = 5;
            this.mainProgress.Location = new System.Drawing.Point(252, 304);
            this.mainProgress.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.mainProgress.MaximumValue = 100;
            this.mainProgress.Name = "mainProgress";
            this.mainProgress.ProgressColor = System.Drawing.Color.Teal;
            this.mainProgress.Size = new System.Drawing.Size(547, 14);
            this.mainProgress.TabIndex = 0;
            this.mainProgress.Value = 0;
            // 
            // lblTitle
            // 
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI Light", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(252, 210);
            this.lblTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(547, 62);
            this.lblTitle.TabIndex = 1;
            this.lblTitle.Text = "CPass";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tmrMainProgress
            // 
            this.tmrMainProgress.Enabled = true;
            this.tmrMainProgress.Interval = 15;
            this.tmrMainProgress.Tick += new System.EventHandler(this.tmrMainProgress_Tick);
            // 
            // lblVersion
            // 
            this.lblVersion.AutoSize = true;
            this.lblVersion.Font = new System.Drawing.Font("Segoe UI Semilight", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVersion.ForeColor = System.Drawing.Color.White;
            this.lblVersion.Location = new System.Drawing.Point(587, 235);
            this.lblVersion.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(53, 32);
            this.lblVersion.TabIndex = 2;
            this.lblVersion.Text = "v1.0";
            // 
            // frmCMessage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(10)))), ((int)(((byte)(10)))), ((int)(((byte)(10)))));
            this.ClientSize = new System.Drawing.Size(1067, 554);
            this.Controls.Add(this.lblVersion);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.mainProgress);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximumSize = new System.Drawing.Size(1067, 554);
            this.MinimumSize = new System.Drawing.Size(1067, 554);
            this.Name = "frmCMessage";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CPass";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Bunifu.Framework.UI.BunifuProgressBar mainProgress;
        private Bunifu.Framework.UI.BunifuCustomLabel lblTitle;
        private System.Windows.Forms.Timer tmrMainProgress;
        private Bunifu.Framework.UI.BunifuCustomLabel lblVersion;
    }
}

