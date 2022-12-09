// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global

/* WordEntryDetail.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace AM.Linguistics.Hunspell;

/// <summary>
///
/// </summary>
public class WordEntryDetail
    : IEquatable<WordEntryDetail>
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static bool operator ==
        (
            WordEntryDetail? a,
            WordEntryDetail? b
        )
    {
        return a?.Equals (b) ?? ReferenceEquals (b, null);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static bool operator != (WordEntryDetail? a, WordEntryDetail? b) => !(a == b);

    /// <summary>
    ///
    /// </summary>
    public static WordEntryDetail Default { get; } = new (FlagSet.Empty, MorphSet.Empty, WordEntryOptions.None);

    /// <summary>
    ///
    /// </summary>
    /// <param name="flags"></param>
    /// <param name="morphs"></param>
    /// <param name="options"></param>
    public WordEntryDetail
        (
            FlagSet? flags,
            MorphSet? morphs,
            WordEntryOptions options
        )
    {
        Flags = flags ?? FlagSet.Empty;
        Morphs = morphs ?? MorphSet.Empty;
        Options = options;
    }

    /// <summary>
    ///
    /// </summary>
    public FlagSet Flags { get; }

    /// <summary>
    ///
    /// </summary>
    public MorphSet Morphs { get; }

    /// <summary>
    ///
    /// </summary>
    public WordEntryOptions Options { get; }

    /// <summary>
    ///
    /// </summary>
    public bool HasFlags => Flags.HasItems;

    /// <summary>
    ///
    /// </summary>
    /// <param name="flag"></param>
    /// <returns></returns>
    public bool ContainsFlag (FlagValue flag) => Flags.Contains (flag);

    /// <summary>
    ///
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public bool ContainsAnyFlags (FlagValue a, FlagValue b) => Flags.ContainsAny (a, b);

    /// <summary>
    ///
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <param name="c"></param>
    /// <returns></returns>
    public bool ContainsAnyFlags (FlagValue a, FlagValue b, FlagValue c) => Flags.ContainsAny (a, b, c);

    /// <summary>
    ///
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <param name="c"></param>
    /// <param name="d"></param>
    /// <returns></returns>
    public bool ContainsAnyFlags (FlagValue a, FlagValue b, FlagValue c, FlagValue d) =>
        Flags.ContainsAny (a, b, c, d);

    /// <inheritdoc cref="IEquatable{T}.Equals(T?)"/>
    public bool Equals
        (
            WordEntryDetail? other
        )
    {
        if (ReferenceEquals (other, null))
        {
            return false;
        }

        if (ReferenceEquals (this, other))
        {
            return true;
        }

        return other.Options == Options
               && other.Flags.Equals (Flags)
               && other.Morphs.Equals (Morphs);
    }

    /// <inheritdoc cref="object.Equals(object?)"/>
    public override bool Equals (object? obj)
    {
        return obj is WordEntryDetail d && Equals (d);
    }

    /// <inheritdoc cref="object.GetHashCode"/>
    public override int GetHashCode()
    {
        return HashCode.Combine (Flags, Morphs, Options);
    }

    internal WordEntry ToEntry (string word) => new (word, this);
}
