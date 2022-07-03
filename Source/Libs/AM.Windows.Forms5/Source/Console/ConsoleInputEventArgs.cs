// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* ConsoleInputEventArgs.cs -- аргументы события для консольного контрола
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace AM.Windows.Forms;

/// <summary>
/// Аргументы события для консольного контрола <see cref="ConsoleControl"/>
/// </summary>
public sealed class ConsoleInputEventArgs
    : EventArgs
{
    #region Properties

    /// <summary>
    /// Текст, ассоциированный с событием.
    /// </summary>
    public string? Text { get; set; }

    #endregion
}
