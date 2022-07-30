// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* IrbisFormat.cs -- работа с форматами ИРБИС64
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM;
using AM.Text;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace ManagedIrbis.Infrastructure;

/// <summary>
/// Работа с форматами ИРБИС64.
/// </summary>
public static class IrbisFormat
{
    #region Constants

    /// <summary>
    /// Формат ALL.
    /// </summary>
    public const string All = "&uf('+0')";

    /// <summary>
    /// Формат BRIEF.
    /// </summary>
    public const string Brief = "@brief";

    /// <summary>
    /// Формат IBIS.
    /// </summary>
    public const string Ibis = "@ibiskw_h";

    /// <summary>
    /// Информационный формат.
    /// </summary>
    public const string Informational = "@info_w";

    /// <summary>
    /// Оптимизированный формат..
    /// </summary>
    public const string Optimized = "@";

    #endregion

    #region Public methods

    /// <summary>
    /// Удаление пробелов из формата.
    /// </summary>
    public static string? RemoveComments
        (
            string? text
        )
    {
        const char zero = '\0';

        if (string.IsNullOrEmpty (text))
        {
            return text;
        }

        if (!text.Contains ("/*"))
        {
            return text;
        }

        int index = 0, length = text.Length;
        var builder = StringBuilderPool.Shared.Get();
        var state = zero;

        while (index < length)
        {
            var c = text[index];

            switch (state)
            {
                case '\'':
                case '"':
                case '|':
                    if (c == state)
                    {
                        state = zero;
                    }

                    builder.Append (c);
                    break;

                default:
                    if (c == '/')
                    {
                        if (index + 1 < length && text[index + 1] == '*')
                        {
                            while (index < length)
                            {
                                c = text[index];
                                if (c == '\r' || c == '\n')
                                {
                                    builder.Append (c);
                                    break;
                                }

                                index++;
                            }
                        }
                        else
                        {
                            builder.Append (c);
                        }
                    }
                    else if (c is '\'' or '"' or '|')
                    {
                        state = c;
                        builder.Append (c);
                    }
                    else
                    {
                        builder.Append (c);
                    }

                    break;
            }

            index++;
        }

        var result = builder.ToString();
        StringBuilderPool.Shared.Return (builder);

        return result;
    }

    /// <summary>
    /// Подготовка динамического формата к отправке
    /// в составе клиентского запроса на сервер.
    /// </summary>
    /// <remarks>Dynamic format string
    /// mustn't contains comments and
    /// string delimiters (no matter
    /// real or IRBIS).
    /// </remarks>
    public static string? PrepareFormat
        (
            string? text
        )
    {
        if (string.IsNullOrEmpty (text))
        {
            return text;
        }

        text = RemoveComments (text);
        if (string.IsNullOrEmpty (text))
        {
            return text;
        }

        var length = text.Length;
        var flag = false;
        for (var i = 0; i < length; i++)
        {
            if (text[i] < ' ')
            {
                flag = true;
                break;
            }
        }

        if (!flag)
        {
            return text;
        }

        var builder = StringBuilderPool.Shared.Get();
        for (var i = 0; i < length; i++)
        {
            var c = text[i];
            if (c >= ' ')
            {
                builder.Append (c);
            }
        }

        var result = builder.ToString();
        StringBuilderPool.Shared.Return (builder);

        return result;
    }

    /// <summary>
    /// Верификация строки запроса.
    /// </summary>
    public static bool VerifyFormat
        (
            string? text,
            bool throwOnError
        )
    {
        if (string.IsNullOrEmpty (text))
        {
            Magna.Logger.LogError
                (
                    nameof (IrbisFormat) + "::" + nameof (VerifyFormat)
                    + ": format text is absent"
                );

            if (throwOnError)
            {
                throw new VerificationException ("format text is absent");
            }

            return false;
        }

        foreach (var c in text)
        {
            if (c < ' ')
            {
                Magna.Logger.LogError
                    (
                        nameof (IrbisFormat) + "::" + nameof (VerifyFormat)
                        + ": format contains forbidden symbols"
                    );
                if (throwOnError)
                {
                    throw new VerificationException ("format contains forbidden symbols");
                }

                return false;
            }
        }

        const char zero = '\0';
        var state = zero;
        var index = 0;
        var length = text.Length;
        while (index < length)
        {
            var c = text[index];

            switch (state)
            {
                case '\'':
                case '"':
                case '|':
                    if (c == state)
                    {
                        state = zero;
                    }

                    break;

                default:
                    if (c == '/' && index + 1 < length && text[index + 1] == '*')
                    {
                        Magna.Logger.LogError
                            (
                                nameof (IrbisFormat) + "::" + nameof (VerifyFormat)
                                + ": format contains comment"
                            );
                        if (throwOnError)
                        {
                            throw new VerificationException ("format contains comment");
                        }

                        return false;
                    }

                    if (c == '\'' || c == '"' || c == '|')
                    {
                        state = c;
                    }

                    break;
            }

            index++;
        }

        if (state != zero)
        {
            Magna.Logger.LogError
                (
                    nameof (IrbisFormat) + "::" + nameof (VerifyFormat)
                    + ": nonclosed literal"
                );
            if (throwOnError)
            {
                throw new VerificationException ("nonclosed literal");
            }

            return false;
        }

        return true;
    }

    #endregion
}
