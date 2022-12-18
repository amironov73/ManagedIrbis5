// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* TagBuilderExtensions.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace AM.HtmlTags.Extended.TagBuilders;

/// <summary>
///
/// </summary>
public static class TagBuilderExtensions
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="tag"></param>
    /// <param name="configure"></param>
    /// <returns></returns>
    public static HtmlTag Span
        (
            this HtmlTag tag,
            Action<HtmlTag> configure
        )
    {
        Sure.NotNull (tag);
        Sure.NotNull (configure);

        var span = new HtmlTag ("span");
        configure (span);
        return tag.Append (span);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="tag"></param>
    /// <param name="configure"></param>
    /// <returns></returns>
    public static HtmlTag Div
        (
            this HtmlTag tag,
            Action<HtmlTag> configure
        )
    {
        Sure.NotNull (tag);
        Sure.NotNull (configure);

        var div = new HtmlTag ("div");
        configure (div);

        return tag.Append (div);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="tag"></param>
    /// <param name="text"></param>
    /// <param name="classes"></param>
    /// <returns></returns>
    public static LinkTag ActionLink
        (
            this HtmlTag tag,
            string text,
            params string[] classes
        )
    {
        Sure.NotNull (tag);
        Sure.NotNullNorEmpty (text);
        Sure.NotNull (classes);

        var child = new LinkTag (text, "#", classes);
        tag.Append (child);

        return child;
    }
}
