// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global

/* EnglishHyphenator.cs -- simple hyphenator for English language
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;

#endregion

#nullable enable

namespace AM.Text.Hyphenation;

/// <summary>
/// Simple <see cref="Hyphenator"/> for English language.
/// </summary>
public class EnglishHyphenator
    : Hyphenator
{
    #region Private members

    private static readonly char[] _vowels =
    {
        'a', 'e', 'i', 'o', 'u', 'y'
    };

    private static readonly char[] _consonants =
    {
        'b', 'c', 'd', 'f', 'g', 'h', 'j', 'k', 'l', 'm',
        'n', 'p', 'q', 'r', 's', 't', 'v', 'w', 'x', 'z'
    };

    private static readonly string[] _prefixes =
    {
        "dis", "per", "pre", "sub"
    };

    private static bool _IsVowel (string str, int index)
    {
        return (Array.IndexOf (_vowels, str[index]) >= 0);
    }

    private static bool _IsConsonant (string str, int index)
    {
        return (Array.IndexOf (_consonants, str[index]) >= 0);
    }

    #endregion

    #region Hyphenator members

    /// <summary>
    /// Gets the language name ("English" for this case).
    /// </summary>
    /// <value>The name of the language ("english for this case).
    /// </value>
    public override string LanguageName => "English";

    /// <summary>
    /// Determines whether the <see cref="Hyphenator"/>
    /// can split specified word.
    /// </summary>
    /// <param name="theWord">Word to check.</param>
    /// <returns>
    /// 	<c>true</c> if word can be processed;
    /// otherwise <c>false</c>.
    /// </returns>
    public override bool RecognizeWord
        (
            string theWord
        )
    {
        Magna.Error
            (
                "EnglishHyphenator::RecognizeWord: "
                + "not implemented"
            );

        throw new NotImplementedException();
    }

    /// <summary>
    /// Hyphenates the word.
    /// </summary>
    /// <param name="word">Word to hyphenate.</param>
    /// <returns>
    /// Array of positions where hyphen can be inserted.
    /// </returns>
    public override int[] Hyphenate
        (
            string? word
        )
    {
        if (string.IsNullOrEmpty (word)
            || word.Length < 4)
        {
            return Array.Empty<int>();
        }

        // Нельзя переносить слова, содержащие прописные буквы
        // (кроме первой, разумеется).
        for (var i = 1; i < word.Length; i++)
        {
            if (char.IsUpper (word, i))
            {
                return Array.Empty<int>();
            }
        }

        var result = new List<int>();
        var len = word.Length - 2;

        // Можно переносить сразу за гласной
        for (var i = 1; i < len; i++)
        {
            if (_IsVowel (word, i))
            {
                // Если после гласной много согласных,
                // переносим по согласным
                if (i + 1 < len
                    && _IsConsonant (word, i + 1)
                    && _IsConsonant (word, i + 2)
                    && _IsConsonant (word, i + 3))
                {
                    result.Add (++i);
                    i++;
                }
                else
                {
                    result.Add (i);
                }
            }
        }

        // Можно переносить между двумя согласными
        for (var i = 1; i < len; i++)
        {
            if (_IsConsonant (word, i)
                && _IsConsonant (word, i + 1))
            {
                result.Add (i);
            }
        }

        result.Sort();

        // Отдаем предпочтение переносу по удвоенной согласной
        for (var i = 0; i < result.Count;)
        {
            var pos = result[i];
            if (_IsConsonant (word, pos + 1)
                && word[pos + 1] == word[pos + 2])
            {
                result.RemoveAt (i);
                continue;
            }

            if (pos > 2 && _IsConsonant (word, pos - 1) && word[pos - 1] == word[pos - 2])
            {
                result.RemoveAt (i);
                continue;
            }

            i++;
        }

        // Нельзя переносить после двух согласных подряд
        for (var i = 0; i < result.Count;)
        {
            var pos = result[i];
            if (pos > 2 && _IsConsonant (word, pos)
                        && _IsConsonant (word, pos - 1))
            {
                result.RemoveAt (i);
            }
            else
            {
                i++;
            }
        }

        result.Sort();

        // Нельзя разрывать приставку
        if (result.Count > 0)
        {
            foreach (string prefix in _prefixes)
            {
                if (word.StartsWith (prefix))
                {
                    result[0] = prefix.Length - 1;
                    break;
                }
            }
        }

        // Нельзя переносить часть слова, состоящую только
        // из согласных
        if (result.Count > 0)
        {
            var last = result[^1];
            var canBreak = false;
            for (var i = last + 1; i < word.Length; i++)
            {
                if (_IsVowel (word, i))
                {
                    canBreak = true;
                }
            }

            if (!canBreak)
            {
                result.Remove (last);
            }
        }

        return result
            .Distinct()
            .OrderBy (_ => _)
            .ToArray();
    }

    #endregion
}
