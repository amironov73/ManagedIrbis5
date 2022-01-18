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

#nullable enable

namespace AM.Linguistics.Hunspell;

public abstract class ReplacementEntry
{
    protected ReplacementEntry (string pattern)
    {
        Pattern = pattern ?? string.Empty;
    }

    public string Pattern { get; }

    /// <seealso cref="ReplacementValueType.Med"/>
    public abstract string Med { get; }

    /// <seealso cref="ReplacementValueType.Ini"/>
    public abstract string Ini { get; }

    /// <seealso cref="ReplacementValueType.Fin"/>
    public abstract string Fin { get; }

    /// <seealso cref="ReplacementValueType.Isol"/>
    public abstract string Isol { get; }

    public abstract string this [ReplacementValueType type] { get; }

    internal string ExtractReplacementText (int remainingCharactersToReplace, bool atStart)
    {
        var type = remainingCharactersToReplace == Pattern.Length
            ? ReplacementValueType.Fin
            : ReplacementValueType.Med;

        if (atStart) type |= ReplacementValueType.Ini;

        while (type != ReplacementValueType.Med && string.IsNullOrEmpty (this[type]))
            type = type == ReplacementValueType.Fin && !atStart ? ReplacementValueType.Med : type - 1;

        return this[type];
    }
}
