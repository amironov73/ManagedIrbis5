// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global

/* RegexTokenizer.cs -- токенайзер на регулярных выражениях
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Text.RegularExpressions;

#endregion

#nullable enable

namespace AM.Kotik.Tokenizers;

/// <summary>
/// Токенайзер на регулярных выражениях.
/// </summary>
public sealed class RegexTokenizer
    : Tokenizer
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public RegexTokenizer
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
    public RegexTokenizer
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
    public override Token? Parse()
    {
        var line = navigator.Line;
        var column = navigator.Column;
        var position = navigator.SavePosition();

        var goodLength = 0;
        var maxLength = Math.Min (_maxLength, navigator.Length - navigator.Position);
        for (var length = 1; length < maxLength; length++)
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
            return new Token (_kind, text, line, column);
        }

        return null;
    }

    #endregion
}
