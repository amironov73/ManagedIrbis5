// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* FstLine.cs -- FST file line
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;
using AM.IO;
using AM.Runtime;

using ManagedIrbis.Infrastructure;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace ManagedIrbis.Fst;

//
// ТВП состоит из набора строк, каждая из которых содержит
// следующие три параметра, разделенные знаком пробел:
// * формат выборки данных, представленный на языке форматирования системы,
// * идентификатор поля(ИП),
// * метод индексирования(МИ).
//

/// <summary>
/// FST file line.
/// </summary>
[XmlRoot ("line")]
[DebuggerDisplay ("{Tag} {Method} {Format}")]
public sealed class FstLine
    : IHandmadeSerializable,
        IVerifiable
{
    #region Properties

    /// <summary>
    /// Line number.
    /// </summary>
    [XmlIgnore]
    [JsonIgnore]
    [Browsable (false)]
    public int LineNumber { get; set; }

    /// <summary>
    /// Field tag.
    /// </summary>
    [XmlAttribute ("tag")]
    [JsonPropertyName ("tag")]
    [Description ("Метка поля")]
    [DisplayName ("Метка поля")]
    public int Tag { get; set; }

    /// <summary>
    /// Index method.
    /// </summary>
    [XmlAttribute ("method")]
    [JsonPropertyName ("method")]
    [Description ("Метод")]
    [DisplayName ("Метод")]
    public FstIndexMethod Method { get; set; }

    /// <summary>
    /// Format itself.
    /// </summary>
    [XmlElement ("format")]
    [JsonPropertyName ("format")]
    [Description ("Формат")]
    [DisplayName ("Формат")]
    public string? Format { get; set; }

    /// <summary>
    /// Arbitrary user data.
    /// </summary>
    [XmlIgnore]
    [JsonIgnore]
    [Browsable (false)]
    [ExcludeFromCodeCoverage]
    public object? UserData { get; set; }

    #endregion

    #region Public methods

    /// <summary>
    /// Parse one line from the stream.
    /// </summary>
    public static FstLine? ParseStream
        (
            TextReader reader
        )
    {
        Sure.NotNull (reader);

        string? line;
        while (true)
        {
            line = reader.ReadLine();
            if (ReferenceEquals (line, null))
            {
                return null;
            }

            line = line.Trim();
            if (!string.IsNullOrEmpty (line))
            {
                break;
            }
        }

        line = line.Replace ('\x1A', ' ');
        line = line.Trim();
        if (string.IsNullOrEmpty (line))
        {
            return null;
        }

        char[] delimiters = { ' ', '\t' };
        var parts = line.Split
            (
                delimiters,
                3,
                StringSplitOptions.RemoveEmptyEntries
            );

        if (parts.Length != 3)
        {
            Magna.Logger.LogError
                (
                    nameof (FstLine) + "::" + nameof (ParseStream)
                    + ": bad line: {Line}",
                    line.ToVisibleString()
                );

            throw new FormatException
                (
                    "Bad FST line: "
                    + line.ToVisibleString()
                );
        }

        var result = new FstLine
        {
            Tag = FastNumber.ParseInt32 (parts[0]),
            Method = (FstIndexMethod)int.Parse (parts[1]),
            Format = parts[2]
        };

        return result;
    }

    /// <summary>
    /// Convert line to the IRBIS format.
    /// </summary>
    public string ToFormat()
    {
        var result = new StringBuilder();

        result.AppendFormat
            (
                "mpl,'{0}',/,",
                Tag
            );
        result.Append
            (
                IrbisFormat.PrepareFormat (Format)
            );
        result.Append (",'\x07'");

        return result.ToString();
    }

    #endregion

    #region IHandmadeSerializable

    /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
    public void RestoreFromStream
        (
            BinaryReader reader
        )
    {
        Sure.NotNull (reader);

        LineNumber = reader.ReadPackedInt32();
        Tag = reader.ReadPackedInt32();
        Method = (FstIndexMethod)reader.ReadPackedInt32();
        Format = reader.ReadNullableString();
    }

    /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
    public void SaveToStream
        (
            BinaryWriter writer
        )
    {
        Sure.NotNull (writer);

        writer.WritePackedInt32 (LineNumber)
            .WritePackedInt32 (Tag)
            .WritePackedInt32 ((int)Method)
            .WriteNullable (Format);
    }

    #endregion

    #region IVerifiable members

    /// <inheritdoc cref="IVerifiable.Verify" />
    public bool Verify
        (
            bool throwOnError
        )
    {
        var verifier = new Verifier<FstLine> (this, throwOnError);

        verifier
            .NotNullNorEmpty (Format);

        return verifier.Result;
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString" />
    public override string ToString()
    {
        return $"{Tag} {(int)Method} {Format.ToVisibleString()}";
    }

    #endregion
}
