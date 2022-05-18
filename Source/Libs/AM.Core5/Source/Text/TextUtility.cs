// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* TextUtility.cs -- различные методы для работы с текстом
 * Ars Magna project, http://arsmagna.ru
 */

using System.Text;

#nullable enable

namespace AM.Text;

/// <summary>
/// Различные методы для работы с текстом.
/// </summary>
public static class TextUtility
{
    #region Public methods

    /// <summary>
    /// Определяем, что за текст.
    /// </summary>
    public static TextKind DetermineTextKind
        (
            string? text
        )
    {
        if (string.IsNullOrEmpty (text))
        {
            return TextKind.PlainText;
        }

        if (text.StartsWith ("{") || text.EndsWith ("}"))
        {
            return TextKind.RichText;
        }

        if (text.StartsWith ("<") || text.EndsWith (">"))
        {
            return TextKind.Html;
        }

        var curly = text.Contains ("{") && text.Contains ("}");
        var angle = text.Contains ("<") && text.Contains (">");

        if (curly && !angle)
        {
            return TextKind.RichText;
        }

        if (angle && !curly)
        {
            return TextKind.Html;
        }

        return TextKind.PlainText;
    }

    /// <summary>
    /// Удаление пробельных символов в конце текста.
    /// </summary>
    public static void TrimEnd
        (
            this StringBuilder builder
        )
    {
        Sure.NotNull (builder);

        while (builder.Length > 1)
        {
            var last = builder.Length - 1;
            if (char.IsWhiteSpace (builder[last]))
            {
                builder.Remove (last, 1);
            }
            else
            {
                break;
            }
        }
    }

    /// <summary>
    /// Удаление пробельных символов в начале и в конце текста.
    /// </summary>
    public static void Trim
        (
            this StringBuilder builder
        )
    {
        Sure.NotNull (builder);

        while (builder.Length > 1)
        {
            if (char.IsWhiteSpace (builder[0]))
            {
                builder.Remove (0, 1);
            }
            else
            {
                break;
            }
        }

        while (builder.Length > 1)
        {
            var last = builder.Length - 1;
            if (char.IsWhiteSpace (builder[last]))
            {
                builder.Remove (last, 1);
            }
            else
            {
                break;
            }
        }
    }

    /// <summary>
    /// Удаление указанных символов в начале и в конце текста.
    /// </summary>
    public static void Trim
        (
            this StringBuilder builder,
            char[] whitespace
        )
    {
        Sure.NotNull (builder);
        Sure.NotNull (whitespace);

        while (builder.Length > 1)
        {
            if (builder[0].IsOneOf (whitespace))
            {
                builder.Remove (0, 1);
            }
            else
            {
                break;
            }
        }

        while (builder.Length > 1)
        {
            var last = builder.Length - 1;
            if (builder[last].IsOneOf (whitespace))
            {
                builder.Remove (last, 1);
            }
            else
            {
                break;
            }
        }
    }

    #endregion
}
