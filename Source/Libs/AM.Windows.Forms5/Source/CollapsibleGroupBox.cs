// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable EventNeverSubscribedTo.Global
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedType.Global

/* CollapsibleGroupBox.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms
{
    /// <summary>
    /// <see cref="GroupBox"/> that can be collapsed.
    /// </summary>
    [System.ComponentModel.DesignerCategory("Code")]
    public class CollapsibleGroupBox
        : GroupBox
    {
        #region Events

        /// <summary>
        /// Raised when collapse action performed.
        /// </summary>
        public event EventHandler? Collapse;

        #endregion

        #region Properties

        private bool _collapsed;

        /// <summary>
        /// Gets or sets a value indicating whether this
        /// <see cref="CollapsibleGroupBox"/> is collapsed.
        /// </summary>
        /// <value><c>true</c> if collapsed;
        /// otherwise, <c>false</c>.</value>
        [System.ComponentModel.DefaultValue(false)]
        public bool Collapsed
        {
            get => _collapsed;
            set
            {
                if (_collapsed != value)
                {
                    _Collapse(value);
                }
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="CollapsibleGroupBox"/> class.
        /// </summary>
        public CollapsibleGroupBox()
        {
            _savedState = new Dictionary<Control, bool>();
            MouseDoubleClick += _MouseDoubleClick;
        }

        #endregion

        #region Private members

        private int _savedHeight;
        private readonly Dictionary<Control, bool> _savedState;

        private void _Collapse(bool coll)
        {
            if (coll)
            {
                _savedState.Clear();
                _savedHeight = Height;
                Height = FontHeight + 2;
            }
            else
            {
                Height = _savedHeight;
            }

            foreach (Control control in Controls)
            {
                if (coll)
                {
                    _savedState.Add(control, control.Enabled);
                    control.Enabled = false;
                }
                else
                {
                    if (_savedState.ContainsKey(control))
                    {
                        control.Enabled = _savedState[control];
                    }
                    else
                    {
                        control.Enabled = true;
                    }
                }
            }
            _collapsed = coll;

            Collapse?.Invoke(this, EventArgs.Empty);
        }

        private void _MouseDoubleClick
            (
                object? sender,
                MouseEventArgs e
            )
        {
            if (e.Y < FontHeight)
            {
                _Collapse(!Collapsed);
            }
        }

        #endregion

        #region Control members

        /// <summary>
        /// Paints the background of the control.
        /// </summary>
        protected override void OnPaintBackground
            (
                PaintEventArgs paintEvent
            )
        {
            base.OnPaintBackground(paintEvent);
            var g = paintEvent.Graphics;
            var rectangle = ClientRectangle;
            rectangle.Height = FontHeight + 2;
            using var b = new LinearGradientBrush
                (
                    rectangle,
                    SystemColors.ControlDark,
                    SystemColors.ControlLight,
                    0f
                );
            g.FillRectangle(b, rectangle);
        }

        #endregion
    }
}
