// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global

/* TagParser.cs -- разбирает текст подсказки на теги
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;

using AM.Text;

using JetBrains.Annotations;

#endregion

namespace AM.StableDiffusion.PromptEngineering;

/// <summary>
/// Разбирает текст подсказки на теги.
/// </summary>
[PublicAPI]
public sealed class TagParser
{
    #region Private members

    private static string ReadOneTag
        (
            TextNavigator navigator
        )
    {
        char chr;
        var accumulator = new ValueStringBuilder (stackalloc char[16]);
        while ((chr = navigator.PeekChar()) != TextNavigator.EOF)
        {
            var one = chr switch
            {
                <= ' ' => ' ',
                _ => chr
            };

            if (one is ' ')
            {
                // предотвращаем удвоение пробелов
                if (accumulator.LastChar is not ' ')
                {
                    accumulator.Append (one);
                }

                navigator.ReadChar();
            }
            else if (char.IsLetter (chr))
            {
                accumulator.Append (one);
                navigator.ReadChar();
            }
            else
            {
                break;
            }
        }

        while (accumulator.LastChar is ' ')
        {
            // убираем замыкающие пробелы
            accumulator.Length--;
        }

        return accumulator.ToString();
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Разбор текста подсказки.
    /// </summary>
    public List<TagInfo> Parse
        (
            string prompt
        )
    {
        var result = new List<TagInfo>();
        if (string.IsNullOrWhiteSpace (prompt))
        {
            return result;
        }

        var navigator = new TextNavigator (prompt);
        while (!navigator.IsEOF)
        {
            if (!navigator.SkipWhitespace())
            {
                break;
            }

            var chr = navigator.PeekChar();
            if (chr == '<')
            {
                // лора или что-нибудь в этом роде
                var title = navigator.ReadTo ('>').ToString();
                if (!string.IsNullOrEmpty (title))
                {
                    result.Add (new TagInfo { Title = title });
                }
            }
            else if (char.IsLetter (chr))
            {
                // похоже, это тег
                var title = ReadOneTag (navigator);
                if (!string.IsNullOrEmpty (title))
                {
                    result.Add (new TagInfo { Title = title });
                }
            }
            else
            {
                // это не тег, а служебный символ
                // например, открывающая скобка или запятая
                navigator.ReadChar();
                continue;
            }
        }

        return result;
    }

    #endregion
}
