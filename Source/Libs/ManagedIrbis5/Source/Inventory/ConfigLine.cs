// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global

/* ConfigLine.cs -- одна строка в базе CONFIG
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.ComponentModel;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;
using AM.Text;

using ManagedIrbis.Mapping;

#endregion

#nullable enable

namespace ManagedIrbis.Inventory
{
    /// <summary>
    /// Одна строка в базе данных CONFIG.
    /// Соответствует одному повторению поля 100.
    /// Представляет собой диапазон инвентарных номеров.
    /// </summary>
    public sealed class ConfigLine
        : IVerifiable
    {
        #region Constants

        /// <summary>
        /// Рабочий лист, означающий "книги находятся в обработке".
        /// </summary>
        public const string ObrabWorksheet = "OBRAB";

        /// <summary>
        /// Метка поля.
        /// </summary>
        public const int Tag = 100;

        /// <summary>
        /// Известные коды подполей.
        /// </summary>
        public const string KnownCodes = "abcd";

        #endregion

        #region Properties

        /// <summary>
        /// Начальный инвентарный номер. Подполе A.
        /// </summary>
        [SubField ('a')]
        [JsonPropertyName ("from")]
        [XmlAttribute ("from")]
        [Description ("Начальный инвентарный номер")]
        public string? FromNumber { get; set; }

        /// <summary>
        /// Конечный инвентарный номер (включая). Подполе B.
        /// </summary>
        [SubField ('b')]
        [JsonPropertyName ("to")]
        [XmlAttribute ("to")]
        [Description ("Конечный инвентарный номер (включая)")]
        public string? ToNumber { get; set; }

        /// <summary>
        /// Номер партии книг.
        /// </summary>
        [SubField ('c')]
        [JsonPropertyName ("part")]
        [XmlAttribute ("part")]
        [Description ("Номер партии книг")]
        public string? PartNumber { get; set; }

        /// <summary>
        /// Примечания к партии.
        /// </summary>
        [SubField ('d')]
        [JsonPropertyName ("comment")]
        [XmlAttribute ("comment")]
        public string? Comment { get; set; }

        /// <summary>
        /// Ассоциированное поле.
        /// </summary>
        [JsonIgnore]
        [XmlIgnore]
        [Browsable (false)]
        [EditorBrowsable (EditorBrowsableState.Advanced)]
        public Field? Field { get; set; }

        /// <summary>
        /// Произвольные пользовательские данные.
        /// </summary>
        [JsonIgnore]
        [XmlIgnore]
        [Browsable (false)]
        [EditorBrowsable (EditorBrowsableState.Advanced)]
        public object? UserData { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Проверка, содержит ли данный диапазон указанный номер.
        /// Пустой номер не считается входящим в диапазон.
        /// </summary>
        public bool ContainsNumber (string? text) => ContainsNumber (new NumberText (text));

        /// <summary>
        /// Проверка, содержит ли данный диапазон указанный номер.
        /// Пустой номер не считается входящим в диапазон.
        /// </summary>
        public bool ContainsNumber
            (
                NumberText number
            )
        {
            if (number.Empty)
            {
                return false;
            }

            var result = !string.IsNullOrEmpty (FromNumber)
                && number.CompareTo (FromNumber) >= 0;

            if (!string.IsNullOrEmpty (ToNumber))
            {
                result &= number.CompareTo (ToNumber) <= 0;
            }

            return result;

        } // method ContainsNumber

        /// <summary>
        /// Разбор поля.
        /// </summary>
        public static ConfigLine Parse (Field field) => new ()
            {
                FromNumber = field.GetFirstSubFieldValue ('a'),
                ToNumber = field.GetFirstSubFieldValue ('b'),
                PartNumber = field.GetFirstSubFieldValue ('c'),
                Comment = field.GetFirstSubFieldValue ('d'),
                Field = field
            };  // method Parse

        /// <summary>
        /// Преобразование в поле.
        /// </summary>
        public Field ToField() => new Field (Tag)
                .AddNonEmpty ('a', FromNumber)
                .AddNonEmpty ('b', ToNumber)
                .AddNonEmpty ('c', PartNumber)
                .AddNonEmpty ('d', Comment);

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify"/>
        public bool Verify
            (
                bool throwOnError
            )
        {
            var verifier = new Verifier<ConfigLine>(this, throwOnError);

            // свойство Comment может отсутствовать - это нормально

            verifier
                .NotNullNorEmpty (FromNumber)
                .NotNullNorEmpty (ToNumber)
                .NotNullNorEmpty (PartNumber);

            return verifier.Result;

        } // method Verify

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString() =>
            $"{FromNumber.ToVisibleString()} - {ToNumber.ToVisibleString()}: {PartNumber.ToVisibleString()}";

        #endregion

    } // class ConfigLine

} // namespace ManagedIrbis.Inventory
