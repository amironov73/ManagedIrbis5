// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassWithVirtualMembersNeverInherited.Global
// ReSharper disable CommentTypo
// ReSharper disable EventNeverSubscribedTo.Global
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* SiberianRow.cs -- строка грида
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using AM.Collections;

#endregion

#nullable enable

namespace ManagedIrbis.WinForms.Grid
{
    /// <summary>
    /// Строка грида. Состоит из ячеек.
    /// </summary>
    public class SiberianRow
    {
        #region Constants

        /// <summary>
        /// Высота строки по умолчанию, пикселы.
        /// </summary>
        public const int DefaultHeight = 20;

        #endregion

        #region Events

        /// <summary>
        /// Событие, возникающее при щелчке по строке.
        /// </summary>
        public event EventHandler<SiberianClickEventArgs>? Click;

        #endregion

        #region Properties

        /// <summary>
        /// Порядковый номер.
        /// </summary>
        public int Index { get; internal set; }

        /// <summary>
        /// Данные.
        /// </summary>
        public object? Data { get; set; }

        /// <summary>
        /// Грид, которому принадлежит строка.
        /// </summary>
        public SiberianGrid? Grid { get; internal set; }

        /// <summary>
        /// Ячейки, входящие в строку.
        /// </summary>
        public NonNullCollection<SiberianCell> Cells { get; private set; }

        /// <summary>
        /// Актальная высота строки.
        /// </summary>
        [DefaultValue(DefaultHeight)]
        public int Height { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        internal SiberianRow()
        {
            Height = DefaultHeight;
            Cells = new NonNullCollection<SiberianCell>();

        } // constructor

        #endregion

        #region Private members

        /// <summary>
        /// Handle <see cref="Click"/> event.
        /// </summary>
        protected internal void HandleClick ( SiberianClickEventArgs eventArgs ) =>
            Click?.Invoke(this, eventArgs);

        #endregion

        #region Public methods

        /// <summary>
        /// Get data from the bound object
        /// and put it to the cells.
        /// </summary>
        public virtual void GetData()
        {
            foreach (var cell in Cells)
            {
                cell.Column?.GetData ( Data, cell );
            }
        }

        /// <summary>
        /// Get first editable cell in the row.
        /// </summary>
        public SiberianCell? GetFirstEditableCell()
        {
            foreach (var cell in Cells)
            {
                if (!(cell.Column?.ReadOnly ?? true))
                {
                    return cell;
                }
            }

            return null;
        }

        /// <summary>
        /// Measure cells.
        /// </summary>
        public virtual void MeasureCells()
        {
            var height = Height;

            foreach (var cell in Cells)
            {
                var dimensions = new SiberianDimensions
                {
                    Width = cell.Column?.Width ?? 0,
                    Height = height
                };

                cell.MeasureCell(dimensions);
            }

            if (height > Height)
            {
                Height = height;
            }
        }

        /// <summary>
        /// Handles click on the row.
        /// </summary>
        public virtual void OnClick ( SiberianClickEventArgs eventArgs ) =>
            Click?.Invoke(this, eventArgs);

        /// <summary>
        /// Draw the column.
        /// </summary>
        public virtual void Paint
            (
                PaintEventArgs args
            )
        {
            var graphics = args.Graphics;
            var clip = args.ClipRectangle;

            // TODO: разобраться, почему не перерисовывается с белым фоном
            if (ReferenceEquals(this, Grid?.CurrentRow))
            {
                var backColor = Color.DarkBlue;
                using var brush = new SolidBrush(backColor);
                graphics.FillRectangle(brush, clip);
            }
        }

        /// <summary>
        /// Get data from the cells
        /// and put it to the bound object.
        /// </summary>
        public virtual void PutData()
        {
            foreach (var cell in Cells)
            {
                cell.Column?.PutData
                    (
                        Data,
                        cell
                    );
            }

            Grid?.Invalidate();
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString() => $"Index: {Index}, Data: {Data}";

        #endregion

    } // class SiberianRow

} // namespace ManagedIrbis.WinForms.Grid
