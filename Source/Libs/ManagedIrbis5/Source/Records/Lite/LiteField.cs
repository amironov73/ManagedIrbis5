// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* LiteField.cs -- облегченное поле библиографической записи
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
    /// Облегченное поле библиографической записи.
    /// </summary>
    public sealed class LiteField
    {
        #region Properties

        /// <summary>
        /// Метка поля.
        /// </summary>
        public int Tag { get; set; }

        /// <summary>
        /// Значение поля до первого разделителя.
        /// </summary>
        public ReadOnlyMemory<byte> Value => GetFirstSubFieldValue('\0');

        /// <summary>
        /// Список подполей.
        /// </summary>
        public List<LiteSubField> Subfields { get; } = new ();

        #endregion

        #region Public methods

        /// <summary>
        /// Добавление подполя с указанными кодом и значением
        /// в конец списка подполей.
        /// </summary>
        public LiteField Add
            (
                byte code,
                ReadOnlyMemory<byte> value
            )
        {
            var subfield = new LiteSubField { Code = code, Value = value };
            Subfields.Add(subfield);

            return this;
        } // method Add

        /// <summary>
        /// Добавление подполя в конец списка подполей
        /// при условии, что значение поля не пустое.
        /// </summary>
        public LiteField AddNonEmpty
            (
                byte code,
                ReadOnlyMemory<byte> value
            )
        {
            if (!value.IsEmpty)
            {
                var subfield = new LiteSubField {Code = code, Value = value};
                Subfields.Add(subfield);
            }

            return this;
        } // method AddNonEmpty

        /// <summary>
        /// Очистка подполей.
        /// </summary>
        public LiteField Clear()
        {
            Subfields.Clear();

            return this;
        } // method Clear

        /// <summary>
        /// Клонирование поля.
        /// </summary>
        public LiteField Clone()
        {
            var result = (LiteField) MemberwiseClone();

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
                ReadOnlyMemory<byte> line
            )
        {
            var span = line.Span;
            var index = span.IndexOf((byte)'#');
            Tag = FastNumber.ParseInt32(line.Slice(0, index));
            line = line.Slice(index + 1);
            DecodeBody(line);
        } // method Decode

        /// <summary>
        /// Декодирование тела поля.
        /// </summary>
        public void DecodeBody
            (
                ReadOnlyMemory<byte> line
            )
        {
            while (!line.IsEmpty)
            {
                var span = line.Span;
                var index = span.IndexOf(LiteSubField.Delimiter);
                var subField = new LiteSubField();
                if (index < 0)
                {
                    subField.Decode(line);
                }
                else
                {
                    subField.Decode(line.Slice(0, index));
                    line = line.Slice(index + 1);
                }
                Subfields.Add(subField);
            }
        } // method DecodeBody

        /// <summary>
        /// Получение первого подполя с указанным кодом.
        /// </summary>
        public LiteSubField? GetFirstSubField
            (
                char code
            )
        {
            var byteCode = (byte)Char.ToLowerInvariant(code);
            foreach (var subfield in Subfields)
            {
                if (subfield.Code == byteCode)
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
        public LiteSubField[] GetSubField
            (
                char code
            )
        {
            var byteCode = (byte)char.ToLowerInvariant(code);
            List<LiteSubField> result = new ();
            foreach (var subField in Subfields)
            {
                if (subField.Code == byteCode)
                {
                    result.Add(subField);
                }
            }

            return result.ToArray();
        } // method GetSubField

        /// <summary>
        /// Указанное повторение подполя с данным кодом.
        /// </summary>
        /// <param name="code">Искомый код подполя.</param>
        /// <param name="occurrence">Номер повторения.
        /// Нумерация начинается с нуля.
        /// Отрицательные индексы отсчитываются с конца массива.</param>
        /// <returns>Найденное подполе или <c>null</c>.</returns>
        public LiteSubField? GetSubField
            (
                char code,
                int occurrence
            )
        {
            var byteCode = (byte)char.ToLowerInvariant(code);
            foreach (var subfield in Subfields)
            {
                if (subfield.Code == byteCode)
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
        public ReadOnlyMemory<byte> GetSubFieldValue
            (
                char code,
                int occurrence
            )
        {
            var result = GetSubField(code, occurrence);

            return result?.Value ?? ReadOnlyMemory<byte>.Empty;
        } // method GetSubFieldValue

        /// <summary>
        /// Получает значение первого появления подполя
        /// с указанным кодом.
        /// </summary>
        public ReadOnlyMemory<byte> GetFirstSubFieldValue
            (
                char code
            )
        {
            var result = GetFirstSubField(code);

            return result?.Value ?? ReadOnlyMemory<byte>.Empty;
        } // method GetFirstSubFieldValue

        /// <summary>
        /// For * specification.
        /// </summary>
        public ReadOnlyMemory<byte> GetValueOrFirstSubField()
        {
            var result = Value;
            if (result.IsEmpty && Subfields.Count != 0)
            {
                result = Subfields.FirstOrDefault()?.Value
                         ?? ReadOnlyMemory<byte>.Empty;
            }

            return result;
        } // method GetValueOrFirstSubField

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            var result = new StringBuilder ();
            result.Append(Tag.ToInvariantString())
                .Append('#');
            foreach (var subfield in Subfields)
            {
                result.Append(subfield);
            }

            return result.ToString();
        } // method ToString

        #endregion

    } // class LiteField

} // namespace ManagedIrbis
