// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* ChairInfo.cs -- кафедра обслуживания
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
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

namespace ManagedIrbis.Readers
{
    /// <summary>
    /// Информация о кафедре обслуживания.
    /// </summary>
    [XmlRoot("chair")]
    [DebuggerDisplay("{Code} {Title}")]
    public sealed class ChairInfo
        : IHandmadeSerializable
    {
        #region Constants

        /// <summary>
        /// Имя меню с кафедрами по умолчанию.
        /// </summary>
        public const string ChairMenu = "kv.mnu";

        /// <summary>
        /// Имя меню с местами хранения по умолчанию.
        /// </summary>
        public const string PlacesMenu = "mhr.mnu";

        #endregion

        #region Properties

        /// <summary>
        /// Код.
        /// </summary>
        [XmlAttribute("code")]
        [JsonPropertyName("code")]
        public string? Code { get; set; }

        /// <summary>
        /// Название.
        /// </summary>
        [XmlAttribute("title")]
        [JsonPropertyName("title")]
        public string? Title { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ChairInfo()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="code">Chair code.</param>
        public ChairInfo
            (
                string code
            )
        {
            Code = code;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="code">Chair code.</param>
        /// <param name="title">Chair title.</param>
        public ChairInfo
            (
                string code,
                string title
            )
        {
            Code = code;
            Title = title;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Разбор текста меню-файла.
        /// </summary>
        public static ChairInfo[] Parse
            (
                string text,
                bool addAllItem
            )
        {
            var result = new List<ChairInfo>();
            var lines = text.SplitLines();
            for (var i = 0; i < lines.Length; i += 2)
            {
                if (lines[i].StartsWith("*"))
                {
                    break;
                }

                var item = new ChairInfo
                    {
                        Code = lines[i],
                        Title = lines[i + 1]
                    };
                result.Add(item);
            }

            if (addAllItem)
            {
                result.Add
                    (
                        new ChairInfo
                            {
                                Code = "*",
                                Title = "Все подразделения"
                            }
                    );
            }

            return result
                .OrderBy(item => item.Code)
                .ToArray();
        } // method Parse

        /// <summary>
        /// Загрузка перечня кафедр обслуживания с сервера.
        /// </summary>
        public static async Task<ChairInfo[]> ReadAsync
            (
                IIrbisConnection connection,
                string fileName = ChairMenu,
                bool addAllItem = true
            )
        {
            var specification = FileSpecification.Build
                (
                    IrbisPath.MasterFile,
                    connection.Database,
                    fileName
                );
            var content = await connection.ReadTextFileAsync
                (
                    specification
                );

            if (string.IsNullOrEmpty(content))
            {
                Magna.Error
                    (
                        "ChairInfo::ReadAsync: "
                        + "file is missing or empty: "
                        + fileName
                    );

                throw new IrbisException();
            }

            var result = Parse(content, addAllItem);

            return result;

        } // method ReadAsync

        /// <summary>
        /// Should serialize the <see cref="Title"/> field?
        /// </summary>
        [ExcludeFromCodeCoverage]
        public bool ShouldSerializeTitle()
        {
            return !string.IsNullOrEmpty(Title);
        }

        #endregion

        #region IHandmadeSerializable

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            writer.WriteNullable(Code);
            writer.WriteNullable(Title);
        } // method SaveToStream

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Code = reader.ReadNullableString();
            Title = reader.ReadNullableString();
        } // method RestoreFromStream

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString() =>
            string.IsNullOrEmpty(Title) ? Code.ToVisibleString()
                : $"{Code.ToVisibleString()} - {Title.ToVisibleString()}";

        #endregion

    } // class ChairInfo

} // namespace ManagedIrbis.Readers
