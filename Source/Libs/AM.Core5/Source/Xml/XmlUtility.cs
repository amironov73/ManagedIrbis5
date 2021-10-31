// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local

/* XmlUtility.cs -- возня с XML
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

#endregion

#nullable enable

namespace AM.Xml
{
    /// <summary>
    /// Возня с XML.
    /// </summary>
    public static class XmlUtility
    {
        #region Public methods

        /// <summary>
        /// Deserialize object from file.
        /// </summary>
        public static T DeserializeFile<T>
            (
                string fileName
            )
        {
            Sure.FileExists (fileName);

            var serializer = new XmlSerializer (typeof (T));
            using Stream stream = File.OpenRead (fileName);

            return (T)serializer.Deserialize (stream).ThrowIfNull();

        } // method DeserializeFile

        /// <summary>
        /// Deserialize the string.
        /// </summary>
        public static T DeserializeString<T>
            (
                string xmlText
            )
        {
            Sure.NotNullNorEmpty (xmlText, nameof (xmlText));

            var reader = new StringReader (xmlText);
            var serializer = new XmlSerializer (typeof (T));

            return (T)serializer.Deserialize (reader).ThrowIfNull ();

        } // method DeserializeString

        /// <summary>
        /// Serialize object to file.
        /// </summary>
        public static void SerializeToFile<T>
            (
                string fileName,
                T obj
            )
            where T : class
        {
            Sure.NotNullNorEmpty (fileName);

            var settings = new XmlWriterSettings
            {
                OmitXmlDeclaration = true,
                Indent = true
            };
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add (string.Empty, string.Empty);
            using var output = new StreamWriter (fileName);
            using var writer = XmlWriter.Create (output, settings);
            var objType = obj.GetType();
            var serializer = new XmlSerializer (objType);
            serializer.Serialize (writer, obj, namespaces);

        } // method SerializeToFile

        /// <summary>
        /// Serialize to string without standard
        /// XML header and namespaces.
        /// </summary>
        public static string SerializeShort
            (
                object obj
            )
        {
            Sure.NotNull (obj);

            var settings = new XmlWriterSettings
            {
                OmitXmlDeclaration = true,
                Indent = false,
                NewLineHandling = NewLineHandling.None
            };
            var output = new StringBuilder();
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add (string.Empty, string.Empty);
            using (var writer = XmlWriter.Create (output, settings))
            {
                var serializer = new XmlSerializer (obj.GetType());
                serializer.Serialize (writer, obj, namespaces);
            }

            return output.ToString();

        } // method SerializeShort

        #endregion

    } // class XmlUtility

} // nameof AM.Xml
