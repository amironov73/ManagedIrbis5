// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* OptionalParser.cs -- парсер, который безболезненно может зафейлиться
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using JetBrains.Annotations;

#endregion

#nullable enable

namespace AM.Purr.Parsers;

/// <summary>
/// Парсер, который безболезненно может зафейлиться.
/// </summary>
[PublicAPI]
public sealed class OptionalParser<TResult>
    : Parser<TResult>
{
    #region Construciton

    /// <summary>
    /// Конструктор.
    /// </summary>
    public OptionalParser
        (
            Parser<TResult> parser
        )
    {
        Sure.NotNull (parser);

        _parser = parser;
    }

    #endregion

    #region Private members

    private readonly Parser<TResult> _parser;

    #endregion

    #region Parser<TResult> members

    /// <inheritdoc cref="Parser{TResult}.TryParse"/>
    public override bool TryParse
        (
            ParseState state,
            out TResult result
        )
    {
        using var _ = state.Enter (this);
        result = default!;
        DebugHook (state);

        var location = state.Location;
        if (_parser.TryParse (state, out var temporary))
        {
            result = temporary;
        }
        else
        {
            // откатываем state на всякий случай
            // (вдруг вложенный парсер забыл это сделать?!)
            state.Location = location;
        }

        return DebugSuccess (state, true);
    }

    #endregion
}
