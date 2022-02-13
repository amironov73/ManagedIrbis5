// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* UdkException.cs -- исключение, возникающее при работе с УДК
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace ManagedIrbis.Systematization;

/// <summary>
/// Исключение, возникающее при работе с УДК.
/// </summary>
public sealed class UdkException
    : IrbisException
{
    #region Construciton

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public UdkException()
    {
    }

    #endregion

    /// <summary>
    /// Конструктор.
    /// </summary>
    public UdkException
        (
            string message
        )
        : base (message)
    {
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public UdkException
        (
            string message,
            Exception innerException
        )
        : base (message, innerException)
    {
    }
}
