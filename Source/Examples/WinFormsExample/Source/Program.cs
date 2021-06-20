// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace

#region Using directives

using System;
using System.Windows.Forms;

using AM.Windows.Forms;

#endregion

namespace WinFormsExample
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                Application.SetHighDpiMode(HighDpiMode.SystemAware);
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                InputLanguageUtility.InstallWmInputLanguageRequestFix();

                Application.Run(new SimpleSearchForm());
            }
            catch (Exception exception)
            {
                ExceptionBox.Show(exception);
            }

        } // method Main

    } // class Program

} // namespace WinFormsExample
