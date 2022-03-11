// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable ReplaceSliceWithRangeIndexer
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local

/* PollyClientSocket.cs -- Polly-обертка над клиентским сокетом
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM;

using Polly;

#endregion

#nullable enable

namespace ManagedIrbis.Infrastructure.Sockets;

/// <summary>
/// Polly-обертка над клиентским сокетом.
/// </summary>
public sealed class PollyClientSocket
    : ISyncClientSocket
{
    #region Properties

    /// <summary>
    /// Политика обработки сбоев.
    /// </summary>
    public ISyncPolicy Policy { get; }

    /// <summary>
    /// Обрабатываемое подключение.
    /// </summary>
    public ISyncClientSocket InnerSocket { get; }

    /// <inheritdoc cref="ISyncClientSocket.RetryCount"/>.
    public int RetryCount { get; set; }

    /// <inheritdoc cref="ISyncClientSocket.RetryDelay"/>
    public int RetryDelay { get; set; }

    /// <inheritdoc cref="ISyncClientSocket.Connection"/>
    public ISyncConnection? Connection { get; set; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public PollyClientSocket
        (
            ISyncPolicy policy,
            ISyncClientSocket innerSocket
        )
    {
        Sure.NotNull (policy);
        Sure.NotNull (innerSocket);

        Policy = policy;
        InnerSocket = innerSocket;
        Connection = innerSocket.Connection.ThrowIfNull();
    }

    #endregion

    #region Private members

    private Response _TransactSync
        (
            SyncQuery query
        )
    {
        var result = InnerSocket.TransactSync (query);

        return result.ThrowIfNull();
    }

    #endregion

    #region ISyncClientSocket members

    /// <inheritdoc cref="ISyncClientSocket.TransactSync"/>
    public Response? TransactSync
        (
            SyncQuery query
        )
    {
        return Policy.Execute (() => _TransactSync (query));
    }

    #endregion
}
