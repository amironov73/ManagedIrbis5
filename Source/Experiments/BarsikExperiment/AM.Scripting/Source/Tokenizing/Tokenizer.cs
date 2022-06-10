// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable NonReadonlyMemberInGetHashCode
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global

/* Tokenizer.cs -- генерализованный токенайзер
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Text;

#endregion

#nullable enable

namespace AM.Scripting;

/// <summary>
/// Генерализованный токенайзер.
/// </summary>
public class Tokenizer
{
    #region Private members

    // пространство имен нужно, чтобы не делать using
    // а если сделать using, то пересекутся имена классов
    // вроде Token
    private Text.TextNavigator _navigator = null!;

    private static readonly char[] _firstIdentifierLetter =
        (
            "abcdefghijklmnopqrstuvwxyz"
            + "ABCDEFGHIJKLMNOPQRSTUVWXYZ"
            + "абвгдеёжзийклмнопрстуфхцчшщъыьэюя"
            + "АБСГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ"
            + "_$"
        )
        .ToCharArray();

    private static readonly char[] _nextIdentifierLetter =
        (
            "abcdefghijklmnopqrstuvwxyz"
            + "ABCDEFGHIJKLMNOPQRSTUVWXYZ"
            + "0123456789"
            + "абвгдеёжзийклмнопрстуфхцчшщъыьэюя"
            + "АБСГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ"
            + "_$"
        )
        .ToCharArray();

    private bool IsEOF => _navigator.IsEOF;

    private char PeekChar() => _navigator.PeekChar();

    private char ReadChar() => _navigator.ReadChar();

    private bool SkipWhitespace() => _navigator.SkipWhitespace();

    #endregion

    #region Protected members

    /// <summary>
    /// Разбор единичного символа в кавычках.
    /// </summary>
    /// <returns><c>null</c>, если в текущей позиции не символ.</returns>
    protected virtual Token? ParseCharacter()
    {
        if (PeekChar() != '\'')
        {
            return null;
        }



        return null;
    }

    /// <summary>
    /// Разбор обычной строки (пока без экранирования).
    /// </summary>
    protected virtual Token? ParseString()
    {
        return null;
    }

    /// <summary>
    /// Разбор идентификатора.
    /// </summary>
    /// <returns><c>null</c>, если в текущей позиции не идентификатор.</returns>
    protected virtual Token? ParseIdentifier()
    {
        var firstChar = PeekChar();
        if (Array.IndexOf (_firstIdentifierLetter, firstChar) < 0)
        {
            return null;
        }

        var value = new StringBuilder();
        var line = _navigator.Line;
        var column = _navigator.Column;
        value.Append (ReadChar());

        while (IsEOF)
        {
            if (Array.IndexOf (_nextIdentifierLetter, PeekChar()) < 0)
            {
                break;
            }

            value.Append(ReadChar());
        }

        return new Token
            (
                TokenKind.Identifier,
                value.ToString(),
                line,
                column
            );
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Разбор текста на токены.
    /// </summary>
    public List<Token> Parse
        (
            string text
        )
    {
        Sure.NotNull (text);

        var result = new List<Token>();
        _navigator = new Text.TextNavigator (text);

        while (!IsEOF)
        {
            if (!SkipWhitespace())
            {
                break;
            }

            var token = ParseCharacter()
                ?? ParseString()
                ?? ParseIdentifier()
                ?? throw new Exception();

            result.Add (token);
        }

        return result;
    }

    #endregion
}
