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

#region Using directives

using AM.HtmlTags.Conventions;
using AM.HtmlTags.Conventions.Elements;
using AM.HtmlTags.Conventions.Elements.Builders;

#endregion

#nullable enable

namespace AM.HtmlTags;

public static class ModelStateTagExtensions
{
    public static HtmlConventionRegistry ModelState (this HtmlConventionRegistry registry)
        => registry.ModelStateBuilders().ModelStateNamingConvention();

    public static HtmlConventionRegistry ModelStateNamingConvention (this HtmlConventionRegistry registry)
    {
        registry.ValidationMessages.NamingConvention (new DotNotationElementNamingConvention());

        return registry;
    }

    public static HtmlConventionRegistry ModelStateBuilders (this HtmlConventionRegistry registry)
    {
        registry.ValidationMessages.Always.BuildBy<DefaultValidationMessageBuilder>();

        return registry;
    }
}
