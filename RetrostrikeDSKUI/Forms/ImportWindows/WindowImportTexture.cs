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
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.

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
        MagickImage[][] _importedImages;

        bool _wasImportImageValid = false;
        RedTextureXBox.cFaceData[] _faceData;
        bool _facesEdited = false;
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
            Stream imgStream = null;
            try
            {
                MagickImage initialImage = new MagickImage(imgStream = File.Open(this.TargetFileName, FileMode.Open, FileAccess.Read, FileShare.Read));
                int maxNumMips = ImageUtils.GetMaxNumMips((int)initialImage.Width, (int)initialImage.Height);

                //when we first load, we're only loading 1 image so we can load it and dispose of the stream
                //instead of assigning the face count to 6 here, we might should assign it to 1 and then when the user selects cubemap, we can then Array.Resize
                //      it to 6 and then load the other images for the other faces.
                //NOTE!!!: When the user changes from CubeMap to Texture (or other), and there are edited images in the other faces, we need to alert
                //              the user that those images will be lost if they change the type, and ask them if they want to continue or cancel the change.    
                _importedImages = new MagickImage[6][];
                _importedImages[0] = new MagickImage[maxNumMips];

                _importedImages[0][0] = initialImage;
                _wasImportImageValid = true;
                imgStream.Close();
                imgStream.Dispose();

            }
            catch (Exception ex)
            {
                
                if (imgStream != null && 
                    !(ex is FileNotFoundException)
                    && !(ex is IOException)
                    && !(ex is UnauthorizedAccessException)
                    && !(ex is DirectoryNotFoundException))
                {
                    imgStream.Close();
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
                var newBMP = this._importedImages[0][0].ToBitmap();
                pictureBoxTexturePreview.SizeMode = (newBMP.Width > pictureBoxTexturePreview.Width || newBMP.Height > pictureBoxTexturePreview.Height)
                    ? PictureBoxSizeMode.StretchImage
                    : PictureBoxSizeMode.CenterImage;
                pictureBoxTexturePreview.Image = newBMP;

                labelWidth.Text = this._importedImages[0][0].Width.ToString();
                labelHeight.Text = this._importedImages[0][0].Height.ToString();
                updownDepth.Value = 1;
                updownMips.Minimum = 1;
                updownMips.Maximum = updownMips.Value = ImageUtils.GetMaxNumMips((int)this._importedImages[0][0].Width, (int)this._importedImages[0][0].Height);
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
            CreateFaceDataFromImportedImages(asCubeMap);
            //TODO: Next make a function that creates the face data (so when EditMips/Faces window is returned from, we can get the images from it and create the faces).
            //          Doing this should cover both a normal single image import, and custom faces/mips.
            //RedTextureXBox.cFaceData faceData = createfacedate/getfacedata
        }
        void CreateFaceDataFromImportedImages(bool asCubeMap)
        {
            var numMips = (int)updownMips.Value;
            var numFaces = asCubeMap ? 6 : 1;

            // Face 0 / mip 0 is the canonical base image: it defines the dimensions,
            // and any null (missing) mip on any face is derived from it by resizing.
            var baseImage = _importedImages[0][0];
            var imgWidth = baseImage.Width;
            var imgHeight = baseImage.Height;

            _faceData = new RedTextureXBox.cFaceData[numFaces];

            // Missing mips are ALWAYS generated from face0/mip0, so a derived mip is
            // identical no matter which face requested it - compute once, share the
            // byte[] reference across faces. Only useful when there's more than one
            // face; with a single face each mip is visited exactly once.
            byte[][] mipCache = numFaces > 1 ? new byte[numMips][] : null;

            MagickImage working = null; // scratch image for deriving missing mips
            try
            {
                for (int face = 0; face < numFaces; face++)
                {
                    var curFace = _faceData[face] = new RedTextureXBox.cFaceData();
                    curFace.MipData = new byte[numMips][];

                    // Source mips for this face; a face with no imported images at all
                    // just derives everything from face0/mip0.
                    var faceImages = (face < _importedImages.Length) ? _importedImages[face] : null;

                    for (int mip = 0; mip < numMips; mip++)
                    {
                        var provided = (faceImages != null && mip < faceImages.Length)
                            ? faceImages[mip]
                            : null;

                        if (provided != null)
                        {
                            //the user supplied this face/mip explicitly - use it as-is.
                            curFace.MipData[mip] = provided.ToByteArray(MagickFormat.Rgba);
                        }
                        else if (mipCache != null && mipCache[mip] != null)
                        {
                            //tlready derived this mip level from face0/mip0 earlier.
                            curFace.MipData[mip] = mipCache[mip];
                        }
                        else
                        {
                            // Missing mip: derive it from face0/mip0 by resizing.
                            var w = Math.Max(1, imgWidth >> mip);
                            var h = Math.Max(1, imgHeight >> mip);

                            // Re-clone from the base if we have no scratch image yet, or
                            // if the scratch image has already been shrunk below the
                            // target size (never upscale - always resize down from base).
                            if (working == null || working.Width < w || working.Height < h)
                            {
                                working?.Dispose();
                                working = (MagickImage)baseImage.Clone();
                            }
                            if (working.Width != w || working.Height != h)
                                working.Resize(w, h);

                            var derived = working.ToByteArray(MagickFormat.Rgba);
                            if (mipCache != null)
                                mipCache[mip] = derived;
                            curFace.MipData[mip] = derived;
                        }
                    }
                }
            }
            finally
            {
                working?.Dispose();
            }
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
