// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

/* Adverbs.cs -- наречия
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace AM.Linguistics;

/// <summary>
/// Наречия
/// </summary>
public static class Adverbs
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="sourceForm"></param>
    /// <returns></returns>
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
    /// <summary>
    ///
    /// </summary>
    public string Word { get; internal set; } = null!;

    /// <summary>
    /// Разряд наречия
    /// </summary>
    public Comparability Comparability =>
        Word[^1] == 'о'
            ? Comparability.Comparable
            : Comparability.Incomparable;

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

    /// <summary>
    ///
    /// </summary>
    /// <param name="comp"></param>
    public string this [Comparison comp]
    {
        get
        {
            var c = Word[^1];
            var @base = Word[..^1];
            var po = "по";
            if (char.IsUpper (@base[0]))
            {
                po = "По";
                @base = @base.ToLowerInvariant();
            }

            switch (c)
            {
                case 'о':
                    return comp switch
                    {
                        Comparison.Comparative1 => @base + "ее",
                        Comparison.Comparative2 => po + @base + "ее",
                        Comparison.Comparative3 => @base + "ей",
                        Comparison.Comparative4 => po + @base + "ей",
                        Comparison.Comparative5 => "более " + @base,
                        _ => ""
                    };

                default:
                    return "";
            }
        }
    }
}
