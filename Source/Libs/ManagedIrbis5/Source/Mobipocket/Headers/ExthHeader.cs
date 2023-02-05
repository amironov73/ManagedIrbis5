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

/* ExthHeader.cs --
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
public sealed class ExthHeader
{
    private readonly FileStream _stream;

    private readonly byte[] _identifier = new byte[4];
    private readonly byte[] _headerLength = new byte[4];
    private readonly byte[] _recordCount = new byte[4];

    /// <summary>
    ///
    /// </summary>
    public string Identifier => ByteUtils.ToString (_identifier);

    /// <summary>
    ///
    /// </summary>
    public int HeaderLength => ByteUtils.GetInt32 (_headerLength);

    /// <summary>
    ///
    /// </summary>
    public int RecordCount => ByteUtils.GetInt32 (_recordCount);

    /// <summary>
    ///
    /// </summary>
    public List<ExthRecord> Records { get; set; }

    /// <summary>
    ///
    /// </summary>
    public int Size => GetSize();

    /// <summary>
    ///
    /// </summary>
    public ExthHeader 
        (
            FileStream stream
        )
    {
        Records = null!;
        
        _stream = stream;

        LoadExthHeader();
    }

    /// <summary>
    ///
    /// </summary>
    private void LoadExthHeader()
    {
        // ReSharper disable MustUseReturnValue

        _stream.Read (_identifier, 0, _identifier.Length);

        if (Identifier != "EXTH")
        {
            throw new ApplicationException ("EXTHHeader is invalid");
        }

        _stream.Read (_headerLength, 0, _headerLength.Length);
        _stream.Read (_recordCount, 0, _recordCount.Length);
        
        Records = new List<ExthRecord>();

        for (var i = 0; i < RecordCount; i++)
        {
            var record = new ExthRecord (_stream);
            Records.Add (record);
        }

        var padding = new byte[GetPaddingSize (GetDataSize())];
        _stream.Read (padding, 0, padding.Length);

        // ReSharper restore MustUseReturnValue
    }

    private int GetSize()
    {
        var dataSize = GetDataSize();
        var paddingSize = GetPaddingSize (dataSize);

        return 12 + dataSize + paddingSize;
    }

    private int GetDataSize()
    {
        var dataSize = 0;
        foreach (var record in Records)
        {
            dataSize += record.Size;
        }

        return dataSize;
    }

    private int GetPaddingSize (int dataSize)
    {
        var paddingSize = dataSize % 4;
        if (paddingSize != 0)
        {
            paddingSize = 4 - paddingSize;
        }

        return paddingSize;
    }

    /// <summary>
    ///
    /// </summary>
    public void Write (FileStream outStream)
    {
        outStream.Write (_identifier, 0, _identifier.Length);
        outStream.Write (_headerLength, 0, _headerLength.Length);
        outStream.Write (_recordCount, 0, _recordCount.Length);

        foreach (var record in Records)
        {
            record.Write (outStream);
        }

        var padding = new byte[GetPaddingSize (GetDataSize())];
        outStream.Write (padding, 0, padding.Length);
    }
}

/// <summary>
///
/// </summary>
public sealed class ExthRecord
{
    private readonly FileStream _stream;
    private readonly byte[] _type = new byte[4];
    private readonly byte[] _length = new byte[4];

    /// <summary>
    ///
    /// </summary>
    public int Type => ByteUtils.GetInt32 (_type);

    /// <summary>
    ///
    /// </summary>
    public int Length => ByteUtils.GetInt32 (_length);

    /// <summary>
    ///
    /// </summary>
    public int Size => GetSize();

    /// <summary>
    ///
    /// </summary>
    public byte[] Data { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string Value => ByteUtils.ToString (Data);

    /// <summary>
    ///
    /// </summary>
    /// <param name="stream"></param>
    public ExthRecord 
        (
            FileStream stream
        )
    {
        Data = null!;

        _stream = stream;

        LoadExthRecords();
    }

    private void LoadExthRecords()
    {
        // ReSharper disable MustUseReturnValue

        _stream.Read (_type, 0, _type.Length);
        _stream.Read (_length, 0, _length.Length);
        
        Data = new byte[Length - 8];

        _stream.Read (Data, 0, Data.Length);

        // ReSharper restore MustUseReturnValue
    }

    private int GetSize()
    {
        return Data.Length + 8;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="outStream"></param>
    public void Write (FileStream outStream)
    {
        outStream.Write (_type, 0, _type.Length);
        outStream.Write (_length, 0, _length.Length);
        outStream.Write (Data, 0, Data.Length);
    }
}
