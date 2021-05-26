﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* GlobalCounter.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;
using AM.IO;
using AM.Runtime;
using AM.Text;

using ManagedIrbis.Mapping;

#endregion

#nullable enable

namespace ManagedIrbis.Fields
{
    //
    // Для хранения глобальных счетчиков служит специальная база
    // данных с именем COUNT (зарезервированное имя).
    // Каждая запись базы данных служит для хранения и описания
    // одного глобального счетчика и содержит три обязательных поля:
    //
    // * Индекс глобального счетчика - уникальный идентификатор
    // счетчика (в простейшем случае - номер); метка поля - 1;
    //
    // * Текущее значение глобального счетчика; метка поля - 2;
    //
    // * Шаблон глобального счетчика - определяет структуру (маску)
    // счетчика; метка поля - 3.
    //
    // Шаблон глобального счетчика в общем случае может содержать
    // три части:
    //
    // * Префиксная часть - любой набор символов (кроме символа *),
    // в частном случае может отсутствовать;
    //
    // * Числовая часть - обязательная, обозначается символами *;
    //
    // * Суффиксная часть - любой набор символов (кроме символа *),
    // в частном случае может отсутствовать.
    //
    // Если числовая часть счетчика не имеет фиксированной длины
    // (т.е. не имеет лидирующих нулей), то она обозначается одним
    // символом *. Если числовая часть имеет фиксированную длину
    // (с лидирующими нулями), то она обозначается соответствующим
    // количеством символов *.

    /// <summary>
    /// Экземпляр глобального счетчика.
    /// </summary>
    [XmlRoot("counter")]
    public sealed class GlobalCounter
        : IHandmadeSerializable,
        IVerifiable
    {
        #region Properties

        /// <summary>
        /// Индекс глобального счетчика. Поле 1.
        /// </summary>
        [Field(1)]
        [XmlAttribute("index")]
        [JsonPropertyName("index")]
        public string? Index { get; set; }

        /// <summary>
        /// Текущее значение глобального счетчика. Поле 2.
        /// </summary>
        [Field(2)]
        [XmlAttribute("value")]
        [JsonPropertyName("value")]
        public string? Value { get; set; }

        /// <summary>
        /// Numeric value of the <see cref="Value"/> property.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public int NumericValue
        {
            get
            {
                var template = Template;
                var value = Value;
                if (string.IsNullOrEmpty(template) || string.IsNullOrEmpty(value))
                {
                    return value.SafeToInt32();
                }

                var navigator = new TextNavigator(template);
                var prefix = navigator.ReadUntil("*").ToString();

                if (!value.StartsWith(prefix))
                {
                    return value.SafeToInt32();
                }

                navigator.ReadWhile('*');
                var suffix = navigator.GetRemainingText().ToString();

                navigator = new TextNavigator(value);
                navigator.SkipChar(prefix.Length);
                value = navigator.GetRemainingText().ToString();
                value = value.Length < suffix.Length
                    ? string.Empty
                    : value.Substring(0, value.Length - suffix.Length);

                return value.SafeToInt32();
            }
            set
            {
                var correctValue = value < 0 ? 0 : value;

                var template = Template;
                if (string.IsNullOrEmpty(template))
                {
                    Value = correctValue.ToInvariantString();
                    return;
                }

                var navigator = new TextNavigator(template);
                var prefix = navigator.ReadUntil("*").ToString();
                var stars = navigator.ReadWhile('*').ToString();
                var width = stars.Length;
                var suffix = navigator.GetRemainingText().ToString();

                Value = prefix
                      + correctValue.ToInvariantString().PadLeft(width, '0')
                      + suffix;
            }
        }

        /// <summary>
        /// Шаблон глобального счетчика. Поле 3.
        /// </summary>
        [Field(3)]
        [XmlAttribute("template")]
        [JsonPropertyName("template")]
        public string? Template { get; set; }

        /// <summary>
        /// Associated record.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        [Browsable(false)]
        public Record? Record { get; set; }

        /// <summary>
        /// Arbitrary user data.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        [Browsable(false)]
        public object? UserData { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Apply the counter to the <see cref="Record"/>.
        /// </summary>
        public void ApplyTo
            (
                Record record
            )
        {
            record.Fields
                .ApplyFieldValue(1, Index)
                .ApplyFieldValue(2, Value)
                .ApplyFieldValue(3, Template);
        }

        /// <summary>
        /// Increment the counter value.
        /// </summary>
        public GlobalCounter Increment
            (
                int delta
            )
        {
            var value = NumericValue;
            value += delta;
            NumericValue = value;

            return this;
        }

        /// <summary>
        /// Parse the <see cref="Record"/>.
        /// </summary>
        public static GlobalCounter Parse
            (
                Record record
            )
        {
            // TODO: реализовать оптимально

            var result = new GlobalCounter
            {
                Index = record.FM(1),
                Value = record.FM(2),
                Template = record.FM(3),
                Record = record
            };

            return result;
        }

        /// <summary>
        /// Should serialize the <see cref="Index"/> field?
        /// </summary>
        [ExcludeFromCodeCoverage]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeIndex() => !string.IsNullOrEmpty(Index);

        /// <summary>
        /// Should serialize the <see cref="Template"/> field?
        /// </summary>
        [ExcludeFromCodeCoverage]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeTemplate() => !string.IsNullOrEmpty(Template);

        /// <summary>
        /// Should serialize the <see cref="Value"/> field?
        /// </summary>
        [ExcludeFromCodeCoverage]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeValue()
        {
            return !string.IsNullOrEmpty(Value);
        }

        /// <summary>
        /// Convert the counter back to the <see cref="Record"/>.
        /// </summary>
        public Record ToRecord() => new Record()
                .AddNonEmptyField(1, Index)
                .AddNonEmptyField(2, Value)
                .AddNonEmptyField(3, Template);

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Index = reader.ReadNullableString();
            Value = reader.ReadNullableString();
            Template = reader.ReadNullableString();
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            writer
                .WriteNullable(Index)
                .WriteNullable(Value)
                .WriteNullable(Template);
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify" />
        public bool Verify
            (
                bool throwOnError
            )
        {
            var verifier
                = new Verifier<GlobalCounter>(this, throwOnError);

            verifier
                .NotNullNorEmpty(Index, "Index")
                .NotNullNorEmpty(Value, "Value")
                .NotNullNorEmpty(Template, "Template");

            return verifier.Result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return Index.ToVisibleString() + ":" + Value.ToVisibleString();
        }

        #endregion
    }
}
