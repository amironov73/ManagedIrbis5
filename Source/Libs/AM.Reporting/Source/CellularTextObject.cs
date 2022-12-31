// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/*
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Drawing;
using System.ComponentModel;

using AM.Reporting.Utils;
using AM.Reporting.Table;

#endregion

#nullable enable

namespace AM.Reporting
{
    /// <summary>
    /// Represents a text object which draws each symbol of text in its own cell.
    /// </summary>
    /// <remarks>
    /// <para/>The text may be aligned to left or right side, or centered. Use the <see cref="HorzAlign"/>
    /// property to do this. The "justify" align is not supported now, as well as vertical alignment.
    /// <para/>The cell size is defined in the <see cref="CellWidth"/> and <see cref="CellHeight"/> properties.
    /// These properties are 0 by default, in this case the size of cell is calculated automatically based
    /// on the object's <b>Font</b>.
    /// <para/>To define a spacing (gap) between cells, use the <see cref="HorzSpacing"/> and
    /// <see cref="VertSpacing"/> properties.
    /// </remarks>
    public partial class CellularTextObject : TextObject
    {
        #region Fields

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the width of cell, in pixels.
        /// </summary>
        /// <remarks>
        /// If zero width and/or height specified, the object will calculate the cell size
        /// automatically based on its font.
        /// </remarks>
        [Category ("Appearance")]
        [TypeConverter ("AM.Reporting.TypeConverters.UnitsConverter, AM.Reporting")]
        public float CellWidth { get; set; }

        /// <summary>
        /// Gets or sets the height of cell, in pixels.
        /// </summary>
        /// <remarks>
        /// If zero width and/or height specified, the object will calculate the cell size
        /// automatically based on its font.
        /// </remarks>
        [Category ("Appearance")]
        [TypeConverter ("AM.Reporting.TypeConverters.UnitsConverter, AM.Reporting")]
        public float CellHeight { get; set; }

        /// <summary>
        /// Gets or sets the horizontal spacing between cells, in pixels.
        /// </summary>
        [Category ("Appearance")]
        [TypeConverter ("AM.Reporting.TypeConverters.UnitsConverter, AM.Reporting")]
        public float HorzSpacing { get; set; }

        /// <summary>
        /// Gets or sets the vertical spacing between cells, in pixels.
        /// </summary>
        [Category ("Appearance")]
        [TypeConverter ("AM.Reporting.TypeConverters.UnitsConverter, AM.Reporting")]
        public float VertSpacing { get; set; }

        #endregion

        #region Private Methods

