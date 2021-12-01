// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* AthruRecord.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;
using AM.Linq;
using AM.Runtime;

using ManagedIrbis.Mapping;

#endregion

#nullable enable

namespace ManagedIrbis.Systematization
{
    /// <summary>
    /// Запись в базе данных ATHRU.
    /// </summary>
    [XmlRoot ("athru")]
    public sealed class AthruRecord
        : IHandmadeSerializable,
        IVerifiable
    {
        #region Properties

        /// <summary>
        /// Основной заголовок рубрики.
        /// Поле 210.
        /// </summary>
        [Field (210)]
        [XmlElement ("main-heading")]
        [JsonPropertyName ("mainHeading")]
        [Description ("Основной заголовок рубрики")]
        public AthrbHeading? MainHeading { get; set; }

        /// <summary>
        /// Связанные заголовки рубрики.
        /// Поле 510.
        /// </summary>
        [Field (510)]
        [XmlElement ("linked-heading")]
        [JsonPropertyName ("linkedHeading")]
        [Description ("Связанные заголовки рубрики")]
        public AthrbHeading[]? LinkedHeadings { get; set; }

        /// <summary>
        /// Методические указания / описания.
        /// Поле 300.
        /// </summary>
        [Field (300)]
        [XmlElement ("guidelines")]
        [JsonPropertyName ("guidelines")]
        [Description ("Методические указания")]
        public AthrbGuidelines[]? Guidelines { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Разбор библиографической записи.
        /// </summary>
        public static AthrbRecord ParseRecord
            (
                Record record
            )
        {
            Sure.NotNull (record);

            var result = new AthrbRecord
            {
                MainHeading = AthrbHeading.Parse (record.Fields.GetFirstField (210)),

                LinkedHeadings = record.Fields
                    .GetField (510)
                    .Select (AthrbHeading.Parse)
                    .NonNullItems()
                    .ToArray(),

                Guidelines = record.Fields
                    .GetField (300)
                    .Select (AthrbGuidelines.Parse)
                    .ToArray()
            };

            return result;
        }

        /// <summary>
        /// Преобразование информации в библиографическую запись.
        /// </summary>
        public Record ToRecord()
        {
            throw new NotImplementedException();
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

            throw new NotImplementedException();
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream"/>
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Sure.NotNull (writer);

            throw new NotImplementedException();
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify"/>
        public bool Verify
            (
                bool throwOnError
            )
        {
            var verifier = new Verifier<AthruRecord> (this, throwOnError);

            verifier
                .NotNull (MainHeading);

            return verifier.Result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return MainHeading.ToVisibleString();
        }

        #endregion
    }
}
