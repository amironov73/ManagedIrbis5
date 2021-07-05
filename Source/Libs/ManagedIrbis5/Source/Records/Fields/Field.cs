// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable ReplaceSliceWithRangeIndexer
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local

/* Field.cs -- поле библиографической записи
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;
using AM.IO;
using AM.Runtime;

using ManagedIrbis.Infrastructure;

#endregion

#nullable enable

namespace ManagedIrbis
{
    /// <summary>
    /// Поле библиографической записи.
    /// </summary>
    [XmlRoot("field")]
    public class Field
        : IHandmadeSerializable,
        IReadOnly<Field>,
        IEnumerable<SubField>,
        IVerifiable
    {
        #region Constants

        /// <summary>
        /// Специальный код, зарезервированный для
        /// значения поля до первого разделителя.
        /// </summary>
        private const char ValueCode = '\0';

        /// <summary>
        /// Нет тега, т. е. тег ещё не присвоен.
        /// </summary>
        public const int NoTag = 0;

        /// <summary>
        /// Разделитель подполей.
        /// </summary>
        public const char Delimiter = '^';

        /// <summary>
        /// Количество индикаторов поля.
        /// </summary>
        public const int IndicatorCount = 2;

        #endregion

        #region Properties

        /// <summary>
        /// Метка поля.
        /// </summary>
        [XmlAttribute("tag")]
        [JsonPropertyName("tag")]
        public int Tag { get; set; }

        /// <summary>
        /// Значение поля до первого разделителя.
        /// </summary>
        /// <remarks>
        /// Значение имитируется с помощью первого подполя,
        /// код которого должен быть равен '\0'.
        /// </remarks>
        public string? Value
        {
            get => GetValueSubField()?.Value ?? default;
            set
            {
                Clear();
                if (value.SafeContains(Delimiter))
                {
                    DecodeBody(value);
                }
                else
                {
                    if (!string.IsNullOrEmpty(value))
                    {
                        CreateValueSubField().Value = value;
                    }
                }
            } // set
        } // property Value

        /// <summary>
        /// Список подполей.
        /// </summary>
        [XmlArrayItem("subfield")]
        [JsonPropertyName("subfields")]
        public SubFieldCollection Subfields { get; } = new ();

        /// <summary>
        /// Номер повторения поля.
        /// </summary>
        /// <remarks>
        /// Формируется автоматически.
        /// </remarks>
        [XmlIgnore]
        [JsonIgnore]
        public int Repeat { get; internal set; }

        /// <summary>
        /// Запись, которой принадлежит поле.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public Record? Record { get; internal set; }

        /// <summary>
        /// Пустое ли поле?
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public bool IsEmpty => Subfields.Count == 0;

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        public Field()
        {
        } // constructor

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="tag">Метка поля.</param>
        /// <param name="value">Значение поля до первого разделителя
        /// (опционально).</param>
        public Field
            (
                int tag,
                string? value
            )
        {
            Tag = tag;
            Value = value;
        } // constructor

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="subfield1"></param>
        public Field
            (
                int tag,
                SubField subfield1
            )
        {
            Tag = tag;
            Subfields.Add(subfield1);
        } // constructor

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="subfield1"></param>
        /// <param name="subfield2"></param>
        public Field
            (
                int tag,
                SubField subfield1,
                SubField subfield2
            )
        {
            Tag = tag;
            Subfields.Add(subfield1);
            Subfields.Add(subfield2);
        } // constructor

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="subfield1"></param>
        /// <param name="subfield2"></param>
        /// <param name="subfield3"></param>
        public Field
            (
                int tag,
                SubField subfield1,
                SubField subfield2,
                SubField subfield3
            )
        {
            Tag = tag;
            Subfields.Add(subfield1);
            Subfields.Add(subfield2);
            Subfields.Add(subfield3);
        } // constructor

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="tag">Метка поля.</param>
        /// <param name="subfields">Подполя.</param>
        public Field
            (
                int tag,
                params SubField[] subfields
            )
        {
            Tag = tag;
            Subfields.AddRange(subfields);
        } // constructor

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="tag">Метка поля.</param>
        /// <param name="code1">Код подполя.</param>
        /// <param name="value1">Значение подполя (опционально).</param>
        public Field
            (
                int tag,
                char code1,
                ReadOnlyMemory<char> value1 = default
            )
        {
            Tag = tag;
            Subfields.Add(new SubField(code1, value1));
        } // constructor

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="tag">Метка поля.</param>
        /// <param name="code1">Код подполя.</param>
        /// <param name="value1">Значение подполя.</param>
        /// <param name="code2">Код подполя.</param>
        /// <param name="value2">Значение подполя (опционально).</param>
        public Field
            (
                int tag,
                char code1,
                string? value1,
                char code2,
                string? value2 = default
            )
        {
            Tag = tag;
            Subfields.Add(new SubField(code1, value1));
            Subfields.Add(new SubField(code2, value2));
        } // constructor

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="tag">Метка поля.</param>
        /// <param name="code1">Код подполя.</param>
        /// <param name="value1">Значение подполя.</param>
        /// <param name="code2">Код подполя.</param>
        /// <param name="value2">Значение подполя.</param>
        /// <param name="code3">Код подполя.</param>
        /// <param name="value3">Значение подполя (опционально).</param>
        public Field
            (
                int tag,
                char code1,
                string? value1,
                char code2,
                string? value2,
                char code3,
                string? value3 = default
            )
        {
            Tag = tag;
            Subfields.Add(new SubField(code1, value1));
            Subfields.Add(new SubField(code2, value2));
            Subfields.Add(new SubField(code3, value3));
        } // constructor

        // /// <summary>
        // /// Конструктор.
        // /// </summary>
        // /// <param name="tag">Метка поля.</param>
        // /// <param name="subfields">Коды и значения подполей.</param>
        // public Field
        //     (
        //         int tag,
        //         params string?[] subfields
        //     )
        // {
        //     Tag = tag;
        //     for (var i = 0; i < subfields.Length; i += 2)
        //     {
        //         var code = subfields[i]![0];
        //         var value = subfields[i + 1];
        //         Subfields.Add(new SubField(code, value));
        //     }
        // } // constructor

        #endregion

        #region Public methods

        /// <summary>
        /// Поле с подполями.
        /// </summary>
        public static Field WithSubFields
            (
                int tag,
                params string[] subfields
            )
        {
            var result = new Field(tag);
            for (var i = 0; i < subfields.Length; i += 2)
            {
                var code = subfields[i][0];
                var value = subfields[i + 1];
                result.Subfields.Add(new SubField(code, value));
            }

            return result;
        }

        /// <summary>
        /// Добавление подполя в конец списка подполей.
        /// </summary>
        /// <param name="subfield">Добавляемое подполе.</param>
        /// <returns>this</returns>
        public Field Add
            (
                SubField subfield
            )
        {
            Subfields.Add(subfield);
            return this;
        } // method Add

        /// <summary>
        /// Добавление подполя в конец списка подполей.
        /// </summary>
        /// <param name="code">Код подполя.</param>
        /// <param name="value">Значение подполя (опционально).</param>
        /// <returns>this</returns>
        public Field Add
            (
                char code,
                ReadOnlyMemory<char> value
            )
        {
            Subfields.Add(new SubField(code, value));
            return this;
        } // method Add

        /// <summary>
        /// Добавление подполя в конец списка подполей.
        /// </summary>
        /// <param name="code">Код подполя.</param>
        /// <param name="value">Значение подполя (опционально).</param>
        /// <returns>this</returns>
        public Field Add
            (
                char code,
                string? value = default
            )
        {
            Subfields.Add(new SubField(code, value));
            return this;
        } // method Add

        /// <summary>
        /// Assign the field from another.
        /// </summary>
        public Field AssignFrom
            (
                Field source
            )
        {
            Value = source.Value;
            Subfields.Clear();
            foreach (var subField in source.Subfields)
            {
                Subfields.Add(subField.Clone());
            }

            return this;
        } // method AssignFrom

        /// <summary>
        /// Compares the specified fields.
        /// </summary>
        public static int Compare
            (
                Field field1,
                Field field2
            )
        {
            var result = field1.Tag - field2.Tag;
            if (result != 0)
            {
                return result;
            }

            result = Utility.CompareOrdinal
                (
                    field1.Value,
                    field2.Value
                );
            if (result != 0)
            {
                return result;
            }

            result = field1.Subfields.Count
                     - field2.Subfields.Count;
            if (result != 0)
            {
                return result;
            }

            for (int i = 0; i < field1.Subfields.Count; i++)
            {
                var subField1 = field1.Subfields[i];
                var subField2 = field2.Subfields[i];

                result = SubField.Compare
                    (
                        subField1,
                        subField2
                    );
                if (result != 0)
                {
                    return result;
                }
            }

            return result;

        } // method Compare

        /// <summary>
        /// Если нет подполя, выделенного для хранения
        /// значения поля до первого разделителя,
        /// создаем его (оно должно быть первым в списке подполей).
        /// </summary>
        public SubField CreateValueSubField()
        {
            SubField result;

            if (Subfields.Count == 0)
            {
                result = new SubField { Code = ValueCode };
                Subfields.Add(result);
                return result;

            }

            result = Subfields[0];
            if (result.Code != ValueCode)
            {
                result = new SubField { Code = ValueCode };
                Subfields.Insert(0, result);
            }

            return result;
        } // method CreateValueSubField

        /// <summary>
        /// Получаем подполе, выделенное для хранения
        /// значения поля до первого разделителя.
        /// </summary>
        public SubField? GetValueSubField()
        {
            if (Subfields.Count == 0)
            {
                return null;
            }

            var result = Subfields[0];
            if (result.Code == ValueCode)
            {
                return result;
            }

            return null;
        } // method GetValueSubField

        /// <summary>
        /// Добавление подполя в конец списка подполей.
        /// </summary>
        public Field Add
            (
                char code,
                object? value
            )
        {
            if (code == ValueCode)
            {
                Value = value?.ToString();
                return this;
            }

            var text = value?.ToString();
            var subfield = new SubField { Code = code, Value = text };
            Subfields.Add(subfield);

            return this;

        } // method Add

        /// <summary>
        /// Добавление поля, если переданное значение не равно 0.
        /// </summary>
        public Field AddNonEmpty
            (
                char code,
                int value
            )
        {
            if (value is not 0)
            {
                Add(code, value.ToInvariantString());
            }

            return this;

        } // method AddNonEmpty

        /// <summary>
        /// Добавление поля, если переданное значение не равно 0.
        /// </summary>
        public Field AddNonEmpty
            (
                char code,
                long value
            )
        {
            if (value is not 0)
            {
                Add(code, value.ToInvariantString());
            }

            return this;

        } // method AddNonEmpty

        /// <summary>
        /// Добавление поля, если переданная дата имеет смысл
        /// (не равна <see cref="DateTime.MinValue"/>).
        /// </summary>
        public Field AddNonEmpty
            (
                char code,
                DateTime? value
            )
        {
            if (value is not null)
            {
                if (value.Value != DateTime.MinValue)
                {
                    Add(code, IrbisDate.ConvertDateToString(value.Value));
                }
            }

            return this;

        } // method AddNonEmpty

        /// <summary>
        /// Добавление подполя в конец списка подполей
        /// при условии, что значение поля не пустое.
        /// </summary>
        public Field AddNonEmpty
            (
                char code,
                object? value
            )
        {
            if (value is not null)
            {
                if (code == ValueCode)
                {
                    Value = value.ToString();
                    return this;
                }

                var text = value.ToString();
                if (!string.IsNullOrEmpty(text))
                {
                    var subfield = new SubField { Code = code, Value = text };
                    Subfields.Add(subfield);
                }
            }

            return this;
        } // method AddNonEmpty

        /// <summary>
        /// Очистка подполей.
        /// </summary>
        public Field Clear()
        {
            Subfields.Clear();

            return this;
        } // method Clear

        /// <summary>
        /// Клонирование поля.
        /// </summary>
        public Field Clone()
        {
            var result = (Field) MemberwiseClone();

            for (var i = 0; i < Subfields.Count; i++)
            {
                Subfields[i] = Subfields[i].Clone();
            }

            return result;
        } // method Clone

        /// <summary>
        /// Декодирование строки.
        /// </summary>
        public void Decode
            (
                ReadOnlySpan<char> line
            )
        {
            var index = line.IndexOf('#');
            Tag = line.Slice(0, index).ParseInt32();
            line = line.Slice(index + 1);
            DecodeBody(line);
        } // method Decode

        /// <summary>
        /// Декодирование тела поля.
        /// </summary>
        public void DecodeBody
            (
                ReadOnlySpan<char> line
            )
        {
            var index = line.IndexOf('^');
            if (index < 0)
            {
                // TODO: реализовать оптимально
                Value = line.ToString();
                return;
            }

            if (index != 0)
            {
                // TODO: реализовать оптимально
                Value = line.Slice(0, index).ToString();
            }

            line = line.Slice(index + 1);

            while (true)
            {
                index = line.IndexOf('^');
                if (index < 0)
                {
                    Add(line[0], line.Slice(1).ToString());
                    return;
                }

                Add(line[0], line.Slice(1, index - 1).ToString());
                line = line.Slice(index + 1);
            }
        } // method DecodeBody

        /// <summary>
        /// Получение первого подполя с указанным кодом.
        /// </summary>
        public SubField? GetFirstSubField
            (
                char code
            )
        {
            if (code == ValueCode)
            {
                var firstSubfield = Subfields.FirstOrDefault();
                if (firstSubfield?.Code == ValueCode)
                {
                    return firstSubfield;
                }

                return null;
            }

            foreach (var subfield in Subfields)
            {
                if (subfield.Code.SameChar(code))
                {
                    return subfield;
                }
            }

            return null;
        } // method GetFirstSubField

        /// <summary>
        /// Перечисление подполей с указанным кодом.
        /// </summary>
        public IEnumerable<SubField> EnumerateSubFields
            (
                char code
            )
        {
            foreach (var subfield in Subfields)
            {
                if (subfield.Code.SameChar(code))
                {
                    yield return subfield;
                }
            }
        } // method EnumerateSubFields

        /// <summary>
        /// Получение всех подполей с указанным кодом.
        /// </summary>
        public SubField[] GetSubFields
            (
                char code
            )
        {
            var result = new List<SubField>();

            foreach (var subfield in Subfields)
            {
                if (subfield.Code.SameChar(code))
                {
                    result.Add(subfield);
                }
            }

            return result.ToArray();
        } // method GetSubFields

        /// <summary>
        /// Получение первого подполя с указанным кодом
        /// либо создание нового подполя, если таковое отсуствует.
        /// </summary>
        /// <param name="code">Искомый код подполя.</param>
        /// <returns>Найденное или созданное подполе.</returns>
        public SubField GetOrAddSubField
            (
                char code
            )
        {
            if (code == '\0')
            {

            }

            foreach (var subfield in Subfields)
            {
                if (subfield.Code.SameChar(code))
                {
                    return subfield;
                }
            }

            var result = new SubField { Code = code };
            Subfields.Add(result);

            return result;
        } // method GetOrAddSubField

        /// <summary>
        /// Указанное повторение подполя с данным кодом.
        /// </summary>
        /// <param name="code">Искомый код подполя.</param>
        /// <param name="occurrence">Номер повторения.
        /// Нумерация начинается с нуля.
        /// Отрицательные индексы отсчитываются с конца массива.</param>
        /// <returns>Найденное подполе или <c>null</c>.</returns>
        public SubField? GetSubField
            (
                char code,
                int occurrence = 0
            )
        {
            if (code == ValueCode)
            {
                if (occurrence != 0)
                {
                    return null;
                }

                return GetValueSubField();
            }

            if (occurrence < 0)
            {
                // отрицательные индексы отсчитываются от конца
                occurrence = Subfields.Count(sf => sf.Code.SameChar(code)) + occurrence;
                if (occurrence < 0)
                {
                    return null;
                }
            }

            foreach (var subfield in Subfields)
            {
                if (subfield.Code.SameChar(code))
                {
                    if (occurrence == 0)
                    {
                        return subfield;
                    }

                    --occurrence;
                }
            }

            return null;
        } // method GetSubField

        /// <summary>
        /// Получение текста указанного подполя.
        /// </summary>
        /// <param name="code">Искомый код подполя.</param>
        /// <param name="occurrence">Номер повторения.
        /// Нумерация начинается с нуля.
        /// Отрицательные индексы отсчитываются с конца массива.</param>
        /// <returns>Текст найденного подполя или <c>null</c>.</returns>
        public string? GetSubFieldValue
            (
                char code,
                int occurrence = 0
            )
            => GetSubField(code, occurrence)?.Value ?? default;

        /// <summary>
        /// For * specification.
        /// </summary>
        public string? GetValueOrFirstSubField()
            => Subfields.FirstOrDefault()?.Value ?? default;

        /// <summary>
        /// Установка значения подполя.
        /// </summary>
        /// <param name="code">Искомый код подполя.</param>
        /// <param name="value">Новое значение подполя.</param>
        /// <returns>this</returns>
        public Field SetSubFieldValue
            (
                char code,
                string? value
            )
        {
            if (code == ValueCode)
            {
                Value = value;
            }
            else
            {
                if (string.IsNullOrEmpty(value))
                {
                    RemoveSubField(code);
                }
                else
                {
                    GetOrAddSubField(code).Value = value;
                }
            }

            return this;
        } // method SetSubFieldValue

        /// <summary>
        /// Удаление подполей с указанным кодом.
        /// </summary>
        /// <param name="code">Искомый код подполя.</param>
        /// <returns>this</returns>
        public Field RemoveSubField
            (
                char code
            )
        {
            SubField? subfield;

            while ((subfield = GetFirstSubField(code)) is not null)
            {
                Subfields.Remove(subfield);
            }

            return this;
        } // method RemoveSubField

        /// <summary>
        /// Текстовое представление только значимой части поля.
        /// Метка поля не выводится.
        /// </summary>
        public string ToText()
        {
            var length = Subfields.Sum
                (
                    sf => (sf.Value!.Length)
                          + (sf.Code == ValueCode ? 1 : 2)
                );
            var result = new StringBuilder (length);

            // if (!string.IsNullOrEmpty(Value))
            // {
            //     result.Append(Value);
            // }

            foreach (var subField in Subfields)
            {
                var subText = subField.ToString();
                result.Append(subText);
            }

            return result.ToString();
        } // method ToText

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Tag = reader.ReadPackedInt32();
            Subfields.RestoreFromStream(reader);
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Sure.NotNull(writer, nameof(writer));

            writer.WritePackedInt32(Tag);
            Subfields.SaveToStream(writer);
        }

        #endregion

        #region IReadOnly<T> members

        /// <inheritdoc cref="IReadOnly{T}.AsReadOnly"/>
        public Field AsReadOnly()
        {
            var result = Clone();
            result.SetReadOnly();

            return result;
        } // method AsReadOnly

        /// <inheritdoc cref="IReadOnly{T}.ReadOnly"/>
        public bool ReadOnly { get; internal set; }

        /// <inheritdoc cref="IReadOnly{T}.SetReadOnly"/>
        public void SetReadOnly()
        {
            ReadOnly = true;

            foreach (var subfield in Subfields)
            {
                subfield.SetReadOnly();
            }
        } // method SetReadOnly

        /// <inheritdoc cref="IReadOnly{T}.ThrowIfReadOnly"/>
        public void ThrowIfReadOnly()
        {
            if (ReadOnly)
            {
                Magna.Error(nameof(ThrowIfReadOnly));

                throw new ReadOnlyException();
            }
        } // method ThrowIfReadOnly

        #endregion

        #region IEnumerable<SubField> members

        /// <inheritdoc cref="IEnumerable.GetEnumerator"/>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <inheritdoc cref="IEnumerable{T}.GetEnumerator"/>
        public IEnumerator<SubField> GetEnumerator() => Subfields.GetEnumerator();

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify"/>
        public bool Verify
            (
                bool throwOnError
            )
        {
            // TODO: удостовериться, что подполе-значение единственное и первое!

            var verifier = new Verifier<Field>(this, throwOnError);

            verifier.Positive(Tag, nameof(Tag));
            verifier.Positive(Subfields.Count, "Subfields.Count");
            foreach (var subfield in Subfields)
            {
                verifier.VerifySubObject(subfield);
            }

            return verifier.Result;
        } // method Verify

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            var length = 4 + Subfields.Sum
                (
                    sf => (sf.Value!.Length)
                    + (sf.Code == ValueCode ? 1 : 2)
                );
            var result = new StringBuilder (length);
            result.Append(Tag.ToInvariantString())
                .Append('#');
            foreach (var subfield in Subfields)
            {
                result.Append(subfield);
            }

            return result.ToString();
        } // method ToString

        #endregion

    } // class Field

} // namespace ManagedIrbis
