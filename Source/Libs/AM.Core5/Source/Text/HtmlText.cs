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

using System.Net;
using System.Text;
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
    /// Encode html entities.
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

        var result = new StringBuilder (text.Length);

        foreach (var c in text)
        {
            switch (c)
            {
                case '"':
                    result.Append ("&quot;");
                    break;

                case '#':
                    result.Append ("&num;");
                    break;

                case '&':
                    result.Append ("&amp;");
                    break;

                case '\'':
                    result.Append ("&apos;");
                    break;

                case '<':
                    result.Append ("&lt;");
                    break;

                case '>':
                    result.Append ("&gt;");
                    break;

                case '\x00A0':
                    // non-breaking space
                    result.Append ("&nbsp;");
                    break;

                case '\x00A2':
                    // cent sign
                    result.Append ("&cent;");
                    break;

                case '\x00A3':
                    // pound sign
                    result.Append ("&pound;");
                    break;

                case '\x00A5':
                    // yen sign
                    result.Append ("&yen;");
                    break;

                case '\x00A7':
                    // section sign
                    result.Append ("&sect;");
                    break;

                case '\x00A9':
                    // copyright sign
                    result.Append ("&copy;");
                    break;

                case '\x00AD':
                    // soft hyphen
                    result.Append ("&shy;");
                    break;

                case '\x00AE':
                    // registered sign
                    result.Append ("&reg;");
                    break;

                case '\x20AC':
                    // euro sign
                    result.Append ("&euro;");
                    break;

                default:
                    result.Append (c);
                    break;
            }
        }

        return result.ToString();
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
                @"<.*?>",
                string.Empty
            );

        result = WebUtility.HtmlDecode (result);

        return result;
    }

    #endregion
}
