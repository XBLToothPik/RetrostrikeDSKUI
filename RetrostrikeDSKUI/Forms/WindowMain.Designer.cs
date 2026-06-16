namespace RetrostrikeDSKUI
{
    partial class WindowMain
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            ReaLTaiizor.ControlRenderer controlRenderer1 = new ReaLTaiizor.ControlRenderer();
            ReaLTaiizor.MSColorTable msColorTable1 = new ReaLTaiizor.MSColorTable();
            FileOptionsContextMenu = new ContextMenuStrip(components);
            mainStatusStrip_Label_Status_old = new ToolStripStatusLabel();
            mainMenuStrip = new ReaLTaiizor.Controls.ParrotFlatMenuStrip();
            mainMenuStrip_File = new ToolStripMenuItem();
            mainMenuStrip_File_OpenDSK = new ToolStripMenuItem();
            mainMenuStrip_File_Save = new ToolStripMenuItem();
            currentDSKToolStripMenuItem = new ToolStripMenuItem();
            mainMenuStrip_File_Save_AsNew = new ToolStripMenuItem();
            mainMenuStrip_Debug = new ToolStripMenuItem();
            mainStatusStrip = new ReaLTaiizor.Controls.FormStatusStrip();
            mainStatusStrip_Label_Status = new ToolStripStatusLabel();
            mainTabControl = new TabControl();
            mainMenuStrip_Debug_Test = new ToolStripMenuItem();
            mainMenuStrip.SuspendLayout();
            mainStatusStrip.SuspendLayout();
            SuspendLayout();
            // 
            // FileOptionsContextMenu
            // 
            FileOptionsContextMenu.Name = "FileOptionsContextMenu";
            FileOptionsContextMenu.Size = new Size(61, 4);
            FileOptionsContextMenu.Opening += FileOptionsContextMenu_Opening;
            // 
            // mainStatusStrip_Label_Status_old
            // 
            mainStatusStrip_Label_Status_old.Name = "mainStatusStrip_Label_Status_old";
            mainStatusStrip_Label_Status_old.Size = new Size(45, 17);
            mainStatusStrip_Label_Status_old.Text = "STATUS";
            // 
            // mainMenuStrip
            // 
            mainMenuStrip.BackColor = SystemColors.Window;
            mainMenuStrip.HoverBackColor = Color.Gray;
            mainMenuStrip.HoverTextColor = Color.White;
            mainMenuStrip.ItemBackColor = SystemColors.Window;
            mainMenuStrip.Items.AddRange(new ToolStripItem[] { mainMenuStrip_File, mainMenuStrip_File_Save, mainMenuStrip_Debug });
            mainMenuStrip.Location = new Point(3, 24);
            mainMenuStrip.Name = "mainMenuStrip";
            mainMenuStrip.Padding = new Padding(4, 2, 0, 2);
            mainMenuStrip.SelectedBackColor = Color.White;
            mainMenuStrip.SelectedTextColor = Color.Black;
            mainMenuStrip.SeparatorColor = Color.White;
            mainMenuStrip.Size = new Size(589, 24);
            mainMenuStrip.TabIndex = 8;
            mainMenuStrip.Text = "parrotFlatMenuStrip1";
            mainMenuStrip.TextColor = Color.Black;
            // 
            // mainMenuStrip_File
            // 
            mainMenuStrip_File.DropDownItems.AddRange(new ToolStripItem[] { mainMenuStrip_File_OpenDSK });
            mainMenuStrip_File.ForeColor = Color.Black;
            mainMenuStrip_File.Name = "mainMenuStrip_File";
            mainMenuStrip_File.Size = new Size(37, 20);
            mainMenuStrip_File.Text = "File";
            // 
            // mainMenuStrip_File_OpenDSK
            // 
            mainMenuStrip_File_OpenDSK.ForeColor = Color.Black;
            mainMenuStrip_File_OpenDSK.Name = "mainMenuStrip_File_OpenDSK";
            mainMenuStrip_File_OpenDSK.Size = new Size(127, 22);
            mainMenuStrip_File_OpenDSK.Text = "Open DSK";
            mainMenuStrip_File_OpenDSK.Click += mainMenuStrip_File_OpenDSK_Click;
            // 
            // mainMenuStrip_File_Save
            // 
            mainMenuStrip_File_Save.DropDownItems.AddRange(new ToolStripItem[] { currentDSKToolStripMenuItem, mainMenuStrip_File_Save_AsNew });
            mainMenuStrip_File_Save.ForeColor = Color.Black;
            mainMenuStrip_File_Save.Name = "mainMenuStrip_File_Save";
            mainMenuStrip_File_Save.Size = new Size(43, 20);
            mainMenuStrip_File_Save.Text = "Save";
            // 
            // currentDSKToolStripMenuItem
            // 
            currentDSKToolStripMenuItem.ForeColor = Color.White;
            currentDSKToolStripMenuItem.Name = "currentDSKToolStripMenuItem";
            currentDSKToolStripMenuItem.Size = new Size(135, 22);
            currentDSKToolStripMenuItem.Text = "Current File";
            // 
            // mainMenuStrip_File_Save_AsNew
            // 
            mainMenuStrip_File_Save_AsNew.ForeColor = Color.White;
            mainMenuStrip_File_Save_AsNew.Name = "mainMenuStrip_File_Save_AsNew";
            mainMenuStrip_File_Save_AsNew.Size = new Size(135, 22);
            mainMenuStrip_File_Save_AsNew.Text = "As New";
            mainMenuStrip_File_Save_AsNew.Click += mainMenuStrip_File_Save_AsNew_Click;
            // 
            // mainMenuStrip_Debug
            // 
            mainMenuStrip_Debug.DropDownItems.AddRange(new ToolStripItem[] { mainMenuStrip_Debug_Test });
            mainMenuStrip_Debug.ForeColor = Color.Black;
            mainMenuStrip_Debug.Name = "mainMenuStrip_Debug";
            mainMenuStrip_Debug.Size = new Size(54, 20);
            mainMenuStrip_Debug.Text = "Debug";
            // 
            // mainStatusStrip
            // 
            mainStatusStrip.GripStyle = ToolStripGripStyle.Visible;
            mainStatusStrip.Items.AddRange(new ToolStripItem[] { mainStatusStrip_Label_Status });
            mainStatusStrip.Location = new Point(3, 418);
            mainStatusStrip.Name = "mainStatusStrip";
            controlRenderer1.ColorTable = msColorTable1;
            controlRenderer1.RoundedEdges = true;
            mainStatusStrip.Renderer = controlRenderer1;
            mainStatusStrip.Size = new Size(589, 22);
            mainStatusStrip.SizingGrip = false;
            mainStatusStrip.TabIndex = 15;
            mainStatusStrip.Text = "formStatusStrip1";
            // 
            // mainStatusStrip_Label_Status
            // 
            mainStatusStrip_Label_Status.Name = "mainStatusStrip_Label_Status";
            mainStatusStrip_Label_Status.Size = new Size(45, 17);
            mainStatusStrip_Label_Status.Text = "STATUS";
            // 
            // mainTabControl
            // 
            mainTabControl.Dock = DockStyle.Fill;
            mainTabControl.Location = new Point(3, 48);
            mainTabControl.Name = "mainTabControl";
            mainTabControl.SelectedIndex = 0;
            mainTabControl.Size = new Size(589, 370);
            mainTabControl.TabIndex = 16;
            // 
            // mainMenuStrip_Debug_Test
            // 
            mainMenuStrip_Debug_Test.ForeColor = Color.Black;
            mainMenuStrip_Debug_Test.Name = "mainMenuStrip_Debug_Test";
            mainMenuStrip_Debug_Test.Size = new Size(180, 22);
            mainMenuStrip_Debug_Test.Text = "Test";
            mainMenuStrip_Debug_Test.Click += mainMenuStrip_Debug_Test_Click;
            // 
            // MainWindow
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(30, 30, 30);
            ClientSize = new Size(593, 442);
            Controls.Add(mainTabControl);
            Controls.Add(mainStatusStrip);
            Controls.Add(mainMenuStrip);
            FormStyle = ReaLTaiizor.Enum.Material.FormStyles.ActionBar_None;
            Margin = new Padding(2);
            Name = "MainWindow";
            Padding = new Padding(3, 24, 1, 2);
            Text = "RETROSTRIKE DSK EDITOR";
            mainMenuStrip.ResumeLayout(false);
            mainMenuStrip.PerformLayout();
            mainStatusStrip.ResumeLayout(false);
            mainStatusStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private ToolStripMenuItem mainMenuStrip_File;
        private ToolStripMenuItem mainMenuStrip_File_OpenDSK;
        private ToolStripMenuItem mainMenuStrip_File_Save;
        private ToolStripMenuItem currentDSKToolStripMenuItem;
        private ToolStripMenuItem mainMenuStrip_File_Save_AsNew;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel mainStatusStrip_Label_Status_old;
        private ContextMenuStrip FileOptionsContextMenu;
        private ReaLTaiizor.Controls.ParrotFlatMenuStrip mainMenuStrip;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ReaLTaiizor.Controls.MetroTabControl metroTabControl1;
        private TabPage tabPage3;
        private TabPage tabPage4;
        private TabPage tabPage7;
        private TabPage tabPage8;
        private ReaLTaiizor.Controls.FormStatusStrip mainStatusStrip;
        private ToolStripStatusLabel mainStatusStrip_Label_Status;
        private TabControl mainTabControl;
        private ToolStripMenuItem debugToolStripMenuItem;
        private ToolStripMenuItem mainMenuStrip_Debug;
        private ToolStripMenuItem mainMenuStrip_Debug_Test;
    }
}
