// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

/* DataGridViewUtility.cs -- полезные методы для DataGridView
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Windows.Forms;

using AM.Data;
using AM.Xml;

#endregion

#nullable enable

namespace AM.Windows.Forms
{
    /// <summary>
    /// Полезные методы для <see cref="DataGridView"/>
    /// </summary>
    public static class DataGridViewUtility
    {
        #region Public methods

        /// <summary>
        /// Apply specified columns to the <see cref="DataGridView"/>.
        /// </summary>
        public static void ApplyColumns
            (
                DataGridView grid,
                IEnumerable<DataColumnInfo> columns
            )
        {
            try
            {
                grid.SuspendLayout();

                grid.AutoGenerateColumns = false;
                grid.Columns.Clear();

                foreach (var info in columns)
                {
                    var column = info.ToGridColumn();
                    grid.Columns.Add(column);
                }
            }
            finally
            {
                grid.ResumeLayout();
            }

        } // method ApplyColumns

        /// <summary>
        /// Converts the column description into
        /// <see cref="DataGridViewColumn"/>
        /// </summary>
        public static DataGridViewColumn ToGridColumn
            (
                this DataColumnInfo column
            )
        {
            var columnTypeName = column.Type;
            Type? columnType = null;
            if (!string.IsNullOrEmpty (columnTypeName))
            {
                columnType = Type.GetType (columnTypeName, true);
            }

            var result = columnType is null
                ? new DataGridViewTextBoxColumn()
                : (DataGridViewColumn) Activator.CreateInstance (columnType).ThrowIfNull();

            result.DataPropertyName = column.Name;
            result.HeaderText = column.Title;
            result.DataPropertyName = column.Name;
            result.ReadOnly = column.ReadOnly;
            result.Frozen = column.Frozen;
            result.Visible = !column.Invisible;
            if (column.Width != 0)
            {
                if (column.Width > 0)
                {
                    result.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                    result.Width = column.Width;
                }
                else
                {
                    result.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    result.FillWeight = Math.Abs(column.Width);
                }
            }

            result.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            return result;

        } // method ToGridColumn

        /// <summary>
        /// Gets the columns.
        /// </summary>
        public static List<DataGridViewColumn> GetColumns
            (
                string xmlText
            )
        {
            var info = XmlUtility.DeserializeString<DataSetInfo> (xmlText);

            var result = new List<DataGridViewColumn>();
            foreach (var column in info.Tables[0].Columns)
            {
                result.Add(column.ToGridColumn());
            }

            return result;

        } // method GetColumns

        ///// <summary>
        ///// Generates the table HTML.
        ///// </summary>
        ///// <param name="grid">The grid.</param>
        ///// <param name="title">The title.</param>
        ///// <returns></returns>
        //public static string GenerateTableHtml
        //    (
        //        DataGridView grid,
        //        string title
        //    )
        //{
        //    ArgumentUtility.NotNull(grid, "grid");
        //    ArgumentUtility.NotNull(title, "title");

        //    StringWriter writer = new StringWriter();
        //    Html32TextWriter html = new Html32TextWriter(writer);

        //    html.RenderBeginTag(HtmlTextWriterTag.Html);
        //    html.RenderBeginTag(HtmlTextWriterTag.Head);
        //    html.RenderBeginTag(HtmlTextWriterTag.Title);
        //    html.WriteEncodedText(title);
        //    html.RenderEndTag(); // title
        //    html.RenderEndTag(); // head
        //    html.RenderBeginTag(HtmlTextWriterTag.Body);
        //    html.AddAttribute(HtmlTextWriterAttribute.Width, "90%");
        //    html.AddAttribute(HtmlTextWriterAttribute.Border, "1");
        //    html.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "0");
        //    html.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "3");
        //    html.AddAttribute(HtmlTextWriterAttribute.Bordercolor, "black");
        //    html.AddAttribute(HtmlTextWriterAttribute.Align, "center");
        //    html.RenderBeginTag(HtmlTextWriterTag.Table);

        //    html.RenderBeginTag(HtmlTextWriterTag.Tr);
        //    foreach (DataGridViewColumn column in grid.Columns)
        //    {
        //        if (column.Visible)
        //        {
        //            html.RenderBeginTag(HtmlTextWriterTag.Td);
        //            html.RenderBeginTag(HtmlTextWriterTag.B);
        //            html.WriteEncodedText(column.HeaderText);
        //            html.RenderEndTag();
        //            html.RenderEndTag(); // td
        //        }
        //    }
        //    html.RenderEndTag(); // tr

        //    foreach (DataGridViewRow row in grid.Rows)
        //    {
        //        html.RenderBeginTag(HtmlTextWriterTag.Tr);
        //        foreach (DataGridViewCell cell in row.Cells)
        //        {
        //            if (cell.OwningColumn.Visible)
        //            {
        //                html.RenderBeginTag(HtmlTextWriterTag.Td);
        //                object formattedValue = cell.FormattedValue;
        //                bool written = false;
        //                if (formattedValue != null)
        //                {
        //                    string text = formattedValue.ToString();
        //                    if (!string.IsNullOrEmpty(text))
        //                    {
        //                        html.WriteEncodedText(formattedValue.ToString());
        //                        written = true;
        //                    }
        //                }
        //                if (!written)
        //                {
        //                    html.Write("&nbsp;");
        //                }
        //                html.RenderEndTag(); // td
        //            }
        //        }
        //        html.RenderEndTag(); // tr
        //    }

        //    html.RenderEndTag(); // table

        //    html.RenderEndTag(); // body
        //    html.RenderEndTag(); // html

        //    html.Flush();

        //    return writer.ToString();
        //}

        #endregion

    } // class DataGridViewUtility

} // namespace AM.Windows.Forms
