// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global

/* SubField.cs -- подполе библиографической записи
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;
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
    /// Подполе библиографической записи.
    /// </summary>
    public sealed class SubField
        : IVerifiable,
        IHandmadeSerializable,
        IReadOnly<SubField>,
        IDisposable
    {
        #region Constants

        /// <summary>
        /// Нет кода подполя, т. е. код пока не задан.
        /// Также используется для обозначения, что подполе
        /// используется для хранения значения поля
        /// до первого разделителя.
        /// </summary>
        public const char NoCode = '\0';

        /// <summary>
        /// Subfield delimiter.
        /// </summary>
        public const char Delimiter = '^';

        #endregion

        #region Properties

        /// <summary>
        /// Код подполя.
        /// </summary>
        public char Code { get; set; } = NoCode;

        /// <summary>
        /// Значение подполя.
        /// </summary>
        public ReadOnlyMemory<char> Value { get; set; }

        /// <summary>
        /// Подполе хранит значение поля до первого разделителя.
        /// </summary>
        public bool RepresentsValue => Code == NoCode;

        /// <summary>
        /// Подполе модифицировано?
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public bool Modified { get; internal set; }

        /// <summary>
        /// Ссылка на поле.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public Field? Field { get; internal set; }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        public SubField()
        {
        } // constructor

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="code">Код подполя.</param>
        /// <param name="value">Значение подполя (опционально).</param>
        public SubField
            (
                char code,
                ReadOnlyMemory<char> value = default
            )
        {
            Code = code;
            Value = value;
        } // constructor

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="code">Код подполя.</param>
        /// <param name="value">Значение подполя (опционально).</param>
        public SubField
            (
                char code,
                string? value
            )
        {
            Code = code;
            Value = value.AsMemory();
        } // constructor

        #endregion

        #region Public methods

        /// <summary>
        /// Клонирование подполя.
        /// </summary>
        public SubField Clone()
        {
            return (SubField) MemberwiseClone();
        } // method Clone

        /// <summary>
        /// Сравнение двух подполей.
        /// </summary>
        public static int Compare
            (
                SubField subField1,
                SubField subField2
            )
        {
            // сравниваем коды подполей с точностью до регистра символов
            var result = char.ToUpperInvariant(subField1.Code)
                .CompareTo(char.ToUpperInvariant(subField2.Code));
            if (result != 0)
            {
                return result;
            }

            result = subField1.Value.Span.CompareTo(subField2.Value.Span, StringComparison.Ordinal);

            return result;
        } // method Compare

        /// <summary>
        /// Декодирование строки.
        /// </summary>
        public void Decode
            (
                ReadOnlyMemory<char> text
            )
        {
            if (!text.IsEmpty)
            {
                Code = char.ToLowerInvariant(text.Span[0]);
                Value = text.Slice(1);
            }
        } // method Decode

        /// <summary>
        /// Получение подполя из пула.
        /// </summary>
        /// <returns>Объект из пула.</returns>
        public static SubField FromPool() => SubFieldPool.Default.Get();

        /// <summary>
        /// Возврат объекта в пул.
        /// </summary>
        public void ToPool() => SubFieldPool.Default.Return(this);

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Code = reader.ReadChar();
            Value = reader.ReadNullableString().AsMemory();
        } // method RestoreFromStream

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            writer.Write(Code);
            Value.SaveToStream(writer);
        } // method SaveToStream

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify"/>
        public bool Verify
            (
                bool throwOnError
            )
        {
            var verifier = new Verifier<SubField>(this, throwOnError);

            verifier.Assert(Code == NoCode || Code > ' ', "Wrong Code");
            if (!Value.IsEmpty)
            {
                verifier.Assert(!Value.Span.Contains(Delimiter));
            }

            return verifier.Result;
        } // method Verify

        #endregion

        #region IReadOnly<T> members

        /// <inheritdoc cref="IReadOnly{T}.AsReadOnly"/>
        public SubField AsReadOnly()
        {
            var result = Clone();
            result.SetReadOnly();

            return result;
        } // method AsReadOnly

        /// <inheritdoc cref="IReadOnly{T}.ReadOnly"/>
        [XmlIgnore]
        [JsonIgnore]
        public bool ReadOnly { get; private set; }

        /// <inheritdoc cref="IReadOnly{T}.SetReadOnly"/>
        public void SetReadOnly() => ReadOnly = true;

        /// <inheritdoc cref="IReadOnly{T}.ThrowIfReadOnly"/>
        public void ThrowIfReadOnly()
        {
            if (ReadOnly)
            {
                throw new ReadOnlyException();
            }
        } // method ThrowIfReadOnly

        #endregion

        #region IDisposable members

        /// <summary>
        /// Очистка поля перед помещением его в пул.
        /// </summary>
        /// <remarks>
        /// <para>Начиная с ManagedIrbis5, подполе поддерживает пулинг,
        /// см. класс <see cref="SubFieldPool"/>.</para>
        /// <para>Очистка перед помещением в пул выполняется с помощью
        /// вызова <see cref="Dispose"/>.</para>
        /// <para>При обычном использовании вызывать метод
        /// <see cref="Dispose"/> не нужно.</para>
        /// </remarks>
        public void Dispose()
        {
            Code = NoCode;
            Value = null;
            Field = null;
        } // method Dispose

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return Code == NoCode
                ? Value.ToString()
                : "^" + char.ToLowerInvariant(Code) + Value;
        } // method ToString

        #endregion

    } // class SubField

} // namespace ManagedIrbis
