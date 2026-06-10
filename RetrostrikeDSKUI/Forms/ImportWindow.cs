using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging.Effects;
using System.Text;
using System.Windows.Forms;
using RetrostrikeDSKUI.Application;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8604 // Possible null reference argument.

using RetrostrikeDSKUI.Core;
using static System.Net.Mime.MediaTypeNames;
namespace RetrostrikeDSKUI.Forms
{
    public partial class ImportWindow : Form
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
        public ImportWindow(uint currentFileTypeSelected, string? targetFileName = null)
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
            assetTypes = new AssetTypeObj[Globals.HashResolver.TypesHashDict.Count];
            for (int i = 0; i < assetTypes.Length; i++)
            {
                assetTypes[i] = new AssetTypeObj()
                {
                    FriendlyName = Globals.HashResolver.TypesHashDict.ElementAt(i).Value,
                    Hash = Globals.HashResolver.TypesHashDict.ElementAt(i).Key
                };
            }
        }
        void PopulateView()
        {
            if (!string.IsNullOrEmpty(_targetImportFileName))
                textbox_ImportFileName.Text = Utils.ShortenPath(_targetImportFileName, 60);
            comboBox_AssetType.BeginUpdate();
            comboBox_AssetType.Items.Clear();
            int selIndex = 0;
            foreach (var curAsset in assetTypes)
            {
                ComboBoxItemFileType item = new ComboBoxItemFileType(curAsset.Hash, Globals.HashResolver.ResolveHash(curAsset.Hash, RetroStrike.HashNameResolver.eHashTypeSelector.FileTypes));
                comboBox_AssetType.Items.Add(item);

                //If the target import file type is known or can be detected, we should update the "selIndex"
                //  with the correct corresponding index of that file type in the combobox.
                //  i.e., "lua" should be detected and imported as "script", likewise with models and every other type.

            }
            comboBox_AssetType.SelectedIndex = selIndex;
            comboBox_AssetType.EndUpdate();
        }
        #endregion

        #region Button Handlers
        private void button_Import_Click(object sender, EventArgs e)
        {
            //TODO: Work on importing files from here.
            //TODO: Work on Processing known file types for import/replace

            var targetFileName = System.IO.Path.GetFileNameWithoutExtension(this.TargetImportFile);
            var targetFileType = ((ComboBoxItemFileType)comboBox_AssetType.SelectedItem).Hash;
            if (Globals.ActiveDSK.CanAddFile(targetFileType, targetFileName))
            {
                Stream xIn = File.Open(TargetImportFile, FileMode.Open, FileAccess.Read);
                if (Globals.ActiveDSK.AddFile(targetFileType, xIn, targetFileName))
                {
                    this.ImportSuccess = true;
                    this.WasFileImported = true;
                    this.Close();
                }
                else
                    MessageBox.Show("Uknown Error", "Import Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            MessageBox.Show("Import Error", "Cannot add file", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        private void button_Cancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }
        #endregion


    }
}
