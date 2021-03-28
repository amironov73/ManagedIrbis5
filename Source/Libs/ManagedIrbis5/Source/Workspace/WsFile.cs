// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedMember.Global

/* WsFile.cs -- рабочий лист
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;
using AM.Collections;
using AM.IO;
using AM.Runtime;

using ManagedIrbis.Client;
using ManagedIrbis.Infrastructure;

#endregion

#nullable enable

namespace ManagedIrbis.Workspace
{
    /// <summary>
    /// Рабочий лист.
    /// </summary>
    [XmlRoot("worksheet")]
    public sealed class WsFile
        : IHandmadeSerializable,
        IVerifiable
    {
        #region Properties

        /// <summary>
        /// Имя рабочего листа.
        /// </summary>
        [XmlAttribute("name")]
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        /// <summary>
        /// Страницы рабочего листа.
        /// </summary>
        [XmlArray("pages")]
        [XmlArrayItem("page")]
        [JsonPropertyName("pages")]
        public NonNullCollection<WorksheetPage> Pages { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public WsFile()
        {
            Pages = new NonNullCollection<WorksheetPage>();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Разбор потока.
        /// </summary>
        public static WsFile ParseStream
            (
                TextReader reader
            )
        {
            var result = new WsFile();
            var count = int.Parse(reader.RequireLine());

            var pairs = new Pair<string, int>[count];

            for (var i = 0; i < count; i++)
            {
                var name = reader.ReadLine();
                pairs[i] = new Pair<string, int>(name);
            }
            for (var i = 0; i < count; i++)
            {
                var text = reader.ReadLine().ThrowIfNull("text");
                var length = int.Parse(text);
                pairs[i].Second = length;
            }

            for (var i = 0; i < count; i++)
            {
                var name = pairs[i].First.ThrowIfNull("name");
                var page = WorksheetPage.ParseStream
                    (
                        reader,
                        name,
                        pairs[i].Second
                    );
                result.Pages.Add(page);
            }

            return result;
        }

        /// <summary>
        /// Read from server.
        /// </summary>
        public static WsFile? ReadFromServer
            (
                ISyncIrbisProvider provider,
                FileSpecification specification
            )
        {
            var content = provider.ReadTextFile(specification);
            if (string.IsNullOrEmpty(content))
            {
                return null;
            }

            WsFile result;

            using (var reader = new StringReader(content))
            {
                result = ParseStream(reader);
                result.Name = specification.FileName;
            }

            for (var i = 0; i < result.Pages.Count; )
            {
                var page = result.Pages[i];
                var name = page.Name.ThrowIfNull("page.Name");
                if (name.StartsWith("@"))
                {
                    var extension = Path.GetExtension(specification.FileName);
                    var nestedSpecification = new FileSpecification
                        {
                            Path = specification.Path,
                            Database = specification.Database,
                            FileName = name.Substring(1) + extension
                        };
                    var nestedFile = ReadFromServer
                        (
                            provider,
                            nestedSpecification
                        );
                    if (ReferenceEquals(nestedFile, null))
                    {
                        // TODO: somehow report error
                        i++;
                    }
                    else
                    {
                        result.Pages.RemoveAt(i);
                        for (var j = 0; j < nestedFile.Pages.Count; j++)
                        {
                            result.Pages.Insert
                                (
                                    i + j,
                                    nestedFile.Pages[j]
                                );
                        }
                    }
                }
                else
                {
                    i++;
                }
            }

            return result;
        }

        /// <summary>
        /// Fixup nested worksheets for local file.
        /// </summary>
        public static WsFile FixupLocalFile
            (
                string fileName,
                Encoding encoding,
                WsFile wsFile
            )
        {
            for (var i = 0; i < wsFile.Pages.Count; )
            {
                var page = wsFile.Pages[i];
                var name = page.Name.ThrowIfNull("page.Name");
                if (name.StartsWith("@"))
                {
                    var directory = Path.GetDirectoryName(fileName)
                                    ?? string.Empty;
                    var extension = Path.GetExtension(fileName);
                    var nestedName = Path.Combine
                        (
                            directory,
                            name.Substring(1) + extension
                        );
                    var nestedFile = ReadLocalFile
                        (
                            nestedName,
                            encoding
                        );
                    wsFile.Pages.RemoveAt(i);
                    for (var j = 0; j < nestedFile.Pages.Count; j++)
                    {
                        wsFile.Pages.Insert
                            (
                                i + j,
                                nestedFile.Pages[j]
                            );
                    }
                }
                else
                {
                    i++;
                }
            }

            return wsFile;
        }

        /// <summary>
        /// Считывание из локального файла.
        /// </summary>
        public static WsFile ReadLocalFile
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
            result.Name = Path.GetFileName(fileName);

            return result;
        }

        /// <summary>
        /// Считывание из локального файла.
        /// </summary>
        public static WsFile ReadLocalFile
            (
                string fileName
            )
        {
            return ReadLocalFile
                (
                    fileName,
                    IrbisEncoding.Ansi
                );
        }

        /// <summary>
        /// Should serialize the <see cref="Pages"/> collection?
        /// </summary>
        [ExcludeFromCodeCoverage]
        public bool ShouldSerializePages()
        {
            return Pages.Count != 0;
        }

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Name = reader.ReadNullableString();
            Pages = reader.ReadNonNullCollection<WorksheetPage>();
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            writer.WriteNullable(Name);
            writer.Write(Pages);
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify" />
        public bool Verify
            (
                bool throwOnError
            )
        {
            var verifier = new Verifier<WsFile>(this, throwOnError);

            foreach (var page in Pages)
            {
                verifier.VerifySubObject(page, "page");
            }

            return verifier.Result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return Name.ToVisibleString();
        }

        #endregion
    }
}
