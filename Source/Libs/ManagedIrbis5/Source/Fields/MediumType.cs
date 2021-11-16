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
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global

/* MediumType.cs -- тип средства, поле 182
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.ComponentModel;
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
    /// Поле 182 - тип средства.
    /// Введено в связи с ГОСТ 7.0.100-2018.
    /// </summary>
    [XmlRoot ("medium")]
    public sealed class MediumType
        : IHandmadeSerializable,
        IVerifiable
    {
        #region Constants

        /// <summary>
        /// Метка поля.
        /// </summary>
        public const int Tag = 182;

        /// <summary>
        /// Известные коды подполей.
        /// </summary>
        public const string KnownCodes = "a";

        #endregion

        #region Properties

        /// <summary>
        /// Код типа средства. Подполе A.
        /// </summary>
        [SubField ('a')]
        [XmlText]
        [JsonPropertyName ("medium")]
        [Description ("Код типа средства")]
        [DisplayName ("Код типа средства")]
        public string? MediumCode { get; set; }

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
        /// Применение данных к указанному полю библиографической записи <see cref="Field"/>.
        /// </summary>
        public Field ApplyTo
            (
                Field field
            )
        {
            Sure.NotNull (field);

            return field
                .SetSubFieldValue ('a', MediumCode);
        }

        /// <summary>
        /// Разбор указанного поля библиографической записи.
        /// </summary>
        public static MediumType ParseField
            (
                Field field
            )
        {
            Sure.NotNull (field);

            return new MediumType
            {
                MediumCode = field.GetFirstSubFieldValue ('a'),
                UnknownSubFields = field.Subfields.GetUnknownSubFields (KnownCodes),
                Field = field
            };
        }

        /// <summary>
        /// Преобразование данных в поле библиографической записи <see cref="Field"/>.
        /// </summary>
        public Field ToField()
        {
            return new Field (Tag)
                .AddNonEmpty ('a', MediumCode)
                .AddRange (UnknownSubFields);
        }

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Sure.NotNull (reader);

            MediumCode = reader.ReadNullableString();
            UnknownSubFields = reader.ReadNullableArray<SubField>();
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Sure.NotNull (writer);

            writer.WriteNullable (MediumCode);
            writer.WriteNullableArray (UnknownSubFields);
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify"/>
        public bool Verify
            (
                bool throwOnError
            )
        {
            var verifier = new Verifier<MediumType>(this, throwOnError);

            verifier
                .NotNullNorEmpty (MediumCode);

            return verifier.Result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return MediumCode.EmptyToNull().ToVisibleString();
        }

        #endregion

    } // class MediumType

} // namespace ManagedIrbis.Fields
