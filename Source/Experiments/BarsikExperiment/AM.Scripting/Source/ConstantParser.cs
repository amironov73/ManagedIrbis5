// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

/* ConstantParser.cs -- парсер барсиковых констант
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using Pidgin;

#endregion

#nullable enable

namespace AM.Scripting;

/// <summary>
/// Парсер барсиковых констант.
/// </summary>
internal sealed class ConstantParser
    : Parser<BarsikToken, ConstantNode>
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
    public ConstantParser
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
            out ConstantNode result
        )
    {
        result = default!;
        if (!state.HasCurrent)
        {
            return false;
        }

        var current = state.Current;
        foreach (var kind in ExpectedKinds)
        {
            if (current.Kind == kind)
            {
                result = new ConstantNode (current.Value.ToString());
                state.Advance();
                return true;
            }
        }

        return false;
    }

    #endregion
}
