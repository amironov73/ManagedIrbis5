// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* Epub2NcxNavigationTarget.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;

#endregion

#nullable enable

namespace ManagedIrbis.Epub.Schema;

/// <summary>
///
/// </summary>
public class Epub2NcxNavigationTarget
{
    /// <summary>
    ///
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string? Class { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string? Value { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string? PlayOrder { get; set; }

    /// <summary>
    ///
    /// </summary>
    public List<Epub2NcxNavigationLabel>? NavigationLabels { get; set; }

    /// <summary>
    ///
    /// </summary>
    public Epub2NcxContent? Content { get; set; }
}
