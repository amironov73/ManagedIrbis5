// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable ForCanBeConvertedToForeach
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable RedundantOverflowCheckingContext

/* CharacterSet.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using AM.Linguistics.Hunspell.Infrastructure;

#endregion

#nullable enable

namespace AM.Linguistics.Hunspell;

/// <summary>
///
/// </summary>
public sealed class CharacterSet
    : ArrayWrapper<char>
{
    /// <summary>
    ///
    /// </summary>
    public static readonly CharacterSet Empty = new (Array.Empty<char>());

    /// <summary>
    ///
    /// </summary>
    public static readonly ArrayWrapperComparer<char, CharacterSet> DefaultComparer = new ();

    /// <summary>
    ///
    /// </summary>
    /// <param name="values"></param>
    /// <returns></returns>
    public static CharacterSet Create (string values)
    {
        return values == null! ? Empty : TakeArray (values.ToCharArray());
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static CharacterSet Create (char value)
    {
        return TakeArray (new[] { value });
    }

    internal static CharacterSet Create (ReadOnlySpan<char> values)
    {
        return TakeArray (values.ToArray());
    }

    internal static CharacterSet TakeArray (char[] values)
    {


#if DEBUG
        if (values == null) throw new ArgumentNullException (nameof (values));
#endif

        Array.Sort (values);
        return new CharacterSet (values);
    }

    private CharacterSet (char[] values)
        : base (values)
    {
        mask = default;
        for (var i = 0; i < values.Length; i++)
        {
            unchecked
            {
                mask |= values[i];
            }
        }
    }

    private readonly char mask;

    /// <summary>
    ///
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public bool Contains (char value)
    {
        return unchecked ((value & mask) != default)
               &&
               Array.BinarySearch (_items, value) >= 0;
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public string GetCharactersAsString() => new (_items);
}
