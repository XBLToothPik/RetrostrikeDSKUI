using ReaLTaiizor.Forms;
using RetroStrike.Pbl;
using RetroStrike.Platform.XBox;
using RetrostrikeDSKUI.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using static RetroStrike.VirtualDisk.DSKFile;

namespace RetrostrikeDSKUI.Forms.ExportWindows
{
    public partial class WindowExportTexture : MaterialForm, IExportWindow
    {
        #region Const
        public const string WindowTitle = "Export Texture";
        #endregion

        #region Fields
        bool _exportSuccess = false;
        int mipsPreviewActualIndex = 0;
        byte[][] mipsData;

        RFI _targetRFI;
        PblFile _targetPblFile;
        RedTextureXBox xboxtexture;
        #endregion

        #region Properties

        #endregion

        #region CTORS
        public WindowExportTexture(RFI targetRFI)
        {
            InitializeComponent();
            this._targetRFI = targetRFI;
            ((IExportWindow)this).LoadData();
            ((IExportWindow)this).LoadDataIntoView();
        }


        #endregion

        #region Interface
        bool IExportWindow.ExportSucess => _exportSuccess;

        void IExportWindow.LoadData()
        {
            this._targetPblFile = PblFile.CreateFromRFI(this._targetRFI, true);
            xboxtexture = RedTextureXBox.CreateFromPBLChunk(_targetPblFile.RootChunk.GetChildByID("tex_"));
            int numMips = -1;
            string mipsExportError = string.Empty;
            xboxtexture.ExportMips(ref this.mipsData, out numMips, out mipsExportError);
        }

        void IExportWindow.LoadDataIntoView()
        {
            SetWindowTitle(xboxtexture.TextureName);
            labelWidth.Text = xboxtexture.Width.ToString();
            labelHeight.Text = xboxtexture.Height.ToString();
            labelDepth.Text = xboxtexture.Depth.ToString();
            labelMips.Text = xboxtexture.MaxMaps.ToString();
            labelMipBias.Text = xboxtexture.MipBias.ToString();
            labelTexFormat.Text = xboxtexture.TextureFormat.ToString();
            labelRedFormat.Text = xboxtexture.RedTextureType.ToString();
            SetMipsPreviewInfo();
        }
        #endregion

        #region Methods
        void SetWindowTitle(string title)
        {
            this.Text = $"{WindowTitle} - {title}";
        }
        void SetMipsPreviewInfo()
        {
            int mipWidth = xboxtexture.Width >> mipsPreviewActualIndex;
            int mipHeight = xboxtexture.Height >> mipsPreviewActualIndex;
            SetMipsPreviewIndexLabelText(mipsPreviewActualIndex, xboxtexture.MaxMaps, mipWidth, mipHeight);
            this.pictureboxMipsPreview.Image = ImageUtils.MipToBMP(this.mipsData[mipsPreviewActualIndex], mipWidth, mipHeight);
            if (mipWidth > this.pictureboxMipsPreview.Size.Width || mipHeight > this.pictureboxMipsPreview.Height)
                this.pictureboxMipsPreview.SizeMode = PictureBoxSizeMode.StretchImage;
            else
                this.pictureboxMipsPreview.SizeMode = PictureBoxSizeMode.CenterImage;
            buttonMipsPreviewGoLeft.Enabled = mipsPreviewActualIndex > 0;
            buttonMipsPreviewGoRight.Enabled = mipsPreviewActualIndex < xboxtexture.MaxMaps - 1;
        }
        void SetMipsPreviewIndexLabelText(int index, int max, int width, int height)
        {
            labelMipsPreviewIndex.Text = $"{index} / {max - 1} ({width}x{height})";
        }
        #endregion

        #region Button Handlers
        private void buttonMipsPreviewGoLeft_Click(object sender, EventArgs e)
        {
            if (mipsPreviewActualIndex > 0)
            {
                mipsPreviewActualIndex--;
                SetMipsPreviewInfo();
            }
        }
        private void buttonMipsPreviewGoRight_Click(object sender, EventArgs e)
        {
            if (mipsPreviewActualIndex < xboxtexture.MaxMaps - 1)
            {
                mipsPreviewActualIndex++;
                SetMipsPreviewInfo();
            }
        }
        #endregion

    }
}
