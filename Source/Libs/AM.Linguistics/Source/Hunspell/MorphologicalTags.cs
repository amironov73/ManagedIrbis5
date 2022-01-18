// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global

/* .cs --
 * Ars Magna project, http://arsmagna.ru
 */

namespace AM.Linguistics.Hunspell;

public static class MorphologicalTags
{
    public static readonly string Stem = "st:";
    public static readonly string AlloMorph = "al:";
    public static readonly string Pos = "po:";
    public static readonly string DeriPfx = "dp:";
    public static readonly string InflPfx = "ip:";
    public static readonly string TermPfx = "tp:";
    public static readonly string DeriSfx = "ds:";
    public static readonly string InflSfx = "is:";
    public static readonly string TermSfx = "ts:";
    public static readonly string SurfPfx = "sp:";
    public static readonly string Freq = "fr:";
    public static readonly string Phon = "ph:";
    public static readonly string Hyph = "hy:";
    public static readonly string Part = "pa:";
    public static readonly string Flag = "fl:";
    public static readonly string HashEntry = "_H:";
    public static readonly int TagLength = Stem.Length;
}