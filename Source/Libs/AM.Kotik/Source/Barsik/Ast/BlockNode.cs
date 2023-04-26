// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

/* Block.cs -- блок стейтментов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;

using AM.Kotik.Ast;

#endregion

#nullable enable

namespace AM.Kotik.Barsik.Ast;

/// <summary>
/// Блок стейтментов.
/// </summary>
public class BlockNode
    : StatementBase,
    IStatementBlock
{
    #region Properties

    /// <inheritdoc cref="IStatementBlock.Directives"/>
    public IList<DirectiveNode> Directives { get; set; }

    /// <inheritdoc cref="IStatementBlock.Functions"/>
    public IList<FunctionDefinitionNode> Functions { get; set; }

    /// <inheritdoc cref="IStatementBlock.Locals"/>
    public IList<LocalNode> Locals { get; set; }

    /// <inheritdoc cref="IStatementBlock.Statements"/>
    public IList<StatementBase> Statements { get; set; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public BlockNode
        (
            int line,
            IList<StatementBase> statements
        )
        : base (line)
    {
        Sure.NotNull (statements);

        Directives = null!;
        Functions = null!;
        Locals = null!;
        Statements = statements;
        ((IStatementBlock) this).RefineStatements();
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Дамп синтаксического дерева.
    /// </summary>
    public void Dump
        (
            TextWriter? writer = null
        )
    {
        writer ??= Console.Out;

        DumpHierarchyItem (null, 0, writer);
        foreach (var function in Functions)
        {
            function.DumpHierarchyItem ("Function", 0, writer);
        }
    }

    #endregion

    #region StatementBase members

    /// <inheritdoc cref="StatementBase.Execute"/>
    public override void Execute
        (
            Context context
        )
    {
        PreExecute (context);

        context = ((IStatementBlock) this).ApplyTo (context);

        var index = 0;
        while (index < Statements.Count)
        {
            var statement = Statements[index];
            try
            {
                statement.Execute (context);
                index++;
            }
            catch (GotoException gotoException)
            {
                var whereLabel = ((IStatementBlock) this).FindLabel (gotoException.Label);
                if (!whereLabel.HasValue)
                {
                    // передаем исключение наверх
                    // логировать не надо
                    throw;
                }

                index = whereLabel.Value;
            }
        }
    }

    #endregion

    #region AstNode members

    /// <inheritdoc cref="AstNode.DumpHierarchyItem(string?,int,System.IO.TextWriter,string?)"/>
    internal override void DumpHierarchyItem
        (
            string? name,
            int level,
            TextWriter writer
        )
    {
        base.DumpHierarchyItem (name, level, writer, ToString());

        foreach (var statement in Statements)
        {
            statement.DumpHierarchyItem ("Statement", level + 1, writer);
        }
    }

    /// <inheritdoc cref="AstNode.GetNodeInfo"/>
    public override AstNodeInfo GetNodeInfo()
    {
        var result = new AstNodeInfo (this)
        {
            Name = "block"
        };

        foreach (var statement in Statements)
        {
            result.Children.Add (statement.GetNodeInfo());
        }

        return result;
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Перечисление элементов.
    /// </summary>
    public IEnumerator<StatementBase> GetEnumerator() => Statements.GetEnumerator();

    #endregion
}
