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

using System;

#endregion

#nullable enable

namespace AM.HtmlTags.Conventions;

public interface ITagBuilderPolicy
{
    bool Matches (ElementRequest subject);
    ITagBuilder BuilderFor (ElementRequest subject);
}

public class ConditionalTagBuilderPolicy : ITagBuilderPolicy
{
    private readonly Func<ElementRequest, bool> _filter;
    private readonly ITagBuilder _builder;

    public ConditionalTagBuilderPolicy (Func<ElementRequest, bool> filter, Func<ElementRequest, HtmlTag> builder)
    {
        _filter = filter;
        _builder = new LambdaTagBuilder (builder);
    }

    public ConditionalTagBuilderPolicy (Func<ElementRequest, bool> filter, ITagBuilder builder)
    {
        _filter = filter;
        _builder = builder;
    }


    public bool Matches (ElementRequest subject) => _filter (subject);

    public ITagBuilder BuilderFor (ElementRequest subject) => _builder;
}

public interface ITagBuilder
{
    HtmlTag Build (ElementRequest request);
}

public abstract class TagBuilder : ITagBuilderPolicy, ITagBuilder
{
    public abstract bool Matches (ElementRequest subject);

    public ITagBuilder BuilderFor (ElementRequest subject) => this;

    public abstract HtmlTag Build (ElementRequest request);
}
