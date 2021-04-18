// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* SiberianColumn.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#endregion

#nullable enable

namespace ManagedIrbis.WinForms.Grid
{
    /// <summary>
    ///
    /// </summary>
    public abstract class SiberianColumn
    {
        #region Constants

        /// <summary>
        /// Default fill width.
        /// </summary>
        public const int DefaultFillWidth = 0;

        /// <summary>
        /// Default width.
        /// </summary>
        public const int DefaultWidth = 200;

        /// <summary>
        /// Default minimal width.
        /// </summary>
        public const int DefaultMinWidth = 100;

        #endregion

        #region Events

        /// <summary>
        /// Fired on click.
        /// </summary>
        public event EventHandler<SiberianClickEventArgs>? Click;

        #endregion

        #region Properties

        /// <summary>
        /// Column index.
        /// </summary>
        public int Index { get; internal set; }

        /// <summary>
        /// Data member (property name).
        /// </summary>
        public string? Member { get; set; }

        /// <summary>
        /// Read only column?
        /// </summary>
        [DefaultValue(false)]
        public bool ReadOnly { get; set; }

        /// <summary>
        /// Grid.
        /// </summary>
        public SiberianGrid? Grid { get; internal set; }

        /// <summary>
        /// Title.
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// Fill width.
        /// </summary>
        [DefaultValue(DefaultFillWidth)]
        public int FillWidth
        {
            get => _fillWidth;
            set
            {
                _fillWidth = value;
                Grid?.AutoSizeColumns();
            }
        }

        /// <summary>
        /// Width, pixels.
        /// </summary>
        [DefaultValue(DefaultWidth)]
        public int Width
        {
            get => _width;
            set
            {
                _width = Math.Max(value, MinWidth);
                Grid?.AutoSizeColumns();
            }
        }

        /// <summary>
        /// Minimal width of the column, pixels.
        /// </summary>
        [DefaultValue(DefaultMinWidth)]
        public int MinWidth { get; set; }

        /// <summary>
        /// Options.
        /// </summary>
        public SiberianOptions Options { get; set; }

        /// <summary>
        /// Palette.
        /// </summary>
        public SiberianPalette Palette { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        // ReSharper disable once NotNullMemberIsNotInitialized
        protected SiberianColumn()
        {
            FillWidth = DefaultFillWidth;
            Width = DefaultWidth;
            MinWidth = DefaultMinWidth;

            Palette = SiberianPalette.DefaultPalette.Clone();
        }

        #endregion

        #region Private members

        private int _fillWidth;

        private int _width;

        /// <summary>
        /// Handle <see cref="Click"/> event.
        /// </summary>
        protected internal void HandleClick
            (
                SiberianClickEventArgs eventArgs
            )
        {
            Click?.Invoke(this, eventArgs);
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Create cell.
        /// </summary>
        public abstract SiberianCell CreateCell();

        /// <summary>
        /// Create editor for the cell.
        /// </summary>
        public virtual Control? CreateEditor
            (
                SiberianCell cell,
                bool edit,
                object? state
            )
        {
            return null;
        }

        /// <summary>
        /// Get data from the object and put it to the cell.
        /// </summary>
        public virtual void GetData
            (
                object? theObject,
                SiberianCell cell
            )
        {
            // Nothing to do here
        }

        /// <summary>
        /// Draw the column.
        /// </summary>
        public virtual void Paint
            (
                PaintEventArgs args
            )
        {
            var graphics = args.Graphics;
            var rectangle = args.ClipRectangle;

            if (Palette.BackColor != Color.Transparent
                && Palette.BackColor != Color.Empty)
            {
                using Brush brush = new SolidBrush(Palette.BackColor);
                graphics.FillRectangle
                    (
                        brush,
                        rectangle
                    );
            }
        }

        /// <summary>
        /// Handles click on the column.
        /// </summary>
        public virtual void OnClick
            (
                SiberianClickEventArgs eventArgs
            )
        {
            Click?.Invoke(this, eventArgs);
        }

        /// <summary>
        /// Draw the column header.
        /// </summary>
        public virtual void PaintHeader
            (
                PaintEventArgs args
            )
        {
            var grid = Grid;
            if (grid is null)
            {
                // TODO: some paint?
                return;
            }

            var graphics = args.Graphics;
            var rectangle = args.ClipRectangle;

            using (var brush = new SolidBrush(Palette.HeaderBackColor))
            {
                graphics.FillRectangle(brush, rectangle);
            }

            using (var headerFont = new Font(grid.Font, FontStyle.Bold))
            {
                var flags = TextFormatFlags.Left
                    | TextFormatFlags.NoPrefix
                    | TextFormatFlags.VerticalCenter
                    | TextFormatFlags.EndEllipsis;

                rectangle.Inflate(-2, 0);

                TextRenderer.DrawText
                    (
                        graphics,
                        Title,
                        headerFont,
                        rectangle,
                        Palette.HeaderForeColor,
                        flags
                    );
            }
        }

        /// <summary>
        /// Get data from the cell and put it to the object.
        /// </summary>
        public virtual void PutData
            (
                object? theObject,
                SiberianCell cell
            )
        {
            // Nothing to do here
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString() => $"{GetType().Name}: {Title}";

        #endregion

    } // class SiberianColumn

} // namespace ManagedIrbis.WinForms.Grid
