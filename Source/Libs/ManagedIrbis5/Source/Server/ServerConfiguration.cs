// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* ServerConfiguration.cs -- конфигурация сервера
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.IO;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;
using AM.IO;
using AM.Runtime;

using ManagedIrbis.Infrastructure;

#endregion

#nullable enable

namespace ManagedIrbis.Server
{
    /// <summary>
    /// Конфигурация сервера.
    /// </summary>
    [XmlRoot ("configuration")]
    public sealed class ServerConfiguration
        : IHandmadeSerializable,
            IVerifiable
    {
        #region Properties

        /// <summary>
        /// Путь до таблицы символов (без расширения).
        /// </summary>
        [XmlElement ("alphabetTablePath")]
        [JsonPropertyName ("alphabetTablePath")]
        public string? AlphabetTablePath { get; set; }

        /// <summary>
        /// Путь к директории с данными.
        /// </summary>
        [XmlElement ("dataPath")]
        [JsonPropertyName ("dataPath")]
        public string? DataPath { get; set; }

        /// <summary>
        /// Системный путь.
        /// </summary>
        [XmlElement ("systemPath")]
        [JsonPropertyName ("systemPath")]
        public string? SystemPath { get; set; }

        /// <summary>
        /// Путь до таблицы преобразования символов
        /// в верхний регистр (без расширения).
        /// </summary>
        [XmlElement ("upperCaseTable")]
        [JsonPropertyName ("upperCaseTable")]
        public string? UpperCaseTable { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Создание конфигурации из INI-файла.
        /// </summary>
        public static ServerConfiguration FromIniFile
            (
                ServerIniFile iniFile
            )
        {
            Sure.NotNull (iniFile);

            var result = new ServerConfiguration
            {
                SystemPath = iniFile.SystemPath,
                DataPath = iniFile.DataPath,
                AlphabetTablePath = iniFile.AlphabetTablePath,
                UpperCaseTable = iniFile.UpperCaseTable
            };

            return result;

        } // method FromIniFile

        /// <summary>
        /// Создание серверной конфигурации из INI-файла.
        /// </summary>
        public static ServerConfiguration FromIniFile
            (
                string fileName
            )
        {
            Sure.NotNullNorEmpty (fileName);

            using var iniFile = new IniFile
                (
                    fileName,
                    IrbisEncoding.Ansi
                );
            var serverIni = new ServerIniFile (iniFile);
            var result = FromIniFile (serverIni);

            return result;

        } // method FromIniFile

        /// <summary>
        /// Загрузка серверной конфигурации по пути до системной директории.
        /// </summary>
        public static ServerConfiguration FromPath
            (
                string path
            )
        {
            Sure.NotNullNorEmpty (path);

            var systemPath = Path.GetFullPath (path);
            systemPath = PathUtility.StripTrailingBackslash (systemPath);

            var result = new ServerConfiguration
            {
                SystemPath = systemPath + Path.DirectorySeparatorChar,
                DataPath = Path.Combine
                    (
                        systemPath,
                        "DATAI"
                        + Path.DirectorySeparatorChar
                    ),
                AlphabetTablePath = Path.Combine (systemPath, "isisacw"),
                UpperCaseTable = Path.Combine (systemPath, "isisucw")
            };

            return result;

        } // method FromPath

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Sure.NotNull (reader);

            AlphabetTablePath = reader.ReadNullableString();
            DataPath = reader.ReadNullableString();
            SystemPath = reader.ReadNullableString();
            UpperCaseTable = reader.ReadNullableString();

        } // method RestoreFromStream

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Sure.NotNull (writer);

            writer
                .WriteNullable (AlphabetTablePath)
                .WriteNullable (DataPath)
                .WriteNullable (SystemPath)
                .WriteNullable (UpperCaseTable);

        } // method SaveToStream

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify" />
        public bool Verify
            (
                bool throwOnError
            )
        {
            var verifier = new Verifier<ServerConfiguration> (this, throwOnError);

            // IRBIS64 doesn't use external upper case table

            verifier
                .DirectoryExist (SystemPath.ThrowIfNull())
                .DirectoryExist (DataPath.ThrowIfNull())
                .NotNullNorEmpty (AlphabetTablePath);

            //.NotNullNorEmpty(UpperCaseTable, "UpperCaseTable");

            return verifier.Result;

        } // method Verify

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString() => SystemPath.ToVisibleString();

        #endregion

    } // class ServerConfiguration

} // namespace ManagedIrbis.Server
