// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* UnsupportedCodepointException.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace AM.Text;

/// <summary>
///
/// </summary>
public sealed class UnsupportedCodepointException
    : Exception
{
    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public UnsupportedCodepointException()
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public UnsupportedCodepointException
        (
            string message
        )
        : base (message)
    {
        // пустое тело конструктора
    }
}
