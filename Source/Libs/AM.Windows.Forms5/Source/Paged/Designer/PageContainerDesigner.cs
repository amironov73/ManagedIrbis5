// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global
// ReSharper disable VirtualMemberCallInConstructor

/* PageContainerDesigner.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;

#endregion

#nullable enable

namespace Manina.Windows.Forms;

public partial class PagedControl
{
    /// <summary>
    ///
    /// </summary>
    protected internal class PageContainerDesigner
        : ParentControlDesigner
    {
        #region Properties

        /// <summary>
        /// Gets the designer host.
        /// </summary>
        public IDesignerHost? DesignerHost { get; private set; }

        /// <summary>
        /// Gets the selection service.
        /// </summary>
        public ISelectionService? SelectionService { get; private set; }

        /// <summary>
        /// Gets the parent control.
        /// </summary>
        public new PagedControl Control => (PagedControl)base.Control;

        #endregion

        #region Initialize/Dispose

        /// <inheritdoc cref="ParentControlDesigner.Initialize"/>
        public override void Initialize
            (
                IComponent component
            )
        {
            base.Initialize (component);

            DesignerHost = (IDesignerHost) GetService (typeof (IDesignerHost));
            SelectionService = (ISelectionService) GetService (typeof (ISelectionService));
        }

        /// <inheritdoc cref="ParentControlDesigner.InitializeNewComponent"/>
        public override void InitializeNewComponent
            (
                IDictionary defaultValues
            )
        {
            base.InitializeNewComponent (defaultValues);

            if (Control.Pages.Count == 0)
            {
                // add two default pages
                AddPageHandler (this, EventArgs.Empty);
                AddPageHandler (this, EventArgs.Empty);

                var member = TypeDescriptor.GetProperties (Component)["Controls"];
                RaiseComponentChanging (member);
                RaiseComponentChanged (member, null, null);

                Control.SelectedIndex = 0;
            }
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Gets the designer of the current page.
        /// </summary>
        /// <returns>The designer of the page currently active in the designer.</returns>
        internal Page.PageDesigner? CurrentPageDesigner
        {
            get
            {
                if (Control.SelectedPage is { } page && DesignerHost != null)
                {
                    return (Page.PageDesigner?) DesignerHost.GetDesigner (page);
                }

                return null;
            }
        }

        #endregion

        #region Verb Handlers

        /// <summary>
        /// Adds a new page.
        /// </summary>
        protected void AddPageHandler
            (
                object sender,
                EventArgs eventArgs
            )
        {
            if (DesignerHost != null)
            {
                var page = (Page)DesignerHost.CreateComponent (typeof (Page));
                page.Text = $"Page {Control.Pages.Count + 1}";
                Control.Pages.Add (page);
                Control.SelectedPage = page;

                if (SelectionService != null)
                {
                    SelectionService.SetSelectedComponents (new Component[] { Control.SelectedPage });
                }
            }
        }

        /// <summary>
        /// Removes the current page.
        /// </summary>
        protected void RemovePageHandler
            (
                object sender,
                EventArgs eventArgs
            )
        {
            if (DesignerHost != null)
            {
                if (Control.Pages.Count > 1)
                {
                    var page = Control.SelectedPage;
                    if (page != null)
                    {
                        var index = Control.SelectedIndex;

                        DesignerHost.DestroyComponent (page);
                        if (index == Control.Pages.Count)
                        {
                            index = Control.Pages.Count - 1;
                        }

                        Control.SelectedIndex = index;

                        SelectionService?.SetSelectedComponents (new Component[] { Control.SelectedPage! });
                    }
                }
            }
        }

        /// <summary>
        /// Navigates to the previous page.
        /// </summary>
        protected void NavigateBackHandler
            (
                object sender,
                EventArgs eventArgs
            )
        {
            Control.GoBack();

            SelectionService?.SetSelectedComponents (new Component[] { Control.SelectedPage! });
        }

        /// <summary>
        /// Navigates to the next page.
        /// </summary>
        protected void NavigateNextHandler
            (
                object sender,
                EventArgs eventArgs
            )
        {
            Control.GoNext();

            SelectionService?.SetSelectedComponents (new Component[] { Control.SelectedPage! });
        }

        #endregion

        #region Parent/Child Relation

        /// <inheritdoc cref="ParentControlDesigner.CanParent(System.Windows.Forms.Control)"/>
        public override bool CanParent
            (
                Control control
            )
        {
            return control is Page;
        }

        /// <inheritdoc cref="ParentControlDesigner.CanParent(System.Windows.Forms.Design.ControlDesigner)"/>
        public override bool CanParent
            (
                ControlDesigner? controlDesigner
            )
        {
            return controlDesigner is { Component: Page };
        }

        #endregion

        #region Delegate All Drag Events To The Current Page

        /// <inheritdoc cref="ParentControlDesigner.OnDragEnter"/>
        protected override void OnDragEnter
            (
                DragEventArgs eventArgs
            )
        {
            var pageDesigner = CurrentPageDesigner;
            if (pageDesigner == null)
            {
                base.OnDragEnter (eventArgs);
            }
            else
            {
                pageDesigner.OnDragEnter (eventArgs);
            }
        }

        /// <inheritdoc cref="ParentControlDesigner.OnDragOver"/>
        protected override void OnDragOver
            (
                DragEventArgs eventArgs
            )
        {
            var pageDesigner = CurrentPageDesigner;
            if (pageDesigner == null)
            {
                base.OnDragOver (eventArgs);
            }
            else
            {
                var pt = Control.PointToClient (new Point (eventArgs.X, eventArgs.Y));

                if (pageDesigner.Control.DisplayRectangle.Contains (pt))
                {
                    pageDesigner.OnDragOver (eventArgs);
                }
                else
                {
                    eventArgs.Effect = DragDropEffects.None;
                }
            }
        }

        /// <inheritdoc cref="ParentControlDesigner.OnDragLeave"/>
        protected override void OnDragLeave
            (
                EventArgs eventArgs
            )
        {
            var pageDesigner = CurrentPageDesigner;
            if (pageDesigner == null)
            {
                base.OnDragLeave (eventArgs);
            }
            else
            {
                pageDesigner.OnDragLeave (eventArgs);
            }
        }

        /// <inheritdoc cref="ParentControlDesigner.OnDragDrop"/>
        protected override void OnDragDrop
            (
                DragEventArgs eventArgs
            )
        {
            var pageDesigner = CurrentPageDesigner;
            if (pageDesigner == null)
            {
                base.OnDragDrop (eventArgs);
            }
            else
            {
                pageDesigner.OnDragDrop (eventArgs);
            }
        }

        /// <inheritdoc cref="ControlDesigner.OnGiveFeedback"/>
        protected override void OnGiveFeedback
            (
                GiveFeedbackEventArgs eventArgs
            )
        {
            var pageDesigner = CurrentPageDesigner;
            if (pageDesigner == null)
            {
                base.OnGiveFeedback (eventArgs);
            }
            else
            {
                pageDesigner.OnGiveFeedback (eventArgs);
            }
        }

        /// <inheritdoc cref="ParentControlDesigner.OnDragComplete"/>
        protected override void OnDragComplete
            (
                DragEventArgs eventArgs
            )
        {
            var pageDesigner = CurrentPageDesigner;
            if (pageDesigner == null)
            {
                base.OnDragComplete (eventArgs);
            }
            else
            {
                pageDesigner.OnDragComplete (eventArgs);
            }
        }

        #endregion
    }
}
