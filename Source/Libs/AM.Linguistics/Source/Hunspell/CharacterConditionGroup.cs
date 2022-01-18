﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global

/* .cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Linq;

using AM.Linguistics.Hunspell.Infrastructure;

#endregion

#nullable enable

namespace AM.Linguistics.Hunspell;

public sealed class CharacterConditionGroup : ArrayWrapper<CharacterCondition>
{
    public static readonly CharacterConditionGroup Empty = TakeArray(Array.Empty<CharacterCondition>());

    public static readonly CharacterConditionGroup AllowAnySingleCharacter = Create(CharacterCondition.AllowAny);

    public static readonly ArrayWrapperComparer<CharacterCondition, CharacterConditionGroup> DefaultComparer = new ArrayWrapperComparer<CharacterCondition, CharacterConditionGroup>();

    public static CharacterConditionGroup Create(CharacterCondition condition) => TakeArray(new[] { condition });

    internal static CharacterConditionGroup TakeArray(CharacterCondition[] conditions) => conditions == null ? Empty : new CharacterConditionGroup(conditions);

    private CharacterConditionGroup(CharacterCondition[] conditions)
        : base(conditions)
    {
    }

    public bool AllowsAnySingleCharacter => items.Length == 1 && items[0].AllowsAny;

    public string GetEncoded() => string.Concat(items.Select(c => c.GetEncoded()));

    public override string ToString() => GetEncoded();

    /// <summary>
    /// Determines if the start of the given <paramref name="text"/> matches the conditions.
    /// </summary>
    /// <param name="text">The text to check.</param>
    /// <returns>True when the start of the <paramref name="text"/> is matched by the conditions.</returns>
    public bool IsStartingMatch(string text)
    {
        if (string.IsNullOrEmpty(text) || items.Length > text.Length)
        {
            return false;
        }

        for (int i = 0; i < items.Length; i++)
        {
            if (!items[i].IsMatch(text[i]))
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Determines if the end of the given <paramref name="text"/> matches the conditions.
    /// </summary>
    /// <param name="text">The text to check.</param>
    /// <returns>True when the end of the <paramref name="text"/> is matched by the conditions.</returns>
    public bool IsEndingMatch(string text)
    {
        if (items.Length > text.Length)
        {
            return false;
        }

        for (int conditionIndex = items.Length - 1, textIndex = text.Length - 1; conditionIndex >= 0; conditionIndex--, textIndex--)
        {
            if (!items[conditionIndex].IsMatch(text[textIndex]))
            {
                return false;
            }
        }

        return true;
    }

    public bool IsOnlyPossibleMatch(string text)
    {
        if (string.IsNullOrEmpty(text) || items.Length != text.Length)
        {
            return false;
        }

        for (var i = 0; i < text.Length; i++)
        {
            var condition = items[i];
            if (!condition.PermitsSingleCharacter || condition.Characters[0] != text[i])
            {
                return false;
            }
        }

        return true;
    }
}
