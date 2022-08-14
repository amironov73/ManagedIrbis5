// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* DataGridViewRatingColumn.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms;

/// <summary>
///
/// </summary>
public class DataGridViewRatingColumn
    : DataGridViewColumn
{
    #region Properties

    /// <summary>
    /// Gets or sets the maximum.
    /// </summary>
    public int Maximum
    {
        get => ((DataGridViewRatingCell)CellTemplate).Maximum;
        set => ((DataGridViewRatingCell)CellTemplate).Maximum = value;
    }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public DataGridViewRatingColumn()
        : base (new DataGridViewRatingCell())
    {
        // пустое тело конструктора
    }

    #endregion
}
