using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging.Effects;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using RetrostrikeDSKUI.Application;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8604 // Possible null reference argument.

using RetrostrikeDSKUI.Core;
using static System.Net.Mime.MediaTypeNames;
using ReaLTaiizor.Forms;
using RetroStrike.VirtualDisk;
using RetrostrikeDSKUI.RetroStrike;
using System.Formats.Tar;
namespace RetrostrikeDSKUI.Forms
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
            button_Import.Enabled = File.Exists(TargetImportFile);
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
            checkbox_ProcessKnownType.Checked = true;
        }
        #endregion

        #region Methods
        void CreateEventHandlers()
        {
            targetImportFileNameChanged = TagetImportFileNameChanged_EventHandler;
        }
        void GetAssetFileTypes()
        {
            assetTypes = new AssetTypeObj[RetroStrikeGlobals.HashResolver.TypesHashDict.Count];
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
            if (AppGlobals.ActiveDSK.CanAddFile(targetFileType, targetFileName))
            {
                if (checkbox_ProcessKnownType.Checked && DSKFile.SupportedProcessingTypes.ContainsKey(targetFileType))
                {
                    //If it's supported then we'll do as "Next"
                }
                else
                {
                    //Otherwise, we'll import it as raw
                    Stream xIn = File.Open(TargetImportFile, FileMode.Open, FileAccess.Read);
                    DSKFile.RFI newFile = null;
                    if (AppGlobals.ActiveDSK.AddFile(targetFileType, xIn, targetFileName, out newFile))
                    {
                        this.ImportSuccess = true;
                        this.WasFileImported = true;
                        this.Close();
                    }
                    else
                        MessageBox.Show("Uknown Error", "Import Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return;
            }
            else
            {
                MessageBox.Show("Import Error", "Cannot add file", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        void SetupImportedFileData()
        {

        }
        private void button_Cancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }
        #endregion

        private void checkbox_ProcessKnownType_CheckedChanged(object sender, EventArgs e)
        {
            var targetFileType = ((ComboBoxItemFileType)comboBox_AssetType.SelectedItem).Hash;

            if (checkbox_ProcessKnownType.Checked && DSKFile.SupportedProcessingTypes.ContainsKey(targetFileType))
            {
                //If it's supported then we'll do as "Next";
                button_Import.Text = "Next..";
            }
            else
                button_Import.Text = "Import";
        }

        private void comboBox_AssetType_SelectedIndexChanged(object sender, EventArgs e)
        {
            var box = (ComboBox)sender;
            var selIndex = box.SelectedIndex;
            ComboBoxItemFileType item = (ComboBoxItemFileType)box.Items[selIndex];
            checkbox_ProcessKnownType.Checked = DSKFile.SupportedProcessingTypes.ContainsKey(item.Hash);
        }
    }
}
