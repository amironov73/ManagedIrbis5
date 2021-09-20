// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Program.cs -- program entry point
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Windows.Forms;

#endregion

#nullable enable

namespace MachineInfo
{
    /// <summary>
    /// Class that contains program entry point.
    /// </summary>
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode (HighDpiMode.SystemAware);
            Application.EnableVisualStyles ();
            Application.SetCompatibleTextRenderingDefault (false);
            Application.Run (new MainForm());

        } // method Main

    } // class Program

} // namespace MachineInfo
