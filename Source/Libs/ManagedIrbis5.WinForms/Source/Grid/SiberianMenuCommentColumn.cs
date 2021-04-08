// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* SiberianMenuCommentColumn.cs --
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
    public class SiberianMenuCommentColumn
        : SiberianColumn
    {
        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public SiberianMenuCommentColumn()
        {
            ReadOnly = true;

            Palette.BackColor = Color.White;
            Palette.ForeColor = Color.Black;
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion

        #region SiberianColumn members

        /// <inheritdoc/>
        public override SiberianCell CreateCell()
        {
            SiberianCell result = new SiberianMenuCommentCell();
            result.Column = this;

            return result;
        }

        #endregion

        #region Object members

        #endregion
    }
}
