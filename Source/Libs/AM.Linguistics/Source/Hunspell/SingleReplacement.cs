// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global

/* SingleReplacement.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace AM.Linguistics.Hunspell;

/// <summary>
///
/// </summary>
public sealed class SingleReplacement
    : ReplacementEntry
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="pattern"></param>
    /// <param name="outString"></param>
    /// <param name="type"></param>
    public SingleReplacement (string pattern, string outString, ReplacementValueType type)
        : base (pattern)
    {
        OutString = outString;
        Type = type;
    }

    /// <summary>
    ///
    /// </summary>
    public string OutString { get; }

    /// <summary>
    ///
    /// </summary>
    public ReplacementValueType Type { get; }

    /// <summary>
    ///
    /// </summary>
    public override string Med => this[ReplacementValueType.Med];

    /// <summary>
    ///
    /// </summary>
    public override string Ini => this[ReplacementValueType.Ini];

    /// <summary>
    ///
    /// </summary>
    public override string Fin => this[ReplacementValueType.Fin];

    /// <summary>
    ///
    /// </summary>
    public override string Isol => this[ReplacementValueType.Isol];

    /// <summary>
    ///
    /// </summary>
    /// <param name="type"></param>
    public override string this [ReplacementValueType type] => Type == type ? OutString : null!;
}
