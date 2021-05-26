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
            var result = (List<Field>) Items;
            // ReSharper restore SuspiciousTypeConversion.Global

            return result;
        }

        private bool _dontRenumber;

        // ReSharper disable InconsistentNaming

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
                    : seen.Increment(tag);
            }
        }

        internal FieldCollection _SetRecord
            (
                Record? newRecord
            )
        {
            Record = newRecord;

            foreach (var field in this)
            {
                field.Record = newRecord;
            }

            return this;
        }

        internal void SetModified()
        {
            if (!ReferenceEquals(Record, null))
            {
                Record.Modified = true;
            }
        }

        // ReSharper restore InconsistentNaming

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
        }

        /// <summary>
        /// Add range of <see cref="Field"/>s.
        /// </summary>
        public void AddRange
            (
                IEnumerable<Field> fields
            )
        {
            ThrowIfReadOnly();

            foreach (var field in fields)
            {
                Add(field);
            }
        }

        /// <summary>
        /// Apply the field value.
        /// </summary>
        /// <remarks>
        /// For non-repeating fields only.
        /// </remarks>
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

            if (string.IsNullOrEmpty(value))
            {
                if (!ReferenceEquals(field, null))
                {
                    Remove(field);
                }
            }
            else
            {
                if (field is null)
                {
                    field = new Field {Tag = tag};
                    Add(field);
                }

                field.Value = value;
            }

            return this;
        }

        /// <summary>
        /// Apply the field value.
        /// </summary>
        /// <remarks>
        /// For non-repeating fields only.
        /// </remarks>
        public FieldCollection ApplyFieldValue
            (
                int tag,
                ReadOnlyMemory<char> value
            )
        {
            var field = this.FirstOrDefault
                (
                    item => item.Tag == tag
                );

            if (value.IsEmpty)
            {
                if (!ReferenceEquals(field, null))
                {
                    Remove(field);
                }
            }
            else
            {
                if (field is null)
                {
                    field = new Field {Tag = tag};
                    Add(field);
                }

                field.Value = value.ToString();
            }

            return this;
        }

        /// <summary>
        /// Begin record update.
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
            var result = new FieldCollection
            {
                Record = Record
            };

            foreach (var field in this)
            {
                var clone = field.Clone();
                clone.Record = Record;
                result.Add(clone);
            }

            return result;
        }

        /// <summary>
        /// End record update.
        /// </summary>
        public void EndUpdate()
        {
            _dontRenumber = false;
            _RenumberFields();
        }

        /// <summary>
        /// Ensure the capacity.
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
        }

        /// <summary>
        /// Find first occurrence of the field with given predicate.
        /// </summary>
        public Field? Find
            (
                Predicate<Field> predicate
            )
        {
            return this.FirstOrDefault
            (
                field => predicate(field)
            );
        }

        /// <summary>
        /// Find all occurrences of the field
        /// with given predicate.
        /// </summary>
        public Field[] FindAll
            (
                Predicate<Field> predicate
            )
        {
            return this.Where
                (
                    field => predicate(field)
                )
                .ToArray();
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
            ThrowIfReadOnly();
            Sure.NotNull(item, "item");

            item.Record = Record;

            base.InsertItem(index, item);

            SetModified();

            _RenumberFields();
        }

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
                if (field != null)
                {
                    field.Record = null;
                }
            }

            base.RemoveItem(index);

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
            ThrowIfReadOnly();

            if (item is null)
            {
                throw new ArgumentNullException();
            }

            item.Record = Record;

            base.SetItem(index, item);

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
            ThrowIfReadOnly();

            ClearItems();
            var array = reader.ReadArray<Field>();
            AddRange(array);
        } // method RestoreFromStream

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            writer.WriteArray(this.ToArray());
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
                Magna.Error
                    (
                        "FieldCollection::ThrowIfReadOnly"
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
