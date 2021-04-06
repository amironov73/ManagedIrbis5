// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* ToolStripCustomizer.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms
{
    /// <summary>
    ///
    /// </summary>
    public static class ToolStripCustomizer
    {
        /// <summary>
        /// Customizes the specified tool strip.
        /// </summary>
        public static bool Customize
            (
                ToolStrip toolStrip
            )
        {
            using (var form = new ToolStripCustomizationForm(toolStrip))
            {
                return form.ShowDialog(toolStrip.FindForm())
                    == DialogResult.OK;
            }
        }
    }
}
