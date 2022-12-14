// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local

/* PdbHeader.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;

#endregion

#nullable enable

namespace ManagedIrbis.Mobipocket.Headers;

/// <summary>
///
/// </summary>
public sealed class PdbHeader
{
    private readonly FileStream _stream;

    #region Byte Arrays

    private readonly byte[] _name = new byte[32];
    private readonly byte[] _attributes = new byte[2];
    private readonly byte[] _version = new byte[2];
    private readonly byte[] _createDate = new byte[4];
    private readonly byte[] _lastBackupDate = new byte[4];
    private readonly byte[] _modificationDate = new byte[4];
    private readonly byte[] _modificationNumber = new byte[4];
    private readonly byte[] _appInfoId = new byte[4];
    private readonly byte[] _sortInfoId = new byte[4];
    private readonly byte[] _type = new byte[4];
    private readonly byte[] _creator = new byte[4];
    private readonly byte[] _uniqueIdSeed = new byte[4];
    private readonly byte[] _nextRecordListId = new byte[4];
    private readonly byte[] _numberOfRecords = new byte[2];
    private readonly byte[] _gapToData = new byte[2];

    #endregion

    /// <summary>
    ///
    /// </summary>
    public string Name => ByteUtils.ToString (_name);

    /// <summary>
    ///
    /// </summary>
    public int Attributes => ByteUtils.GetInt32 (_attributes);

    /// <summary>
    ///
    /// </summary>
    public int Version => ByteUtils.GetInt32 (_version);

    /// <summary>
    ///
    /// </summary>
    public DateTime? CreateDate => GetHeaderDate (_createDate);

    /// <summary>
    ///
    /// </summary>
    public DateTime? ModificationDate => GetHeaderDate (_modificationDate);

    /// <summary>
    ///
    /// </summary>
    public DateTime? LastBackupDate => GetHeaderDate (_lastBackupDate);

    /// <summary>
    ///
    /// </summary>
    public int ModificationNumber => ByteUtils.GetInt32 (_modificationNumber);

    /// <summary>
    ///
    /// </summary>
    public int AppInfoId => ByteUtils.GetInt32 (_appInfoId);

    /// <summary>
    ///
    /// </summary>
    public int SortInfoId => ByteUtils.GetInt32 (_sortInfoId);

    /// <summary>
    ///
    /// </summary>
    public int Type => ByteUtils.GetInt32 (_type);

    /// <summary>
    ///
    /// </summary>
    public int Creator => ByteUtils.GetInt32 (_creator);

    /// <summary>
    ///
    /// </summary>
    public int UniqueIdSeed => ByteUtils.GetInt32 (_uniqueIdSeed);

    /// <summary>
    ///
    /// </summary>
    public int NextRecordListId => ByteUtils.GetInt32 (_nextRecordListId);

    /// <summary>
    ///
    /// </summary>
    public int NumberOfRecords => ByteUtils.GetInt32 (_numberOfRecords);

    /// <summary>
    ///
    /// </summary>
    public List<PdbRecord> Records { get; set; }

    /// <summary>
    ///
    /// </summary>
    public int GapToData => ByteUtils.GetInt32 (_gapToData);

    /// <summary>
    ///
    /// </summary>
    public int MobiHeaderSize => Records.Count > 1 ? Records[1].Offset - Records[0].Offset : 0;

    /// <summary>
    ///
    /// </summary>
    public int OffsetAfterMobiHeader => Records.Count > 1 ? Records[1].Offset : 0;

    /// <summary>
    ///
    /// </summary>
    public PdbHeader (FileStream stream)
    {
        _stream = stream;

        LoadPdbHeader();
    }

    private void LoadPdbHeader()
    {
        // ReSharper disable MustUseReturnValue
        _stream.Read (_name, 0, _name.Length);
        _stream.Read (_attributes, 0, _attributes.Length);
        _stream.Read (_version, 0, _version.Length);
        _stream.Read (_createDate, 0, _createDate.Length);
        _stream.Read (_lastBackupDate, 0, _lastBackupDate.Length);
        _stream.Read (_modificationDate, 0, _modificationDate.Length);
        _stream.Read (_modificationNumber, 0, _modificationNumber.Length);
        _stream.Read (_appInfoId, 0, _appInfoId.Length);
        _stream.Read (_sortInfoId, 0, _sortInfoId.Length);
        _stream.Read (_type, 0, _type.Length);
        _stream.Read (_creator, 0, _creator.Length);
        _stream.Read (_uniqueIdSeed, 0, _uniqueIdSeed.Length);
        _stream.Read (_nextRecordListId, 0, _nextRecordListId.Length);
        _stream.Read (_numberOfRecords, 0, _numberOfRecords.Length);

        Records = new List<PdbRecord>();
        for (var i = 0; i < NumberOfRecords; i++)
        {
            Records.Add (new PdbRecord (_stream));
        }

        _stream.Read (_gapToData, 0, _gapToData.Length);
        // ReSharper restore MustUseReturnValue
    }

    /// <summary>
    ///
    /// </summary>
    private DateTime? GetHeaderDate (byte[] secondBytes)
    {
        var seconds = ByteUtils.GetInt32 (secondBytes);

        if (seconds == 0)
        {
            return null;
        }

        var date = new DateTime (1970, 1, 1, 0, 0, 0);
        return date.AddSeconds (seconds);
    }

    /// <summary>
    ///
    /// </summary>
    public void Write (FileStream outStream)
    {
        outStream.Write (_name, 0, _name.Length);
        outStream.Write (_attributes, 0, _attributes.Length);
        outStream.Write (_version, 0, _version.Length);
        outStream.Write (_createDate, 0, _createDate.Length);
        outStream.Write (_lastBackupDate, 0, _lastBackupDate.Length);
        outStream.Write (_modificationDate, 0, _modificationDate.Length);
        outStream.Write (_modificationNumber, 0, _modificationNumber.Length);
        outStream.Write (_appInfoId, 0, _appInfoId.Length);
        outStream.Write (_sortInfoId, 0, _sortInfoId.Length);
        outStream.Write (_type, 0, _type.Length);
        outStream.Write (_creator, 0, _creator.Length);
        outStream.Write (_uniqueIdSeed, 0, _uniqueIdSeed.Length);
        outStream.Write (_nextRecordListId, 0, _nextRecordListId.Length);
        outStream.Write (_numberOfRecords, 0, _numberOfRecords.Length);

        foreach (var pdbRecord in Records)
        {
            pdbRecord.Write (outStream);
        }

        outStream.Write (_gapToData, 0, _gapToData.Length);
    }
}

/// <summary>
///
/// </summary>
public class PdbRecord
{
    private readonly FileStream _stream;

    /// <summary>
    ///
    /// </summary>
    public PdbRecord (FileStream stream)
    {
        _stream = stream;

        LoadRecordInfo();
    }

    /// <summary>
    ///
    /// </summary>
    private void LoadRecordInfo()
    {
        _stream.Read (_offset, 0, _offset.Length);
        _stream.Read (_attributes, 0, _attributes.Length);
        _stream.Read (_uniqueId, 0, _uniqueId.Length);
    }

    /// <summary>
    ///
    /// </summary>
    public void Write (FileStream outStream)
    {
        outStream.Write (_offset, 0, _offset.Length);
        outStream.Write (_attributes, 0, _attributes.Length);
        outStream.Write (_uniqueId, 0, _uniqueId.Length);
    }

    private readonly byte[] _offset = new byte[4];
    private readonly byte[] _uniqueId = new byte[3];
    private readonly byte[] _attributes = new byte[1];

    /// <summary>
    ///
    /// </summary>
    public int Offset => ByteUtils.GetInt32 (_offset);

    /// <summary>
    ///
    /// </summary>
    public byte Attributes => _attributes[0];

    /// <summary>
    ///
    /// </summary>
    public int UniqueId => ByteUtils.GetInt32 (_uniqueId);
}
