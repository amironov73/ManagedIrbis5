// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo

/* MailerConfiguration.cs -- конфигурация мэйлера
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Xml.Serialization;

#endregion

#nullable enable

namespace BulletinMaker;

/// <summary>
/// Конфигурация мэйлера.
/// </summary>
public sealed class MailerConfiguration
{
    #region Properties

    [XmlAttribute("server")]
    public string? Server { get; set; }

    [XmlAttribute("from")]
    public string? From { get; set; }

    #endregion
}
