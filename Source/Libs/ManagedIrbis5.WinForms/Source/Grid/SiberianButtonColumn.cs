// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* SiberianButtonColumn.cs -- столбец ячеек с кнопками
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace ManagedIrbis.WinForms.Grid;

/// <summary>
/// Столбец ячеек с кнопками.
/// </summary>
public class SiberianButtonColumn
    : SiberianColumn
{
    #region SiberianColumn members

    /// <inheritdoc/>
    public override SiberianCell CreateCell()
    {
        var result = new SiberianButtonCell
        {
            Column = this
        };

        return result;
    }

    #endregion

}
