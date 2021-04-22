// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* TreeGridTreeColumn.cs
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Drawing;

#endregion

#nullable enable

namespace AM.Windows.Forms
{
    public class TreeGridTreeColumn
        : TreeGridColumn
    {
        #region TreeGridColumn members

        /// <summary>
        /// Gets a value indicating whether this <see cref="TreeGridColumn"/> is editable.
        /// </summary>
        /// <value><c>true</c> if editable; otherwise, <c>false</c>.</value>
        public override bool Editable
        {
            get
            {
                return false;
            }
        }

        protected internal override void OnMouseClick
            (
                TreeGridMouseEventArgs args
            )
        {
            //base.OnMouseClick(args);
            var node = args.Node;
            if (node != null)
            {
                var layout = MakeLayout(node,node.Bounds);
                var clickKind = layout.DetermineClickKind(args.Location);
                switch (clickKind)
                {
                    case TreeGridClickKind.Expand:
                        node.Expanded = !node.Expanded;
                        break;

                    case TreeGridClickKind.Check:
                        node.Checked = !node.Checked;
                        break;

                    case TreeGridClickKind.Icon:
                        // Do not know what to do
                        break;

                    case TreeGridClickKind.Text:
                        if (node.Checkable)
                        {
                            node.Checked = !node.Checked;
                        }
                        else
                        {
                            node.Expanded = !node.Expanded;
                        }
                        break;
                }
            }
        }

        public virtual TreeGridDrawLayout MakeLayout
            (
                TreeGridNode node,
                Rectangle bounds
            )
        {
            var result = new TreeGridDrawLayout();

            //TreeGridNode node = args.Node;
            //Rectangle bounds = args.Bounds;
            var left = 0;

            left += node.Level * 16;

            if (node.HasChildren)
            {
                result.Expand = new Rectangle
                    (
                        left,
                        bounds.Top,
                        16,
                        bounds.Height
                    );
                left += result.Expand.Width;
            }

            if (node.Checkable)
            {
                result.Check = new Rectangle
                    (
                        left,
                        bounds.Top,
                        16,
                        bounds.Height
                    );

                left += result.Check.Width;
            }

            if (node.Icon != null)
            {
                result.Icon = new Rectangle
                    (
                        left,
                        bounds.Top,
                        node.Icon.Width + 2,
                        bounds.Height
                    );
                left += result.Icon.Width;
            }

            result.Text = new Rectangle
                (
                    left,
                    bounds.Top,
                    bounds.Width - left,
                    bounds.Height
                );

            return result;
        }

        /// <summary>
        /// Overrides <see cref="TreeGridColumn.OnDrawCell"/> method.
        /// </summary>
        /// <param name="args">The <see cref="TreeGridDrawCellEventArgs"/>
        /// instance containing the event data.</param>
        protected internal override void OnDrawCell
            (
                TreeGridDrawCellEventArgs args
            )
        {
            var node = args.Node;
            var bounds = args.Bounds;
            var layout = MakeLayout(node,bounds);
            TreeGridUtilities.DrawTreeCell
                (
                    args,
                    layout
                );
        }

        #endregion
    }
}
