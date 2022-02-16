// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* DatabaseDescriptor.cs -- описание базы данных
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Text.Json.Serialization;
using System.Xml.Serialization;

#endregion

#nullable enable

namespace AM.Data;

/// <summary>
/// Описание базы данных.
/// </summary>
[XmlRoot ("database")]
public sealed class DatabaseDescriptor
    : IVerifiable
{
    #region Properties

    /// <summary>
    /// Имя базы данных.
    /// </summary>
    [XmlAttribute ("name")]
    [JsonPropertyName ("name")]
    public string? Name { get; set; }

    /// <summary>
    /// Таблицы.
    /// </summary>
    [XmlElement ("table")]
    [JsonPropertyName ("tables")]
    public TableDescriptor[]? Tables { get; set; }

    #endregion

    #region IVerifiable members

    /// <inheritdoc cref="IVerifiable.Verify"/>
    public bool Verify
        (
            bool throwOnError
        )
    {
        var verifier = new Verifier<DatabaseDescriptor> (this, throwOnError);

        verifier
            .NotNullNorEmpty (Name)
            .NotNullNorEmpty (Tables);

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
