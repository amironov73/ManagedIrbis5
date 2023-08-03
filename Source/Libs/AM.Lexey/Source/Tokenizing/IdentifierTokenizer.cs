// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo

/* IdentifierTokenizer.cs -- токенайзер для идентификаторов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using AM.Text;

using CommunityToolkit.HighPerformance.Buffers;

using JetBrains.Annotations;

#endregion

#nullable enable

// поля, не являющиеся константными, не должны быть видимы
#pragma warning disable CA2211

namespace AM.Lexey.Tokenizing;

/// <summary>
/// Токенайзер для идентификаторов.
/// </summary>
[PublicAPI]
public sealed class IdentifierTokenizer
    : ITokenizer
{
    #region Common data

    /// <summary>
    /// Первый символ идентификатора.
    /// </summary>
    public static char[] FirstIdentifierLetter =
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

    /// <summary>
    /// Последующие символы идентификатора.
    /// </summary>
    public static char[] NextIdentifierLetter =
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

    #endregion

    #region ITokenizer members

    /// <inheritdoc cref="ITokenizer.RecognizeToken"/>
    public Token? RecognizeToken
        (
            TextNavigator navigator
        )
    {
        var firstIdentifierLetter = FirstIdentifierLetter;
        var nextIdentifierLetter = NextIdentifierLetter;
        if (Array.IndexOf (firstIdentifierLetter, navigator.PeekChar()) < 0)
        {
            return null;
        }

        var line = navigator.Line;
        var column = navigator.Column;
        var offset = navigator.Position;
        navigator.ReadChar();
        while (!navigator.IsEOF)
        {
            if (Array.IndexOf (nextIdentifierLetter, navigator.PeekChar()) < 0)
            {
                break;
            }

            navigator.ReadChar();
        }

        var length = navigator.Position - offset;
        var memory = navigator.Substring (offset, length);
        var value = StringPool.Shared.GetOrAdd (memory.Span);
        var result = new Token
            (
                TokenKind.Identifier,
                value,
                line,
                column,
                offset
            )
            {
                UserData = value
            };

        return result;
    }

    #endregion
}
