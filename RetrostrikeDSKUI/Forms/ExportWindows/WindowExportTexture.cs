using ReaLTaiizor.Forms;
using RetroStrike.Pbl;
using RetroStrike.Platform.XBox;
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

        }

        void IExportWindow.LoadDataIntoView()
        {
            SetWindowTitle(xboxtexture.TextureName);
        }
        #endregion


        #region Methods
        void SetWindowTitle(string title)
        {
            this.Text = $"{WindowTitle} - {title}";
        }
        #endregion

        private void WindowExportTexture_Load(object sender, EventArgs e)
        {

        }

 
    }
}
