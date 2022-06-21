// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* TreeNodeExtensions.cs -- методы расширения для TreeNode
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms.MarkupExtensions;

/// <summary>
/// Методы расширения для <see cref="TreeNode"/>.
/// </summary>
public static class TreeNodeExtensions
{
    #region Public methods

    /// <summary>
    /// Добавление дочерних узлов.
    /// </summary>
    public static TTreeNode Nodes<TTreeNode>
        (
            this TTreeNode treeNode,
            params TreeNode[] nodes
        )
        where TTreeNode: TreeNode
    {
        Sure.NotNull (treeNode);
        Sure.NotNull (nodes);

        treeNode.Nodes.AddRange(nodes);

        return treeNode;
    }

    #endregion
}
