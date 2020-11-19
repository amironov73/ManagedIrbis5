// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* MenuSpecification.cs -- спецификация ввода данных с помощью меню
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.IO;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;
using AM.IO;
using AM.Runtime;
using AM.Text;

using ManagedIrbis.Infrastructure;

#endregion

#nullable enable

namespace ManagedIrbis.Menus
{
    //
    // Official documentation says:
    //
    // 1 - ввод через простое меню (неиерархический справочник).
    //
    // Параметр ДОП.ИНФ. имеет следующую структуру:
    // <Menu_file_name>\<SYS|DBN>,<N>\<MnuSort> где:
    // <Menu_file_name> - имя файла справочника (с расширением);
    // <SYS|DBN>,<N> - указывает путь, по которому находится
    // файл справочника. Может принимать следующие значения:
    // SYS,0 - директория исполняемых модулей;
    // SYS,N - (N>0) рабочая директория (указываемая в параметре WORKDIR);
    // DBN,N - директория БД ввода (N - любая цифра);
    // <MnuSort> - порядок сортировки справочника:
    // 0-без сортировки;
    // 1-по значениям (по элементам меню);
    // 2-по пояснениям.
    //

    /// <summary>
    /// Спецификация ввода данных с помощью ИРБИС-меню.
    /// </summary>
    [XmlRoot("menu")]
    public sealed class MenuSpecification
        : IHandmadeSerializable,
        IVerifiable
    {
        #region Properties

        /// <summary>
        /// File name (with extension).
        /// </summary>
        [XmlAttribute("file")]
        [JsonPropertyName("file")]
        public string? FileName { get; set; }

        /// <summary>
        /// Database name.
        /// </summary>
        [XmlAttribute("db")]
        [JsonPropertyName("db")]
        public string? Database { get; set; }

        /// <summary>
        /// Path.
        /// </summary>
        [XmlAttribute("path")]
        [JsonPropertyName("path")]
        public IrbisPath Path { get; set; }

        /// <summary>
        /// Sort mode.
        /// </summary>
        [XmlAttribute("sort")]
        [JsonPropertyName("sort")]
        public int SortMode { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Convert <see cref="FileSpecification"/> to menu specification.
        /// </summary>
        public static MenuSpecification FromFileSpecification
            (
                FileSpecification specification
            )
        {
            var result = new MenuSpecification
            {
                Database = specification.Database,
                Path = specification.Path,
                FileName = specification.FileName
            };

            return result;
        }

        /// <summary>
        /// Parse the text.
        /// </summary>
        public static MenuSpecification Parse
            (
                string? text
            )
        {
            var result = new MenuSpecification
            {
                Path = IrbisPath.MasterFile
            };

            if (!string.IsNullOrEmpty(text))
            {
                var navigator = new TextNavigator(text);
                result.FileName = navigator.ReadUntil('\\').ToString();
                if (navigator.PeekChar() == '\\')
                {
                    navigator.ReadChar();
                }
                if (!navigator.IsEOF)
                {
                    var db = navigator.ReadUntil('\\').ToString();
                    if (navigator.PeekChar() == '\\')
                    {
                        navigator.ReadChar();
                    }

                    result.Database = db;

                    if (!navigator.IsEOF)
                    {
                        var sortText = navigator.GetRemainingText().ToString();
                        int.TryParse(sortText, out var sortMode);
                        result.SortMode = sortMode;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Should serialize <see cref="Database"/> field?
        /// </summary>
        public bool ShouldSerializeDatabase()
        {
            return !string.IsNullOrEmpty(Database);
        }

        /// <summary>
        /// Should serialize <see cref="SortMode"/> field?
        /// </summary>
        public bool ShouldSerializeSortMode()
        {
            return SortMode != 0;
        }

        /// <summary>
        /// Convert menu specification to <see cref="FileSpecification"/>.
        /// </summary>
        public FileSpecification ToFileSpecification()
        {
            var result = new FileSpecification
                {
                    Path = Path,
                    Database = Database,
                    FileName = FileName.ThrowIfNull("FileName")
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

            FileName = reader.ReadNullableString();
            Database = reader.ReadNullableString();
            Path = (IrbisPath) reader.ReadPackedInt32();
            SortMode = reader.ReadPackedInt32();
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Sure.NotNull(writer, nameof(writer));

            writer
                .WriteNullable(FileName)
                .WriteNullable(Database)
                .WritePackedInt32((int)Path)
                .WritePackedInt32(SortMode);
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify" />
        public bool Verify
            (
                bool throwOnError
            )
        {
            var verifier
                = new Verifier<MenuSpecification>
                    (
                        this,
                        throwOnError
                    );

            verifier
                .NotNullNorEmpty(FileName, "FileName");

            return verifier.Result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return FileName.ToVisibleString();
        }

        #endregion
    }
}
