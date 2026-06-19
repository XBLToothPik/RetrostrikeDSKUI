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
            groupBox1 = new GroupBox();
            materialLabel2 = new ReaLTaiizor.Controls.MaterialLabel();
            numericUpDown3 = new NumericUpDown();
            label5 = new Label();
            comboBox2 = new ComboBox();
            label4 = new Label();
            label3 = new Label();
            numericUpDown2 = new NumericUpDown();
            label2 = new Label();
            comboBox1 = new ComboBox();
            numericUpDown1 = new NumericUpDown();
            label1 = new Label();
            materialLabel1 = new ReaLTaiizor.Controls.MaterialLabel();
            labelWidth = new ReaLTaiizor.Controls.MaterialLabel();
            labelHeightDesc = new Label();
            labelWidthDesc = new Label();
            pictureBoxTexturePreview = new PictureBox();
            panel1.SuspendLayout();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDown3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxTexturePreview).BeginInit();
            SuspendLayout();
            // 
            // buttonImport
            // 
            buttonImport.Location = new Point(3, 3);
            buttonImport.Name = "buttonImport";
            buttonImport.Size = new Size(245, 23);
            buttonImport.TabIndex = 0;
            buttonImport.Text = "Import";
            buttonImport.UseSelectable = true;
            // 
            // panel1
            // 
            panel1.BorderStyle = BorderStyle.FixedSingle;
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
            // groupBox1
            // 
            groupBox1.Controls.Add(materialLabel2);
            groupBox1.Controls.Add(numericUpDown3);
            groupBox1.Controls.Add(label5);
            groupBox1.Controls.Add(comboBox2);
            groupBox1.Controls.Add(label4);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(numericUpDown2);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(comboBox1);
            groupBox1.Controls.Add(numericUpDown1);
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(materialLabel1);
            groupBox1.Controls.Add(labelWidth);
            groupBox1.Controls.Add(labelHeightDesc);
            groupBox1.Controls.Add(labelWidthDesc);
            groupBox1.Controls.Add(pictureBoxTexturePreview);
            groupBox1.Font = new Font("Segoe UI", 15F);
            groupBox1.Location = new Point(6, 27);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(320, 226);
            groupBox1.TabIndex = 2;
            groupBox1.TabStop = false;
            groupBox1.Text = "Import Texture";
            // 
            // materialLabel2
            // 
            materialLabel2.Depth = 0;
            materialLabel2.Font = new Font("Roboto", 12F, FontStyle.Regular, GraphicsUnit.Pixel);
            materialLabel2.FontType = ReaLTaiizor.Manager.MaterialSkinManager.FontType.Caption;
            materialLabel2.Location = new Point(202, 32);
            materialLabel2.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            materialLabel2.Name = "materialLabel2";
            materialLabel2.Size = new Size(112, 19);
            materialLabel2.TabIndex = 28;
            materialLabel2.Text = "Preview";
            materialLabel2.TextAlign = ContentAlignment.BottomCenter;
            // 
            // numericUpDown3
            // 
            numericUpDown3.Font = new Font("Segoe UI", 10F);
            numericUpDown3.Location = new Point(105, 140);
            numericUpDown3.Name = "numericUpDown3";
            numericUpDown3.Size = new Size(91, 25);
            numericUpDown3.TabIndex = 27;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Microsoft Sans Serif", 15F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label5.Location = new Point(6, 140);
            label5.Name = "label5";
            label5.Size = new Size(93, 25);
            label5.TabIndex = 26;
            label5.Text = "Mip Bias:";
            // 
            // comboBox2
            // 
            comboBox2.Font = new Font("Segoe UI", 10F);
            comboBox2.FormattingEnabled = true;
            comboBox2.Location = new Point(130, 196);
            comboBox2.Name = "comboBox2";
            comboBox2.Size = new Size(184, 25);
            comboBox2.TabIndex = 25;
            comboBox2.Text = "CUBEMAP";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Microsoft Sans Serif", 15F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label4.Location = new Point(6, 168);
            label4.Name = "label4";
            label4.Size = new Size(102, 25);
            label4.TabIndex = 24;
            label4.Text = "Tex Type:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Microsoft Sans Serif", 15F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label3.Location = new Point(6, 196);
            label3.Name = "label3";
            label3.Size = new Size(118, 25);
            label3.TabIndex = 23;
            label3.Text = "Tex Format:";
            // 
            // numericUpDown2
            // 
            numericUpDown2.Font = new Font("Segoe UI", 10F);
            numericUpDown2.Location = new Point(105, 112);
            numericUpDown2.Name = "numericUpDown2";
            numericUpDown2.Size = new Size(91, 25);
            numericUpDown2.TabIndex = 22;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Microsoft Sans Serif", 15F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.Location = new Point(6, 112);
            label2.Name = "label2";
            label2.Size = new Size(60, 25);
            label2.TabIndex = 21;
            label2.Text = "Mips:";
            // 
            // comboBox1
            // 
            comboBox1.Font = new Font("Segoe UI", 10F);
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(130, 168);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(184, 25);
            comboBox1.TabIndex = 20;
            comboBox1.Text = "XBOXFMT_D16_LOCKABLE";
            // 
            // numericUpDown1
            // 
            numericUpDown1.Font = new Font("Segoe UI", 10F);
            numericUpDown1.Location = new Point(105, 84);
            numericUpDown1.Name = "numericUpDown1";
            numericUpDown1.Size = new Size(91, 25);
            numericUpDown1.TabIndex = 19;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Microsoft Sans Serif", 15F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.Location = new Point(6, 84);
            label1.Name = "label1";
            label1.Size = new Size(70, 25);
            label1.TabIndex = 18;
            label1.Text = "Depth:";
            // 
            // materialLabel1
            // 
            materialLabel1.Depth = 0;
            materialLabel1.Font = new Font("Roboto", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            materialLabel1.Location = new Point(105, 63);
            materialLabel1.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            materialLabel1.Name = "materialLabel1";
            materialLabel1.Size = new Size(91, 19);
            materialLabel1.TabIndex = 17;
            materialLabel1.Text = "1080";
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
            // WindowImportTexture
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(332, 286);
            Controls.Add(groupBox1);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            FormStyle = ReaLTaiizor.Enum.Material.FormStyles.ActionBar_None;
            MaximizeBox = false;
            MaximumSize = new Size(332, 286);
            MinimizeBox = false;
            Name = "WindowImportTexture";
            Padding = new Padding(3, 24, 3, 3);
            Text = "WindowImportTexture";
            TopMost = true;
            Shown += WindowImportTexture_Shown;
            panel1.ResumeLayout(false);
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDown3).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown2).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxTexturePreview).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private ReaLTaiizor.Controls.PoisonButton buttonImport;
        private Panel panel1;
        private ReaLTaiizor.Controls.PoisonButton poisonButton1;
        private GroupBox groupBox1;
        private PictureBox pictureBoxTexturePreview;
        private Label labelWidthDesc;
        private Label labelHeightDesc;
        private Label label1;
        private ReaLTaiizor.Controls.MaterialLabel materialLabel1;
        private ReaLTaiizor.Controls.MaterialLabel labelWidth;
        private NumericUpDown numericUpDown2;
        private Label label2;
        private ComboBox comboBox1;
        private NumericUpDown numericUpDown1;
        private Label label3;
        private ComboBox comboBox2;
        private Label label4;
        private NumericUpDown numericUpDown3;
        private Label label5;
        private ReaLTaiizor.Controls.MaterialLabel materialLabel2;
    }
}