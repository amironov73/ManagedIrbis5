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
using System.Collections;

using AM.Reporting.Utils;

#endregion

#nullable enable

namespace AM.Reporting.Data
{
  /// <summary>
  /// Represents the collection of <see cref="Column"/> objects.
  /// </summary>
  public class ColumnCollection : FRCollectionBase
  {
    /// <summary>
    /// Gets or sets a column.
    /// </summary>
    /// <param name="index">The index of a column in this collection.</param>
    /// <returns>The column with specified index.</returns>
    public Column this[int index]
    {
      get { return List[index] as Column; }
      set { List[index] = value; }
    }

    /// <summary>
    /// Finds a column by its name.
    /// </summary>
    /// <param name="name">The name of a column.</param>
    /// <returns>The <see cref="Column"/> object if found; otherwise <b>null</b>.</returns>
    public Column FindByName(string name)
    {
      foreach (Column c in this)
      {
        if (String.Compare(c.Name, name, true) == 0)
          return c;
      }
      return null;
    }

    /// <summary>
    /// Finds a column by its alias.
    /// </summary>
    /// <param name="alias">The alias of a column.</param>
    /// <returns>The <see cref="Column"/> object if found; otherwise <b>null</b>.</returns>
    public Column FindByAlias(string alias)
    {
      foreach (Column c in this)
      {
        if (String.Compare(c.Alias, alias, true) == 0)
          return c;
      }
      return null;
    }

    /// <summary>
    /// Returns an unique column name based on given name.
    /// </summary>
    /// <param name="name">The base name.</param>
    /// <returns>The unique name.</returns>
    public string CreateUniqueName(string name)
    {
      string baseName = name;
      int i = 1;
      while (FindByName(name) != null)
      {
        name = baseName + i.ToString();
        i++;
      }
      return name;
    }

    /// <summary>
    /// Returns an unique column alias based on given alias.
    /// </summary>
    /// <param name="alias">The base alias.</param>
    /// <returns>The unique alias.</returns>
    public string CreateUniqueAlias(string alias)
    {
      string baseAlias = alias;
      int i = 1;
      while (FindByAlias(alias) != null)
      {
        alias = baseAlias + i.ToString();
        i++;
      }
      return alias;
    }

    /// <summary>
    /// Sorts the collection of columns.
    /// </summary>
    public void Sort()
    {
        InnerList.Sort(new ColumnComparer());
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ColumnCollection"/> class with default settings.
    /// </summary>
    /// <param name="owner">The owner of this collection.</param>
    public ColumnCollection(Base owner) : base(owner)
    {
    }
  }

    /// <summary>
    /// Represents the comparer class that used for sorting the collection of columns.
    /// </summary>
    public class ColumnComparer : IComparer
    {
        /// <inheritdoc/>
        public int Compare(object x, object y)
        {
            object xValue = x.GetType().GetProperty("Name").GetValue(x, null);
            object yValue = y.GetType().GetProperty("Name").GetValue(y, null);
            IComparable comp = xValue as IComparable;
            return comp.CompareTo(yValue);
        }
    }
}
