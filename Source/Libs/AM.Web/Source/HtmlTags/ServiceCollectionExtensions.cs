// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UseNameofExpression

/*
 * Ars Magna project, http://arsmagna.ru
 */

namespace Microsoft.Extensions.DependencyInjection;

using System;

using AM.HtmlTags;
using AM.HtmlTags.Conventions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Configures HtmlTags without ASP.NET Core defaults without modifying the library
    /// </summary>
    /// <param name="services">Service collection</param>
    /// <param name="library">Convention library</param>
    /// <returns>Service collection</returns>
    public static IServiceCollection
        AddHtmlTags (this IServiceCollection services, HtmlConventionLibrary library) =>
        services.AddSingleton (library);

    /// <summary>
    /// Configures HtmlTags with ASP.NET Core defaults
    /// </summary>
    /// <param name="services">Service collection</param>
    /// <param name="registries">Custom convention registries</param>
    /// <returns>Service collection</returns>
    public static IServiceCollection AddHtmlTags (this IServiceCollection services,
        params HtmlConventionRegistry[] registries)
    {
        var library = new HtmlConventionLibrary();
        var defaultRegistry = new HtmlConventionRegistry()
            .DefaultModifiers()
            .ModelMetadata();

        var defaultBuilders = new HtmlConventionRegistry()
            .DefaultBuilders()
            .ModelStateBuilders()
            .DefaultNamingConvention()
            .ModelStateNamingConvention();

        defaultRegistry.Apply (library);

        foreach (var registry in registries)
        {
            registry.Apply (library);
        }

        defaultBuilders.Apply (library);

        return services.AddHtmlTags (library);
    }

    /// <summary>
    /// Configures HtmlTags with ASP.NET Core defaults
    /// </summary>
    /// <param name="services">Service collection</param>
    /// <param name="config">Additional configuration callback</param>
    /// <returns>Service collection</returns>
    public static IServiceCollection AddHtmlTags (this IServiceCollection services,
        Action<HtmlConventionRegistry> config)
    {
        var registry = new HtmlConventionRegistry();

        config (registry);

        services.AddHtmlTags (registry);

        return services;
    }
}
