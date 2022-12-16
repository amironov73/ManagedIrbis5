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

public class LambdaTagModifier : ITagModifier
{
    private readonly Func<ElementRequest, bool> _matcher;
    private readonly Action<ElementRequest> _modify;

    public LambdaTagModifier (Func<ElementRequest, bool> matcher, Action<ElementRequest> modify)
    {
        _matcher = matcher;
        _modify = modify;
    }

    public LambdaTagModifier (Action<ElementRequest> modify)
        : this (x => true, modify)
    {
    }

    public bool Matches (ElementRequest token) => _matcher (token);

    public void Modify (ElementRequest request) => _modify (request);
}
