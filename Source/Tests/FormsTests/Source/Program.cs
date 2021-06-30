
#region Using directives

using System;
using System.Windows.Forms;

using AM.Windows.Forms;

#endregion

namespace FormsTests
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            InputLanguageUtility.InstallWmInputLanguageRequestFix();

            Application.Run(new MainForm());
        }
    }
}
