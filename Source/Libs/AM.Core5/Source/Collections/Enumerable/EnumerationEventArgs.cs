// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* EnumerationEventArgs.cs -- аргументы событий при перечислении коллекций
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace AM.Collections.Generic;

/// <summary>
/// Аргументы событий, возникающих при перечислении коллекций.
/// </summary>
public sealed class EnumerationEventArgs
    : EventArgs
{
    #region Properties

    /// <summary>
    /// Описание события, например, <c>"MoveNext"</c>.
    /// </summary>
    public required string Event { get; init; }

    /// <summary>
    /// Результат события (если имеет смысл).
    /// </summary>
    public object? Result { get; init; }

    #endregion
}
