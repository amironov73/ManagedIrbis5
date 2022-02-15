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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;
using AM.Text;

using ManagedIrbis.Direct;
using ManagedIrbis.Infrastructure;

#endregion

#nullable enable

namespace ManagedIrbis.Records;

/// <summary>
/// Библиографическая запись, адаптированная для пулинга.
/// </summary>
[XmlRoot ("record")]
public sealed class PooledRecord
    : IRecord,
    IDisposable
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

    #region Private members

    /// <summary>
    /// Пул, из которого взята запись.
    /// </summary>
    internal RecordPool _pool = default!;

    #endregion

    #region IRecord members

    /// <inheritdoc cref="IRecord.Decode(ManagedIrbis.Infrastructure.Response)"/>
    public void Decode
        (
            Response response
        )
    {
        Sure.NotNull (response);

        throw new NotImplementedException();
    }

    /// <inheritdoc cref="IRecord.Decode(ManagedIrbis.Direct.MstRecord64)"/>
    public void Decode
        (
            MstRecord64 record
        )
    {
        Sure.NotNull (record);

        throw new NotImplementedException();
    }

    /// <inheritdoc cref="IRecord.Encode(string?)"/>
    public string Encode
        (
            string? delimiter = IrbisText.IrbisDelimiter
        )
    {
        var builder = StringBuilderPool.Shared.Get();
        builder.EnsureCapacity (512);

        builder.Append (Mfn.ToInvariantString())
            .Append ('#')
            .Append (((int)Status).ToInvariantString())
            .Append (delimiter)
            .Append ("0#")
            .Append (Version.ToInvariantString())
            .Append (delimiter);

        foreach (var field in Fields)
        {
            builder.Append (field).Append (delimiter);
        }

        var result = builder.ToString();
        StringBuilderPool.Shared.Return (builder);

        return result;
    }

    /// <inheritdoc cref="IRecord.Encode(ManagedIrbis.Direct.MstRecord64)"/>
    public void Encode
        (
            MstRecord64 record
        )
    {
        Sure.NotNull (record);

        throw new NotImplementedException();
    }

    /// <inheritdoc cref="IRecord.FM"/>
    public string? FM
        (
            int tag
        )
    {
        Sure.Positive (tag);

        foreach (var field in Fields)
        {
            if (field.Tag == tag)
            {
                return field.Subfields.FirstOrDefault()?.Value;
            }
        }

        return null;
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Добавление поля.
    /// </summary>
    /// <param name="tag">Метка добавляемого поля.</param>
    public PooledRecord Add
        (
            int tag
        )
    {
        Sure.Positive (tag);

        var field = _pool.GetField (tag);
        Fields.Add (field);

        return this;
    }

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

        // ReSharper disable ConditionIsAlwaysTrueOrFalse
        if (Fields is not null)
        // ReSharper restore ConditionIsAlwaysTrueOrFalse
        {
            foreach (var field in Fields)
            {
                _pool.Return (field);
            }

            Fields.Clear();
        }

        Fields = null!;
        Description = null;
        Modified = false;
    }

    #endregion
}
