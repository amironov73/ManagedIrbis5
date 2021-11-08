// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable ConvertClosureToMethodGroup
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement
// ReSharper disable PropertyCanBeMadeInitOnly.Local
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* Holder.cs -- держатель документа
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.ComponentModel;
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
    /// <summary>
    /// Держатель документа, поле 902.
    /// </summary>
    [XmlRoot ("holder")]
    public sealed class Holder
        : IHandmadeSerializable,
        IVerifiable
    {
        #region Constants

        /// <summary>
        /// Метка поля.
        /// </summary>
        public const int Tag = 902;

        /// <summary>
        /// Известные коды подполей
        /// </summary>
        public const string KnownCodes = "abds";

        #endregion

        #region Properties

        /// <summary>
        /// Наименование организации, подполе A.
        /// </summary>
        [SubField ('a')]
        [XmlElement ("organization")]
        [JsonPropertyName ("organization")]
        [Description ("Наименование организации")]
        [DisplayName ("Наименование организации")]
        public string? Organization { get; set; }

        /// <summary>
        /// Почтовый адрес, подполе B.
        /// </summary>
        [SubField ('b')]
        [XmlElement ("address")]
        [JsonPropertyName ("address")]
        [Description ("Почтовый адрес")]
        [DisplayName ("Почтовый адрес")]
        public string? Address { get; set; }

        /// <summary>
        /// Адрес телекоммуникаций, например, e-mail, подполе D.
        /// </summary>
        [SubField ('d')]
        [XmlElement ("communication")]
        [JsonPropertyName ("communication")]
        [Description ("Адрес телекоммуникаций")]
        [DisplayName ("Адрес телекоммуникаций")]
        public string? Communication { get; set; }

        /// <summary>
        /// Сигла (национальный код организации), подполе S.
        /// </summary>
        [SubField ('s')]
        [XmlElement ("sigla")]
        [JsonPropertyName ("sigla")]
        [Description ("Сигла (национальный код организации)")]
        [DisplayName ("Сигла (национальный код организации)")]
        public string? Sigla { get; set; }

        /// <summary>
        /// Неизвестные подполя.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        [Browsable (false)]
        public SubField[]? UnknownSubFields { get; set; }

        /// <summary>
        /// Связанное поле в нераскодированном виде.
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
        /// Применение информации о держателе документа к заданному полю.
        /// </summary>
        public Field ApplyToField (Field field) => field
            .ThrowIfNull ()
            .SetSubFieldValue ('a', Organization)
            .SetSubFieldValue ('b', Address)
            .SetSubFieldValue ('d', Communication)
            .SetSubFieldValue ('s', Sigla);

        /// <summary>
        /// Разбор поля с информацией о держателе документа.
        /// </summary>
        public static Holder ParseField (Field field) => new ()
            {
                Organization = field.GetFirstSubFieldValue ('a'),
                Address = field.GetFirstSubFieldValue ('b'),
                Communication = field.GetFirstSubFieldValue ('d'),
                Sigla = field.GetFirstSubFieldValue ('s'),
                UnknownSubFields = field.Subfields.GetUnknownSubFields (KnownCodes),
                Field = field
            };

        /// <summary>
        /// Разбор библиографической записи на массив держателей документа.
        /// </summary>
        public static Holder[] ParseRecord
            (
                Record record
            )
        {
            Sure.NotNull (record);

            var result = new List<Holder>();
            foreach (var field in record.Fields)
            {
                if (field.Tag == Tag)
                {
                    var holder = ParseField (field);
                    result.Add (holder);
                }
            }

            return result.ToArray();

        } // method ParseRecord

        /// <summary>
        /// Создание поля с информацией о держателе документа.
        /// </summary>
        public Field ToField() => new Field (Tag)
            .AddNonEmpty ('a', Organization)
            .AddNonEmpty ('b', Address)
            .AddNonEmpty ('d', Communication)
            .AddNonEmpty ('s', Sigla)
            .AddRange (UnknownSubFields);

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream"/>
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Sure.NotNull (reader);

            Organization = reader.ReadNullableString();
            Address = reader.ReadNullableString();
            Communication = reader.ReadNullableString();
            Sigla = reader.ReadNullableString();
            UnknownSubFields = reader.ReadNullableArray<SubField>();

        } // method RestoreFromStream

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream"/>
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Sure.NotNull (writer);

            writer
                .WriteNullable (Organization)
                .WriteNullable (Address)
                .WriteNullable (Communication)
                .WriteNullable (Sigla)
                .WriteNullableArray (UnknownSubFields);

        } // method SaveToStream

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify"/>
        public bool Verify
            (
                bool throwOnError
            )
        {
            var verifier = new Verifier<Holder> (this, throwOnError);

            verifier
                .NotNullNorEmpty (Organization);

            return verifier.Result;

        } // method Verify

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString()
        {
            if (string.IsNullOrEmpty (Organization))
            {
                return Organization.ToVisibleString();
            }

            var builder = StringBuilderPool.Shared.Get();
            builder
                .AppendWithDelimiter (Organization)
                .AppendWithDelimiter (Address)
                .AppendWithDelimiter (Communication)
                .AppendWithDelimiter (Sigla);

            var result = builder.ToString();
            StringBuilderPool.Shared.Return (builder);

            return result.ToVisibleString();

        } // method ToString

        #endregion

    } // class Holder

} // namespace ManagedIrbis.Fields
