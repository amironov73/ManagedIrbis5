// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global

/* MultiReplacementEntry.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;

using AM.Linguistics.Hunspell.Infrastructure;

#endregion

#nullable enable

namespace AM.Linguistics.Hunspell;

/// <summary>
///
/// </summary>
public sealed class MultiReplacementEntry
    : ReplacementEntry
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="pattern"></param>
    public MultiReplacementEntry (string pattern)
        : base (pattern)
    {
        med = null!;
        ini = null!;
        fin = null!;
        isol = null!;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="pattern"></param>
    /// <param name="type"></param>
    /// <param name="value"></param>
    public MultiReplacementEntry (string pattern, ReplacementValueType type, string value)
        : base (pattern)
    {
        med = null!;
        ini = null!;
        fin = null!;
        isol = null!;

        Set (type, value);
    }

    private string med;
    private string ini;
    private string fin;
    private string isol;

    /// <summary>
    ///
    /// </summary>
    public override string Med => med;

    /// <summary>
    ///
    /// </summary>
    public override string Ini => ini;

    /// <summary>
    ///
    /// </summary>
    public override string Fin => fin;

    /// <summary>
    ///
    /// </summary>
    public override string Isol => isol;

    /// <summary>
    ///
    /// </summary>
    /// <param name="type"></param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public override string this [ReplacementValueType type]
    {
        get
        {
            switch (type)
            {
                case ReplacementValueType.Med: return med;
                case ReplacementValueType.Ini: return ini;
                case ReplacementValueType.Fin: return fin;
                case ReplacementValueType.Isol: return isol;
                default: throw new ArgumentOutOfRangeException (nameof (type));
            }
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="type"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public MultiReplacementEntry With (ReplacementValueType type, string value)
    {
        var result = Clone();
        result.Set (type, value);
        return result;
    }

    internal void Set (ReplacementValueType type, string value)
    {
        switch (type)
        {
            case ReplacementValueType.Med:
                med = value;
                break;
            case ReplacementValueType.Ini:
                ini = value;
                break;
            case ReplacementValueType.Fin:
                fin = value;
                break;
            case ReplacementValueType.Isol:
                isol = value;
                break;
            default: throw new ArgumentOutOfRangeException (nameof (type));
        }
    }

    internal MultiReplacementEntry Clone()
    {
        return new (Pattern)
        {
            med = med,
            ini = ini,
            fin = fin,
            isol = isol
        };
    }
}

internal static class MultiReplacementEntryExtensions
{
    public static bool AddReplacementEntry
        (
            this Dictionary<string, MultiReplacementEntry> list,
            string pattern1,
            string? pattern2
        )
    {
        if (string.IsNullOrEmpty (pattern1) || pattern2 == null)
        {
            return false;
        }

        var pattern1Builder = StringBuilderPool.Get (pattern1);
        ReplacementValueType type;
        var trailingUnderscore = pattern1Builder.EndsWith ('_');
        if (pattern1Builder.StartsWith ('_'))
        {
            if (trailingUnderscore)
            {
                type = ReplacementValueType.Isol;
                pattern1Builder.Remove (pattern1Builder.Length - 1, 1);
            }
            else
            {
                type = ReplacementValueType.Ini;
            }

            pattern1Builder.Remove (0, 1);
        }
        else
        {
            if (trailingUnderscore)
            {
                type = ReplacementValueType.Fin;
                pattern1Builder.Remove (pattern1Builder.Length - 1, 1);
            }
            else
            {
                type = ReplacementValueType.Med;
            }
        }

        pattern1Builder.Replace ('_', ' ');

        pattern1 = StringBuilderPool.GetStringAndReturn (pattern1Builder);
        pattern2 = pattern2.Replace ('_', ' ');

        // find existing entry
        if (list.TryGetValue (pattern1, out var entry))
        {
            entry.Set (type, pattern2);
        }
        else

            // make a new entry if none exists
        {
            entry = new MultiReplacementEntry (pattern1, type, pattern2);
        }

        list[pattern1] = entry;

        return true;
    }
}
