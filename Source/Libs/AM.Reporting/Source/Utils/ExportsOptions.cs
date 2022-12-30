// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/*
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;

#endregion

#nullable enable

namespace AM.Reporting.Utils
{
#if !COMMUNITY
    /// <summary>
    /// Class for handling Exports visibility in the Preview control.
    /// </summary>
    public partial class ExportsOptions
    {
        private static ExportsOptions instance;

        private List<ExportsTreeNode> menuNodes;

        /// <summary>
        /// All exports available in the Preview control.
        /// </summary>
        public List<ExportsTreeNode> ExportsMenu
        {
            get
            {
                RemoveCloudsAndMessengersDuplicatesInMenuNodes();
                return menuNodes;
            }
        }

        private ExportsOptions()
        {
            menuNodes = new List<ExportsTreeNode>();
        }

        private void RemoveCloudsAndMessengersDuplicatesInMenuNodes()
        {
            var last = menuNodes.Count - 1;
            for (var i = last; i >= 0; i--)
            {
                var node = menuNodes[i];
                if (node.Name == "Cloud" || node.Name == "Messengers")
                {
                    menuNodes.Remove (node);
                }
            }
        }

        /// <summary>
        /// Gets an instance of ExportOptions.
        /// </summary>
        /// <returns></returns>
        public static ExportsOptions GetInstance()
        {
            if (instance == null)
            {
                instance = new ExportsOptions();
            }

            return instance;
        }

        /// <summary>
        /// Exports menu node.
        /// </summary>
        public partial class ExportsTreeNode
        {
            private const string EXPORT_ITEM_PREFIX = "Export,";
            private const string ITEM_POSTFIX = ",Name";
            private const string EXPORT_ITEM_POSTFIX = ",File";
            private const string CATEGORY_PREFIX = "Export,ExportGroups,";

            /// <summary>
            /// Gets the name.
            /// </summary>
            public string Name { get; }

            /// <summary>
            /// Gets nodes.
            /// </summary>
            public List<ExportsTreeNode> Nodes { get; } = new List<ExportsTreeNode>();

            /// <summary>
            /// Gets type of the export.
            /// </summary>
            public Type ExportType { get; }

            /// <summary>
            /// Gets index of the image.
            /// </summary>
            public int ImageIndex { get; } = -1;

            /// <summary>
            /// Gets or sets the tag.
            /// </summary>
            public ObjectInfo Tag { get; set; }

            /// <summary>
            /// Gets or sets a value that indicates is node enabled.
            /// </summary>
            public bool Enabled { get; set; } = true;

            /// <summary>
            /// Gets true if node is export, otherwise false.
            /// </summary>
            public bool IsExport { get; }

            public ExportsTreeNode (string name, bool isExport)
            {
                this.Name = name;
                this.IsExport = isExport;
            }

            public ExportsTreeNode (string name, Type exportType, bool isExport)
                : this (name, isExport)
            {
                this.ExportType = exportType;
            }

            public ExportsTreeNode (string name, Type exportType, bool isExport, ObjectInfo tag)
                : this (name, exportType, isExport)
            {
                this.Tag = tag;
            }

            public ExportsTreeNode (string name, int imageIndex, bool isExport)
                : this (name, isExport)
            {
                this.ImageIndex = imageIndex;
            }

            public ExportsTreeNode (string name, Type exportType, int imageIndex, bool isExport)
                : this (name, exportType, isExport)
            {
                this.ImageIndex = imageIndex;
            }

            public ExportsTreeNode (string name, Type exportType, int imageIndex, bool isExport, ObjectInfo tag)
                : this (name, exportType, imageIndex, isExport)
            {
                this.Tag = tag;
            }

            public ExportsTreeNode (string name, Type exportType, int imageIndex, bool enabled, bool isExport)
                : this (name, exportType, imageIndex, isExport)
            {
                this.Enabled = enabled;
            }

            ///<inheritdoc/>
            public override bool Equals (object obj)
            {
                if (obj is ExportsTreeNode treeNode)
                {
                    var equalNodes = true;

                    foreach (var node in Nodes)
                    {
                        equalNodes &= node.ContainsIn (treeNode.Nodes);
                    }

                    return equalNodes && treeNode.Name == Name && treeNode.ImageIndex == ImageIndex && treeNode.IsExport == IsExport &&
                           treeNode.ExportType == ExportType && treeNode.Enabled == Enabled;
                }

                return base.Equals (obj);
            }

            /// <summary>
            /// If it is equal node or its contained in node childnodes, it returns true, otherwise false.
            /// </summary>
            /// <param name="node"></param>
            /// <returns></returns>
            public bool ContainsIn (ExportsTreeNode node)
            {
                return Equals (node) || ContainsIn (node.Nodes);
            }

            /// <summary>
            /// If it is contained in the list or in its elements, it returns true, otherwise false.
            /// </summary>
            /// <param name="nodes"></param>
            /// <returns></returns>
            public bool ContainsIn (List<ExportsTreeNode> nodes)
            {
                foreach (var n in nodes)
                {
                    if (Equals (n) || ContainsIn (n))
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        /// <summary>
        /// Saves current visible exports in config file.
        /// </summary>
        public void SaveExportOptions()
        {
            SaveOptions();
        }

        /// <summary>
        /// Restores visible exports from config file.
        /// </summary>
        public void RestoreExportOptions()
        {
            RestoreOptions();
        }

        /// <summary>
        ///
        /// </summary>
        public void RegisterExports()
        {
            Queue<ExportsTreeNode> queue = new Queue<ExportsTreeNode> (menuNodes);

            while (queue.Count != 0)
            {
                var node = queue.Dequeue();
                ObjectInfo tag = null;
                if (node.ExportType != null)
                {
                    tag = RegisteredObjects.AddExport (node.ExportType, node.ToString(), node.ImageIndex);
                }

                node.Tag = tag;
                foreach (var nextNode in node.Nodes)
                {
                    queue.Enqueue (nextNode);
                }
            }
        }

        private ExportsTreeNode FindItem (string name, Type exportType)
        {
            Queue<ExportsTreeNode> queue = new Queue<ExportsTreeNode> (menuNodes);

            while (queue.Count != 0)
            {
                var node = queue.Dequeue();

                if (exportType != null && node.ExportType == exportType ||
                    !string.IsNullOrEmpty (name) && node.Name == name)
                {
                    return node;
                }

                foreach (var nextNode in node.Nodes)
                {
                    queue.Enqueue (nextNode);
                }
            }

            return null;
        }

        /// <summary>
        /// Sets Export category visibility.
        /// </summary>
        /// <param name="name">Export category name.</param>
        /// <param name="enabled">Visibility state.</param>
        public void SetExportCategoryEnabled (string name, bool enabled)
        {
            FindItem (name, null).Enabled = enabled;
        }

        /// <summary>
        /// Sets Export visibility.
        /// </summary>
        /// <param name="exportType">Export type.</param>
        /// <param name="enabled">Visibility state.</param>
        public void SetExportEnabled (Type exportType, bool enabled)
        {
            var node = FindItem (null, exportType);
            if (node != null)
            {
                node.Enabled = enabled;
            }
        }
    }
#endif
}
