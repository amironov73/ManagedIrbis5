// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMethodReturnValue.Global

/* ReportCollectionBase.cs -- базовая коллекция для всех элементов отчета
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections;
using System.Collections.Generic;

#endregion

#nullable enable

namespace AM.Reporting.Utils;

/// <summary>
/// Базовая коллекция для всех элементов отчета.
/// </summary>
public class ReportCollectionBase
    : CollectionBase
{
    #region Properties

    /// <summary>
    /// Gets an owner of this collection.
    /// </summary>
    public Base? Owner { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public ReportCollectionBase()
        : this (null)
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Конструктор, задающий владельца коллекции.
    /// </summary>
    /// <param name="owner">Владелец коллекции (опционально).</param>
    public ReportCollectionBase
        (
            Base? owner
        )
    {
        Owner = owner;
    }

    #endregion

    #region Private members

    /// <inheritdoc cref="CollectionBase.OnClear" />
    protected override void OnClear()
    {
        if (Owner is not null)
        {
            while (Count > 0)
            {
                (List[0] as Base)?.Dispose();
            }
        }
    }

    /// <inheritdoc cref="CollectionBase.OnInsert" />
    protected override void OnInsert
        (
            int index,
            object? value
        )
    {
        if (Owner is not null)
        {
            if (value is Base baseItem)
            {
                baseItem.Parent = null;
                baseItem.SetParent (Owner);
            }
        }
    }

    /// <inheritdoc cref="CollectionBase.OnRemove" />
    protected override void OnRemove
        (
            int index,
            object? value
        )
    {
        if (Owner is not null)
        {
            (value as Base)?.SetParent (null);
        }
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Adds the specified elements to the end of this collection.
    /// </summary>
    /// <param name="range">Range of elements.</param>
    public void AddRange
        (
            IEnumerable<Base> range
        )
    {
        Sure.NotNull (range);

        foreach (var item in range)
        {
            Add (item);
        }
    }

    /// <summary>
    /// Adds the specified elements to the end of this collection.
    /// </summary>
    /// <param name="range">Collection of elements.</param>
    public void AddRange
        (
            ObjectCollection range
        )
    {
        Sure.NotNull (range);

        foreach (Base item in range)
        {
            Add (item);
        }
    }

    /// <summary>
    /// Adds an object to the end of this collection.
    /// </summary>
    /// <param name="value">Object to add.</param>
    /// <returns>Index of the added object.</returns>
    public int Add
        (
            Base? value
        )
    {
        if (value is null)
        {
            return -1;
        }

        return List.Add (value);
    }

    /// <summary>
    /// Inserts an object into this collection at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index at which value should be inserted.</param>
    /// <param name="value">The object to insert.</param>
    public void Insert
        (
            int index,
            Base? value
        )
    {
        if (value is not null)
        {
            List.Insert (index, value);
        }
    }

    /// <summary>
    /// Removes the specified object from the collection.
    /// </summary>
    /// <param name="value">Object to remove.</param>
    public void Remove
        (
            Base value
        )
    {
        if (Contains (value))
        {
            List.Remove (value);
        }
    }

    /// <summary>
    /// Returns the zero-based index of the first occurrence of an object.
    /// </summary>
    /// <param name="value">The object to locate in the collection.</param>
    /// <returns>The zero-based index of the first occurrence of value within the entire collection, if found;
    /// otherwise, -1.</returns>
    public int IndexOf
        (
            Base value
        )
    {
        return List.IndexOf (value);
    }

    /// <summary>
    /// Determines whether an element is in the collection.
    /// </summary>
    /// <param name="value">The object to locate in the collection.</param>
    /// <returns><b>true</b> if object is found in the collection; otherwise, <b>false</b>.</returns>
    public bool Contains
        (
            Base value
        )
    {
        return List.Contains (value);
    }

    /// <summary>
    /// Returns an array of collection items.
    /// </summary>
    /// <returns></returns>
    public object[] ToArray()
    {
        return InnerList.ToArray()!;
    }

    /// <summary>
    /// Determines whether two collections are equal.
    /// </summary>
    /// <param name="list">The collection to compare with.</param>
    /// <returns><b>true</b> if collections are equal; <b>false</b> otherwise.</returns>
    public bool Equals
        (
            ReportCollectionBase list
        )
    {
        Sure.NotNull (list);

        var result = Count == list.Count;
        if (result)
        {
            for (var i = 0; i < list.Count; i++)
            {
                if (List[i] != list.List[i])
                {
                    result = false;
                    break;
                }
            }
        }

        return result;
    }

    /// <summary>
    /// Copies the content to another collection.
    /// </summary>
    /// <param name="list">The collection to copy to.</param>
    public void CopyTo
        (
            ReportCollectionBase list
        )
    {
        Sure.NotNull (list);

        list.Clear();
        for (var i = 0; i < Count; i++)
        {
            list.Add (List[i] as Base);
        }
    }

    #endregion
}
