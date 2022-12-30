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
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Collections;

#endregion

#nullable enable

namespace AM.Reporting.Data
{
    internal static class DataHelper
    {
        public static DataSourceBase GetDataSource (Dictionary dictionary, string complexName)
        {
            if (string.IsNullOrEmpty (complexName))
            {
                return null;
            }

            string[] names = complexName.Split ('.');
            if (dictionary.FindByAlias (names[0]) is not DataSourceBase data)
            {
                return null;
            }

            Column column = data;

            // take into account nested tables. Table may even be nested into Column.
            for (var i = 1; i < names.Length; i++)
            {
                var childColumn = column.Columns.FindByAlias (names[i]);
                if (childColumn == null)
                {
                    break;
                }

                if (childColumn is DataSourceBase @base)
                {
                    data = @base;
                }

                column = childColumn;
            }

            return data;
        }

        public static Column GetColumn (Dictionary dictionary, string complexName)
        {
            if (string.IsNullOrEmpty (complexName))
            {
                return null;
            }

            string[] names = complexName.Split ('.');

            // the first part of complex name is always datasource name.
            var data = dictionary.FindByAlias (names[0]) as DataSourceBase;

            return GetColumn (dictionary, data, names, false);
        }

        public static Column GetColumn (Dictionary dictionary, DataSourceBase data, string[] names, bool initRelation)
        {
            // Process the following cases:
            // - Table.Column
            // - Table.NestedTable.Column (specific to BO)
            // - Table.RelatedTable.Column
            // - Table.Column where Column has '.' inside (specific to MSSQL)
            // - Table.ComplexColumn.Column (specific to BO)

            if (data == null || names.Length < 2)
            {
                return null;
            }

            // search for relation
            var i = 1;
            for (; i < names.Length; i++)
            {
                var found = false;
                foreach (Relation r in dictionary.Relations)
                {
                    if (r.ChildDataSource == data &&
                        (r.ParentDataSource != null && r.ParentDataSource.Alias == names[i] || r.Alias == names[i]))
                    {
                        data = r.ParentDataSource;
                        if (initRelation)
                        {
                            data.FindParentRow (r);
                        }

                        found = true;
                        break;
                    }
                }

                // nothing found, break and try column name.
                if (!found)
                {
                    break;
                }
            }

            // the rest is column name.
            var columns = data.Columns;

            // try full name first
            var columnName = string.Empty;
            for (var j = i; j < names.Length; j++)
                columnName += (columnName.Length == 0 ? "" : ".") + names[j];

            var column = columns.FindByAlias (columnName);
            if (column != null)
            {
                return column;
            }

            // try nested columns
            for (var j = i; j < names.Length; j++)
            {
                column = columns.FindByAlias (names[j]);
                if (column == null)
                {
                    return null;
                }

                columns = column.Columns;
            }

            return column;
        }

        public static bool IsValidColumn (Dictionary dictionary, string complexName)
        {
            return GetColumn (dictionary, complexName) != null;
        }

        // Checks if the specified column name is simple column.
        // The column is simple if it belongs to the datasource, and that datasource
        // is simple as well. This check is needed when we prepare a script for compile.
        // Simple columns are handled directly by the Report component, so they should be
        // skipped when generating the expression handler code.
        public static bool IsSimpleColumn (Dictionary dictionary, string complexName)
        {
            var column = GetColumn (dictionary, complexName);
            return column != null && column.Parent is DataSourceBase @base &&
                   @base.FullName + "." + column.Alias == complexName;
        }

        public static bool IsValidParameter (Dictionary dictionary, string complexName)
        {
            string[] names = complexName.Split ('.');
            var par = dictionary.Parameters.FindByName (names[0]);
            if (par == null)
            {
                if (names.Length == 1)
                {
                    par = dictionary.SystemVariables.FindByName (names[0]);
                }

                return par != null;
            }

            for (var i = 1; i < names.Length; i++)
            {
                par = par.Parameters.FindByName (names[i]);
                if (par == null)
                {
                    return false;
                }
            }

            return true;
        }

        public static bool IsValidTotal (Dictionary dictionary, string name)
        {
            var sum = dictionary.Totals.FindByName (name);
            return sum != null;
        }

        public static Parameter GetParameter (Dictionary dictionary, string complexName)
        {
            string[] names = complexName.Split ('.');
            var par = dictionary.Parameters.FindByName (names[0]);
            if (par == null)
            {
                par = dictionary.SystemVariables.FindByName (names[0]);
                return par;
            }

            for (var i = 1; i < names.Length; i++)
            {
                par = par.Parameters.FindByName (names[i]);
                if (par == null)
                {
                    return null;
                }
            }

            return par;
        }

        public static Parameter CreateParameter (Dictionary dictionary, string complexName)
        {
            string[] names = complexName.Split ('.');
            var parameters = dictionary.Parameters;
            Parameter par = null;

            for (var i = 0; i < names.Length; i++)
            {
                par = parameters.FindByName (names[i]);
                if (par == null)
                {
                    par = new Parameter (names[i]);
                    parameters.Add (par);
                }

                parameters = par.Parameters;
            }

            return par;
        }

        public static object GetTotal (Dictionary dictionary, string name)
        {
            var sum = dictionary.Totals.FindByName (name);
            if (sum != null)
            {
                return sum.Value;
            }

            return null;
        }

        public static Type GetColumnType (Dictionary dictionary, string complexName)
        {
            var column = GetColumn (dictionary, complexName);
            return column.DataType;
        }

        public static Relation FindRelation (Dictionary dictionary, DataSourceBase parent, DataSourceBase child)
        {
            foreach (Relation relation in dictionary.Relations)
            {
                if (relation.ParentDataSource == parent && relation.ChildDataSource == child)
                {
                    return relation;
                }
            }

            return null;
        }
    }
}
