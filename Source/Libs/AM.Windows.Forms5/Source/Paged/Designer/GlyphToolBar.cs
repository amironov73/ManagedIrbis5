// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* GlyphToolBar.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Windows.Forms.Design.Behavior;

#endregion

#nullable enable

namespace Manina.Windows.Forms;

public partial class PagedControl
{
    /// <summary>
    /// Represent a toolbar on the designer.
    /// </summary>
    protected internal class GlyphToolBar
        : Glyph, IDisposable
    {
        #region GlyphIndexer

        /// <summary>
        /// Provides a named indexer for the Glyphs property.
        /// </summary>
        public class GlyphIndexer
        {
            private readonly GlyphToolBar _toolbar;

            /// <summary>
            ///
            /// </summary>
            /// <param name="index"></param>
            public BaseGlyph this [int index]
            {
                get => _toolbar._buttons[index];
                set => _toolbar._buttons[index] = value;
            }

            /// <summary>
            ///
            /// </summary>
            /// <param name="toolbar"></param>
            public GlyphIndexer (GlyphToolBar toolbar)
            {
                _toolbar = toolbar;
            }
        }

        #endregion

        #region Member Variables

        private readonly List<BaseGlyph> _buttons = new ();

        private Rectangle _bounds;

        private readonly Timer _tooltipTimer = new Timer();
        private const int TooltipDelay = 750;
        private ButtonGlyph? _lastHotButton;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the designed control.
        /// </summary>
        public Control Control { get; private set; }

        /// <summary>
        /// Gets the control designer.
        /// </summary>
        public ControlDesigner Designer { get; private set; }

        /// <summary>
        /// Gets the behaviour service.
        /// </summary>
        public BehaviorService BehaviorService { get; private set; }

        /// <summary>
        /// Gets the associated adorner.
        /// </summary>
        public Adorner Adorner { get; private set; }

        /// <summary>
        /// Gets or sets a glyph on the toolbar.
        /// </summary>
        public GlyphIndexer Glyphs { get; }

        /// <summary>
        /// Gets or sets whether the toolbar is visible.
        /// </summary>
        public bool Visible { get; set; } = true;

        /// <summary>
        /// Gets or sets the padding around the toolbar buttons and the toolbar border.
        /// </summary>
        public Size Padding { get; set; } = new (2, 2);

        /// <summary>
        /// Gets the default size of button icons.
        /// </summary>
        public Size DefaultIconSize = new (16, 16);

        /// <summary>
        /// Gets or sets the location of the toolbar relative to the parent control.
        /// </summary>
        public Point Location { get; set; }

        /// <summary>
        /// Gets the size of the toolbar.
        /// </summary>
        public Size Size { get; private set; }

        /// <summary>
        /// Gets the bounding rectangle of the toolbar.
        /// </summary>
        public override Rectangle Bounds => _bounds;

        /// <summary>
        /// Gets the background color of the toolbar.
        /// </summary>
        public Color BackColor { get; set; } = Color.FromArgb (207, 214, 229);

        /// <summary>
        /// Gets the border color of the toolbar.
        /// </summary>
        public Color BorderColor { get; set; } = Color.FromArgb (128, 128, 128);

        /// <summary>
        /// Gets the background color of toolbar buttons.
        /// </summary>
        public Color ButtonBackColor { get; set; } = Color.FromArgb (207, 214, 229);

        /// <summary>
        /// Gets the background color of hot toolbar buttons.
        /// </summary>
        public Color HotButtonBackColor { get; set; } = Color.FromArgb (253, 244, 191);

        /// <summary>
        /// Gets the border color of toolbar buttons.
        /// </summary>
        public Color ButtonBorderColor { get; set; } = Color.FromArgb (207, 214, 229);

        /// <summary>
        /// Gets the border color of hot toolbar buttons.
        /// </summary>
        public Color HotButtonBorderColor { get; set; } = Color.FromArgb (229, 195, 101);

        /// <summary>
        /// Gets the foreground color of toolbar button texts and icon border.
        /// </summary>
        public Color ButtonForeColor { get; set; } = Color.Black;

        /// <summary>
        /// Gets the foreground color of disabled toolbar button texts and icon border.
        /// </summary>
        public Color DisabledButtonForeColor { get; set; } = Color.FromArgb (109, 109, 109);

        /// <summary>
        /// Gets the fill color of toolbar button icons.
        /// </summary>
        public Color ButtonFillColor { get; set; } = Color.FromArgb (231, 186, 10);

        /// <summary>
        /// Gets the fill color of hot toolbar button icons.
        /// </summary>
        public Color HotButtonFillColor { get; set; } = Color.FromArgb (231, 186, 10);

        /// <summary>
        /// Gets the fill color of disabled toolbar button icons.
        /// </summary>
        public Color DisabledButtonFillColor { get; set; } = Color.FromArgb (186, 186, 186);

        /// <summary>
        /// Gets the color of toolbar separators.
        /// </summary>
        public Color SeparatorColor { get; set; } = Color.FromArgb (109, 109, 109);

        #endregion

        #region Constructor

        /// <summary>
        ///
        /// </summary>
        /// <param name="behaviorService"></param>
        /// <param name="designer"></param>
        /// <param name="adorner"></param>
        public GlyphToolBar
            (
                BehaviorService behaviorService,
                ControlDesigner designer,
                Adorner adorner
            )
            : base (new GlyphToolBarBehavior())
        {
            BehaviorService = behaviorService;
            Designer = designer;
            Control = (Control)designer.Component;
            Adorner = adorner;

            Glyphs = new GlyphIndexer (this);

            _tooltipTimer.Interval = TooltipDelay;
            _tooltipTimer.Tick += TooltipTimer_Tick;
        }

        #endregion

        #region Behavior

        /// <summary>
        /// Represents the behaviour associated with toolbars.
        /// The behaviour raises a click event when a button is clicked.
        /// </summary>
        internal class GlyphToolBarBehavior
            : Behavior
        {
            public override bool OnMouseDown
                (
                    Glyph? g,
                    MouseButtons mouseButton,
                    Point mouseLoc
                )
            {
                if (mouseButton == MouseButtons.Left)
                {
                    var toolbar = (GlyphToolBar)g!;

                    foreach (var glyph in toolbar._buttons.OfType<ButtonGlyph>())
                    {
                        var button = glyph;
                        if (button.Enabled && button.Bounds.Contains (mouseLoc))
                        {
                            button.OnClick (EventArgs.Empty);
                            return true;
                        }
                    }
                }

                return false;
            }
        }

        #endregion

        #region Event Handlers

        private void TooltipTimer_Tick
            (
                object? sender,
                EventArgs eventArgs
            )
        {
            _tooltipTimer.Stop();
            if (_lastHotButton != null!)
            {
                _lastHotButton.ShowToolTip = true;
            }

            Refresh();
        }

        #endregion

        #region Instance Methods

        /// <summary>
        /// Adds a new toolbar glyph.
        /// </summary>
        /// <param name="button"></param>
        public void AddButton (BaseGlyph button)
        {
            button.Parent = this;
            _buttons.Add (button);
        }

        /// <summary>
        /// Recalculates button locations and sizes.
        /// </summary>
        public void UpdateLayout()
        {
            if (BehaviorService == null!)
            {
                return;
            }

            var pt = BehaviorService.ControlToAdornerWindow (Control);

            // calculate toolbar size
            var width = 0;
            var height = 0;

            foreach (var button in _buttons)
            {
                var size = button.Size;

                height = Math.Max (height, size.Height);
                width += size.Width + 1;
            }

            Size = new Size (width - 1, height) + Padding + Padding + new Size (2, 2);
            _bounds = new Rectangle (pt.X + Location.X, pt.Y + Location.Y, Size.Width, Size.Height);

            // update button locations
            var x = pt.X + Location.X + Padding.Width + 1;
            var y = pt.Y + Location.Y + Padding.Height + 1;

            foreach (var button in _buttons)
            {
                var size = button.Size;

                button.Bounds = new Rectangle (x, y, size.Width, height);
                x += size.Width + 1;
            }
        }

        /// <summary>
        /// Refreshes the toolbar.
        /// </summary>
        public void Refresh()
        {
            if (Visible && Adorner != null!)
            {
                Adorner.Invalidate();
            }
        }

        #endregion

        #region Overriden Methods

        /// <summary>
        /// Determines whether the given point is over a toolbar button.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public override Cursor? GetHitTest (Point p)
        {
            var insideToolbar = _bounds.Contains (p);
            var needsPaint = false;
            var hasHit = false;
            ButtonGlyph? currentHotButton = null;

            foreach (var button in _buttons.OfType<ButtonGlyph>())
            {
                if (button is { Enabled: false, IsHot: true })
                {
                    button.IsHot = false;
                    needsPaint = true;
                }
                else if (button.Enabled)
                {
                    var newIsHot = button.Bounds.Contains (p);

                    if (newIsHot)
                    {
                        hasHit = true;
                        currentHotButton = button;
                    }

                    if (button.IsHot != newIsHot)
                    {
                        button.IsHot = newIsHot;
                        needsPaint = true;
                    }
                }
            }

            if (currentHotButton != _lastHotButton)
            {
                if (_lastHotButton is { ShowToolTip: true })
                {
                    _lastHotButton.ShowToolTip = false;
                    needsPaint = true;
                }

                _tooltipTimer.Stop();

                if (currentHotButton != null)
                {
                    _tooltipTimer.Start();
                }

                _lastHotButton = currentHotButton!;
            }

            if (needsPaint)
            {
                Refresh();
            }

            return insideToolbar || hasHit ? Cursors.Default : null;
        }

        /// <summary>
        /// Paints the toolbar.
        /// </summary>
        /// <param name="pe">Paint event arguments.</param>
        public override void Paint (PaintEventArgs pe)
        {
            UpdateLayout();
            var bounds = Bounds;

            if (!pe.ClipRectangle.IntersectsWith (bounds))
            {
                return;
            }

            using (Brush brush = new SolidBrush (BackColor))
            using (var pen = new Pen (BorderColor))
            {
                pe.Graphics.FillRectangle (brush, bounds);
                pe.Graphics.DrawRectangle (pen, bounds);
            }

            foreach (var button in _buttons)
            {
                button.Paint (pe);
            }

            if (_lastHotButton is { ShowToolTip: true })
            {
                using (Brush backBrush = new SolidBrush (HotButtonBackColor))
                using (var borderPen = new Pen (HotButtonBorderColor))
                {
                    var toolTipSize = TextRenderer.MeasureText (_lastHotButton.ToolTipText, Control.Font);

                    var toolTipBounds = new Rectangle (_lastHotButton.Bounds.Left,
                        _lastHotButton.Bounds.Top - toolTipSize.Height - 2 * Padding.Width - 2 + 1,
                        toolTipSize.Width + 2 * Padding.Width + 2, toolTipSize.Height + 2 * Padding.Height + 2);

                    pe.Graphics.FillRectangle (backBrush, toolTipBounds);
                    pe.Graphics.DrawLines (borderPen, new Point[]
                    {
                        new (toolTipBounds.Left, toolTipBounds.Bottom),
                        new (toolTipBounds.Left, toolTipBounds.Top),
                        new (toolTipBounds.Right, toolTipBounds.Top),
                        new (toolTipBounds.Right, toolTipBounds.Bottom),
                        new (_lastHotButton.Bounds.Right, toolTipBounds.Bottom)
                    });

                    TextRenderer.DrawText (pe.Graphics, _lastHotButton.ToolTipText, Control.Font, toolTipBounds,
                        ButtonForeColor,
                        TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter |
                        TextFormatFlags.SingleLine);
                }
            }
        }

        #endregion

        #region IDisposable

        /// <summary>
        ///
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose (bool disposing)
        {
            if (disposing)
            {
                _tooltipTimer.Dispose();
            }
        }

        /// <summary>
        ///
        /// </summary>
        public void Dispose()
        {
            Dispose (true);
            GC.SuppressFinalize (this);
        }

        #endregion
    }
}
