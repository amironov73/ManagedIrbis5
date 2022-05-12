// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* IrbisNetworkException.cs -- исключение, возникшее при сетевом обмене с сервером ИРБИС64
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace ManagedIrbis;

/// <summary>
/// Исключение, возникшее при сетевом обмене с сервером ИРБИС64.
/// </summary>
public sealed class IrbisNetworkException
    : IrbisException
{
    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public IrbisNetworkException()
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public IrbisNetworkException
        (
            int errorCode
        )
        : base (errorCode)
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public IrbisNetworkException
        (
            string message
        )
        : base (message)
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public IrbisNetworkException
        (
            string message,
            Exception innerException
        )
        : base (message, innerException)
    {
        // пустое тело конструктора
    }

    #endregion
}
