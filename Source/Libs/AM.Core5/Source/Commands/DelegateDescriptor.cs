// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming

/* DelegateDescriptor.cs -- описатель делегата
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Text.Json.Serialization;
using System.Xml.Serialization;

using JetBrains.Annotations;

#endregion

namespace AM.Commands;

/// <summary>
/// Описатель делегата.
/// </summary>
[PublicAPI]
[XmlRoot ("delegate")]
public sealed class DelegateDescriptor
{
    #region Properties

    /// <summary>
    /// Объект, к которому привязан делегат.
    /// </summary>
    [XmlAttribute ("object")]
    [JsonPropertyName ("object")]
    public string? Object { get; set; }

    /// <summary>
    /// Метод, выполняющий действие
    /// </summary>
    [XmlAttribute ("method")]
    [JsonPropertyName ("method")]
    public string? Method { get; set; }

    #endregion
}
