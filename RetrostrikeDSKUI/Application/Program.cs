using System.Windows.Forms;
namespace RetrostrikeDSKUI.Application
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            System.Windows.Forms.Application.EnableVisualStyles();
#if NETCOREAPP3_1 || NET6_0 || NET7_0 || NET8_0 || NET9_0 || NET10_0 || NET11_0
            System.Windows.Forms.Application.SetHighDpiMode(HighDpiMode.SystemAware);
#endif
            System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);
            System.Windows.Forms.Application.Run(new WindowMain());
        }
    }
}