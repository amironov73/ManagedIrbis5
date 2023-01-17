// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* TraceParser.cs -- трассирующий парсер
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Diagnostics.CodeAnalysis;

#endregion

#nullable enable

namespace AM.Kotik;

/// <summary>
/// Трассирующий парсер.
/// </summary>
public sealed class TraceParser<TResult>
    : Parser<TResult>
    where TResult: class
{
    #region Construciton

    /// <summary>
    /// Конструктор.
    /// </summary>
    public TraceParser
        (
            Parser<TResult> parser,
            string? message
        )
    {
        _message = message;
        _parser = parser;
    }

    #endregion

    #region Private members

    private readonly string? _message;
    private readonly Parser<TResult> _parser;

    #endregion

    #region Parser<TResult> members

    /// <inheritdoc cref="Parser{TResult}.TryParse"/>
    public override bool TryParse
        (
            ParseState state,
            [MaybeNullWhen (false)] out TResult result
        )
    {
        result = default;

        if (!string.IsNullOrEmpty (_message))
        {
            state.Trace (_message);
        }

        state.DebugCurrentPosition (this);
        if (!_parser.TryParse (state, out var temporary))
        {
            state.Trace ("Failure");
            return false;
        }

        result = temporary;
        state.Trace ($"Success: {temporary}");

        return true;
    }

    #endregion
}
