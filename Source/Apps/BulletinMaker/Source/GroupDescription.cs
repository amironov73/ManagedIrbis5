// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo

/* GroupDescription.cs -- описание группы
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Xml.Serialization;

#endregion

namespace BulletinMaker;

/// <summary>
/// Описание группы.
/// </summary>
public sealed class GroupDescription
{
    /// <summary>
    ///
    /// </summary>
    [XmlAttribute("title")]
    public string? Title { get; set; }

    /// <summary>
    ///
    /// </summary>
    [XmlAttribute("filter")]
    public string? Filter { get; set; }

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        return Title;
    }
}
