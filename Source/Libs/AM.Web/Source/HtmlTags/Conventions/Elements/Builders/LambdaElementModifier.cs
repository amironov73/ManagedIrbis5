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
public class LambdaElementModifier : LambdaTagModifier, IElementModifier
{
    public LambdaElementModifier (Func<ElementRequest, bool> matcher, Action<ElementRequest> modify)
        : base (matcher, modify)
    {
    }

    public LambdaElementModifier (Action<ElementRequest> modify) : base (modify)
    {
    }

    public string ConditionDescription { get; set; }
    public string ModifierDescription { get; set; }
}
