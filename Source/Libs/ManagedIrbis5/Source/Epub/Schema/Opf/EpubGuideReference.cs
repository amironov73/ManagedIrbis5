// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* EpubGuideReference.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace ManagedIrbis.Epub.Schema;

/// <summary>
///
/// </summary>
public class EpubGuideReference
{
    #region Properties

    /// <summary>
    ///
    /// </summary>
    public string? Type { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string? Href { get; set; }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        return $"Type: {Type}, Href: {Href}";
    }

    #endregion
}
