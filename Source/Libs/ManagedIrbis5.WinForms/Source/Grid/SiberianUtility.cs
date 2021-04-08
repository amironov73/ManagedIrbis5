// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* SiberianUtility.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Drawing;
using System.Windows.Forms;

#endregion

#nullable enable

namespace ManagedIrbis.WinForms.Grid
{
    /// <summary>
    ///
    /// </summary>
    public static class SiberianUtility
    {
        #region Public methods

        /// <summary>
        /// Measure given cell text.
        /// </summary>
        public static void MeasureText
            (
                this SiberianGrid grid,
                string text,
                SiberianDimensions dimensions
            )
        {
            var size = dimensions.ToSize();
            var font = grid.Font;
            var flags = TextFormatFlags.Left
                        | TextFormatFlags.Top
                        | TextFormatFlags.NoPrefix;

            var result = TextRenderer.MeasureText
                (
                    text,
                    font,
                    size,
                    flags
                );

            dimensions.Width = result.Width;
            dimensions.Height = result.Height;
        }

        #endregion
    }
}
