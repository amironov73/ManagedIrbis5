// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable ArgumentsStyleLiteral
// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable RedundantSuppressNullableWarningExpression
// ReSharper disable StaticMemberInitializerReferesToMemberBelow

/* AdditionalStatementParser.cs -- обработка дополнительных парсеров стейтментов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Diagnostics.CodeAnalysis;

#endregion

#nullable enable

namespace AM.Kotik.Barsik.Parsers;

/// <summary>
/// Обработка дополнительных парсеров стейтментов.
/// </summary>
public sealed class AdditionalStatementParser
    : Parser<StatementBase>
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public AdditionalStatementParser
        (
            IGrammar grammar
        )
    {
        Sure.NotNull (grammar);

        _grammar = grammar;
    }

    #endregion

    #region Private members

    private readonly IGrammar _grammar;

    #endregion

    #region Parser<TResult> members

    /// <inheritdoc cref="Parser{TResult}.TryParse"/>
    public override bool TryParse
        (
            ParseState state,
            [MaybeNullWhen (false)] out StatementBase result
        )
    {
        result = null;
        if (!state.HasCurrent)
        {
            return false;
        }

        var location = state.Location;
        foreach (var parser in _grammar.AdditionalStatements)
        {
            state.Location = location;
            if (parser.TryParse (state, out var temporary))
            {
                result = temporary;
                return true;
            }
        }

        state.Location = location;
        return false;
    }

    #endregion
}
