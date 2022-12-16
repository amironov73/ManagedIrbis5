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

public class AddClassModifier : IElementModifier
{
    private readonly string _className;

    public AddClassModifier (string className)
    {
        _className = className;
    }

    public bool Matches (ElementRequest token) => true;

    public void Modify (ElementRequest request) => request.CurrentTag.AddClass (_className);
}
