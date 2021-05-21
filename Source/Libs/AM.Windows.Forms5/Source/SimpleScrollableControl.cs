// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable UnusedMember.Global

/*
 * SimpleScrollableControl.cs
 */

#region Using directives

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms
{
    /// <summary>
    ///
    /// </summary>
    // ReSharper disable RedundantNameQualifier
    [System.ComponentModel.DesignerCategory("Code")]
    // ReSharper restore RedundantNameQualifier
    public class SimpleScrollableControl
        : Control
    {
        #region Events

        /// <summary>
        ///   Occurs when [scroll].
        /// </summary>
        [Category("Action")]
        public event ScrollEventHandler? Scroll;

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        public SimpleScrollableControl()
        {
            HorizontalScroll = new ScrollSettings(this, NativeMethods.ScrollBarKind.SB_HORZ);
            VerticalScroll = new ScrollSettings(this, NativeMethods.ScrollBarKind.SB_VERT);
        }

        #endregion

        #region Private members

        private void HandleScroll
            (
            ref Message message,
            ScrollSettings settings
            )
        {
            int newPosition = settings.Position;
            int oldPosition = newPosition;
            ScrollEventType type = ScrollEventType.ThumbTrack;

            switch (NativeMethods.LOWORD(message.WParam))
            {
                case NativeMethods.SB_LINEUP:
                    newPosition -= settings.SmallChange;
                    type = ScrollEventType.SmallDecrement;
                    break;
                case NativeMethods.SB_LINEDOWN:
                    newPosition += settings.SmallChange;
                    type = ScrollEventType.SmallIncrement;
                    break;
                case NativeMethods.SB_PAGEUP:
                    newPosition -= settings.LargeChange;
                    type = ScrollEventType.LargeDecrement;
                    break;
                case NativeMethods.SB_PAGEDOWN:
                    newPosition += settings.LargeChange;
                    type = ScrollEventType.LargeIncrement;
                    break;
                case NativeMethods.SB_THUMBPOSITION:
                    newPosition = settings.TrackPosition;
                    type = ScrollEventType.ThumbPosition;
                    break;
                case NativeMethods.SB_THUMBTRACK:
                    newPosition = settings.TrackPosition;
                    type = ScrollEventType.ThumbTrack;
                    break;
                case NativeMethods.SB_TOP:
                    newPosition = settings.Minimum;
                    type = ScrollEventType.First;
                    break;
                case NativeMethods.SB_BOTTOM:
                    newPosition = settings.Maximum;
                    type = ScrollEventType.Last;
                    break;
                case NativeMethods.SB_ENDSCROLL:
                    type = ScrollEventType.EndScroll;
                    break;
            }

            newPosition = Math.Max(settings.Minimum, newPosition);
            newPosition = Math.Min(settings.Maximum, newPosition);

            settings.Position = newPosition;

            ScrollOrientation orientation =
                settings._kind == NativeMethods.ScrollBarKind.SB_HORZ
                    ? ScrollOrientation.HorizontalScroll
                    : ScrollOrientation.VerticalScroll;
            ScrollEventArgs args = new ScrollEventArgs
                (
                type,
                oldPosition,
                newPosition,
                orientation
                );
            OnScroll(args);

            //Debug.WriteLine(settings);
        }

        /// <summary>
        ///   Raises the <see cref = "Scroll" /> event.
        /// </summary>
        /// <param name = "args">The <see cref = "System.Windows.Forms.ScrollEventArgs" /> instance containing the event data.</param>
        protected virtual void OnScroll (ScrollEventArgs args)
            => Scroll?.Invoke(this, args);

        /// <summary>
        ///   Processes Windows messages.
        /// </summary>
        /// <param name = "m">The Windows
        ///   <see cref = "T:System.Windows.Forms.Message" />
        ///   to process.</param>
        protected override void WndProc(ref Message m)
        {
            //Debug.WriteLine(string.Format("Message: {0:x}",m.Msg));
            switch (m.Msg)
            {
                case NativeMethods.WM_HSCROLL:
                    HandleScroll(ref m, HorizontalScroll);
                    return;

                case NativeMethods.WM_VSCROLL:
                    HandleScroll(ref m, VerticalScroll);
                    return;
            }
            base.WndProc(ref m);
        }

        #endregion

        #region Properties

        /// <summary>
        ///   Gets the horizontal scroll.
        /// </summary>
        /// <value>The horizontal scroll.</value>
        [Category("Layout")]
        [DesignerSerializationVisibility (DesignerSerializationVisibility.Content)]
        public ScrollSettings HorizontalScroll { get; }

        /// <summary>
        ///   Gets the vertical scroll.
        /// </summary>
        /// <value>The vertical scroll.</value>
        [Category("Layout")]
        [DesignerSerializationVisibility (DesignerSerializationVisibility.Content)]
        public ScrollSettings VerticalScroll { get; }

        /// <summary>
        ///   Gets the required creation parameters when the control handle is created.
        /// </summary>
        /// <value></value>
        /// <returns>
        ///   A <see cref = "T:System.Windows.Forms.CreateParams" /> that contains the required creation parameters when the handle to the control is created.
        /// </returns>
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams result = base.CreateParams;
                result.Style |= NativeMethods.WS_HSCROLL | NativeMethods.WS_VSCROLL;
                return result;
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        ///   Retrieves the size of a rectangular area into
        ///   which a control can be fitted.
        /// </summary>
        /// <param name = "proposedSize">The custom-sized area
        ///   for a control.</param>
        /// <returns>
        ///   An ordered pair of type
        ///   <see cref = "T:System.Drawing.Size" /> representing
        ///   the width and height of a rectangle.
        /// </returns>
        public override Size GetPreferredSize(Size proposedSize)
        {
            return new Size(200, 100);
        }

        #endregion
    }
}
