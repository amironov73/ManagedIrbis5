// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable ConvertClosureToMethodGroup
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global

/* AccessRight.cs -- право доступа к ресурсу
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;
using AM.IO;
using AM.Runtime;

using ManagedIrbis.Mapping;

#endregion

#nullable enable

namespace ManagedIrbis.Drm
{
    /// <summary>
    /// Право доступа к ресурсу. Поле 3.
    /// </summary>
    public sealed class AccessRight
        : IHandmadeSerializable,
        IVerifiable
    {
        #region Constants

        /// <summary>
        /// Метка поля.
        /// </summary>
        public const int Tag = 3;

        /// <summary>
        /// Известные коды подполей.
        /// </summary>
        public const string KnownCodes = "abcdefg";

        #endregion

        #region Properties

        /// <summary>
        /// Элемент доступа. Подполе A.
        /// </summary>
        /// <remarks>
        /// Типичное значение: "02".
        /// </remarks>
        [SubField ('a')]
        [XmlAttribute ("element-kind")]
        [JsonPropertyName ("elementKind")]
        public string? ElementKind { get; set; }

        /// <summary>
        /// Значение элемента доступа. Подполе B.
        /// </summary>
        /// <remarks>
        /// Типичное значние: "В01".
        /// </remarks>
        [SubField ('b')]
        [XmlAttribute ("element-value")]
        [JsonPropertyName ("elementValue")]
        public string? ElementValue { get; set; }

        /// <summary>
        /// Значение права доступа. Подполе C.
        /// </summary>
        /// <remarks>
        /// Типичное значение: "2".
        /// </remarks>
        [SubField ('c')]
        [XmlAttribute ("access-kind")]
        [JsonPropertyName ("accessKind")]
        public string? AccessKind { get; set; }

        /// <summary>
        /// Количественное ограничение. Подполе F.
        /// </summary>
        [SubField ('f')]
        [XmlAttribute ("limit-value")]
        [JsonPropertyName ("limitValue")]
        public string? LimitValue { get; set; }

        /// <summary>
        /// Единицы ограничения. Подполе G.
        /// </summary>
        [SubField ('g')]
        [XmlAttribute ("limit-kind")]
        [JsonPropertyName ("limitKind")]
        public string? LimitKind { get; set; }

        /// <summary>
        /// Начальная дата периода доступа. Подполе D.
        /// </summary>
        [SubField ('d')]
        [XmlAttribute ("from")]
        [JsonPropertyName ("from")]
        public string? FromDate { get; set; }

        /// <summary>
        /// Конечная дата периода доступа. Подполе E.
        /// </summary>
        [SubField ('e')]
        [XmlAttribute ("till")]
        [JsonPropertyName ("till")]
        public string? TillDate { get; set; }

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
        /// Разбор заданного поля библиографической записи.
        /// </summary>
        public static AccessRight ParseField
            (
                Field field
            )
        {
            Sure.NotNull (field);

            var result = new AccessRight
            {
                ElementKind = field.GetFirstSubFieldValue ('a'),
                ElementValue = field.GetFirstSubFieldValue ('b'),
                AccessKind = field.GetFirstSubFieldValue ('c'),
                LimitValue = field.GetFirstSubFieldValue ('f'),
                LimitKind = field.GetFirstSubFieldValue ('g'),
                FromDate = field.GetFirstSubFieldValue ('d'),
                TillDate = field.GetFirstSubFieldValue ('e'),
                UnknownSubFields = field.Subfields.GetUnknownSubFields (KnownCodes),
                Field = field
            };

            return result;
        }

        /// <summary>
        /// Разбор заданной библиографической записи.
        /// </summary>
        public static AccessRight[] ParseRecord
            (
                Record record
            )
        {
            Sure.NotNull (record);

            return record
                .EnumerateField (3)
                .Select (field => ParseField (field))
                .ToArray();
        }

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream"/>
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Sure.NotNull (reader);

            ElementKind = reader.ReadNullableString();
            ElementValue = reader.ReadNullableString();
            AccessKind = reader.ReadNullableString();
            LimitValue = reader.ReadNullableString();
            LimitKind = reader.ReadNullableString();
            FromDate = reader.ReadNullableString();
            TillDate = reader.ReadNullableString();
            UnknownSubFields = reader.ReadNullableArray<SubField>();
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream"/>
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Sure.NotNull (writer);

            writer
                .WriteNullable (ElementKind)
                .WriteNullable (ElementValue)
                .WriteNullable (AccessKind)
                .WriteNullable (LimitValue)
                .WriteNullable (LimitKind)
                .WriteNullable (FromDate)
                .WriteNullable (TillDate)
                .WriteNullableArray (UnknownSubFields);
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify"/>
        public bool Verify
            (
                bool throwOnError
            )
        {
            var verifier = new Verifier<AccessRight> (this, throwOnError);

            verifier
                .NotNullNorEmpty (ElementKind)
                .NotNullNorEmpty (ElementValue)
                .NotNullNorEmpty (AccessKind);

            return verifier.Result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString()
        {
            return $"{ElementKind.ToVisibleString()} {ElementValue.ToVisibleString()} {AccessKind.ToVisibleString()}";
        }

        #endregion
    }
}
