// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* MenuFile.cs -- работа с MNU-файлами
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Serialization;

using AM;
using AM.Collections;
using AM.IO;
using AM.Runtime;

using ManagedIrbis.Infrastructure;

#endregion

#nullable enable

namespace ManagedIrbis.Menus
{
    /// <summary>
    /// Работа с MNU-файлами.
    /// </summary>
    [XmlRoot("menu")]
    public sealed class MenuFile
        : IHandmadeSerializable
    {
        #region Constants

        /// <summary>
        /// End of menu marker.
        /// </summary>
        public const string StopMarker = "*****";

        #endregion

        #region Properties

        /// <summary>
        /// Name of menu file -- for identification
        /// purposes only.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public string? FileName { get; set; }

        /// <summary>
        /// Gets the entries.
        /// </summary>
        [XmlElement("entry")]
        [JsonPropertyName("entries")]
        public NonNullCollection<MenuEntry> Entries { get; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public MenuFile()
        {
            Entries = new NonNullCollection<MenuEntry>();
        } // constructor

        /// <summary>
        /// Internal constructor.
        /// </summary>
        internal MenuFile
            (
                NonNullCollection<MenuEntry> entries
            )
        {
            Entries = entries;
        } // constructor

        #endregion

        #region Private members

        /// <summary>
        /// Separators for the menu entries.
        /// </summary>
        public static char[] MenuSeparators = { ' ', '-', '=', ':' };

        #endregion

        #region Public methods

        /// <summary>
        /// Adds the specified code and comment.
        /// </summary>
        public MenuFile Add
            (
                string code,
                string? comment
            )
        {
            var entry = new MenuEntry
            {
                Code = code,
                Comment = comment
            };
            Entries.Add(entry);

            return this;
        } // method Add

        /// <summary>
        /// Trims the code.
        /// </summary>
        public static string TrimCode
            (
                string code
            )
        {
            code = code.Trim();
            var parts = code.Split(MenuSeparators);
            if (parts.Length != 0)
            {
                code = parts[0];
            }

            return code;
        } // method TrimCode

        /// <summary>
        /// Finds the entry.
        /// </summary>
        public MenuEntry? FindEntry (string code) =>
            Entries.FirstOrDefault (entry => entry.Code.SameString(code));

        /// <summary>
        /// Finds the entry (case sensitive).
        /// </summary>
        public MenuEntry? FindEntrySensitive (string code) =>
            Entries.FirstOrDefault(entry => string.CompareOrdinal (entry.Code, code) == 0);

        /// <summary>
        /// Finds the entry.
        /// </summary>
        public MenuEntry? GetEntry
            (
                string code
            )
        {
            var result = FindEntry(code);
            if (!ReferenceEquals(result, null))
            {
                return result;
            }

            code = code.Trim();
            result = FindEntry(code);
            if (!ReferenceEquals(result, null))
            {
                return result;
            }

            code = TrimCode(code);
            result = FindEntry(code);

            return result;
        } // method GetEntry

        /// <summary>
        /// Finds the entry (case sensitive).
        /// </summary>
        public MenuEntry? GetEntrySensitive
            (
                string code
            )
        {
            var result = FindEntrySensitive(code);
            if (!ReferenceEquals(result, null))
            {
                return result;
            }

            code = code.Trim();
            result = FindEntrySensitive(code);
            if (!ReferenceEquals(result, null))
            {
                return result;
            }

            code = TrimCode(code);
            result = FindEntrySensitive(code);

            return result;
        } // method GetEntrySensitive

        /// <summary>
        /// Finds comment by the code.
        /// </summary>
        public string? GetString
            (
                string code,
                string? defaultValue = null
            )
        {
            var found = FindEntry(code);

            return ReferenceEquals(found, null)
                ? defaultValue
                : found.Comment;
        } // method GetString

        /// <summary>
        /// Finds comment by the code (case sensitive).
        /// </summary>
        public string? GetStringSensitive
            (
                string code,
                string? defaultValue = null
            )
        {
            var found = FindEntrySensitive(code);

            return ReferenceEquals(found, null)
                ? defaultValue
                : found.Comment;
        } // method GetStringSensitive

        /// <summary>
        /// Parses the specified stream.
        /// </summary>
        public static MenuFile ParseStream
            (
                TextReader reader
            )
        {
            var result = new MenuFile();

            while (true)
            {
                var code = reader.ReadLine();
                if (string.IsNullOrEmpty(code))
                {
                    break;
                }
                if (code.StartsWith(StopMarker))
                {
                    break;
                }

                var comment = reader.RequireLine();
                var entry = new MenuEntry
                {
                    Code = code,
                    Comment = comment
                };
                result.Entries.Add(entry);
            }

            return result;
        } // method ParseStream

        /// <summary>
        /// Parses the local file.
        /// </summary>
        public static MenuFile ParseLocalFile
            (
                string fileName,
                Encoding encoding
            )
        {
            using var reader = TextReaderUtility.OpenRead
                (
                    fileName,
                    encoding
                );
            var result = ParseStream(reader);
            result.FileName = Path.GetFileName(fileName);

            return result;
        } // method ParseLocalFile

        /// <summary>
        /// Parses the local file.
        /// </summary>
        public static MenuFile ParseLocalFile ( string fileName ) =>
            ParseLocalFile(fileName, IrbisEncoding.Ansi);

        // /// <summary>
        // /// Parse server response.
        // /// </summary>
        // public static MenuFile ParseServerResponse
        //     (
        //         Response response
        //     )
        // {
        //     var reader = response.GetReader(IrbisEncoding.Ansi);
        //     var result = ParseStream(reader);
        //
        //     return result;
        // }

        /// <summary>
        /// Parse server response.
        /// </summary>
        public static MenuFile ParseServerResponse
            (
                string response
            )
        {
            var reader = new StringReader(response);
            var result = ParseStream(reader);

            return result;
        } // method ParseServerResponse

        /// <summary>
        /// Read <see cref="MenuFile"/> from server.
        /// </summary>
        public static MenuFile? ReadFromServer
            (
                ISyncProvider connection,
                FileSpecification fileSpecification
            )
        {
            var response = connection.ReadTextFile(fileSpecification);
            if (string.IsNullOrEmpty(response))
            {
                return null;
            }

            var result = ParseServerResponse(response);

            return result;
        } // method ReadFromServer

        /// <summary>
        /// Sorts the entries.
        /// </summary>
        public MenuEntry[] SortEntries
            (
                MenuSort sortBy
            )
        {
            var copy = new List<MenuEntry>(Entries);
            switch (sortBy)
            {
                case MenuSort.None:
                    // Nothing to do
                    break;

                case MenuSort.ByCode:
                    copy = copy.OrderBy(entry => entry.Code).ToList();
                    break;

                case MenuSort.ByComment:
                    copy = copy.OrderBy(entry => entry.Comment).ToList();
                    break;

                default:
                    Magna.Error
                        (
                            nameof(MenuFile) + "::" + nameof(SortEntries)
                            + ": unexpected sortBy="
                            + sortBy
                        );
                    throw new IrbisException("Unexpected sortBy=" + sortBy);
            }

            return copy.ToArray();
        } // method SortEntries

        /// <summary>
        /// Builds text representation.
        /// </summary>
        public string ToText()
        {
            var result = new StringBuilder();

            foreach (var entry in Entries)
            {
                result.AppendLine(entry.Code);
                result.AppendLine(entry.Comment);
            }
            result.AppendLine(StopMarker);

            return result.ToString();
        } // method ToText

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            FileName = reader.ReadNullableString();
            reader.ReadCollection(Entries);
        } // method RestoreFromStream

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            writer.WriteNullable(FileName);
            writer.Write(Entries);
        } // method SaveToStream

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString() => FileName.ToVisibleString();

        #endregion

    } // class MenuFile

} // namespace ManagedIrbis.Menus
