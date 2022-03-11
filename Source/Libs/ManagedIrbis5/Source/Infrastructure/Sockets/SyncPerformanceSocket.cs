// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable ReplaceSliceWithRangeIndexer
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* SyncPerformanceSocket.cs -- синхронный сокет, собирающий данные о производительности
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics.Tracing;
using System.Net.Sockets;

using AM;

#endregion

#nullable enable

namespace ManagedIrbis.Infrastructure.Sockets;

/// <summary>
/// Синхронный сокет, собирающий данные о производительности.
/// </summary>
public sealed class SyncPerformanceSocket
    : SyncNestedSocket
{
    #region Properties

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public SyncPerformanceSocket
        (
            ISyncClientSocket innerSocket
        )
        : base(innerSocket)
    {
    } // constructor

    #endregion

    #region ISyncClientSocket members

    /// <inheritdoc cref="ISyncClientSocket.TransactSync"/>
    public override Response? TransactSync
        (
            SyncQuery query
        )
    {
        var result = base.TransactSync(query);

        // TODO implement

        return result;

    }

    #endregion

}
