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

/* MobiHeader.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;
using System.Linq;

#endregion

#nullable enable

namespace ManagedIrbis.Mobipocket.Headers;

/// <summary>
///
/// </summary>
public sealed class MobiHeader
{
    private readonly FileStream _stream;
    private readonly int _mobiHeaderSize;

    #region Byte Arrays

    private readonly byte[] _compression = new byte[2];
    private readonly byte[] _unused0 = new byte[2];
    private readonly byte[] _textLength = new byte[4];
    private readonly byte[] _recordCount = new byte[2];
    private readonly byte[] _recordSize = new byte[2];
    private readonly byte[] _encryptionType = new byte[2];
    private readonly byte[] _unused1 = new byte[2];
    private readonly byte[] _identifier = new byte[4];
    private readonly byte[] _headerLength = new byte[4];
    private readonly byte[] _mobiType = new byte[4];
    private readonly byte[] _textEncoding = new byte[4];
    private readonly byte[] _uniqueId = new byte[4];
    private readonly byte[] _fileVersion = new byte[4];
    private readonly byte[] _orthographicIndex = new byte[4];
    private readonly byte[] _inflectionIndex = new byte[4];
    private readonly byte[] _indexNames = new byte[4];
    private readonly byte[] _indexKeys = new byte[4];
    private readonly byte[] _extraIndex0 = new byte[4];
    private readonly byte[] _extraIndex1 = new byte[4];
    private readonly byte[] _extraIndex2 = new byte[4];
    private readonly byte[] _extraIndex3 = new byte[4];
    private readonly byte[] _extraIndex4 = new byte[4];
    private readonly byte[] _extraIndex5 = new byte[4];
    private readonly byte[] _firstNonBookIndex = new byte[4];
    private readonly byte[] _fullNameOffset = new byte[4];
    private readonly byte[] _fullNameLength = new byte[4];
    private readonly byte[] _locale = new byte[4];
    private readonly byte[] _inputLanguage = new byte[4];
    private readonly byte[] _outputLanguage = new byte[4];
    private readonly byte[] _minVersion = new byte[4];
    private readonly byte[] _firstImageIndex = new byte[4];
    private readonly byte[] _huffmanRecordOffset = new byte[4];
    private readonly byte[] _huffmanRecordCount = new byte[4];
    private readonly byte[] _huffmanTableOffset = new byte[4];
    private readonly byte[] _huffmanTableLength = new byte[4];
    private readonly byte[] _exthFlags = new byte[4];
    private byte[] _restOfMobiHeader = null!;
    private byte[] _remainder = null!;
    private byte[] _fullname = null!;

    #endregion

    /// <summary>
    ///
    /// </summary>
    public string Compression
    {
        get
        {
            var compression = ByteUtils.ToInt16 (_compression);
            return compression switch
            {
                1 => "No Compression",
                2 => "PalmDOC Compression",
                17480 => "HUFF/CDIC Compression",
                _ => string.Empty
            };
        }
    }

    /// <summary>
    ///
    /// </summary>
    public int TextLength => ByteUtils.ToInt32 (_textLength);

    /// <summary>
    ///
    /// </summary>
    public short RecordCount => ByteUtils.ToInt16 (_recordCount);

    /// <summary>
    ///
    /// </summary>
    public short RecordSize => ByteUtils.ToInt16 (_recordSize);

    /// <summary>
    ///
    /// </summary>
    public string EncryptionType => GetEncryptionType();

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
    public string MobiType => GetMobiType();

    /// <summary>
    ///
    /// </summary>
    public string TextEncoding => GetTextEncoding();

    /// <summary>
    ///
    /// </summary>
    public long UniqueId => ByteUtils.ToUInt32 (_uniqueId);

    /// <summary>
    ///
    /// </summary>
    public long FileVersion => ByteUtils.ToUInt32 (_fileVersion);

    /// <summary>
    ///
    /// </summary>
    public int Locale => ByteUtils.ToInt32 (_headerLength);

    /// <summary>
    ///
    /// </summary>
    public int InputLanguage => ByteUtils.ToInt32 (_headerLength);

    /// <summary>
    ///
    /// </summary>
    public int OutputLanguage => ByteUtils.ToInt32 (_headerLength);

    /// <summary>
    ///
    /// </summary>
    public int MinVersion => ByteUtils.ToInt32 (_minVersion);

    /// <summary>
    ///
    /// </summary>
    public int FirstImageIndex => ByteUtils.ToInt32 (_firstImageIndex);

    /// <summary>
    ///
    /// </summary>
    public string FullName => ByteUtils.ToString (_fullname);

    /// <summary>
    ///
    /// </summary>
    public ExthHeader ExthHeader { get; set; }

    /// <summary>
    ///
    /// </summary>
    public MobiHeader 
        (
            FileStream stream, 
            int mobiHeaderSize
        )
    {
        ExthHeader = null!;

        _stream = stream;
        _mobiHeaderSize = mobiHeaderSize;

        LoadMobiHeader();
    }

    private void LoadMobiHeader()
    {
        // ReSharper disable MustUseReturnValue
        
        _stream.Read (_compression, 0, _compression.Length);
        _stream.Read (_unused0, 0, _unused0.Length);
        _stream.Read (_textLength, 0, _textLength.Length);
        _stream.Read (_recordCount, 0, _recordCount.Length);
        _stream.Read (_recordSize, 0, _recordSize.Length);
        _stream.Read (_encryptionType, 0, _encryptionType.Length);
        _stream.Read (_unused1, 0, _unused1.Length);
        _stream.Read (_identifier, 0, _identifier.Length);
        _stream.Read (_headerLength, 0, _headerLength.Length);
        _stream.Read (_mobiType, 0, _mobiType.Length);
        _stream.Read (_textEncoding, 0, _textEncoding.Length);
        _stream.Read (_uniqueId, 0, _uniqueId.Length);
        _stream.Read (_fileVersion, 0, _fileVersion.Length);
        _stream.Read (_orthographicIndex, 0, _orthographicIndex.Length);
        _stream.Read (_inflectionIndex, 0, _inflectionIndex.Length);
        _stream.Read (_indexNames, 0, _indexNames.Length);
        _stream.Read (_indexKeys, 0, _indexKeys.Length);
        _stream.Read (_extraIndex0, 0, _extraIndex0.Length);
        _stream.Read (_extraIndex1, 0, _extraIndex1.Length);
        _stream.Read (_extraIndex2, 0, _extraIndex2.Length);
        _stream.Read (_extraIndex3, 0, _extraIndex3.Length);
        _stream.Read (_extraIndex4, 0, _extraIndex4.Length);
        _stream.Read (_extraIndex5, 0, _extraIndex5.Length);
        _stream.Read (_firstNonBookIndex, 0, _firstNonBookIndex.Length);
        _stream.Read (_fullNameOffset, 0, _fullNameOffset.Length);
        _stream.Read (_fullNameLength, 0, _fullNameLength.Length);
        _stream.Read (_locale, 0, _locale.Length);
        _stream.Read (_inputLanguage, 0, _inputLanguage.Length);
        _stream.Read (_outputLanguage, 0, _outputLanguage.Length);
        _stream.Read (_minVersion, 0, _minVersion.Length);
        _stream.Read (_firstImageIndex, 0, _firstImageIndex.Length);
        _stream.Read (_huffmanRecordOffset, 0, _huffmanRecordOffset.Length);
        _stream.Read (_huffmanRecordCount, 0, _huffmanRecordCount.Length);
        _stream.Read (_huffmanTableOffset, 0, _huffmanTableOffset.Length);
        _stream.Read (_huffmanTableLength, 0, _huffmanTableLength.Length);
        _stream.Read (_exthFlags, 0, _exthFlags.Length);

        _restOfMobiHeader = new byte[HeaderLength + 16 - 132];
        _stream.Read (_restOfMobiHeader, 0, _restOfMobiHeader.Length);

        // ReSharper restore MustUseReturnValue

        var exthHeaderExists = GetExthHeaderExists();
        if (exthHeaderExists)
        {
            ExthHeader = new ExthHeader (_stream);
        }

        var exthHeaderSize = exthHeaderExists ? ExthHeader.Size : 0;
        var currentOffset = 132 + _restOfMobiHeader.Length + exthHeaderSize;

        // ReSharper disable MustUseReturnValue

        _remainder = new byte[_mobiHeaderSize - currentOffset];
        _stream.Read (_remainder, 0, _remainder.Length);

        // ReSharper restore MustUseReturnValue

        var fullNameIndexInRemainder = ByteUtils.GetInt32 (_fullNameOffset) - currentOffset;
        var fullNameLength = ByteUtils.GetInt32 (_fullNameLength);
        _fullname = new byte[fullNameLength];

        if (fullNameIndexInRemainder >= 0
            && (fullNameIndexInRemainder < _remainder.Length)
            && ((fullNameIndexInRemainder + fullNameLength) <= _remainder.Length)
            && (fullNameLength > 0))
        {
            Array.Copy (_remainder, fullNameIndexInRemainder, _fullname, 0, fullNameLength);
        }
    }

    private bool GetExthHeaderExists()
    {
        var bytes = (byte[])_exthFlags.Clone();

        if (BitConverter.IsLittleEndian)
        {
            Array.Reverse (bytes);
        }

        var exthFlagsValue = BitConverter.ToInt32 (bytes, 0);

        return (exthFlagsValue & 0x40) != 0;
    }

    private string GetTextEncoding()
    {
        var textEncodingValue = ByteUtils.ToInt32 (_textEncoding);
        switch (textEncodingValue)
        {
            case 1252:
                return "Cp1252";
            case 65001:
                return "UTF-8";
        }

        return "";
    }

    private string GetMobiType()
    {
        var mobiTypeValue = ByteUtils.ToInt32 (_mobiType);
        return mobiTypeValue switch
        {
            2 => "MobiPocket Book",
            3 => "PalmDOC Book",
            4 => "Audio",
            232 => "MobiPocket Generated by Kindlegen 1.2",
            248 => "KF8 Generated by Kindlegen 1.2",
            257 => "News",
            258 => "News Feed",
            259 => "News Magazine",
            513 => "PICS",
            514 => "Word",
            515 => "XLS",
            516 => "PPT",
            517 => "TEXT",
            518 => "HTML",
            _ => string.Empty
        };
    }

    private string GetEncryptionType()
    {
        var encType = ByteUtils.ToInt16 (_encryptionType);
        return encType switch
        {
            0 => "No Encryption",
            1 => "Old Mobipocket Encryption",
            2 => "Mobipocket Encryption",
            _ => string.Empty
        };
    }

    /// <summary>
    ///
    /// </summary>
    public string GetExthRecordValue (int type)
    {
        if (ExthHeader == null! || ExthHeader.Records == null!)
        {
            return "";
        }

        var record = ExthHeader.Records.FirstOrDefault (r => r.Type == type);

        if (record == null)
        {
            return "";
        }

        return record.Value;
    }

    /// <summary>
    ///
    /// </summary>
    public void Write (FileStream outStream)
    {
        outStream.Write (_compression, 0, _compression.Length);
        outStream.Write (_unused0, 0, _unused0.Length);
        outStream.Write (_textLength, 0, _textLength.Length);
        outStream.Write (_recordCount, 0, _recordCount.Length);
        outStream.Write (_recordSize, 0, _recordSize.Length);
        outStream.Write (_encryptionType, 0, _encryptionType.Length);
        outStream.Write (_unused1, 0, _unused1.Length);
        outStream.Write (_identifier, 0, _identifier.Length);
        outStream.Write (_headerLength, 0, _headerLength.Length);
        outStream.Write (_mobiType, 0, _mobiType.Length);
        outStream.Write (_textEncoding, 0, _textEncoding.Length);
        outStream.Write (_uniqueId, 0, _uniqueId.Length);
        outStream.Write (_fileVersion, 0, _fileVersion.Length);
        outStream.Write (_orthographicIndex, 0, _orthographicIndex.Length);
        outStream.Write (_inflectionIndex, 0, _inflectionIndex.Length);
        outStream.Write (_indexNames, 0, _indexNames.Length);
        outStream.Write (_indexKeys, 0, _indexKeys.Length);
        outStream.Write (_extraIndex0, 0, _extraIndex0.Length);
        outStream.Write (_extraIndex1, 0, _extraIndex1.Length);
        outStream.Write (_extraIndex2, 0, _extraIndex2.Length);
        outStream.Write (_extraIndex3, 0, _extraIndex3.Length);
        outStream.Write (_extraIndex4, 0, _extraIndex4.Length);
        outStream.Write (_extraIndex5, 0, _extraIndex5.Length);
        outStream.Write (_firstNonBookIndex, 0, _firstNonBookIndex.Length);
        outStream.Write (_fullNameOffset, 0, _fullNameOffset.Length);
        outStream.Write (_fullNameLength, 0, _fullNameLength.Length);
        outStream.Write (_locale, 0, _locale.Length);
        outStream.Write (_inputLanguage, 0, _inputLanguage.Length);
        outStream.Write (_outputLanguage, 0, _outputLanguage.Length);
        outStream.Write (_minVersion, 0, _minVersion.Length);
        outStream.Write (_firstImageIndex, 0, _firstImageIndex.Length);
        outStream.Write (_huffmanRecordOffset, 0, _huffmanRecordOffset.Length);
        outStream.Write (_huffmanRecordCount, 0, _huffmanRecordCount.Length);
        outStream.Write (_huffmanTableOffset, 0, _huffmanTableOffset.Length);
        outStream.Write (_huffmanTableLength, 0, _huffmanTableLength.Length);
        outStream.Write (_exthFlags, 0, _exthFlags.Length);
        outStream.Write (_restOfMobiHeader, 0, _restOfMobiHeader.Length);

        if (ExthHeader != null!)
        {
            ExthHeader.Write (outStream);
        }

        outStream.Write (_remainder, 0, _remainder.Length);
    }
}
