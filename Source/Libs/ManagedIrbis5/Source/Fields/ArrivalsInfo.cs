// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* ArrivalsInfo.cs -- информация о поступлениях в базе CMPL
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
using AM.IO;
using AM.Runtime;

using ManagedIrbis.Mapping;

#endregion

#nullable enable

namespace ManagedIrbis.Fields
{
    /// <summary>
    /// Число наименований, поступивших впервые (на баланс,
    /// не на баланс, учебников), поле 17 в БД CMPL.
    /// </summary>
    [XmlRoot("arrivals")]
    public sealed class ArrivalsInfo
        : IHandmadeSerializable,
        IVerifiable
    {
        #region Constants

        /// <summary>
        /// Tag number.
        /// </summary>
        public const int Tag = 17;

        /// <summary>
        /// Known subfield codes.
        /// </summary>
        public const string KnownCodes = "ab123";

        #endregion

        #region Properties

        /// <summary>
        /// Поступило впервые на баланс (без периодики). Подполе 1.
        /// </summary>
        [SubField('1')]
        [XmlAttribute("onBalanceWithoutPeriodicals")]
        [JsonPropertyName("onBalanceWithoutPeriodicals")]
        [Description("Поступило впервые на баланс (без периодики)")]
        [DisplayName("Поступило впервые на баланс (без периодики)")]
        public string? OnBalanceWithoutPeriodicals { get; set; }

        /// <summary>
        /// Поступило впервые не на баланс (без периодики). Подполе b.
        /// </summary>
        [SubField('b')]
        [XmlAttribute("offBalanceWithoutPeriodicals")]
        [JsonPropertyName("offBalanceWithoutPeriodicals")]
        [Description("Поступило впервые не на баланс (без периодики)")]
        [DisplayName("Поступило впервые не на баланс (без периодики)")]
        public string? OffBalanceWithoutPeriodicals { get; set; }

        /// <summary>
        /// Поступило впервые всего (без периодики). Подполе a.
        /// </summary>
        [SubField('a')]
        [XmlAttribute("totalWithoutPeriodicals")]
        [JsonPropertyName("totalWithoutPeriodicals")]
        [Description("Поступило впервые всего (без периодики)")]
        [DisplayName("Поступило впервые всего (без периодики)")]
        public string? TotalWithoutPeriodicals { get; set; }

        /// <summary>
        /// Поступило впервые не на баланс (с периодикой). Подполе 2.
        /// </summary>
        [SubField('2')]
        [XmlAttribute("offBalanceWithPeriodicals")]
        [JsonPropertyName("offBalanceWithPeriodicals")]
        [Description("Поступило впервые не на баланс (с периодикой)")]
        [DisplayName("Поступило впервые не на баланс (с периодикой)")]
        public string? OffBalanceWithPeriodicals { get; set; }

        /// <summary>
        /// Учебные издания. Подполе 3.
        /// </summary>
        [SubField('3')]
        [XmlAttribute("educational")]
        [JsonPropertyName("educational")]
        [Description("Учебные издания")]
        [DisplayName("Учебные издания")]
        public string? Educational { get; set; }

        /// <summary>
        /// Unknown subfields.
        /// </summary>
        [XmlElement("unknown")]
        [JsonPropertyName("unknown")]
        [Browsable(false)]
        public SubField[]? UnknownSubFields { get; set; }

        /// <summary>
        /// Associated field.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        [Browsable(false)]
        public Field? Field { get; set; }

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
        /// Apply the <see cref="ArrivalsInfo"/>
        /// to the <see cref="Field"/>.
        /// </summary>
        public void ApplyToField
            (
                Field field
            )
        {
            field
                .ApplySubField('1', OnBalanceWithoutPeriodicals)
                .ApplySubField('b', OffBalanceWithoutPeriodicals)
                .ApplySubField('a', TotalWithoutPeriodicals)
                .ApplySubField('2', OffBalanceWithPeriodicals)
                .ApplySubField('3', Educational);
        }

        /// <summary>
        /// Parse the <see cref="Field"/>.
        /// </summary>
        public static ArrivalsInfo ParseField
            (
                Field field
            )
        {
            // TODO: реализовать эффективно

            var result = new ArrivalsInfo
            {
                OnBalanceWithoutPeriodicals = field.GetFirstSubFieldValue('1'),
                OffBalanceWithoutPeriodicals = field.GetFirstSubFieldValue('b'),
                TotalWithoutPeriodicals = field.GetFirstSubFieldValue('a'),
                OffBalanceWithPeriodicals = field.GetFirstSubFieldValue('2'),
                Educational = field.GetFirstSubFieldValue('3'),
                UnknownSubFields = field.Subfields.GetUnknownSubFields(KnownCodes),
                Field = field
            };

            return result;
        }

        /// <summary>
        /// Parse the <see cref="Record"/>.
        /// </summary>
        public static ArrivalsInfo[] ParseRecord
            (
                Record record
            )
        {
            var result = new List<ArrivalsInfo>();
            foreach (Field field in record.Fields)
            {
                if (field.Tag == Tag)
                {
                    result.Add(ParseField(field));
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// Should serialize the <see cref="UnknownSubFields"/> array?
        /// </summary>
        [ExcludeFromCodeCoverage]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeUnknownSubFields() =>
            !ArrayUtility.IsNullOrEmpty(UnknownSubFields);

        /// <summary>
        /// Convert the <see cref="ArrivalsInfo"/>
        /// back to <see cref="Field"/>.
        /// </summary>
        public Field ToField() => new Field (Tag)
                .AddNonEmpty ('1', OnBalanceWithoutPeriodicals)
                .AddNonEmpty ('b', OffBalanceWithoutPeriodicals)
                .AddNonEmpty ('a', TotalWithoutPeriodicals)
                .AddNonEmpty ('2', OffBalanceWithPeriodicals)
                .AddNonEmpty ('3', Educational)
                .AddSubFields (UnknownSubFields);

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            OnBalanceWithoutPeriodicals = reader.ReadNullableString();
            OffBalanceWithoutPeriodicals = reader.ReadNullableString();
            TotalWithoutPeriodicals = reader.ReadNullableString();
            OffBalanceWithPeriodicals = reader.ReadNullableString();
            Educational = reader.ReadNullableString();
            UnknownSubFields = reader.ReadNullableArray<SubField>();
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            writer
                .WriteNullable(OnBalanceWithoutPeriodicals)
                .WriteNullable(OffBalanceWithoutPeriodicals)
                .WriteNullable(TotalWithoutPeriodicals)
                .WriteNullable(OffBalanceWithPeriodicals)
                .WriteNullable(Educational)
                .WriteNullableArray(UnknownSubFields);
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify" />
        public bool Verify
            (
                bool throwOnError
            )
        {
            var verifier = new Verifier<ArrivalsInfo>(this, throwOnError);

            verifier.Assert
                (
                    !string.IsNullOrEmpty(OnBalanceWithoutPeriodicals)
                    || !string.IsNullOrEmpty(OffBalanceWithoutPeriodicals)
                    || !string.IsNullOrEmpty(TotalWithoutPeriodicals)
                    || !string.IsNullOrEmpty(OffBalanceWithPeriodicals)
                    || !string.IsNullOrEmpty(Educational)
                );

            return verifier.Result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        // ReSharper disable UseStringInterpolation
        public override string ToString() => string.Format
            (
                "OnBalanceWithoutPeriodicals: {0}, "
                + "OffBalanceWithoutPeriodicals: {1}, "
                + "TotalWithoutPeriodicals: {2}, "
                + "OffBalanceWithPeriodicals: {3}, "
                + "Educational: {4}",
                OnBalanceWithoutPeriodicals.ToVisibleString(),
                OffBalanceWithoutPeriodicals.ToVisibleString(),
                TotalWithoutPeriodicals.ToVisibleString(),
                OffBalanceWithPeriodicals.ToVisibleString(),
                Educational.ToVisibleString()
            );
        // ReSharper restore UseStringInterpolation

        #endregion

    } // class ArrivalsInfo

} // namespace ManagedIrbis.Fields
