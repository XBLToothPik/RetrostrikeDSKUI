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
using RetrostrikeDSKUI.Application;
using RetroStrike.Utils;
using System.Media;

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
        MagickImage[][] _importedFaceImages;
        bool _wasImportImageValid = false;
        bool AnyFacesEdited
        {
            get
            {
                for (int face = 0; face < _importedFaceImages.Length; face++)
                {
                    var faceImages = _importedFaceImages[face];
                    if (faceImages != null)
                    {
                        //TODO: !!!!!! this could provide errors if there is is only 1 face and 1 mip maybe, check this
                        int mip = face == 0 ? 1 : 0; // Skip face 0, mip 0 if checking face 0
                        for (; mip < faceImages.Length; mip++)
                        {
                            if (faceImages[mip] != null)
                            {
                                return true; // Found an edited image
                            }
                        }
                    }
                }
                return false;
            }
        }
        bool AnyFacesEditedBeyondFaceZero
        {
            get
            {
                for (int i = 0; i < _importedFaceImages.Length; i++)
                {
                    if (i == 0) 
                        continue; // Skip face 0
                    var faceImages = _importedFaceImages[i];
                    if (faceImages != null)
                    {
                        for (int mip = 0; mip < faceImages.Length; mip++)
                        {
                            if (faceImages[mip] != null)
                            {
                                return true; // Found an edited image beyond face 0
                            }
                        }
                    }
                }
                return false;
            }
        }
        bool AnyFacesEditedBeyondBaseMipZero
        {
            get
            {
                for (int face = 0; face < _importedFaceImages.Length; face++)
                {
                    var faceImages = _importedFaceImages[face];
                    if (faceImages != null)
                    {
                        int mip = face == 0 ? 1 : 0;
                        for (; mip < faceImages.Length; mip++) // Start from mip 1
                        {
                            if (faceImages[mip] != null)
                            {
                                return true; // Found an edited image beyond mip 0
                            }
                        }
                    }
                }
                return false;
            }
        }
        int prevTexTypeIndex = -1;
        int prevTexFormatIndex = -1;
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

            //TODO:
            //          1) calculate max num of mips for the numeric updown [done]
            //          2) populate the tex format combobox's with the items for the platform type [done]
            //          3) populate the tex type combobox (TEXTURE, CUBEMAP, VOLUME) [done]
            //          4) When click cancel, close the active stream [dont need to do this, as we close the stream after we load the image into memory]
            //          5) Create the RFI.  Set it's custom data, add to DSKFile [done]
            //          6) Finish the ProcessNewRFIAsTexture in the DSKFile. [done]
            //          7) Create the "Custom Mips" window (!!! NEXT !!!)
            //          8) If the NumMips is changed, then we need to see if there any any edited mips in the new range, if so, warn prompt the user that those changes will be lost if they change the number of mips, and ask them if they want to continue or cancel the change.
            //              If they continue, then we need to resize the array and set the new mips to null (or maybe we can just leave them as is, but then we need to make sure that when we create the face data, we only use the number of mips specified by the user).
            //              If they cancel, then we need to revert the numeric updown back to the previous value.
        }
        #endregion

        #region Interface Methods
        void ITypeImportWindow.LoadData()
        {
            Stream imgStream = null;
            try
            {
                imgStream = File.Open(this.TargetFileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                MagickImage initialImage = new MagickImage(imgStream);
                int maxNumMips = ImageUtils.GetMaxNumMips((int)initialImage.Width, (int)initialImage.Height);

                //when we first load, we're only loading 1 image so we can load it and dispose of the stream
                //instead of assigning the face count to 6 here, we assign it to 1 and then when the user selects cubemap, we can then Array.Resize
                //      it to 6 and then load the other images for the other faces.
                //NOTE!!!: When the user changes from CubeMap to Texture (or other), and there are edited images in the other faces, we need to alert
                //              the user that those images will be lost if they change the type, and ask them if they want to continue or cancel the change.    
                _importedFaceImages = new MagickImage[1][];
                _importedFaceImages[0] = new MagickImage[maxNumMips];

                _importedFaceImages[0][0] = initialImage;
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
                var newBMP = this._importedFaceImages[0][0].ToBitmap();
                pictureBoxTexturePreview.SizeMode = (newBMP.Width > pictureBoxTexturePreview.Width || newBMP.Height > pictureBoxTexturePreview.Height)
                    ? PictureBoxSizeMode.StretchImage
                    : PictureBoxSizeMode.CenterImage;
                pictureBoxTexturePreview.Image = newBMP;

                labelWidth.Text = this._importedFaceImages[0][0].Width.ToString();
                labelHeight.Text = this._importedFaceImages[0][0].Height.ToString();
                updownDepth.Value = 1;
                updownMips.Minimum = 1;
                updownMips.Maximum = updownMips.Value = ImageUtils.GetMaxNumMips((int)this._importedFaceImages[0][0].Width, (int)this._importedFaceImages[0][0].Height);
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
            int curSelIndex = comboBoxTexFormat.SelectedIndex;
            buttonImport.Enabled = selTexFormat != eTexFormat.P8; // Disable import button for unsupported format
            if (selTexFormat == eTexFormat.P8)
                MessageBox.Show("Pallette (P8) format is not yet supported.  Please select a different format.", "Unsupported Format", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
                buttonImport.Enabled = true;
        }
        private void comboBoxTexType_SelectedIndexChanged(object sender, EventArgs e)
        {
            int curSelIndex = comboBoxTexType.SelectedIndex;
            var selTexType = GetSelectedTexType();
            if (prevTexTypeIndex >= 0 && prevTexTypeIndex != curSelIndex)
            {
                var prevTexType = ((sTexTypeItem)comboBoxTexType.Items[prevTexTypeIndex]).TexType;
                //the index was changed, we need to see if we need to prompt,
                if (AnyFacesEditedBeyondFaceZero && prevTexType == eTexType.CUBEMAP)
                {
                    var result = MessageBox.Show("You have edited the CubeMap faces.  If you change the type, those changes will be lost.  Do you want to continue?", "Confirm Type Change", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (result == DialogResult.No)
                    {
                        //revert the selection back to the previous index
                        comboBoxTexType.SelectedIndexChanged -= this.comboBoxTexFormat_SelectedIndexChanged;
                        comboBoxTexType.SelectedIndex = prevTexTypeIndex;
                        comboBoxTexType.SelectedIndexChanged += this.comboBoxTexFormat_SelectedIndexChanged;
                        return;
                    }
                    else
                    {
                        //change of texType confirmed by user, resize array.
                       
                        Array.Resize(ref _importedFaceImages, 1);
                    }
                }
            }

            prevTexTypeIndex = curSelIndex;

            comboBoxTexType.SelectedIndexChanged += this.comboBoxTexFormat_SelectedIndexChanged;
        }
        private void buttonImport_Click(object sender, EventArgs e)
        {
            string importErrors = string.Empty;
            if (ImportTexture(out importErrors))
            {
                SystemSounds.Asterisk.Play();
                this._importSuccess = true;
                this.Close();
            }
            else
            {
                MessageBox.Show($"Failed to import texture: {importErrors}", "Import Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Methods
        bool ImportTexture(out string errors)
        {
            var selDepth = (int)updownDepth.Value;
            var selMips = (int)updownMips.Value;
            var selMipBias = (int)updownMipBias.Value;
            var selTexType = GetSelectedTexType();
            var selTexFormat = GetSelectedTexFormat();
            var faceData = CreateFaceDataFromImportedImages(selMips, selTexType == eTexType.CUBEMAP ? 6 : 1, _importedFaceImages);
            var activeDsk = AppGlobals.ActiveDSK;

            RetroStrike.VirtualDisk.DSKFile.RFI newRFI = new RetroStrike.VirtualDisk.DSKFile.RFI(AppGlobals.ActiveDSK);
            newRFI.IsNewImportedFile = true;
            newRFI.ProcessAsFileType = true;
            newRFI.NewIncomingTypeHash = Hashing.MakeFNV1A("texture");
            newRFI.NewIncomingFileName = Path.GetFileNameWithoutExtension(this.TargetFileName);

            var customData = newRFI.CustomData = new Dictionary<string, object>();
            customData.Add("tex_maxmaps", selMips);
            customData.Add("tex_depth", selDepth);
            customData.Add("tex_formatversion", 1);
            customData.Add("tex_width", (int)_importedFaceImages[0][0].Width);
            customData.Add("tex_height", (int)_importedFaceImages[0][0].Height);
            customData.Add("tex_format", selTexFormat);
            customData.Add("tex_type", selTexType);
            customData.Add("face_data", faceData);

            return activeDsk.AddFile(newRFI, out errors);
        }
        RedTextureXBox.cFaceData[] CreateFaceDataFromImportedImages(int numMips, int numFaces, MagickImage[][] importedImages)
        {
            // Face 0 / mip 0 is the canonical base image: it defines the dimensions,
            // and any null (missing) mip on any face is derived from it by resizing.
            var baseImage = importedImages[0][0];
            var imgWidth = baseImage.Width;
            var imgHeight = baseImage.Height;
            var _faceData = new RedTextureXBox.cFaceData[numFaces];

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

                    //s ource mips for this face; a face with no imported images at all
                    //just derives everything from face0/mip0.
                    var faceImages = (face < importedImages.Length) ? importedImages[face] : null;

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
                            //already derived this mip level from face0/mip0 earlier.
                            curFace.MipData[mip] = mipCache[mip];
                        }
                        else
                        {
                            //missing mip: derive it from face0/mip0 by resizing.
                            var w = Math.Max(1, imgWidth >> mip);
                            var h = Math.Max(1, imgHeight >> mip);

                            //re-clone from the base if we have no scratch image yet, or
                            //  if the scratch image has already been shrunk below the
                            //  target size (never upscale - always resize down from base).
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
            return _faceData;
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
