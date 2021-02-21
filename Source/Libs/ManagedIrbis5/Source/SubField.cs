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
        public string? Value { get; set; }

        /// <summary>
        /// Подполе хранит значение поля до первого разделителя.
        /// </summary>
        public bool RepresentsValue => Code == NoCode;

        public bool Modified { get; internal set; }

        /// <summary>
        /// Ссылка на поле.
        /// </summary>
        public Field? Field { get; internal set; }

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
            var result = subField1.Code.CompareTo(subField2.Code);
            if (result != 0)
            {
                return result;
            }

            result = string.CompareOrdinal
                (
                    subField1.Value,
                    subField2.Value
                );

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
                Code = char.ToLowerInvariant(text[0]);
                Value = text.Slice(1).ToString();
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
            Value = reader.ReadNullableString();
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            writer.Write(Code);
            writer.WriteNullable(Value);
        }

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
            if (!string.IsNullOrEmpty(Value))
            {
                verifier.Assert(!Value.Contains(Delimiter));
            }

            return verifier.Result;
        } // method Verify

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return Code == NoCode
                ? Value ?? string.Empty
                : "^" + char.ToLowerInvariant(Code) + Value;
        } // method ToString

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

    } // class SubField

} // namespace ManagedIrbis
