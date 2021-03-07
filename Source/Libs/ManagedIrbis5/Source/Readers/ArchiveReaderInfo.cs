// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* ArchiveReaderInfo.cs -- архивная информация о читателе.
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
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

namespace ManagedIrbis.Readers
{
    /// <summary>
    /// Информация о читателе.
    /// </summary>
    [XmlRoot("reader")]
    public sealed class ArchiveReaderInfo
        : IHandmadeSerializable
    {
        #region Properties

        /// <summary>
        /// Identifier for LiteDB.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public int Id { get; set; }

        /// <summary>
        /// Номер читательского. Поле 30.
        /// </summary>
        [Field(30)]
        [XmlAttribute("ticket")]
        [JsonPropertyName("ticket")]
        public string? Ticket { get; set; }

        /// <summary>
        /// Информация о посещениях.
        /// </summary>
        [XmlArray("visits")]
        [JsonPropertyName("visits")]
        public VisitInfo[]? Visits { get; set; }

        /// <summary>
        /// Произвольные данные, ассоциированные с читателем.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public object? UserData { get; set; }

        /// <summary>
        /// Дата первого посещения
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public DateTime FirstVisitDate
            => Visits?.FirstOrDefault()?.DateGiven ?? DateTime.MinValue;

        /// <summary>
        /// Дата последнего посещения.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public DateTime LastVisitDate
            => Visits?.LastOrDefault()?.DateGiven ?? DateTime.MinValue;

        /// <summary>
        /// Кафедра последнего посещения.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public string? LastVisitPlace => Visits?.LastOrDefault()?.Department;

        /// <summary>
        /// Последний обслуживавший библиотекарь.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public string? LastVisitResponsible => Visits?.LastOrDefault()?.Responsible;

        /// <summary>
        /// MFN записи.
        /// </summary>
        [XmlAttribute("mfn")]
        [JsonPropertyName("mfn")]
        public int Mfn { get; set; }

        /// <summary>
        /// Flag for the reader info.
        /// </summary>
        [XmlAttribute("marked")]
        [JsonPropertyName("marked")]
        public bool Marked { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Parse the specified record.
        /// </summary>
        public static ArchiveReaderInfo Parse
            (
                Record record
            )
        {
            // TODO Support for unknown fields

            var result = new ArchiveReaderInfo
            {
                Ticket = record.FM(30),
                Visits = record.Fields
                    .GetField(40)
                    .Select(VisitInfo.Parse)
                    .ToArray(),
                Mfn = record.Mfn,
            };

            return result;
        } // method Parse

        /// <summary>
        /// Считывание из файла.
        /// </summary>
        public static ArchiveReaderInfo[] ReadFromFile
            (
                string fileName
            )
        {
            var result = SerializationUtility
                .RestoreArrayFromFile<ArchiveReaderInfo>(fileName);

            return result;
        } // method ReadFromFile

        /// <summary>
        /// Сохранение в файле.
        /// </summary>
        public static void SaveToFile
            (
                string fileName,
                ArchiveReaderInfo[] readers
            )
        {
            readers.SaveToFile(fileName);
        } // method SaveToFile

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            writer.WritePackedInt32(Id);
            writer.WriteNullable(Ticket);
            writer.WriteNullableArray(Visits);
            writer.WritePackedInt32(Mfn);
        } // method SaveToStream

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Id = reader.ReadPackedInt32();
            Ticket = reader.ReadNullableString();
            Visits = reader.ReadNullableArray<VisitInfo>();
            Mfn = reader.ReadPackedInt32();
        } // method RestoreFromStream

        /// <summary>
        /// Формирование записи по данным о читателе.
        /// </summary>
        public Record ToRecord()
        {
            var result = new Record
            {
                Mfn = Mfn
            };

            result.AddNonEmptyField(30, Ticket);
            if (Visits is not null)
            {
                foreach (VisitInfo visit in Visits)
                {
                    result.Fields.Add(visit.ToField());
                }
            }

            return result;
        } // method ToRecord

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString() => Ticket.ToVisibleString();

        #endregion

    } // class ArchiveReaderInfo

} // namespace ManagedIrbis.Readers
