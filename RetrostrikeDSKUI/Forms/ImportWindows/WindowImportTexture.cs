using ImageMagick;
using ImageMagick.Drawing;
using ReaLTaiizor.Forms;
using RetroStrike.Enum;
using RetroStrike.Platform.XBox;
using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
#pragma warning disable CS8605 // Unboxing a possibly null value.

using RetroStrike.Image;

namespace RetrostrikeDSKUI.Forms.ImportWindows
{
    public partial class WindowImportTexture : MaterialForm, ITypeImportWindow
    {
        #region Const
        public const string WindowTitle = "Import Texture";
        #endregion

        #region Struct
        struct sTexFormatItem
        {
            public string FriendlyName;
            public eTexFormat TexFormat;
            public override string ToString()
            {
                return FriendlyName;
            }
        }
        struct sTexTypeItem
        {
            public string FriendlyName;
            public eTexType TexType;

            public override string ToString()
            {
                return FriendlyName;
            }
        }
        #endregion

        #region Fields
        bool _importSuccess;
        Dictionary<string, object> _customData;
        MagickImage _importImage;
        Stream _importImageStream;
        bool _wasImportImageValid = false;
        #endregion

        #region Properties
        string TargetFileName => (string)_customData["filename"];
        #endregion

        #region Interface Properties
        Dictionary<string, object> ITypeImportWindow.CustomData => _customData;
        bool ITypeImportWindow.ImportSuccess => _importSuccess;

        #endregion

        #region CTORS
        public WindowImportTexture()
        {
            InitializeComponent();
            _customData = new Dictionary<string, object>();
            //TODO: If imported successfully, need to set the CustomData on the RFI (check DSKFile.ProcessNewRFIAsTexture)
            //TODO: Next, import the texture file (PNG, TGA..etc..) and then create the RFI entry for it, set it's CustomData, set it to Process, add to the DSK
            //tex_maxmaps
            //tex_depth
            //tex_formatversion
            //tex_format
            //tex_type
            //NOTE: We'll let RedTextureXBox handle the MagickImage on the file's stream (to get width, height..etc..)
            //          but we can verify the file is an actual image and everything here before that,

            //TODO:
            //          1) calculate max num of mips for the numeric updown [done]
            //          2) populate the tex format combobox's with the items for the platform type [done]
            //          3) populate the tex type combobox (TEXTURE, CUBEMAP, VOLUME) [done]
            //          4) When click cancel, close the active stream
            //          5) Create the RFI.  Set it's custom data, add to DSKFile
            //          6) Finish the ProcessNewRFIAsTexture in the DSKFile.
            //          7) Add "Set Custom Mips" button for new window to allow each mip to be different (use new window in conjunction with #8 here:)
            //          8) If select cubemap, change "Edit Mips" to "Edit CubeMap", and have it open a new window to set up the 6 faces with images.
        }
        #endregion

