// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

/* MorphologicalTags.cs --
 * Ars Magna project, http://arsmagna.ru
 */

namespace AM.Linguistics.Hunspell;

/// <summary>
///
/// </summary>
public static class MorphologicalTags
{
    /// <summary>
    ///
    /// </summary>
    public static readonly string Stem = "st:";

    /// <summary>
    ///
    /// </summary>
    public static readonly string AlloMorph = "al:";

    /// <summary>
    ///
    /// </summary>
    public static readonly string Pos = "po:";

    /// <summary>
    ///
    /// </summary>
    public static readonly string DeriPfx = "dp:";

    /// <summary>
    ///
    /// </summary>
    public static readonly string InflPfx = "ip:";

    /// <summary>
    ///
    /// </summary>
    public static readonly string TermPfx = "tp:";

    /// <summary>
    ///
    /// </summary>
    public static readonly string DeriSfx = "ds:";

    /// <summary>
    ///
    /// </summary>
    public static readonly string InflSfx = "is:";

    /// <summary>
    ///
    /// </summary>
    public static readonly string TermSfx = "ts:";

    /// <summary>
    ///
    /// </summary>
    public static readonly string SurfPfx = "sp:";

    /// <summary>
    ///
    /// </summary>
    public static readonly string Freq = "fr:";

    /// <summary>
    ///
    /// </summary>
    public static readonly string Phon = "ph:";

    /// <summary>
    ///
    /// </summary>
    public static readonly string Hyph = "hy:";

    /// <summary>
    ///
    /// </summary>
    public static readonly string Part = "pa:";

    /// <summary>
    ///
    /// </summary>
    public static readonly string Flag = "fl:";

    /// <summary>
    ///
    /// </summary>
    public static readonly string HashEntry = "_H:";

    /// <summary>
    ///
    /// </summary>
    public static readonly int TagLength = Stem.Length;
}
