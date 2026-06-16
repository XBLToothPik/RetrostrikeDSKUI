using System;
using System.Collections.Generic;
using System.Text;

namespace RetrostrikeDSKUI.Forms.ExportWindows
{
    public interface IExportWindow
    {
        public bool ExportSucess { get; }
        void LoadData();
        void LoadDataIntoView();
    }
}
