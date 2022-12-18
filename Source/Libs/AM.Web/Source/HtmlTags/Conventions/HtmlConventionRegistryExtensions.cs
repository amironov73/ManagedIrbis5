// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming

/* HtmlConventionRegistryExtensions.cs --
 * Ars Magna project, http://arsmagna.ru
 */

namespace AM.HtmlTags.Conventions;

#region Using directives

using Elements;
using Elements.Builders;

#endregion

#nullable enable

/// <summary>
///
/// </summary>
public static class HtmlConventionRegistryExtensions
{
    #region Public methods

    /// <summary>
    ///
    /// </summary>
    /// <param name="registry"></param>
    /// <returns></returns>
    public static HtmlConventionRegistry Defaults
        (
            this HtmlConventionRegistry registry
        )
    {
        Sure.NotNull (registry);

        return registry
            .DefaultBuilders()
            .DefaultModifiers()
            .DefaultNamingConvention();
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="registry"></param>
    /// <returns></returns>
    public static HtmlConventionRegistry DefaultNamingConvention
        (
            this HtmlConventionRegistry registry
        )
    {
        Sure.NotNull (registry);

        registry.Editors.NamingConvention (new DotNotationElementNamingConvention());
        registry.Labels.NamingConvention (new DotNotationElementNamingConvention());

        return registry;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="registry"></param>
    /// <returns></returns>
    public static HtmlConventionRegistry DefaultModifiers
        (
            this HtmlConventionRegistry registry
        )
    {
        Sure.NotNull (registry);

        registry.Editors.Modifier<AddNameModifier>();
        registry.Editors.Modifier<AddIdModifier>();

        return registry;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="registry"></param>
    /// <returns></returns>
    public static HtmlConventionRegistry DefaultBuilders
        (
            this HtmlConventionRegistry registry
        )
    {
        Sure.NotNull (registry);

        registry.Editors.BuilderPolicy<CheckboxBuilder>();
        registry.Editors.Always.BuildBy<TextboxBuilder>();
        registry.Displays.Always.BuildBy<SpanDisplayBuilder>();
        registry.Labels.Always.BuildBy<DefaultLabelBuilder>();

        return registry;
    }

    #endregion
}
