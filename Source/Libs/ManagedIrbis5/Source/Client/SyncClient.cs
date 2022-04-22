// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* SyncClient.cs -- синхронный клиент ИРБИС64
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using AM;
using AM.IO;
using AM.Runtime;

#endregion

#nullable enable

namespace ManagedIrbis.Client;

/// <summary>
/// Сихронный клиент ИРБИС64.
/// </summary>
public class SyncClient
    : IDisposable
{
    #region Properties

    /// <summary>
    /// Менеджер псевдонимов.
    /// </summary>
    public AliasManager Aliases { get; }

    /// <summary>
    /// Синхронный провайдер.
    /// </summary>
    public ISyncProvider Provider { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public SyncClient
        (
            ISyncProvider provider
        )
    {
        Sure.NotNull (provider);

        Provider = provider;
        Aliases = new AliasManager();
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Чтение записи.
    /// </summary>
    public Record? ReadRecord
        (
            int mfn
        )
    {
        throw new NotImplementedException();
    }

    #endregion

    #region IDisposable members

    /// <inheritdoc cref="IDisposable.Dispose"/>
    public void Dispose()
    {
        throw new NotImplementedException();
    }

    #endregion
}
