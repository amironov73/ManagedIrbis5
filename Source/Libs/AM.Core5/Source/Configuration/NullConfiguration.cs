// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* NullConfiguration.cs -- конфигурация, не выполняющая никаких полезных действий
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

#endregion

namespace AM.Configuration;

/// <summary>
/// Конфигурация, не выполняющая никаких полезных действий.
/// </summary>
public sealed class NullConfiguration
    : IConfiguration
{
    #region IConfiguration members

    /// <inheritdoc cref="IConfiguration.GetSection"/>
    public IConfigurationSection GetSection
        (
            string key
        )
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc cref="GetChildren"/>
    public IEnumerable<IConfigurationSection> GetChildren()
    {
        return Array.Empty<IConfigurationSection>();
    }

    /// <inheritdoc cref="IConfiguration.GetReloadToken"/>
    public IChangeToken GetReloadToken()
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc cref="IConfiguration.this"/>
    public string? this [string key]
    {
        get => default;
        set => throw new NotImplementedException();
    }

    #endregion
}
