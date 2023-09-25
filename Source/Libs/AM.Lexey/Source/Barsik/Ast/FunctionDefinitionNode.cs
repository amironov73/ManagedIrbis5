// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* FunctionDefinitionNode.cs -- псевдо-узел: определение функции в скрипте
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.IO;

using AM.Lexey.Ast;

#endregion

namespace AM.Lexey.Barsik.Ast;

/// <summary>
/// Псевдо-узел AST: определение функции в скрипте.
/// </summary>
public sealed class FunctionDefinitionNode
    : PseudoNode,
    IStatementBlock
{
    #region Properties

    /// <inheritdoc cref="IStatementBlock.Directives"/>
    IList<DirectiveNode> IStatementBlock.Directives
    {
        get => Body.Directives;
        set => Body.Directives = value;
    }

    /// <inheritdoc cref="IStatementBlock.Functions"/>
    IList<FunctionDefinitionNode> IStatementBlock.Functions
    {
        get => Body.Functions;
        set => Body.Functions = value;
    }

    /// <inheritdoc cref="IStatementBlock.Locals"/>
    IList<LocalNode> IStatementBlock.Locals
    {
        get => Body.Locals;
        set => Body.Locals = value;
    }

    /// <inheritdoc cref="IStatementBlock.Statements"/>
    IList<StatementBase> IStatementBlock.Statements
    {
        get => Body.Statements;
        set => Body.Statements = value;
    }

    /// <inheritdoc cref="IStatementBlock.Statements"/>
    public BlockNode Body { get; set; }

    /// <summary>
    /// Имя функции.
    /// </summary>
    public string Name { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public FunctionDefinitionNode
        (
            int line,
            string functionName,
            IList<string> argumentNames,
            BlockNode body
        )
        : base (line)
    {
        Sure.NotNullNorEmpty (functionName);
        Sure.NotNull (argumentNames);
        Sure.NotNull (body);

        Name = functionName;
        this.argumentNames = argumentNames;
        Body = body;
    }

    #endregion

    #region Private members

    internal readonly IList<string> argumentNames;

    #endregion

    #region AstNode members

    /// <inheritdoc cref="AstNode.DumpHierarchyItem(string?,int,System.IO.TextWriter)"/>
    internal override void DumpHierarchyItem
        (
            string? name,
            int level,
            TextWriter writer
        )
    {
        base.DumpHierarchyItem (name, level, writer);

        DumpHierarchyItem ("Name", level + 1, writer, Name);
        foreach (var argumentName in argumentNames)
        {
            DumpHierarchyItem ("Arg", level + 1, writer, argumentName);
        }

        Body.DumpHierarchyItem ("Body", level + 1, writer);
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="AstNode.ToString"/>
    public override string ToString() => $"Function Definition at line {Line}";

    #endregion
}
