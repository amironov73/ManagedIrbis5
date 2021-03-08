// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* ServerConfiguration.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.IO;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
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
    ///
    /// </summary>
    [XmlRoot("configuration")]
    public sealed class ServerConfiguration
        : IHandmadeSerializable,
        IVerifiable
    {
        #region Properties

        /// <summary>
        /// Path for AlphabetTable (without extension).
        /// </summary>
        [XmlElement("alphabetTablePath")]
        [JsonPropertyName("alphabetTablePath")]
        public string? AlphabetTablePath { get; set; }

        /// <summary>
        /// Data path.
        /// </summary>
        [XmlElement("dataPath")]
        [JsonPropertyName("dataPath")]
        public string? DataPath { get; set; }

        /// <summary>
        /// System path.
        /// </summary>
        [XmlElement("systemPath")]
        [JsonPropertyName("systemPath")]
        public string? SystemPath { get; set; }

        /// <summary>
        /// Path for UpperCaseTable (without extension).
        /// </summary>
        [XmlElement("upperCaseTable")]
        [JsonPropertyName("upperCaseTable")]
        public string? UpperCaseTable { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Create server configuration from INI-file.
        /// </summary>
        public static ServerConfiguration FromIniFile
            (
                ServerIniFile iniFile
            )
        {
            Sure.NotNull(iniFile, nameof(iniFile));

            var result = new ServerConfiguration
            {
                SystemPath = iniFile.SystemPath,
                DataPath = iniFile.DataPath,
                AlphabetTablePath = iniFile.AlphabetTablePath,
                UpperCaseTable = iniFile.UpperCaseTable
            };

            return result;
        }

        /// <summary>
        /// Create server configuration from INI file.
        /// </summary>
        public static ServerConfiguration FromIniFile
            (
                string fileName
            )
        {
            Sure.NotNullNorEmpty(fileName, nameof(fileName));

            using var iniFile = new IniFile
                (
                    fileName,
                    IrbisEncoding.Ansi,
                    false
                );
            ServerIniFile serverIni = new ServerIniFile(iniFile);
            ServerConfiguration result = FromIniFile(serverIni);

            return result;
        }

        /// <summary>
        /// Create server configuration from path.
        /// </summary>
        public static ServerConfiguration FromPath
            (
                string path
            )
        {
            Sure.NotNullNorEmpty(path, nameof(path));

            string systemPath = Path.GetFullPath(path);
            systemPath = PathUtility.StripTrailingBackslash(systemPath);

            ServerConfiguration result = new ServerConfiguration
            {
                SystemPath = systemPath
                    + Path.DirectorySeparatorChar,
                DataPath = Path.Combine
                    (
                        systemPath,
                        "DATAI"
                        + Path.DirectorySeparatorChar
                    ),
                AlphabetTablePath = Path.Combine(systemPath, "isisacw"),
                UpperCaseTable = Path.Combine(systemPath, "isisucw")
            };

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
            Sure.NotNull(reader, nameof(reader));

            AlphabetTablePath = reader.ReadNullableString();
            DataPath = reader.ReadNullableString();
            SystemPath = reader.ReadNullableString();
            UpperCaseTable = reader.ReadNullableString();
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Sure.NotNull(writer, nameof(writer));

            writer
                .WriteNullable(AlphabetTablePath)
                .WriteNullable(DataPath)
                .WriteNullable(SystemPath)
                .WriteNullable(UpperCaseTable);
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify" />
        public bool Verify(bool throwOnError)
        {
            var verifier = new Verifier<ServerConfiguration>(this, throwOnError);

            // IRBIS64 doesn't use external upper case table

            verifier
                .DirectoryExist
                    (
                        SystemPath.ThrowIfNull(nameof(SystemPath)),
                        nameof(SystemPath)
                    )
                .DirectoryExist
                    (
                        DataPath.ThrowIfNull(nameof(DataPath)),
                        nameof(DataPath)
                    )
                .NotNullNorEmpty
                    (
                        AlphabetTablePath,
                        nameof(AlphabetTablePath)
                    );
                //.NotNullNorEmpty(UpperCaseTable, "UpperCaseTable");

            return verifier.Result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString() => SystemPath.ToVisibleString();

        #endregion

    } // class ServerConfiguration

} // namespace ManagedIrbis.Server
