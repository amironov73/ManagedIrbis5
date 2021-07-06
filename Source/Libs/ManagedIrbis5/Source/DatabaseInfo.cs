// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* DatabaseInfo.cs -- информация о базе данных ИРБИС
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
using ManagedIrbis.Menus;

#endregion

#nullable enable

namespace ManagedIrbis
{
    /// <summary>
    /// Информация о базе данных ИРБИС.
    /// </summary>
    public sealed class DatabaseInfo
        : IHandmadeSerializable
    {
        #region Constants

        /// <summary>
        /// Разделитель элементов
        /// </summary>
        public const char ItemDelimiter = (char)0x1E;

        #endregion

        #region Properties

        /// <summary>
        /// Имя базы данных.
        /// </summary>
        [XmlAttribute("name")]
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        /// <summary>
        /// Описание базы данных
        /// </summary>
        [XmlAttribute("description")]
        [JsonPropertyName("description")]
        public string? Description { get; set; }

        /// <summary>
        /// Максимальный MFN.
        /// </summary>
        [XmlAttribute("maxMfn")]
        [JsonPropertyName("maxMfn")]
        public int MaxMfn { get; set; }

        /// <summary>
        /// Список логически удаленных записей.
        /// </summary>
        [XmlArrayItem("mfn")]
        [XmlArray("logicallyDeleted")]
        [JsonPropertyName("logicallyDeleted")]
        public int[]? LogicallyDeletedRecords { get; set; }

        /// <summary>
        /// Список физически удаленных записей.
        /// </summary>
        [XmlArrayItem("mfn")]
        [XmlArray("physicallyDeleted")]
        [JsonPropertyName("physicallyDeleted")]
        public int[]? PhysicallyDeletedRecords { get; set; }

        /// <summary>
        /// Список неактуализированных записей.
        /// </summary>
        [XmlArrayItem("mfn")]
        [XmlArray("nonActualizedRecords")]
        [JsonPropertyName("nonActualizedRecords")]
        public int[]? NonActualizedRecords { get; set; }

        /// <summary>
        /// Список заблокированных записей.
        /// </summary>
        [XmlArrayItem("mfn")]
        [XmlArray("lockedRecords")]
        [JsonPropertyName("lockedRecords")]
        public int[]? LockedRecords { get; set; }

        /// <summary>
        /// Флаг монопольной блокировки базы данных.
        /// </summary>
        [XmlAttribute("databaseLocked")]
        [JsonPropertyName("databaseLocked")]
        public bool DatabaseLocked { get; set; }

        /// <summary>
        /// База данных только для чтения.
        /// </summary>
        [XmlAttribute("readOnly")]
        [JsonPropertyName("readOnly")]
        public bool ReadOnly { get; set; }

        #endregion

        #region Private members

        private static void _Write
            (
                TextWriter writer,
                string name,
                int[]? mfns
            )
        {
            writer.Write($"{name}: ");
            writer.WriteLine
                (
                    mfns is null or { Length: 0 }
                    ? "None"
                    : string.Join(", ", mfns)
                );
        }

        private static int[] _ParseLine
            (
                string? text
            )
        {
            if (string.IsNullOrEmpty(text))
            {
                return Array.Empty<int>();
            }

            var items = text.Split(ItemDelimiter);
            var result = new int[items.Length];
            for (var i = 0; i < items.Length; i++)
            {
                result[i] = int.Parse(items[i]);
            }

            Array.Sort(result);

            return result;

        } // method ParseLine

        #endregion

        #region Public methods

        /// <summary>
        /// Разбор ответа сервера.
        /// </summary>
        public static DatabaseInfo Parse
            (
                string? name,
                Response response
            )
        {
            var result = new DatabaseInfo
            {
                Name = name,
                LogicallyDeletedRecords = _ParseLine(response.ReadAnsi()),
                PhysicallyDeletedRecords = _ParseLine(response.ReadAnsi()),
                NonActualizedRecords = _ParseLine(response.ReadAnsi()),
                LockedRecords = _ParseLine(response.ReadAnsi()),
                MaxMfn = _ParseLine(response.ReadAnsi())[0],
                DatabaseLocked = _ParseLine(response.ReadAnsi())[0] != 0
            };

            return result;

        } // method Parse

        /// <summary>
        /// Разбор меню со списком баз данных.
        /// </summary>
        public static DatabaseInfo[] ParseMenu
            (
                MenuFile menu
            )
        {
            var result = new List<DatabaseInfo>();

            foreach (var entry in menu.Entries)
            {
                var readOnly = false;
                var name = entry.Code;
                if (!string.IsNullOrEmpty(name))
                {
                    if (name.FirstChar() == '-')
                    {
                        readOnly = true;
                        name = name.Substring(1);
                    }

                    var database = new DatabaseInfo
                    {
                        Name = name,
                        Description = entry.Comment,
                        ReadOnly = readOnly
                    };
                    result.Add(database);
                }
            }

            return result.ToArray();

        } // method ParseMenu

        /// <summary>
        /// Вывод сведений о базе данных.
        /// </summary>
        public void Write
            (
                TextWriter writer
            )
        {
            writer.WriteLine($"Database: {Name}");
            writer.WriteLine($"Max MFN={MaxMfn}");
            writer.WriteLine($"Locked={DatabaseLocked}");
            _Write(writer, "Logically deleted records", LogicallyDeletedRecords);
            _Write(writer, "Physically deleted records", PhysicallyDeletedRecords);
            _Write(writer, "Non-actualized records", NonActualizedRecords);
            _Write(writer, "Locked records", LockedRecords);

        } // method Write

        #endregion

        #region IHandmadeSerializable membrs

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Sure.NotNull(reader, nameof(reader));

            Name = reader.ReadNullableString();
            Description = reader.ReadNullableString();
            MaxMfn = reader.ReadPackedInt32();
            LogicallyDeletedRecords = reader.ReadNullableInt32Array();
            PhysicallyDeletedRecords = reader.ReadNullableInt32Array();
            NonActualizedRecords = reader.ReadNullableInt32Array();
            LockedRecords = reader.ReadNullableInt32Array();
            DatabaseLocked = reader.ReadBoolean();

        } // method ReadFromStream

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Sure.NotNull(writer, nameof(writer));

            writer
                .WriteNullable(Name)
                .WriteNullable(Description)
                .WritePackedInt32(MaxMfn)
                .WriteNullableArray(LogicallyDeletedRecords)
                .WriteNullableArray(PhysicallyDeletedRecords)
                .WriteNullableArray(NonActualizedRecords)
                .WriteNullableArray(LockedRecords)
                .Write(DatabaseLocked);

        } // method SaveToStream

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            if (string.IsNullOrEmpty(Description))
            {
                return Name.ToVisibleString();
            }

            return $"{Name} - {Description}";

        } // method ToString

        #endregion

    } // class DatabaseInfo

} // namespace ManagedIrbis
