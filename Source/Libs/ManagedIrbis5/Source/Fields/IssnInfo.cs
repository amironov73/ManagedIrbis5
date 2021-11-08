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

/* IssnInfo.cs -- ISSN, поле 11
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
    /// ISSN, поле 11.
    /// </summary>
    [XmlRoot ("issn")]
    public sealed class IssnInfo
        : IHandmadeSerializable,
        IVerifiable
    {
        #region Constants

        /// <summary>
        /// Метка поля.
        /// </summary>
        public const int Tag = 11;

        /// <summary>
        /// Известные коды подполей.
        /// </summary>
        public const string KnownCodes = "a";

        #endregion

        #region Properties

        /// <summary>
        /// ISSN, подполе A.
        /// </summary>
        [SubField ('a')]
        [XmlText]
        [JsonPropertyName ("issn")]
        [Description ("ISSN")]
        [DisplayName ("ISSN")]
        public string? Issn { get; set; }

        /// <summary>
        /// Неизвестные подполя.
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
        /// Применение информации об ISSN к заданному полю.
        /// </summary>
        public Field ApplyToField (Field field) => field
            .SetSubFieldValue ('a', Issn);

        /// <summary>
        /// Разбор поля с информацией о ISSN.
        /// </summary>
        public static IssnInfo ParseField (Field field) => new ()
            {
                Issn = field.GetFirstSubFieldValue ('a') ?? field.Value,
                UnknownSubFields = field.Subfields.GetUnknownSubFields (KnownCodes),
                Field = field
            };

        /// <summary>
        /// Разбор библиографической записи на массив ISSN.
        /// </summary>
        public static IssnInfo[] ParseRecord
            (
                Record record,
                int tag = Tag
            )
        {
            var result = new List<IssnInfo>();
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
        /// Нужно ли сериализовать массив <see cref="UnknownSubFields"/>?
        /// </summary>
        [ExcludeFromCodeCoverage]
        [EditorBrowsable (EditorBrowsableState.Never)]
        public bool ShouldSerializeUnknownSubFields() =>
            !ArrayUtility.IsNullOrEmpty (UnknownSubFields);

        /// <summary>
        /// Создание поля с информацией о ISSN.
        /// </summary>
        public Field ToField() => new Field (Tag)
                .AddNonEmpty ('a', Issn);

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Issn = reader.ReadNullableString();
            UnknownSubFields = reader.ReadNullableArray<SubField>();

        } // method RestoreFromStream

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            writer
                .WriteNullable (Issn)
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
            var verifier = new Verifier<IssnInfo> (this, throwOnError);

            verifier.NotNullNorEmpty (Issn);

            return verifier.Result;

        } // method Verify

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString() => Issn.ToVisibleString();

        #endregion

    } // class IssnInfo

} // namespace ManagedIrbis.Fields
