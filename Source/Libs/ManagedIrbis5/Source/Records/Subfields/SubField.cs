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
using System.Xml;
using System.Xml.Schema;
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
    [XmlRoot("subfield")]
    public sealed class SubField
        : IVerifiable,
        IXmlSerializable,
        IHandmadeSerializable,
        IReadOnly<SubField>
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
        /// Разделитель подполей.
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
        public string? Value
        {
            get => _value;
            set => SetValue(value);
        }

        /// <summary>
        /// Подполе хранит значение поля до первого разделителя.
        /// </summary>
        public bool RepresentsValue => Code == NoCode;

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
            if (value.Span.Contains(Delimiter))
            {
                throw new ArgumentException(nameof(value));
            }

            Value = value.ToString();
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
            Value = value;
        } // constructor

        #endregion

        #region Private members

        private string? _value;

        #endregion

        #region Public methods

        /// <summary>
        /// Клонирование подполя.
        /// </summary>
        public SubField Clone() => (SubField) MemberwiseClone();

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

            result = string.CompareOrdinal(subField1.Value, subField2.Value);

            return result;
        } // method Compare

        /// <summary>
        /// Декодирование строки.
        /// </summary>
        public void Decode
            (
                ReadOnlySpan<char> text
            )
        {
            if (!text.IsEmpty)
            {
                var code = char.ToLowerInvariant(text[0]);
                SubFieldCode.Verify(code, true);
                Code = code;
                var value = text[1..];
                SubFieldValue.Verify(value, true);
                Value = value.EmptyToNull();
            }

        } // method Decode

        /// <summary>
        /// Установка нового значения подполя.
        /// </summary>
        public void SetValue
            (
                ReadOnlySpan<char> value
            )
        {
            ThrowIfReadOnly();
            SubFieldValue.Verify(value, true);
            _value = value.ToString();

        } // method SetValue

        /// <summary>
        /// Установка нового значения подполя.
        /// </summary>
        public void SetValue
            (
                string? value
            )
        {
            ThrowIfReadOnly();
            SubFieldValue.Verify(value, true);
            _value = value;

        } // method SetValue

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Code = reader.ReadChar();
            Value = reader.ReadNullableString();

        } // method RestoreFromStream

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            writer.Write(Code);
            writer.WriteNullable(Value);

        } // method SaveToStream

        #endregion

        #region IXmlSerializable members

        /// <inheritdoc cref="IXmlSerializable.GetSchema"/>
        XmlSchema? IXmlSerializable.GetSchema() => null;

        /// <inheritdoc cref="IXmlSerializable.ReadXml"/>
        void IXmlSerializable.ReadXml
            (
                XmlReader reader
            )
        {
            Code = reader.GetAttribute("code").FirstChar();
            Value = reader.GetAttribute("value");

        } // method ReadXml

        /// <inheritdoc cref="IXmlSerializable.WriteXml"/>
        void IXmlSerializable.WriteXml
            (
                XmlWriter writer
            )
        {
            writer.WriteAttributeString("code", Code.ToString());
            writer.WriteAttributeString("value", Value);

        } // method WriteXml

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify"/>
        public bool Verify
            (
                bool throwOnError
            )
        {
            var verifier = new Verifier<SubField>(this, throwOnError);

            verifier.Assert
                (
                    Code is NoCode or > ' ',
                    $"Wrong subfield code {Code}"
                );

            verifier.Assert
                (
                    SubFieldValue.Verify(Value, throwOnError)
                );

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

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString() =>
            Code == NoCode
                ? Value ?? string.Empty
                : "^" + char.ToLowerInvariant(Code) + Value;

        #endregion

    } // class SubField

} // namespace ManagedIrbis
