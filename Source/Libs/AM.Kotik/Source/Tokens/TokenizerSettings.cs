// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* TokenizerSettings.cs -- настройки токенизации
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Text;

#endregion

#nullable enable

namespace AM.Kotik;

/// <summary>
/// Настройки токенизации.
/// </summary>
public sealed class TokenizerSettings
{
    #region Properties

    /// <summary>
    /// Первый символ идентификатора.
    /// </summary>
    public char[] FirstIdentifierLetter { get; set; }

    /// <summary>
    /// Последующие символы идентификатора.
    /// </summary>
    public char[] NextIdentifierLetter { get; set; }

    /// <summary>
    /// Распознаваемые термы.
    /// </summary>
    public string[] KnownTerms { get; set; }
    
    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public TokenizerSettings()
    {
        FirstIdentifierLetter =
            (
                "abcdefghijklmnopqrstuvwxyz" // строчная латиница
                + "ABCDEFGHIJKLMNOPQRSTUVWXYZ" // заглавная латиница
                + "абвгдеёжзийклмнопрстуфхцчшщъыьэюя" // строчная кириллица
                + "АБСГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ" // заглавная кириллица
                + "αβϐγδεϵζηθϑικϰλμνξοπϖρϱσςτυφϕχψω" // строчные греческие
                + "ΑΒΓΔΕΖΗΘϴΙΚΛΜΝΞΟΠΡΣΤΥϒΦΧΨΩ" // заглавные греческие
                + "_$"
            )
            .ToCharArray();

        NextIdentifierLetter =
            (
                "abcdefghijklmnopqrstuvwxyz" // строчная латиница
                + "ABCDEFGHIJKLMNOPQRSTUVWXYZ" // заглавная латиница
                + "0123456789" // цифры
                + "абвгдеёжзийклмнопрстуфхцчшщъыьэюя" // строчная кирилица
                + "АБСГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ" // заглавная кириллица
                + "αβϐγδεϵζηθϑικϰλμνξοπϖρϱσςτυφϕχψω" // строчные греческие
                + "ΑΒΓΔΕΖΗΘϴΙΚΛΜΝΞΟΠΡΣΤΥϒΦΧΨΩ" // заглавные греческие
                + "_$"
            )
            .ToCharArray();

        KnownTerms = new[]
        {
            "!", ";", ":", ",", "(", ")", "+", "-", "*", "/", "[", "]",
            "{", "}", "|", "%", "~", "=", "++", "--", "+=", "-=", "*=",
            "/=", "==", "<", ">", "<<", ">>", "<=", ">=", "||", "&&",
            ".", ","
        };
    }

    #endregion
    
    #region Public methods

    /// <summary>
    /// Выдача настроек по умолчанию.
    /// </summary>
    public static TokenizerSettings CreateDefault()
    {
        return new TokenizerSettings();
    }

    #endregion
}
