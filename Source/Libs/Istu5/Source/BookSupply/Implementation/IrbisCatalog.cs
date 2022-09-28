// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UseNameofExpression

/* IrbisCatalog.cs -- реализация интерфейса электронного каталога
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

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
    public ISyncProvider Provider => _provider;

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public IrbisCatalog()
    {
        _provider = ConnectionUtility.GetConnectionFromConfig();
    }

    /// <summary>
    /// Конструктор для служебных целей.
    /// </summary>
    private IrbisCatalog
        (
            ISyncProvider provider
        )
    {
        _provider = provider;
    }

    #endregion

    #region Private members

    private readonly ISyncProvider _provider;

    #endregion

    #region Public methods

    /// <summary>
    /// Получение интерфейса из провайдера.
    /// </summary>
    public static ICatalog FromProvider (ISyncProvider provider) =>
        new IrbisCatalog (provider);

    #endregion

    #region ICatalog members

    /// <inheritdoc cref="ICatalog.FormatRecord"/>
    public string? FormatRecord (int mfn, string? format = null) =>
        _provider.FormatRecord (format ?? IrbisFormat.Brief, mfn);

    /// <inheritdoc cref="ICatalog.ListTerms"/>
    public string[] ListTerms (string prefix) => throw new NotImplementedException();

    /// <inheritdoc cref="ICatalog.ReadRecord"/>
    public Record? ReadRecord (int mfn) => _provider.ReadRecord (mfn);

    /// <inheritdoc cref="ICatalog.SearchRecords"/>
    public int[] SearchRecords (string expression) => _provider.Search (expression);

    #endregion

    #region IDisposable members

    /// <inheritdoc cref="IDisposable.Dispose"/>
    public void Dispose() => _provider.Dispose();

    #endregion
}
