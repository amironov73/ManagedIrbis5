// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CollectionNeverUpdated.Global
// ReSharper disable CommentTypo
// ReSharper disable ForCanBeConvertedToForeach
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable NonReadonlyMemberInGetHashCode
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global

/* SubField.cs -- подполе библиографической записи
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

namespace ManagedIrbis
{
    /// <summary>
    /// Подполе библиографической записи.
    /// </summary>
    public sealed class SubField
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
            set => SetValue (value);
        }

        /// <summary>
        /// Подполе хранит значение поля до первого разделителя.
        /// </summary>
        public bool RepresentsValue => Code == NoCode;

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        public SubField()
        {
            // пустое тело конструктора
        }

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
            Utility.VerifySubFieldCode (code);
            Code = code;
            Utility.VerifySubFieldValue (value.Span);
            Value = value.ToString();
        }

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
            Utility.VerifySubFieldCode (code);
            Code = code;
            Utility.VerifySubFieldValue (value.AsSpan());
            Value = value;
        }

        #endregion

        #region Private members

        private string? _value;

        #endregion

        #region Public methods

        /// <summary>
        /// Клонирование подполя.
        /// </summary>
        public SubField Clone() => (SubField)MemberwiseClone();

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
            var result = char.ToUpperInvariant (subField1.Code)
                .CompareTo (char.ToUpperInvariant (subField2.Code));
            if (result != 0)
            {
                return result;
            }

            result = string.CompareOrdinal (subField1.Value, subField2.Value);

            return result;
        }

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
                var code = char.ToLowerInvariant (text[0]);
                Utility.VerifySubFieldCode (code);
                Code = code;
                var value = text.Slice (1);
                Utility.VerifySubFieldValue (value);
                Value = value.EmptyToNull();
            }
        }

        /// <summary>
        /// Установка нового значения подполя.
        /// </summary>
        public void SetValue
            (
                ReadOnlySpan<char> value
            )
        {
            Utility.VerifySubFieldValue (value);
            _value = value.ToString();
        }

        /// <summary>
        /// Установка нового значения подполя.
        /// </summary>
        public void SetValue
            (
                string? value
            )
        {
            Utility.VerifySubFieldValue (value.AsSpan());
            _value = value;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString() => Code == NoCode
            ? Value ?? string.Empty
            : "^" + char.ToLowerInvariant (Code) + Value;

        #endregion
    }
}
