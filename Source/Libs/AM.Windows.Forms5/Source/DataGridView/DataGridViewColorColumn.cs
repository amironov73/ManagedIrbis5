// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* DataGridViewColorColumn.cs --
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
public class DataGridViewColorColumn
    : DataGridViewColumn
{
    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public DataGridViewColorColumn()
        : base (new DataGridViewColorCell())
    {
        // пустое тело конструктора
    }

    #endregion
}
