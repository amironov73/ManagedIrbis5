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
public class ConditionalElementBuilder : IElementBuilderPolicy, ITagBuilder
{
    private readonly Func<ElementRequest, bool> _filter;
    private readonly IElementBuilder _inner;

    public ConditionalElementBuilder (Func<ElementRequest, bool> filter, IElementBuilder inner)
    {
        _filter = filter;
        _inner = inner;
    }

    public string ConditionDescription { get; set; }
    public bool Matches (ElementRequest subject) => _filter (subject);

    public ITagBuilder BuilderFor (ElementRequest subject) => this;

    public HtmlTag Build (ElementRequest request) => _inner.Build (request);
}
