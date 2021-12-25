// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global

/* ProgramNode.cs -- корневой узел AST
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directive

using System.Collections.Generic;

#endregion

#nullable enable

namespace AM.Scripting.Barsik;

/// <summary>
/// Корневой узел AST.
/// </summary>
public sealed class ProgramNode
    : AstNode
{
    #region Properties

    /// <summary>
    /// Стейтменты программы.
    /// </summary>
    public List<StatementNode> Statements { get; }

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
        Statements = new List<StatementNode> (statements);
    }

    #endregion

    #region Private methods

    #endregion

    #region AstNode members

    /// <summary>
    /// Выполнение действий, предусмотренных данной программой.
    /// </summary>
    /// <param name="context">Контекст исполнения программы.</param>
    public void Execute (Context context)
    {
        foreach (var statement in Statements)
        {
            statement.Execute (context);
        }
    }

    #endregion
}
