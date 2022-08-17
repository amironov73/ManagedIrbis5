// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* TreeGridTextColumn.cs -- колонка в гриде, содержащая простой текст
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace AM.Windows.Forms;

/// <summary>
/// Колонка в гриде <see cref="TreeGrid"/>, содержащая простой текст.
/// </summary>
public class TreeGridTextColumn
    : TreeGridColumn
{
    #region TreeGridColumn members

    /// <inheritdoc cref="TreeGridColumn.Editable"/>
    public override bool Editable => true;

    #endregion
}
