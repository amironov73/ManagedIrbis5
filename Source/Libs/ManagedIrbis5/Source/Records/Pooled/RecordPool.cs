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

/* RecordPool.cs -- пул записей
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using AM;

using Microsoft.Extensions.ObjectPool;

#endregion

#nullable enable

namespace ManagedIrbis.Records;

/// <summary>
/// Пул записей, полей и подполей.
/// </summary>
public sealed class RecordPool
    : IDisposable
{
    #region Properties

    /// <summary>
    /// Общий пул.
    /// </summary>
    public static RecordPool Shared { get; } = new ();

    #endregion

    #region Constructor

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public RecordPool()
    {
        var provider = new DefaultObjectPoolProvider();
        _subFields = provider.Create<PooledSubField>();
        _fields = provider.Create<PooledField>();
        _records = provider.Create<PooledRecord>();
    }

    #endregion

    #region Private members

    private readonly ObjectPool<PooledSubField> _subFields;
    private readonly ObjectPool<PooledField> _fields;
    private readonly ObjectPool<PooledRecord> _records;

    #endregion

    #region Public methods

    /// <summary>
    /// Запрос подполя из пула.
    /// </summary>
    public PooledSubField GetSubField
        (
            char code,
            string? value = null
        )
    {
        var result = _subFields.Get();
        result.Init (code, value);

        return result;
    }

    /// <summary>
    /// Запрос лоля из пула.
    /// </summary>
    public PooledField GetField
        (
            int tag
        )
    {
        var result = _fields.Get();
        result.Init (tag);
        result._pool = this;

        return result;
    }

    /// <summary>
    /// Запрос записи из пула.
    /// </summary>
    public PooledRecord GetRecord()
    {
        var result = _records.Get();
        result.Init();
        result._pool = this;

        return result;
    }

    /// <summary>
    /// Возврат подполя в пул.
    /// </summary>
    public void Return
        (
            PooledSubField subField
        )
    {
        Sure.NotNull (subField);

        subField.Dispose();
        _subFields.Return (subField);
    }

    /// <summary>
    /// Возврат поля в пул.
    /// </summary>
    public void Return
        (
            PooledField field
        )
    {
        Sure.NotNull (field);

        field.Dispose();
        _fields.Return (field);
    }

    /// <summary>
    /// Возврат записи в пул.
    /// </summary>
    public void Return
        (
            PooledRecord record
        )
    {
        Sure.NotNull (record);

        record.Dispose();
        _records.Return (record);
    }

    #endregion

    #region IDisposable members

    /// <inheritdoc cref="IDisposable.Dispose"/>
    public void Dispose()
    {
        if (this != Shared)
        {
            // TODO implement
        }
    }

    #endregion
}
