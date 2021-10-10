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

using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;

#endregion

#nullable enable

namespace ManagedIrbis.Tables
{
    /// <summary>
    /// Описание таблицы для комплектования в ИРБИС64.
    /// </summary>
    [XmlRoot ("table")]
    public sealed class AcquisitionTable
    {
        #region Properties

        /// <summary>
        /// 1-я строка - имя таблицы.
        /// </summary>
        [XmlElement ("name")]
        [JsonPropertyName ("name")]
        public string? TableName { get; set; }

        /// <summary>
        /// 2-я строка - способ отбора записей.
        /// </summary>
        [XmlElement ("selectionMethod")]
        [JsonPropertyName ("selectionMethod")]
        public int SelectionMethod { get; set; }

        /// <summary>
        /// 3-я строка - имя опросного рабочего листа,
        /// в котором задаются параметры для отбора записей
        /// и для построения значения модельного поля.
        /// </summary>
        [XmlElement ("worksheet")]
        [JsonPropertyName ("worksheet")]
        public string? Worksheet { get; set; }

        /// <summary>
        /// 4-я строка - формат.
        /// </summary>
        [XmlElement ("format")]
        [JsonPropertyName ("format")]
        public string? Format { get; set; }

        /// <summary>
        /// 5-я строка – формат, который «фильтрует» отобранные записи.
        /// </summary>
        [XmlElement ("filter")]
        [JsonPropertyName ("filter")]
        public string? Filter { get; set; }

        /// <summary>
        /// 6-я – формат для определения значения
        /// модельного поля с меткой 991.
        /// </summary>
        [XmlElement ("modelField")]
        [JsonPropertyName ("modelField")]
        public string? ModelField { get; set; }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString() => TableName.ToVisibleString();

        #endregion

    } // class AcquisitionTable

} // namespace ManagedIrbis.Tables
