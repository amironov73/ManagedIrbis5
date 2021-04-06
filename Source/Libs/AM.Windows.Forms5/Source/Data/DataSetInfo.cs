// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* DataSetInfo.cs -- information about dataset
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.ComponentModel;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM.Collections;
using AM.IO;
using AM.Runtime;

#endregion

#nullable enable

namespace AM.Data
{
    /// <summary>
    /// Information about <see cref="DataSet"/>.
    /// </summary>
    [XmlRoot("dataset")]
    public class DataSetInfo
        : IHandmadeSerializable,
        IVerifiable
    {
        #region Properties

        /// <summary>
        /// Gets or sets the connection string.
        /// </summary>
        [XmlElement("connectionString")]
        [JsonPropertyName("connectionString")]
        public string? ConnectionString { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether
        /// the dataset is read only.
        /// </summary>
        [XmlAttribute("readOnly")]
        [JsonPropertyName("readOnly")]
        public bool ReadOnly { get; set; }

        /// <summary>
        /// Gets or sets the select command text.
        /// </summary>
        /// <value>The select command text.</value>
        [XmlElement("selectCommand")]
        [JsonPropertyName("selectCommand")]
        public string? SelectCommandText { get; set; }

        /// <summary>
        /// Gets the table list.
        /// </summary>
        [XmlElement("table")]
        [JsonPropertyName("tables")]
        public NonNullCollection<DataTableInfo> Tables
        {
            get; private set;
        }

        /// <summary>
        /// Arbitrary user data.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        [Browsable(false)]
        public object? UserData { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public DataSetInfo()
        {
            Tables = new NonNullCollection<DataTableInfo>();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Loads <see cref="DataSetInfo"/> from the specified file.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        public static DataSetInfo Load
            (
                string fileName
            )
        {
            XmlSerializer serializer = new XmlSerializer(typeof(DataSetInfo));
            using (Stream stream = File.OpenRead(fileName))
            {
                return (DataSetInfo)serializer.Deserialize(stream);
            }
        }

        /// <summary>
        /// Saves this instance into the specified file.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        public void Save
            (
                string fileName
            )
        {
            XmlSerializer serializer = new XmlSerializer(typeof(DataSetInfo));
            using (Stream stream = File.Create(fileName))
            {
                serializer.Serialize(stream, this);
            }
        }

        /// <summary>
        /// Should serialize the <see cref="ReadOnly"/> field?
        /// </summary>
        [ExcludeFromCodeCoverage]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeReadOnly()
        {
            return ReadOnly;
        }

        /// <summary>
        /// Should serialize the <see cref="Tables"/> collection?
        /// </summary>
        [ExcludeFromCodeCoverage]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeTables()
        {
            return Tables.Count != 0;
        }

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            ConnectionString = reader.ReadNullableString();
            ReadOnly = reader.ReadBoolean();
            SelectCommandText = reader.ReadNullableString();
            Tables = reader.ReadNonNullCollection<DataTableInfo>();
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            /*

            writer.WriteNullable(ConnectionString);
            writer.Write(ReadOnly);
            writer.WriteNullable(SelectCommandText);
            writer.WriteCollection(Tables);

            */
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify" />
        public bool Verify
            (
                bool throwOnError
            )
        {
            var verifier = new Verifier<DataSetInfo>(this, throwOnError);

            foreach (DataTableInfo table in Tables)
            {
                verifier.VerifySubObject(table, "Table");
            }

            return verifier.Result;
        }

        #endregion
    }
}
