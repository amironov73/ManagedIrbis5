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

using System.IO;
using System.Text.Encodings.Web;

#endregion

#nullable enable

namespace AM.HtmlTags;

/// <summary>
///     HtmlTag that *only outputs the literal html put into it in the
///     constructor function
/// </summary>
public class LiteralTag : HtmlTag
{
    public LiteralTag (string html) : base ("div")
    {
        Text (html);
        Encoded (false);
    }

    protected override void WriteHtml (TextWriter html, HtmlEncoder encoder) => html.Write (Text());
}

public static class LiteralTagExtensions
{
    /// <summary>
    ///     Adds a LiteralTag to the Children collection
    /// </summary>
    /// <param name="tag"></param>
    /// <param name="html"></param>
    /// <returns></returns>
    public static HtmlTag AppendHtml (this HtmlTag tag, string html) => tag.Append (new LiteralTag (html));
}
