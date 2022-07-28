// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedParameter.Local

/* GblFile.cs -- файл GBL
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;
using AM.Collections;
using AM.IO;
using AM.Runtime;

using ManagedIrbis.Infrastructure;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace ManagedIrbis.Gbl;

/// <summary>
/// Файл GBL.
/// </summary>
[XmlRoot ("gbl")]
public sealed class GblFile
    : IHandmadeSerializable,
        IVerifiable
{
    #region Properties

    /// <summary>
    /// Имя файла.
    /// </summary>
    [XmlIgnore]
    [JsonIgnore]
    public string? FileName { get; set; }

    /// <summary>
    /// Операторы глобальной корректировки.
    /// </summary>
    [XmlElement ("item")]
    [JsonPropertyName ("items")]
    public NonNullCollection<GblStatement> Statements { get; } = new ();

    /// <summary>
    /// Параметры глобальной корректировки.
    /// </summary>
    [XmlElement ("parameter")]
    [JsonPropertyName ("parameters")]
    public NonNullCollection<GblParameter> Parameters { get; } = new ();

    #endregion

    #region Public methods

    /// <summary>
    /// Выполнение всех операторов глобальной корректировки.
    /// </summary>
    public void Execute
        (
            Record record,
            ISyncProvider provider
        )
    {
        Sure.NotNull (record);
        Sure.NotNull (provider);

        // TODO: implement
    }

    /// <summary>
    /// Чтение локального файла.
    /// </summary>
    public static GblFile ParseLocalFile
        (
            string fileName,
            Encoding? encoding = null
        )
    {
        Sure.FileExists (fileName);

        encoding ??= IrbisEncoding.Ansi;

        using var reader = new StreamReader (fileName, encoding);
        var result = Decode (reader);

        return result;
    }

    /// <summary>
    /// Разбор текстового потока.
    /// </summary>
    public static GblFile Decode
        (
            TextReader reader
        )
    {
        Sure.NotNull (reader);

        var result = new GblFile();

        var line = reader.RequireLine();
        var count = line.ParseInt32();
        for (var i = 0; i < count; i++)
        {
            var parameter = GblParameter.Decode (reader);
            result.Parameters.Add (parameter);
        }

        while (true)
        {
            var statement = GblStatement.ParseStream (reader);
            if (statement is null)
            {
                break;
            }

            result.Statements.Add (statement);
        }

        return result;
    }

    #endregion

    #region IHandmadeSerializable members

    /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream"/>
    public void RestoreFromStream
        (
            BinaryReader reader
        )
    {
        Sure.NotNull (reader);

        FileName = reader.ReadNullableString();
        reader.ReadCollection (Parameters);
        reader.ReadCollection (Statements);
    }

    /// <inheritdoc cref="IHandmadeSerializable.SaveToStream"/>
    public void SaveToStream
        (
            BinaryWriter writer
        )
    {
        Sure.NotNull (writer);

        writer.WriteNullable (FileName);
        writer.WriteCollection (Parameters);
        writer.WriteCollection (Statements);
    }

    #endregion

    #region IVerifiable members

    /// <inheritdoc cref="IVerifiable.Verify"/>
    public bool Verify
        (
            bool throwOnError
        )
    {
        var result = Statements.Count != 0;

        if (result)
        {
            result = Statements.All
                (
                    item => item.Verify (throwOnError)
                );
        }

        if (result && Parameters.Count != 0)
        {
            result = Parameters.All
                (
                    parameter => parameter.Verify (throwOnError)
                );
        }

        if (!result)
        {
            Magna.Logger.LogError
                (
                    nameof (GblFile) + "::" + nameof (Verify)
                    + ": verification error"
                );

            if (throwOnError)
            {
                throw new VerificationException();
            }
        }

        return result;
    }

    #endregion
}
