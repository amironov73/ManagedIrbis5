// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* PagedControlDesigner.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Windows.Forms.Design;
using System.Windows.Forms.Design.Behavior;

#endregion

#nullable enable

namespace Manina.Windows.Forms;

public partial class PagedControl
{
    /// <summary>
    ///
    /// </summary>
    protected internal class PagedControlDesigner
        : PageContainerDesigner
    {
        #region Member Variables

        /// <summary>
        ///
        /// </summary>
        protected DesignerVerb? addPageVerb;

        /// <summary>
        ///
        /// </summary>
        protected DesignerVerb? removePageVerb;

        /// <summary>
        ///
        /// </summary>
        protected DesignerVerb? navigateBackVerb;

        /// <summary>
        ///
        /// </summary>
        protected DesignerVerb? navigateNextVerb;

        /// <summary>
        ///
        /// </summary>
        protected DesignerVerbCollection? verbs;

        /// <summary>
        ///
        /// </summary>
        protected bool toolbarAtBottom = true;

        /// <summary>
        ///
        /// </summary>
        protected GlyphToolBar? toolbar;

        /// <summary>
        ///
        /// </summary>
        protected ButtonGlyph? moveToolbarButton;

        /// <summary>
        ///
        /// </summary>
        protected ButtonGlyph? addPageButton;

        /// <summary>
        ///
        /// </summary>
        protected ButtonGlyph? removePageButton;

        /// <summary>
        ///
        /// </summary>
        protected ButtonGlyph? navigateBackButton;

        /// <summary>
        ///
        /// </summary>
        protected ButtonGlyph? navigateNextButton;

        /// <summary>
        ///
        /// </summary>
        protected LabelGlyph? currentPageLabel;

        /// <summary>
        ///
        /// </summary>
        private Adorner? toolbarAdorner;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the list of designer verbs.
        /// </summary>
        public override DesignerVerbCollection Verbs => verbs!;

        /// <summary>
        /// Gets the location of the designer toolbar relative to the parent control.
        /// </summary>
        public Point ToolbarLocation => new Point (8, toolbarAtBottom ? Control.Height - toolbar!.Size.Height - 8 : 8);

        /// <summary>
        /// Gets the glyph toolbar of the designer.
        /// </summary>
        public GlyphToolBar ToolBar => toolbar!;

        #endregion

        #region Glyph Icons

        private static PointF[][] GetUpDownArrowSign (float size)
        {
            var arrowSize = size;
            var arrowSeparator = 0.25f * size;

            return new[]
            {
                new PointF[]
                {
                    new (size / 2f, 0),
                    new (size / 2f + arrowSize / 2f, size / 2f - arrowSeparator / 2f),
                    new (size / 2f - arrowSize / 2f, size / 2f - arrowSeparator / 2f),
                },
                new PointF[]
                {
                    new (size / 2f, size),
                    new (size / 2f + arrowSize / 2f, size / 2f + arrowSeparator / 2f),
                    new (size / 2f - arrowSize / 2f, size / 2f + arrowSeparator / 2f),
                }
            };
        }

        private static PointF[][] GetLeftArrowSign (float size)
        {
            var arrowHeadThickness = size;
            var arrowTailThickness = 0.375f * size;
            var arrowHeadLength = 0.5625f * size;
            var arrowTailLength = size - arrowHeadLength;

            return new[]
            {
                new[]
                {
                    new PointF (0, size / 2f),
                    new PointF (arrowHeadLength, size / 2f - arrowHeadThickness / 2f),
                    new PointF (arrowHeadLength, size / 2f - arrowTailThickness / 2f),
                    new PointF (arrowHeadLength + arrowTailLength, size / 2f - arrowTailThickness / 2f),
                    new PointF (arrowHeadLength + arrowTailLength, size / 2f + arrowTailThickness / 2f),
                    new PointF (arrowHeadLength, size / 2f + arrowTailThickness / 2f),
                    new PointF (arrowHeadLength, size / 2f + arrowHeadThickness / 2f),
                }
            };
        }

        private static PointF[][] GetRightArrowSign (float size)
        {
            var arrowHeadThickness = size;
            var arrowTailThickness = 0.375f * size;
            var arrowHeadLength = 0.5625f * size;
            var arrowTailLength = size - arrowHeadLength;

            return new[]
            {
                new[]
                {
                    new PointF (size, size / 2f),
                    new PointF (size - arrowHeadLength, size / 2f - arrowHeadThickness / 2f),
                    new PointF (size - arrowHeadLength, size / 2f - arrowTailThickness / 2f),
                    new PointF (size - arrowHeadLength - arrowTailLength, size / 2f - arrowTailThickness / 2f),
                    new PointF (size - arrowHeadLength - arrowTailLength, size / 2f + arrowTailThickness / 2f),
                    new PointF (size - arrowHeadLength, size / 2f + arrowTailThickness / 2f),
                    new PointF (size - arrowHeadLength, size / 2f + arrowHeadThickness / 2f),
                }
            };
        }

        private static PointF[][] GetPlusSign (float size)
        {
            var thickness = 0.375f * size;

            return new[]
            {
                new[]
                {
                    new PointF (0, size / 2f - thickness / 2f),
                    new PointF (size / 2f - thickness / 2f, size / 2f - thickness / 2f),
                    new PointF (size / 2f - thickness / 2f, 0),
                    new PointF (size / 2f + thickness / 2f, 0),
                    new PointF (size / 2f + thickness / 2f, size / 2f - thickness / 2f),
                    new PointF (size, size / 2f - thickness / 2f),
                    new PointF (size, size / 2f + thickness / 2f),
                    new PointF (size / 2f + thickness / 2f, size / 2f + thickness / 2f),
                    new PointF (size / 2f + thickness / 2f, size),
                    new PointF (size / 2f - thickness / 2f, size),
                    new PointF (size / 2f - thickness / 2f, size / 2f + thickness / 2f),
                    new PointF (0, size / 2f + thickness / 2f),
                }
            };
        }

        private static PointF[][] GetMinusSign (float size)
        {
            var thickness = 0.375f * size;

            return new[]
            {
                new[]
                {
                    new PointF (0, size / 2f - thickness / 2f),
                    new PointF (size, size / 2f - thickness / 2f),
                    new PointF (size, size / 2f + thickness / 2f),
                    new PointF (0, size / 2f + thickness / 2f),
                }
            };
        }

        #endregion

        #region Initialize/Dispose

        /// <inheritdoc cref="PageContainerDesigner.Initialize"/>
        public override void Initialize (IComponent component)
        {
            base.Initialize (component);

            CreateVerbs();
            CreateGlyphs();

            Control.PageChanged += Control_CurrentPageChanged;
            Control.PageAdded += Control_PageAdded;
            Control.PageRemoved += Control_PageRemoved;
            Control.Resize += Control_Resize;
            Control.Move += Control_Move;

            if (SelectionService != null)
            {
                SelectionService.SelectionChanged += SelectionService_SelectionChanged;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="defaultValues"></param>
        public override void InitializeNewComponent
            (
                IDictionary defaultValues
            )
        {
            base.InitializeNewComponent (defaultValues);

            UpdateGlyphsAndVerbs();
        }

        /// <inheritdoc cref="ParentControlDesigner.Dispose(bool)"/>
        protected override void Dispose (bool disposing)
        {
            if (disposing)
            {
                Control.PageChanged -= Control_CurrentPageChanged;
                Control.PageAdded -= Control_PageAdded;
                Control.PageRemoved -= Control_PageRemoved;
                Control.Resize -= Control_Resize;
                Control.Move -= Control_Move;

                navigateBackButton!.Click -= NavigateBackButton_Click;
                navigateNextButton!.Click -= NavigateNextButton_Click;
                addPageButton!.Click -= AddPageButton_Click;
                removePageButton!.Click -= RemovePageButton_Click;

                if (BehaviorService != null)
                {
                    BehaviorService.Adorners.Remove (toolbarAdorner);
                }

                if (SelectionService != null)
                {
                    SelectionService.SelectionChanged -= SelectionService_SelectionChanged;
                }

                toolbar!.Dispose();
            }

            base.Dispose (disposing);
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Creates the designer verbs.
        /// </summary>
        private void CreateVerbs()
        {
            navigateBackVerb = new DesignerVerb ("Previous page", NavigateBackHandler!);
            navigateNextVerb = new DesignerVerb ("Next page", NavigateNextHandler!);
            addPageVerb = new DesignerVerb ("Add page", AddPageHandler!);
            removePageVerb = new DesignerVerb ("Remove page", RemovePageHandler!);

            verbs = new DesignerVerbCollection();
            verbs.AddRange (new[]
            {
                navigateBackVerb, navigateNextVerb, addPageVerb, removePageVerb
            });
        }

        /// <summary>
        /// Creates the glyphs for navigation and manipulating pages
        /// </summary>
        private void CreateGlyphs()
        {
            toolbarAdorner = new Adorner();
            if (BehaviorService != null)
            {
                BehaviorService.Adorners.Add (toolbarAdorner);
            }

            toolbar = new GlyphToolBar (BehaviorService!, this, toolbarAdorner);

            moveToolbarButton = new ButtonGlyph
            {
                Path = GetUpDownArrowSign (toolbar.DefaultIconSize.Height),
                ToolTipText = "Move toolbar"
            };
            moveToolbarButton.Click += MoveToolbarButton_Click;

            navigateBackButton = new ButtonGlyph
            {
                Path = GetLeftArrowSign (toolbar.DefaultIconSize.Height),
                ToolTipText = "Previous page"
            };
            navigateBackButton.Click += NavigateBackButton_Click;

            navigateNextButton = new ButtonGlyph
            {
                Path = GetRightArrowSign (toolbar.DefaultIconSize.Height),
                ToolTipText = "Next page"
            };
            navigateNextButton.Click += NavigateNextButton_Click;

            addPageButton = new ButtonGlyph
            {
                Path = GetPlusSign (toolbar.DefaultIconSize.Height),
                ToolTipText = "Add a new page"
            };
            addPageButton.Click += AddPageButton_Click;

            removePageButton = new ButtonGlyph
            {
                Path = GetMinusSign (toolbar.DefaultIconSize.Height),
                ToolTipText = "Remove current page"
            };
            removePageButton.Click += RemovePageButton_Click;

            currentPageLabel = new LabelGlyph
            {
                Text = $"Page {Control.SelectedIndex + 1} of {Control.Pages.Count}"
            };

            toolbar.AddButton (moveToolbarButton);
            toolbar.AddButton (new SeparatorGlyph());
            toolbar.AddButton (navigateBackButton);
            toolbar.AddButton (currentPageLabel);
            toolbar.AddButton (navigateNextButton);
            toolbar.AddButton (new SeparatorGlyph());
            toolbar.AddButton (addPageButton);
            toolbar.AddButton (removePageButton);

            toolbarAdorner.Glyphs.Add (toolbar);
        }

        /// <summary>
        /// Updates the adorner when the selection is changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void SelectionService_SelectionChanged
            (
                object? sender,
                EventArgs eventArgs
            )
        {
            var showAdorner = false;

            if (SelectionService is { PrimarySelection: { } })
            {
                if (SelectionService.PrimarySelection == Control)
                {
                    showAdorner = true;
                }
                else if (SelectionService.PrimarySelection is Page page && page.Parent == Control)
                {
                    showAdorner = true;
                }
            }

            toolbarAdorner!.Enabled = toolbar!.Visible && showAdorner;
        }

        /// <summary>
        /// Updates verbs and toolbar buttons when the current page is changed.
        /// </summary>
        private void Control_CurrentPageChanged
            (
                object? sender,
                PageChangedEventArgs eventArgs
            )
        {
            UpdateGlyphsAndVerbs();
        }

        /// <summary>
        /// Updates verbs and toolbar buttons when a new page is added.
        /// </summary>
        private void Control_PageAdded
            (
                object? sender,
                PageEventArgs eventArgs
            )
        {
            UpdateGlyphsAndVerbs();
        }

        /// <summary>
        /// Updates verbs and toolbar buttons when a page is removed.
        /// </summary>
        private void Control_PageRemoved
            (
                object? sender,
                PageEventArgs eventArgs
            )
        {
            UpdateGlyphsAndVerbs();
        }

        /// <summary>
        /// Relocate the toolbar when the control is resized.
        /// </summary>
        private void Control_Resize
            (
                object? sender,
                EventArgs eventArgs
            )
        {
            UpdateGlyphsAndVerbs();
        }

        /// <summary>
        /// Relocate the toolbar when the control is moved.
        /// </summary>
        private void Control_Move
            (
                object? sender,
                EventArgs eventArgs
            )
        {
            UpdateGlyphsAndVerbs();
        }

        /// <summary>
        /// Updates the visual states of the toolbar and its glyphs.
        /// </summary>
        private void UpdateGlyphsAndVerbs()
        {
            removePageVerb!.Enabled = removePageButton!.Enabled = (Control.Pages.Count > 1);
            navigateBackVerb!.Enabled = navigateBackButton!.Enabled = (Control.SelectedIndex > 0);
            navigateNextVerb!.Enabled = navigateNextButton!.Enabled = (Control.SelectedIndex < Control.Pages.Count - 1);
            currentPageLabel!.Text = Control.Pages.Count == 0
                ? string.Empty
                : $"Page {Control.SelectedIndex + 1} of {Control.Pages.Count}";

            toolbar!.UpdateLayout();
            toolbar.Location = ToolbarLocation;
            toolbar.Refresh();
        }

        private void MoveToolbarButton_Click
            (
                object? sender,
                EventArgs eventArgs
            )
        {
            toolbarAtBottom = !toolbarAtBottom;

            UpdateGlyphsAndVerbs();
        }

        private void NavigateBackButton_Click
            (
                object? sender,
                EventArgs eventArgs
            )
        {
            NavigateBackHandler (this, EventArgs.Empty);
        }

        private void NavigateNextButton_Click
            (
                object? sender,
                EventArgs eventArgs
            )
        {
            NavigateNextHandler (this, EventArgs.Empty);
        }

        private void AddPageButton_Click
            (
                object? sender,
                EventArgs eventArgs
            )
        {
            AddPageHandler (this, EventArgs.Empty);
        }

        private void RemovePageButton_Click
            (
                object? sender,
                EventArgs eventArgs
            )
        {
            RemovePageHandler (this, EventArgs.Empty);
        }

        #endregion
    }
}
