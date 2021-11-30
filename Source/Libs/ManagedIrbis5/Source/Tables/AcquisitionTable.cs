// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* AcquisitionTable.cs -- описание таблицы для комплектования в ИРБИС64
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.ComponentModel;
using System.IO;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;
using AM.IO;
using AM.Runtime;

#endregion

#nullable enable

namespace ManagedIrbis.Tables
{
    /// <summary>
    /// Описание таблицы для комплектования в ИРБИС64.
    /// </summary>
    [XmlRoot ("table")]
    public sealed class AcquisitionTable
        : IHandmadeSerializable,
            IVerifiable
    {
        #region Properties

        /// <summary>
        /// 1-я строка - имя таблицы.
        /// </summary>
        [XmlElement ("name")]
        [JsonPropertyName ("name")]
        [Description ("Имя таблицы")]
        public string? TableName { get; set; }

        /// <summary>
        /// 2-я строка - способ отбора записей.
        /// </summary>
        [XmlAttribute ("selectionMethod")]
        [JsonPropertyName ("selectionMethod")]
        [Description ("Способ отбора записей")]
        public int SelectionMethod { get; set; }

        /// <summary>
        /// 3-я строка - имя опросного рабочего листа,
        /// в котором задаются параметры для отбора записей
        /// и для построения значения модельного поля.
        /// </summary>
        [XmlElement ("worksheet")]
        [JsonPropertyName ("worksheet")]
        [Description ("Имя опросного рабочего листа")]
        public string? Worksheet { get; set; }

        /// <summary>
        /// 4-я строка - формат.
        /// </summary>
        [XmlElement ("format")]
        [JsonPropertyName ("format")]
        [Description ("Формат")]
        public string? Format { get; set; }

        /// <summary>
        /// 5-я строка – формат, который «фильтрует» отобранные записи.
        /// </summary>
        [XmlElement ("filter")]
        [JsonPropertyName ("filter")]
        [Description ("Фильтрующий формат")]
        public string? Filter { get; set; }

        /// <summary>
        /// 6-я – формат для определения значения
        /// модельного поля с меткой 991.
        /// </summary>
        [XmlElement ("modelField")]
        [JsonPropertyName ("modelField")]
        [Description ("Формат для поля 991")]
        public string? ModelField { get; set; }

        /// <summary>
        /// Произвольные пользовательские данные.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        [Browsable (false)]
        public object? UserData { get; set; }

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream"/>
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Sure.NotNull (reader);

            TableName = reader.ReadNullableString();
            SelectionMethod = reader.ReadPackedInt32();
            Worksheet = reader.ReadNullableString();
            Format = reader.ReadNullableString();
            Filter = reader.ReadNullableString();
            ModelField = reader.ReadNullableString();
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream"/>
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Sure.NotNull (writer);

            writer
                .WriteNullable (TableName)
                .WritePackedInt32 (SelectionMethod)
                .WriteNullable (Worksheet)
                .WriteNullable (Format)
                .WriteNullable (Filter)
                .WriteNullable (ModelField);
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify"/>
        public bool Verify
            (
                bool throwOnError
            )
        {
            var verifier = new Verifier<AcquisitionTable> (this, throwOnError);

            // TODO implement

            verifier
                .NotNullNorEmpty (TableName)
                .NotNullNorEmpty (Worksheet)
                .NotNullNorEmpty (Format);

            return verifier.Result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return TableName.ToVisibleString();
        }

        #endregion
    }
}
