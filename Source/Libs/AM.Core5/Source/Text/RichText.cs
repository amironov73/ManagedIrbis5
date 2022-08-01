// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* RichText.cs -- работа с RTF
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Text;

#endregion

#nullable enable

namespace AM.Text;

/// <summary>
/// Работа с RTF.
/// </summary>
public static class RichText
{
    #region Properties

    /// <summary>
    /// Central European prologue for RTF file.
    /// </summary>
    public static string CentralEuropeanPrologue
        = @"{\rtf1\ansi\ansicpg1250\deff0\deflang1033"
          + @"{\fonttbl{\f0\fnil\fcharset238 MS Sans Serif;}}"
          + @"\viewkind4\uc1\pard\f0\fs16 ";

    /// <summary>
    /// Common prologue for RTF file.
    /// </summary>
    public static string CommonPrologue
        = @"{\rtf1\ansi\deff0"
          + @"{\fonttbl{\f0\fnil\fcharset0 MS Sans Serif;}}"
          + @"\viewkind4\uc1\pard\f0\fs16 ";


    /// <summary>
    /// Western European prologue for RTF file.
    /// </summary>
    public static string WesternEuropeanPrologue
        = @"{\rtf1\ansi\ansicpg1252\deff0\deflang1033"
          + @"{\fonttbl{\f0\fnil\fcharset0 MS Sans Serif;}}"
          + @"\viewkind4\uc1\pard\f0\fs16 ";

    /// <summary>
    /// Russian prologue for RTF file.
    /// </summary>
    public static string RussianPrologue
        = @"{\rtf1\ansi\ansicpg1251\deff0\deflang1049"
          + @"{\fonttbl{\f0\fnil\fcharset204 Times New Roman;}"
          + @"{\f1\fnil\fcharset238 Times New Roman;}}"
          + @"{\stylesheet{\s0\f0\fs24\snext0 Normal;}"
          + @"{\s1\f1\fs40\b\snext0 Heading;}}"
          + @"\viewkind4\uc1\pard\f0\fs16 ";

    #endregion

    #region Public methods

    /// <summary>
    /// Проверка парных фигурных скобок.
    /// Пустой текст считается валидным.
    /// </summary>
    public static bool CheckBraces
        (
            ReadOnlySpan<char> text
        )
    {
        var counter = 0;
        var escape = false;
        foreach (var chr in text)
        {
            if (chr == '\\')
            {
                // включаем экранирование следующего символа
                escape = true;
            }
            else
            {
                if (chr == '{')
                {
                    if (!escape)
                    {
                        ++counter;
                    }
                }
                else if (chr == '}')
                {
                    if (!escape)
                    {
                        if (--counter < 0)
                        {
                            return false;
                        }
                    }
                }

                escape = false;
            }
        }

        return !escape && counter == 0;
    }

    /// <summary>
    /// Decode text.
    /// </summary>
    public static string? Decode
        (
            string? text
        )
    {
        if (string.IsNullOrWhiteSpace (text))
        {
            return text;
        }

        var builder = StringBuilderPool.Shared.Get();
        builder.EnsureCapacity (text.Length);
        var navigator = new TextNavigator (text);
        while (!navigator.IsEOF)
        {
            var chunk = navigator.ReadUntil ('\\');
            builder.Append (chunk);
            var prefix = navigator.ReadChar();
            if (prefix != '\\')
            {
                break;
            }

            var c = navigator.ReadChar();
            if (c == '\0')
            {
                builder.Append (prefix);
                break;
            }

            if (c != 'u')
            {
                builder.Append (prefix);
                builder.Append (c);
                continue;
            }

            var buffer = new StringBuilder();
            while (!navigator.IsEOF)
            {
                c = navigator.ReadChar();
                if (!char.IsDigit (c))
                {
                    break;
                }

                buffer.Append (c);
            }

            if (buffer.Length != 0)
            {
                c = (char) int.Parse (buffer.ToString());
                builder.Append (c);
            }
        }

        var result = builder.ToString();
        StringBuilderPool.Shared.Return (builder);

        return result;
    }

    /// <summary>
    /// Encode text.
    /// </summary>
    public static string? Encode
        (
            string? text,
            UnicodeRange? goodRange
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
            if (c < 0x20)
            {
                builder.AppendFormat ("\\'{0:x2}", (byte) c);
            }
            else if (c < 0x80)
            {
                switch (c)
                {
                    case '{':
                        builder.Append ("\\{");
                        break;

                    case '}':
                        builder.Append ("\\}");
                        break;

                    case '\\':
                        builder.Append ("\\\\");
                        break;

                    default:
                        builder.Append (c);
                        break;
                }
            }
            else if (c < 0x100)
            {
                builder.AppendFormat ("\\'{0:x2}", (byte) c);
            }
            else
            {
                var simple = false;
                if (!ReferenceEquals (goodRange, null))
                {
                    if (c >= goodRange.From && c <= goodRange.To)
                    {
                        simple = true;
                    }
                }

                if (simple)
                {
                    builder.Append (c);
                }
                else
                {
                    // После \u следующий символ съедается
                    // поэтому подсовываем знак вопроса
                    builder.AppendFormat ("\\u{0}?", (short) c);
                }
            }
        }

        var result = builder.ToString();
        StringBuilderPool.Shared.Return (builder);

        return result;
    }

    /// <summary>
    /// Encode text.
    /// </summary>
    public static string? Encode2
        (
            string? text,
            UnicodeRange? goodRange
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
            if (c < 0x20)
            {
                builder.AppendFormat ("\\'{0:x2}", (byte) c);
            }
            else if (c < 0x80)
            {
                builder.Append (c);
            }
            else if (c < 0x100)
            {
                builder.AppendFormat ("\\'{0:x2}", (byte) c);
            }
            else
            {
                var simple = false;
                if (!ReferenceEquals (goodRange, null))
                {
                    if (c >= goodRange.From && c <= goodRange.To)
                    {
                        simple = true;
                    }
                }

                if (simple)
                {
                    builder.Append (c);
                }
                else
                {
                    // После \u следующий символ съедается
                    // поэтому подсовываем знак вопроса
                    builder.AppendFormat ("\\u{0}?", (int) c);
                }
            }
        }

        var result = builder.ToString();
        StringBuilderPool.Shared.Return (builder);

        return result;
    }

    /// <summary>
    /// Encode text.
    /// </summary>
    public static string? Encode3
        (
            string? text,
            UnicodeRange? goodRange,
            string? modeSwitch
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
            if (c < 0x20)
            {
                builder.AppendFormat ("\\'{0:x2}", (byte) c);
            }
            else if (c < 0x80)
            {
                builder.Append (c);
            }
            else if (c < 0x100)
            {
                builder.Append ('{');
                builder.AppendFormat ("{0}\\'{1:x2}", modeSwitch, (byte) c);
                builder.Append ('}');
            }
            else
            {
                var simple = false;
                if (!ReferenceEquals (goodRange, null))
                {
                    if (c >= goodRange.From && c <= goodRange.To)
                    {
                        simple = true;
                    }
                }

                if (simple)
                {
                    builder.Append (c);
                }
                else
                {
                    // После \u следующий символ съедается
                    // поэтому подсовываем знак вопроса
                    builder.AppendFormat ("\\u{0}?", (int) c);
                }
            }
        }

        var result = builder.ToString();
        StringBuilderPool.Shared.Return (builder);

        return result;
    }

    #endregion
}
