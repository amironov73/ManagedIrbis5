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

namespace AM.HtmlTags;

public class DLTag : HtmlTag
{
    public DLTag()
        : base ("dl")
    {
    }

    public DLTag (Action<DLTag> configure)
        : this()
    {
        configure (this);
    }

    public DLTag AddDefinition (string term, string definition)
    {
        Add ("dt").Text (term);
        Add ("dd").Text (definition);

        return this;
    }
}
