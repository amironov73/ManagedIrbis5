// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* IBlock.cs -- интерфейс блока стейтментов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.Linq;

#endregion

#nullable enable

namespace AM.Kotik.Barsik.Ast;

/// <summary>
/// Интерфейс блока стейтментов.
/// </summary>
public interface IStatementBlock
{
    /// <summary>
    /// Директивы.
    /// </summary>
    IList<DirectiveNode> Directives { get; set; }

    /// <summary>
    /// Функции, входящие в блок.
    /// </summary>
    IList<FunctionDefinitionNode> Functions { get; set; }

    /// <summary>
    /// Локальные переменные, заданные в блоке.
    /// </summary>
    IList<LocalNode> Locals { get; set; }

    /// <summary>
    /// Стейтменты, входящие в блок.
    /// </summary>
    IList<StatementBase> Statements { get; set; }

    /// <summary>
    /// Применение локальных переменных и функций к указанному контексту.
    /// </summary>
    Context ApplyTo
        (
            Context context
        )
    {
        Sure.NotNull (context);

        foreach (var directive in Directives)
        {
            directive.Execute (context);
        }

        foreach (var local in Locals)
        {
            context = local.ApplyTo (context);
        }

        context = KotikUtility.ApplyFunctionDefinitions (context, Functions);

        return context;
    }

    /// <summary>
    /// Поиск стейтмента с указанной меткой.
    /// </summary>
    int? FindLabel
        (
            string label
        )
    {
        for (var index = 0; index < Statements.Count; index++)
        {
            if (Statements[index] is LabelNode labelNode
                && string.CompareOrdinal (label, labelNode.Name) == 0)
            {
                return index;
            }
        }

        return null;
    }

    /// <summary>
    /// Поиск стейтмента в указанной строке.
    /// </summary>
    StatementBase? FindStatementAt
        (
            int lineNumber
        )
    {
        foreach (var statement in Statements)
        {
            if (statement.Line == lineNumber)
            {
                return statement;
            }

            if (statement is IStatementBlock subBlock)
            {
                var result = subBlock.FindStatementAt (lineNumber);
                if (result is not null)
                {
                    return result;
                }
            }
        }

        foreach (IStatementBlock function in Functions)
        {
            var result = function.FindStatementAt (lineNumber);
            if (result is not null)
            {
                return result;
            }
        }

        return null;
    }

    /// <summary>
    /// Отделение овец от козлищ.
    /// </summary>
    void RefineStatements()
    {
        Directives = Statements.Where (x => x is DirectiveNode)
            .Cast<DirectiveNode>().ToList();
        
        Locals = Statements.Where (x => x is LocalNode)
            .Cast<LocalNode>().ToList();

        Functions = Statements.Where (x => x is FunctionDefinitionNode)
            .Cast<FunctionDefinitionNode>().ToList();

        Statements = Statements.Where (x => x is not PseudoNode).ToList();
    }
}
