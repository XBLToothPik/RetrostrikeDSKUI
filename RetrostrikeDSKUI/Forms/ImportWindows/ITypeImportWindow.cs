using System;
using System.Collections.Generic;
using System.Text;

namespace RetrostrikeDSKUI.Forms.ImportWindows
{
    public interface ITypeImportWindow
    {
        public Dictionary<string, object> CustomData { get; }
        public bool ImportSuccess { get; }
        void LoadData();
        void LoadDataIntoView();
    }
}
