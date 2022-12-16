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

public class AddIdModifier : IElementModifier
{
    public bool Matches (ElementRequest token) => true;

    public void Modify (ElementRequest request)
    {
        var tag = request.CurrentTag;
        if (tag.IsInputElement() && !tag.HasAttr ("id"))
        {
            tag.Id (DefaultIdBuilder.Build (request));
        }
    }
}
