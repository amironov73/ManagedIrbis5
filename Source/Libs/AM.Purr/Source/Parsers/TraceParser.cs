// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* TraceParser.cs -- трассирующий парсер
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using JetBrains.Annotations;

#endregion

#nullable enable

namespace AM.Purr.Parsers;

/// <summary>
/// Трассирующий парсер.
/// </summary>
[PublicAPI]
public sealed class TraceParser<TResult>
    : Parser<TResult>
{
    #region Construciton

    /// <summary>
    /// Конструктор.
    /// </summary>
    public TraceParser
        (
            IParser<TResult> parser
        )
    {
        _parser = parser;
    }

    #endregion

    #region Private members

    private readonly IParser<TResult> _parser;

    #endregion

    #region Parser<TResult> members

    /// <inheritdoc cref="Parser{TResult}.TryParse"/>
    public override bool TryParse
        (
            ParseState state,
            out TResult result
        )
    {
        result = default!;

        state.DebugCurrentPosition (this);
        if (!_parser.TryParse (state, out var temporary))
        {
            return false;
        }

        state.Trace (Convert.ToString (temporary).ToVisibleString());
        result = temporary;

        return true;
    }

    #endregion
}
