// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* IrbisCatalog.cs -- реализация интерфейса электронного каталога
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using AM;

using Istu.BookSupply.Interfaces;

using ManagedIrbis;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Providers;

#endregion

#nullable enable

namespace Istu.BookSupply.Implementation;

/// <summary>
/// Реализация интерфейса электронного каталога
/// для подисистемы книгообеспеченности
/// на основе подключения к серверу ИРБИС64.
/// </summary>
public sealed class IrbisCatalog
    : ICatalog
{
    #region Properties

    /// <summary>
    /// Синхронный провайдер.
    /// </summary>
    public ISyncProvider Provider { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public IrbisCatalog()
    {
        Provider = ConnectionUtility.GetConnectionFromConfig();
    }

    /// <summary>
    /// Конструктор для служебных целей.
    /// </summary>
    private IrbisCatalog
        (
            ISyncProvider provider
        )
    {
        Provider = provider;
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Получение интерфейса из провайдера.
    /// </summary>
    public static ICatalog FromProvider
        (
            ISyncProvider provider
        )
    {
        Sure.NotNull (provider);

        return new IrbisCatalog (provider);
    }

    #endregion

    #region ICatalog members

    /// <inheritdoc cref="ICatalog.FormatRecord"/>
    public string? FormatRecord
        (
            int mfn,
            string? format = null
        )
    {
        Sure.Positive (mfn);

        return Provider.FormatRecord (format ?? IrbisFormat.Brief, mfn);
    }

    /// <inheritdoc cref="ICatalog.ListTerms"/>
    public string[] ListTerms
        (
            string prefix
        )
    {
        Sure.NotNullNorEmpty (prefix);

        throw new NotImplementedException();
    }

    /// <inheritdoc cref="ICatalog.ReadRecord"/>
    public Record? ReadRecord
        (
            int mfn
        )
    {
        Sure.Positive (mfn);

        return Provider.ReadRecord (mfn);
    }

    /// <inheritdoc cref="ICatalog.SearchRecords"/>
    public int[] SearchRecords
        (
            string expression
        )
    {
        Sure.NotNullNorEmpty (expression);

        return Provider.Search (expression);
    }

    #endregion

    #region IDisposable members

    /// <inheritdoc cref="IDisposable.Dispose"/>
    public void Dispose() => Provider.Dispose();

    #endregion
}
