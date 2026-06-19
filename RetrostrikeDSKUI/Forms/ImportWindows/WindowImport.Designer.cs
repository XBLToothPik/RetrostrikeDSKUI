namespace RetrostrikeDSKUI.Forms.ImportWindows
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
            groupBox_Details = new GroupBox();
            linkLabel1 = new LinkLabel();
            checkbox_ProcessKnownType = new CheckBox();
            comboBox_AssetType = new ComboBox();
            label_Description_ImportFileType = new Label();
            label_Description_ImportFileName = new Label();
            button_SelectImportFile = new Button();
            textbox_ImportFileName = new TextBox();
            buttonImport = new ReaLTaiizor.Controls.PoisonButton();
            buttonCancel = new ReaLTaiizor.Controls.PoisonButton();
            panel1 = new Panel();
            groupBox_Details.SuspendLayout();
            panel1.SuspendLayout();
            SuspendLayout();
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
            checkbox_ProcessKnownType.Font = new Font("Segoe UI", 9F);
            checkbox_ProcessKnownType.Location = new Point(6, 130);
            checkbox_ProcessKnownType.Name = "checkbox_ProcessKnownType";
            checkbox_ProcessKnownType.Size = new Size(149, 19);
            checkbox_ProcessKnownType.TabIndex = 5;
            checkbox_ProcessKnownType.Text = "Process As Known Type";
            checkbox_ProcessKnownType.UseVisualStyleBackColor = true;
            checkbox_ProcessKnownType.CheckedChanged += checkbox_ProcessKnownType_CheckedChanged;
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
            comboBox_AssetType.SelectedIndexChanged += comboBox_AssetType_SelectedIndexChanged;
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
            // buttonImport
            // 
            buttonImport.Location = new Point(3, 3);
            buttonImport.Name = "buttonImport";
            buttonImport.Size = new Size(252, 23);
            buttonImport.TabIndex = 7;
            buttonImport.Text = "Import/Next";
            buttonImport.UseSelectable = true;
            buttonImport.Click += button_Import_Click;
            // 
            // buttonCancel
            // 
            buttonCancel.Location = new Point(261, 3);
            buttonCancel.Name = "buttonCancel";
            buttonCancel.Size = new Size(75, 23);
            buttonCancel.TabIndex = 8;
            buttonCancel.Text = "Cancel";
            buttonCancel.UseSelectable = true;
            buttonCancel.Click += button_Cancel_Click;
            // 
            // panel1
            // 
            panel1.BorderStyle = BorderStyle.FixedSingle;
            panel1.Controls.Add(buttonImport);
            panel1.Controls.Add(buttonCancel);
            panel1.Dock = DockStyle.Bottom;
            panel1.Location = new Point(3, 188);
            panel1.Name = "panel1";
            panel1.Size = new Size(340, 29);
            panel1.TabIndex = 9;
            // 
            // WindowImport
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(346, 220);
            Controls.Add(panel1);
            Controls.Add(groupBox_Details);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            FormStyle = ReaLTaiizor.Enum.Material.FormStyles.ActionBar_None;
            MaximumSize = new Size(346, 220);
            Name = "WindowImport";
            Padding = new Padding(3, 24, 3, 3);
            StartPosition = FormStartPosition.CenterParent;
            Text = "Import New File";
            groupBox_Details.ResumeLayout(false);
            groupBox_Details.PerformLayout();
            panel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private GroupBox groupBox_Details;
        private Button button_SelectImportFile;
        private TextBox textbox_ImportFileName;
        private Label label_Description_ImportFileName;
        private Label label_Description_ImportFileType;
        private ComboBox comboBox_AssetType;
        private LinkLabel linkLabel1;
        private CheckBox checkbox_ProcessKnownType;
        private ReaLTaiizor.Controls.PoisonButton buttonImport;
        private ReaLTaiizor.Controls.PoisonButton buttonCancel;
        private Panel panel1;
    }
}