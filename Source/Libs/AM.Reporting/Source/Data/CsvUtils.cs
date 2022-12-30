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
using System.ComponentModel;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Net;

using AM.Reporting.Utils;

using System.Globalization;
using System.Collections;

#endregion

#nullable enable

namespace AM.Reporting.Data
{
    internal static class CsvUtils
    {
        /// <summary>
        /// The default field name.
        /// </summary>
        public const string DEFAULT_FIELD_NAME = "Field";

        private static void DetermineTypes (List<string[]> lines, DataTable table, NumberFormatInfo numberInfo,
            NumberFormatInfo currencyInfo, DateTimeFormatInfo dateTimeInfo)
        {
            for (var i = 0; i < table.Columns.Count; i++)
            {
                // gather types here
                Dictionary<Type, int> types = new Dictionary<Type, int>();

                // check all values in the column
                for (var j = 0; j < lines.Count; j++)
                {
                    if (i >= lines[j].Length)
                    {
                        // number of values is less than number of table columns. Reasons: wrong separator or bad-formed csv file?
                        // just skip this line
                    }
                    else
                    {
                        var value = lines[j][i];
                        if (!string.IsNullOrEmpty (value))
                        {
                            if (int.TryParse (value, out var intTemp))
                            {
                                types[typeof (int)] = 1;
                            }
                            else if (value.Contains (currencyInfo.CurrencySymbol) && decimal.TryParse (value,
                                         NumberStyles.Currency, currencyInfo, out var decimalTemp))
                            {
                                types[typeof (decimal)] = 1;
                            }
                            else if (double.TryParse (value, NumberStyles.Number, numberInfo, out var doubleTemp))
                            {
                                types[typeof (double)] = 1;
                            }
                            else if (DateTime.TryParse (value, dateTimeInfo, DateTimeStyles.NoCurrentDateDefault,
                                         out var dateTemp))
                            {
                                types[typeof (DateTime)] = 1;
                            }
                            else
                            {
                                types[typeof (string)] = 1;
                                break;
                            }
                        }
                    }
                }

                // cases allowed:
                // - single type -> the type
                // - mix of ints and doubles -> double
                // - all others should not be mixed -> string
                var guessType = typeof (string);
                if (types.Count == 1)
                {
                    // get a single value this way
                    foreach (var t in types.Keys)
                    {
                        guessType = t;
                    }
                }
                else if (types.Count == 2 && types.ContainsKey (typeof (int)) && types.ContainsKey (typeof (double)))
                {
                    guessType = typeof (double);
                }

                table.Columns[i].DataType = guessType;
            }
        }

