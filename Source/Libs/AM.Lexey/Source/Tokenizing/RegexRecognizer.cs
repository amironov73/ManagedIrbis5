// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global

/* RegexRecognizer.cs -- распознает токены на регулярных выражениях
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Text.RegularExpressions;

using AM.Text;

using JetBrains.Annotations;

#endregion

#nullable enable

namespace AM.Lexey.Tokenizing;

/// <summary>
/// Распознает токены на регулярных выражениях.
/// </summary>
/// <remarks>
/// Не может быть добавлен в <c>tokenizer.settings</c>,
/// т. к. не содержит конструктора по умолчанию.
/// </remarks>
[PublicAPI]
public sealed class RegexRecognizer
    : ITokenRecognizer
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public RegexRecognizer
        (
            string kind,
            string regex,
            int maxLength = int.MaxValue
        )
        : this (kind, new Regex (regex), maxLength)
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public RegexRecognizer
        (
            string kind,
            Regex regex,
            int maxLength = int.MaxValue
        )
    {
        Sure.NotNullNorEmpty (kind);
        Sure.NotNull (regex);
        Sure.Positive (maxLength);

        _regex = regex;
        _kind = kind;
        _maxLength = maxLength;
    }

    #endregion

    #region Private members

    private readonly Regex _regex;
    private readonly string _kind;
    private readonly int _maxLength;

    #endregion

    #region Tokeninzer members

    /// <inheritdoc cref="Tokenizer.Parse"/>
    public Token? RecognizeToken
        (
            TextNavigator navigator
        )
    {
        Sure.NotNull (navigator);

        var line = navigator.Line;
        var column = navigator.Column;
        var position = navigator.SavePosition();

        var goodLength = 0;
        var maxLength = Math.Min (_maxLength, navigator.Length - navigator.Position);
        for (var length = 1; length <= maxLength; length++)
        {
            var slice = navigator.Substring (position, length).Span;
            if (_regex.IsMatch (slice))
            {
                goodLength = length;
            }
        }

        if (goodLength != 0)
        {
            navigator.RestorePosition (position + goodLength);
            var text = navigator.Substring (position, goodLength).ToString();
            var result = new Token (_kind, text, line, column) { UserData = text };

            return result;
        }

        return default;
    }

    #endregion
}
