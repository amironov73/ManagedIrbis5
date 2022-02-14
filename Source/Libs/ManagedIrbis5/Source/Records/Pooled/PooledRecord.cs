// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable ReplaceSliceWithRangeIndexer
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local

/* PooledRecord.cs -- библиографическая запись
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

#endregion

#nullable enable

namespace ManagedIrbis.Records;

/// <summary>
/// Библиографическая запись, адаптированная для пулинга.
/// </summary>
[XmlRoot ("record")]
public sealed class PooledRecord
{
    #region Properties

    /// <summary>
    /// База данных, в которой хранится запись.
    /// Для вновь созданных записей -- <c>null</c>.
    /// </summary>
    [XmlAttribute ("database")]
    [JsonPropertyName ("database")]
    public string? Database { get; set; }

    /// <summary>
    /// MFN (порядковый номер в базе данных) записи.
    /// Для вновь созданных записей равен <c>0</c>.
    /// Для хранящихся в базе записей нумерация начинается
    /// с <c>1</c>.
    /// </summary>
    [XmlAttribute ("mfn")]
    [JsonPropertyName ("mfn")]
    public int Mfn { get; set; }

    /// <summary>
    /// Версия записи. Для вновь созданных записей равна <c>0</c>.
    /// Для хранящихся в базе записей нумерация версий начинается
    /// с <c>1</c>.
    /// </summary>
    [XmlAttribute ("version")]
    [JsonPropertyName ("version")]
    public int Version { get; set; }

    /// <summary>
    /// Статус записи. Для вновь созданных записей <c>None</c>.
    /// </summary>
    [XmlAttribute ("status")]
    [JsonPropertyName ("status")]
    public RecordStatus Status { get; set; }

    /// <summary>
    /// Признак -- запись помечена как логически удаленная.
    /// </summary>
    public bool Deleted => (Status & Record.IsDeleted) != 0;

    /// <summary>
    /// Список полей.
    /// </summary>
    [XmlElement ("field")]
    [JsonPropertyName ("fields")]
    public List<PooledField> Fields { get; private set; } = default!;

    /// <summary>
    /// Описание в произвольной форме (опциональное).
    /// </summary>
    [XmlIgnore]
    [JsonIgnore]
    public string? Description { get; set; }

    /// <summary>
    /// Признак того, что запись модифицирована.
    /// </summary>
    [XmlIgnore]
    [JsonIgnore]
    public bool Modified { get; internal set; }

    #endregion

    #region Public methods

    /// <summary>
    /// Инициализация.
    /// </summary>
    public void Init()
    {
        Mfn = default;
        Version = 0;
        Status = RecordStatus.None;
        Fields = new();
        Description = null;
        Modified = false;
    }

    /// <summary>
    /// Возврат в пул.
    /// </summary>
    public void Dispose()
    {
        Mfn = default;
        Version = 0;
        Status = RecordStatus.None;
        Fields = null!;
        Description = null;
        Modified = false;
    }

    #endregion
}
