// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* SiberianMenuGrid.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using ManagedIrbis.Menus;

#endregion

#nullable enable

namespace ManagedIrbis.WinForms.Grid
{
    /// <summary>
    ///
    /// </summary>
    public class SiberianMenuGrid
        : SiberianGrid
    {
        #region Properties

        /// <summary>
        /// Column for menu code.
        /// </summary>
        public SiberianMenuCodeColumn CodeColumn { get; private set; }

        /// <summary>
        /// Column for menu comment.
        /// </summary>
        public SiberianMenuCommentColumn CommentColumn { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public SiberianMenuGrid()
        {
            HeaderHeight = 26;

            CodeColumn = (SiberianMenuCodeColumn)CreateColumn<SiberianMenuCodeColumn>();
            CodeColumn.Title = "Code";
            CodeColumn.FillWidth = 100;

            CommentColumn = (SiberianMenuCommentColumn)CreateColumn<SiberianMenuCommentColumn>();
            CommentColumn.Title = "Comment";
            CommentColumn.FillWidth = 100;
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Use given WSS.
        /// </summary>
        public void Load
            (
                MenuEntry[] entries
            )
        {
            Rows.Clear();

            foreach (var entry in entries)
            {
                CreateRow(entry);
            }
        }

        #endregion

        #region SiberianGrid members

        /// <inheritdoc/>
        protected override SiberianRow CreateRow()
        {
            var result = base.CreateRow();
            result.Height = 24;

            return result;
        }

        #endregion

        #region Object members

        #endregion
    }
}
