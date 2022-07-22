﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedMember.Global

/* RussianHyphenator.cs -- simple hyphenator for Russian language
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace AM.Text.Hyphenation;

/// <summary>
/// Simple <see cref="Hyphenator"/> for Russian language.
/// </summary>
public class RussianHyphenator
    : Hyphenator
{
    #region Private members

    private static readonly char[] _vowels =
    {
        'а', 'е', 'ё', 'и', 'о', 'ы', 'у', 'ю', 'я'
    };

    private static readonly char[] _consonants =
    {
        'б', 'в', 'г', 'д', 'ж', 'з', 'к', 'л', 'м', 'н',
        'п', 'р', 'с', 'т', 'ф', 'х', 'ц', 'ч', 'ш', 'щ'
    };

    private static readonly char[] _forbidden =
    {
        'й', 'ь', 'ъ'
    };

    private static readonly string[] _prefixes =
    {
        "без", "бес", "дез", "дис", "контр", "над", "под", "раз",
        "рас", "сверх", "супер", "экс"
    };

    private static bool _IsVowel (string str, int index) =>
        Array.IndexOf (_vowels, str[index]) >= 0;

    private static bool _IsConsonant (string str, int index) =>
        Array.IndexOf (_consonants, str[index]) >= 0;

    private static bool _IsForbidden (string str, int index) =>
        Array.IndexOf (_forbidden, str[index]) >= 0;

    #endregion

    #region Hyphenator members

    /// <summary>
    ///
    /// </summary>
    public override string LanguageName => "Русский";

    /// <summary>
    ///
    /// </summary>
    /// <param name="theWord"></param>
    /// <returns></returns>
    public override bool RecognizeWord
        (
            string theWord
        )
    {
        Magna.Logger.LogError
            (
                nameof (RussianHyphenator) + "::" + nameof (RecognizeWord)
                + ": not implemented"
            );

        throw new NotImplementedException();
    }

    /// <summary>
    /// Переносы для русского языка.
    /// </summary>
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

        // Нельзя отрывать й, ь и ъ от предшествующих гласных
        for (var i = 0; i < result.Count; i++)
        {
            var pos = result[i];
            if (_IsForbidden (word, pos + 1))
            {
                result[i] = pos + 1;
            }
        }

        result.Sort();

        // Нельзя разрывать приставку. Также нельзя, чтобы
        // после приставки была буква й, ы, ь, ъ
        if (result.Count > 0)
        {
            foreach (var prefix in _prefixes)
            {
                if (word.StartsWith (prefix))
                {
                    int first;
                    if (_IsForbidden (word, prefix.Length)
                        || word[prefix.Length] == 'ы')
                    {
                        first = prefix.Length;
                    }
                    else
                    {
                        first = prefix.Length - 1;
                    }

                    result[0] = first;
                    break;
                }
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
