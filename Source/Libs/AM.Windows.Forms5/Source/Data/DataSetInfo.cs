// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* DataSetInfo.cs -- информация о датасете
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
    /// Информация о датасете <see cref="DataSet"/>.
    /// </summary>
    [XmlRoot ("dataset")]
    public sealed class DataSetInfo
        : IHandmadeSerializable,
        IVerifiable
    {
        #region Properties

        /// <summary>
        /// Строка подключения.
        /// </summary>
        [XmlElement ("connectionString")]
        [JsonPropertyName ("connectionString")]
        public string? ConnectionString { get; set; }

        /// <summary>
        /// Датасет только для чтения?
        /// </summary>
        [XmlAttribute ("readOnly")]
        [JsonPropertyName ("readOnly")]
        public bool ReadOnly { get; set; }

        /// <summary>
        /// Команда выборки данных.
        /// </summary>
        [XmlElement ("selectCommand")]
        [JsonPropertyName ("selectCommand")]
        public string? SelectCommandText { get; set; }

        /// <summary>
        /// Список таблиц, входящих в датасет.
        /// </summary>
        [XmlElement ("table")]
        [JsonPropertyName ("tables")]
        public NonNullCollection<DataTableInfo> Tables { get; private set; }

        /// <summary>
        /// Произвольные пользовательские данные.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        [Browsable (false)]
        public object? UserData { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public DataSetInfo()
        {
            Tables = new NonNullCollection<DataTableInfo>();

        } // constructor

        #endregion

        #region Public methods

        /// <summary>
        /// Чтение информации <see cref="DataSetInfo"/> из указанного файла.
        /// </summary>
        public static DataSetInfo Load
            (
                string fileName
            )
        {
            var serializer = new XmlSerializer (typeof (DataSetInfo));
            using var stream = File.OpenRead (fileName);

            return (DataSetInfo) serializer.Deserialize (stream).ThrowIfNull();

        } // method Load

        /// <summary>
        /// Сохранение датасета в файл.
        /// </summary>
        public void Save
            (
                string fileName
            )
        {
            var serializer = new XmlSerializer (typeof (DataSetInfo));
            using var stream = File.Create (fileName);
            serializer.Serialize(stream, this);

        } // method Save

        /// <summary>
        /// Should serialize the <see cref="ReadOnly"/> field?
        /// </summary>
        [ExcludeFromCodeCoverage]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeReadOnly() => ReadOnly;

        /// <summary>
        /// Should serialize the <see cref="Tables"/> collection?
        /// </summary>
        [ExcludeFromCodeCoverage]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeTables() => Tables.Count != 0;

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

        } // method RestoreFromStream

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            writer.WriteNullable (ConnectionString);
            writer.Write (ReadOnly);
            writer.WriteNullable (SelectCommandText);
            writer.WriteCollection (Tables);

        } // method SaveToStream

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify" />
        public bool Verify
            (
                bool throwOnError
            )
        {
            var verifier = new Verifier<DataSetInfo>(this, throwOnError);

            foreach (var table in Tables)
            {
                verifier.VerifySubObject (table);
            }

            return verifier.Result;

        } // method Verify

        #endregion

    } // class DataSetInfo

} // namespace AM.Data
