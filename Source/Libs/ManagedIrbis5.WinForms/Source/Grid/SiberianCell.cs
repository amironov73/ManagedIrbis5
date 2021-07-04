// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable EventNeverSubscribedTo.Global
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* SiberianCell.cs -- базовый класс для ячейки грида
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Windows.Forms;

using AM;

#endregion

#nullable enable

namespace ManagedIrbis.WinForms.Grid
{
    /// <summary>
    /// Базовый класс для ячейки грида.
    /// </summary>
    public class SiberianCell
    {
        #region Events

        /// <summary>
        /// Клик по ячейке (мышкой либо нажатие Enter на клавиатуре).
        /// </summary>
        public event EventHandler<SiberianClickEventArgs>? Click;

        /// <summary>
        /// Получение размеров содержимого ячейки.
        /// </summary>
        public event EventHandler<SiberianMeasureEventArgs>? Measure;

        /// <summary>
        /// Получение тултипа (опционального) для ячейки.
        /// </summary>
        public event EventHandler<SiberianToolTipEventArgs>? ToolTip;

        /// <summary>
        /// Отрисовка ячейки.
        /// </summary>
        public event EventHandler<PaintEventArgs>? Paint;

        #endregion

        #region Properties

        /// <summary>
        /// Колонка, которой принадлежит ячейка.
        /// </summary>
        public SiberianColumn? Column { get; internal set; }

        /// <summary>
        /// Грид, которому принадлежит ячейка.
        /// </summary>
        public SiberianGrid? Grid => Row?.Grid;

        /// <summary>
        /// Строка, которой принадлежит ячейка.
        /// </summary>
        public SiberianRow? Row { get; internal set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        protected SiberianCell()
        {
        }

        #endregion

        #region Private members

        /// <summary>
        /// Возбуждение события <see cref="Click"/>.
        /// </summary>
        protected internal virtual void HandleClick
            (
                SiberianClickEventArgs eventArgs
            )
        {
            Click.Raise(this, eventArgs);

        } // method HandleClick

        /// <summary>
        /// Возбуждение события <see cref="Measure"/>.
        /// </summary>
        protected internal virtual void HandleMeasure
            (
                SiberianMeasureEventArgs eventArgs
            )
        {
            Measure.Raise(this, eventArgs);

        } // method HandleMeasure

        /// <summary>
        /// Возбуждение события <see cref="Paint"/>.
        /// </summary>
        protected internal virtual void HandlePaint
            (
                PaintEventArgs eventArgs
            )
        {
            Paint.Raise(this, eventArgs);

        } // method HandlePaint

        /// <summary>
        /// Получение тултипа для ячейки.
        /// </summary>
        protected internal virtual void HandleToolTip
            (
                SiberianToolTipEventArgs eventArgs
            )
        {
            ToolTip.Raise(this, eventArgs);

        } // method HandleToolTip

        /// <summary>
        /// Измерение размера ячейки.
        /// </summary>
        protected internal virtual void MeasureCell
            (
                SiberianDimensions dimensions
            )
        {
            HandleMeasure(new SiberianMeasureEventArgs(dimensions));

        } // method MeasureCell

        #endregion

        #region Public methods

        /// <summary>
        /// Закрытие редактора (возможно, отсутствующего).
        /// </summary>
        /// <param name="accept">Принимаются ли введенные данные.
        /// Если нет, то они просто отбрасываются, иначе
        /// они становятся новым значением ячейки.</param>
        public virtual void CloseEditor
            (
                bool accept
            )
        {
            var grid = this.EnsureGrid();

            // в базовом классе мы никак не отрабатываем
            // параметр accept
            if (grid.Editor is { } editor)
            {
                editor.Dispose();
                grid.Editor = null;

                grid.Invalidate();
            }

        } // method CloseEditor

        /// <summary>
        /// Обработка клика по ячейке.
        /// </summary>
        public virtual void OnClick
            (
                SiberianClickEventArgs eventArgs
            )
        {
            HandleClick(eventArgs);

        } // method OnClick

        /// <summary>
        /// Отрисовка ячейки.
        /// </summary>
        public virtual void OnPaint
            (
                PaintEventArgs args
            )
        {
            HandlePaint(args);

        } // method OnPaint

        #endregion

    } // class SiberianCell

} // namespace ManagedIrbis.WinForms.Grid
