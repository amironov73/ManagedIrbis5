// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* ExitException.cs -- исключение, используемое для завершения скрипта
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using JetBrains.Annotations;

#endregion

#nullable enable

namespace AM.Lexey.Barsik;

/// <summary>
/// Исключение, используемое для завершения скрипта.
/// </summary>
[PublicAPI]
public sealed class ExitException
    : ApplicationException
{
    #region Properties

    /// <summary>
    /// Код выхода.
    /// </summary>
    public int ExitCode { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public ExitException()
        : base ("Исполнение скрипта завершено")
    {
        ExitCode = 0;
    }

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="exitCode">Код выхода.</param>
    public ExitException
        (
            int exitCode
        )
        : base ("Исполнение скрипта завершено")
    {
        ExitCode = exitCode;
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="exitCode">Код выхода.</param>
    /// <param name="message">Сообщение.</param>
    public ExitException
        (
            int exitCode,
            string? message
        )
        : base(message)
    {
        ExitCode = exitCode;
    }

    #endregion
}
