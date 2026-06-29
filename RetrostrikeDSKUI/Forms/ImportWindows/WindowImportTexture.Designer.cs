namespace RetrostrikeDSKUI.Forms.ImportWindows
{
    partial class WindowImportTexture
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
            buttonImport = new ReaLTaiizor.Controls.PoisonButton();
            panel1 = new Panel();
            poisonButton1 = new ReaLTaiizor.Controls.PoisonButton();
            groupBoxImportTexture = new GroupBox();
            labelMipsPreviewHeader = new ReaLTaiizor.Controls.MaterialLabel();
            updownMipBias = new NumericUpDown();
            labelMipBiasDesc = new Label();
            comboBoxTexFormat = new ComboBox();
            labelTexTypeDesc = new Label();
            labelTexFormatDesc = new Label();
            updownMips = new NumericUpDown();
            labelMipsDesc = new Label();
            comboBoxTexType = new ComboBox();
            updownDepth = new NumericUpDown();
            labelDepthDesc = new Label();
            labelHeight = new ReaLTaiizor.Controls.MaterialLabel();
            labelWidth = new ReaLTaiizor.Controls.MaterialLabel();
            labelHeightDesc = new Label();
            labelWidthDesc = new Label();
            pictureBoxTexturePreview = new PictureBox();
            buttonEditFacesOrMips = new ReaLTaiizor.Controls.PoisonButton();
            panel1.SuspendLayout();
            groupBoxImportTexture.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)updownMipBias).BeginInit();
            ((System.ComponentModel.ISupportInitialize)updownMips).BeginInit();
            ((System.ComponentModel.ISupportInitialize)updownDepth).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxTexturePreview).BeginInit();
            SuspendLayout();
            // 
            // buttonImport
            // 
            buttonImport.Location = new Point(3, 2);
            buttonImport.Name = "buttonImport";
            buttonImport.Size = new Size(121, 23);
            buttonImport.TabIndex = 0;
            buttonImport.Text = "Import";
            buttonImport.UseSelectable = true;
            buttonImport.Click += buttonImport_Click;
            // 
            // panel1
            // 
            panel1.BorderStyle = BorderStyle.FixedSingle;
            panel1.Controls.Add(buttonEditFacesOrMips);
            panel1.Controls.Add(poisonButton1);
            panel1.Controls.Add(buttonImport);
            panel1.Dock = DockStyle.Bottom;
            panel1.Location = new Point(3, 254);
            panel1.Name = "panel1";
            panel1.Size = new Size(326, 29);
            panel1.TabIndex = 1;
            // 
            // poisonButton1
            // 
            poisonButton1.Location = new Point(254, 2);
            poisonButton1.Name = "poisonButton1";
            poisonButton1.Size = new Size(69, 23);
            poisonButton1.TabIndex = 2;
            poisonButton1.Text = "Cancel";
            poisonButton1.UseSelectable = true;
            // 
            // groupBoxImportTexture
            // 
            groupBoxImportTexture.Controls.Add(labelMipsPreviewHeader);
            groupBoxImportTexture.Controls.Add(updownMipBias);
            groupBoxImportTexture.Controls.Add(labelMipBiasDesc);
            groupBoxImportTexture.Controls.Add(comboBoxTexFormat);
            groupBoxImportTexture.Controls.Add(labelTexTypeDesc);
            groupBoxImportTexture.Controls.Add(labelTexFormatDesc);
            groupBoxImportTexture.Controls.Add(updownMips);
            groupBoxImportTexture.Controls.Add(labelMipsDesc);
            groupBoxImportTexture.Controls.Add(comboBoxTexType);
            groupBoxImportTexture.Controls.Add(updownDepth);
            groupBoxImportTexture.Controls.Add(labelDepthDesc);
            groupBoxImportTexture.Controls.Add(labelHeight);
            groupBoxImportTexture.Controls.Add(labelWidth);
            groupBoxImportTexture.Controls.Add(labelHeightDesc);
            groupBoxImportTexture.Controls.Add(labelWidthDesc);
            groupBoxImportTexture.Controls.Add(pictureBoxTexturePreview);
            groupBoxImportTexture.Font = new Font("Segoe UI", 15F);
            groupBoxImportTexture.Location = new Point(6, 27);
            groupBoxImportTexture.Name = "groupBoxImportTexture";
            groupBoxImportTexture.Size = new Size(320, 226);
            groupBoxImportTexture.TabIndex = 2;
            groupBoxImportTexture.TabStop = false;
            groupBoxImportTexture.Text = "Import Texture";
            // 
            // labelMipsPreviewHeader
            // 
            labelMipsPreviewHeader.Depth = 0;
            labelMipsPreviewHeader.Font = new Font("Roboto", 12F, FontStyle.Regular, GraphicsUnit.Pixel);
            labelMipsPreviewHeader.FontType = ReaLTaiizor.Manager.MaterialSkinManager.FontType.Caption;
            labelMipsPreviewHeader.Location = new Point(202, 32);
            labelMipsPreviewHeader.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            labelMipsPreviewHeader.Name = "labelMipsPreviewHeader";
            labelMipsPreviewHeader.Size = new Size(112, 19);
            labelMipsPreviewHeader.TabIndex = 28;
            labelMipsPreviewHeader.Text = "Preview";
            labelMipsPreviewHeader.TextAlign = ContentAlignment.BottomCenter;
            // 
            // updownMipBias
            // 
            updownMipBias.DecimalPlaces = 2;
            updownMipBias.Font = new Font("Segoe UI", 10F);
            updownMipBias.Location = new Point(105, 140);
            updownMipBias.Name = "updownMipBias";
            updownMipBias.Size = new Size(91, 25);
            updownMipBias.TabIndex = 27;
            // 
            // labelMipBiasDesc
            // 
            labelMipBiasDesc.AutoSize = true;
            labelMipBiasDesc.Font = new Font("Microsoft Sans Serif", 15F, FontStyle.Regular, GraphicsUnit.Point, 0);
            labelMipBiasDesc.Location = new Point(6, 140);
            labelMipBiasDesc.Name = "labelMipBiasDesc";
            labelMipBiasDesc.Size = new Size(93, 25);
            labelMipBiasDesc.TabIndex = 26;
            labelMipBiasDesc.Text = "Mip Bias:";
            // 
            // comboBoxTexFormat
            // 
            comboBoxTexFormat.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxTexFormat.Font = new Font("Segoe UI", 10F);
            comboBoxTexFormat.FormattingEnabled = true;
            comboBoxTexFormat.Location = new Point(130, 196);
            comboBoxTexFormat.Name = "comboBoxTexFormat";
            comboBoxTexFormat.Size = new Size(184, 25);
            comboBoxTexFormat.TabIndex = 25;
            comboBoxTexFormat.SelectedIndexChanged += comboBoxTexFormat_SelectedIndexChanged;
            // 
            // labelTexTypeDesc
            // 
            labelTexTypeDesc.AutoSize = true;
            labelTexTypeDesc.Font = new Font("Microsoft Sans Serif", 15F, FontStyle.Regular, GraphicsUnit.Point, 0);
            labelTexTypeDesc.Location = new Point(6, 168);
            labelTexTypeDesc.Name = "labelTexTypeDesc";
            labelTexTypeDesc.Size = new Size(102, 25);
            labelTexTypeDesc.TabIndex = 24;
            labelTexTypeDesc.Text = "Tex Type:";
            // 
            // labelTexFormatDesc
            // 
            labelTexFormatDesc.AutoSize = true;
            labelTexFormatDesc.Font = new Font("Microsoft Sans Serif", 15F, FontStyle.Regular, GraphicsUnit.Point, 0);
            labelTexFormatDesc.Location = new Point(6, 196);
            labelTexFormatDesc.Name = "labelTexFormatDesc";
            labelTexFormatDesc.Size = new Size(118, 25);
            labelTexFormatDesc.TabIndex = 23;
            labelTexFormatDesc.Text = "Tex Format:";
            // 
            // updownMips
            // 
            updownMips.Font = new Font("Segoe UI", 10F);
            updownMips.Location = new Point(105, 112);
            updownMips.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            updownMips.Name = "updownMips";
            updownMips.Size = new Size(91, 25);
            updownMips.TabIndex = 22;
            updownMips.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // labelMipsDesc
            // 
            labelMipsDesc.AutoSize = true;
            labelMipsDesc.Font = new Font("Microsoft Sans Serif", 15F, FontStyle.Regular, GraphicsUnit.Point, 0);
            labelMipsDesc.Location = new Point(6, 112);
            labelMipsDesc.Name = "labelMipsDesc";
            labelMipsDesc.Size = new Size(60, 25);
            labelMipsDesc.TabIndex = 21;
            labelMipsDesc.Text = "Mips:";
            // 
            // comboBoxTexType
            // 
            comboBoxTexType.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxTexType.Font = new Font("Segoe UI", 10F);
            comboBoxTexType.FormattingEnabled = true;
            comboBoxTexType.Location = new Point(130, 168);
            comboBoxTexType.Name = "comboBoxTexType";
            comboBoxTexType.Size = new Size(184, 25);
            comboBoxTexType.TabIndex = 20;
            comboBoxTexType.SelectedIndexChanged += comboBoxTexType_SelectedIndexChanged;
            // 
            // updownDepth
            // 
            updownDepth.Font = new Font("Segoe UI", 10F);
            updownDepth.Location = new Point(105, 84);
            updownDepth.Name = "updownDepth";
            updownDepth.Size = new Size(91, 25);
            updownDepth.TabIndex = 19;
            // 
            // labelDepthDesc
            // 
            labelDepthDesc.AutoSize = true;
            labelDepthDesc.Font = new Font("Microsoft Sans Serif", 15F, FontStyle.Regular, GraphicsUnit.Point, 0);
            labelDepthDesc.Location = new Point(6, 84);
            labelDepthDesc.Name = "labelDepthDesc";
            labelDepthDesc.Size = new Size(70, 25);
            labelDepthDesc.TabIndex = 18;
            labelDepthDesc.Text = "Depth:";
            // 
            // labelHeight
            // 
            labelHeight.Depth = 0;
            labelHeight.Font = new Font("Roboto", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            labelHeight.Location = new Point(105, 63);
            labelHeight.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            labelHeight.Name = "labelHeight";
            labelHeight.Size = new Size(91, 19);
            labelHeight.TabIndex = 17;
            labelHeight.Text = "1080";
            // 
            // labelWidth
            // 
            labelWidth.Depth = 0;
            labelWidth.Font = new Font("Roboto", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            labelWidth.Location = new Point(105, 38);
            labelWidth.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            labelWidth.Name = "labelWidth";
            labelWidth.Size = new Size(91, 19);
            labelWidth.TabIndex = 16;
            labelWidth.Text = "1920";
            // 
            // labelHeightDesc
            // 
            labelHeightDesc.AutoSize = true;
            labelHeightDesc.Font = new Font("Microsoft Sans Serif", 15F, FontStyle.Regular, GraphicsUnit.Point, 0);
            labelHeightDesc.Location = new Point(6, 59);
            labelHeightDesc.Name = "labelHeightDesc";
            labelHeightDesc.Size = new Size(74, 25);
            labelHeightDesc.TabIndex = 12;
            labelHeightDesc.Text = "Height:";
            // 
            // labelWidthDesc
            // 
            labelWidthDesc.AutoSize = true;
            labelWidthDesc.Font = new Font("Microsoft Sans Serif", 15F, FontStyle.Regular, GraphicsUnit.Point, 0);
            labelWidthDesc.Location = new Point(6, 32);
            labelWidthDesc.Name = "labelWidthDesc";
            labelWidthDesc.Size = new Size(69, 25);
            labelWidthDesc.TabIndex = 11;
            labelWidthDesc.Text = "Width:";
            // 
            // pictureBoxTexturePreview
            // 
            pictureBoxTexturePreview.BorderStyle = BorderStyle.FixedSingle;
            pictureBoxTexturePreview.Location = new Point(202, 53);
            pictureBoxTexturePreview.Name = "pictureBoxTexturePreview";
            pictureBoxTexturePreview.Size = new Size(112, 112);
            pictureBoxTexturePreview.TabIndex = 0;
            pictureBoxTexturePreview.TabStop = false;
            // 
            // buttonEditFacesOrMips
            // 
            buttonEditFacesOrMips.Location = new Point(129, 2);
            buttonEditFacesOrMips.Name = "buttonEditFacesOrMips";
            buttonEditFacesOrMips.Size = new Size(121, 23);
            buttonEditFacesOrMips.TabIndex = 3;
            buttonEditFacesOrMips.Text = "Edit Mips";
            buttonEditFacesOrMips.UseSelectable = true;
            // 
            // WindowImportTexture
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(332, 286);
            Controls.Add(groupBoxImportTexture);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            FormStyle = ReaLTaiizor.Enum.Material.FormStyles.ActionBar_None;
            MaximizeBox = false;
            MaximumSize = new Size(332, 286);
            MinimizeBox = false;
            Name = "WindowImportTexture";
            Padding = new Padding(3, 24, 3, 3);
            Sizable = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "WindowImportTexture";
            TopMost = true;
            Shown += WindowImportTexture_Shown;
            panel1.ResumeLayout(false);
            groupBoxImportTexture.ResumeLayout(false);
            groupBoxImportTexture.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)updownMipBias).EndInit();
            ((System.ComponentModel.ISupportInitialize)updownMips).EndInit();
            ((System.ComponentModel.ISupportInitialize)updownDepth).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxTexturePreview).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private ReaLTaiizor.Controls.PoisonButton buttonImport;
        private Panel panel1;
        private ReaLTaiizor.Controls.PoisonButton poisonButton1;
        private GroupBox groupBoxImportTexture;
        private PictureBox pictureBoxTexturePreview;
        private Label labelWidthDesc;
        private Label labelHeightDesc;
        private Label labelDepthDesc;
        private ReaLTaiizor.Controls.MaterialLabel labelHeight;
        private ReaLTaiizor.Controls.MaterialLabel labelWidth;
        private NumericUpDown updownMips;
        private Label labelMipsDesc;
        private ComboBox comboBoxTexType;
        private NumericUpDown updownDepth;
        private Label labelTexFormatDesc;
        private ComboBox comboBoxTexFormat;
        private Label labelTexTypeDesc;
        private NumericUpDown updownMipBias;
        private Label labelMipBiasDesc;
        private ReaLTaiizor.Controls.MaterialLabel labelMipsPreviewHeader;
        private ReaLTaiizor.Controls.PoisonButton buttonEditFacesOrMips;
    }
}