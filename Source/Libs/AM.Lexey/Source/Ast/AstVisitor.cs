// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* AstVisitor.cs -- посетитель AST-узлов
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace AM.Lexey.Ast;

/// <summary>
/// Посетитель узлов AST-дерева.
/// </summary>
public abstract class AstVisitor
{
    #region Public methods

    /// <summary>
    /// Посещение узла.
    /// </summary>
    /// <returns><c>true</c> означает "продолжать посещения".
    /// </returns>
    public abstract bool VisitNode (AstNode node);

    #endregion
}