        #region Interface Methods
        void ITypeImportWindow.LoadData()
        {
            //TODO: When you click cancel, it should close the ImageStream

            try
            {
                _importImageStream = File.Open(this.TargetFileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                _importImage = new MagickImage(_importImageStream);
                _wasImportImageValid = true;
                //We don't want to close the stream here because if it's imported then the DSKFile will use that stream.  If cancel is selected however, then we close it.
            }
            catch (Exception ex)
            {
                if (!(ex is FileNotFoundException)
                    && !(ex is IOException)
                    && !(ex is UnauthorizedAccessException)
                    && !(ex is DirectoryNotFoundException))
                {
                    _importImageStream.Close();
                }
                _wasImportImageValid = false;
            }
        }
        void ITypeImportWindow.LoadDataIntoView()
        {
            SetWindowTitle(Path.GetFileName(TargetFileName));
            PopulateView();

            //TODO: If it wasn't valid, then we need to show the user that information in some way
            if (_wasImportImageValid)
            {
                var newBMP = this._importImage.ToBitmap();
                pictureBoxTexturePreview.SizeMode = (newBMP.Width > pictureBoxTexturePreview.Width || newBMP.Height > pictureBoxTexturePreview.Height)
                    ? PictureBoxSizeMode.StretchImage
                    : PictureBoxSizeMode.CenterImage;
                pictureBoxTexturePreview.Image = newBMP;

                labelWidth.Text = _importImage.Width.ToString();
                labelHeight.Text = _importImage.Height.ToString();
                updownDepth.Value = 1;
                updownMips.Minimum = 1;
                updownMips.Maximum = updownMips.Value = ImageUtils.GetMaxNumMips((int)_importImage.Width, (int)_importImage.Height);
                updownMipBias.Value = 0;
                comboBoxTexFormat.SelectedIndex = comboBoxTexFormat.Items
                    .Cast<sTexFormatItem>()
                    .ToList()
                    .FindIndex(item => item.TexFormat == eTexFormat.DXT1); //TODO: Only if XBox
                comboBoxTexType.SelectedIndex = comboBoxTexType.Items
                    .Cast<sTexTypeItem>()
                    .ToList()
                    .FindIndex(item => item.TexType == eTexType.TEXTURE);

            }
        }
        #endregion

        #region Events
        private void WindowImportTexture_Shown(object sender, EventArgs e)
        {
            if (!_wasImportImageValid)
            {

                this._importSuccess = false;
                MessageBox.Show("Could not open image.", "Error Importing", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        #endregion

        #region Buttons
        private void comboBoxTexFormat_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selTexFormat = GetSelectedTexFormat();
        }
        private void comboBoxTexType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //TODO: If the selected Type is a CubeMap, then we need to set the "Import" button to "Setup CubeMap..."
            //          and open a new window for setting those images.....
            var selTexType = GetSelectedTexType();
            if (selTexType == eTexType.CUBEMAP)
                buttonEditFacesOrMips.Text = "Edit Faces";
            else
                buttonEditFacesOrMips.Text = "Edit Mips";
        }
        private void buttonImport_Click(object sender, EventArgs e)
        {
            ImportTexture(GetSelectedTexType() == eTexType.CUBEMAP);
        }
        #endregion

        #region Methods
        void ImportTexture(bool asCubeMap)
        {
            var selDepth = (int)updownDepth.Value;
            var selMips = (int)updownMips.Value;
            var selMipBias = (int)updownMipBias.Value;
            var selTexType = GetSelectedTexType();
            var selTexFormat = GetSelectedTexFormat();
            //RedTextureXBox.cFaceData faceData = createfacedate/getfacedata
        }
        void SetWindowTitle(string titleExtension)
        {
            this.Text = $"{WindowTitle} - {titleExtension}";
        }
        void PopulateView()
        {
            string[] texFormats = Enum.GetNames(typeof(eTexFormat));
            foreach (string texFormat in texFormats)
            {
                sTexFormatItem item = new sTexFormatItem()
                {
                    FriendlyName = texFormat,
                    TexFormat = (eTexFormat)Enum.Parse(typeof(eTexFormat), texFormat)
                };
                comboBoxTexFormat.Items.Add(item);
            }

            string[] texTypes = Enum.GetNames(typeof(eTexType));
            foreach (string texType in texTypes)
            {
                sTexTypeItem item = new sTexTypeItem()
                {
                    FriendlyName = texType,
                    TexType = (eTexType)Enum.Parse(typeof(eTexType), texType)
                };
                comboBoxTexType.Items.Add(item);
            }
        }
        public eTexFormat GetSelectedTexFormat()
        {
            return ((sTexFormatItem)comboBoxTexFormat.SelectedItem).TexFormat;
        }
        public eTexType GetSelectedTexType()
        {
            return ((sTexTypeItem)comboBoxTexType.SelectedItem).TexType;
        }
        #endregion
    }
}
