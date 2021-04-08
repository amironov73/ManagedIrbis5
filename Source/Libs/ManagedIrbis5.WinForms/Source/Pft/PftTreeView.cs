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
        public PftNodeInfo? CurrentNode
        {
            get
            {
                TreeNode currentNode = _tree.SelectedNode;
                if (ReferenceEquals(currentNode, null))
                {
                    return null;
                }

                PftNodeInfo result = currentNode.Tag as PftNodeInfo;

                return result;
            }
        }

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
            string text = info.ToString();

            TreeNode result = new TreeNode
            {
                Tag = info,
                Text = text,
                ToolTipText = text
            };

            foreach (PftNodeInfo child in info.Children)
            {
                if (!ReferenceEquals(child, null))
                {
                    TreeNode node = _ConvertNode(child);
                    if (!ReferenceEquals(node, null))
                    {
                        result.Nodes.Add(node);
                    }
                }
            }

            return result;
        }

        private void _tree_AfterCheck
            (
                object sender,
                TreeViewEventArgs e
            )
        {
            NodeChecked.Raise(sender, e);
        }

        void _tree_AfterSelect
            (
                object sender,
                TreeViewEventArgs e
            )
        {
            CurrentNodeChanged.Raise(sender, e);
        }

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
                PftNodeInfo rootInfo = rootNode.GetNodeInfo();
                TreeNode treeNode = _ConvertNode(rootInfo);
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

