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

/* HtmlTagHelper.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Linq;

using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.DependencyInjection;

#endregion

#nullable enable

namespace AM.HtmlTags;

#region Using directives

using Conventions;

#endregion

/// <summary>
///
/// </summary>
public abstract class HtmlTagTagHelper
    : TagHelper
{
    /// <summary>
    ///
    /// </summary>
    public const string ForAttributeName = "for";

    [HtmlAttributeName (ForAttributeName)] public ModelExpression For { get; set; }

    [ViewContext] [HtmlAttributeNotBound] public ViewContext ViewContext { get; set; }

    protected abstract string Category { get; }

    public override void Process (TagHelperContext context, TagHelperOutput output)
    {
        if (For == null)
        {
            throw new InvalidOperationException (
                "Missing or invalid 'for' attribute value. Specify a valid model expression for the 'for' attribute value.");
        }

        var request = new ElementRequest (new ModelMetadataAccessor (For))
        {
            Model = For.Model
        };

        var library = ViewContext.HttpContext.RequestServices.GetService<HtmlConventionLibrary>();

        var additionalServices = new object[]
        {
            For.ModelExplorer,
            ViewContext,
            new ElementName (For.Name)
        };

        object ServiceLocator (Type t) => additionalServices.FirstOrDefault (t.IsInstanceOfType) ??
                                          ViewContext.HttpContext.RequestServices.GetService (t);

        var tagGenerator = new TagGenerator (library.TagLibrary, new ActiveProfile(), ServiceLocator);

        var tag = tagGenerator.Build (request, Category);

        foreach (var attribute in output.Attributes)
        {
            tag.Attr (attribute.Name, attribute.Value);
        }

        output.TagName = null;
        output.PreElement.AppendHtml (tag);
    }
}
