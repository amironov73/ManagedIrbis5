// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

/* ProgramNode.cs -- корневой узел AST
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;

#endregion

#nullable enable

namespace AM.Kotik.Barsik;

/// <summary>
/// Корневой узел AST, означающий всю программу,
/// содержащуюся в скрипте.
/// </summary>
public sealed class ProgramNode
    : AstNode
{
    #region Properties

    /// <summary>
    /// Стейтменты, из которых состоит прогамма.
    /// </summary>
    public List<StatementBase> Statements { get; internal set; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public ProgramNode
        (
            IEnumerable<StatementBase> statements
        )
    {
        Sure.NotNull (statements);

        Statements = new List<StatementBase> (statements);
    }

    #endregion

    #region Public members

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
    }

    /// <summary>
    /// Выполнение действий, предусмотренных данной программой.
    /// </summary>
    /// <param name="context">Контекст исполнения программы.</param>
    public void Execute
        (
            Context context
        )
    {
        Sure.NotNull (context);

        foreach (var statement in Statements)
        {
            statement.Execute (context);
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
        base.DumpHierarchyItem (name, level, writer, "Program");

        foreach (var statement in Statements)
        {
            statement.DumpHierarchyItem ("Statement", level + 1, writer);
        }
    }

    #endregion
}
