// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global

/* ServiceProviderUtility.cs -- полезные методы для IServiceProvider
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

#endregion

#nullable enable

namespace AM.ComponentModel;

/// <summary>
/// Полезные методы для <see cref="IServiceProvider"/>.
/// </summary>
public static class ServiceProviderUtility
{
    #region Public methods

    /// <summary>
    /// Создание пустого провайдера сервисов.
    /// </summary>
    public static IServiceProvider CreateNullProvider()
    {
        var container = new ServiceCollection();
        container.AddTransient<ILogger, NullLogger>();

        var result = container.BuildServiceProvider();

        return result;
    }

    #endregion
}
