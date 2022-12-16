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

namespace AM.HtmlTags.Extended
{
    namespace TagBuilders
    {
        public static class TagBuilderExtensions
        {
            public static HtmlTag Span (this HtmlTag tag, Action<HtmlTag> configure)
            {
                var span = new HtmlTag ("span");
                configure (span);
                return tag.Append (span);
            }

            public static HtmlTag Div (this HtmlTag tag, Action<HtmlTag> configure)
            {
                var div = new HtmlTag ("div");
                configure (div);

                return tag.Append (div);
            }

            public static LinkTag ActionLink (this HtmlTag tag, string text, params string[] classes)
            {
                var child = new LinkTag (text, "#", classes);
                tag.Append (child);

                return child;
            }
        }
    }
}
