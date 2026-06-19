using ImageMagick;
using ImageMagick.Drawing;
using ReaLTaiizor.Forms;
using RetroStrike.Platform.XBox;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace RetrostrikeDSKUI.Forms.ImportWindows
{
    public partial class WindowImportTexture : MaterialForm, ITypeImportWindow
    {
        #region Const
        public const string WindowTitle = "Import Texture";
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
            //tex_d3dformat
            //tex_type
            //NOTE: We'll let RedTextureXBox handle the MagickImage on the file's stream (to get width, height..etc..)
            //          but we can verify the file is an actual image and everything here before that,

            //TODO: NEXT!!!!!!!!: Name all of the controls on the form.  Then:
            //          1) calculate max num of mips for the numeric updown
            //          2) populate the tex format combobox's with the items for the platform type
            //          3) populate the tex type combobox (TEXTURE, CUBEMAP, VOLUME)
            //          4) When click cancel, close the active stream
            //          5) Create the RFI.  Set it's custom data, add to DSKFile

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
            MagickImage img = new MagickImage();

            SetWindowTitle(Path.GetFileName(TargetFileName));

            //TODO: If it wasn't valid, then we need to show the user that information in some way
            if (_wasImportImageValid)
            {
                var newBMP = this._importImage.ToBitmap();
                pictureBoxTexturePreview.SizeMode = (newBMP.Width > pictureBoxTexturePreview.Width || newBMP.Height > pictureBoxTexturePreview.Height)
                    ? PictureBoxSizeMode.StretchImage
                    : PictureBoxSizeMode.CenterImage;
                pictureBoxTexturePreview.Image = newBMP;
            }
            else
            {
                //Disable controls, set image to "INVALID_IMAGE" 
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

        #region Methods
        void SetWindowTitle(string titleExtension)
        {
            this.Text = $"{WindowTitle} - {titleExtension}";
        }

        #endregion


    }
}
