using ImageMagick;
using ReaLTaiizor.Forms;
using RetroStrike.Pbl;
using RetroStrike.Platform.XBox;
using RetrostrikeDSKUI.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Forms;
using static RetroStrike.VirtualDisk.DSKFile;
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.

namespace RetrostrikeDSKUI.Forms.ExportWindows
{
    public partial class WindowExportTexture : MaterialForm, ITypeExportWindow
    {
        #region Const
        public const string WindowTitle = "Export Texture";
        #endregion

        #region Fields
        bool _exportSuccess = false;
        int mipsPreviewActualIndex = 0;
        int mipsPreviewActualFaceIndex = 0;

        RFI _targetRFI;
        PblFile _targetPblFile;
        RedTextureXBox xboxtexture;

        ContextMenuStrip contextMenuStripExportMipsOptions;
        #endregion

        #region CTORS
        public WindowExportTexture(RFI targetRFI)
        {
            InitializeComponent();
            CreateMipsExportOptionsContextMenuStrip();

            this._targetRFI = targetRFI;
            ((ITypeExportWindow)this).LoadData();
            ((ITypeExportWindow)this).LoadDataIntoView();
        }


        #endregion

        #region Interface Properties
        bool ITypeExportWindow.ExportSucess => _exportSuccess;
        #endregion

