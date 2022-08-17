// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* TreeGridNodeState.cs -- состояние узла грида
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace AM.Windows.Forms;

/// <summary>
/// Состояние узла грида.
/// </summary>
[Flags]
public enum TreeGridNodeState
{
    /// <summary>
    /// Нормальное (по умолчанию).
    /// </summary>
    Normal = 0,

    /// <summary>
    /// Узел выбран.
    /// </summary>
    Selected = 1,

    /// <summary>
    /// Узел запрещен.
    /// </summary>
    Disabled = 2,

    /// <summary>
    /// Узел отмечен.
    /// </summary>
    Checked = 4,

    /// <summary>
    /// Узел только для чтения.
    /// </summary>
    ReadOnly = 8
}
