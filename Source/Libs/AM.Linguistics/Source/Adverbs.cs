// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global

/* Adverbs.cs -- наречия
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

#nullable enable

namespace AM.Linguistics;

/// <summary>
/// Наречия
/// </summary>
public static class Adverbs
{
    public static Adverb FindOne (string sourceForm)
    {
        return new Adverb { Word = sourceForm };
    }
}

/// <summary>
/// Наречие
/// </summary>
public class Adverb
{
    public string Word { get; internal set; }

    /// <summary>
    /// Разряд наречия
    /// </summary>
    public Comparability Comparability
    {
        get
        {
            if (Word[Word.Length - 1] == 'о')
                return Comparability.Comparable;
            else
                return Comparability.Incomparable;
        }
    }

//долго-дольше
//круто-круче
//свято-свяче
//легко-легче
//мягко-мягче
//мелко-мельче
//хорошо-лучше
//плохо-хуже
//высоко-выше
//низко-ниже
//далеко-дальше
//трудно-труднее трудней

    public string this [Comparison comp]
    {
        get
        {
            var c = Word[Word.Length - 1];
            var @base = Word.Substring (0, Word.Length - 1);
            var po = "по";
            if (char.IsUpper (@base[0]))
            {
                po = "По";
                @base = @base.ToLowerInvariant();
            }

            switch (c)
            {
                case 'о':
                    switch (comp)
                    {
                        case Comparison.Comparative1: return @base + "ее";
                        case Comparison.Comparative2: return po + @base + "ее";
                        case Comparison.Comparative3: return @base + "ей";
                        case Comparison.Comparative4: return po + @base + "ей";
                        case Comparison.Comparative5: return "более " + @base;
                    }

                    return "";
                default:
                    return "";
            }
        }
    }
}
