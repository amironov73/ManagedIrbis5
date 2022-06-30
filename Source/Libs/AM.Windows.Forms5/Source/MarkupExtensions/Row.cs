// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* Row.cs -- строка контролов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms.MarkupExtensions;

/// <summary>
/// Строка контролов для метода <see cref="ControlExtensions.Pack{TControl}"/>.
/// </summary>
public sealed class Row
    : Control,
    IEnumerable<Control>
{
    #region Nested classes

    /// <summary>
    /// Элемент строки - Контрол и его ширина.
    /// </summary>
    public sealed class Item
    {
        #region Properties

        /// <summary>
        /// Собственно контрол.
        /// </summary>
        public Control Control { get; }

        /// <summary>
        /// Тип ширины.
        /// </summary>
        public SizeType SizeType { get; }

        /// <summary>
        /// Числовое значение ширины.
        /// </summary>
        public float Width { get;  }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public Item
            (
                Control control,
                float width
            )
        {
            Sure.NotNull (control);

            Control = control;
            Width = width;

            switch (width)
            {
                case > 0f:
                    SizeType = SizeType.Absolute;
                    break;

                case < 0f:
                    SizeType = SizeType.Percent;
                    Width = -width;
                    break;

                default:
                    SizeType = SizeType.AutoSize;
                    break;
            }
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        public Item
            (
                Control control,
                SizeType sizeType,
                float width
            )
        {
            Sure.NotNull (control);

            Control = control;
            Width = width;
            SizeType = sizeType;
        }

        #endregion
    }

    #endregion

    #region Properties

    /// <summary>
    /// Контролы, составляющие строку.
    /// </summary>
    public List<Item> Items { get; } = new ();

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public Row
        (
            params Control[] items
        )
    {
        Sure.NotNull (items);

        foreach (var item in items)
        {
            Items.Add (new Item (item, 0f));
        }
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Добавление элемента.
    /// </summary>
    public Row Add
        (
            Item item
        )
    {
        Sure.NotNull (item);

        Items.Add (item);

        return this;
    }

    /// <summary>
    /// Добавление элемента.
    /// </summary>
    public Row Add
        (
            Control control
        )
    {
        Sure.NotNull (control);

        Items.Add (new Item (control, 0f));

        return this;
    }

    /// <summary>
    /// Добавление элемента.
    /// </summary>
    public Row Add
        (
            float width,
            Control control
        )
    {
        Sure.NotNull (control);

        Items.Add (new Item (control, width));

        return this;
    }

    /// <summary>
    /// Добавление элемента.
    /// </summary>
    public Row Add
        (
            SizeType sizeType,
            float width,
            Control control
        )
    {
        Sure.NotNull (control);

        Items.Add (new Item (control, width));

        return this;
    }

    /// <summary>
    /// Преобразование в <see cref="TableLayoutPanel"/>.
    /// </summary>
    public TableLayoutPanel ToTableLayoutPanel()
    {
        var result = new TableLayoutPanel
        {
            RowCount = 1,
            ColumnCount = Items.Count,
            AutoSize = true
        };

        result.RowStyles.Add (new RowStyle (SizeType.AutoSize));

        var index = 0;
        foreach (var item in Items)
        {
            var control = item.Control;
            result.Controls.Add (control);
            result.SetColumn (control, index);
            result.SetRow (control, 0);
            result.ColumnStyles.Add (new ColumnStyle (item.SizeType, item.Width));

            ++index;
        }

        return result;
    }

    #endregion

    #region IEnumerable<T> members

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    /// <inheritdoc cref="IEnumerable{T}.GetEnumerator"/>
    public IEnumerator<Control> GetEnumerator()
    {
        return Controls.Cast<Control>().GetEnumerator();
    }

    #endregion
}
