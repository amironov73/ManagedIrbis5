// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* SiberianMenuCodeColumn.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Drawing;

#endregion

#nullable enable

namespace ManagedIrbis.WinForms.Grid
{
    /// <summary>
    ///
    /// </summary>
    public class SiberianMenuCodeColumn
        : SiberianColumn
    {
        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public SiberianMenuCodeColumn()
        {
            ReadOnly = true;

            Palette.BackColor = Color.White;
            Palette.ForeColor = Color.Black;
        }

        #endregion

        #region SiberianColumn members

        /// <inheritdoc/>
        public override SiberianCell CreateCell()
        {
            SiberianCell result = new SiberianMenuCodeCell();
            result.Column = this;

            return result;
        }

        #endregion

    }
}