        #region Interface Methods
        void ITypeExportWindow.LoadData()
        {
            this._targetPblFile = PblFile.CreateFromRFI(this._targetRFI, true);
            xboxtexture = RedTextureXBox.CreateFromPBLChunk(_targetPblFile.RootChunk.GetChildByID("tex_"));
            int numMips = -1;
            string mipsExportError = string.Empty;
            xboxtexture.DecodeMips(out numMips, out mipsExportError);
            if (numMips <= 0)
            {
                MessageBox.Show($"Mips Count Was {numMips}", "Export Error - Mips Count Too Low", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this._exportSuccess = false;
                this.Close();
            }
            if (!string.IsNullOrEmpty(mipsExportError))
            {
                MessageBox.Show($"Mips Export Error:\r\n{mipsExportError}", "Mips Export Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this._exportSuccess = false;
                this.Close();
            }
        }

        void ITypeExportWindow.LoadDataIntoView()
        {
            SetWindowTitle(xboxtexture.TextureName);
            labelWidth.Text = xboxtexture.Width.ToString();
            labelHeight.Text = xboxtexture.Height.ToString();
            labelDepth.Text = xboxtexture.Depth.ToString();
            labelMips.Text = xboxtexture.MaxMaps.ToString();
            labelMipBias.Text = xboxtexture.MipBias.ToString();
            labelTexFormat.Text = xboxtexture.TextureFormat.ToString();
            labelRedFormat.Text = xboxtexture.RedTextureType.ToString();
            labelNumFaces.Text = xboxtexture.NumFaces.ToString();
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
            SetMipsPreviewIndexLabelText(mipsPreviewActualIndex, xboxtexture.MaxMaps, mipsPreviewActualFaceIndex, xboxtexture.NumFaces, mipWidth, mipHeight);
            this.pictureboxMipsPreview.Image = ImageUtils.MipToBMP(this.xboxtexture.FaceData[mipsPreviewActualFaceIndex].MipData[mipsPreviewActualIndex], mipWidth, mipHeight);
            if (mipWidth > this.pictureboxMipsPreview.Size.Width || mipHeight > this.pictureboxMipsPreview.Height)
                this.pictureboxMipsPreview.SizeMode = PictureBoxSizeMode.StretchImage;
            else
                this.pictureboxMipsPreview.SizeMode = PictureBoxSizeMode.CenterImage;
            buttonMipsPreviewGoLeft.Enabled = mipsPreviewActualIndex > 0;
            buttonMipsPreviewGoRight.Enabled = mipsPreviewActualIndex < xboxtexture.MaxMaps - 1;
            buttonFacePreviewGoLeft.Enabled = mipsPreviewActualFaceIndex > 0;
            buttonFacePreviewGoRight.Enabled = mipsPreviewActualFaceIndex < xboxtexture.NumFaces - 1;
        }
        void SetMipsPreviewIndexLabelText(int index, int max, int faceIndex, int maxFaces, int width, int height)
        {
            labelMipsPreviewIndex.Text = $"{index} / {max - 1} ({width}x{height})\nFace {faceIndex} / {maxFaces - 1}";
        }
        void CreateMipsExportOptionsContextMenuStrip()
        {
            var _menu = this.contextMenuStripExportMipsOptions = new ContextMenuStrip();
            buttonExportMips.ContextMenuStrip = this.contextMenuStripExportMipsOptions;
            _menu.Name = "contextMenuStripExportMipsOptions";

            //Create the context menu strip

            //TGA
            ToolStripMenuItem option_tga = new ToolStripMenuItem();
            option_tga.Name = "contextMenuStripExportMipsOptions_tga";
            option_tga.Text = ".TGA File";
            option_tga.Tag = "tga";
            option_tga.Click += this.buttonExportMipsType_Click;

            //JPG
            ToolStripMenuItem option_jpg = new ToolStripMenuItem();
            option_jpg.Name = "contextMenuStripExportMipsOptions_jpg";
            option_jpg.Text = ".JPG File";
            option_jpg.Tag = "jpg";
            option_jpg.Click += this.buttonExportMipsType_Click;

            //BMP
            ToolStripMenuItem option_bmp = new ToolStripMenuItem();
            option_bmp.Name = "contextMenuStripExportMipsOptions_bmp";
            option_bmp.Text = ".bmp File";
            option_bmp.Tag = "bmp";
            option_bmp.Click += this.buttonExportMipsType_Click;

            //PNG
            ToolStripMenuItem option_png = new ToolStripMenuItem();
            option_png.Name = "contextMenuStripExportMipsOptions_png";
            option_png.Text = ".png File";
            option_png.Tag = "png";
            option_png.Click += this.buttonExportMipsType_Click;

            //Add the items
            this.contextMenuStripExportMipsOptions.Items.Add(option_tga);
            this.contextMenuStripExportMipsOptions.Items.Add(option_jpg);
            this.contextMenuStripExportMipsOptions.Items.Add(option_bmp);
            this.contextMenuStripExportMipsOptions.Items.Add(option_png);
        }
        #endregion

        #region Button Handlers
        private void buttonFacePreviewGoLeft_Click(object sender, EventArgs e)
        {
            if (mipsPreviewActualFaceIndex > 0)
            {
                mipsPreviewActualFaceIndex--;
                mipsPreviewActualIndex = 0;
                SetMipsPreviewInfo();
            }
        }
        private void buttonFacePreviewGoRight_Click(object sender, EventArgs e)
        {
            if (mipsPreviewActualFaceIndex < xboxtexture.NumFaces - 1)
            {
                mipsPreviewActualFaceIndex++;
                mipsPreviewActualIndex = 0;
                SetMipsPreviewInfo();
            }
        }
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
        private void buttonExportImage_Click(object sender, EventArgs e)
        {
            if (this.xboxtexture.FaceData.Length == 1)
            {
                SaveFileDialog SFD = new SaveFileDialog();
                SFD.Title = "Export Texture Image...(MIP0)";
                SFD.Filter = "Targa Image|*.tga|JPEG Image|*.jpg;*.jpeg|Bitmap Image|*.bmp|PNG Image|*.png";
                SFD.FileName = xboxtexture.TextureName;
                if (SFD.ShowDialog() == DialogResult.OK)
                {
                    using (Stream xOut = File.Open(SFD.FileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None))
                    {
                        int mipWidth = xboxtexture.Width;
                        int mipHeight = xboxtexture.Height;

                        MagickFormat fmt = SFD.FilterIndex switch
                        {
                            0 => MagickFormat.Tga,
                            1 => MagickFormat.Jpg,
                            2 => MagickFormat.Bmp,
                            3 => MagickFormat.Png,
                            _ => MagickFormat.Tga //default
                        };
                        ExportMip(xOut, this.xboxtexture.FaceData[0].MipData[0], xboxtexture.Width, xboxtexture.Height, fmt);
                    }
                }
            }
            else
            {
                FolderBrowserDialog FBD = new FolderBrowserDialog();
                FBD.Description = "Export Faces";
                FBD.Multiselect = false;
                FBD.ShowNewFolderButton = true;
                if (FBD.ShowDialog() == DialogResult.OK)
                {
                    var targetDirectory = FBD.SelectedPath;
                    for (int face = 0; face < this.xboxtexture.FaceData.Length; face++)
                    {
                        using (Stream xOut = File.Open($"{targetDirectory}\\{xboxtexture.TextureName}_face{face}.tga", FileMode.OpenOrCreate))
                        {
                            ExportMip(xOut, this.xboxtexture.FaceData[face].MipData[0], this.xboxtexture.Width, this.xboxtexture.Height, MagickFormat.Tga);
                        }
                    }
                }
            }
        }
        void ExportMip(Stream xOut, byte[] mipdata, int mipWidth, int mipHeight, MagickFormat fmt)
        {
            var settings = new ImageMagick.PixelReadSettings(
                (uint)mipWidth, (uint)mipHeight,
                ImageMagick.StorageType.Char,        // 8 bits per channel
                ImageMagick.PixelMapping.RGBA);
            var img = new ImageMagick.MagickImage(mipdata, settings);
            img.Write(xOut, fmt);
        }

        private void buttonExportMips_Click(object sender, EventArgs e)
        {
            buttonExportMips.OpenDropDown();
        }
        private void buttonExportMipsType_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem option = (ToolStripMenuItem)sender;
            FolderBrowserDialog FBD = new FolderBrowserDialog();
            FBD.Description = "Choose location to export the Mips";
            FBD.Multiselect = false;
            FBD.ShowNewFolderButton = true;
            FBD.ShowHiddenFiles = true;
            FBD.ShowPinnedPlaces = true;
            if (FBD.ShowDialog() == DialogResult.OK)
            {
                MagickFormat fmt = option.Tag switch
                {
                    "tga" => MagickFormat.Tga,
                    "jpg" => MagickFormat.Jpg,
                    "bmp" => MagickFormat.Bmp,
                    "png" => MagickFormat.Png,
                    _ => MagickFormat.Tga //default
                };

                var targetDirectory = FBD.SelectedPath;
                string fileType = (string)option.Tag;
                for (int face = 0; face < this.xboxtexture.FaceData.Length; face++)
                {
                    for (int mip = 0; mip < this.xboxtexture.FaceData[face].MipData[mip].Length; mip++)
                    {
                        int mipWidth = xboxtexture.Width >> mip;
                        int mipHeight = xboxtexture.Height >> mip;
                        var settings = new ImageMagick.PixelReadSettings(
                            (uint)mipWidth, (uint)mipHeight,
                            ImageMagick.StorageType.Char,        // 8 bits per channel
                            ImageMagick.PixelMapping.RGBA);
                        var imgf = new ImageMagick.MagickImage(this.xboxtexture.FaceData[face].MipData[mip], settings);
                        using (Stream xOut = File.Open($"{targetDirectory}\\{xboxtexture.TextureName}_face{face}_mip{mip}.{fileType}", FileMode.OpenOrCreate))
                        {
                            switch (option.Tag as string)
                            {
                                case "tga":
                                    imgf.Write(xOut, ImageMagick.MagickFormat.Tga);
                                    break;
                                case "jpg":
                                    imgf.Write(xOut, ImageMagick.MagickFormat.Jpg);
                                    break;
                                case "bmp":
                                    imgf.Write(xOut, ImageMagick.MagickFormat.Bmp);
                                    break;
                                case "png":
                                    imgf.Write(xOut, ImageMagick.MagickFormat.Png);
                                    break;
                            }
                        }
                    }
                }
            }
        }
        #endregion
    }
}
