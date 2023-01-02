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
using System.Collections;
using System.ComponentModel;
using System.Drawing;

using AM.Reporting.Utils;

using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Reporting.Data
{
    internal partial class BusinessObjectConverter
    {
        private Dictionary dictionary;
        private int nestingLevel;
        private int maxNestingLevel;
        private FastNameCreator nameCreator;

        private PropertyKind GetPropertyKind (string name, Type type)
        {
            if (type == null)
            {
                return PropertyKind.Simple;
            }

            var kind = PropertyKind.Complex;
            if (type.IsValueType ||
                type == typeof (string) ||
                type == typeof (byte[]) ||
                typeof (Image).IsAssignableFrom (type))
            {
                kind = PropertyKind.Simple;
            }
            else if (typeof (IEnumerable).IsAssignableFrom (type))
            {
                kind = PropertyKind.Enumerable;
            }

            var args = new GetPropertyKindEventArgs (name, type, kind);
            Config.ReportSettings.OnGetBusinessObjectPropertyKind (null, args);
            return args.PropertyKind;
        }

        private bool IsSimpleType (string name, Type type)
        {
            return GetPropertyKind (name, type) == PropertyKind.Simple;
        }

        private bool IsEnumerable (string name, Type type)
        {
            return GetPropertyKind (name, type) == PropertyKind.Enumerable;
        }

        private bool IsLoop (Column column, Type type)
        {
            while (column != null)
            {
                if (column.DataType == type)
                {
                    return true;
                }

                column = column.Parent as Column;
            }

            return false;
        }

        private PropertyDescriptorCollection GetProperties (Column column)
        {
            using (var source = new BindingSource())
            {
                source.DataSource = column.Reference != null ? column.Reference : column.DataType;

                // to get properties list of ICustomTypeDescriptor type, we need an instance
                object instance = null;
                if (source.DataSource is Type type &&
                    typeof (ICustomTypeDescriptor).IsAssignableFrom (type))
                {
                    try
                    {
                        var args = new GetTypeInstanceEventArgs (type);
                        Config.ReportSettings.OnGetBusinessObjectTypeInstance (null, args);
                        instance = args.Instance;
                        source.DataSource = instance;
                    }
                    catch
                    {
                    }
                }

                // generic list? get element type
                if (column.Reference == null && column.DataType.IsGenericType)
                {
                    source.DataSource = column.DataType.GetGenericArguments()[0];
                }

                var properties = source.GetItemProperties (null);
                var filteredProperties = new PropertyDescriptorCollection (null);

                foreach (PropertyDescriptor prop in properties)
                {
                    var args = new FilterPropertiesEventArgs (prop);
                    Config.ReportSettings.OnFilterBusinessObjectProperties (source.DataSource, args);
                    if (!args.Skip)
                    {
                        filteredProperties.Add (args.Property);
                    }
                }

                if (instance is IDisposable disposable)
                {
                    try
                    {
                        disposable.Dispose();
                    }
                    catch
                    {
                    }
                }

                return filteredProperties;
            }
        }

        private Column CreateListValueColumn (Column column)
        {
            var itemType = ListBindingHelper.GetListItemType (column.DataType);

            // find existing column
            var childColumn = column.FindByPropName ("Value");

            // column not found, create new one with default settings
            if (childColumn == null)
            {
                childColumn = new Column();
                childColumn.Name = "Value";
                childColumn.Enabled = IsSimpleType (childColumn.Name, itemType);
                childColumn.SetBindableControlType (itemType);
                column.Columns.Add (childColumn);
            }

            childColumn.DataType = itemType;
            childColumn.PropertyName = "Value";
            childColumn.PropDescriptor = null;

            return childColumn;
        }

        private void GetReference (Column column, Column childColumn)
        {
            if (!Config.ReportSettings.UsePropValuesToDiscoverBO)
            {
                childColumn.Reference = null;
                return;
            }

            object obj = null;
            if (column is BusinessObjectDataSource)
            {
                if (column.Reference is IEnumerable enumerable)
                {
                    var enumerator = enumerable.GetEnumerator();
                    while (enumerator.MoveNext())
                    {
                        obj = enumerator.Current;
                    }
                }
            }
            else
            {
                obj = column.Reference;
            }

            if (obj != null)
            {
                try
                {
                    childColumn.Reference = childColumn.PropDescriptor.GetValue (obj);
                }
                catch
                {
                    childColumn.Reference = null;
                }
            }
        }

        private void CreateInitialObjects (Column column)
        {
            if (nestingLevel >= maxNestingLevel)
            {
                return;
            }

            nestingLevel++;

            var properties = GetProperties (column);
            foreach (PropertyDescriptor prop in properties)
            {
                var type = prop.PropertyType;
                var isSimpleProperty = IsSimpleType (prop.Name, type);
                var isEnumerable = IsEnumerable (prop.Name, type);

                Column childColumn = null;
                if (isEnumerable)
                {
                    childColumn = new BusinessObjectDataSource();
                }
                else
                {
                    childColumn = new Column();
                }

                column.Columns.Add (childColumn);

                childColumn.Name = isEnumerable ? dictionary.CreateUniqueName (prop.Name) : prop.Name;
                childColumn.Alias = prop.DisplayName;
                childColumn.DataType = type;
                childColumn.PropertyName = prop.Name;
                childColumn.PropDescriptor = prop;
                childColumn.SetBindableControlType (type);
                childColumn.Enabled = !isEnumerable || nestingLevel < maxNestingLevel;

                if (!isSimpleProperty)
                {
                    GetReference (column, childColumn);
                    CreateInitialObjects (childColumn);
                }
            }

            if (IsEnumerable (column.Name, column.DataType) && properties.Count == 0)
            {
                CreateListValueColumn (column);
            }

            nestingLevel--;
        }

        // create initial n-level structure
        public void CreateInitialObjects (Column column, int maxNestingLevel)
        {
            this.maxNestingLevel = maxNestingLevel;
            CreateInitialObjects (column);
        }

        // update existing columns - add new, delete non-existent, update PropDescriptor
        public void UpdateExistingObjects (Column column, int maxNestingLevel)
        {
            this.maxNestingLevel = maxNestingLevel;
            nameCreator = new FastNameCreator (dictionary.Report.AllNamedObjects);
            UpdateExistingObjects (column);
        }

        private void UpdateExistingObjects (Column column)
        {
            nestingLevel++;

            // reset property descriptors to determine later which columns are outdated
            foreach (Column c in column.Columns)
            {
                c.PropDescriptor = null;
            }

            var properties = GetProperties (column);
            if (properties.Count > 0)
            {
                foreach (PropertyDescriptor prop in properties)
                {
                    var type = prop.PropertyType;
                    var isSimpleProperty = IsSimpleType (prop.Name, type);
                    var isEnumerable = IsEnumerable (prop.Name, type);

                    // find existing column
                    var childColumn = column.FindByPropName (prop.Name);

                    // column not found, create new one
                    if (childColumn == null)
                    {
                        if (isEnumerable)
                        {
                            childColumn = new BusinessObjectDataSource();
                        }
                        else
                        {
                            childColumn = new Column();
                        }

                        column.Columns.Add (childColumn);

                        if (isEnumerable)
                        {
                            nameCreator.CreateUniqueName (childColumn);
                        }
                        else
                        {
                            childColumn.Name = prop.Name;
                        }

                        childColumn.Alias = prop.DisplayName;
                        childColumn.SetBindableControlType (type);

                        // enable column if it is simple property, or max nesting level is not reached
                        childColumn.Enabled = isSimpleProperty || nestingLevel < maxNestingLevel;
                    }

                    // update column's prop data - the schema may be changed
                    childColumn.DataType = prop.PropertyType;
                    childColumn.PropertyName = prop.Name;
                    childColumn.PropDescriptor = prop;

                    if (childColumn.Enabled && !isSimpleProperty)
                    {
                        GetReference (column, childColumn);
                        UpdateExistingObjects (childColumn);
                    }
                }

                // remove non-existent columns
                for (var i = 0; i < column.Columns.Count; i++)
                {
                    var c = column.Columns[i];

                    // delete columns with empty descriptors, except the "Value" columns
                    if (c.PropDescriptor == null && c.PropertyName != "Value")
                    {
                        column.Columns.RemoveAt (i);
                        i--;
                    }
                }
            }
            else if (IsEnumerable (column.Name, column.DataType))
            {
                CreateListValueColumn (column);
            }

            nestingLevel--;
        }

        public BusinessObjectConverter (Dictionary dictionary)
        {
            this.dictionary = dictionary;
        }
    }
}
