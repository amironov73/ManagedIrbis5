// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global

/* ColumnCollection.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using AM.Reporting.Utils;

#endregion

#nullable enable

namespace AM.Reporting.Data;

/// <summary>
/// Represents the collection of <see cref="Column"/> objects.
/// </summary>
public class ColumnCollection
    : ReportCollectionBase
{
    #region Properties

    /// <summary>
    /// Gets or sets a column.
    /// </summary>
    /// <param name="index">The index of a column in this collection.</param>
    /// <returns>The column with specified index.</returns>
    public Column? this [int index]
    {
        get => List[index] as Column;
        set => List[index] = value;
    }

    #endregion

    #region Construction

    /// <summary>
    /// Initializes a new instance of the <see cref="ColumnCollection"/> class with default settings.
    /// </summary>
    /// <param name="owner">The owner of this collection.</param>
    public ColumnCollection
        (
            Base owner
        )
        : base (owner)
    {
        // пустое тело конструктора
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Finds a column by its name.
    /// </summary>
    /// <param name="name">The name of a column.</param>
    /// <returns>The <see cref="Column"/> object if found; otherwise <b>null</b>.</returns>
    public Column? FindByName
        (
            string name
        )
    {
        foreach (Column column in this)
        {
            if (string.Compare (column.Name, name, StringComparison.OrdinalIgnoreCase) == 0)
            {
                return column;
            }
        }

        return null;
    }

    /// <summary>
    /// Finds a column by its alias.
    /// </summary>
    /// <param name="alias">The alias of a column.</param>
    /// <returns>The <see cref="Column"/> object if found; otherwise <b>null</b>.</returns>
    public Column? FindByAlias
        (
            string alias
        )
    {
        foreach (Column c in this)
        {
            if (string.Compare (c.Alias, alias, StringComparison.OrdinalIgnoreCase) == 0)
            {
                return c;
            }
        }

        return null;
    }

    /// <summary>
    /// Returns an unique column name based on given name.
    /// </summary>
    /// <param name="name">The base name.</param>
    /// <returns>The unique name.</returns>
    public string CreateUniqueName
        (
            string name
        )
    {
        var baseName = name;
        var i = 1;
        while (FindByName (name) != null)
        {
            name = baseName + i;
            i++;
        }

        return name;
    }

    /// <summary>
    /// Returns an unique column alias based on given alias.
    /// </summary>
    /// <param name="alias">The base alias.</param>
    /// <returns>The unique alias.</returns>
    public string CreateUniqueAlias
        (
            string alias
        )
    {
        var baseAlias = alias;
        var i = 1;
        while (FindByAlias (alias) != null)
        {
            alias = baseAlias + i;
            i++;
        }

        return alias;
    }

    /// <summary>
    /// Sorts the collection of columns.
    /// </summary>
    public void Sort()
    {
        InnerList.Sort (new ColumnComparer());
    }

    #endregion
}
