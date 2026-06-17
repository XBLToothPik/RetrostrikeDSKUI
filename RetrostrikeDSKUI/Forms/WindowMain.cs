using ReaLTaiizor.Controls;
using ReaLTaiizor.Forms;
using ReaLTaiizor.Manager;
using RetroStrike.Pbl;
using RetroStrike.Platform.XBox;
using RetroStrike.Utils;
using RetroStrike.VirtualDisk;
using RetrostrikeDSKUI.Application;
using RetrostrikeDSKUI.Core;
using RetrostrikeDSKUI.Forms;
using RetrostrikeDSKUI.Forms.ExportWindows;
using RetrostrikeDSKUI.RetroStrike;
using System.Diagnostics;
using System.Media;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;
using System.Xml.Schema;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
#pragma warning disable CS8622 // Nullability of reference types in type of parameter doesn't match the target delegate (possibly because of nullability attributes).
#pragma warning disable CS8604 // Possible null reference argument.
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.


namespace RetrostrikeDSKUI
{
    public partial class WindowMain : MaterialForm
    {
        #region Fields
        public const string MainWindowTitle = "RetroStrike DSK Editor";
        string currentDSKFileName = string.Empty;
        MaterialListView mainListView;
        Color ImportedColor = Color.FromArgb(200, 240, 200);  // light green
        Color ReplacedColor = Color.FromArgb(200, 225, 245);  // light blue
        Color RemovedColor = Color.FromArgb(245, 205, 205);  // light red
        #endregion

        #region CTORS
        public WindowMain()
        {

            ///
            /// 
            ///
            ///
            Initialize();
        }
        #endregion

        #region Initialization
        void Initialize()
        {
            InitializeComponent();
            CreateMainListView();
            SetMainListViewContextMenuItems();

            AppGlobals.InitGlobals();
            RetroStrikeGlobals.HashResolver = new HashNameResolver();
            using (Stream xIn = File.Open("knownfilenames.txt", FileMode.Open, FileAccess.Read))
                RetroStrikeGlobals.HashResolver.LoadHashDict(xIn, HashNameResolver.eHashDictType.FileNames);
            using (Stream xIn = File.Open("knowntypesdict.txt", FileMode.Open, FileAccess.Read))
                RetroStrikeGlobals.HashResolver.LoadHashDict(xIn, HashNameResolver.eHashDictType.FileTypes);
        }
        #endregion

