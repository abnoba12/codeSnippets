namespace Sitecore_Log
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
            this.buttonFile = new System.Windows.Forms.Button();
            this.txtBrowse = new System.Windows.Forms.TextBox();
            this.chkAudit = new System.Windows.Forms.CheckBox();
            this.chkError = new System.Windows.Forms.CheckBox();
            this.chkInfo = new System.Windows.Forms.CheckBox();
            this.textDisplay = new System.Windows.Forms.TextBox();
            this.chkWarn = new System.Windows.Forms.CheckBox();
            this.text = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // buttonFile
            // 
            this.buttonFile.Location = new System.Drawing.Point(197, 6);
            this.buttonFile.Name = "buttonFile";
            this.buttonFile.Size = new System.Drawing.Size(75, 23);
            this.buttonFile.TabIndex = 0;
            this.buttonFile.Text = "File";
            this.buttonFile.UseVisualStyleBackColor = true;
            this.buttonFile.Click += new System.EventHandler(this.buttonFile_Click);
            // 
            // txtBrowse
            // 
            this.txtBrowse.Location = new System.Drawing.Point(12, 9);
            this.txtBrowse.Name = "txtBrowse";
            this.txtBrowse.Size = new System.Drawing.Size(179, 20);
            this.txtBrowse.TabIndex = 1;
            // 
            // chkAudit
            // 
            this.chkAudit.AutoSize = true;
            this.chkAudit.Checked = true;
            this.chkAudit.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAudit.Location = new System.Drawing.Point(279, 11);
            this.chkAudit.Name = "chkAudit";
            this.chkAudit.Size = new System.Drawing.Size(50, 17);
            this.chkAudit.TabIndex = 2;
            this.chkAudit.Text = "Audit";
            this.chkAudit.UseVisualStyleBackColor = true;
            this.chkAudit.CheckedChanged += new System.EventHandler(this.chkAudit_CheckedChanged);
            // 
            // chkError
            // 
            this.chkError.AutoSize = true;
            this.chkError.Checked = true;
            this.chkError.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkError.Location = new System.Drawing.Point(335, 12);
            this.chkError.Name = "chkError";
            this.chkError.Size = new System.Drawing.Size(48, 17);
            this.chkError.TabIndex = 3;
            this.chkError.Text = "Error";
            this.chkError.UseVisualStyleBackColor = true;
            this.chkError.CheckedChanged += new System.EventHandler(this.chkError_CheckedChanged);
            // 
            // chkInfo
            // 
            this.chkInfo.AutoSize = true;
            this.chkInfo.Checked = true;
            this.chkInfo.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkInfo.Location = new System.Drawing.Point(390, 13);
            this.chkInfo.Name = "chkInfo";
            this.chkInfo.Size = new System.Drawing.Size(44, 17);
            this.chkInfo.TabIndex = 4;
            this.chkInfo.Text = "Info";
            this.chkInfo.UseVisualStyleBackColor = true;
            this.chkInfo.CheckedChanged += new System.EventHandler(this.chkInfo_CheckedChanged);
            // 
            // textDisplay
            // 
            this.textDisplay.Location = new System.Drawing.Point(12, 36);
            this.textDisplay.MaxLength = 0;
            this.textDisplay.Multiline = true;
            this.textDisplay.Name = "textDisplay";
            this.textDisplay.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textDisplay.Size = new System.Drawing.Size(764, 424);
            this.textDisplay.TabIndex = 5;
            // 
            // chkWarn
            // 
            this.chkWarn.AutoSize = true;
            this.chkWarn.Checked = true;
            this.chkWarn.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkWarn.Location = new System.Drawing.Point(441, 11);
            this.chkWarn.Name = "chkWarn";
            this.chkWarn.Size = new System.Drawing.Size(52, 17);
            this.chkWarn.TabIndex = 6;
            this.chkWarn.Text = "Warn";
            this.chkWarn.UseVisualStyleBackColor = true;
            this.chkWarn.CheckedChanged += new System.EventHandler(this.chkWarn_CheckedChanged);
            // 
            // text
            // 
            this.text.AutoSize = true;
            this.text.Checked = true;
            this.text.CheckState = System.Windows.Forms.CheckState.Checked;
            this.text.Location = new System.Drawing.Point(499, 11);
            this.text.Name = "text";
            this.text.Size = new System.Drawing.Size(47, 17);
            this.text.TabIndex = 7;
            this.text.Text = "Text";
            this.text.UseVisualStyleBackColor = true;
            this.text.CheckedChanged += new System.EventHandler(this.text_CheckedChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(788, 472);
            this.Controls.Add(this.text);
            this.Controls.Add(this.chkWarn);
            this.Controls.Add(this.textDisplay);
            this.Controls.Add(this.chkInfo);
            this.Controls.Add(this.chkError);
            this.Controls.Add(this.chkAudit);
            this.Controls.Add(this.txtBrowse);
            this.Controls.Add(this.buttonFile);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonFile;
        private System.Windows.Forms.TextBox txtBrowse;
        private System.Windows.Forms.CheckBox chkAudit;
        private System.Windows.Forms.CheckBox chkError;
        private System.Windows.Forms.CheckBox chkInfo;
        private System.Windows.Forms.TextBox textDisplay;
        private System.Windows.Forms.CheckBox chkWarn;
        private System.Windows.Forms.CheckBox text;


    }
}

