// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* ProxyWorker.cs -- рабочая лошадка прокси
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using AM;
using AM.Text;

using Microsoft.Extensions.Hosting;

#endregion

#nullable enable

namespace ManagedIrbis.Proxying;

/// <summary>
/// Рабочая лошадка прокси.
/// </summary>
public sealed class ProxyWorker
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public ProxyWorker
        (
            IHost host
        )
    {
        Sure.NotNull (host);

        _host = host;
    }

    #endregion

    #region Private members

    private readonly IHost _host;

    #endregion

    #region Public methods

    /// <summary>
    /// Инициализация.
    /// </summary>
    public void Initialize()
    {

    }

    /// <summary>
    /// Запись о произошедшем исключении.
    /// </summary>
    public void LogException
        (
            Exception exception
        )
    {
        Sure.NotNull (exception);
    }

    #endregion
}
