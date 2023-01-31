// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable ArgumentsStyleLiteral
// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable RedundantSuppressNullableWarningExpression
// ReSharper disable StaticMemberInitializerReferesToMemberBelow

/* IGrammar.cs -- интерфейс грамматики
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;

using AM.Kotik.Barsik.Ast;

#endregion

#nullable enable

namespace AM.Kotik.Barsik;

/// <summary>
/// Интерфейс грамматики.
/// </summary>
public interface IGrammar
{
    /// <summary>
    /// Дополнительно распознаваемые вычислимые выражения.
    /// </summary>
    IList<Parser<AtomNode>> Atoms { get; }

    /// <summary>
    /// Дополнительно распознаваемые инфиксные операции.
    /// </summary>
    IList<InfixOperator<AtomNode>> Infixes { get; }

    /// <summary>
    /// Дополнительно распознаваемые постфиксные операции.
    /// </summary>
    IList<Parser<Func<AtomNode, AtomNode>>> Postfixes { get; }

    /// <summary>
    /// Дополнительно распознаваемые префиксные операции.
    /// </summary>
    IList<Parser<Func<AtomNode, AtomNode>>> Prefixes { get; }

    /// <summary>
    /// Дополнительно распознаваемые стейтменты.
    /// </summary>
    IList<Parser<StatementBase>> Statements { get; }

    /// <summary>
    /// Разбор текста выражения.
    /// </summary>
    AtomNode ParseExpression
        (
            string sourceCode,
            Tokenizer tokenizer,
            TextWriter? debugOutput = null
        );

    /// <summary>
    /// Разбор текста стейтмента.
    /// </summary>
    StatementBase ParseStatement
        (
            string sourceCode,
            Tokenizer tokenizer,
            TextWriter? debugOutput = null
        );

    /// <summary>
    /// Разбор программы.
    /// </summary>
    ProgramNode ParseProgram
        (
            string sourceText,
            Tokenizer tokenizer,
            bool requireEnd = true,
            TextWriter? debugOutput = null
        );

    /// <summary>
    /// Пересоздание грамматики после внесения изменений.
    /// </summary>
    void Rebuild();
}
