// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* RecordField.cs -- поле библиографической записи
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AM;

#endregion

#nullable enable

namespace ManagedIrbis
{
    /// <summary>
    /// Поле библиографической записи.
    /// </summary>
    public class Field
    {
        #region Properties

        /// <summary>
        /// Метка поля.
        /// </summary>
        public int Tag { get; set; } = 0;

        /// <summary>
        /// Значение поля до первого разделителя.
        /// </summary>
        public string? Value { get; set; }

        /// <summary>
        /// Список подполей.
        /// </summary>
        public List<SubField> Subfields { get; } = new ();

        #endregion

        #region Public methods

        /// <summary>
        /// Добавление подполя в конец списка подполей.
        /// </summary>
        public Field Add
            (
                char code,
                object? value
            )
        {
            var text = value?.ToString();
            var subfield = new SubField { Code = code, Value = text };
            Subfields.Add(subfield);

            return this;
        } // method Add

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
            if (!ReferenceEquals(value, null))
            {
                var text = value.ToString();
                if (!string.IsNullOrEmpty(text))
                {
                    var subfield = new SubField {Code = code, Value = text};
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
            Value = null;
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
        /// Перечень подполей с указанным кодом.
        /// </summary>
        /// <param name="code">Искомый код подполя.</param>
        /// <remarks>Сравнение кодов происходит без учета
        /// регистра символов.</remarks>
        /// <returns>Найденные подполя.</returns>
        public SubField[] GetSubField
            (
                char code
            )
        {
            SubField[] result = Subfields
                .Where(_ => _.Code.SameChar(code))
                .ToArray();

            return result;
        } // method GetSubField

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
                int occurrence
            )
        {
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
                int occurrence
            )
        {
            var result = GetSubField(code, occurrence);

            return result?.Value;
        } // method GetSubFieldValue

        /// <summary>
        /// Получает значение первого появления подполя
        /// с указанным кодом.
        /// </summary>
        public string? GetFirstSubFieldValue
            (
                char code
            )
        {
            var result = GetFirstSubField(code);

            return result?.Value;
        } // method GetFirstSubFieldValue

        /// <summary>
        /// For * specification.
        /// </summary>
        public string? GetValueOrFirstSubField()
        {
            var result = Value;
            if (string.IsNullOrEmpty(result))
            {
                result = Subfields.FirstOrDefault()?.Value;
            }

            return result;
        } // method GetValueOrFirstSubField

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            int length = 4 + (Value?.Length ?? 0)
                           + Subfields.Sum(sf => (sf.Value?.Length ?? 0) + 2);
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
    }
}
