﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* MappedMstFile64.cs -- super-fast MST-file accessor using memory-mapped files
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Text;

using AM;
using AM.IO;

using ManagedIrbis.Infrastructure;

#endregion

#nullable enable

namespace ManagedIrbis.Direct;

/// <summary>
/// Super-fast MST-file accessor using memory-mapped files.
/// </summary>
public sealed class MappedMstFile64
    : IDisposable
{
    #region Properties

    /// <summary>
    /// Control record.
    /// </summary>
    public MstControlRecord64 ControlRecord { get; private set; }

    /// <summary>
    /// File name.
    /// </summary>
    public string FileName { get; private set; }

    #endregion

    #region Construction

    /// <summary>
    /// Constructor.
    /// </summary>
    public MappedMstFile64
        (
            string fileName
        )
    {
        FileName = fileName;

        _lockObject = new object();
        _mapping = DirectUtility.OpenMemoryMappedFile(fileName);
        _stream = _mapping.CreateViewStream(0, 0, MemoryMappedFileAccess.Read);
        ControlRecord = MstControlRecord64.Read(_stream);
    }

    #endregion

    #region Private members

    private readonly object _lockObject;

    private readonly MemoryMappedFile _mapping;

    private readonly MemoryMappedViewStream _stream;

    #endregion

    #region Public methods

    /// <summary>
    /// Read the record (with preload optimization).
    /// </summary>
    public MstRecord64 ReadRecord
        (
            long position
        )
    {
        Sure.NonNegative (position);

        lock (_lockObject)
        {
            Encoding encoding = IrbisEncoding.Utf8;

            _stream.Seek(position, SeekOrigin.Begin);
            MstRecordLeader64 leader = MstRecordLeader64.Read(_stream);
            List<MstDictionaryEntry64> dictionary
                = new List<MstDictionaryEntry64>(leader.Nvf);

            for (int i = 0; i < leader.Nvf; i++)
            {
                MstDictionaryEntry64 entry = new MstDictionaryEntry64
                {
                    Tag = _stream.ReadInt32Network(),
                    Position = _stream.ReadInt32Network(),
                    Length = _stream.ReadInt32Network()
                };
                long saveOffset = _stream.Position;
                long endOffset = leader.Base + entry.Position;
                _stream.Seek(position + endOffset, SeekOrigin.Begin);
                var bytes = StreamUtility.ReadBytes(_stream, entry.Length);
                if (!ReferenceEquals(bytes, null))
                {
                    entry.Bytes = bytes;
                    entry.Text = encoding.GetString(bytes, 0, bytes.Length);
                }
                _stream.Seek(saveOffset, SeekOrigin.Begin);
                dictionary.Add(entry);
            }

            MstRecord64 result = new MstRecord64
            {
                Leader = leader,
                Dictionary = dictionary
            };

            return result;
        }
    }

    #endregion

    #region IDisposable members

    /// <inheritdoc cref="IDisposable.Dispose" />
    public void Dispose()
    {
        _stream.Dispose();
        _mapping.Dispose();
    }

    #endregion
}
