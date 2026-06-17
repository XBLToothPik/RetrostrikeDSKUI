using ReaLTaiizor.Forms;
using RetroStrike.Pbl;
using RetroStrike.Platform.XBox;
using RetrostrikeDSKUI.Core;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
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

        ContextMenuStrip contextMenuStripExportMipsOptions;
        #endregion

        #region CTORS
        public WindowExportTexture(RFI targetRFI)
        {
            InitializeComponent();
            CreateMipsExportOptionsContextMenuStrip();

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
            SaveFileDialog SFD = new SaveFileDialog();
            SFD.Title = "Export Texture Image...(MIP0)";
            SFD.Filter = "Targa Image|*.tga|JPEG Image|*.jpg;*.jpeg|Bitmap Image|*.bmp|PNG Image|*.png";
            SFD.FileName = xboxtexture.TextureName;
            if (SFD.ShowDialog() == DialogResult.OK)
            {
                using (Stream xOut = File.Open(SFD.FileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None))
                {
                    var img = SixLabors.ImageSharp.Image.LoadPixelData<Rgba32>(this.mipsData[0], xboxtexture.Width, xboxtexture.Height);
                    switch (SFD.FilterIndex)
                    {
                        case 0: //TGA
                            img.SaveAsTga(xOut);
                            break;
                        case 1: //JPG
                            img.SaveAsJpeg(xOut);
                            break;
                        case 2: //BMP
                            img.SaveAsBmp(xOut);
                            break;
                        case 3: //PNG
                            img.SaveAsPng(xOut);
                            break;
                    }
                }
            }
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
                var targetDirectory = FBD.SelectedPath;
                string fileType = (string)option.Tag;
                for (int mip = 0; mip < this.mipsData.Length; mip++)
                {
                    var img = SixLabors.ImageSharp.Image.LoadPixelData<Rgba32>(this.mipsData[mip], xboxtexture.Width >> mip, xboxtexture.Height >> mip);
                    using (Stream xOut = File.Open($"{targetDirectory}\\{xboxtexture.TextureName}_mip{mip}.{fileType}", FileMode.OpenOrCreate))
                    {
                        switch (option.Tag as string)
                        {
                            case "tga":
                                img.SaveAsTga(xOut);
                                break;
                            case "jpg":
                                img.SaveAsJpeg(xOut);
                                break;
                            case "bmp":
                                img.SaveAsBmp(xOut);
                                break;
                            case "png":
                                img.SaveAsPng(xOut);
                                break;
                        }
                    }
                }
            }
        }
        #endregion



    }
}
