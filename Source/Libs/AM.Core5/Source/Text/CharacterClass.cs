// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* CharacterClass.cs -- класс символов Unicode
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace AM.Text
{
    /// <summary>
    /// Класс символов Unicode.
    /// </summary>
    [Flags]
    public enum CharacterClass
    {
        /// <summary>
        /// None.
        /// </summary>
        None = 0x00,

        /// <summary>
        /// Control character.
        /// </summary>
        ControlCharacter = 0x01,

        /// <summary>
        /// Digit.
        /// </summary>
        Digit = 0x02,

        /// <summary>
        /// Basic Latin.
        /// </summary>
        BasicLatin = 0x04,

        /// <summary>
        /// Cyrillic.
        /// </summary>
        Cyrillic = 0x08
    }
}
