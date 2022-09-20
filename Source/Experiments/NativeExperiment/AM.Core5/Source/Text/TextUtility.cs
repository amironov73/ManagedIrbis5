// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* TextUtility.cs -- различные методы для работы с текстом
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Text;

#endregion

#nullable enable

namespace AM.Text;

/// <summary>
/// Различные методы для работы с текстом.
/// </summary>
public static class TextUtility
{
    #region Private members

    /// <summary>
    /// Ограничители, после которых нельзя ставить точку.
    /// </summary>
    private static readonly char[] _delimiters = { '!', '?', '.', ',' };

    #endregion

    #region Public methods

    /// <summary>
    /// Добавление заданного количества повторений указанного символа.
    /// </summary>
    public static StringBuilder AppendRepeat
        (
            this StringBuilder builder,
            char c,
            int count
        )
    {
        Sure.NotNull (builder);

        if (count > 0)
        {
            builder.EnsureCapacity (builder.Length + count);
            for (var i = 0; i < count; i++)
            {
                builder.Append (c);
            }
        }

        return builder;
    }

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

        var curly = text.Contains ('{') && text.Contains ('}');
        var angle = text.Contains ('<') && text.Contains ('>');

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
    /// Получение последнего символа.
    /// </summary>
    public static char GetLastChar
        (
            this StringBuilder builder
        )
    {
        Sure.NotNull (builder);

        return builder.Length == 0 ? '\0' : builder[^1];
    }

    /// <summary>
    /// Получение последнего непробельного символа.
    /// </summary>
    public static char LastNonSpaceChar
        (
            this StringBuilder builder
        )
    {
        Sure.NotNull (builder);

        var position = builder.Length - 1;
        while (true)
        {
            if (position < 0)
            {
                break;
            }

            var result = builder[position];
            if (!char.IsWhiteSpace (result))
            {
                return result;
            }

            --position;
        }

        return '\0';
    }

    /// <summary>
    /// Добавление точки в конец текста.
    /// </summary>
    public static StringBuilder AppendDot
        (
            this StringBuilder builder,
            string dot = ". ",
            char[]? delimiters = null
        )
    {
        Sure.NotNull (builder);

        delimiters ??= _delimiters;
        var lastChar = builder.LastNonSpaceChar();
        if (Array.IndexOf (delimiters, lastChar) < 0)
        {
            builder.TrimEnd();
            builder.Append (dot);
        }

        return builder;
    }

    /// <summary>
    /// Добавление текста с префиксом, начинающимся с точки.
    /// </summary>
    public static StringBuilder AppendWithDotPrefix
        (
            this StringBuilder builder,
            string? text,
            string? prefix
        )
    {
        Sure.NotNull (builder);

        if (!string.IsNullOrEmpty (text))
        {
            if (!string.IsNullOrEmpty (prefix))
            {
                if (prefix.StartsWith ('.'))
                {
                    builder.AppendDot (prefix);
                }
                else
                {
                    builder.Append (prefix);
                }
            }

            builder.Append (text);
        }

        return builder;
    }

    /// <summary>
    /// Добавление текста с префиксом, начинающимся с пробельного символа.
    /// </summary>
    public static StringBuilder AppendWithSpacePrefix
        (
            this StringBuilder builder,
            string? text,
            string? prefix
        )
    {
        Sure.NotNull (builder);

        if (!string.IsNullOrEmpty (text))
        {
            if (!string.IsNullOrEmpty (prefix))
            {
                if (char.IsWhiteSpace (builder.GetLastChar())
                    && char.IsWhiteSpace (prefix[0]))
                {
                    // лишний пробел не выводим
                    builder.Append (prefix.AsSpan()[1..]);
                }
                else
                {
                    builder.Append (prefix);
                }
            }

            builder.Append (text);
        }

        return builder;
    }

    /// <summary>
    /// Добавление пробела в конец текста, если последний символ не пробельный.
    /// К пустому тексту ничего не добавляется.
    /// </summary>
    public static StringBuilder AppendSpace
        (
            this StringBuilder builder
        )
    {
        Sure.NotNull (builder);

        if (builder.Length != 0)
        {
            var lastChar = builder[^1];
            if (!char.IsWhiteSpace (lastChar))
            {
                builder.Append (' ');
            }
        }

        return builder;
    }

    /// <summary>
    /// Удаление пробельных символов в конце текста.
    /// </summary>
    public static StringBuilder TrimEnd
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

        return builder;
    }

    /// <summary>
    /// Удаление пробельных символов в начале и в конце текста.
    /// </summary>
    public static StringBuilder Trim
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

        return builder;
    }

    /// <summary>
    /// Удаление указанных символов в начале и в конце текста.
    /// </summary>
    public static StringBuilder Trim
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

        return builder;
    }

    #endregion
}
