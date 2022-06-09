// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

/* ReservedParser.cs -- парсер барсиковых зарезервированных слов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using Pidgin;

#endregion

#nullable enable

namespace AM.Scripting;

/// <summary>
/// Парсер барсиковых зарезервированных слов.
/// </summary>
public sealed class ReservedParser
    : Parser<BarsikToken,string?>
{
    #region Properties

    /// <summary>
    /// Ожидаемый тип токена.
    /// </summary>
    public string[] ExpectedKinds { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="expectedKinds">Ожидаемые типы токена</param>
    public ReservedParser
        (
            params string[] expectedKinds
        )
    {
        Sure.NotNull (expectedKinds);

        ExpectedKinds = expectedKinds;
    }

    #endregion

    #region Parser<T1,T2> members

    /// <inheritdoc cref="Parser{TToken,T}.TryParse"/>
    public override bool TryParse
        (
            ref ParseState<BarsikToken> state,
            ref PooledList<Expected<BarsikToken>> expecteds,
            out string? result
        )
    {
        result = default;
        if (!state.HasCurrent)
        {
            return false;
        }

        var current = state.Current;
        if (current.Kind == BarsikToken.ReservedWord)
        {
            var valueSpan = current.Value.Span;
            foreach (var kind in ExpectedKinds)
            {
                if (Utility.CompareOrdinal (kind.AsSpan(), valueSpan) == 0)
                {
                    result = current.Value.ToString();
                    state.Advance();
                    return true;
                }
            }
        }

        return false;
    }

    #endregion
}
