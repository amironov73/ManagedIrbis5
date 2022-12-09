// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global

/* CharacterConditionGroup.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Linq;

using AM.Linguistics.Hunspell.Infrastructure;

#endregion

#nullable enable

namespace AM.Linguistics.Hunspell;

/// <summary>
///
/// </summary>
public sealed class CharacterConditionGroup
    : ArrayWrapper<CharacterCondition>
{
    /// <summary>
    ///
    /// </summary>
    public static readonly CharacterConditionGroup Empty = TakeArray (Array.Empty<CharacterCondition>());

    /// <summary>
    ///
    /// </summary>
    public static readonly CharacterConditionGroup AllowAnySingleCharacter = Create (CharacterCondition.AllowAny);

    /// <summary>
    ///
    /// </summary>
    public static readonly ArrayWrapperComparer<CharacterCondition, CharacterConditionGroup> DefaultComparer = new ();

    /// <summary>
    ///
    /// </summary>
    /// <param name="condition"></param>
    /// <returns></returns>
    public static CharacterConditionGroup Create (CharacterCondition condition)
    {
        return TakeArray (new[] { condition });
    }

    internal static CharacterConditionGroup TakeArray (CharacterCondition[] conditions)
    {
        return conditions == null! ? Empty : new CharacterConditionGroup (conditions);
    }

    private CharacterConditionGroup (CharacterCondition[] conditions)
        : base (conditions)
    {
        // пустое тело конструктора
    }

    /// <summary>
    ///
    /// </summary>
    public bool AllowsAnySingleCharacter => _items.Length == 1 && _items[0].AllowsAny;

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public string GetEncoded()
    {
        return string.Concat (_items.Select (c => c.GetEncoded()));
    }

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        return GetEncoded();
    }

    /// <summary>
    /// Determines if the start of the given <paramref name="text"/> matches the conditions.
    /// </summary>
    /// <param name="text">The text to check.</param>
    /// <returns>True when the start of the <paramref name="text"/> is matched by the conditions.</returns>
    public bool IsStartingMatch (string text)
    {
        if (string.IsNullOrEmpty (text) || _items.Length > text.Length)
        {
            return false;
        }

        for (var i = 0; i < _items.Length; i++)
        {
            if (!_items[i].IsMatch (text[i]))
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
    public bool IsEndingMatch (string text)
    {
        if (_items.Length > text.Length)
        {
            return false;
        }

        for (int conditionIndex = _items.Length - 1, textIndex = text.Length - 1;
             conditionIndex >= 0;
             conditionIndex--, textIndex--)
        {
            if (!_items[conditionIndex].IsMatch (text[textIndex]))
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public bool IsOnlyPossibleMatch (string text)
    {
        if (string.IsNullOrEmpty (text) || _items.Length != text.Length)
        {
            return false;
        }

        for (var i = 0; i < text.Length; i++)
        {
            var condition = _items[i];
            if (!condition.PermitsSingleCharacter || condition.Characters[0] != text[i])
            {
                return false;
            }
        }

        return true;
    }
}
