// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* HabitualDataGridView.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Drawing;
using System.Windows.Forms;

#endregion

namespace AM.Windows.Forms
{
    /// <summary>
    ///
    /// </summary>
    [System.ComponentModel.DesignerCategory("Code")]
    public class HabitualDataGridView
        : DataGridView
    {
        #region Properties

        //
        // TODO: объединить с  HandyGrid
        //

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public HabitualDataGridView()
        {
            AutoGenerateColumns = false;
            RowHeadersVisible = false;
            SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            AllowUserToResizeRows = false;

            var mainStyle = new DataGridViewCellStyle();
            DefaultCellStyle = mainStyle;

            var alternateStyle = new DataGridViewCellStyle(mainStyle)
                {
                    BackColor = Color.LightGray
                };
            AlternatingRowsDefaultCellStyle = alternateStyle;
        }

        #endregion
    }
}
