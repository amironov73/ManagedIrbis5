// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* CharacterClassifier.cs -- классификатор символов Unicode
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace AM.Text;

/// <summary>
/// Классификатор символов Unicode.
/// </summary>
public static class CharacterClassifier
{
    #region Public methods

    /// <summary>
    /// Выявление классов символов.
    /// </summary>
    public static CharacterClass DetectCharacterClasses
        (
            string? text
        )
    {
        var result = CharacterClass.None;

        if (string.IsNullOrEmpty (text))
        {
            return result;
        }

        foreach (var c in text)
        {
            int index = c;
            if (index < 0x20)
            {
                result |= CharacterClass.ControlCharacter;
            }
            else if (index >= 0x30 && index < 0x3A)
            {
                result |= CharacterClass.Digit;
            }
            else if (index >= 0x40 && index < 0x80)
            {
                result |= CharacterClass.BasicLatin;
            }
            else if (index >= 0x0400 && index < 0x0500)
            {
                result |= CharacterClass.Cyrillic;
            }
        }

        return result;
    }

    /// <summary>
    /// Смешаны ли в тексте латиница с кириллицей?
    /// </summary>
    public static bool IsBothCyrillicAndLatin
        (
            CharacterClass value
        )
    {
        var mixed = CharacterClass.BasicLatin | CharacterClass.Cyrillic;

        return (value & mixed) == mixed;
    }

    #endregion
}
