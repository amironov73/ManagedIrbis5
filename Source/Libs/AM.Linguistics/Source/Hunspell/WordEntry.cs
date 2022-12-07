// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable RedundantOverflowCheckingContext

/* WordEntry.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace AM.Linguistics.Hunspell;

internal sealed class WordEntry
    : IEquatable<WordEntry>
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static bool operator == (WordEntry? a, WordEntry? b)
    {
        return a?.Equals (b) ?? ReferenceEquals (b, null);
    }

    public static bool operator != (WordEntry? a, WordEntry? b) => !(a == b);

    public WordEntry
        (
            string word,
            FlagSet flags,
            MorphSet morphs,
            WordEntryOptions options
        )
        : this (word, new WordEntryDetail (flags, morphs, options))
    {
        // пустое тело конструктора
    }

    public WordEntry
        (
            string? word,
            WordEntryDetail? detail
        )
    {
        Word = word ?? string.Empty;
        Detail = detail ?? WordEntryDetail.Default;
    }

    /// <summary>
    ///
    /// </summary>
    public string Word { get; }

    /// <summary>
    ///
    /// </summary>
    public WordEntryDetail Detail { get; }

    public bool ContainsFlag (FlagValue flag) => Detail.ContainsFlag (flag);

    /// <inheritdoc cref="IEquatable{T}.Equals(T?)"/>
    public bool Equals
        (
            WordEntry? other
        )
    {
        if (ReferenceEquals (other, null)) return false;
        if (ReferenceEquals (this, other)) return true;

        return string.Equals (other.Word, Word, StringComparison.Ordinal)
               && other.Detail.Equals (Detail);
    }

    /// <inheritdoc cref="object.Equals(object?)"/>
    public override bool Equals
        (
            object? obj
        )
    {
        return Equals (obj as WordEntry);
    }

    /// <inheritdoc cref="object.GetHashCode"/>
    public override int GetHashCode()
    {
        return unchecked (Word.GetHashCode() ^ Detail.GetHashCode());
    }
}
