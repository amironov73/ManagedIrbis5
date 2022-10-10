// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* IfpControlRecord64.cs -- управляющая запись IFP-файла в ИРБИС64
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;

using AM;
using AM.IO;

#endregion

#nullable enable

namespace ManagedIrbis.Direct;

/// <summary>
/// Управляющая запись IFP-файла в ИРБИС64.
/// </summary>
[StructLayout (LayoutKind.Sequential)]
public sealed class IfpControlRecord64
{
    #region Constants

    /// <summary>
    /// Размер управляющей записи (байты).
    /// </summary>
    public const int RecordSize = 20;

    #endregion

    #region Properties

    /// <summary>
    /// Ссылка на свободное место в IFP-файле.
    /// </summary>
    [Description ("Ссылка на свободное место")]
    public long NextOffset { get; set; }

    /// <summary>
    /// Количество блоков в N01-файле.
    /// </summary>
    [Description ("Количество блоков в N01-файле")]
    public int NodeBlockCount { get; set; }

    /// <summary>
    /// Количество блоков в L01 файле.
    /// </summary>
    [Description ("Количество блоков в L01-файле")]
    public int LeafBlockCount { get; set; }

    /// <summary>
    /// Резерв.
    /// </summary>
    [Description ("Резерв")]
    public int Reserved { get; set; }

    #endregion

    #region Public methods

    /// <summary>
    /// Чтение управляющей записи из указанного потока.
    /// </summary>
    public static IfpControlRecord64 Read
        (
            Stream stream
        )
    {
        Sure.NotNull (stream);

        var result = new IfpControlRecord64()
        {
            NextOffset = stream.ReadInt64Network(),
            NodeBlockCount = stream.ReadInt32Network(),
            LeafBlockCount = stream.ReadInt32Network(),
            Reserved = stream.ReadInt32Network()
        };

        return result;
    }

    /// <summary>
    /// Сохранение управляющей записи в указанный поток.
    /// </summary>
    public void Write
        (
            Stream stream
        )
    {
        Sure.NotNull (stream);

        stream.WriteInt64Network (NextOffset);
        stream.WriteInt32Network (NodeBlockCount);
        stream.WriteInt32Network (LeafBlockCount);
        stream.WriteInt32Network (Reserved);
    }

    #endregion
}
