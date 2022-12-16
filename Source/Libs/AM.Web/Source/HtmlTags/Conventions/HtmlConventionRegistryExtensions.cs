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

namespace AM.HtmlTags.Conventions;

using Elements;
using Elements.Builders;

public static class HtmlConventionRegistryExtensions
{
    public static HtmlConventionRegistry Defaults (this HtmlConventionRegistry registry) =>
        registry
            .DefaultBuilders()
            .DefaultModifiers()
            .DefaultNamingConvention();

    public static HtmlConventionRegistry DefaultNamingConvention (this HtmlConventionRegistry registry)
    {
        registry.Editors.NamingConvention (new DotNotationElementNamingConvention());
        registry.Labels.NamingConvention (new DotNotationElementNamingConvention());

        return registry;
    }

    public static HtmlConventionRegistry DefaultModifiers (this HtmlConventionRegistry registry)
    {
        registry.Editors.Modifier<AddNameModifier>();

        registry.Editors.Modifier<AddIdModifier>();

        return registry;
    }

    public static HtmlConventionRegistry DefaultBuilders (this HtmlConventionRegistry registry)
    {
        registry.Editors.BuilderPolicy<CheckboxBuilder>();

        registry.Editors.Always.BuildBy<TextboxBuilder>();

        registry.Displays.Always.BuildBy<SpanDisplayBuilder>();

        registry.Labels.Always.BuildBy<DefaultLabelBuilder>();

        return registry;
    }
}
