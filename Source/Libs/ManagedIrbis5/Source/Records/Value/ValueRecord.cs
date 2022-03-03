// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMethodReturnValue.Global
// ReSharper disable UnusedParameter.Local

/* ValueRecord.cs -- библиографическая запись, оформленная как структура
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;
using AM.Collections;
using AM.Text;

using ManagedIrbis.Direct;
using ManagedIrbis.ImportExport;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Records;

using static ManagedIrbis.RecordStatus;

#endregion

namespace ManagedIrbis;

/// <summary>
/// Библиографическая запись, оформленная как структура.
/// </summary>
public readonly struct ValueRecord
{
    #region Constants

    /// <summary>
    /// Запись удалена любым способом (логически или физически).
    /// </summary>
    internal const RecordStatus IsDeleted = LogicallyDeleted | PhysicallyDeleted;

    #endregion

    #region Properties

    /// <summary>
    /// База данных, в которой хранится запись.
    /// Для вновь созданных записей -- <c>null</c>.
    /// </summary>
    public readonly string? Database;

    /// <summary>
    /// MFN (порядковый номер в базе данных) записи.
    /// Для вновь созданных записей равен <c>0</c>.
    /// Для хранящихся в базе записей нумерация начинается
    /// с <c>1</c>.
    /// </summary>
    public readonly int Mfn;

    /// <summary>
    /// Версия записи. Для вновь созданных записей равна <c>0</c>.
    /// Для хранящихся в базе записей нумерация версий начинается
    /// с <c>1</c>.
    /// </summary>
    public readonly int Version;

    /// <summary>
    /// Статус записи. Для вновь созданных записей <c>None</c>.
    /// </summary>
    public readonly RecordStatus Status;

    /// <summary>
    /// Признак -- запись помечена как логически удаленная.
    /// </summary>
    public bool Deleted => (Status & IsDeleted) != 0;

    /// <summary>
    /// Список полей.
    /// </summary>
    public readonly Memory<ValueField> Fields;

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="database">Имя базы данных.</param>
    /// <param name="mfn">MFN записи.</param>
    /// <param name="version">Версия записи.</param>
    /// <param name="status">Статус записи.</param>
    /// <param name="fields">Поля</param>
    public ValueRecord
        (
            string? database,
            int mfn,
            int version,
            RecordStatus status,
            Memory<ValueField> fields
        )
    {
        Database = database;
        Mfn = mfn;
        Version = version;
        Status = status;
        Fields = fields;
    }

    #endregion
}
