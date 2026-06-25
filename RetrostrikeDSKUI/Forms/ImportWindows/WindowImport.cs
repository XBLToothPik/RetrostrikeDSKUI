using RetroStrike;
using RetrostrikeDSKUI.Core;
using RetrostrikeDSKUI.Application;
using ReaLTaiizor.Forms;
using RetroStrike.VirtualDisk;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8604 // Possible null reference argument.

namespace RetrostrikeDSKUI.Forms.ImportWindows
{
    public partial class WindowImport : MaterialForm
    {
        #region Classes
        class ComboBoxItemFileType
        {
            public uint Hash { get; private set; }
            public string Name { get; private set; }

            public ComboBoxItemFileType(uint hash, string name)
            {
                this.Hash = hash;
                this.Name = name;
            }
            public override string ToString()
            {
                return Name;
            }
        }
        #endregion

        #region Fields
        string? _targetImportFileName;
        uint _selectedFileType;
        EventHandler targetImportFileNameChanged;

        AssetTypeObj[] assetTypes;
        #endregion

        #region Properties
        public string? TargetImportFile
        {
            get
            {
                return _targetImportFileName;
            }
            private set
            {
                _targetImportFileName = value;
                targetImportFileNameChanged(this, null);
            }
        }
        public bool ImportSuccess { get; private set; } = true;
        public bool WasFileImported { get; private set; } = false;

        #endregion

        #region Event Handlers
        void TagetImportFileNameChanged_EventHandler(object sender, EventArgs e)
        {
            buttonImport.Enabled = File.Exists(TargetImportFile);
        }
        #endregion

        #region Classes
        class AssetTypeObj
        {
            public string FriendlyName;
            public uint Hash;
        }
        #endregion

        #region CTORS
        public WindowImport(uint currentFileTypeSelected, string? targetFileName = null)
        {
            InitializeComponent();

            CreateEventHandlers();
            this._targetImportFileName = targetFileName;
            this._selectedFileType = currentFileTypeSelected;
            GetAssetFileTypes();
            PopulateView();
        }
        #endregion

        #region Methods
        void CreateEventHandlers()
        {
            targetImportFileNameChanged = TagetImportFileNameChanged_EventHandler;
        }
        void GetAssetFileTypes()
        {
            assetTypes = new AssetTypeObj[RetroStrike.RetroStrikeGlobals.HashResolver.TypesHashDict.Count];
            for (int i = 0; i < assetTypes.Length; i++)
            {
                assetTypes[i] = new AssetTypeObj()
                {
                    FriendlyName = RetroStrikeGlobals.HashResolver.TypesHashDict.ElementAt(i).Value,
                    Hash = RetroStrikeGlobals.HashResolver.TypesHashDict.ElementAt(i).Key
                };
            }
        }
        void PopulateView()
        {
            if (!string.IsNullOrEmpty(_targetImportFileName))
                textbox_ImportFileName.Text = Utils.ShortenPath(_targetImportFileName, 60);
            comboBox_AssetType.BeginUpdate();
            comboBox_AssetType.Items.Clear();
            foreach (var curAsset in assetTypes)
            {
                ComboBoxItemFileType item = new ComboBoxItemFileType(curAsset.Hash, RetroStrikeGlobals.HashResolver.ResolveHash(curAsset.Hash, RetroStrike.HashNameResolver.eHashTypeSelector.FileTypes));
                comboBox_AssetType.Items.Add(item);
            }
            for (int i = 0; i < comboBox_AssetType.Items.Count; i++)
            {
                if (((ComboBoxItemFileType)comboBox_AssetType.Items[i]).Hash == _selectedFileType)
                {
                    comboBox_AssetType.SelectedIndex = i;
                    break;
                }
            }
            comboBox_AssetType.EndUpdate();
        }
        #endregion

        #region Button Handlers
        private void button_Import_Click(object sender, EventArgs e)
        {
            var targetFileType = ((ComboBoxItemFileType)comboBox_AssetType.SelectedItem).Hash;
            var targetFileName = System.IO.Path.GetFileNameWithoutExtension(this.TargetImportFile);


            //TODO: Work on importing files from here.
            //TODO: Work on Processing known file types for import/replace
            //TODO: Work on better error messages (like adding "errors" to CanAddFile and AddFile)
            string addFileFailReason = string.Empty;
            if (AppGlobals.ActiveDSK.CanAddFile(targetFileType, targetFileName, out addFileFailReason))
            {
                if (checkbox_ProcessKnownType.Checked && DSKFile.SupportedProcessingTypes.ContainsKey(targetFileType))
                {
                    //If it's supported then we'll do as "Next" amd open up the corresponding type import window.
                    var typeImportWin = GetSupportedImportTypeForm(targetFileType);
                    Form importWin = (Form)Activator.CreateInstance(typeImportWin);
                    ((ITypeImportWindow)importWin).CustomData.Add("filename", this.TargetImportFile);
                    ((ITypeImportWindow)importWin).LoadData();
                    ((ITypeImportWindow)importWin).LoadDataIntoView();
                    this.Opacity = 0.0f;
                    this.Enabled = false;
                    importWin.StartPosition = FormStartPosition.CenterParent;
                    importWin.ShowDialog();
                    if (((ITypeImportWindow)importWin).ImportSuccess)
                    {
                        this.ImportSuccess = true;
                        this.WasFileImported = true;
                        this.Close();
                        return;
                    }
                    else
                    {
                        this.Opacity = 1.0f;
                        this.Enabled = true;
                    }
                }
                else
                {
                    //Otherwise, we'll import it as raw
                    Stream xIn = File.Open(TargetImportFile, FileMode.Open, FileAccess.Read);
                    DSKFile.RFI newFile = null;
                    if (AppGlobals.ActiveDSK.AddFile(targetFileType, xIn, targetFileName, out newFile, out addFileFailReason))
                    {
                        this.ImportSuccess = true;
                        this.WasFileImported = true;
                        this.Close();
                    }
                }
                return;
            }

            if (!string.IsNullOrEmpty(addFileFailReason))
                MessageBox.Show(addFileFailReason, "Import Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            
        }
        private void button_Cancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }
        #endregion

        #region Event
        private void checkbox_ProcessKnownType_CheckedChanged(object sender, EventArgs e)
        {
            var targetFileType = ((ComboBoxItemFileType)comboBox_AssetType.SelectedItem).Hash;

            if (checkbox_ProcessKnownType.Checked && DSKFile.SupportedProcessingTypes.ContainsKey(targetFileType))
            {
                //If it's supported then we'll do as "Next";
                buttonImport.Text = "Next";
            }
            else
                buttonImport.Text = "Import";
        }

        private void comboBox_AssetType_SelectedIndexChanged(object sender, EventArgs e)
        {
            var box = (ComboBox)sender;
            var selIndex = box.SelectedIndex;
            ComboBoxItemFileType item = (ComboBoxItemFileType)box.Items[selIndex];
            checkbox_ProcessKnownType.Checked = checkbox_ProcessKnownType.Enabled = DSKFile.SupportedProcessingTypes.ContainsKey(item.Hash);
            checkbox_ProcessKnownType_CheckedChanged(checkbox_ProcessKnownType_CheckedChanged, null); //little hacky but
        }
        #endregion

        #region Methods
        Type GetSupportedImportTypeForm(uint typeHash)
        {
            if (DSKFile.SupportedProcessingTypes.ContainsKey(typeHash))
            {
                switch (DSKFile.SupportedProcessingTypes[typeHash])
                {
                    case "texture":
                        return typeof(WindowImportTexture);
                }
            }
            return null;
        }
        #endregion

    }
}
