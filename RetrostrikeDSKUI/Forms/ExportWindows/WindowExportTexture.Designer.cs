namespace RetrostrikeDSKUI.Forms.ExportWindows
{
    partial class WindowExportTexture
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
            mipsPreviewTitle = new ReaLTaiizor.Controls.MaterialLabel();
            pictureboxMipsPreview = new PictureBox();
            groupBoxMipsPreview = new GroupBox();
            labelMipsPreviewIndex = new ReaLTaiizor.Controls.MaterialLabel();
            buttonMipsPreviewGoRight = new Button();
            buttonMipsPreviewGoLeft = new Button();
            labelMips = new ReaLTaiizor.Controls.MaterialLabel();
            labelWidthDesc = new Label();
            labelHeightDesc = new Label();
            labelMipBiasDesc = new Label();
            labelHeight = new ReaLTaiizor.Controls.MaterialLabel();
            labelDepth = new ReaLTaiizor.Controls.MaterialLabel();
            labelMipBias = new ReaLTaiizor.Controls.MaterialLabel();
            labelTexFormatDesc = new Label();
            labelTexFormat = new ReaLTaiizor.Controls.MaterialLabel();
            labelRedFormatDesc = new Label();
            labelWidth = new ReaLTaiizor.Controls.MaterialLabel();
            labelRedFormat = new ReaLTaiizor.Controls.MaterialLabel();
            panelBottom = new Panel();
            buttonExportMips = new ReaLTaiizor.Controls.PoisonDropDownButton();
            buttonCancel = new ReaLTaiizor.Controls.PoisonDropDownButton();
            buttonExportImage = new ReaLTaiizor.Controls.PoisonDropDownButton();
            labelDepthDesc = new Label();
            labelMipsDesc = new Label();
            ((System.ComponentModel.ISupportInitialize)pictureboxMipsPreview).BeginInit();
            groupBoxMipsPreview.SuspendLayout();
            panelBottom.SuspendLayout();
            SuspendLayout();
            // 
            // mipsPreviewTitle
            // 
            mipsPreviewTitle.AutoSize = true;
            mipsPreviewTitle.Depth = 0;
            mipsPreviewTitle.Font = new Font("Roboto", 16F, FontStyle.Regular, GraphicsUnit.Pixel);
            mipsPreviewTitle.FontType = ReaLTaiizor.Manager.MaterialSkinManager.FontType.Subtitle1;
            mipsPreviewTitle.Location = new Point(48, 12);
            mipsPreviewTitle.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            mipsPreviewTitle.Name = "mipsPreviewTitle";
            mipsPreviewTitle.Size = new Size(36, 19);
            mipsPreviewTitle.TabIndex = 2;
            mipsPreviewTitle.Text = "Mips";
            // 
            // pictureboxMipsPreview
            // 
            pictureboxMipsPreview.BackColor = Color.Transparent;
            pictureboxMipsPreview.BorderStyle = BorderStyle.FixedSingle;
            pictureboxMipsPreview.Location = new Point(20, 33);
            pictureboxMipsPreview.Name = "pictureboxMipsPreview";
            pictureboxMipsPreview.Size = new Size(84, 84);
            pictureboxMipsPreview.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureboxMipsPreview.TabIndex = 3;
            pictureboxMipsPreview.TabStop = false;
            // 
            // groupBoxMipsPreview
            // 
            groupBoxMipsPreview.BackColor = Color.Transparent;
            groupBoxMipsPreview.Controls.Add(labelMipsPreviewIndex);
            groupBoxMipsPreview.Controls.Add(buttonMipsPreviewGoRight);
            groupBoxMipsPreview.Controls.Add(buttonMipsPreviewGoLeft);
            groupBoxMipsPreview.Controls.Add(mipsPreviewTitle);
            groupBoxMipsPreview.Controls.Add(pictureboxMipsPreview);
            groupBoxMipsPreview.Location = new Point(210, 35);
            groupBoxMipsPreview.Name = "groupBoxMipsPreview";
            groupBoxMipsPreview.Size = new Size(124, 139);
            groupBoxMipsPreview.TabIndex = 7;
            groupBoxMipsPreview.TabStop = false;
            // 
            // labelMipsPreviewIndex
            // 
            labelMipsPreviewIndex.Depth = 0;
            labelMipsPreviewIndex.Dock = DockStyle.Bottom;
            labelMipsPreviewIndex.Font = new Font("Roboto", 12F, FontStyle.Regular, GraphicsUnit.Pixel);
            labelMipsPreviewIndex.FontType = ReaLTaiizor.Manager.MaterialSkinManager.FontType.Caption;
            labelMipsPreviewIndex.Location = new Point(3, 119);
            labelMipsPreviewIndex.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            labelMipsPreviewIndex.Name = "labelMipsPreviewIndex";
            labelMipsPreviewIndex.Size = new Size(118, 17);
            labelMipsPreviewIndex.TabIndex = 10;
            labelMipsPreviewIndex.Text = "0 / 0 (512x512)";
            labelMipsPreviewIndex.TextAlign = ContentAlignment.TopCenter;
            // 
            // buttonMipsPreviewGoRight
            // 
            buttonMipsPreviewGoRight.Location = new Point(104, 32);
            buttonMipsPreviewGoRight.Name = "buttonMipsPreviewGoRight";
            buttonMipsPreviewGoRight.Size = new Size(16, 86);
            buttonMipsPreviewGoRight.TabIndex = 9;
            buttonMipsPreviewGoRight.Text = ">";
            buttonMipsPreviewGoRight.UseVisualStyleBackColor = true;
            buttonMipsPreviewGoRight.Click += buttonMipsPreviewGoRight_Click;
            // 
            // buttonMipsPreviewGoLeft
            // 
            buttonMipsPreviewGoLeft.Location = new Point(4, 32);
            buttonMipsPreviewGoLeft.Name = "buttonMipsPreviewGoLeft";
            buttonMipsPreviewGoLeft.Size = new Size(16, 86);
            buttonMipsPreviewGoLeft.TabIndex = 8;
            buttonMipsPreviewGoLeft.Text = "<";
            buttonMipsPreviewGoLeft.UseVisualStyleBackColor = true;
            buttonMipsPreviewGoLeft.Click += buttonMipsPreviewGoLeft_Click;
            // 
            // labelMips
            // 
            labelMips.Depth = 0;
            labelMips.Font = new Font("Roboto", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            labelMips.Location = new Point(130, 127);
            labelMips.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            labelMips.Name = "labelMips";
            labelMips.Size = new Size(74, 19);
            labelMips.TabIndex = 9;
            labelMips.Text = "10";
            // 
            // labelWidthDesc
            // 
            labelWidthDesc.AutoSize = true;
            labelWidthDesc.Font = new Font("Microsoft Sans Serif", 15F, FontStyle.Regular, GraphicsUnit.Point, 0);
            labelWidthDesc.Location = new Point(6, 41);
            labelWidthDesc.Name = "labelWidthDesc";
            labelWidthDesc.Size = new Size(69, 25);
            labelWidthDesc.TabIndex = 10;
            labelWidthDesc.Text = "Width:";
            // 
            // labelHeightDesc
            // 
            labelHeightDesc.AutoSize = true;
            labelHeightDesc.Font = new Font("Microsoft Sans Serif", 15F, FontStyle.Regular, GraphicsUnit.Point, 0);
            labelHeightDesc.Location = new Point(6, 68);
            labelHeightDesc.Name = "labelHeightDesc";
            labelHeightDesc.Size = new Size(74, 25);
            labelHeightDesc.TabIndex = 11;
            labelHeightDesc.Text = "Height:";
            // 
            // labelMipBiasDesc
            // 
            labelMipBiasDesc.AutoSize = true;
            labelMipBiasDesc.Font = new Font("Microsoft Sans Serif", 15F, FontStyle.Regular, GraphicsUnit.Point, 0);
            labelMipBiasDesc.Location = new Point(6, 149);
            labelMipBiasDesc.Name = "labelMipBiasDesc";
            labelMipBiasDesc.Size = new Size(93, 25);
            labelMipBiasDesc.TabIndex = 14;
            labelMipBiasDesc.Text = "Mip Bias:";
            // 
            // labelHeight
            // 
            labelHeight.Depth = 0;
            labelHeight.Font = new Font("Roboto", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            labelHeight.Location = new Point(130, 73);
            labelHeight.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            labelHeight.Name = "labelHeight";
            labelHeight.Size = new Size(74, 19);
            labelHeight.TabIndex = 16;
            labelHeight.Text = "512";
            // 
            // labelDepth
            // 
            labelDepth.Depth = 0;
            labelDepth.Font = new Font("Roboto", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            labelDepth.Location = new Point(130, 101);
            labelDepth.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            labelDepth.Name = "labelDepth";
            labelDepth.Size = new Size(74, 15);
            labelDepth.TabIndex = 17;
            labelDepth.Text = "1";
            // 
            // labelMipBias
            // 
            labelMipBias.Depth = 0;
            labelMipBias.Font = new Font("Roboto", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            labelMipBias.Location = new Point(130, 154);
            labelMipBias.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            labelMipBias.Name = "labelMipBias";
            labelMipBias.Size = new Size(74, 19);
            labelMipBias.TabIndex = 18;
            labelMipBias.Text = "0.75";
            // 
            // labelTexFormatDesc
            // 
            labelTexFormatDesc.AutoSize = true;
            labelTexFormatDesc.Font = new Font("Microsoft Sans Serif", 15F, FontStyle.Regular, GraphicsUnit.Point, 0);
            labelTexFormatDesc.Location = new Point(6, 176);
            labelTexFormatDesc.Name = "labelTexFormatDesc";
            labelTexFormatDesc.Size = new Size(118, 25);
            labelTexFormatDesc.TabIndex = 19;
            labelTexFormatDesc.Text = "Tex Format:";
            // 
            // labelTexFormat
            // 
            labelTexFormat.Depth = 0;
            labelTexFormat.Font = new Font("Roboto", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            labelTexFormat.Location = new Point(130, 181);
            labelTexFormat.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            labelTexFormat.Name = "labelTexFormat";
            labelTexFormat.Size = new Size(200, 19);
            labelTexFormat.TabIndex = 20;
            labelTexFormat.Text = "XBOXFMT_D16_LOCKABLE";
            // 
            // labelRedFormatDesc
            // 
            labelRedFormatDesc.AutoSize = true;
            labelRedFormatDesc.Font = new Font("Microsoft Sans Serif", 15F, FontStyle.Regular, GraphicsUnit.Point, 0);
            labelRedFormatDesc.Location = new Point(6, 203);
            labelRedFormatDesc.Name = "labelRedFormatDesc";
            labelRedFormatDesc.Size = new Size(119, 25);
            labelRedFormatDesc.TabIndex = 21;
            labelRedFormatDesc.Text = "Red Format:";
            // 
            // labelWidth
            // 
            labelWidth.Depth = 0;
            labelWidth.Font = new Font("Roboto", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            labelWidth.Location = new Point(130, 46);
            labelWidth.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            labelWidth.Name = "labelWidth";
            labelWidth.Size = new Size(74, 19);
            labelWidth.TabIndex = 15;
            labelWidth.Text = "1920";
            // 
            // labelRedFormat
            // 
            labelRedFormat.Depth = 0;
            labelRedFormat.Font = new Font("Roboto", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            labelRedFormat.Location = new Point(130, 208);
            labelRedFormat.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            labelRedFormat.Name = "labelRedFormat";
            labelRedFormat.Size = new Size(200, 19);
            labelRedFormat.TabIndex = 22;
            labelRedFormat.Text = "CUBEMAP";
            // 
            // panelBottom
            // 
            panelBottom.BorderStyle = BorderStyle.FixedSingle;
            panelBottom.Controls.Add(buttonExportMips);
            panelBottom.Controls.Add(buttonCancel);
            panelBottom.Controls.Add(buttonExportImage);
            panelBottom.Dock = DockStyle.Bottom;
            panelBottom.Location = new Point(3, 232);
            panelBottom.Name = "panelBottom";
            panelBottom.Size = new Size(332, 29);
            panelBottom.TabIndex = 25;
            // 
            // buttonExportMips
            // 
            buttonExportMips.AutoSize = true;
            buttonExportMips.Location = new Point(116, 2);
            buttonExportMips.Name = "buttonExportMips";
            buttonExportMips.Size = new Size(108, 23);
            buttonExportMips.TabIndex = 30;
            buttonExportMips.Text = "Export Mips";
            buttonExportMips.UseSelectable = true;
            buttonExportMips.Click += buttonExportMips_Click;
            // 
            // buttonCancel
            // 
            buttonCancel.AutoSize = true;
            buttonCancel.Location = new Point(267, 2);
            buttonCancel.Name = "buttonCancel";
            buttonCancel.Size = new Size(59, 23);
            buttonCancel.TabIndex = 29;
            buttonCancel.Text = "Cancel";
            buttonCancel.UseSelectable = true;
            // 
            // buttonExportImage
            // 
            buttonExportImage.AutoSize = true;
            buttonExportImage.Location = new Point(2, 2);
            buttonExportImage.Name = "buttonExportImage";
            buttonExportImage.Size = new Size(108, 23);
            buttonExportImage.TabIndex = 28;
            buttonExportImage.Text = "Export Image";
            buttonExportImage.UseSelectable = true;
            buttonExportImage.Click += buttonExportImage_Click;
            // 
            // labelDepthDesc
            // 
            labelDepthDesc.AutoSize = true;
            labelDepthDesc.Font = new Font("Microsoft Sans Serif", 15F, FontStyle.Regular, GraphicsUnit.Point, 0);
            labelDepthDesc.Location = new Point(6, 96);
            labelDepthDesc.Name = "labelDepthDesc";
            labelDepthDesc.Size = new Size(70, 25);
            labelDepthDesc.TabIndex = 26;
            labelDepthDesc.Text = "Depth:";
            // 
            // labelMipsDesc
            // 
            labelMipsDesc.AutoSize = true;
            labelMipsDesc.Font = new Font("Microsoft Sans Serif", 15F, FontStyle.Regular, GraphicsUnit.Point, 0);
            labelMipsDesc.Location = new Point(6, 123);
            labelMipsDesc.Name = "labelMipsDesc";
            labelMipsDesc.Size = new Size(60, 25);
            labelMipsDesc.TabIndex = 27;
            labelMipsDesc.Text = "Mips:";
            // 
            // WindowExportTexture
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            ClientSize = new Size(338, 264);
            Controls.Add(labelMipsDesc);
            Controls.Add(labelDepthDesc);
            Controls.Add(panelBottom);
            Controls.Add(labelRedFormat);
            Controls.Add(labelRedFormatDesc);
            Controls.Add(labelTexFormat);
            Controls.Add(labelTexFormatDesc);
            Controls.Add(labelMipBias);
            Controls.Add(labelDepth);
            Controls.Add(labelHeight);
            Controls.Add(labelWidth);
            Controls.Add(labelMipBiasDesc);
            Controls.Add(labelHeightDesc);
            Controls.Add(labelWidthDesc);
            Controls.Add(labelMips);
            Controls.Add(groupBoxMipsPreview);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            FormStyle = ReaLTaiizor.Enum.Material.FormStyles.ActionBar_None;
            MaximizeBox = false;
            MaximumSize = new Size(338, 264);
            MinimizeBox = false;
            Name = "WindowExportTexture";
            Padding = new Padding(3, 24, 3, 3);
            Sizable = false;
            Text = "Export Texture - N/A";
            TopMost = true;
            ((System.ComponentModel.ISupportInitialize)pictureboxMipsPreview).EndInit();
            groupBoxMipsPreview.ResumeLayout(false);
            groupBoxMipsPreview.PerformLayout();
            panelBottom.ResumeLayout(false);
            panelBottom.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private ReaLTaiizor.Controls.MaterialLabel mipsPreviewTitle;
        private PictureBox pictureboxMipsPreview;
        private GroupBox groupBoxMipsPreview;
        private Button buttonMipsPreviewGoLeft;
        private Button buttonMipsPreviewGoRight;
        private ReaLTaiizor.Controls.MaterialLabel labelMips;
        private ReaLTaiizor.Controls.MaterialLabel labelMipsPreviewIndex;
        private Label labelWidthDesc;
        private Label labelHeightDesc;
        private Label labelMipBiasDesc;
        private ReaLTaiizor.Controls.MaterialLabel labelHeight;
        private ReaLTaiizor.Controls.MaterialLabel labelDepth;
        private ReaLTaiizor.Controls.MaterialLabel labelMipBias;
        private Label labelTexFormatDesc;
        private ReaLTaiizor.Controls.MaterialLabel labelTexFormat;
        private Label labelRedFormatDesc;
        private ReaLTaiizor.Controls.MaterialLabel labelWidth;
        private ReaLTaiizor.Controls.MaterialLabel labelRedFormat;
        private Panel panelBottom;
        private Label labelDepthDesc;
        private Label labelMipsDesc;
        private ReaLTaiizor.Controls.PoisonDropDownButton buttonCancel;
        private ReaLTaiizor.Controls.PoisonDropDownButton buttonExportImage;
        private ReaLTaiizor.Controls.PoisonDropDownButton buttonExportMips;
    }
}