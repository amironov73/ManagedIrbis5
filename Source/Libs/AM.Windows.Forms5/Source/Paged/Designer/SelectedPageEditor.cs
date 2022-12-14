// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* SelectedPageEditor.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.ComponentModel;
using System.ComponentModel.Design;

#endregion

#nullable enable

namespace Manina.Windows.Forms;

public partial class PagedControl
{
    /// <summary>
    ///
    /// </summary>
    protected internal class SelectedPageEditor
        : ObjectSelectorEditor
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="selector"></param>
        /// <param name="context"></param>
        /// <param name="provider"></param>
        protected override void FillTreeWithData
            (
                Selector selector,
                ITypeDescriptorContext context,
                IServiceProvider provider
            )
        {
            base.FillTreeWithData (selector, context, provider);

            var control = (PagedControl)context.Instance;

            foreach (var page in control.Pages)
            {
                SelectorNode node = new SelectorNode (page.Name, page);
                selector.Nodes.Add (node);

                if (page == control.SelectedPage)
                {
                    selector.SelectedNode = node;
                }
            }
        }
    }
}
