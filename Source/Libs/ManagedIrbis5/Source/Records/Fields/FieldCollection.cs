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

#endregion

#nullable enable

namespace ManagedIrbis
{
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
        /// Record.
        /// </summary>
        [JsonIgnore]
        public Record? Record { get; internal set; }

        #endregion

        #region Private members

        private List<Field> _GetInnerList()
        {
            // ReSharper disable SuspiciousTypeConversion.Global
            var result = (List<Field>)Items;

            // ReSharper restore SuspiciousTypeConversion.Global

            return result;
        }

        private bool _dontRenumber;

        internal void _RenumberFields()
        {
            if (_dontRenumber)
            {
                return;
            }

            var seen = new DictionaryCounterInt32<int>();

            foreach (var field in this)
            {
                var tag = field.Tag;
                field.Repeat = tag <= 0
                    ? 0
                    : seen.Increment (tag);
            }

        } // method _RenumberField

        internal void SetModified()
        {
            if (Record is not null)
            {
                Record.Modified = true;
            }

        } // method SetModified

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
            var innerList = _GetInnerList();
            var newCapacity = innerList.Count + delta;
            if (newCapacity > innerList.Capacity)
            {
                innerList.Capacity = newCapacity;
            }

        } // method AddCapacity

        /// <summary>
        /// Add range of <see cref="Field"/>s.
        /// </summary>
        public void AddRange
            (
                IEnumerable<Field> fields
            )
        {
            ThrowIfReadOnly();

            if (fields is IList<Field> outer)
            {
                var inner = _GetInnerList();
                var newCapacity = inner.Count + outer.Count;
                EnsureCapacity (newCapacity);
            }

            foreach (var field in fields)
            {
                Add (field);
            }

        } // method AddRange

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

        } // method ApplyFieldValue

        /// <summary>
        /// Запрет перенумерации полей перед большим обновлением.
        /// </summary>
        public void BeginUpdate() => _dontRenumber = true;

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

        } // method Clone

        /// <summary>
        /// Разрешение перенумерации полей по окончании большого обновления.
        /// </summary>
        public void EndUpdate()
        {
            _dontRenumber = false;
            _RenumberFields();

        } // method EndUpdate

        /// <summary>
        /// Убеждаемся, что емкость списка не меньше указанного числа.
        /// </summary>
        public void EnsureCapacity
            (
                int capacity
            )
        {
            var innerList = _GetInnerList();
            if (innerList.Capacity < capacity)
            {
                innerList.Capacity = capacity;
            }

        } // method EnsureCapacity

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

        } // method ClearItems

        /// <inheritdoc cref="Collection{T}.InsertItem" />
        protected override void InsertItem
            (
                int index,
                Field item
            )
        {
            ThrowIfReadOnly();
            Sure.NotNull (item);

            item.Record = Record;

            base.InsertItem (index, item);

            SetModified();

            _RenumberFields();

        } // method InsertItem

        /// <inheritdoc cref="Collection{T}.RemoveItem" />
        protected override void RemoveItem
            (
                int index
            )
        {
            ThrowIfReadOnly();

            if (index >= 0 && index < Count)
            {
                var field = this[index];
                field.Record = null;
            }

            base.RemoveItem (index);

            SetModified();

            _RenumberFields();

        } // method RemoveItem

        /// <inheritdoc cref="Collection{T}.SetItem" />
        protected override void SetItem
            (
                int index,
                Field? item
            )
        {
            ThrowIfReadOnly();
            Sure.NotNull (item);

            item!.Record = Record;

            base.SetItem (index, item);

            SetModified();

            _RenumberFields();

        } // method SetItem

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            ThrowIfReadOnly();

            ClearItems();
            var array = reader.ReadArray<Field>();
            AddRange (array);

        } // method RestoreFromStream

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            writer.WriteArray (this.ToArray());

        } // method SaveToStream

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

        } // method AsReadOnly

        /// <inheritdoc cref="IReadOnly{T}.SetReadOnly" />
        public void SetReadOnly()
        {
            ReadOnly = true;
            foreach (var field in this)
            {
                field.SetReadOnly();
            }

        } // method SetReadOnly

        /// <inheritdoc cref="IReadOnly{T}.ThrowIfReadOnly" />
        public void ThrowIfReadOnly()
        {
            if (ReadOnly)
            {
                Magna.Error
                    (
                        nameof (FieldCollection) + "::" + nameof (ThrowIfReadOnly)
                    );

                throw new ReadOnlyException();
            }

        } // method ThrowIfReadOnly

        #endregion

        #region Object members

        /// <inheritdoc cref="Object.ToString" />
        public override string ToString()
        {
            return string.Join
                (
                    Environment.NewLine,
                    this
                );
        } // method ToString

        #endregion

    } // class FieldCollection

} // namespace ManagedIrbis
