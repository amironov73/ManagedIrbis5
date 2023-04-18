// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* SimplestParser.cs -- простейший парсер, полагающийся на результат, оставленный токенайзером
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Diagnostics.CodeAnalysis;

using AM;
using AM.Kotik;

#endregion

#nullable enable

namespace AM.Kotik.Parsers;

/// <summary>
/// Простейший парсер, полагающийся на результат, оставленный токенайзером.
/// </summary>
public sealed class SimplestParser<TResult>
    : Parser<TResult>
    where TResult: class
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public SimplestParser
        (
            string expectedTokenKind
        )
    {
        Sure.NotNullNorEmpty (expectedTokenKind);

        _expectedTokenKind = expectedTokenKind;
    }

    #endregion

    #region Private members

    private readonly string _expectedTokenKind;

    #endregion

    #region Parser<TResult> members

    /// <inheritdoc cref="Parser{TResult}.TryParse"/>
    public override bool TryParse
        (
            ParseState state,
            [MaybeNullWhen (false)] out TResult result
        )
    {
        result = null!;
        if (!state.HasCurrent)
        {
            return false;
        }

        var current = state.Current;
        if (current.Kind == _expectedTokenKind)
        {
            result = (TResult) current.UserData.ThrowIfNull();
            state.Advance();
            return true;
        }

        return false;
    }

    #endregion
}
