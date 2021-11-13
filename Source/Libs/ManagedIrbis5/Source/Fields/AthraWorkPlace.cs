// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* AthraWorkplace.cs --
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
    /// Место работы в базе данных ATHRA.
    /// Поле 910.
    /// </summary>
    [XmlRoot ("workplace")]
    public sealed class AthraWorkPlace
        : IHandmadeSerializable,
        IVerifiable
    {
        #region Constants

        /// <summary>
        /// Метка поля.
        /// </summary>
        public const int Tag = 910;

        /// <summary>
        /// Известные коды подполей.
        /// </summary>
        public const string KnownCodes = "py";

        #endregion

        #region Properties

        /// <summary>
        /// Работает в данной организации.
        /// Подполе Y.
        /// </summary>
        [SubField ('y')]
        [XmlElement ("here")]
        [JsonPropertyName ("here")]
        [Description ("Работает в данной организации")]
        [DisplayName ("Работает в данной организации")]
        public string? WorksHere { get; set; }

        /// <summary>
        /// Место работы автора.
        /// Подполе P.
        /// </summary>
        [SubField ('p')]
        [XmlElement ("place")]
        [JsonPropertyName ("place")]
        [Description ("Место работы автора")]
        [DisplayName ("Место работы автора")]
        public string? WorkPlace { get; set; }

        /// <summary>
        /// Неизвестные подполя.
        /// </summary>
        [XmlElement ("unknown")]
        [JsonPropertyName ("unknown")]
        [Browsable (false)]
        public SubField[]? UnknownSubFields { get; set; }

        /// <summary>
        /// Ассоциированное поле библиографической записи <see cref="Field"/>.
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
        /// Применение данных к полю библиографической записи.
        /// </summary>
        public Field ApplyTo (Field field) => field
            .ThrowIfNull ()
            .SetSubFieldValue ('y', WorksHere)
            .SetSubFieldValue ('p', WorkPlace);

        /// <summary>
        /// Разбор поля библиографической записи.
        /// </summary>
        public static AthraWorkPlace ParseField
            (
                Field field
            )
        {
            Sure.NotNull (field);

            return new AthraWorkPlace
            {
                WorksHere = field.GetFirstSubFieldValue ('y'),
                WorkPlace = field.GetFirstSubFieldValue ('p'),
                Field = field
            };

        } // method ParseField

        /// <summary>
        /// Преобразование в поле библиографической записи <see cref="Field"/>.
        /// </summary>
        public Field ToField() => new Field (Tag)
            .AddNonEmpty ('p', WorkPlace)
            .AddNonEmpty ('y', WorksHere);

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Sure.NotNull (reader);

            WorksHere = reader.ReadNullableString();
            WorkPlace = reader.ReadNullableString();
            UnknownSubFields = reader.ReadNullableArray<SubField>();

        } // method RestoreFromStream

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Sure.NotNull (writer);

            writer
                .WriteNullable (WorksHere)
                .WriteNullable (WorkPlace)
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
            var verifier = new Verifier<AthraWorkPlace> (this, throwOnError);

            verifier
                .Assert (!string.IsNullOrEmpty (WorksHere) || !string.IsNullOrEmpty (WorkPlace));

            return verifier.Result;

        } // method Verify

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString() => WorkPlace.ToVisibleString();

        #endregion

    } // class AthraWorkPlace

} // namespace ManagedIrbis.Fieldss
