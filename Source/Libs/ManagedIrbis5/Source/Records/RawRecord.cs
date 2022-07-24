// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMethodReturnValue.Global

/* RawRecord.cs -- сырая (не разобранная) запись
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using AM;
using AM.Text;

using ManagedIrbis.Direct;
using ManagedIrbis.Infrastructure;

using Microsoft.Extensions.Logging;

using static ManagedIrbis.RecordStatus;

#endregion

#nullable enable

namespace ManagedIrbis.Records;

/// <summary>
/// Сырая (не разобранная) запись.
/// </summary>
/// <remarks>
/// Основной сценарий, ради которых создан класс -- массированная
/// выгрузка/загрузка записей из/в ИРБИС.
/// </remarks>
[DebuggerDisplay ("[{" + nameof (Database) + "}] MFN={"
                  + nameof (Mfn) + "} ({" + nameof (Version) + "})")]
public sealed class RawRecord
    : IRecord
{
    #region Properties

    /// <summary>
    /// База данных, в которой хранится запись.
    /// Для вновь созданных записей -- <c>null</c>.
    /// </summary>
    public string? Database { get; set; }

    /// <summary>
    /// MFN (порядковый номер в базе данных) записи.
    /// Для вновь созданных записей равен <c>0</c>.
    /// Для хранящихся в базе записей нумерация начинается
    /// с <c>1</c>.
    /// </summary>
    public int Mfn { get; set; }

    /// <summary>
    /// Статус записи. Для вновь созданных записей <c>None</c>.
    /// </summary>
    public RecordStatus Status { get; set; }

    /// <summary>
    /// Признак -- запись помечена как логически удаленная.
    /// </summary>
    public bool Deleted
    {
        get => (Status & LogicallyDeleted) != 0;
        set
        {
            if (value)
            {
                Status |= LogicallyDeleted;
            }
            else
            {
                Status &= ~LogicallyDeleted;
            }
        }
    }

    /// <summary>
    /// Версия записи. Для вновь созданных записей равна <c>0</c>.
    /// Для хранящихся в базе записей нумерация версий начинается
    /// с <c>1</c>.
    /// </summary>
    public int Version { get; set; }

    /// <summary>
    /// Текстовые строки с полями.
    /// </summary>
    public List<string> Fields { get; } = new ();

    #endregion

    #region Public methods

    /// <inheritdoc cref="IRecord.Decode(Response)"/>
    public void Decode
        (
            Response response
        )
    {
        Sure.NotNull (response);

        try
        {
            var line = response.ReadUtf();

            var first = line.Split ('#');
            Mfn = int.Parse (first[0]);
            Status = first.Length == 1
                ? None
                : (RecordStatus)first[1].SafeToInt32();

            line = response.ReadUtf();
            var second = line.Split ('#');
            Version = second.Length == 1
                ? 0
                : int.Parse (second[1]);

            while (!response.EOT)
            {
                line = response.ReadUtf();
                if (string.IsNullOrEmpty (line))
                {
                    break;
                }

                Fields.Add (line);
            }
        }
        catch (Exception exception)
        {
            // response.DebugUtf(Console.Error);
            Magna.Logger.LogError (nameof (RawRecord) + "::" + nameof (Decode));

            throw new IrbisException
                (
                    nameof (Record) + "::" + nameof (Decode),
                    exception
                );
        }
    }

    /// <inheritdoc cref="IRecord.Decode(MstRecord64)"/>
    public void Decode
        (
            MstRecord64 record
        )
    {
        Sure.NotNull (record);

        throw new NotImplementedException();
    }

    /// <inheritdoc cref="IRecord.Encode(string)"/>
    public string Encode
        (
            string? delimiter = IrbisText.IrbisDelimiter
        )
    {
        var result = new StringBuilder (512);

        result.Append (Mfn.ToInvariantString());
        result.Append ('#');
        result.Append (((int)Status).ToInvariantString());
        result.Append (delimiter);
        result.Append ("0#");
        result.Append (Version.ToInvariantString());
        result.Append (delimiter);

        foreach (var line in Fields)
        {
            result.Append (line).Append (delimiter);
        }

        return result.ToString();
    }

    /// <inheritdoc cref="IRecord.Encode(MstRecord64)"/>
    public void Encode
        (
            MstRecord64 record
        )
    {
        Sure.NotNull (record);

        throw new NotImplementedException();
    }

    /// <summary>
    /// Поиск поля с указанной меткой.
    /// </summary>
    /// <param name="tag">Искомая метка поля.</param>
    /// <returns>Найденное поле либо <c>null</c>.</returns>
    public string? FM
        (
            int tag
        )
    {
        Sure.Positive (tag);

        Span<char> tagText = stackalloc char[10];
        tagText = tagText[..FastNumber.Int32ToChars (tag, tagText)];
        foreach (var field in Fields)
        {
            var navigator = new ValueTextNavigator (field);
            var opening = navigator.ReadUntil ('#');
            if (Utility.CompareSpans (tagText, opening) == 0)
            {
                return field;
            }
        }

        return null;
    }

    /// <summary>
    /// Parse MFN, status and version of the record
    /// </summary>
    public static RawRecord ParseMfnStatusVersion
        (
            string line1,
            string line2,
            RawRecord record
        )
    {
        Sure.NotNull (line1);
        Sure.NotNull (line2);
        Sure.NotNull (record);

        var regex = new Regex (@"^(-?\d+)\#(\d*)?");
        var match = regex.Match (line1);
        record.Mfn = Math.Abs (int.Parse (match.Groups[1].Value));
        if (match.Groups[2].Length > 0)
        {
            record.Status = (RecordStatus)int.Parse
                (
                    match.Groups[2].Value
                );
        }

        match = regex.Match (line2);
        if (match.Groups[2].Length > 0)
        {
            record.Version = int.Parse (match.Groups[2].Value);
        }

        return record;
    }

    /// <summary>
    /// Parse text.
    /// </summary>
    public static RawRecord Parse
        (
            string text
        )
    {
        Sure.NotNull (text);

        var lines = IrbisText.SplitIrbisToLines (text);

        var startOffset = 0;
        if (lines[0] == lines[1])
        {
            startOffset = 1;
        }

        var result = Parse (lines, startOffset);

        return result;
    }

    /// <summary>
    /// Parse text lines.
    /// </summary>
    public static RawRecord Parse
        (
            string[] lines,
            int startOffset = 0
        )
    {
        Sure.NotNull (lines);
        Sure.NonNegative (startOffset);

        if (lines.Length < 2)
        {
            Magna.Logger.LogError
                (
                    nameof (RawRecord) + "::" + nameof (Parse)
                    + ": text is too short"
                );

            throw new IrbisException ("Text too short");
        }

        var line1 = lines[startOffset + 0];
        var line2 = lines[startOffset + 1];

        var result = new RawRecord();
        result.Fields.AddRange (lines.Skip (startOffset + 2));

        ParseMfnStatusVersion
            (
                line1,
                line2,
                result
            );

        return result;
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString" />
    public override string ToString()
    {
        return Encode (Environment.NewLine);
    }

    #endregion
}
