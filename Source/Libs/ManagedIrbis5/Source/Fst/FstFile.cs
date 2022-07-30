// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UseNameofExpression

/* FstFile.cs -- файл с FST-скриптом
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;
using AM.Collections;
using AM.IO;
using AM.Runtime;
using AM.Text;

#endregion

#nullable enable

namespace ManagedIrbis.Fst;

/// <summary>
/// Работа с FST-скриптом.
/// </summary>
[XmlRoot ("fst")]
[DebuggerDisplay ("FileName = {FileName}")]
public sealed class FstFile
    : IHandmadeSerializable,
    IVerifiable
{
    #region Properties

    /// <summary>
    /// Имя файла (для идентификации).
    /// </summary>
    [XmlAttribute ("fileName")]
    [JsonPropertyName ("fileName")]
    public string? FileName { get; set; }

    /// <summary>
    /// Строки FST-файла.
    /// </summary>
    [XmlElement ("line")]
    [JsonPropertyName ("lines")]
    public NonNullCollection<FstLine> Lines { get; } = new ();

    #endregion

    #region Public methods

    /// <summary>
    /// Соединение форматов в одну строку.
    /// </summary>
    public string ConcatenateFormat()
    {
        var builder = StringBuilderPool.Shared.Get();

        foreach (var line in Lines)
        {
            builder.Append (line.ToFormat());
        }

        var result = builder.ToString();
        StringBuilderPool.Shared.Return (builder);

        return result;
    }

    /// <summary>
    /// Разбор потокового представления скрипта.
    /// </summary>
    public static FstFile ParseStream
        (
            TextReader reader
        )
    {
        Sure.NotNull (reader);

        var result = new FstFile();
        var lineNumber = 1;
        while (FstLine.ParseStream (reader) is { } line)
        {
            line.LineNumber = lineNumber;
            result.Lines.Add (line);
            lineNumber++;
        }

        return result;
    }

    /// <summary>
    /// Чтение локального файла.
    /// </summary>
    public static FstFile ParseLocalFile
        (
            string fileName,
            Encoding encoding
        )
    {
        Sure.FileExists (fileName);
        Sure.NotNull (encoding);

        using TextReader reader = new StreamReader (fileName, encoding);
        var result = ParseStream (reader);
        result.FileName = fileName;

        return result;
    }

    /// <summary>
    /// Should serialize the <see cref="Lines"/> collection?
    /// </summary>
    [ExcludeFromCodeCoverage]
    public bool ShouldSerializeLines()
    {
        return Lines.Count != 0;
    }

    #endregion

    #region IHandmadeSerializable members

    /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
    public void RestoreFromStream
        (
            BinaryReader reader
        )
    {
        Sure.NotNull (reader);

        FileName = reader.ReadNullableString();
        reader.ReadCollection (Lines);
    }

    /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
    public void SaveToStream
        (
            BinaryWriter writer
        )
    {
        Sure.NotNull (writer);

        writer.WriteNullable (FileName);
        writer.WriteCollection (Lines);
    }

    #endregion

    #region IVerifiable members

    /// <inheritdoc cref="IVerifiable.Verify" />
    public bool Verify
        (
            bool throwOnError
        )
    {
        var verifier = new Verifier<FstFile> (this, throwOnError);

        verifier.Assert (Lines.Count != 0);
        foreach (var line in Lines)
        {
            verifier.VerifySubObject (line);
        }

        return verifier.Result;
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString" />
    public override string ToString()
    {
        return FileName.ToVisibleString();
    }

    #endregion
}