        // use the TableObject to represent the contents. It's easier to export it later.
        private TableObject GetTable (bool autoRows)
        {
            var table = new TableObject();
            table.SetPrinting (IsPrinting);
            table.SetReport (Report);

            var cellWidth = CellWidth;
            var cellHeight = CellHeight;

            // calculate cellWidth, cellHeight automatically
            if (cellWidth == 0 || cellHeight == 0)
            {
                var fontHeight = Font.GetHeight() * 96f / DrawUtils.ScreenDpi;
                cellWidth = GetCellWidthInternal (fontHeight);
                cellHeight = cellWidth;
            }

            var colCount = (int)((Width + HorzSpacing + 1) / (cellWidth + HorzSpacing));
            if (colCount == 0)
            {
                colCount = 1;
            }

            var rowCount = (int)((Height + VertSpacing + 1) / (cellHeight + VertSpacing));
            if (rowCount == 0 || autoRows)
            {
                rowCount = 1;
            }

            table.ColumnCount = colCount;
            table.RowCount = rowCount;

            // process the text
            var row = 0;
            var lineBegin = 0;
            var lastSpace = 0;
            var text = Text.Replace ("\r\n", "\n");

            for (var i = 0; i < text.Length; i++)
            {
                var isCRLF = text[i] == '\n';
                if (text[i] == ' ' || isCRLF)
                {
                    lastSpace = i;
                }

                if (i - lineBegin + 1 > colCount || isCRLF)
                {
                    if (WordWrap && lastSpace > lineBegin)
                    {
                        AddText (table, row, text.Substring (lineBegin, lastSpace - lineBegin));
                        lineBegin = lastSpace + 1;
                    }
                    else if (i - lineBegin > 0)
                    {
                        AddText (table, row, text.Substring (lineBegin, i - lineBegin));
                        lineBegin = i;
                    }
                    else
                    {
                        lineBegin = i + 1;
                    }

                    lastSpace = lineBegin;
                    row++;
                    if (autoRows && row >= rowCount)
                    {
                        rowCount++;
                        table.RowCount++;
                    }
                }
            }

            // finish the last line
            if (lineBegin < text.Length)
            {
                AddText (table, row, text.Substring (lineBegin, text.Length - lineBegin));
            }

            // set up cells appearance
            for (var i = 0; i < colCount; i++)
            {
                for (var j = 0; j < rowCount; j++)
                {
                    var cell = table[i, j];
                    cell.Border = Border.Clone();
                    cell.Fill = Fill.Clone();
                    cell.Font = Font;
                    cell.TextFill = TextFill.Clone();
                    cell.HorzAlign = HorzAlign.Center;
                    cell.VertAlign = VertAlign.Center;
                }
            }

            // set cell's width and height
            for (var i = 0; i < colCount; i++)
            {
                table.Columns[i].Width = cellWidth;
            }

            for (var i = 0; i < rowCount; i++)
            {
                table.Rows[i].Height = cellHeight;
            }

            // insert spacing between cells
            if (HorzSpacing > 0)
            {
                for (var i = 0; i < colCount - 1; i++)
                {
                    var newColumn = new TableColumn();
                    newColumn.Width = HorzSpacing;
                    table.Columns.Insert (i * 2 + 1, newColumn);
                }
            }

            if (VertSpacing > 0)
            {
                for (var i = 0; i < rowCount - 1; i++)
                {
                    var newRow = new TableRow();
                    newRow.Height = VertSpacing;
                    table.Rows.Insert (i * 2 + 1, newRow);
                }
            }

            table.Left = AbsLeft;
            table.Top = AbsTop;
            table.Width = table.Columns[table.ColumnCount - 1].Right;
            table.Height = table.Rows[table.RowCount - 1].Bottom;
            return table;
        }

        private void AddText (TableObject table, int row, string text)
        {
            if (row >= table.RowCount)
            {
                return;
            }

            text = text.TrimEnd (' ');
            if (text.Length > table.ColumnCount)
            {
                text = text.Substring (0, table.ColumnCount);
            }

            var offset = 0;
            if (HorzAlign == HorzAlign.Right)
            {
                offset = table.ColumnCount - text.Length;
            }
            else if (HorzAlign == HorzAlign.Center)
            {
                offset = (table.ColumnCount - text.Length) / 2;
            }

            for (var i = 0; i < text.Length; i++)
            {
                table[i + offset, row].Text = text[i].ToString();
            }
        }

        #endregion

        #region Public Methods

        /// <inheritdoc/>
        public override void Assign (Base source)
        {
            base.Assign (source);

            var src = source as CellularTextObject;
            CellWidth = src.CellWidth;
            CellHeight = src.CellHeight;
            HorzSpacing = src.HorzSpacing;
            VertSpacing = src.VertSpacing;
        }

        /// <inheritdoc/>
        public override void Serialize (FRWriter writer)
        {
            var c = writer.DiffObject as CellularTextObject;
            base.Serialize (writer);

            if (FloatDiff (CellWidth, c.CellWidth))
            {
                writer.WriteFloat ("CellWidth", CellWidth);
            }

            if (FloatDiff (CellHeight, c.CellHeight))
            {
                writer.WriteFloat ("CellHeight", CellHeight);
            }

            if (FloatDiff (HorzSpacing, c.HorzSpacing))
            {
                writer.WriteFloat ("HorzSpacing", HorzSpacing);
            }

            if (FloatDiff (VertSpacing, c.VertSpacing))
            {
                writer.WriteFloat ("VertSpacing", VertSpacing);
            }
        }

        /// <inheritdoc/>
        public override void Draw (FRPaintEventArgs eventArgs)
        {
            using (var table = GetTable())
            {
                table.Draw (eventArgs);
            }
        }

        public TableObject GetTable()
        {
            return GetTable (false);
        }

        #endregion

        #region Report Engine

        /// <inheritdoc/>
        public override float CalcHeight()
        {
            using (var table = GetTable (true))
            {
                return table.Height;
            }
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="CellularTextObject"/> class with the default settings.
        /// </summary>
        public CellularTextObject()
        {
            CanBreak = false;
            Border.Lines = BorderLines.All;
        }
    }
}
