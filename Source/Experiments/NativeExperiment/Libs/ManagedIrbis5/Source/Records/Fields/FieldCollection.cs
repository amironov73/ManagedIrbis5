// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedParameter.Local

/* FieldCollection.cs -- коллекция полей записи
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;

using AM;
using AM.Collections;
using AM.IO;
using AM.Runtime;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace ManagedIrbis;

/// <summary>
/// Коллекция полей записи.
/// Отличается тем, что принципиально
/// не принимает значения <c>null</c>.
/// </summary>
[Serializable]
public sealed class FieldCollection
    : Collection<Field>,
    IHandmadeSerializable,
    IReadOnly<FieldCollection>
{
    #region Properties

    /// <summary>
    /// Запись, которой принадлежат поля.
    /// </summary>
    [JsonIgnore]
    public Record? Record { get; internal set; }

    #endregion

    #region Private members

    private List<Field>? _GetInnerList() => Items as List<Field>;

    private bool _dontRenumber;

    internal void _RenumberFields()
    {
        if (_dontRenumber)
        {
            return;
        }

        var seen = new DictionaryCounter<int, int>();

        foreach (var field in this)
        {
            var tag = field.Tag;
            field.Repeat = tag <= 0
                ? 0
                : seen.Increment (tag);
        }
    }

    internal void SetModified()
    {
        if (Record is not null)
        {
            Record.Modified = true;
        }
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Add capacity to eliminate reallocations.
    /// </summary>
    public void AddCapacity
        (
            int delta
        )
    {
        Sure.Positive (delta);

        if (_GetInnerList() is { } innerList)
        {
            var newCapacity = innerList.Count + delta;
            if (newCapacity > innerList.Capacity)
            {
                innerList.Capacity = newCapacity;
            }
        }
    }

    /// <summary>
    /// Add range of <see cref="Field"/>s.
    /// </summary>
    public void AddRange
        (
            IEnumerable<Field> fields
        )
    {
        Sure.NotNull ((object?) fields);
        ThrowIfReadOnly();

        if (fields is IList<Field> outer)
        {
            if (_GetInnerList() is { } innerList)
            {
                var newCapacity = innerList.Count + outer.Count;
                EnsureCapacity (newCapacity);
            }
        }

        foreach (var field in fields)
        {
            Add (field);
        }
    }

    /// <summary>
    /// Применение значения поля к коллекции.
    /// Только для неповторяющихся полей!
    /// </summary>
    public FieldCollection ApplyFieldValue
        (
            int tag,
            string? value
        )
    {
        Sure.Positive (tag);

        var field = this.FirstOrDefault
            (
                item => item.Tag == tag
            );

        if (string.IsNullOrEmpty (value))
        {
            if (field is not null)
            {
                Remove (field);
            }
        }
        else
        {
            // значение не пустое

            if (field is null)
            {
                field = new Field { Tag = tag };
                Add (field);
            }

            field.Value = value;
        }

        return this;
    }

    /// <summary>
    /// Запрет перенумерации полей перед большим обновлением.
    /// </summary>
    public void BeginUpdate()
    {
        _dontRenumber = true;
    }

    /// <summary>
    /// Создание клона коллекции.
    /// </summary>
    public FieldCollection Clone()
    {
        var result = new FieldCollection { Record = Record };

        foreach (var field in this)
        {
            var clone = field.Clone();
            clone.Record = Record;
            result.Add (clone);
        }

        return result;
    }

    /// <summary>
    /// Разрешение перенумерации полей по окончании большого обновления.
    /// </summary>
    public void EndUpdate()
    {
        _dontRenumber = false;
        _RenumberFields();
    }

    /// <summary>
    /// Убеждаемся, что емкость списка не меньше указанного числа.
    /// </summary>
    public void EnsureCapacity
        (
            int capacity
        )
    {
        Sure.NonNegative (capacity);

        if (_GetInnerList() is { } innerList && innerList.Capacity < capacity)
        {
            innerList.Capacity = capacity;
        }
    }

    #endregion

    #region Collection<T> members

    /// <inheritdoc cref="Collection{T}.ClearItems" />
    protected override void ClearItems()
    {
        ThrowIfReadOnly();

        foreach (var field in this)
        {
            field.Record = null;
        }

        SetModified();

        base.ClearItems();
    }

    /// <inheritdoc cref="Collection{T}.InsertItem" />
    protected override void InsertItem
        (
            int index,
            Field item
        )
    {
        Sure.NonNegative (index);
        Sure.NotNull (item);

        ThrowIfReadOnly();
        Sure.NotNull (item);

        item.Record = Record;

        base.InsertItem (index, item);

        SetModified();

        _RenumberFields();
    }

    /// <inheritdoc cref="Collection{T}.RemoveItem" />
    protected override void RemoveItem
        (
            int index
        )
    {
        Sure.NonNegative (index);

        ThrowIfReadOnly();

        if (index >= 0 && index < Count)
        {
            var field = this[index];
            field.Record = null;
        }

        base.RemoveItem (index);

        SetModified();

        _RenumberFields();
    }

    /// <inheritdoc cref="Collection{T}.SetItem" />
    protected override void SetItem
        (
            int index,
            Field? item
        )
    {
        Sure.NonNegative (index);

        ThrowIfReadOnly();
        Sure.NotNull (item);

        item!.Record = Record;

        base.SetItem (index, item);

        SetModified();

        _RenumberFields();
    }

    #endregion

    #region IHandmadeSerializable members

    /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
    public void RestoreFromStream
        (
            BinaryReader reader
        )
    {
        Sure.NotNull (reader);

        ThrowIfReadOnly();

        ClearItems();
        var array = reader.ReadArray<Field>();
        AddRange (array);
    }

    /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
    public void SaveToStream
        (
            BinaryWriter writer
        )
    {
        Sure.NotNull (writer);

        writer.WriteArray (this.ToArray());
    }

    #endregion

    #region IReadOnly<T> members

    /// <inheritdoc cref="IReadOnly{T}.ReadOnly" />
    public bool ReadOnly { get; internal set; }

    /// <inheritdoc cref="IReadOnly{T}.AsReadOnly" />
    public FieldCollection AsReadOnly()
    {
        var result = Clone();
        result.SetReadOnly();

        return result;
    }

    /// <inheritdoc cref="IReadOnly{T}.SetReadOnly" />
    public void SetReadOnly()
    {
        ReadOnly = true;
        foreach (var field in this)
        {
            field.SetReadOnly();
        }
    }

    /// <inheritdoc cref="IReadOnly{T}.ThrowIfReadOnly" />
    public void ThrowIfReadOnly()
    {
        if (ReadOnly)
        {
            Magna.Logger.LogError
                (
                    nameof (FieldCollection) + "::" + nameof (ThrowIfReadOnly)
                );

            throw new ReadOnlyException();
        }
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="Object.ToString" />
    public override string ToString()
    {
        return string.Join (Environment.NewLine, this);
    }

    #endregion
}
