// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable EventNeverSubscribedTo.Global
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMemberInSuper.Global
// ReSharper disable UnusedParameter.Local
// ReSharper disable UnusedType.Global

/* DelegateDescriptor.cs -- описатель делегата
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Text.Json.Serialization;
using System.Xml.Serialization;

#endregion

#nullable enable

namespace AM.Commands;

/// <summary>
/// Описатель делегата.
/// </summary>
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
