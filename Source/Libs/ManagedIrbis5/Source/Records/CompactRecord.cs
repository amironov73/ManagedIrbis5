// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* CompactRecord.cs -- компактное представление библиографической записи
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

using AM;

using ManagedIrbis.Direct;
using ManagedIrbis.Infrastructure;

#endregion

#nullable enable

namespace ManagedIrbis.Records;

/*

    Содержимое записи помещается в непрерывный блок памяти,
    состоящий из заголовка (в котором фиксируется общее количество полей)
    и означенного количества полей, записанных "впритых", т. е. без заполнителей.

    В свою очередь поле состоит из заголовка (в котором фиксируется
    размер поля в байтах, метка поля и количество подполей)
    и означаенного количества подполей, также записанных "впритык".

    Подполе содержит заголовок (в котором фиксируется размер подполя
    в байтах и код подполя) и собственно данных в кодировке UTF8.

    Запись
    ------

    RecordSize: Int32 - общий размер записи в байтах
    Mfn: Int32 - MFN записи
    Version: Int32 - номер версии записи
    Status: Int32 - статус записи
    -- здесь заканчивается постоянная часть заголовка
    FieldCount: CompactInt32 - общее количество полей в данной записи
    Fields: CompactField... - массив полей, записанных без промежутков

    Поле
    ----

    FieldSize: CompactInt32 - размер данного поля в байтах (не считая FieldSize)
    Tag: CompactInt32 - метка поля
    SubfieldCount: CompactInt32 - количество подполей в данном поле
    Subfields: CompactSubfield... - массив подполей, записанных без промежутков

    Подполе
    -------

    SubFieldSize: CompactInt32 - размер данного подполя в байтах (не считая SubFieldSize)
    Code: byte -
    Value: UTF8 bytes...


 */

/// <summary>
/// Компактное представление библиографической записи.
/// Используется, например, для кеширования.
/// </summary>
[DebuggerDisplay ("[{" + nameof (Database) +
                  "}] MFN={" + nameof (Mfn) + "} ({" + nameof (Version) + "})")]
public sealed class CompactRecord
    : IRecord
{
    #region Constants

    /// <summary>
    /// Размер постоянного заголовка: 4 числа Int32.
    /// </summary>
    private const int ConstantHeaderSize = 16;

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public CompactRecord
        (
            byte[]? bytes = null
        )
    {
        // резервируем место под RecordSize, Mfn, Version, Status
        _bytes = bytes ?? new byte[ConstantHeaderSize];
    } // constructor

    #endregion

    #region Private members

    private byte[] _bytes;

    private int _ReadInt32 (int offset) => MemoryMarshal.Cast<byte, int> (_bytes.AsSpan())[offset];

    private void _WriteInt32 (int offset, int value) =>
        MemoryMarshal.Cast<byte, int> (_bytes.AsSpan())[offset] = value;

    #endregion

    #region IRecord members

    /// <inheritdoc cref="IRecord.Database"/>
    public string? Database
    {
        get => null;
        set { }
    } // property Database

    /// <inheritdoc cref="IRecord.Mfn"/>
    public int Mfn
    {
        get => _ReadInt32 (1);
        set => _WriteInt32 (1, value);
    }

    /// <inheritdoc cref="IRecord.Version"/>
    public int Version
    {
        get => _ReadInt32 (2);
        set => _WriteInt32 (2, value);
    }

    /// <inheritdoc cref="IRecord.Status"/>
    public RecordStatus Status
    {
        get => (RecordStatus)_ReadInt32 (3);
        set => _WriteInt32 (3, (int)value);
    }

    /// <inheritdoc cref="IRecord.Decode(ManagedIrbis.Infrastructure.Response)"/>
    public void Decode (Response response) => throw new NotImplementedException();

    /// <inheritdoc cref="IRecord.Decode(ManagedIrbis.Direct.MstRecord64)"/>
    public void Decode (MstRecord64 record) => throw new NotImplementedException();

    /// <inheritdoc cref="IRecord.Encode(string?)"/>
    public string Encode (string? delimiter = IrbisText.IrbisDelimiter) =>
        throw new NotImplementedException();

    /// <inheritdoc cref="IRecord.Encode(ManagedIrbis.Direct.MstRecord64)"/>
    public void Encode (MstRecord64 record) => throw new NotImplementedException();

    /// <inheritdoc cref="IRecord.FM"/>
    public string FM (int tag) => throw new NotImplementedException();

    #endregion

    #region Public members

    /// <summary>
    /// Получение компактной записи из обычной.
    /// </summary>
    public static CompactRecord FromRecord
        (
            Record record
        )
    {
        Sure.NotNull (record);

        var encoding = new UTF8Encoding();
        var stream = new MemoryStream (1024);
        var writer = new BinaryWriter (stream, encoding);
        var result = new CompactRecord();

        // пропускаем заголовок, его мы заполним позже
        writer.Seek (ConstantHeaderSize, SeekOrigin.Begin);
        writer.Write7BitEncodedInt (record.Fields.Count);

        foreach (var field in record.Fields)
        {
            var innerStream = new MemoryStream (1024);
            var innerWriter = new BinaryWriter (innerStream);
            innerWriter.Write7BitEncodedInt (field.Tag);
            innerWriter.Write7BitEncodedInt (field.Subfields.Count);

            foreach (var subfield in field.Subfields)
            {
                var subfieldValue = subfield.Value ?? string.Empty;
                var subfieldSize = 1 + encoding.GetByteCount (subfieldValue);
                innerWriter.Write7BitEncodedInt (subfieldSize);
                innerWriter.Write ((byte) subfield.Code);
                innerWriter.Write (subfieldValue);
            }

            innerWriter.Flush();
            var innerSize = unchecked ((int) innerStream.Length);
            writer.Write7BitEncodedInt (innerSize);
        }

        result._bytes = stream.ToArray();

        // заполняем заголовок
        result._WriteInt32 (0, result._bytes.Length);
        result.Mfn = record.Mfn;
        result.Version = record.Version;
        result.Status = record.Status;

        return result;
    }

    /// <summary>
    /// Получает представление записи в памяти в виде массива байт.
    /// </summary>
    public byte[] ToBytes() => _bytes;

    /// <summary>
    /// Получение обычной библиографической записи из компактной.
    /// </summary>
    public Record ToRecord()
    {
        var result = new Record()
        {
            Mfn = Mfn,
            Version = Version,
            Status = Status
        };

        var encoding = new UTF8Encoding();
        var stream = new MemoryStream (_bytes);
        stream.Position = ConstantHeaderSize; // пропускаем заголовок
        var reader = new BinaryReader (stream);
        var fieldCount = reader.Read7BitEncodedInt();
        for (var i = 0; i < fieldCount; i++)
        {
            /* var fieldSize = */
            reader.Read7BitEncodedInt();
            var field = new Field (reader.Read7BitEncodedInt());
            var subfieldCount = reader.Read7BitEncodedInt();
            for (var j = 0; j < subfieldCount; j++)
            {
                var subfieldSize = reader.Read7BitEncodedInt();
                var bytes = new byte [subfieldSize];
                var subfield = new SubField()
                {
                    Code = (char) reader.ReadByte(),
                };
                reader.Read (bytes, 0, subfieldSize);
                subfield.Value = encoding.GetString (bytes);

                field.Subfields.Add (subfield);
            }

            result.Fields.Add (field);
        }

        return result;
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString" />
    public override string ToString()
    {
        return Encode ("\n");
    }

    #endregion
}
