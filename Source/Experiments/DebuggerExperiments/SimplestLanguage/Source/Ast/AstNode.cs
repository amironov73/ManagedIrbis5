// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedMember.Global

/* AstNode.cs -- элемент синтаксического дерева
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

// using System;
// using System.Collections.Generic;
// using System.Collections.ObjectModel;

#endregion

#nullable enable

namespace SimplestLanguage;

/// <summary>
/// Элемент синтаксического дерева.
/// </summary>
public abstract class AstNode
{
    #region Properties

    // /// <summary>
    // /// Родительский узел.
    // /// </summary>
    // public AstNode? Parent { get; internal set; }
    //
    // /// <summary>
    // /// Дочерние узлы.
    // /// </summary>
    // public IReadOnlyList<AstNode> Children => _children ?? Empty;

    #endregion

    #region Private members

    // private static readonly IReadOnlyList<AstNode> Empty
    //     = new ReadOnlyCollection<AstNode> (Array.Empty<AstNode>());
    // private List<AstNode>? _children;

    #endregion

    #region Construction

    // /// <summary>
    // /// Конструктор по умолчанию.
    // /// </summary>
    // protected AstNode()
    // {
    // }
    //
    // /// <summary>
    // /// Конструктор.
    // /// </summary>
    // protected AstNode
    //     (
    //         IEnumerable<AstNode> children
    //     )
    // {
    //     AppendChildren (children);
    //
    // }

    #endregion

    #region Public methods

    // /// <summary>
    // /// Добавление дочернего узла.
    // /// </summary>
    // public AstNode AppendChild
    //     (
    //         AstNode child
    //     )
    // {
    //     _children ??= new List<AstNode>();
    //     _children.Add (child);
    //
    //     return this;
    //
    // }

    // /// <summary>
    // /// Добавление дочерних узлов.
    // /// </summary>
    // public AstNode AppendChildren
    //     (
    //         IEnumerable<AstNode> children
    //     )
    // {
    //     _children ??= new List<AstNode>();
    //     _children.AddRange (children);
    //
    //     return this;
    //
    // }

    public virtual void Execute
        (
            LanguageContext context
        )
    {
        // foreach (var child in Children)
        // {
        //     child.Execute (context);
        // }
    }

    #endregion
}
