// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMember.Local
// ReSharper disable UseNameofExpression

/* PoolGuard.cs -- следит за своевременным возвращением соединения в пул
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using AM;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace ManagedIrbis.Pooling;

/// <summary>
/// Следит за своевременным возвращением соединения в пул.
/// </summary>
public sealed class PoolGuard
    : IDisposable
{
    #region Properties

    /// <summary>
    /// Отслеживаемое подключение.
    /// </summary>
    public ISyncProvider Connection { get; private set; }

    /// <summary>
    /// Отслеживаемый пул подключений.
    /// </summary>
    public ConnectionPool Pool { get; private set; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public PoolGuard
        (
            ConnectionPool pool
        )
    {
        Sure.NotNull (pool);

        Magna.Logger.LogTrace (nameof(PoolGuard) + "::Constructor");

        Pool = pool;
        Connection = Pool.AcquireConnection();
    }

    #endregion

    // #region Public methods
    //
    // /// <summary>
    // /// Неявное преобразование.
    // /// </summary>
    // public static implicit operator ISyncProvider
    //     (
    //         PoolGuard guard
    //     )
    // {
    //     return guard.Connection;
    // }
    //
    // #endregion

    #region IDisposable members

    /// <inheritdoc cref="IDisposable.Dispose" />
    public void Dispose()
    {
        Magna.Logger.LogTrace (nameof (PoolGuard) + "::" + nameof (Dispose));
        Pool.ReleaseConnection (Connection);
    }

    #endregion
}
