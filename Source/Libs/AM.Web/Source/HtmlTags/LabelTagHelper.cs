﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
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

namespace AM.HtmlTags;

using Conventions.Elements;

using Microsoft.AspNetCore.Razor.TagHelpers;

[HtmlTargetElement ("label-tag", Attributes = ForAttributeName, TagStructure = TagStructure.WithoutEndTag)]
public class LabelTagHelper : HtmlTagTagHelper
{
    protected override string Category { get; } = ElementConstants.Label;
}
