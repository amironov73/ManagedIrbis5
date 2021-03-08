// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* GblUtility.cs -- utility routines for GBL handling
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

using AM;

using ManagedIrbis.Gbl.Infrastructure;
using ManagedIrbis.Infrastructure;

#endregion

#nullable enable

namespace ManagedIrbis.Gbl
{
    /// <summary>
    /// Utility routines for GBL file handling.
    /// </summary>
    public static class GblUtility
    {
        #region Public methods

        /// <summary>
        /// Encode statements for IRBIS64 protocol.
        /// </summary>
        public static string EncodeStatements
            (
                GblStatement[] statements
            )
        {
            Sure.NotNull(statements, nameof(statements));

            StringBuilder result = new StringBuilder();
            result.Append("!0");
            result.Append(GblStatement.Delimiter);

            foreach (GblStatement item in statements)
            {
                result.Append(item.EncodeForProtocol());
            }
            result.Append(GblStatement.Delimiter);

            return result.ToString();
        }

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
        /// Restore <see cref="GblFile"/> from JSON.
        /// </summary>
        public static GblFile FromXml
            (
                string text
            )
        {
            Sure.NotNull(text, nameof(text));

            XmlSerializer serializer = new XmlSerializer(typeof(GblFile));
            using (StringReader reader = new StringReader(text))
            {
                var result = (GblFile?) serializer.Deserialize(reader);

                return result.ThrowIfNull("result");
            }
        }

        //=================================================

        /// <summary>
        /// Build text representation of <see cref="GblNode"/>'s.
        /// </summary>
        public static void NodesToText
            (
                StringBuilder builder,
                IEnumerable<GblNode> nodes
            )
        {
            Sure.NotNull(builder, nameof(builder));
            Sure.NotNull(nodes, nameof(nodes));

            bool first = true;
            foreach (GblNode node in nodes.NonNullItems())
            {
                if (!first)
                {
                    builder.Append(' ');
                }
                builder.Append(node);
                first = false;
            }
        }

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

            string text = File.ReadAllText
                (
                    fileName,
                    IrbisEncoding.Utf8
                );
            GblFile result = FromXml(text);

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
            Sure.NotNull(gbl, nameof(gbl));

            XmlSerializer serializer = new XmlSerializer(typeof(GblFile));
            StringWriter writer = new StringWriter();
            serializer.Serialize(writer, gbl);

            return writer.ToString();
        }

        #endregion

    } // class GblUtility

} // namespace ManagedIrbis.Gbl
