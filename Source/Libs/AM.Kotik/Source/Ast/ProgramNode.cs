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

using System.Collections.Generic;

#endregion

#nullable enable

namespace AM.Kotik;

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
    public List<StatementNode> Statements { get; internal set; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public ProgramNode
        (
            IEnumerable<StatementNode> statements
        )
    {
        Sure.NotNull (statements);

        Statements = new List<StatementNode> (statements);
    }

    #endregion

    #region Public members

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
}
