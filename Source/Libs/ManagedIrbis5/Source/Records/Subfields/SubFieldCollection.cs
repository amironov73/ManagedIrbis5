// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* SubFieldCollection.cs -- коллекция подполей
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;
using AM.IO;
using AM.Runtime;

#endregion

#nullable enable

namespace ManagedIrbis
{
    /// <summary>
    /// Коллекция подполей.
    /// Отличается тем, что принципиально не принимает
    /// значения <c>null</c>.
    /// </summary>
    [Serializable]
    [XmlRoot("subfields")]
    [DebuggerDisplay("Count={" + nameof(Count) + "}")]
    public sealed class SubFieldCollection
        : Collection<SubField>,
        IHandmadeSerializable,
        IReadOnly<SubFieldCollection>,
        IDisposable
    {
        #region Properties

        /// <summary>
        /// Field.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        [field: NonSerialized]
        public Field? Field { get; internal set;  }

        #endregion

        #region Construction

        #endregion

        #region Private members

        [ExcludeFromCodeCoverage]
        internal SubFieldCollection SetField
            (
                Field newField
            )
        {
            foreach (SubField subField in this)
            {
                subField.Field = newField;
            }

            return this;
        }

        /*
        internal void SetModified()
        {
            if (!ReferenceEquals(Field, null))
            {
                Field.SetModified();
            }
        }
        */

        #endregion

        #region Public methods

        /// <summary>
        /// Добавление в коллекцию нескольких подполей сразу
        /// </summary>
        public SubFieldCollection AddRange
            (
                IEnumerable<SubField> subFields
            )
        {
            ThrowIfReadOnly();

            foreach (var subField in subFields)
            {
                Add(subField);
            }

            return this;
        }

        /// <summary>
        /// Assign.
        /// </summary>
        public SubFieldCollection Assign
            (
                SubFieldCollection other
            )
        {
            ThrowIfReadOnly();
            Sure.NotNull(other, nameof(other));

            Clear();
            Field = other.Field;
            AddRange(other);

            return this;
        }

        /// <summary>
        /// Assign clone.
        /// </summary>
        public SubFieldCollection AssignClone
            (
                SubFieldCollection other
            )
        {
            ThrowIfReadOnly();
            Sure.NotNull(other, nameof(other));

            Clear();
            Field = other.Field;
            foreach (var subField in other)
            {
                Add(subField.Clone());
            }

            return this;
        }


        /// <summary>
        /// Создание "глубокой" копии коллекции.
        /// </summary>
        public SubFieldCollection Clone()
        {
            SubFieldCollection result = new()
            {
                Field = Field
            };

            foreach (var subField in this)
            {
                var clone = subField.Clone();
                clone.Field = Field;
                result.Add(clone);
            }

            return result;
        }

        /// <summary>
        /// Поиск с помощью предиката.
        /// </summary>
        public SubField? Find
            (
                Predicate<SubField> predicate
            )
        {
            Sure.NotNull(predicate, nameof(predicate));

            return this.FirstOrDefault
                (
                    subField => predicate(subField)
                );
        }

        /// <summary>
        /// Отбор с помощью предиката.
        /// </summary>
        public SubField[] FindAll
            (
                Predicate<SubField> predicate
            )
        {
            Sure.NotNull(predicate, nameof(predicate));

            return this
                .Where(subField => predicate(subField))
                .ToArray();
        }

        /*

        /// <summary>
        /// Restore the collection from JSON.
        /// </summary>
        public static SubFieldCollection FromJson
            (
                string text
            )
        {
            Sure.NotNullNorEmpty(text, nameof(text));

            SubFieldCollection result = JsonConvert.DeserializeObject<SubFieldCollection>
                (
                    text
                );

            return result;
        }

        /// <summary>
        /// Convert the collection to JSON.
        /// </summary>
        public string ToJson()
        {
            string result = JArray.FromObject(this).ToString();

            return result;
        }

        */

        /// <summary>
        /// Получение коллекции из пула.
        /// </summary>
        /// <returns>Объект из пула.</returns>
        public static SubFieldCollection FromPool()
            => SubFieldCollectionPool.Default.Get();

        /// <summary>
        /// Возврат коллекции в пул.
        /// </summary>
        public void ToPool() => SubFieldCollectionPool.Default.Return(this);

        #endregion

        #region Collection<T> members

        /// <inheritdoc cref="Collection{T}.ClearItems" />
        protected override void ClearItems()
        {
            ThrowIfReadOnly();

            foreach (SubField subField in this)
            {
                subField.Field = null;
            }

            /* SetModified(); */

            base.ClearItems();
        }

        /// <inheritdoc cref="Collection{T}.InsertItem" />
        protected override void InsertItem
            (
                int index,
                SubField item
            )
        {
            ThrowIfReadOnly();

            if (item is null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            item.Field = Field;

            /* SetModified(); */

            base.InsertItem(index, item);
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
                SubField subField = this[index];
                if (subField != null)
                {
                    subField.Field = null;
                }

                /* SetModified(); */
            }

            base.RemoveItem(index);
        }

        /// <inheritdoc cref="Collection{T}.SetItem" />
        protected override void SetItem
            (
                int index,
                SubField item
            )
        {
            ThrowIfReadOnly();

            if (item is null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            item.Field = Field;

            /* SetModified(); */

            base.SetItem(index, item);
        }

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref= "IHandmadeSerializable.RestoreFromStream" />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            ThrowIfReadOnly();
            Sure.NotNull(reader, nameof(reader));

            ClearItems();
            SubField[] array = reader.ReadArray<SubField>();
            AddRange(array);
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Sure.NotNull(writer, nameof(writer));

            writer.WriteArray(this.ToArray());
        }

        #endregion

        #region IReadOnly<T> members

        /// <inheritdoc cref="IReadOnly{T}.ReadOnly" />
        public bool ReadOnly { get; internal set; }

        // ReSharper restore InconsistentNaming

        /// <inheritdoc cref="IReadOnly{T}.AsReadOnly" />
        public SubFieldCollection AsReadOnly()
        {
            SubFieldCollection result = Clone();
            result.SetReadOnly();

            return result;
        }

        /// <inheritdoc cref="IReadOnly{T}.ThrowIfReadOnly" />
        public void ThrowIfReadOnly()
        {
            if (ReadOnly)
            {
                Magna.Error
                    (
                        nameof(SubFieldCollection) + "::" + nameof(ThrowIfReadOnly)
                    );

                throw new ReadOnlyException();
            }
        }

        /// <inheritdoc cref="IReadOnly{T}.SetReadOnly" />
        public void SetReadOnly()
        {
            ReadOnly = true;
            foreach (SubField subField in this)
            {
                subField.SetReadOnly();
            }
        }

        #endregion

        #region IDisposable members

        /// <summary>
        /// Очистка коллекции перед помещением ее в пул.
        /// </summary>
        /// <remarks>
        /// <para>Начиная с ManagedIrbis5, коллекция подполей
        /// поддерживает пулинг, см. класс
        /// <see cref="SubFieldCollectionPool"/>.</para>
        /// <para>Очистка перед помещением в пул выполняется с помощью
        /// вызова <see cref="Dispose"/>.</para>
        /// <para>При обычном использовании вызывать метод
        /// <see cref="Dispose"/> не нужно.</para>
        /// </remarks>
        public void Dispose()
        {
            ClearItems();
        }

        #endregion

    } // class SubFieldCollection

} // namespace ManagedIrbis
