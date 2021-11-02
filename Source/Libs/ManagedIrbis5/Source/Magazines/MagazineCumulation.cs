// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable ConvertClosureToMethodGroup
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* MagazineCumulation.cs -- данные о кумуляции номеров
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;
using AM.IO;
using AM.Runtime;

#endregion

#nullable enable

namespace ManagedIrbis.Magazines
{
    /// <summary>
    /// Данные о кумуляции номеров. Поле 909.
    /// </summary>
    [XmlRoot ("cumulation")]
    public sealed class MagazineCumulation
        : IHandmadeSerializable,
            IVerifiable
    {
        #region Constants

        /// <summary>
        /// Метка поля по умолчанию.
        /// </summary>
        public const int Tag = 909;

        /// <summary>
        /// Коды известных полей.
        /// </summary>
        public const string KnownCodes = "dfhkq";

        #endregion

        #region Properties

        /// <summary>
        /// Год. Подполе Q.
        /// </summary>
        [XmlAttribute ("year")]
        [JsonPropertyName ("year")]
        public string? Year { get; set; }

        /// <summary>
        /// Том. Подполе F.
        /// </summary>
        [XmlAttribute ("volume")]
        [JsonPropertyName ("volume")]
        public string? Volume { get; set; }

        /// <summary>
        /// Место хранения. Подполе D.
        /// </summary>
        [XmlAttribute ("place")]
        [JsonPropertyName ("place")]
        public string? Place { get; set; }

        /// <summary>
        /// Кумулированные номера. Подполе H.
        /// </summary>
        [XmlAttribute ("numbers")]
        [JsonPropertyName ("numbers")]
        public string? Numbers { get; set; }

        /// <summary>
        /// Номер комплекта. Подполе K.
            /// </summary>
        [XmlAttribute ("set")]
        [JsonPropertyName ("set")]
        public string? ComplectNumber { get; set; }

        /// <summary>
        /// Неопознанные подполя.
        /// </summary>
        [XmlElement ("unknown")]
        [JsonPropertyName ("unknown")]
        public SubField[]? UnknownSubFields { get; set; }

        /// <summary>
        /// Ассоциированное поле библиографической записи.
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
        /// Применение кумуляции к полю записи <see cref="Field"/>.
        /// </summary>
        public Field ApplyTo (Field field) => field
            .SetSubFieldValue ('q', Year)
            .SetSubFieldValue ('f', Volume)
            .SetSubFieldValue ('d', Place)
            .SetSubFieldValue ('h', Numbers)
            .SetSubFieldValue ('k', ComplectNumber);

        /// <summary>
        /// Разбор поля.
        /// </summary>
        public static MagazineCumulation Parse
            (
                Field field
            )
        {
            // TODO: реализовать эффективно

            var result = new MagazineCumulation
            {
                Year = field.GetFirstSubFieldValue ('q'),
                Volume = field.GetFirstSubFieldValue ('f'),
                Place = field.GetFirstSubFieldValue ('d'),
                Numbers = field.GetFirstSubFieldValue ('h'),
                ComplectNumber = field.GetFirstSubFieldValue ('k'),
                UnknownSubFields = field.Subfields.GetUnknownSubFields (KnownCodes),
                Field = field
            };

            return result;

        } // method Parse

        /// <summary>
        /// Разбор записи.
        /// </summary>
        public static MagazineCumulation[] Parse
            (
                Record record,
                int tag = Tag
            )
        {
            return record.Fields
                .GetField (tag)
                .Select (field => Parse (field))
                .ToArray();

        } // method Parse


        /// <summary>
        /// Should serialize the <see cref="UnknownSubFields"/> array?
        /// </summary>
        [ExcludeFromCodeCoverage]
        public bool ShouldSerializeUnknownSubFields() =>
            !ReferenceEquals (UnknownSubFields, null)
            && UnknownSubFields.Length != 0;

        /// <summary>
        /// Convert back to <see cref="Field"/>.
        /// </summary>
        public Field ToField()
        {
            Field result = new Field (Tag)
                .AddNonEmpty ('q', Year)
                .AddNonEmpty ('f', Volume)
                .AddNonEmpty ('d', Place)
                .AddNonEmpty ('h', Numbers)
                .AddNonEmpty ('k', ComplectNumber)
                .AddRange (UnknownSubFields);

            return result;

        } // method ToField

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Year = reader.ReadNullableString();
            Volume = reader.ReadNullableString();
            Place = reader.ReadNullableString();
            Numbers = reader.ReadNullableString();
            ComplectNumber = reader.ReadNullableString();
            UnknownSubFields = reader.ReadNullableArray<SubField>();

        } // method RestoreFromStream

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            writer
                .WriteNullable (Year)
                .WriteNullable (Volume)
                .WriteNullable (Place)
                .WriteNullable (Numbers)
                .WriteNullable (ComplectNumber)
                .WriteNullableArray (UnknownSubFields);

        } // method SaveToStream

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify" />
        public bool Verify
            (
                bool throwOnError
            )
        {
            var verifier = new Verifier<MagazineCumulation> (this, throwOnError);

            verifier
                .NotNullNorEmpty (Year, "Year")
                .NotNullNorEmpty (Numbers, "Number");

            return verifier.Result;

        } // method Verify

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString() => Year.ToVisibleString() + ": " + Numbers.ToVisibleString();

        #endregion

    } // class MagazineCumulation

} // namespace ManagedIrbis.Magazines
