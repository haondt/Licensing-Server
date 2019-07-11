namespace WindowsFormApp
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
			this.HWIDLabel = new System.Windows.Forms.Label();
			this.HWIDTextBox = new System.Windows.Forms.TextBox();
			this.HWIDButton = new System.Windows.Forms.Button();
			this.LicenseLabel = new System.Windows.Forms.Label();
			this.LicenseKeyTextBox = new System.Windows.Forms.TextBox();
			this.LicenseKeyButton = new System.Windows.Forms.Button();
			this.SignatureVerificationLabel = new System.Windows.Forms.Label();
			this.HWIDVerificationLabel = new System.Windows.Forms.Label();
			this.LoadLicenseFileButton = new System.Windows.Forms.Button();
			this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			this.ExpiryLabel = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// HWIDLabel
			// 
			this.HWIDLabel.AutoSize = true;
			this.HWIDLabel.Location = new System.Drawing.Point(146, 94);
			this.HWIDLabel.Name = "HWIDLabel";
			this.HWIDLabel.Size = new System.Drawing.Size(40, 13);
			this.HWIDLabel.TabIndex = 0;
			this.HWIDLabel.Text = "HWID:";
			this.HWIDLabel.Click += new System.EventHandler(this.Label1_Click);
			// 
			// HWIDTextBox
			// 
			this.HWIDTextBox.Enabled = false;
			this.HWIDTextBox.Location = new System.Drawing.Point(187, 91);
			this.HWIDTextBox.Name = "HWIDTextBox";
			this.HWIDTextBox.Size = new System.Drawing.Size(380, 20);
			this.HWIDTextBox.TabIndex = 1;
			// 
			// HWIDButton
			// 
			this.HWIDButton.Location = new System.Drawing.Point(573, 89);
			this.HWIDButton.Name = "HWIDButton";
			this.HWIDButton.Size = new System.Drawing.Size(120, 23);
			this.HWIDButton.TabIndex = 2;
			this.HWIDButton.Text = "Get HWID";
			this.HWIDButton.UseVisualStyleBackColor = true;
			this.HWIDButton.Click += new System.EventHandler(this.HWIDButton_Click);
			// 
			// LicenseLabel
			// 
			this.LicenseLabel.AutoSize = true;
			this.LicenseLabel.Location = new System.Drawing.Point(118, 120);
			this.LicenseLabel.Name = "LicenseLabel";
			this.LicenseLabel.Size = new System.Drawing.Size(68, 13);
			this.LicenseLabel.TabIndex = 0;
			this.LicenseLabel.Text = "License Key:";
			this.LicenseLabel.Click += new System.EventHandler(this.Label1_Click);
			// 
			// LicenseKeyTextBox
			// 
			this.LicenseKeyTextBox.Enabled = false;
			this.LicenseKeyTextBox.Location = new System.Drawing.Point(187, 117);
			this.LicenseKeyTextBox.Name = "LicenseKeyTextBox";
			this.LicenseKeyTextBox.Size = new System.Drawing.Size(380, 20);
			this.LicenseKeyTextBox.TabIndex = 1;
			// 
			// LicenseKeyButton
			// 
			this.LicenseKeyButton.Location = new System.Drawing.Point(306, 143);
			this.LicenseKeyButton.Name = "LicenseKeyButton";
			this.LicenseKeyButton.Size = new System.Drawing.Size(120, 23);
			this.LicenseKeyButton.TabIndex = 2;
			this.LicenseKeyButton.Text = "Verify License Key";
			this.LicenseKeyButton.UseVisualStyleBackColor = true;
			this.LicenseKeyButton.Click += new System.EventHandler(this.LicenseKeyButton_Click);
			// 
			// SignatureVerificationLabel
			// 
			this.SignatureVerificationLabel.AutoSize = true;
			this.SignatureVerificationLabel.Location = new System.Drawing.Point(303, 171);
			this.SignatureVerificationLabel.Name = "SignatureVerificationLabel";
			this.SignatureVerificationLabel.Size = new System.Drawing.Size(35, 13);
			this.SignatureVerificationLabel.TabIndex = 3;
			this.SignatureVerificationLabel.Text = "label1";
			// 
			// HWIDVerificationLabel
			// 
			this.HWIDVerificationLabel.AutoSize = true;
			this.HWIDVerificationLabel.Location = new System.Drawing.Point(303, 193);
			this.HWIDVerificationLabel.Name = "HWIDVerificationLabel";
			this.HWIDVerificationLabel.Size = new System.Drawing.Size(35, 13);
			this.HWIDVerificationLabel.TabIndex = 3;
			this.HWIDVerificationLabel.Text = "label1";
			// 
			// LoadLicenseFileButton
			// 
			this.LoadLicenseFileButton.Location = new System.Drawing.Point(573, 117);
			this.LoadLicenseFileButton.Name = "LoadLicenseFileButton";
			this.LoadLicenseFileButton.Size = new System.Drawing.Size(120, 23);
			this.LoadLicenseFileButton.TabIndex = 2;
			this.LoadLicenseFileButton.Text = "Load License File";
			this.LoadLicenseFileButton.UseVisualStyleBackColor = true;
			this.LoadLicenseFileButton.Click += new System.EventHandler(this.LoadLicenseFileButton_Click);
			// 
			// openFileDialog1
			// 
			this.openFileDialog1.FileName = "openFileDialog1";
			// 
			// ExpiryLabel
			// 
			this.ExpiryLabel.AutoSize = true;
			this.ExpiryLabel.Location = new System.Drawing.Point(303, 217);
			this.ExpiryLabel.Name = "ExpiryLabel";
			this.ExpiryLabel.Size = new System.Drawing.Size(35, 13);
			this.ExpiryLabel.TabIndex = 3;
			this.ExpiryLabel.Text = "label1";
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(822, 351);
			this.Controls.Add(this.ExpiryLabel);
			this.Controls.Add(this.HWIDVerificationLabel);
			this.Controls.Add(this.SignatureVerificationLabel);
			this.Controls.Add(this.LicenseKeyButton);
			this.Controls.Add(this.LoadLicenseFileButton);
			this.Controls.Add(this.HWIDButton);
			this.Controls.Add(this.LicenseKeyTextBox);
			this.Controls.Add(this.LicenseLabel);
			this.Controls.Add(this.HWIDTextBox);
			this.Controls.Add(this.HWIDLabel);
			this.Name = "Form1";
			this.Text = "Form1";
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label HWIDLabel;
        private System.Windows.Forms.TextBox HWIDTextBox;
        private System.Windows.Forms.Button HWIDButton;
        private System.Windows.Forms.Label LicenseLabel;
        private System.Windows.Forms.TextBox LicenseKeyTextBox;
        private System.Windows.Forms.Button LicenseKeyButton;
        private System.Windows.Forms.Label SignatureVerificationLabel;
        private System.Windows.Forms.Label HWIDVerificationLabel;
        private System.Windows.Forms.Button LoadLicenseFileButton;
		private System.Windows.Forms.OpenFileDialog openFileDialog1;
		private System.Windows.Forms.Label ExpiryLabel;
	}
}

