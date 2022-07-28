// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedMember.Global

/* WssFile.cs -- вложенный рабочий лист
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

using ManagedIrbis.Infrastructure;

#endregion

#nullable enable

namespace ManagedIrbis.Workspace;

/// <summary>
/// Вложенный рабочий лист.
/// </summary>
[XmlRoot ("wss")]
[DebuggerDisplay ("{" + nameof (Name) + "}")]
public sealed class WssFile
    : IHandmadeSerializable,
        IVerifiable
{
    #region Properties

    /// <summary>
    /// Имя рабочего листа.
    /// </summary>
    [XmlAttribute ("name")]
    [JsonPropertyName ("name")]
    public string? Name { get; set; }

    /// <summary>
    /// Элементы рабочего листа.
    /// </summary>
    [XmlArray ("items")]
    [XmlArrayItem ("item")]
    [JsonPropertyName ("items")]
    public NonNullCollection<WorksheetItem> Items { get; private set; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор
    /// </summary>
    public WssFile()
    {
        Items = new NonNullCollection<WorksheetItem>();
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Разбор потока.
    /// </summary>
    public static WssFile ParseStream
        (
            TextReader reader
        )
    {
        var result = new WssFile();

        var count = int.Parse (reader.RequireLine());

        for (var i = 0; i < count; i++)
        {
            var item = WorksheetItem.ParseStream (reader);
            result.Items.Add (item);
        }

        return result;
    }

    /// <summary>
    /// Read from server.
    /// </summary>
    public static WssFile? ReadFromServer
        (
            ISyncProvider provider,
            FileSpecification specification
        )
    {
        var content = provider.ReadTextFile (specification);
        if (string.IsNullOrEmpty (content))
        {
            return null;
        }

        using var reader = new StringReader (content);
        return ParseStream (reader);
    }

    /// <summary>
    /// Считывание из локального файла.
    /// </summary>
    public static WssFile ReadLocalFile
        (
            string fileName,
            Encoding encoding
        )
    {
        using var reader = TextReaderUtility.OpenRead
            (
                fileName,
                encoding
            );
        var result = ParseStream (reader);

        result.Name = Path.GetFileName (fileName);

        return result;
    }

    /// <summary>
    /// Считывание из локального файла.
    /// </summary>
    public static WssFile ReadLocalFile
        (
            string fileName
        )
    {
        return ReadLocalFile
            (
                fileName,
                IrbisEncoding.Ansi
            );
    }

    /// <summary>
    /// Should serialize the <see cref="Items"/> collection?
    /// </summary>
    [ExcludeFromCodeCoverage]
    public bool ShouldSerializeItems()
    {
        return Items.Count != 0;
    }

    #endregion

    #region IHandmadeSerializable members

    /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
    public void RestoreFromStream
        (
            BinaryReader reader
        )
    {
        Name = reader.ReadNullableString();
        Items = reader.ReadNonNullCollection<WorksheetItem>();
    }

    /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
    public void SaveToStream
        (
            BinaryWriter writer
        )
    {
        writer.WriteNullable (Name);
        writer.WriteCollection (Items);
    }

    #endregion

    #region IVerifiable members

    /// <inheritdoc cref="IVerifiable.Verify" />
    public bool Verify
        (
            bool throwOnError
        )
    {
        var verifier = new Verifier<WssFile> (this, throwOnError);

        foreach (var item in Items)
        {
            verifier.VerifySubObject (item);
        }

        return verifier.Result;
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString" />
    public override string ToString()
    {
        return Name.ToVisibleString();
    }

    #endregion
}
