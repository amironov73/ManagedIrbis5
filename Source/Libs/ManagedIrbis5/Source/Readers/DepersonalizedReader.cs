// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* DepersonalizedReader.cs -- обезличенная информация о читателе.
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;
using AM.IO;
using AM.Runtime;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Mapping;

#endregion

#nullable enable

namespace ManagedIrbis.Readers
{
    /// <summary>
    /// Обезличенная информация о читателе.
    /// </summary>
    [XmlRoot("reader")]
    public sealed class DepersonalizedReader
        : IHandmadeSerializable
    {
        #region Properties

        /// <summary>
        /// Дата рождения. Поле 21.
        /// </summary>
        [Field(21)]
        [XmlAttribute("dateOfBirth")]
        [JsonPropertyName("dateOfBirth")]
        public string? DateOfBirth { get; set; }

        /// <summary>
        /// Номер читательского. Поле 30.
        /// </summary>
        [Field(30)]
        [XmlAttribute("ticket")]
        [JsonPropertyName("ticket")]
        public string? Ticket { get; set; }

        /// <summary>
        /// Пол. Поле 23.
        /// </summary>
        [Field(23)]
        [XmlAttribute("gender")]
        [JsonPropertyName("gender")]
        public string? Gender { get; set; }

        /// <summary>
        /// Категория. Поле 50.
        /// </summary>
        [Field(50)]
        [XmlAttribute("category")]
        [JsonPropertyName("category")]
        public string? Category { get; set; }

        /// <summary>
        /// Дата записи. Поле 51.
        /// </summary>
        [Field(51)]
        [XmlAttribute("registrationDate")]
        [JsonPropertyName("registrationDate")]
        public string? RegistrationDateString { get; set; }

        /// <summary>
        /// Дата регистрации
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public DateTime RegistrationDate =>
            IrbisDate.ConvertStringToDate(RegistrationDateString);

        /// <summary>
        /// Возраст, годы
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public int Age
        {
            get
            {
                var yearText = DateOfBirth;

                if (string.IsNullOrEmpty(yearText))
                {
                    return 0;
                }

                if (yearText.Length > 4)
                {
                    yearText = yearText.Substring(1, 4);
                }

                if (!int.TryParse(yearText, out var year))
                {
                    return 0;
                }

                return DateTime.Today.Year - year;
            }
        }

        /// <summary>
        /// Visits.
        /// </summary>
        public List<VisitInfo> Visits { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public DepersonalizedReader()
        {
            Visits = new List<VisitInfo>();
        }

        #endregion

        #region Private members

        private static void _DegreaseVisitInfo
            (
                VisitInfo visit
            )
        {
            visit.Description = null;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Add the visits if not yet.
        /// </summary>
        public void AddVisits
            (
                IEnumerable<VisitInfo> visits
            )
        {
            foreach (var visit in visits)
            {
                var found = false;
                foreach (var other in Visits)
                {
                    if (visit.SameVisit(other))
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    _DegreaseVisitInfo(visit);
                    Visits.Add(visit);
                }
            }
        }

        /// <summary>
        /// Create <see cref="DepersonalizedReader"/>
        /// from the <see cref="ReaderInfo"/>.
        /// </summary>
        public static DepersonalizedReader FromReaderInfo
            (
                ReaderInfo readerInfo
            )
        {
            var result = new DepersonalizedReader
            {
                Ticket = readerInfo.Ticket,
                Category = readerInfo.Category,
                DateOfBirth = readerInfo.DateOfBirth,
                Gender = readerInfo.Gender,
                RegistrationDateString = readerInfo.RegistrationDateString
            };

            if (readerInfo.Visits is not null)
            {
                result.Visits.AddRange(readerInfo.Visits);
                foreach (var visit in result.Visits)
                {
                    _DegreaseVisitInfo(visit);
                }
            }

            return result;
        }

        /// <summary>
        /// Convert back to the record.
        /// </summary>
        public Record ToMarcRecord()
        {
            var result = new Record();

            result
                .AddNonEmptyField(21, DateOfBirth)
                .AddNonEmptyField(30, Ticket)
                .AddNonEmptyField(23, Gender)
                .AddNonEmptyField(50, Category)
                .AddNonEmptyField(51, RegistrationDateString);

            foreach (var visit in Visits)
            {
                result.Fields.Add(visit.ToField());
            }

            return result;
        }

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            DateOfBirth = reader.ReadNullableString();
            Ticket = reader.ReadNullableString();
            Gender = reader.ReadNullableString();
            Category = reader.ReadNullableString();
            RegistrationDateString = reader.ReadNullableString();
            Visits = reader.ReadList<VisitInfo>();
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            writer
                .WriteNullable(DateOfBirth)
                .WriteNullable(Ticket)
                .WriteNullable(Gender)
                .WriteNullable(Category)
                .WriteNullable(RegistrationDateString)
                .WriteList(Visits);
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString() => Ticket.ToVisibleString();

        #endregion
    }
}
