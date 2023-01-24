// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable ArgumentsStyleLiteral
// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable RedundantSuppressNullableWarningExpression
// ReSharper disable StaticMemberInitializerReferesToMemberBelow

/* AdditionalInfixOperator.cs -- обработка дополнительных инфиксных операций
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics.CodeAnalysis;

#endregion

#nullable enable

namespace AM.Kotik.Barsik.Parsers;

/// <summary>
/// Обработка дополнительных инфиксных операций.
/// </summary>
public sealed class AdditionalInfixOperator
    : InfixOperator<AtomNode>
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public AdditionalInfixOperator
        (
            IGrammar grammar
        )
        : base (new FailParser<string>("fail"), (_, _, _) => null!, "nolabel",
            InfixOperatorKind.LeftAssociative)
    {
        Sure.NotNull (grammar);

        _grammar = grammar;
    }

    #endregion

    #region Private members

    private readonly IGrammar _grammar;

    #endregion
}