        #region MainMenuStrip Menu Handlers
        private void mainMenuStrip_File_OpenDSK_Click(object sender, EventArgs e)
        {
            OpenFileDialog OFD = new OpenFileDialog();
            OFD.Title = "Choose DSK File...";
            OFD.Filter = "DSK Files|*.dsk|All Files|*.*";
            OFD.Multiselect = false;
            if (OFD.ShowDialog() == DialogResult.OK)
            {
                if (File.Exists(OFD.FileName))
                {
                    currentDSKFileName = OFD.FileName;
                    SetWindowTitleExtension(Utils.ShortenPath(OFD.FileName));
                    //TODO: Check if a DSK is already open and prompt about it
                    Stream xIn = File.Open(OFD.FileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                    OpenDSK(xIn);
                    PresentRFIInfo();
                }
            }
        }

        private void mainMenuStrip_File_Save_AsNew_Click(object sender, EventArgs e)
        {
            SaveFileDialog SFD = new SaveFileDialog();
            SFD.Title = "Choose Output DSK File Location...";
            SFD.Filter = "DSK Files|*.dsk|All Files|*.*";
            if (SFD.ShowDialog() == DialogResult.OK)
            {
                Stream xOut = File.Open(SFD.FileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
                xOut.Seek(0, SeekOrigin.Begin);
                xOut.SetLength(0);
                AppGlobals.ActiveDSK.WriteToNewFromCurrent(xOut);
                xOut.Flush();
                xOut.Close();

            }
        }
        private void mainMenuStrip_Debug_Test_Click(object sender, EventArgs e)
        {
            if (AppGlobals.ActiveDSK != null)
            {
                uint textureHash = Hashing.MakeFNV1A("texture");
                if (AppGlobals.ActiveDSK.DoesTypeExist(textureHash))
                {
                    var textureFiles = AppGlobals.ActiveDSK.Files[textureHash];
                    foreach (var texture in textureFiles)
                    {
                        using (MemoryStream xMem = new MemoryStream())
                        {
                            AppGlobals.ActiveDSK.CopyRFITo(texture, xMem);
                            xMem.Seek(0, SeekOrigin.Begin);

                            PblFile texPbl = new PblFile(AppGlobals.ActiveDSK, xMem);
                            texPbl.Read();
                            var texChunk = texPbl.RootChunk.GetChildByID("tex_");
                            if (texChunk != null)
                            {
                                RedTextureXBox xboxTexture = RedTextureXBox.CreateFromPBLChunk(texChunk);
                                var isDXT = xboxTexture.FormatIsDXT((uint)xboxTexture.TextureFormat);
                                var isSwizzled = xboxTexture.FormatIsSwizzled((uint)xboxTexture.TextureFormat);
                                var redTexFormat = xboxTexture.RedTextureType;
                                if (redTexFormat == RedTextureXBox.eRedTextureType.VOLUME)
                                {

                                }
                                break;
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #region FileOptions ContextMenuStrip Handlers
        private void FileOptionsContextMenu_Import_Click(object sender, EventArgs e)
        {
            OpenFileDialog OFD = new OpenFileDialog();
            OFD.Title = "Choose File To Import...";
            OFD.Filter = "|*.*";
            OFD.Multiselect = false;
            if (OFD.ShowDialog() == DialogResult.OK)
            {
                WindowImport window = new WindowImport(this.GetSelectedViewFileType(), OFD.FileName);
                window.ShowDialog();
                if (window.ImportSuccess && window.WasFileImported)
                {

                    PresentRFIInfo();

                    SystemSounds.Asterisk.Play();
                }
            }
        }
        private void FileOptionsContextMenu_Extract_RawData_Click(object sender, EventArgs e)
        {
            var filesToExtract = GetSelectedRFISFromListView();
            if (filesToExtract.Length > 0)
            {
                if (filesToExtract.Length > 1)
                    throw new NotImplementedException(); //if more than 1 then we will use a FolderBrowserDialog
                SaveFileDialog SFD = new SaveFileDialog();
                SFD.Title = filesToExtract.Length > 1
                    ? $"Export {filesToExtract.Length} Files Raw To Files..."
                    : "Export File To Raw File...";
                SFD.Filter = $"All Files|*.*"; //TODO: change extension based upon type
                if (SFD.ShowDialog() == DialogResult.OK)
                {
                    Stream xOut = File.Open(SFD.FileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
                    xOut.Seek(0, SeekOrigin.Begin);
                    xOut.SetLength(0);

                    AppGlobals.ActiveDSK.CopyRFITo(filesToExtract[0], xOut);

                    xOut.Flush();
                    xOut.Close();
                }
            }
        }
        private void FileOptionsContextMenu_Extract_AsType_Click(object? sender, EventArgs e)
        {
            var filesToExtract = GetSelectedRFISFromListView();
            if (filesToExtract.Length > 0)
            {
                if (filesToExtract.Length > 1)
                    throw new NotImplementedException();
                if (!ExportHelpers.IsTypeExportSupported(filesToExtract[0].GetActiveTypeHash()))
                    return;
                var exportWindowType = ExportHelpers.GetExportFormForSupportedType(filesToExtract[0].GetActiveTypeHash());
                var exportWindow = (Form)Activator.CreateInstance(exportWindowType, filesToExtract[0]);
                exportWindow.ShowDialog();
                if (((IExportWindow)exportWindow).ExportSucess)
                {
                    Debug.WriteLine("success");
                }
                else
                {
                    Debug.WriteLine("fail");
                }

                ///
                /// CONTINUE HERE
                /// TODO: Furnish the WindowExportTexture window and make it work.
                ///
                ///


                //MemoryStream xMem = new MemoryStream();
                //Globals.ActiveDSK.CopyRFITo(filesToExtract[0], xMem);
                //xMem.Seek(0, SeekOrigin.Begin);
                //PblFile testPBL = new PblFile(xMem);
                //testPBL.Read();
                //
                //PblChunk texChunk = testPBL.RootChunk.GetChildByID("tex_");
                //PblChunk nameChunk = texChunk.GetChildByID("NAME");
                //PblChunk bodyChunk = texChunk.GetChildByID("BODY");
                //string nameInChunk = nameChunk.GetDataAsString(Encoding.ASCII);
                //
                //RedTextureXBox texture = RedTextureXBox.CreateFromPBLChunk(texChunk);
                //string errors = string.Empty;
                //Stream xOut = File.Open("testdata.dat", FileMode.OpenOrCreate);
                //texture.Exp(xOut, out errors);
                //xOut.Close();
            }
        }
        private void FileOptionsContextMenu_Replace_Click(object sender, EventArgs e)
        {
            var filesToReplace = GetSelectedRFISFromListView();
            if (filesToReplace.Length > 0)
            {
                if (filesToReplace.Length > 1)
                    throw new NotImplementedException();
                OpenFileDialog OFD = new OpenFileDialog();
                OFD.Title = $"Replace {RetroStrikeGlobals.HashResolver.ResolveHash(filesToReplace[0].NameHashOriginal, HashNameResolver.eHashTypeSelector.All)}...";
                OFD.Filter = "|*.*";
                OFD.Multiselect = false;
                if (OFD.ShowDialog() == DialogResult.OK)
                {
                    Stream xIn = File.Open(OFD.FileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                    string errors = string.Empty;
                    if (!AppGlobals.ActiveDSK.ReplaceRFIWithStream(filesToReplace[0], xIn, out errors))
                    {
                        MessageBox.Show(errors, "Import Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        private void FileOptionsContextMenu_CancelReplace_Click(object? sender, EventArgs e)
        {
            var filesToCancelReplacementOf = GetSelectedRFISFromListView();
            if (filesToCancelReplacementOf.Length > 0)
            {
                if (filesToCancelReplacementOf.Length > 1)
                    throw new NotImplementedException();
                if (MessageBox.Show("Are you sure you want to cancel the replacement of this file?", "Cancel Replacement", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string errors = string.Empty;
                    if (AppGlobals.ActiveDSK.CancelRFIReplace(filesToCancelReplacementOf[0], out errors))
                    {
                        SystemSounds.Asterisk.Play();
                    }
                    else
                    {
                        MessageBox.Show($"There was an error cancelling the replacement: \r\n{errors}", "Error Cancelling Replacement", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                }
            }
        }

        private void FileOptionsContextMenu_Remove_Click(object? sender, EventArgs e)
        {
            var filesToRemove = GetSelectedRFISFromListView();
            if (filesToRemove.Length > 0)
            {
                if (filesToRemove.Length > 1)
                    throw new NotImplementedException();
                if (MessageBox.Show("Are you sure you want to remove this file?", $"Remove File", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    filesToRemove[0].IsBeingRemoved = true;
                    mainListView.Update();
                }
            }
        }
        private void FileOptionsContextMenu_CancelImport_Click(object? sender, EventArgs e)
        {
            var filesToCancelImportOf = GetSelectedRFISFromListView();
            if (filesToCancelImportOf.Length > 0)
            {
                if (filesToCancelImportOf.Length > 1)
                    throw new NotImplementedException();
                if (MessageBox.Show("Are you sure you want to cancel this import?", "Confirm Import Cancellation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string errors = string.Empty;
                    if (AppGlobals.ActiveDSK.CancelImportOfFile(filesToCancelImportOf[0], out errors))
                    {
                        SetMainListViewActiveFileType(GetSelectedViewFileType());
                        SystemSounds.Asterisk.Play();
                    }
                    else
                        MessageBox.Show($"Could not cancel import:\r\n{errors}", "Import Cancel Failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void FileOptionsContextMenu_CancelRemove_Click(object? sender, EventArgs e)
        {
            var filesToCancelRemoveOf = GetSelectedRFISFromListView();
            if (filesToCancelRemoveOf.Length > 0)
            {
                if (filesToCancelRemoveOf.Length > 1)
                    throw new NotImplementedException();
                string errors = string.Empty;
                if (AppGlobals.ActiveDSK.CancelRemovalOfFile(filesToCancelRemoveOf[0], out errors))
                {
                    SystemSounds.Asterisk.Play();
                }
                else
                {
                    MessageBox.Show($"Error Cancelling File Removal:\n{errors}", "Error Cancelling File Removal", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

#if DEBUG
        private void FileOptionsContextMenu_DebugOption_Click(object? sender, EventArgs e)
        {
            var filesToView = GetSelectedRFISFromListView();
            if (filesToView.Length > 0 && filesToView.Length < 2)
            {
                var targetRFI = filesToView[0];
                StringBuilder tBuilt = new StringBuilder();
                tBuilt.AppendLine($"Active Name: {RetroStrikeGlobals.HashResolver.ResolveHash(targetRFI.GetActiveNameHash(), HashNameResolver.eHashTypeSelector.FileNames)}");
                tBuilt.AppendLine();
                tBuilt.AppendLine($"IsBeingReplaced:\t{targetRFI.IsBeingReplaced}");
                tBuilt.AppendLine($"IsBeingRemoved:\t{targetRFI.IsBeingRemoved}");
                tBuilt.AppendLine($"IsBeingImported:\t{targetRFI.IsNewImportedFile}");

                tBuilt.AppendLine();

                tBuilt.AppendLine($"ORIGINAL TYPE HASH:\t0x{targetRFI.FileTypeOriginal.ToString("X8").ToUpper()}");
                tBuilt.AppendLine($"ORIGINAL NAME HASH:\t0x{targetRFI.NameHashOriginal.ToString("X8").ToUpper()}");
                tBuilt.AppendLine($"ORIGINAL FILE SIZE:\t{targetRFI.FileSizeOriginal}");

                string newTypeHashStr = "*INVALID*";
                string newName = "*INVALID*";
                string newFileSize = "*INVALID*";

                if (targetRFI.IsNewImportedFileOrReplaced)
                    newFileSize = targetRFI.GetActiveFileSize().ToString();

                if (targetRFI.NewIncomingTypeHash != null)
                    newTypeHashStr = $"0x{targetRFI.NewIncomingTypeHash.Value.ToString("X8").ToUpper()}";
                if (!string.IsNullOrEmpty(targetRFI.NewIncomingFileName))
                    newName = targetRFI.NewIncomingFileName;

                tBuilt.AppendLine();

                tBuilt.AppendLine($"NEW TYPE HASH:\t{newTypeHashStr}");
                tBuilt.AppendLine($"NEW NAME:\t{newName}");
                tBuilt.AppendLine($"NEW FILE SIZE:\t{newFileSize}");
                MessageBox.Show(tBuilt.ToString(), "Dbg");
            }
        }
#endif
        private void FileOptionsContextMenu_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SetMainListViewContextMenuItems();
        }
        #endregion

        #region MainTabControl Events
        private void mainTabControl_Selected(object sender, TabControlEventArgs e)
        {
            if (e.TabPage?.Tag != null && e.TabPage?.Tag is uint)
            {
                SetMainListViewActiveFileType((uint)e.TabPage.Tag);
            }
        }
        #endregion

        #region MainListView Events
        private void MainListView_RetrieveVirtualItem(object? sender, RetrieveVirtualItemEventArgs e)
        {
            //If we want to have more columns (info for individual types) later we should create a function to Create a ListViewItem for a certain type,
            //  and then another function to populate the listview item with info.
            if (AppGlobals.ActiveDSK.Files.ContainsKey(GetSelectedViewFileType()))
            {

                var targettedFiles = AppGlobals.ActiveDSK.Files[GetSelectedViewFileType()];
                if (e.ItemIndex < 0 || e.ItemIndex >= targettedFiles.Count)
                {
#if DEBUG
                    //This happens because the UI repaint tries to trigger a RetrieveVirtualItem in the middle of a changeover, so it will not have the correct sizes. 
                    //  Not sure if there's a way to fix this but I'll look at it more eventually.
                    Debug.WriteLine($"{nameof(WindowMain)}::{nameof(MainListView_RetrieveVirtualItem)} HIT FAILSAFE (len: {targettedFiles.Count}) (req: {e.ItemIndex})");
#endif
                    //Fail-safe for UI painting in the middle of a changeover which triggers a request for virtualitems.
                    e.Item = new ListViewItem(string.Empty);
                    e.Item.SubItems.Add("");
                    e.Item.SubItems.Add("");
                    return;
                }
                if (targettedFiles.Count > 0)
                {
                    var targetItem = targettedFiles[e.ItemIndex];
                    var item = new ListViewItem();

                    item.Text = RetroStrikeGlobals.HashResolver.ResolveHash(targettedFiles[e.ItemIndex].GetActiveNameHash(), HashNameResolver.eHashTypeSelector.All);
                    item.SubItems.Add(RetroStrikeGlobals.HashResolver.ResolveHash(targetItem.GetActiveTypeHash(), HashNameResolver.eHashTypeSelector.FileTypes));
                    item.SubItems.Add($"{targetItem.GetActiveFileSize()}");

                    e.Item = item;
                    e.Item.BackColor =
                        targetItem.IsBeingReplaced
                        ? ReplacedColor
                        : targetItem.IsNewImportedFile
                        ? ImportedColor
                        : targetItem.IsBeingRemoved
                        ? RemovedColor
                        : SystemColors.Window;
                }
            }
        }
        private void MainListView_Resize(object? sender, EventArgs e)
        {
            ResizeColumns();
        }
        private readonly float[] columnRatios = { 0.5f, 0.3f, 0.2f };

        private void ResizeColumns()
        {
            SendMessage(mainListView.Handle, WM_SETREDRAW, false, 0);
            try
            {
                int width = mainListView.ClientSize.Width - SystemInformation.VerticalScrollBarWidth;
                int used = 0;
                for (int i = 0; i < mainListView.Columns.Count - 1; i++)
                {
                    int w = (int)(width * columnRatios[i]);
                    mainListView.Columns[i].Width = w;
                    used += w;
                }
                mainListView.Columns[mainListView.Columns.Count - 1].Width = width - used;
            }
            finally
            {
                SendMessage(mainListView.Handle, WM_SETREDRAW, true, 0);
                mainListView.Invalidate();
            }
        }


        #endregion
        
        #region Methods
        void SetWindowTitleExtension(string titleExtension)
        {
            Text = $"{MainWindowTitle} - {titleExtension}";
        }
        void CreateMainListView()
        {
            //Create the listview
            mainListView = new ReaLTaiizor.Controls.MaterialListView();
            mainListView.Name = "MainListView";
            mainListView.Dock = DockStyle.Fill;
            mainListView.VirtualMode = true;
            mainListView.FullRowSelect = true;
            mainListView.MultiSelect = false;
            mainListView.View = View.Details;

            mainListView.Alignment = ListViewAlignment.Left;
            mainListView.ContextMenuStrip = FileOptionsContextMenu;
            mainListView.RetrieveVirtualItem += MainListView_RetrieveVirtualItem;
            mainListView.Resize += MainListView_Resize;
            //Create the columns

            ColumnHeader col_FileName = new ColumnHeader();
            col_FileName.Name = $"{mainListView.Name}_col_fileName";
            col_FileName.Text = "File Name";
            col_FileName.Width = 120;
            col_FileName.TextAlign = HorizontalAlignment.Left;

            ColumnHeader col_FileType = new ColumnHeader();
            col_FileType.Name = $"{mainListView.Name}_col_fileType";
            col_FileType.Text = "File Type";
            col_FileType.Width = 120;
            col_FileType.TextAlign = HorizontalAlignment.Left;

            ColumnHeader col_FileSize = new ColumnHeader();
            col_FileSize.Name = $"{mainListView.Name}_col_fileSize";
            col_FileSize.Text = "Size";
            col_FileSize.Width = 120;
            col_FileSize.TextAlign = HorizontalAlignment.Left;

            //Add them
            mainListView.Columns.AddRange(col_FileName, col_FileType, col_FileSize);
            mainListView.Update();

        }

        void OpenDSK(Stream xIn)
        {
            AppGlobals.ActiveDSK = new DSKFile(xIn);
            AppGlobals.ActiveDSK.Read();
        }
        void PresentRFIInfo()
        {
            //This function creates the tab controls for each type in the DSK
            var activelySelectedType = GetSelectedViewFileType();
            mainTabControl.Selected -= this.mainTabControl_Selected;
            mainTabControl.TabPages.Clear();
            foreach (var pair in AppGlobals.ActiveDSK.Files)
            {
                if (pair.Value.Count > 0)
                {
                    var typeName = RetroStrikeGlobals.HashResolver.ResolveHash(pair.Key, HashNameResolver.eHashTypeSelector.All);
                    System.Windows.Forms.TabPage newTab = new System.Windows.Forms.TabPage();

                    newTab.Name = $"mainTabControl_tab_{typeName}";
                    newTab.Text = typeName;
                    newTab.Tag = pair.Key;
                    //newTab.BackColor = Color.FromArgb(255, 30, 30, 30);
                    mainTabControl.TabPages.Add(newTab);
                }
            }
            mainTabControl.Selected += this.mainTabControl_Selected;

            if (AppGlobals.ActiveDSK.Files.Count > 0)
            {
                if (AppGlobals.ActiveDSK.DoesTypeExist(activelySelectedType))
                    SetMainListViewActiveFileType(activelySelectedType);
                else
                    SetMainListViewActiveFileType(AppGlobals.ActiveDSK.Files.FirstOrDefault().Key);
            }
        }

        void SetMainListViewActiveFileType(uint fileTypeHash)
        {
            //This function sets the mainListView's parent to the tab who's fileTypeHash matches the parameter
            ValidateSelectedTabType();
            foreach (System.Windows.Forms.TabPage tabPage in mainTabControl.TabPages)
            {
                if (tabPage.Tag != null && ((uint)tabPage.Tag) == fileTypeHash)
                {
                    if (mainTabControl.SelectedTab != tabPage)
                        mainTabControl.SelectedTab = tabPage;
                    SetMainListViewVirtualListSize(AppGlobals.ActiveDSK.Files[fileTypeHash].Count);
                    if (mainListView.Parent != null)
                        mainListView.Parent.Controls.Remove(mainListView);
                    tabPage.Controls.Add(mainListView);
                    break;
                }
            }
        }
        void SetMainListViewFileType(string typeName)
        {
            SetMainListViewActiveFileType(Hashing.MakeFNV1A(typeName));
        }
        void SetMainListViewVirtualListSize(int size)
        {
            mainListView.VirtualListSize = size;
#if DEBUG
            Debug.WriteLine($"{nameof(WindowMain)}::{nameof(SetMainListViewVirtualListSize)} Setting VirtualListSize to {size}");
#endif
        }
        void ValidateSelectedTabType()
        {
            if (mainTabControl.SelectedTab != null && mainTabControl.SelectedTab.Tag != null)
            {
                uint tabFileType = (uint)mainTabControl.SelectedTab.Tag;
                if (!AppGlobals.ActiveDSK.DoesTypeExist(tabFileType))
                {
                    SetMainListViewVirtualListSize(0);
                    mainTabControl.TabPages.Remove(mainTabControl.SelectedTab);
                    SetMainListViewVirtualListSize(0);
                    mainTabControl.SelectedTab = mainTabControl.TabPages[mainTabControl.TabPages.Count - 1];
                }
            }
        }
        void SetMainListViewContextMenuItems()
        {
            FileOptionsContextMenu.Items.Clear();
            var selectedRFIs = GetSelectedRFISFromListView();
            int numSelectedRFIs = selectedRFIs.Length;
            bool areAnySelectedRFIsBeingReplaced = selectedRFIs.Any(rfi => rfi.IsBeingReplaced);
            bool areAnySelectedRFIsBeingImported = selectedRFIs.Any(rfi => rfi.IsNewImportedFile);
            bool areAnySelectedRFIsBeingRemoved = selectedRFIs.Any(rfi => rfi.IsBeingRemoved);

            bool showReplaceOrExtract = (numSelectedRFIs > 0 && numSelectedRFIs < 2)
                                        && !areAnySelectedRFIsBeingReplaced && !areAnySelectedRFIsBeingImported;
            bool showRemove = (numSelectedRFIs > 0 && numSelectedRFIs < 2)
                                        && !areAnySelectedRFIsBeingRemoved && !areAnySelectedRFIsBeingImported && !areAnySelectedRFIsBeingReplaced;
            bool showCancelReplace = (numSelectedRFIs > 0 && numSelectedRFIs < 2)
                                        && areAnySelectedRFIsBeingReplaced;
            bool showCancelImport = (numSelectedRFIs > 0 && numSelectedRFIs < 2)
                                        && areAnySelectedRFIsBeingImported;
            bool showCancelRemove = (numSelectedRFIs > 0 && numSelectedRFIs < 2)
                                        && areAnySelectedRFIsBeingRemoved;
#if DEBUG
            bool showDebugOption = (numSelectedRFIs > 0 && numSelectedRFIs < 2);
#endif

            //Import & subitems
            ToolStripMenuItem option_Import = new ToolStripMenuItem();
            option_Import.Name = "FileOptionsContextMenu_Import";
            option_Import.Text = "Import";
            option_Import.Click += FileOptionsContextMenu_Import_Click;

            ToolStripSeparator seperator = new ToolStripSeparator();
            seperator.Name = "FileOptionsContextMenu_Seperator1";

            //Cancel Import & subitems
            ToolStripMenuItem option_CancelImport = new ToolStripMenuItem();
            option_CancelImport.Name = "FileOptionsContextMenu_CancelImport";
            option_CancelImport.Text = "Cancel Import";
            option_CancelImport.Click += FileOptionsContextMenu_CancelImport_Click;

            //Extraction & subitems
            ToolStripMenuItem option_Extract = new ToolStripMenuItem();
            option_Extract.Name = "FileOptionsContextMenu_Extract";
            option_Extract.Text = "Extract";

            ToolStripMenuItem option_Extract_RawData = new ToolStripMenuItem();
            option_Extract_RawData.Name = "FileOptionsContextMenu_Extract_RawData";
            option_Extract_RawData.Text = "Raw Data";
            option_Extract_RawData.Click += FileOptionsContextMenu_Extract_RawData_Click;

            ToolStripMenuItem option_Extract_AsType = new ToolStripMenuItem();
            option_Extract_AsType.Name = "FileOptionsContextMenu_Extract_AsType";
            option_Extract_AsType.Text = "As Type";
            option_Extract_AsType.Click += FileOptionsContextMenu_Extract_AsType_Click;

            option_Extract.DropDownItems.Add(option_Extract_RawData);
            option_Extract.DropDownItems.Add(option_Extract_AsType);

            //Replacement & subitems
            ToolStripMenuItem option_Replace = new ToolStripMenuItem();
            option_Replace.Name = "FileOptionsContextMenu_Replace";
            option_Replace.Text = "Replace";
            option_Replace.Click += this.FileOptionsContextMenu_Replace_Click;

            //Remove & subitems
            ToolStripMenuItem option_Remove = new ToolStripMenuItem();
            option_Remove.Name = "FileOptionsContextMenu_Remove";
            option_Remove.Text = "Remove";
            option_Remove.Click += FileOptionsContextMenu_Remove_Click;


            //Cancel Replace & subitems
            ToolStripMenuItem option_CancelReplace = new ToolStripMenuItem();
            option_CancelReplace.Name = "FileOptionsContextMenu_CancelReplace";
            option_CancelReplace.Text = "Cancel Replace";
            option_CancelReplace.Click += FileOptionsContextMenu_CancelReplace_Click;

            //Cancel Remove & subitems
            ToolStripMenuItem option_CancelRemove = new ToolStripMenuItem();
            option_CancelRemove.Name = "FileOptionsContextMenu_CancelRemove";
            option_CancelRemove.Text = "Cancel Remove";
            option_CancelRemove.Click += FileOptionsContextMenu_CancelRemove_Click;

#if DEBUG
            //Debug Items
            ToolStripMenuItem option_DebugOption = new ToolStripMenuItem();
            option_DebugOption.Name = "FileOptionsContextMenu_DebugOption";
            option_DebugOption.Text = "Debug";
            option_DebugOption.Click += FileOptionsContextMenu_DebugOption_Click;
#endif

            FileOptionsContextMenu.Items.Add(option_Import);

            if (showReplaceOrExtract && !showCancelRemove)
            {
                FileOptionsContextMenu.Items.Add(seperator);
                FileOptionsContextMenu.Items.Add(option_Extract);
                FileOptionsContextMenu.Items.Add(option_Replace);
            }

            if (showCancelReplace)
            {
                FileOptionsContextMenu.Items.Add(option_CancelReplace);

            }

            if (showCancelRemove)
            {
                FileOptionsContextMenu.Items.Add(seperator);
                FileOptionsContextMenu.Items.Add(option_CancelRemove);
            }


            if (showRemove)
            {
                FileOptionsContextMenu.Items.Add(seperator);
                FileOptionsContextMenu.Items.Add(option_Remove);
            }

            if (showCancelImport)
            {
                FileOptionsContextMenu.Items.Add(seperator);
                FileOptionsContextMenu.Items.Add(option_CancelImport);
            }

#if DEBUG
            if (showDebugOption)
            {
                FileOptionsContextMenu.Items.Add(seperator);
                FileOptionsContextMenu.Items.Add(option_DebugOption);
            }
#endif
        }



        DSKFile.RFI[] GetSelectedRFISFromListView()
        {
            if (AppGlobals.ActiveDSK == null)
                return new DSKFile.RFI[0];
            var _currentFiles = AppGlobals.ActiveDSK.Files[GetSelectedViewFileType()];
            var indices = mainListView.SelectedIndices;
            DSKFile.RFI[] toRet = new DSKFile.RFI[indices.Count];
            for (int i = 0; i < indices.Count; i++)
                toRet[i] = _currentFiles[indices[i]];
            return toRet;
        }
        public uint GetSelectedViewFileType()
        {
            if (mainTabControl.SelectedTab != null)
            {
                if (mainTabControl.SelectedTab.Tag != null)
                {
                    return (uint)mainTabControl.SelectedTab.Tag;
                }
            }
            return 0;
        }
        #endregion

        #region Win32
        private const int WM_SETREDRAW = 0x000B;
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int msg, bool wParam, int lParam);
        #endregion
    }
}
