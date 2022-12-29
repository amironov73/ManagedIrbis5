// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* HtmlText.cs -- утилиты для работы с HTML
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Net;
using System.Text.RegularExpressions;

#endregion

#nullable enable

namespace AM.Text;

/// <summary>
/// Утилиты для работы с HTML.
/// </summary>
public static class HtmlText
{
    #region Public methods

    /// <summary>
    /// Encode HTML entities.
    /// </summary>
    public static string? Encode
        (
            string? text
        )
    {
        if (string.IsNullOrEmpty (text))
        {
            return text;
        }

        var builder = StringBuilderPool.Shared.Get();
        builder.EnsureCapacity (text.Length);

        foreach (var c in text)
        {
            switch (c)
            {
                case '"':
                    builder.Append ("&quot;");
                    break;

                case '#':
                    builder.Append ("&num;");
                    break;

                case '&':
                    builder.Append ("&amp;");
                    break;

                case '\'':
                    builder.Append ("&apos;");
                    break;

                case '<':
                    builder.Append ("&lt;");
                    break;

                case '>':
                    builder.Append ("&gt;");
                    break;

                case '\x00A0':
                    // non-breaking space
                    builder.Append ("&nbsp;");
                    break;

                case '\x00A2':
                    // cent sign
                    builder.Append ("&cent;");
                    break;

                case '\x00A3':
                    // pound sign
                    builder.Append ("&pound;");
                    break;

                case '\x00A5':
                    // yen sign
                    builder.Append ("&yen;");
                    break;

                case '\x00A7':
                    // section sign
                    builder.Append ("&sect;");
                    break;

                case '\x00A9':
                    // copyright sign
                    builder.Append ("&copy;");
                    break;

                case '\x00AD':
                    // soft hyphen
                    builder.Append ("&shy;");
                    break;

                case '\x00AE':
                    // registered sign
                    builder.Append ("&reg;");
                    break;

                case '\x20AC':
                    // euro sign
                    builder.Append ("&euro;");
                    break;

                default:
                    builder.Append (c);
                    break;
            }
        }

        return builder.ReturnShared();
    }

    /// <summary>
    /// Convert HTML to plain text by stripping tags.
    /// </summary>
    public static string? ToPlainText
        (
            string? html
        )
    {
        if (string.IsNullOrEmpty (html))
        {
            return html;
        }

        var result = Regex.Replace
            (
                html,
                @"<br\s*?/?>",
                Environment.NewLine
            );

        result = Regex.Replace
            (
                result,
                @"<.*?>",
                string.Empty
            );

        result = WebUtility.HtmlDecode (result);

        return result;
    }

    #endregion
}
