namespace RetrostrikeDSKUI.Forms.ReplaceWindows
{
    partial class WindowReplace
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
            groupBox_Details = new GroupBox();
            buttonSelectFileToReplaceWith = new Button();
            label1 = new Label();
            this.textboxFileToReplaceWith = new TextBox();
            label2 = new Label();
            this.comboboxFileToReplaceType = new ComboBox();
            this.textboxFileToReplace = new TextBox();
            linkLabel1 = new LinkLabel();
            this.checkboxProcessKnownType = new CheckBox();
            this.comboboxFileToReplaceWithAssetType = new ComboBox();
            label_Description_ImportFileType = new Label();
            label_Description_ImportFileName = new Label();
            groupBox_Details.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox_Details
            // 
            groupBox_Details.Controls.Add(buttonSelectFileToReplaceWith);
            groupBox_Details.Controls.Add(label1);
            groupBox_Details.Controls.Add(this.textboxFileToReplaceWith);
            groupBox_Details.Controls.Add(label2);
            groupBox_Details.Controls.Add(this.comboboxFileToReplaceType);
            groupBox_Details.Controls.Add(this.textboxFileToReplace);
            groupBox_Details.Controls.Add(linkLabel1);
            groupBox_Details.Controls.Add(this.checkboxProcessKnownType);
            groupBox_Details.Controls.Add(this.comboboxFileToReplaceWithAssetType);
            groupBox_Details.Controls.Add(label_Description_ImportFileType);
            groupBox_Details.Controls.Add(label_Description_ImportFileName);
            groupBox_Details.Font = new Font("Segoe UI", 15F);
            groupBox_Details.Location = new Point(6, 27);
            groupBox_Details.Name = "groupBox_Details";
            groupBox_Details.Size = new Size(333, 202);
            groupBox_Details.TabIndex = 4;
            groupBox_Details.TabStop = false;
            groupBox_Details.Text = "Replacement Details";
            // 
            // buttonSelectFileToReplaceWith
            // 
            buttonSelectFileToReplaceWith.Font = new Font("Segoe UI", 9F);
            buttonSelectFileToReplaceWith.Location = new Point(236, 104);
            buttonSelectFileToReplaceWith.Name = "buttonSelectFileToReplaceWith";
            buttonSelectFileToReplaceWith.Size = new Size(91, 24);
            buttonSelectFileToReplaceWith.TabIndex = 13;
            buttonSelectFileToReplaceWith.Text = "Choose File...";
            buttonSelectFileToReplaceWith.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 9F);
            label1.Location = new Point(6, 86);
            label1.Name = "label1";
            label1.Size = new Size(112, 15);
            label1.TabIndex = 12;
            label1.Text = "File To Replace With";
            // 
            // textboxFileToReplaceWith
            // 
            this.textboxFileToReplaceWith.Font = new Font("Segoe UI", 9F);
            this.textboxFileToReplaceWith.Location = new Point(6, 104);
            this.textboxFileToReplaceWith.Name = "textboxFileToReplaceWith";
            this.textboxFileToReplaceWith.ReadOnly = true;
            this.textboxFileToReplaceWith.Size = new Size(224, 23);
            this.textboxFileToReplaceWith.TabIndex = 11;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 9F);
            label2.Location = new Point(236, 39);
            label2.Name = "label2";
            label2.Size = new Size(62, 15);
            label2.TabIndex = 10;
            label2.Text = "Asset Type";
            // 
            // comboboxFileToReplaceType
            // 
            this.comboboxFileToReplaceType.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboboxFileToReplaceType.Enabled = false;
            this.comboboxFileToReplaceType.Font = new Font("Segoe UI", 9F);
            this.comboboxFileToReplaceType.FormattingEnabled = true;
            this.comboboxFileToReplaceType.Location = new Point(236, 58);
            this.comboboxFileToReplaceType.Name = "comboboxFileToReplaceType";
            this.comboboxFileToReplaceType.Size = new Size(91, 23);
            this.comboboxFileToReplaceType.TabIndex = 8;
            // 
            // textboxFileToReplace
            // 
            this.textboxFileToReplace.Font = new Font("Segoe UI", 9F);
            this.textboxFileToReplace.Location = new Point(6, 58);
            this.textboxFileToReplace.Name = "textboxFileToReplace";
            this.textboxFileToReplace.ReadOnly = true;
            this.textboxFileToReplace.Size = new Size(224, 23);
            this.textboxFileToReplace.TabIndex = 7;
            // 
            // linkLabel1
            // 
            linkLabel1.AutoSize = true;
            linkLabel1.Font = new Font("Segoe UI", 11F);
            linkLabel1.Location = new Point(151, 175);
            linkLabel1.Name = "linkLabel1";
            linkLabel1.Size = new Size(16, 20);
            linkLabel1.TabIndex = 6;
            linkLabel1.TabStop = true;
            linkLabel1.Text = "?";
            // 
            // checkboxProcessKnownType
            // 
            this.checkboxProcessKnownType.AutoSize = true;
            this.checkboxProcessKnownType.Font = new Font("Segoe UI", 9F);
            this.checkboxProcessKnownType.Location = new Point(6, 177);
            this.checkboxProcessKnownType.Name = "checkboxProcessKnownType";
            this.checkboxProcessKnownType.Size = new Size(149, 19);
            this.checkboxProcessKnownType.TabIndex = 5;
            this.checkboxProcessKnownType.Text = "Process As Known Type";
            this.checkboxProcessKnownType.UseVisualStyleBackColor = true;
            // 
            // comboboxFileToReplaceWithAssetType
            // 
            this.comboboxFileToReplaceWithAssetType.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboboxFileToReplaceWithAssetType.Font = new Font("Segoe UI", 9F);
            this.comboboxFileToReplaceWithAssetType.FormattingEnabled = true;
            this.comboboxFileToReplaceWithAssetType.Location = new Point(6, 148);
            this.comboboxFileToReplaceWithAssetType.Name = "comboboxFileToReplaceWithAssetType";
            this.comboboxFileToReplaceWithAssetType.Size = new Size(321, 23);
            this.comboboxFileToReplaceWithAssetType.TabIndex = 4;
            // 
            // label_Description_ImportFileType
            // 
            label_Description_ImportFileType.AutoSize = true;
            label_Description_ImportFileType.Font = new Font("Segoe UI", 9F);
            label_Description_ImportFileType.Location = new Point(6, 130);
            label_Description_ImportFileType.Name = "label_Description_ImportFileType";
            label_Description_ImportFileType.Size = new Size(110, 15);
            label_Description_ImportFileType.TabIndex = 3;
            label_Description_ImportFileType.Text = "New File Asset Type";
            // 
            // label_Description_ImportFileName
            // 
            label_Description_ImportFileName.AutoSize = true;
            label_Description_ImportFileName.Font = new Font("Segoe UI", 9F);
            label_Description_ImportFileName.Location = new Point(6, 39);
            label_Description_ImportFileName.Name = "label_Description_ImportFileName";
            label_Description_ImportFileName.Size = new Size(87, 15);
            label_Description_ImportFileName.TabIndex = 2;
            label_Description_ImportFileName.Text = "File To  Replace";
            // 
            // WindowReplace
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(362, 273);
            Controls.Add(groupBox_Details);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            FormStyle = ReaLTaiizor.Enum.Material.FormStyles.ActionBar_None;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "WindowReplace";
            Padding = new Padding(3, 24, 3, 3);
            StartPosition = FormStartPosition.CenterParent;
            Text = "WindowReplace";
            groupBox_Details.ResumeLayout(false);
            groupBox_Details.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox groupBox_Details;
        private LinkLabel linkLabel1;
        private CheckBox checkboxProcessKnownType;
        private ComboBox comboboxFileToReplaceWithAssetType;
        private Label label_Description_ImportFileType;
        private Label label_Description_ImportFileName;
        private TextBox textboxFileToReplaceWith;
        private Label label1;
        private TextBox textboxFileToReplace;
        private Label label2;
        private ComboBox comboboxFileToReplaceType;
        private Button buttonSelectFileToReplaceWith;
    }
}