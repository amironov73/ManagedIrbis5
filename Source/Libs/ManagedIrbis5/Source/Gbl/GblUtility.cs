// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* GblUtility.cs -- вспомогательные методы для работы с глобальной корректировкой
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

using AM;
using AM.Linq;
using AM.Text;

using ManagedIrbis.Gbl.Infrastructure;
using ManagedIrbis.Infrastructure;

#endregion

#nullable enable

namespace ManagedIrbis.Gbl
{
    /// <summary>
    /// Вспомогательные методы для работы с глобальной корректировкой.
    /// </summary>
    public static class GblUtility
    {
        #region Public methods

        /// <summary>
        /// Кодирование операторов глобальной корректировки
        /// для протокола ИРБИС64.
        /// </summary>
        public static string EncodeStatements
            (
                GblStatement[] statements
            )
        {
            Sure.NotNull(statements, nameof(statements));

            var builder = StringBuilderPool.Shared.Get();
            builder.Append ("!0"); // похоже, поддержка параметров выброшена
            builder.Append (GblStatement.Delimiter);

            foreach (var statement in statements)
            {
                builder.Append (statement.EncodeForProtocol());
            }

            builder.Append (GblStatement.Delimiter);

            var result = builder.ToString();
            StringBuilderPool.Shared.Return (builder);

            return result;

        } // method EncodeStatements

        /*

        /// <summary>
        /// Restore <see cref="GblFile"/> from JSON.
        /// </summary>
        public static GblFile FromJson
            (
                string text
            )
        {
            Sure.NotNullNorEmpty(text, nameof(text));

            GblFile result = JsonConvert.DeserializeObject<GblFile>(text);

            return result;
        }

        */

        /// <summary>
        /// Десериализация <see cref="GblFile"/> из формата XML.
        /// </summary>
        public static GblFile FromXml
            (
                string text
            )
        {
            Sure.NotNull(text, nameof(text));

            var serializer = new XmlSerializer (typeof (GblFile));
            using var reader = new StringReader (text);
            var result = (GblFile?) serializer.Deserialize (reader);

            return result.ThrowIfNull (nameof (result));

        } // method FromXml

        //=================================================

        /// <summary>
        /// Построение текстового представления операторов глобальной корректировки.
        /// </summary>
        public static void NodesToText
            (
                StringBuilder builder,
                IEnumerable<GblNode> nodes
            )
        {
            var first = true;
            foreach (var node in nodes.NonNullItems())
            {
                if (!first)
                {
                    builder.Append(' ');
                }
                builder.Append(node);
                first = false;
            }

        } // method NodesToText

        //=================================================

        /*

        /// <summary>
        /// Parses the local JSON file.
        /// </summary>
        public static GblFile ParseLocalJsonFile
            (
                string fileName
            )
        {
            Sure.NotNullNorEmpty(fileName, nameof(fileName));
            string text = File.ReadAllText
                (
                    fileName,
                    IrbisEncoding.Utf8
                );
            GblFile result = FromJson(text);

            return result;
        }

        */

        /// <summary>
        /// Parses the local XML file.
        /// </summary>
        public static GblFile ParseLocalXmlFile
            (
                string fileName
            )
        {
            Sure.NotNullNorEmpty(fileName, nameof(fileName));

            var text = File.ReadAllText
                (
                    fileName,
                    IrbisEncoding.Utf8
                );
            var result = FromXml(text);

            return result;
        }

        /*

        /// <summary>
        /// Saves <see cref="GblFile"/> to local JSON file.
        /// </summary>
        public static void SaveLocalJsonFile
            (
                this GblFile gbl,
                string fileName
            )
        {
            Sure.NotNull(gbl, nameof(gbl));
            Sure.NotNullNorEmpty(fileName, nameof(fileName));

            string contents = JArray.FromObject(gbl)
                .ToString(Formatting.Indented);

            File.WriteAllText
                (
                    fileName,
                    contents,
                    IrbisEncoding.Utf8
                );
        }

        /// <summary>
        /// Convert <see cref="GblFile"/> to JSON.
        /// </summary>
        public static string ToJson
            (
                this GblFile gbl
            )
        {
            Sure.NotNull(gbl, nameof(gbl));

            string result = JObject.FromObject(gbl)
                .ToString(Formatting.None);

            return result;
        }

        */

        /// <summary>
        /// Converts the <see cref="GblFile"/> to XML.
        /// </summary>
        public static string ToXml
            (
                this GblFile gbl
            )
        {
            Sure.NotNull (gbl, nameof (gbl));

            var serializer = new XmlSerializer(typeof(GblFile));
            var writer = new StringWriter();
            serializer.Serialize(writer, gbl);

            return writer.ToString();

        } // method ToXml

        #endregion

    } // class GblUtility

} // namespace ManagedIrbis.Gbl
