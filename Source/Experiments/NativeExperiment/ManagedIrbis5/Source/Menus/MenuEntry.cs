// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* MenuEntry.cs -- пара строк в MNU-файле
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;
using AM.IO;
using AM.Runtime;

#endregion

#nullable enable

namespace ManagedIrbis.Menus;

/// <summary>
/// Пара строк в MNU-файле: код и соответствующее значение
/// (либо комментарий).
/// </summary>
[XmlRoot ("entry")]
[DebuggerDisplay ("{" + nameof(Code) + "} = {" + nameof(Comment) + "}")]
public sealed class MenuEntry
    : IHandmadeSerializable,
    IVerifiable
{
    #region Properties

    /// <summary>
    /// Первая строка - код.
    /// Коды могут повторяться в рамках одного MNU-файла.
    /// </summary>
    [XmlAttribute ("code")]
    [JsonPropertyName ("code")]
    public string? Code { get; set; }

    /// <summary>
    /// Вторая строка - значение либо комментарий.
    /// Часто бывает пустой.
    /// </summary>
    [XmlAttribute ("comment")]
    [JsonPropertyName ("comment")]
    public string? Comment { get; set; }

    /// <summary>
    /// Ссылка на другую пару строк, применяется при построении дерева
    /// (TRE-файла).
    /// </summary>
    [XmlIgnore]
    [JsonIgnore]
    public MenuEntry? OtherEntry { get; set; }

    #endregion

    #region IHandmadeSerializable

    /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
    public void RestoreFromStream
        (
            BinaryReader reader
        )
    {
        Code = reader.ReadNullableString();
        Comment = reader.ReadNullableString();
    }

    /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
    public void SaveToStream (BinaryWriter writer) => writer
        .WriteNullable (Code)
        .WriteNullable (Comment);

    #endregion

    #region Public methods

    /// <summary>
    /// Should JSON serialize the comment?
    /// </summary>
    [ExcludeFromCodeCoverage]
    public bool ShouldSerializeComment() => !string.IsNullOrEmpty (Comment);

    #endregion

    #region IVerifiable members

    /// <inheritdoc cref="IVerifiable.Verify"/>
    public bool Verify
        (
            bool throwOnError
        )
    {
        var verifier = new Verifier<MenuEntry> (this, throwOnError);

        verifier.NotNullNorEmpty (Code);

        return verifier.Result;
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString" />
    public override string ToString() => string.IsNullOrEmpty (Comment)
        ? Code.ToVisibleString()
        : $"{Code} - {Comment}";

    #endregion
}
