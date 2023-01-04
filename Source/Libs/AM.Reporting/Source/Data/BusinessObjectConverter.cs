// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Local

/* BusinessObjectConverter.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;

using AM.Reporting.Utils;

using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Reporting.Data;

internal class BusinessObjectConverter
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public BusinessObjectConverter
        (
            Dictionary dictionary
        )
    {
        Sure.NotNull (dictionary);

        _dictionary = dictionary;
    }

    #endregion

    #region Private members

    private readonly Dictionary _dictionary;
    private int _nestingLevel;
    private int _maxNestingLevel;
    private FastNameCreator? _fastNameCreator;

    private PropertyKind GetPropertyKind
        (
            string name,
            Type? type
        )
    {
        if (type is null)
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

    private bool IsSimpleType
        (
            string name,
            Type type
        )
    {
        return GetPropertyKind (name, type) == PropertyKind.Simple;
    }

    private bool IsEnumerable
        (
            string name,
            Type type
        )
    {
        return GetPropertyKind (name, type) == PropertyKind.Enumerable;
    }

    private bool IsLoop
        (
            Column column,
            Type type
        )
    {
        var item = column;
        while (item is not null)
        {
            if (item.DataType == type)
            {
                return true;
            }

            item = item.Parent as Column;
        }

        return false;
    }

    private PropertyDescriptorCollection GetProperties
        (
            Column column
        )
    {
        using (var source = new BindingSource())
        {
            source.DataSource = column.Reference ?? column.DataType;

            // to get properties list of ICustomTypeDescriptor type, we need an instance
            object? instance = null;
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
                catch (Exception exception)
                {
                    Debug.WriteLine (exception.Message);
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
                catch (Exception exception)
                {
                    Debug.WriteLine (exception.Message);
                }
            }

            return filteredProperties;
        }
    }

    private Column CreateListValueColumn
        (
            Column column
        )
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

    private void GetReference
        (
            Column column,
            Column childColumn
        )
    {
        if (!Config.ReportSettings.UsePropValuesToDiscoverBO)
        {
            childColumn.Reference = null;
            return;
        }

        object? obj = null;
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
                childColumn.Reference = childColumn.PropDescriptor!.GetValue (obj);
            }
            catch
            {
                childColumn.Reference = null;
            }
        }
    }

    private void CreateInitialObjects
        (
            Column column
        )
    {
        if (_nestingLevel >= _maxNestingLevel)
        {
            return;
        }

        _nestingLevel++;

        var properties = GetProperties (column);
        foreach (PropertyDescriptor prop in properties)
        {
            var type = prop.PropertyType;
            var isSimpleProperty = IsSimpleType (prop.Name, type);
            var isEnumerable = IsEnumerable (prop.Name, type);

            var childColumn = isEnumerable
                ? new BusinessObjectDataSource()
                : new Column();

            column.Columns.Add (childColumn);

            childColumn.Name = isEnumerable ? _dictionary.CreateUniqueName (prop.Name) : prop.Name;
            childColumn.Alias = prop.DisplayName;
            childColumn.DataType = type;
            childColumn.PropertyName = prop.Name;
            childColumn.PropDescriptor = prop;
            childColumn.SetBindableControlType (type);
            childColumn.Enabled = !isEnumerable || _nestingLevel < _maxNestingLevel;

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

        _nestingLevel--;
    }

    #endregion

    #region Public methods

    // create initial n-level structure
    public void CreateInitialObjects
        (
            Column column,
            int maxNestingLevel
        )
    {
        _maxNestingLevel = maxNestingLevel;
        CreateInitialObjects (column);
    }

    // update existing columns - add new, delete non-existent, update PropDescriptor
    public void UpdateExistingObjects
        (
            Column column,
            int maxNestingLevel
        )
    {
        _maxNestingLevel = maxNestingLevel;
        _fastNameCreator = new FastNameCreator (_dictionary.Report!.AllNamedObjects);
        UpdateExistingObjects (column);
    }

    private void UpdateExistingObjects
        (
            Column column
        )
    {
        _nestingLevel++;

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
                    childColumn = isEnumerable
                        ? new BusinessObjectDataSource()
                        : new Column();

                    column.Columns.Add (childColumn);

                    if (isEnumerable)
                    {
                        _fastNameCreator!.CreateUniqueName (childColumn);
                    }
                    else
                    {
                        childColumn.Name = prop.Name;
                    }

                    childColumn.Alias = prop.DisplayName;
                    childColumn.SetBindableControlType (type);

                    // enable column if it is simple property, or max nesting level is not reached
                    childColumn.Enabled = isSimpleProperty || _nestingLevel < _maxNestingLevel;
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
                if (c!.PropDescriptor == null && c.PropertyName != "Value")
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

        _nestingLevel--;
    }

    #endregion
}
