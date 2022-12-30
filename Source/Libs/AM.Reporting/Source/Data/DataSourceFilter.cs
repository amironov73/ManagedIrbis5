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

#endregion

#nullable enable

namespace AM.Reporting.Data
{
    /// <summary>
    /// Determines how to filter the data value.
    /// </summary>
    /// <remarks>
    /// The "Data value" is a value contained in the datasource which you filter.
    /// The "Selected value" is a value you have entered or selected in the dialog control.
    /// </remarks>
    public enum FilterOperation
    {
        /// <summary>
        /// Data value is equal to selected value.
        /// </summary>
        Equal,

        /// <summary>
        /// Data value is not equal to selected value.
        /// </summary>
        NotEqual,

        /// <summary>
        /// Data value is less than selected value.
        /// </summary>
        LessThan,

        /// <summary>
        /// Data value is less than or equal to selected value.
        /// </summary>
        LessThanOrEqual,

        /// <summary>
        /// Data value is greater than selected value.
        /// </summary>
        GreaterThan,

        /// <summary>
        /// Data value is greater than or equal to selected value.
        /// </summary>
        GreaterThanOrEqual,

        /// <summary>
        /// Data string contains selected value.
        /// </summary>
        Contains,

        /// <summary>
        /// Data string does not contain selected value.
        /// </summary>
        NotContains,

        /// <summary>
        /// Data string starts with selected value.
        /// </summary>
        StartsWith,

        /// <summary>
        /// Data string does not start with selected value.
        /// </summary>
        NotStartsWith,

        /// <summary>
        /// Data string ends with selected value.
        /// </summary>
        EndsWith,

        /// <summary>
        /// Data string does not end with selected value.
        /// </summary>
        NotEndsWith
    }

    internal class DataSourceFilter
    {
        private List<FilterElement> elements;

        public void Clear()
        {
            elements.Clear();
        }

        public FilterElement Add (object value, FilterOperation operation)
        {
            var element = new FilterElement (value, operation);
            elements.Add (element);
            return element;
        }

        public void Remove (FilterElement element)
        {
            elements.Remove (element);
        }

        public bool ValueMatch (object value)
        {
            foreach (var element in elements)
            {
                var match = false;

                // check if element is a string list
                if (element.SortedList != null)
                {
                    // valid string list operations are
                    // Equal (same as Contains),
                    // NotEqual (same as NotContains)
                    var valStr = value == null ? "" : value.ToString();
                    switch (element.Operation)
                    {
                        case FilterOperation.Equal:
                        case FilterOperation.Contains:
                            match = element.SortedList.ContainsKey (valStr);
                            break;

                        case FilterOperation.NotEqual:
                        case FilterOperation.NotContains:
                            match = !element.SortedList.ContainsKey (valStr);
                            break;
                    }

                    if (!match)
                    {
                        return false;
                    }

                    continue;
                }

                // check if element is DateTime[] array
                if (value is DateTime time && element.Value is DateTime[] times && times.Length == 2)
                {
                    // Check if value is within range.
                    var elementValDateTime1 = times[0];
                    var elementValDateTime2 = times[1];
                    elementValDateTime2 = elementValDateTime2.AddDays (1);

                    match = time >= elementValDateTime1 && time < elementValDateTime2;
                    if (!match)
                    {
                        return false;
                    }

                    continue;
                }

                if (value is not IComparable valComparable)
                {
                    return false;
                }

                // handle DateTime values in the special way
                if (value is DateTime valDateTime)
                {
                    if (element.Value is DateTime elementValDateTime)
                    {
                        // check if element value contains time. If it does not, strip off the time from value.
                        var containsTime = elementValDateTime.TimeOfDay.Ticks != 0;
                        if (!containsTime)
                        {
                            valComparable = new DateTime (valDateTime.Year, valDateTime.Month, valDateTime.Day);
                        }
                    }
                    else
                    {
                        return false;
                    }
                }

                var compareResult = valComparable.CompareTo (element.Value);

                switch (element.Operation)
                {
                    case FilterOperation.Equal:
                        match = compareResult == 0;
                        break;

                    case FilterOperation.NotEqual:
                        match = compareResult != 0;
                        break;

                    case FilterOperation.LessThan:
                        match = compareResult < 0;
                        break;

                    case FilterOperation.LessThanOrEqual:
                        match = compareResult <= 0;
                        break;

                    case FilterOperation.GreaterThan:
                        match = compareResult > 0;
                        break;

                    case FilterOperation.GreaterThanOrEqual:
                        match = compareResult >= 0;
                        break;
                }

                if (value is string strValue)
                {
                    var elementValue = element.Value == null ? "" : element.Value.ToString();

                    switch (element.Operation)
                    {
                        case FilterOperation.Contains:
                            match = strValue.Contains (elementValue);
                            break;

                        case FilterOperation.NotContains:
                            match = !strValue.Contains (elementValue);
                            break;

                        case FilterOperation.StartsWith:
                            match = strValue.StartsWith (elementValue);
                            break;

                        case FilterOperation.NotStartsWith:
                            match = !strValue.StartsWith (elementValue);
                            break;

                        case FilterOperation.EndsWith:
                            match = strValue.EndsWith (elementValue);
                            break;

                        case FilterOperation.NotEndsWith:
                            match = !strValue.EndsWith (elementValue);
                            break;
                    }
                }

                if (!match)
                {
                    return false;
                }
            }

            return true;
        }

        public DataSourceFilter()
        {
            elements = new List<FilterElement>();
        }


        internal class FilterElement
        {
            public object Value { get; }

            public FilterOperation Operation { get; }

            public SortedList<string, object> SortedList { get; }

            public FilterElement (object value, FilterOperation operation)
            {
                this.Value = value;
                this.Operation = operation;
                if (value is string[] strings)
                {
                    SortedList = new SortedList<string, object>();
                    foreach (var s in strings)
                    {
                        if (!SortedList.ContainsKey (s))
                        {
                            SortedList.Add (s, null);
                        }
                    }
                }
            }
        }
    }
}
