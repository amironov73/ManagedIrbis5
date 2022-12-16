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

// Tested through the tests for TagCategory and TagLibrary
public class CategoryExpression
{
    private readonly BuilderSet _parent;
    private readonly Func<ElementRequest, bool> _matcher;

    public CategoryExpression (BuilderSet parent, Func<ElementRequest, bool> matcher)
    {
        _parent = parent;
        _matcher = matcher;
    }

    public void Modify (Action<ElementRequest> modify) => _parent.Add (new LambdaTagModifier (_matcher, modify));

    public void Build (Func<ElementRequest, HtmlTag> build) =>
        _parent.Add (new ConditionalTagBuilderPolicy (_matcher, build));
}
