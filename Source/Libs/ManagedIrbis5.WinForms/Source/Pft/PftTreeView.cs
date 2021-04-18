// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* PftTreeView.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Windows.Forms;

using AM;

using ManagedIrbis.Pft.Infrastructure;
using ManagedIrbis.Pft.Infrastructure.Diagnostics;

#endregion

#nullable enable

namespace ManagedIrbis.WinForms.Pft
{
    /// <summary>
    /// TreeView over <see cref="PftNode"/>'s
    /// </summary>
    public partial class PftTreeView
        : UserControl
    {
        #region Events

        /// <summary>
        /// Fired when current node changed.
        /// </summary>
        public event EventHandler<TreeViewEventArgs>? CurrentNodeChanged;

        /// <summary>
        /// Fired when node check state changed.
        /// </summary>
        public event EventHandler<TreeViewEventArgs>? NodeChecked;

        #endregion

        #region Properties

        /// <summary>
        /// Current node.
        /// </summary>
        public PftNodeInfo? CurrentNode => _tree.SelectedNode?.Tag as PftNodeInfo;

        #endregion

        #region Construction

        /// <summary>
        /// Construction.
        /// </summary>
        public PftTreeView()
        {
            InitializeComponent();

            _tree.AfterSelect += _tree_AfterSelect;
            _tree.AfterCheck += _tree_AfterCheck;
        }

        #endregion

        #region Private members

        private static TreeNode _ConvertNode
            (
                PftNodeInfo info
            )
        {
            var text = info.ToString();

            var result = new TreeNode
            {
                Tag = info,
                Text = text,
                ToolTipText = text
            };

            foreach (var child in info.Children)
            {
                if (!ReferenceEquals(child, null))
                {
                    var node = _ConvertNode(child);
                    if (!ReferenceEquals(node, null))
                    {
                        result.Nodes.Add(node);
                    }
                }
            }

            return result;
        }

        private void _tree_AfterCheck ( object sender, TreeViewEventArgs e ) =>
            NodeChecked?.Invoke(sender, e);

        void _tree_AfterSelect ( object sender, TreeViewEventArgs e ) =>
            CurrentNodeChanged?.Invoke(sender, e);

        #endregion

        #region Public methods

        /// <summary>
        /// Clear.
        /// </summary>
        public void Clear()
        {
            _tree.Nodes.Clear();
        }

        /// <summary>
        /// Set nodes.
        /// </summary>
        public void SetNodes
            (
                PftNode rootNode
            )
        {
            try
            {
                _tree.BeginUpdate();
                _tree.Nodes.Clear();
                var rootInfo = rootNode.GetNodeInfo();
                var treeNode = _ConvertNode(rootInfo);
                _tree.Nodes.Add(treeNode);
                _tree.ExpandAll();
                _tree.SelectedNode = treeNode;
                treeNode.EnsureVisible();
            }
            finally
            {
                _tree.EndUpdate();
            }
        }

        #endregion
    }
}

