// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* QuarterlyOrderInfo.cs -- поквартальные сведения о заказах
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;
using AM.Collections;
using AM.IO;
using AM.Runtime;

using ManagedIrbis.Mapping;

#endregion

#nullable enable

namespace ManagedIrbis.Fields
{
    /// <summary>
    /// Поквартальные сведения о заказах, поле 938.
    /// </summary>
    [XmlRoot("order")]
    public sealed class QuarterlyOrderInfo
        : IHandmadeSerializable,
        IVerifiable
    {
        #region Constants

        /// <summary>
        /// Метка поля.
        /// </summary>
        public const int Tag = 938;

        /// <summary>
        /// Известные коды подполей.
        /// </summary>
        public const string KnownCodes = "abdenqvxy";

        #endregion

        #region Properties

        /// <summary>
        /// Период заказа. Подполе q.
        /// </summary>
        [SubField('q')]
        [XmlAttribute("period")]
        [JsonPropertyName("period")]
        public string? Period { get; set; }

        /// <summary>
        /// Число номеров. Подполе n.
        /// </summary>
        [SubField('n')]
        [XmlAttribute("issues")]
        [JsonPropertyName("issues")]
        public string? NumberOfIssues { get; set; }

        /// <summary>
        /// Первый номер. Подполе a.
        /// </summary>
        [SubField('a')]
        [XmlAttribute("first")]
        [JsonPropertyName("first")]
        public string? FirstIssue { get; set; }

        /// <summary>
        /// Последний номер. Подполе b.
        /// </summary>
        [SubField('b')]
        [XmlAttribute("last")]
        [JsonPropertyName("last")]
        public string? LastIssue { get; set; }

        /// <summary>
        /// Цена заказа. Подполе y.
        /// </summary>
        [SubField('y')]
        [XmlAttribute("totalPrice")]
        [JsonPropertyName("totalPrice")]
        public string? TotalPrice { get; set; }

        /// <summary>
        /// Цена номера по комплектам. Подполе e.
        /// </summary>
        [SubField('e')]
        [XmlAttribute("issuePrice")]
        [JsonPropertyName("issuePrice")]
        public string? IssuePrice { get; set; }

        /// <summary>
        /// Валюта. Подполе v.
        /// </summary>
        [SubField('v')]
        [XmlAttribute("currency")]
        [JsonPropertyName("currency")]
        public string? Currency { get; set; }

        /// <summary>
        /// Периодичность (код). Подполе d.
        /// </summary>
        [SubField('d')]
        [XmlAttribute("code")]
        [JsonPropertyName("code")]
        public string? PeriodicityCode { get; set; }

        /// <summary>
        /// Периодичность (код). Подполе x.
        /// </summary>
        [SubField('x')]
        [XmlAttribute("periodicity")]
        [JsonPropertyName("periodicity")]
        public string? PeriodicityNumber { get; set; }

        /// <summary>
        /// Неизвестные подполя.
        /// </summary>
        [Browsable(false)]
        [XmlElement("unknown")]
        [JsonPropertyName("unknown")]
        public SubField[]? UnknownSubfields { get; set; }

        /// <summary>
        /// Связанное поле.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        [Browsable(false)]
        public Field? Field { get; set; }

        /// <summary>
        /// Произвольные пользовательские данные.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        [Browsable(false)]
        public object? UserData { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Применение к уже имеющемуся полю.
        /// </summary>
        public Field ApplyTo (Field field) => field
            .SetSubFieldValue ('q', Period)
            .SetSubFieldValue ('n', NumberOfIssues)
            .SetSubFieldValue ('a', FirstIssue)
            .SetSubFieldValue ('b', LastIssue)
            .SetSubFieldValue ('y', TotalPrice)
            .SetSubFieldValue ('e', IssuePrice)
            .SetSubFieldValue ('v', Currency)
            .SetSubFieldValue ('d', PeriodicityCode)
            .SetSubFieldValue ('x', PeriodicityNumber);

        /// <summary>
        /// Разбор поля.
        /// </summary>
        public static QuarterlyOrderInfo Parse
            (
                Field field
            )
        {
            // TODO: реализовать эффективно

            var result = new QuarterlyOrderInfo
            {
                Period = field.GetFirstSubFieldValue('q'),
                NumberOfIssues = field.GetFirstSubFieldValue('n'),
                FirstIssue = field.GetFirstSubFieldValue('a'),
                LastIssue = field.GetFirstSubFieldValue('b'),
                TotalPrice = field.GetFirstSubFieldValue('y'),
                IssuePrice = field.GetFirstSubFieldValue('e'),
                Currency = field.GetFirstSubFieldValue('v'),
                PeriodicityCode = field.GetFirstSubFieldValue('d'),
                PeriodicityNumber = field.GetFirstSubFieldValue('x'),
                UnknownSubfields = field.Subfields.GetUnknownSubFields(KnownCodes),
                Field = field
            };

            return result;
        } // method Parse

        /// <summary>
        /// Разбор записи.
        /// </summary>
        public static QuarterlyOrderInfo[] Parse
            (
                Record record
            )
        {
            var result = new List<QuarterlyOrderInfo>();
            foreach (var field in record.Fields)
            {
                if (field.Tag == Tag)
                {
                    result.Add(Parse(field));
                }
            }

            return result.ToArray();
        } // method Parse

        /// <summary>
        /// Should serialize <see cref="UnknownSubfields"/> array?
        /// </summary>
        [ExcludeFromCodeCoverage]
        public bool ShouldSerializeUnknownSubfields() =>
            !UnknownSubfields.IsNullOrEmpty();

        /// <summary>
        /// Преобразование обратно в поле.
        /// </summary>
        public Field ToField()
        {
             var result = new Field (Tag)
                .AddNonEmpty ('q', Period)
                .AddNonEmpty ('n', NumberOfIssues)
                .AddNonEmpty ('a', FirstIssue)
                .AddNonEmpty ('b', LastIssue)
                .AddNonEmpty ('y', TotalPrice)
                .AddNonEmpty ('e', IssuePrice)
                .AddNonEmpty ('v', Currency)
                .AddNonEmpty ('d', PeriodicityCode)
                .AddNonEmpty ('x', PeriodicityNumber)
                .AddRange (UnknownSubfields);

            return result;
        } // class ToFields

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Period = reader.ReadNullableString();
            NumberOfIssues = reader.ReadNullableString();
            FirstIssue = reader.ReadNullableString();
            LastIssue = reader.ReadNullableString();
            TotalPrice = reader.ReadNullableString();
            IssuePrice = reader.ReadNullableString();
            Currency = reader.ReadNullableString();
            PeriodicityCode = reader.ReadNullableString();
            PeriodicityNumber = reader.ReadNullableString();
            UnknownSubfields = reader.ReadNullableArray<SubField>();
        } // method RestoreFromStream

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            writer
                .WriteNullable(Period)
                .WriteNullable(NumberOfIssues)
                .WriteNullable(FirstIssue)
                .WriteNullable(LastIssue)
                .WriteNullable(TotalPrice)
                .WriteNullable(IssuePrice)
                .WriteNullable(Currency)
                .WriteNullable(PeriodicityCode)
                .WriteNullable(PeriodicityNumber)
                .WriteNullableArray(UnknownSubfields);
        } // method SaveToStream

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify" />
        public bool Verify
            (
                bool throwOnError
            )
        {
            Verifier<QuarterlyOrderInfo> verifier
                = new Verifier<QuarterlyOrderInfo>(this, throwOnError);

            verifier
                .NotNullNorEmpty(Period, "Period")
                .NotNullNorEmpty(FirstIssue, "FirstIssue")
                .NotNullNorEmpty(LastIssue, "LastIssue");

            return verifier.Result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString() => Period.ToVisibleString();

        #endregion

    } // class QuarterlyOrderInfo

} // namespace ManagedIrbis.Fields
