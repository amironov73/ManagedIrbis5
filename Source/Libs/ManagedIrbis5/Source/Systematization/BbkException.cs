// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* BbkException.cs -- исключение, возникающее при работе с ББК
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using AM;

#endregion

#nullable enable

namespace ManagedIrbis.Systematization;

/// <summary>
/// Исключение, возникающее при работе с ББК.
/// </summary>
public sealed class BbkException
    : ArsMagnaException
{
    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public BbkException()
    {
    }

    #endregion

    /// <summary>
    /// Конструктор.
    /// </summary>
    public BbkException
        (
            string message
        )
        : base (message)
    {
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public BbkException
        (
            string message,
            Exception innerException
        )
        : base (message, innerException)
    {
    }
}
