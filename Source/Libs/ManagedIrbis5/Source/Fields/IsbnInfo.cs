// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable ConvertClosureToMethodGroup
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Local
// ReSharper disable StringLiteralTypo

/* IsbnInfo.cs -- ISBN и цена, поле 10
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
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
    /// ISBN и цена, поле 10.
    /// </summary>
    [XmlRoot ("isbn")]
    public sealed class IsbnInfo
        : IHandmadeSerializable,
            IVerifiable
    {
        #region Constants

        /// <summary>
        /// Known subfield codes.
        /// </summary>
        public const string KnownCodes = "abcdz";

        /// <summary>
        /// Tag.
        /// </summary>
        public const int Tag = 10;

        #endregion

        #region Properties

        /// <summary>
        /// ISBN, подполе A.
        /// </summary>
        [SubField ('a')]
        [XmlElement ("isbn")]
        [JsonPropertyName ("isbn")]
        [Description ("ISBN. Подполе a.")]
        [DisplayName ("ISBN")]
        public string? Isbn { get; set; }

        /// <summary>
        /// Уточнение, подполе B.
        /// </summary>
        [SubField ('b')]
        [XmlElement ("refinement")]
        [JsonPropertyName ("refinement")]
        [Description ("Уточнение")]
        [DisplayName ("Уточнение")]
        public string? Refinement { get; set; }

        /// <summary>
        /// Ошибочный ISBN, подполе Z.
        /// </summary>
        [SubField ('z')]
        [XmlElement ("erroneous")]
        [JsonPropertyName ("erroneous")]
        [Description ("Ошибочный ISBN")]
        [DisplayName ("Ошибочный ISBN")]
        public string? Erroneous { get; set; }

        /// <summary>
        /// Цена общая для всех экземпляров, подполе D.
        /// </summary>
        [SubField ('d')]
        [XmlElement ("price")]
        [JsonPropertyName ("price")]
        [Description ("Цена общая для всех экземпляров")]
        [DisplayName ("Цена общая для всех экземпляров")]
        public string? PriceString { get; set; }

        /// <summary>
        /// Цена общая для всех экземпляров, подполе D.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        [Browsable (false)]
        public decimal Price
        {
            get => PriceString.SafeToDecimal();
            set => PriceString = value.ToString ("#.00", CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Обозначение валюты, подполе C.
        /// </summary>
        [SubField ('c')]
        [XmlElement ("currency")]
        [JsonPropertyName ("currency")]
        [Description ("Обозначение валюты")]
        [DisplayName ("Обозначение валюты")]
        public string? Currency { get; set; }

        /// <summary>
        /// Unknown subfields.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        [Browsable (false)]
        public SubField[]? UnknownSubFields { get; set; }

        /// <summary>
        /// Поле, из которого были загруженные данные.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        [Browsable (false)]
        public Field? Field { get; set; }

        /// <summary>
        /// Произвольные пользовательские данные.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        [Browsable (false)]
        public object? UserData { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Apply the <see cref="IsbnInfo"/>
        /// to the <see cref="Field"/>.
        /// </summary>
        public Field ApplyToField (Field field) => field
            .SetSubFieldValue ('a', Isbn)
            .SetSubFieldValue ('b', Refinement)
            .SetSubFieldValue ('z', Erroneous)
            .SetSubFieldValue ('d', PriceString)
            .SetSubFieldValue ('c', Currency);

        /// <summary>
        /// Parse the <see cref="Record"/>.
        /// </summary>
        public static IsbnInfo[] ParseRecord
            (
                Record record,
                int tag = Tag
            )
        {
            var result = new List<IsbnInfo>();
            foreach (Field field in record.Fields)
            {
                if (field.Tag == tag)
                {
                    var isbn = ParseField (field);
                    result.Add (isbn);
                }
            }

            return result.ToArray();

        } // method ParseRecord

        /// <summary>
        /// Parse the specified field.
        /// </summary>
        public static IsbnInfo ParseField (Field field) => new ()
            {
                Isbn = field.GetFirstSubFieldValue ('a'),
                Refinement = field.GetFirstSubFieldValue ('b'),
                Erroneous = field.GetFirstSubFieldValue ('z'),
                PriceString = field.GetFirstSubFieldValue ('d'),
                Currency = field.GetFirstSubFieldValue ('c'),
                UnknownSubFields = field.Subfields.GetUnknownSubFields (KnownCodes),
                Field = field
            };

        /// <summary>
        /// Should serialize the <see cref="UnknownSubFields"/> array?
        /// </summary>
        [ExcludeFromCodeCoverage]
        [EditorBrowsable (EditorBrowsableState.Never)]
        public bool ShouldSerializeUnknownSubFields() =>
            !ArrayUtility.IsNullOrEmpty (UnknownSubFields);

        /// <summary>
        /// Transform back to field.
        /// </summary>
        public Field ToField() => new Field (Tag)
                .AddNonEmpty ('a', Isbn)
                .AddNonEmpty ('b', Refinement)
                .AddNonEmpty ('z', Erroneous)
                .AddNonEmpty ('c', Currency)
                .AddNonEmpty
                    (
                        'd',
                        Price != 0.0m ? Price.ToInvariantString() : null
                    );

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Isbn = reader.ReadNullableString();
            Refinement = reader.ReadNullableString();
            Erroneous = reader.ReadNullableString();
            Currency = reader.ReadNullableString();
            PriceString = reader.ReadNullableString();

        } // method RestoreFromStream

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            writer
                .WriteNullable (Isbn)
                .WriteNullable (Refinement)
                .WriteNullable (Erroneous)
                .WriteNullable (Currency)
                .WriteNullable (PriceString);

        } // method SaveToStream

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify" />
        public bool Verify
            (
                bool throwOnError
            )
        {
            var verifier = new Verifier<IsbnInfo> (this, throwOnError);

            verifier.Assert
                (
                    !string.IsNullOrEmpty (PriceString)
                    || !string.IsNullOrEmpty (Isbn)
                    || !string.IsNullOrEmpty (Erroneous)
                );

            return verifier.Result;

        } // method Verify

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            if (string.IsNullOrEmpty (Isbn))
            {
                return string.IsNullOrEmpty (PriceString) ? "(null)" : PriceString;
            }

            if (string.IsNullOrEmpty (PriceString))
            {
                return Isbn;
            }

            return Isbn + " : " + PriceString;

        } // method ToString

        #endregion

    } // class IsbnInfo

} // namespace ManagedIrbis.Fields
