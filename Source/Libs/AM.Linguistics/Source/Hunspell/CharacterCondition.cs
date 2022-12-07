// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global

/* CharacterCondition.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;

#endregion

#nullable enable

namespace AM.Linguistics.Hunspell;

/// <summary>
///
/// </summary>
public readonly struct CharacterCondition
    : IEquatable<CharacterCondition>
{
    private static readonly Regex ConditionParsingRegex = new
        (
            @"^(\[[^\]]*\]|\.|[^\[\]\.])*$",
            RegexOptions.Compiled | RegexOptions.CultureInvariant
        );

    /// <summary>
    ///
    /// </summary>
    public static readonly CharacterCondition AllowAny = new (CharacterSet.Empty, true);

    internal static CharacterCondition TakeArray (char[] characters, bool restricted)
    {
        return new (characters, restricted);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="character"></param>
    /// <param name="restricted"></param>
    /// <returns></returns>
    public static CharacterCondition Create
        (
            char character,
            bool restricted
        )
    {
        return new (character, restricted);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="characters"></param>
    /// <param name="restricted"></param>
    /// <returns></returns>
    public static CharacterCondition Create
        (
            IEnumerable<char>? characters,
            bool restricted
        )
    {
        return TakeArray (characters is null ? Array.Empty<char>() : characters.ToArray(), restricted);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public static CharacterConditionGroup Parse
        (
            string text
        )
    {
        if (string.IsNullOrEmpty (text))
        {
            return CharacterConditionGroup.Empty;
        }

        var match = ConditionParsingRegex.Match (text);
        if (!match.Success || match.Groups.Count < 2)
        {
            return CharacterConditionGroup.Empty;
        }

        var captures = match.Groups[1].Captures;
        var conditions = new CharacterCondition[captures.Count];
        for (var captureIndex = 0; captureIndex < captures.Count; captureIndex++)
        {
            conditions[captureIndex] = ParseSingle (captures[captureIndex].Value.AsSpan());
        }

        return CharacterConditionGroup.TakeArray (conditions);
    }

    private static CharacterCondition ParseSingle (ReadOnlySpan<char> text)
    {
        if (text.Length == 0)
        {
            return AllowAny;
        }

        if (text.Length == 1)
        {
            var singleChar = text[0];
            if (singleChar == '.')
            {
                return AllowAny;
            }

            return Create (singleChar, false);
        }

        if (!text.StartsWith ("[") || !text.EndsWith ("]"))
        {
            throw new InvalidOperationException();
        }

        var restricted = text[1] == '^';
        text = restricted ? text.Slice (2, text.Length - 3) : text.Slice (1, text.Length - 2);
        return TakeArray (text.ToArray(), restricted);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="characters"></param>
    /// <param name="restricted"></param>
    public CharacterCondition
        (
            CharacterSet characters,
            bool restricted
        )
    {
        Characters = characters;
        Restricted = restricted;
    }

    private CharacterCondition
        (
            char character,
            bool restricted
        )
        : this (CharacterSet.Create (character), restricted)
    {
        // пустое тело конструктора
    }

    private CharacterCondition
        (
            char[] characters,
            bool restricted
        )
        : this (CharacterSet.TakeArray (characters), restricted)
    {
        // пустое тело конструктора
    }

    /// <summary>
    ///
    /// </summary>
    public CharacterSet Characters { get; }

    /// <summary>
    /// Indicates that the <see cref="Characters"/> are restricted when <c>true</c>.
    /// </summary>
    public bool Restricted { get; }

    /// <summary>
    ///
    /// </summary>
    /// <param name="c"></param>
    /// <returns></returns>
    public bool IsMatch (char c)
    {
        return (Characters != null && Characters.Contains (c)) ^ Restricted;
    }

    /// <summary>
    ///
    /// </summary>
    public bool AllowsAny => Restricted && (Characters == null || Characters.Count == 0);

    /// <summary>
    ///
    /// </summary>
    public bool PermitsSingleCharacter => !Restricted && Characters is { Count: 1 };

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public string GetEncoded()
    {
        if (AllowsAny)
        {
            return ".";
        }

        if (PermitsSingleCharacter)
        {
            return Characters[0].ToString();
        }

        var lettersText = Characters == null || Characters.Count == 0
            ? string.Empty
            : Characters.GetCharactersAsString();

        return (Restricted ? "[^" : "[") + lettersText + "]";
    }

    /// <inheritdoc cref="ValueType.ToString"/>
    public override string ToString()
    {
        return GetEncoded();
    }

    /// <inheritdoc cref="IEquatable{T}.Equals(T?)"/>
    public bool Equals (CharacterCondition other)
    {
        return Restricted == other.Restricted && CharacterSet.DefaultComparer.Equals (Characters, other.Characters);
    }

    /// <inheritdoc cref="ValueType.Equals(object?)"/>
    public override bool Equals (object? obj)
    {
        return obj is CharacterCondition cc && Equals (cc);
    }

    /// <inheritdoc cref="ValueType.GetHashCode"/>
    public override int GetHashCode()
    {
        return unchecked ((Restricted.GetHashCode() * 149) ^ CharacterSet.DefaultComparer.GetHashCode (Characters));
    }
}
