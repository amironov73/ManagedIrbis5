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

using System.ComponentModel;
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
using ManagedIrbis.Providers;

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
    public NonNullCollection<WorksheetItem> Items { get; private set; } = new ();

    #endregion

    #region Public methods

    /// <summary>
    /// Разбор текстового потока.
    /// </summary>
    public static WssFile ParseStream
        (
            TextReader reader
        )
    {
        Sure.NotNull (reader);

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
    /// Чтение рабочего листа с сервера.
    /// </summary>
    public static WssFile? ReadFromServer
        (
            ISyncProvider provider,
            FileSpecification specification
        )
    {
        Sure.NotNull (provider);
        Sure.VerifyNotNull (specification);
        provider.EnsureConnected();

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
            Encoding? encoding = null
        )
    {
        Sure.FileExists (fileName);

        encoding ??= IrbisEncoding.Ansi;

        using var reader = TextReaderUtility.OpenRead (fileName, encoding);
        var result = ParseStream (reader);
        result.Name = Path.GetFileName (fileName);

        return result;
    }

    /// <summary>
    /// Should serialize the <see cref="Items"/> collection?
    /// </summary>
    [ExcludeFromCodeCoverage]
    [EditorBrowsable (EditorBrowsableState.Never)]
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
        Sure.NotNull (reader);

        Name = reader.ReadNullableString();
        Items = reader.ReadNonNullCollection<WorksheetItem>();
    }

    /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
    public void SaveToStream
        (
            BinaryWriter writer
        )
    {
        Sure.NotNull (writer);

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
