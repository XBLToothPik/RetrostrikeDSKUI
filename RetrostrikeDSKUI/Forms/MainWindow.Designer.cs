namespace RetrostrikeDSKUI
{
    partial class MainWindow
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
            mainMenuStrip = new MenuStrip();
            mainMenuStrip_File = new ToolStripMenuItem();
            mainMenuStrip_File_OpenDSK = new ToolStripMenuItem();
            mainMenuStrip_File_Save = new ToolStripMenuItem();
            currentDSKToolStripMenuItem = new ToolStripMenuItem();
            mainMenuStrip_File_Save_AsNew = new ToolStripMenuItem();
            mainTabControl = new TabControl();
            FileOptionsContextMenu = new ContextMenuStrip(components);
            mainStatusStrip = new StatusStrip();
            mainStatusStrip_Label_Status = new ToolStripStatusLabel();
            mainMenuStrip.SuspendLayout();
            mainStatusStrip.SuspendLayout();
            SuspendLayout();
            // 
            // mainMenuStrip
            // 
            mainMenuStrip.Items.AddRange(new ToolStripItem[] { mainMenuStrip_File });
            mainMenuStrip.Location = new Point(0, 0);
            mainMenuStrip.Name = "mainMenuStrip";
            mainMenuStrip.Size = new Size(847, 24);
            mainMenuStrip.TabIndex = 1;
            mainMenuStrip.Text = "menuStrip1";
            // 
            // mainMenuStrip_File
            // 
            mainMenuStrip_File.DropDownItems.AddRange(new ToolStripItem[] { mainMenuStrip_File_OpenDSK, mainMenuStrip_File_Save });
            mainMenuStrip_File.Name = "mainMenuStrip_File";
            mainMenuStrip_File.Size = new Size(37, 20);
            mainMenuStrip_File.Text = "File";
            // 
            // mainMenuStrip_File_OpenDSK
            // 
            mainMenuStrip_File_OpenDSK.Name = "mainMenuStrip_File_OpenDSK";
            mainMenuStrip_File_OpenDSK.Size = new Size(127, 22);
            mainMenuStrip_File_OpenDSK.Text = "Open DSK";
            mainMenuStrip_File_OpenDSK.Click += mainMenuStrip_File_OpenDSK_Click;
            // 
            // mainMenuStrip_File_Save
            // 
            mainMenuStrip_File_Save.DropDownItems.AddRange(new ToolStripItem[] { currentDSKToolStripMenuItem, mainMenuStrip_File_Save_AsNew });
            mainMenuStrip_File_Save.Name = "mainMenuStrip_File_Save";
            mainMenuStrip_File_Save.Size = new Size(127, 22);
            mainMenuStrip_File_Save.Text = "Save";
            // 
            // currentDSKToolStripMenuItem
            // 
            currentDSKToolStripMenuItem.Name = "currentDSKToolStripMenuItem";
            currentDSKToolStripMenuItem.Size = new Size(138, 22);
            currentDSKToolStripMenuItem.Text = "Current DSK";
            // 
            // mainMenuStrip_File_Save_AsNew
            // 
            mainMenuStrip_File_Save_AsNew.Name = "mainMenuStrip_File_Save_AsNew";
            mainMenuStrip_File_Save_AsNew.Size = new Size(138, 22);
            mainMenuStrip_File_Save_AsNew.Text = "As New";
            mainMenuStrip_File_Save_AsNew.Click += mainMenuStrip_File_Save_AsNew_Click;
            // 
            // mainTabControl
            // 
            mainTabControl.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            mainTabControl.Location = new Point(0, 24);
            mainTabControl.Name = "mainTabControl";
            mainTabControl.SelectedIndex = 0;
            mainTabControl.Size = new Size(847, 477);
            mainTabControl.TabIndex = 3;
            mainTabControl.Selected += mainTabControl_Selected;
            // 
            // FileOptionsContextMenu
            // 
            FileOptionsContextMenu.Name = "FileOptionsContextMenu";
            FileOptionsContextMenu.Size = new Size(61, 4);
            FileOptionsContextMenu.Opening += FileOptionsContextMenu_Opening;
            // 
            // mainStatusStrip
            // 
            mainStatusStrip.Items.AddRange(new ToolStripItem[] { mainStatusStrip_Label_Status });
            mainStatusStrip.Location = new Point(0, 504);
            mainStatusStrip.Name = "mainStatusStrip";
            mainStatusStrip.Size = new Size(847, 22);
            mainStatusStrip.TabIndex = 4;
            mainStatusStrip.Text = "statusStrip2";
            // 
            // mainStatusStrip_Label_Status
            // 
            mainStatusStrip_Label_Status.Name = "mainStatusStrip_Label_Status";
            mainStatusStrip_Label_Status.Size = new Size(45, 17);
            mainStatusStrip_Label_Status.Text = "STATUS";
            // 
            // MainWindow
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(847, 526);
            Controls.Add(mainStatusStrip);
            Controls.Add(mainTabControl);
            Controls.Add(mainMenuStrip);
            MainMenuStrip = mainMenuStrip;
            Name = "MainWindow";
            Text = "RetroStrike DSK Editor";
            mainMenuStrip.ResumeLayout(false);
            mainMenuStrip.PerformLayout();
            mainStatusStrip.ResumeLayout(false);
            mainStatusStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private MenuStrip mainMenuStrip;
        private ToolStripMenuItem mainMenuStrip_File;
        private ToolStripMenuItem mainMenuStrip_File_OpenDSK;
        private TabControl mainTabControl;
        private StatusStrip statusStrip1;
        private StatusStrip mainStatusStrip;
        private ToolStripStatusLabel mainStatusStrip_Label_Status;
        private ToolStripMenuItem mainMenuStrip_File_Save;
        private ToolStripMenuItem currentDSKToolStripMenuItem;
        private ToolStripMenuItem mainMenuStrip_File_Save_AsNew;
        private ContextMenuStrip FileOptionsContextMenu;
    }
}
