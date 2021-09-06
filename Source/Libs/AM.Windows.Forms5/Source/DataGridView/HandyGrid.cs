// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global

/* HandyGrid.cs -- адаптированный DataGridView
   Ars Magna project, http://library.istu.edu/am */

#region Using directives

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;

using AM.Data;

#endregion

#nullable enable

namespace AM.Windows.Forms
{
    /// <summary>
    /// Адаптированный DataGridView.
    /// </summary>
    public sealed class HandyGrid
        : DataGridView
    {
        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public HandyGrid()
        {
            AutoGenerateColumns = true;
            EnableHeadersVisualStyles = false;
            RowHeadersVisible = false;
            ScrollBars = ScrollBars.Vertical;
            ShowEditingIcon = false;
            SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            AllowUserToAddRows = false;
            AllowUserToDeleteRows = false;
            AllowUserToResizeRows = false;
            AllowUserToOrderColumns = false;
            AllowUserToResizeColumns = false;
            ReadOnly = true;
        } // constructor

        #endregion

        #region Private members

        private Type? _firstType;

        #endregion

        #region Public methods

        /// <summary>
        /// Заполнение данными.
        /// </summary>
        public void SetDataObject
            (
                object? obj
            )
        {
            if (obj is null)
            {
                AutoGenerateColumns = false;
                DataSource = obj;
                return;
            }

            var type = obj.GetType();
            if (type.IsAssignableTo(typeof(BindingSource)))
            {
                var bindingSource = (BindingSource)obj;
                type = bindingSource.DataSource.GetType();
            }

            if (type.IsGenericType)
            {
                type = type.GetGenericArguments()[0];
            }
            else if (type.IsArray)
            {
                type = type.GetElementType();
            }
            else
            {
                // мы не умеем обрабатывать такие типы
                return;
            }

            if (type is null)
            {
                return;
            }

            if (_firstType == type)
            {
                DataSource = null;
                DataSource = obj;
                return;
            }

            _firstType = type;
            DataSource = null;
            AutoGenerateColumns = true;
            DataSource = obj;
            AutoGenerateColumns = false;
            var list = new List<KeyValuePair<int, DataGridViewColumn>>();
            var properties = type.GetProperties
                (
                    BindingFlags.Public | BindingFlags.Instance
                );

            foreach (var pinfo in properties)
            {
                var column = Columns[pinfo.Name];
                if (column is null)
                {
                    continue;
                }

                var hca = (HiddenColumnAttribute?) Attribute.GetCustomAttribute
                    (
                        pinfo,
                        typeof(HiddenColumnAttribute)
                    );
                if (hca is not null && hca.Hidden)
                {
                    Columns.Remove(column);
                    continue;
                }

                var roca = (ReadOnlyColumnAttribute?) Attribute.GetCustomAttribute
                    (
                        pinfo,
                        typeof(ReadOnlyColumnAttribute)
                    );
                if (roca is not null)
                {
                    column.ReadOnly = roca.ReadOnly;
                }

                var cha = (ColumnHeaderAttribute?) Attribute.GetCustomAttribute
                    (
                        pinfo,
                        typeof(ColumnHeaderAttribute)
                    );
                if (cha is not null)
                {
                    column.HeaderText = cha.Header;
                }

                var cwa = (ColumnWidthAttribute?) Attribute.GetCustomAttribute
                    (
                        pinfo,
                        typeof(ColumnWidthAttribute)
                    );
                if (cwa is not null)
                {
                    column.FillWeight = cwa.Width;
                }

                var cia = (ColumnIndexAttribute?) Attribute.GetCustomAttribute
                    (
                        pinfo,
                        typeof(ColumnIndexAttribute)
                    );
                var index = cia?.Index ?? 0;

                var fca = (FixedColumnAttribute?) Attribute.GetCustomAttribute
                    (
                        pinfo,
                        typeof(FixedColumnAttribute)
                    );
                if (fca is not null)
                {
                    column.Frozen = fca.Fixed;
                }

                list.Add
                    (
                        new KeyValuePair<int, DataGridViewColumn> (index, column)
                    );
            }

            var min = int.MaxValue;
            var max = int.MinValue;
            foreach (var pair in list)
            {
                min = Math.Min(min, pair.Key);
                max = Math.Max(max, pair.Key);
            }

            if (min != max)
            {
                list.Sort(new PairComparer());
                Columns.Clear();
                foreach (var pair in list)
                {
                    var prev = pair.Value;
                    var column = new DataGridViewColumn(prev.CellTemplate)
                    {
                        FillWeight = prev.FillWeight,
                        HeaderText = prev.HeaderText,
                        DataPropertyName = prev.DataPropertyName
                    };
                    Columns.Add(column);
                }
            }

            foreach (DataGridViewColumn column in Columns)
            {
                column.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            }

            AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

        } //method SetObject

        #endregion
    } // class HandyGrid

    class PairComparer
        : IComparer<KeyValuePair<int, DataGridViewColumn>>
    {
        #region IComparer<KeyValuePair<int,DataGridViewColumn>> members

        public int Compare(KeyValuePair<int, DataGridViewColumn> x, KeyValuePair<int, DataGridViewColumn> y)
        {
            return (x.Key - y.Key);
        }

        #endregion
    } // class PairComparer
} // namespace AM.Windows.Forms
