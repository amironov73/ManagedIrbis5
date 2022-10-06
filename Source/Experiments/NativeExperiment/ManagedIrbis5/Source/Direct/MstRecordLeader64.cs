// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* MstRecordLeader64.cs -- заголовок библиографической записи в MST-файле
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;
using System.IO;

using AM;
using AM.IO;

#endregion

#nullable enable

namespace ManagedIrbis.Direct;

/// <summary>
/// Заголовок библиографической записи в MST-файле.
/// </summary>
[DebuggerDisplay("MFN={Mfn}, Length={Length}, NVF={Nvf}, Status={Status}")]
public struct MstRecordLeader64
{
    #region Constants

    /// <summary>
    /// Фиксированный размер лидера записи (всегда 8 чисел по 32 бита каждое).
    /// </summary>
    public const int LeaderSize = 32;

    #endregion

    #region Properties

    /// <summary>
    /// Номер записи в  файле документов.
    /// </summary>
    public int Mfn { get; set; }

    /// <summary>
    /// Общая длина записи (всегда четное число).
    /// </summary>
    public int Length { get; set; }

    /// <summary>
    /// Ссылка на предыдущую версию записи.
    /// </summary>
    public long Previous { get; set; }

    /// <summary>
    /// Смещение (базовый адрес) полей
    /// переменной длины (это общая часть
    /// лидера и справочника записи в байтах).
    /// </summary>
    public int Base { get; set; }

    /// <summary>
    /// Число полей в записи (т.е. число входов
    /// в справочнике).
    /// </summary>
    public int Nvf { get; set; }

    /// <summary>
    /// Индикатор записи (логически удаленная и т.п.).
    /// </summary>
    public int Status { get; set; }

    /// <summary>
    /// Номер версии записи.
    /// </summary>
    public int Version { get; set; }

    #endregion

    #region Public methods

    /// <summary>
    /// Read the record leader.
    /// </summary>
    public static MstRecordLeader64 Read
        (
            Stream stream
        )
    {
        var result = new MstRecordLeader64
        {
            Mfn = stream.ReadInt32Network(),
            Length = stream.ReadInt32Network(),
            Previous = stream.ReadInt64Network(),
            Base = stream.ReadInt32Network(),
            Nvf = stream.ReadInt32Network(),
            Version = stream.ReadInt32Network(),
            Status = stream.ReadInt32Network()
        };

        //Debug.Assert(result.Base ==
        //    (LeaderSize + result.Nvf * MstDictionaryEntry64.EntrySize));

        return result;
    }

    /// <summary>
    /// Разбор заголовка MST-записи в оперативной памяти.
    /// </summary>
    public static MstRecordLeader64 Parse (ReadOnlySpan<byte> bytes) =>
        new ()
        {
            Mfn = bytes.ReadNetworkInt32 (0),
            Length = bytes.ReadNetworkInt32 (4),
            Previous = bytes.ReadNetworkInt32 (8),
            Base = bytes.ReadNetworkInt32 (16),
            Nvf = bytes.ReadNetworkInt32 (20),
            Version = bytes.ReadNetworkInt32 (24),
            Status = bytes.ReadNetworkInt32 (28)
        };

    /// <summary>
    /// Вывод лидера в поток.
    /// </summary>
    public void Write
        (
            Stream stream
        )
    {
        stream.WriteInt32Network(Mfn);
        stream.WriteInt32Network(Length);
        stream.WriteInt64Network(Previous);
        stream.WriteInt32Network(Base);
        stream.WriteInt32Network(Nvf);
        stream.WriteInt32Network(Version);
        stream.WriteInt32Network(Status);
    }

    /// <summary>
    /// Вывод лидера в структуру в оперативной памяти.
    /// </summary>
    public void Pack
        (
            Span<byte> span
        )
    {
        span.WriteNetworkInt32 (0, Mfn);
        span.WriteNetworkInt32 (4, Length);
        span.WriteNetworkInt64 (8, Previous);
        span.WriteNetworkInt32 (16, Base);
        span.WriteNetworkInt32 (20, Nvf);
        span.WriteNetworkInt32 (24, Version);
        span.WriteNetworkInt32 (28, Status);
    }

    /// <summary>
    /// Выбирает данные из указанной области памяти, хранящей запись.
    /// </summary>
    public ReadOnlySpan<byte> GetData (ReadOnlySpan<byte> record) =>
        record.Slice (Base, Length - Base);

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString" />
    public override string ToString ()
    {
        return $"Mfn: {Mfn}, Length: {Length}, Previous: {Previous}, Base: {Base}, Nvf: {Nvf}, Status: {Status}, Version: {Version}";
    }

    #endregion
}
