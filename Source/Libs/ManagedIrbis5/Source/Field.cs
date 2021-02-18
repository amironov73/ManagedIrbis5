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

/* Field.cs -- поле библиографической записи
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;

using AM;
using AM.IO;

using ManagedIrbis.Infrastructure;

#endregion

#nullable enable

namespace ManagedIrbis
{
    /// <summary>
    /// Поле библиографической записи.
    /// </summary>
    public class Field
        // : IHandmadeSerializable
    {
        #region Constants

        /// <summary>
        /// Специальный код, зарезервированный для
        /// значения поля до первого разделителя.
        /// </summary>
        private const char ValueCode = '\0';

        #endregion

        #region Properties

        /// <summary>
        /// Метка поля.
        /// </summary>
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
            get => GetValueSubField()?.Value;
            set
            {
                if (value is null)
                {
                    var valueSubfield = GetValueSubField();
                    if (valueSubfield is not null)
                    {
                        Subfields.Remove(valueSubfield);
                    }
                }
                else
                {
                    CreateValueSubField().Value = value;
                }
            } // set
        } // property Value

        /// <summary>
        /// Список подполей.
        /// </summary>
        public List<SubField> Subfields { get; } = new ();

        #endregion

        #region Public methods

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
        }

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
        }

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
        }

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
            Tag = int.Parse(line.Slice(0, index));
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
                Value = line.ToString();
                return;
            }

            if (index != 0)
            {
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
            => GetSubField(code, occurrence)?.Value;

        /// <summary>
        /// For * specification.
        /// </summary>
        public string? GetValueOrFirstSubField()
            => Subfields.FirstOrDefault()?.Value;

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

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Tag = reader.ReadPackedInt32();
            Value = reader.ReadNullableString();
            //Subfields.RestoreFromStream(reader);
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Sure.NotNull(writer, nameof(writer));

            writer.WritePackedInt32(Tag);
            writer.WriteNullable(Value);
            //Subfields.SaveToStream(writer);
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            var length = 4 + Subfields.Sum
                (
                    sf => (sf.Value?.Length ?? 0)
                    + (sf.Code == ValueCode ? 1 : 2)
                );
            var result = new StringBuilder (length);
            result.Append(Tag.ToInvariantString())
                .Append('#')
                .Append(Value);
            foreach (var subfield in Subfields)
            {
                result.Append(subfield);
            }

            return result.ToString();
        } // method ToString

        #endregion

    } // class Field

} // namespace ManagedIrbis