        internal static List<string> ReadLines (CsvConnectionStringBuilder builder, int maxLines = 0)
        {
            if (string.IsNullOrEmpty (builder.CsvFile) || string.IsNullOrEmpty (builder.Separator))
            {
                return null;
            }

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
            WebRequest request;
            WebResponse response = null;

            try
            {
                // fix for datafile in current folder
                if (File.Exists (builder.CsvFile))
                {
                    builder.CsvFile = Path.GetFullPath (builder.CsvFile);
                }

                var uri = new Uri (builder.CsvFile);

                if (uri.IsFile)
                {
                    if (Config.ForbidLocalData)
                    {
                        throw new Exception (Res.Get ("ConnectionEditors,Common,OnlyUrlException"));
                    }

                    request = (FileWebRequest)WebRequest.Create (uri);
                    request.Timeout = 5000;
                    response = (FileWebResponse)request.GetResponse();
                }
                else if (uri.OriginalString.StartsWith ("http"))
                {
                    request = (HttpWebRequest)WebRequest.Create (uri);
                    request.Timeout = 5000;
                    response = (HttpWebResponse)request.GetResponse();
                }
                else if (uri.OriginalString.StartsWith ("ftp"))
                {
                    request = (FtpWebRequest)WebRequest.Create (uri);
                    request.Timeout = 5000;
                    response = (FtpWebResponse)request.GetResponse();
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            if (response == null)
            {
                return null;
            }

            List<string> lines = new List<string>();
            if (maxLines == 0)
            {
                maxLines = int.MaxValue;
            }

            // read lines
            using (var reader =
                   new StreamReader (response.GetResponseStream(), Encoding.GetEncoding (builder.Codepage)))
            {
                for (var i = 0; i < maxLines; i++)
                {
                    var line = reader.ReadLine();

                    // end of stream reached
                    if (line == null)
                    {
                        break;
                    }

                    // skip empty lines
                    if (!string.IsNullOrEmpty (line))
                    {
                        lines.Add (line);
                    }
                }
            }

            return lines;
        }

        internal static DataTable CreateDataTable (CsvConnectionStringBuilder builder, List<string> rawLines)
        {
            if (rawLines == null)
            {
                return null;
            }

            // split each line to array of values
            List<string[]> lines = new List<string[]>();
            for (var i = 0; i < rawLines.Count; i++)
            {
                var line = rawLines[i];
                string[] values = line.Split (builder.Separator.ToCharArray());
                if (builder.RemoveQuotationMarks)
                {
                    for (var j = 0; j < values.Length; j++)
                    {
                        values[j] = values[j].Trim ("\"".ToCharArray());
                    }
                }

                lines.Add (values);
            }

            if (lines.Count == 0)
            {
                return null;
            }

            var numberInfo = CultureInfo.GetCultureInfo (builder.NumberFormat)?.NumberFormat ??
                             CultureInfo.CurrentCulture.NumberFormat;
            var currencyInfo = CultureInfo.GetCultureInfo (builder.CurrencyFormat)?.NumberFormat ??
                               CultureInfo.CurrentCulture.NumberFormat;
            var dateTimeInfo = CultureInfo.GetCultureInfo (builder.DateTimeFormat)?.DateTimeFormat ??
                               CultureInfo.CurrentCulture.DateTimeFormat;

            // get table name from file name
            var tableName = Path.GetFileNameWithoutExtension (builder.CsvFile).Replace (".", "_");
            if (string.IsNullOrEmpty (tableName))
            {
                tableName = "Table";
            }

            var table = new DataTable (tableName);

            string[] fields = lines[0];

            // create table columns
            for (var i = 0; i < fields.Length; i++)
            {
                var column = new DataColumn();
                column.DataType = typeof (string);

                // get field names from first string if needed
                var fieldName = fields[i].Replace ("\t", "");
                if (builder.FieldNamesInFirstString && !table.Columns.Contains (fieldName))
                {
                    column.ColumnName = fieldName;
                    column.Caption = column.ColumnName;
                }
                else
                {
                    column.ColumnName = DEFAULT_FIELD_NAME + i.ToString();
                    column.Caption = column.ColumnName;
                }

                table.Columns.Add (column);
            }

            var startIndex = builder.FieldNamesInFirstString ? 1 : 0;

            // cast types of fields if needed
            if (builder.ConvertFieldTypes)
            {
                var number = lines.Count - startIndex;
                DetermineTypes (lines.GetRange (startIndex, number), table, numberInfo, currencyInfo, dateTimeInfo);
            }

            // add table rows
            for (var i = startIndex; i < lines.Count; i++)
            {
                if (lines[i].Length > 0)
                {
                    // get values from the string
                    fields = lines[i];

                    // add a new row
                    var row = table.NewRow();
                    var valuesCount = fields.Length < table.Columns.Count ? fields.Length : table.Columns.Count;
                    for (var j = 0; j < valuesCount; j++)
                    {
                        var value = fields[j];
                        if (!string.IsNullOrEmpty (value))
                        {
                            if (table.Columns[j].DataType == typeof (string))
                            {
                                row[j] = value;
                            }
                            else if (table.Columns[j].DataType == typeof (int))
                            {
                                row[j] = int.Parse (value);
                            }
                            else if (table.Columns[j].DataType == typeof (decimal))
                            {
                                row[j] = decimal.Parse (value, NumberStyles.Currency, currencyInfo);
                            }
                            else if (table.Columns[j].DataType == typeof (double))
                            {
                                row[j] = double.Parse (value, NumberStyles.Number, numberInfo);
                            }
                            else if (table.Columns[j].DataType == typeof (DateTime))
                            {
                                row[j] = DateTime.Parse (value, dateTimeInfo);
                            }
                        }
                    }

                    table.Rows.Add (row);
                }
            }

            return table;
        }
    }
}
