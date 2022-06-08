// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

/* BarsikTerm.cs -- парсер барсиковых токенов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using Pidgin;

#endregion

#nullable enable

namespace AM.Scripting;

/// <summary>
/// Парсер барсиковых токенов.
/// </summary>
internal sealed class BarsikTerm
    : Parser<BarsikToken,string?>
{
    #region Properties

    /// <summary>
    /// Ожидаемый тип токена.
    /// </summary>
    public string ExpectedKind { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="expectedKind">Ожидаемый тип токена</param>
    public BarsikTerm
        (
            string expectedKind
        )
    {
        Sure.NotNull (expectedKind);

        ExpectedKind = expectedKind;
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
        if (current.Kind == ExpectedKind)
        {
            result = current.Kind;
            return true;
        }

        return false;
    }

    #endregion
}
