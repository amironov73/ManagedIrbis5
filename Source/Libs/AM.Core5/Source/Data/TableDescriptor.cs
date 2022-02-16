// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* TableDescriptor.cs -- описание таблицы
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Text.Json.Serialization;
using System.Xml.Serialization;

#endregion

#nullable enable

namespace AM.Data;

/// <summary>
/// Описание таблицы.
/// </summary>
[XmlRoot ("table")]
public sealed class TableDescriptor
    : IVerifiable
{
    #region Properties

    /// <summary>
    /// Имя таблицы.
    /// </summary>
    [XmlAttribute ("name")]
    [JsonPropertyName ("name")]
    public string? Name { get; set; }

    /// <summary>
    /// Поля таблицы.
    /// </summary>
    [XmlElement ("field")]
    [JsonPropertyName ("fields")]
    public FieldDescriptor[]? Fields { get; set; }

    /// <summary>
    /// Индексы.
    /// </summary>
    [XmlElement ("index")]
    [JsonPropertyName ("indexes")]
    public IndexDescriptor[]? Indexes { get; set; }

    #endregion

    #region IVerifiable members

    /// <inheritdoc cref="IVerifiable.Verify"/>
    public bool Verify
        (
            bool throwOnError
        )
    {
        var verifier = new Verifier<TableDescriptor> (this, throwOnError);

        verifier
            .NotNullNorEmpty (Name)
            .NotNullNorEmpty (Fields);

        return verifier.Result;
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        return $"{Name}";
    }

    #endregion
}
