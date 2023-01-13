// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* OptionalParser.cs -- парсер, который безболезненно может зафейлиться
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace AM.Kotik;

/// <summary>
/// Парсер, который безболезненно может зафейлиться.
/// </summary>
public sealed class OptionalParser<TResult>
    : Parser<TResult>
    where TResult: class
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
        result = default!;

        if (_parser.TryParse (state, out var temporary))
        {
            result = temporary;
            return true;
        }

        return false;
    }

    #endregion
}
