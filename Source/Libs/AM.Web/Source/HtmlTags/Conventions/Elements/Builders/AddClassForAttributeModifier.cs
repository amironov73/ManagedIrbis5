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

using Reflection;

public class AddClassForAttributeModifier<T> : IElementModifier where T : Attribute
{
    private readonly string _className;

    public AddClassForAttributeModifier (string className)
    {
        _className = className;
    }

    public bool Matches (ElementRequest token) => token.Accessor.HasAttribute<T>();

    public void Modify (ElementRequest request) => request.CurrentTag.AddClass (_className);
}
