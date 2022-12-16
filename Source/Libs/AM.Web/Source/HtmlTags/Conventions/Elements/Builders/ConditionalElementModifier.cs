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

namespace AM.HtmlTags.Conventions.Elements.Builders;

using System;

// Tested through HtmlConventionRegistry
public class ConditionalElementModifier : IElementModifier
{
    private readonly Func<ElementRequest, bool> _filter;
    private readonly IElementModifier _inner;

    public ConditionalElementModifier (Func<ElementRequest, bool> filter, IElementModifier inner)
    {
        _filter = filter;
        _inner = inner;
    }

    public string ConditionDescription { get; set; }


    public bool Matches (ElementRequest token) => _filter (token);

    public void Modify (ElementRequest request) => _inner.Modify (request);
}
