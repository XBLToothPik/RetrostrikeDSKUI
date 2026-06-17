namespace RetrostrikeDSKUI.Forms
{
    partial class WindowImport
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
            button_Import = new Button();
            groupBox_Details = new GroupBox();
            linkLabel1 = new LinkLabel();
            checkbox_ProcessKnownType = new CheckBox();
            comboBox_AssetType = new ComboBox();
            label_Description_ImportFileType = new Label();
            label_Description_ImportFileName = new Label();
            button_SelectImportFile = new Button();
            textbox_ImportFileName = new TextBox();
            button_Cancel = new Button();
            groupBox_Details.SuspendLayout();
            SuspendLayout();
            // 
            // button_Import
            // 
            button_Import.Location = new Point(6, 193);
            button_Import.Name = "button_Import";
            button_Import.Size = new Size(252, 23);
            button_Import.TabIndex = 1;
            button_Import.Text = "Import";
            button_Import.UseVisualStyleBackColor = true;
            button_Import.Click += button_Import_Click;
            // 
            // groupBox_Details
            // 
            groupBox_Details.Controls.Add(linkLabel1);
            groupBox_Details.Controls.Add(checkbox_ProcessKnownType);
            groupBox_Details.Controls.Add(comboBox_AssetType);
            groupBox_Details.Controls.Add(label_Description_ImportFileType);
            groupBox_Details.Controls.Add(label_Description_ImportFileName);
            groupBox_Details.Controls.Add(button_SelectImportFile);
            groupBox_Details.Controls.Add(textbox_ImportFileName);
            groupBox_Details.Font = new Font("Segoe UI", 15F);
            groupBox_Details.Location = new Point(6, 27);
            groupBox_Details.Name = "groupBox_Details";
            groupBox_Details.Size = new Size(333, 160);
            groupBox_Details.TabIndex = 3;
            groupBox_Details.TabStop = false;
            groupBox_Details.Text = "Import Details";
            // 
            // linkLabel1
            // 
            linkLabel1.AutoSize = true;
            linkLabel1.Font = new Font("Segoe UI", 11F);
            linkLabel1.Location = new Point(151, 128);
            linkLabel1.Name = "linkLabel1";
            linkLabel1.Size = new Size(16, 20);
            linkLabel1.TabIndex = 6;
            linkLabel1.TabStop = true;
            linkLabel1.Text = "?";
            // 
            // checkbox_ProcessKnownType
            // 
            checkbox_ProcessKnownType.AutoSize = true;
            checkbox_ProcessKnownType.Checked = true;
            checkbox_ProcessKnownType.CheckState = CheckState.Checked;
            checkbox_ProcessKnownType.Font = new Font("Segoe UI", 9F);
            checkbox_ProcessKnownType.Location = new Point(6, 130);
            checkbox_ProcessKnownType.Name = "checkbox_ProcessKnownType";
            checkbox_ProcessKnownType.Size = new Size(149, 19);
            checkbox_ProcessKnownType.TabIndex = 5;
            checkbox_ProcessKnownType.Text = "Process As Known Type";
            checkbox_ProcessKnownType.UseVisualStyleBackColor = true;
            // 
            // comboBox_AssetType
            // 
            comboBox_AssetType.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox_AssetType.Font = new Font("Segoe UI", 9F);
            comboBox_AssetType.FormattingEnabled = true;
            comboBox_AssetType.Location = new Point(6, 101);
            comboBox_AssetType.Name = "comboBox_AssetType";
            comboBox_AssetType.Size = new Size(321, 23);
            comboBox_AssetType.TabIndex = 4;
            // 
            // label_Description_ImportFileType
            // 
            label_Description_ImportFileType.AutoSize = true;
            label_Description_ImportFileType.Font = new Font("Segoe UI", 9F);
            label_Description_ImportFileType.Location = new Point(6, 83);
            label_Description_ImportFileType.Name = "label_Description_ImportFileType";
            label_Description_ImportFileType.Size = new Size(62, 15);
            label_Description_ImportFileType.TabIndex = 3;
            label_Description_ImportFileType.Text = "Asset Type";
            // 
            // label_Description_ImportFileName
            // 
            label_Description_ImportFileName.AutoSize = true;
            label_Description_ImportFileName.Font = new Font("Segoe UI", 9F);
            label_Description_ImportFileName.Location = new Point(6, 39);
            label_Description_ImportFileName.Name = "label_Description_ImportFileName";
            label_Description_ImportFileName.Size = new Size(79, 15);
            label_Description_ImportFileName.TabIndex = 2;
            label_Description_ImportFileName.Text = "File To Import";
            // 
            // button_SelectImportFile
            // 
            button_SelectImportFile.Font = new Font("Segoe UI", 9F);
            button_SelectImportFile.Location = new Point(294, 57);
            button_SelectImportFile.Name = "button_SelectImportFile";
            button_SelectImportFile.Size = new Size(33, 23);
            button_SelectImportFile.TabIndex = 1;
            button_SelectImportFile.Text = "...";
            button_SelectImportFile.UseVisualStyleBackColor = true;
            // 
            // textbox_ImportFileName
            // 
            textbox_ImportFileName.Font = new Font("Segoe UI", 9F);
            textbox_ImportFileName.Location = new Point(6, 57);
            textbox_ImportFileName.Name = "textbox_ImportFileName";
            textbox_ImportFileName.ReadOnly = true;
            textbox_ImportFileName.Size = new Size(282, 23);
            textbox_ImportFileName.TabIndex = 0;
            // 
            // button_Cancel
            // 
            button_Cancel.Location = new Point(264, 193);
            button_Cancel.Name = "button_Cancel";
            button_Cancel.Size = new Size(75, 23);
            button_Cancel.TabIndex = 2;
            button_Cancel.Text = "Cancel";
            button_Cancel.UseVisualStyleBackColor = true;
            button_Cancel.Click += button_Cancel_Click;
            // 
            // WindowImport
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(345, 221);
            Controls.Add(groupBox_Details);
            Controls.Add(button_Cancel);
            Controls.Add(button_Import);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            FormStyle = ReaLTaiizor.Enum.Material.FormStyles.ActionBar_None;
            Name = "WindowImport";
            Padding = new Padding(3, 24, 3, 3);
            StartPosition = FormStartPosition.CenterParent;
            Text = "Import New File";
            groupBox_Details.ResumeLayout(false);
            groupBox_Details.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Button button_Import;
        private GroupBox groupBox_Details;
        private Button button_SelectImportFile;
        private TextBox textbox_ImportFileName;
        private Label label_Description_ImportFileName;
        private Label label_Description_ImportFileType;
        private ComboBox comboBox_AssetType;
        private Button button_Cancel;
        private LinkLabel linkLabel1;
        private CheckBox checkbox_ProcessKnownType;
    }
}