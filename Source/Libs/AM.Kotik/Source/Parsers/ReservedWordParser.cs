// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* ReservedWordParser.cs -- парсит зарезервированное слово
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Diagnostics.CodeAnalysis;

using AM.Kotik.Tokenizers;

#endregion

#nullable enable

namespace AM.Kotik;

/// <summary>
/// Парсит зарезервированное слово
/// </summary>
public sealed class ReservedWordParser
    : Parser<string>
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public ReservedWordParser
        (
            string? expected
        )
    {
        _expected = expected;
    }

    #endregion

    #region Private members

    private readonly string? _expected;

    #endregion

    #region Parser<TResult> members

    /// <inheritdoc cref="Parser{TResult}.TryParse"/>
    public override bool TryParse
        (
            ParseState state,
            [MaybeNullWhen (false)] out string result
        )
    {
        using var _ = state.Enter (this);
        result = default;
        DebugHook (state);

        if (state.HasCurrent)
        {
            if (string.IsNullOrEmpty (_expected))
            {
                if (state.Current.Kind == TokenKind.ReservedWord)
                {
                    result = state.Current.Value;
                    state.Advance();
                    return DebugSuccess (state, true);
                }
            }
            else if (state.Current.IsReservedWord (_expected))
            {
                result = _expected;
                state.Advance();
                return DebugSuccess (state, true);
            }

        }

        return DebugSuccess (state, false);
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="Parser{TResult}.ToString"/>
    public override string ToString() => $"Reserved {_expected}";

    #endregion
}
