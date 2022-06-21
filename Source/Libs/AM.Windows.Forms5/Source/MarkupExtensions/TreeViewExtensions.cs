// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* TreeViewExtensions.cs -- методы расширения для TreeView
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms.MarkupExtensions;

/// <summary>
/// Методы расширения для <see cref="TreeView"/>
/// </summary>
public static class TreeViewExtensions
{
    #region Public methods

    /// <summary>
    /// Добавление корневых узлов в дерево.
    /// </summary>
    public static TTreeView Nodes<TTreeView>
        (
            this TTreeView treeView,
            params TreeNode[] nodes
        )
        where TTreeView : TreeView
    {
        Sure.NotNull (treeView);
        Sure.NotNull (nodes);

        treeView.Nodes.AddRange(nodes);

        return treeView;
    }

    /// <summary>
    /// Регистрация обработчика события
    /// <see cref="TreeView.AfterSelect"/>.
    /// </summary>
    public static TTreeView OnAfterSelect<TTreeView>
        (
            this TTreeView treeView,
            TreeViewEventHandler handler
        )
        where TTreeView : TreeView
    {
        Sure.NotNull (treeView);
        Sure.NotNull (handler);

        treeView.AfterSelect += handler;

        return treeView;
    }

    #endregion
}
