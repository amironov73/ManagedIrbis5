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

/* PartInfo.cs -- выпуск, часть: поле 923
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
    /// Выпуск, часть. Поле 923.
    /// </summary>
    [XmlRoot("part")]
    public sealed class PartInfo
        : IHandmadeSerializable,
        IVerifiable
    {
        #region Constants

        /// <summary>
        /// Известные коды полей.
        /// </summary>
        public const string KnownCodes = "hiklu";

        /// <summary>
        /// Метка поля.
        /// </summary>
        public const int Tag = 923;

        #endregion

        #region Properties

        /// <summary>
        /// Обозначение и № 2-й единицы деления (Выпуск). Подполе h.
        /// </summary>
        [SubField('h')]
        [XmlElement("secondLevelNumber")]
        [JsonPropertyName("secondLevelNumber")]
        [Description("Обозначение и № 2-й единицы деления (Выпуск)")]
        [DisplayName("Обозначение и № 2-й единицы деления (Выпуск)")]
        public string? SecondLevelNumber { get; set; }

        /// <summary>
        /// Заглавие 2-й единицы деления (Выпуск). Подполе i.
        /// </summary>
        [SubField('i')]
        [XmlElement("secondLevelTitle")]
        [JsonPropertyName("secondLevelTitle")]
        [Description("Заглавие 2-й единицы деления (Выпуск)")]
        [DisplayName("Заглавие 2-й единицы деления (Выпуск)")]
        public string? SecondLevelTitle { get; set; }

        /// <summary>
        /// Обозначение и № 3-й единицы деления (Часть). Подполе k.
        /// </summary>
        [SubField('k')]
        [XmlElement("thirdLevelNumber")]
        [JsonPropertyName("thirdLevelNumber")]
        [Description("Обозначение и № 3-й единицы деления (Часть)")]
        [DisplayName("Обозначение и № 3-й единицы деления (Часть)")]
        public string? ThirdLevelNumber { get; set; }

        /// <summary>
        /// Заглавие 3-й единицы деления (Часть). Подполе l.
        /// </summary>
        [SubField('l')]
        [XmlElement("thirdLevelTitle")]
        [JsonPropertyName("thirdLevelTitle")]
        [Description("Заглавие 3-й единицы деления (Часть)")]
        [DisplayName("Заглавие 3-й единицы деления (Часть)")]
        public string? ThirdLevelTitle { get; set; }

        /// <summary>
        /// Роль (как выводить в словарь?). Подполе u.
        /// </summary>
        [SubField('u')]
        [XmlElement("role")]
        [JsonPropertyName("role")]
        [Description("Роль (как выводить в словарь?)")]
        [DisplayName("Роль (как выводить в словарь?)")]
        public string? Role { get; set; }

        /// <summary>
        /// Неизвестные подполя.
        /// </summary>
        [XmlElement("unknown")]
        [JsonPropertyName("unknown")]
        [Browsable(false)]
        public SubField[]? UnknownSubFields { get; set; }

        /// <summary>
        /// Связанное поле записи.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        [DisplayName("Поле с подполями")]
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
        /// Применение данных о выпуске/части <see cref="PartInfo"/>
        /// к полю записи <see cref="Field"/>.
        /// </summary>
        public Field ApplyToField (Field field) => field
            .SetSubFieldValue ('h', SecondLevelNumber)
            .SetSubFieldValue ('i', SecondLevelTitle)
            .SetSubFieldValue ('k', ThirdLevelNumber)
            .SetSubFieldValue ('l', ThirdLevelTitle)
            .SetSubFieldValue ('u', Role);

        /// <summary>
        /// Разбор библиографической записи <see cref="Record"/>.
        /// </summary>
        public static PartInfo[] ParseRecord
            (
                Record record
            )
        {
            Sure.NotNull(record, "record");

            var result = new List<PartInfo>();
            foreach (var field in record.Fields)
            {
                if (field.Tag == Tag)
                {
                    var part = ParseField (field);
                    result.Add (part);
                }
            }

            return result.ToArray();

        } // method ParseRecord

        /// <summary>
        /// Разбор поля библиографической записи <see cref="Field"/>.
        /// </summary>
        public static PartInfo ParseField (Field field) => new ()
            {
                SecondLevelNumber = field.GetFirstSubFieldValue('h'),
                SecondLevelTitle = field.GetFirstSubFieldValue('i'),
                ThirdLevelNumber = field.GetFirstSubFieldValue('k'),
                ThirdLevelTitle = field.GetFirstSubFieldValue('l'),
                Role = field.GetFirstSubFieldValue('u'),
                UnknownSubFields = field.Subfields.GetUnknownSubFields(KnownCodes),
                Field = field
            };

        /// <summary>
        /// Should serialize the <see cref="UnknownSubFields"/> array?
        /// </summary>
        [ExcludeFromCodeCoverage]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeUnknownSubFields() => !ArrayUtility.IsNullOrEmpty (UnknownSubFields);

        /// <summary>
        /// Transform back to field.
        /// </summary>
        public Field ToField() => new Field (Tag)
            .AddNonEmpty ('h', SecondLevelNumber)
            .AddNonEmpty ('i', SecondLevelTitle)
            .AddNonEmpty ('k', ThirdLevelNumber)
            .AddNonEmpty ('l', ThirdLevelTitle)
            .AddNonEmpty ('u', Role)
            .AddRange (UnknownSubFields);

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Sure.NotNull(reader, nameof(reader));

            SecondLevelNumber = reader.ReadNullableString();
            SecondLevelTitle = reader.ReadNullableString();
            ThirdLevelNumber = reader.ReadNullableString();
            ThirdLevelTitle = reader.ReadNullableString();
            Role = reader.ReadNullableString();
            UnknownSubFields = reader.ReadNullableArray<SubField>();

        } // method RestoreFromStream

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Sure.NotNull(writer, nameof(writer));

            writer
                .WriteNullable(SecondLevelNumber)
                .WriteNullable(SecondLevelTitle)
                .WriteNullable(ThirdLevelNumber)
                .WriteNullable(ThirdLevelTitle)
                .WriteNullable(Role)
                .WriteNullableArray(UnknownSubFields);

        } // method SaveToStream

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify" />
        public bool Verify (bool throwOnError) => new Verifier<PartInfo>(this, throwOnError)
            .Assert
            (
                !string.IsNullOrEmpty(SecondLevelNumber)
                || !string.IsNullOrEmpty(SecondLevelTitle)
                || !string.IsNullOrEmpty(ThirdLevelNumber)
                || !string.IsNullOrEmpty(ThirdLevelTitle)
            )
            .Result;

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString() =>
            Utility.JoinNonEmpty
                (
                    " -- ",
                    SecondLevelNumber,
                    SecondLevelTitle,
                    ThirdLevelNumber,
                    ThirdLevelTitle
                )
            .EmptyToNull()
            .ToVisibleString();

        #endregion

    } // class PartInfo

} // namespace ManagedIrbis.Fields
