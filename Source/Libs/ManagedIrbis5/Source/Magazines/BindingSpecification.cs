// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local

/* BindingSpecification.cs -- спецификация подшивки
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.ComponentModel;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;

#endregion

#nullable enable

namespace ManagedIrbis.Magazines;

/// <summary>
/// Спецификация подшивки газет/журналов.
/// </summary>
public sealed class BindingSpecification
    : IVerifiable
{
    #region Properties

    /// <summary>
    /// Шифр журнала.
    /// </summary>
    /// <remarks>
    /// Например, "Л680583".
    /// </remarks>
    [JsonPropertyName ("magazine")]
    [XmlAttribute ("magazine")]
    [Description ("Шифр журнала")]
    public string? MagazineIndex { get; set; }

    /// <summary>
    /// Год.
    /// </summary>
    /// <remarks>
    /// Например, "2017".
    /// </remarks>
    [JsonPropertyName ("year")]
    [XmlAttribute ("year")]
    [Description ("Год")]
    public string? Year { get; set; }

    /// <summary>
    /// Номер тома.
    /// </summary>
    /// <remarks>
    /// Например, "123".
    /// </remarks>
    [JsonPropertyName ("volume")]
    [XmlAttribute ("volume")]
    [Description ("Номер тома")]
    public string? VolumeNumber { get; set; }

    /// <summary>
    /// Номера выпусков.
    /// </summary>
    /// <remarks>
    /// Например, "1-27,29-58,60-72".
    /// </remarks>
    [JsonPropertyName ("numbers")]
    [XmlAttribute ("numbers")]
    [Description ("Номера выпусков")]
    public string? IssueNumbers { get; set; }

    /// <summary>
    /// Описание.
    /// </summary>
    /// <remarks>
    /// Например, "янв.-июнь"
    /// </remarks>
    [JsonPropertyName ("description")]
    [XmlAttribute ("description")]
    [Description ("Описание подшивки")]
    public string? Description { get; set; }

    /// <summary>
    /// Номер подшивки в году.
    /// </summary>
    /// <remarks>
    /// Например, "6284".
    /// </remarks>
    [JsonPropertyName ("number")]
    [XmlAttribute ("number")]
    [Description ("Номер подшивки в году")]
    public string? BindingNumber { get; set; }

    /// <summary>
    /// Инвентарный номер подшивки.
    /// </summary>
    /// <remarks>
    /// Например, "Г6284".
    /// </remarks>
    [JsonPropertyName ("inventory")]
    [XmlAttribute ("inventory")]
    [Description ("Инвентарный номер подшивки")]
    public string? Inventory { get; set; }

    /// <summary>
    /// Место хранения подшивки.
    /// </summary>
    /// <remarks>
    /// Например, "ФП".
    /// </remarks>
    [JsonPropertyName ("place")]
    [XmlAttribute ("place")]
    [Description ("Место хранения подшивки")]
    public string? Place { get; set; }

    /// <summary>
    /// Номер комплекта.
    /// </summary>
    /// <remarks>
    /// Например, "1".
    /// </remarks>
    [JsonPropertyName ("complect")]
    [XmlAttribute ("complect")]
    [Description ("Номер комплекта")]
    public string? Complect { get; set; }

    #endregion

    #region IVerifiable members

    /// <inheritdoc cref="IVerifiable.Verify"/>
    public bool Verify (bool throwOnError)
    {
        throw new NotImplementedException();
    }

    #endregion
}