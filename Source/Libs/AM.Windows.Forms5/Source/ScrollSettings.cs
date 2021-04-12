// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/*
 * ScrollSettings.cs
 */

#region Using directives

using System.ComponentModel;

#endregion

#nullable enable

namespace AM.Windows.Forms
{
    /// <summary>
    ///   Represents a scroll bar with own settings
    /// </summary>
    [TypeConverter(typeof (ExpandableObjectConverter))]
    public sealed class ScrollSettings
    {
        #region Constants

        public const bool DefaultEnabled = true;
        public const int DefaultLargeChange = 10;
        public const int DefaultMaximum = 100;
        public const int DefaultMinimum = 0;
        public const int DefaultPosition = 0;
        public const int DefaultSmallChange = 1;
        public const bool DefaultVisible = true;

        #endregion

        #region Construction

        /// <summary>
        ///   Initializes a new instance of the
        ///   <see cref = "ScrollSettings" /> class.
        /// </summary>
        /// <param name = "control">The control.</param>
        /// <param name = "kind">The kind.</param>
        internal ScrollSettings
            (
            SimpleScrollableControl control,
            NativeMethods.ScrollBarKind kind
            )
        {
            _control = control;
            _kind = kind;

            Minimum = DefaultMinimum;
            Maximum = DefaultMaximum;
            Position = DefaultPosition;
            LargeChange = DefaultLargeChange;
            SmallChange = DefaultSmallChange;
            Visible = DefaultVisible;
            Enabled = DefaultEnabled;
        }

        #endregion

        #region Private members

        private readonly SimpleScrollableControl _control;
        private bool _enabled;
        internal readonly NativeMethods.ScrollBarKind _kind;

        private bool _visible;

        private void _GetRange(out int min, out int max)
        {
            NativeMethods.GetScrollRange
                (
                    _control.Handle,
                    _kind,
                    out min,
                    out max
                );
        }

        private void _SetRange(int min, int max, bool redraw)
        {
            NativeMethods.SetScrollRange
                (
                    _control.Handle,
                    _kind,
                    min,
                    max,
                    redraw
                );
        }

        #endregion

        #region Properties

        /// <summary>
        ///   Gets or sets a value indicating whether this
        ///   <see cref = "ScrollSettings" /> is enabled.
        /// </summary>
        /// <value><c>true</c> if enabled; otherwise,
        ///   <c>false</c>.</value>
        [DefaultValue(DefaultEnabled)]
        public bool Enabled
        {
            get { return _enabled; }
            set
            {
                NativeMethods.EnableScrollBar
                    (
                        _control.Handle,
                        _kind,
                        value
                            ? NativeMethods.EnableScrollBarFlags.ESB_ENABLE_BOTH
                            : NativeMethods.EnableScrollBarFlags.ESB_DISABLE_BOTH
                    );
                _enabled = value;
            }
        }

        /// <summary>
        ///   Gets or sets the large change.
        /// </summary>
        /// <value>The large change.</value>
        [DefaultValue(DefaultLargeChange)]
        public int LargeChange { get; set; }

        /// <summary>
        ///   Gets or sets the maximum.
        /// </summary>
        /// <value>The maximum.</value>
        [DefaultValue(DefaultMaximum)]
        public int Maximum
        {
            get
            {
                int min, max;
                _GetRange(out min, out max);
                return max;
            }
            set
            {
                int min, max;
                _GetRange(out min, out max);
                _SetRange(min, value, true);
            }
        }

        /// <summary>
        ///   Gets or sets the minimum.
        /// </summary>
        /// <value>The minimum.</value>
        [DefaultValue(DefaultMinimum)]
        public int Minimum
        {
            get
            {
                int min, max;
                _GetRange(out min, out max);
                return min;
            }
            set
            {
                int min, max;
                _GetRange(out min, out max);
                _SetRange(value, max, true);
            }
        }

        /// <summary>
        ///   Gets or sets the position.
        /// </summary>
        /// <value>The position.</value>
        [DefaultValue(DefaultPosition)]
        public int Position
        {
            get
            {
                return NativeMethods.GetScrollPos
                    (
                        _control.Handle,
                        _kind
                    );
            }
            set
            {
                NativeMethods.SetScrollPos
                    (
                        _control.Handle,
                        _kind,
                        value,
                        true
                    );
                _control.Invalidate();
            }
        }

        /// <summary>
        ///   Gets the track position.
        /// </summary>
        /// <value>The track position.</value>
        [Browsable(false)]
        [DesignerSerializationVisibility
            (DesignerSerializationVisibility.Hidden)]
        public int TrackPosition
        {
            get
            {
                NativeMethods.SCROLLINFO info
                    = new NativeMethods.SCROLLINFO
                          {
                              cbSize = NativeMethods.SCROLLINFO.StructureSize,
                              fMask = NativeMethods.ScrollInfoFlags.SIF_TRACKPOS
                          };
                NativeMethods.GetScrollInfo
                    (
                        _control.Handle,
                        _kind,
                        ref info
                    );
                return info.nTrackPos;
            }
        }

        /// <summary>
        ///   Gets or sets the small change.
        /// </summary>
        /// <value>The small change.</value>
        [DefaultValue(DefaultSmallChange)]
        public int SmallChange { get; set; }

        /// <summary>
        ///   Gets or sets a value indicating whether this <see cref = "ScrollSettings" /> is visible.
        /// </summary>
        /// <value><c>true</c> if visible; otherwise, <c>false</c>.</value>
        [DefaultValue(DefaultVisible)]
        public bool Visible
        {
            get { return _visible; }
            set
            {
                NativeMethods.ShowScrollBar
                    (
                        _control.Handle,
                        _kind,
                        value
                    );
                _visible = value;
            }
        }

        #endregion

        #region Object members

        /// <summary>
        ///   Returns a <see cref = "System.String" /> that
        ///   represents this instance.
        /// </summary>
        /// <returns>
        ///   A <see cref = "System.String" /> that represents
        ///   this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format
                (
                    "Kind: {0}, Enabled: {1}, Visible: {2}, "
                    + "LargeChange: {3}, SmallChange: {4} "
                    + "Minimum: {5}, Maximum: {6}, Position: {7}",
                    _kind,
                    _enabled,
                    _visible,
                    LargeChange,
                    SmallChange,
                    Minimum,
                    Maximum,
                    Position
                );
        }

        #endregion
    }
}
