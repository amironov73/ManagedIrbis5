// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* IndexDescriptor.cs -- описание индекса в таблице
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Text.Json.Serialization;
using System.Xml.Serialization;

#endregion

#nullable enable

namespace AM.Data;

/// <summary>
/// Описание индекса в таблице.
/// </summary>
[XmlRoot ("index")]
public sealed class IndexDescriptor
    : IVerifiable
{
    #region Properties

    /// <summary>
    /// Имя индекса.
    /// </summary>
    [XmlAttribute ("name")]
    [JsonPropertyName ("name")]
    public string? Name { get; set; }

    /// <summary>
    /// Уникальный?
    /// </summary>
    [XmlAttribute ("unique")]
    [JsonPropertyName ("unique")]
    public bool Unique { get; set; }

    /// <summary>
    /// Поля.
    /// </summary>
    public string[]? Fields { get; set; }

    #endregion

    #region IVerifiable members

    /// <inheritdoc cref="IVerifiable.Verify"/>
    public bool Verify
        (
            bool throwOnError
        )
    {
        var verifier = new Verifier<IndexDescriptor> (this, throwOnError);

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
        return Name.ToVisibleString();
    }

    #endregion
}
