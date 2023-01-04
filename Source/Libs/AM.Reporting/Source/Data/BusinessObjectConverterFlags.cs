// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global

/* BusinessObjectConverterFlags.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

namespace AM.Reporting.Data;

/// <summary>
/// <b>Obsolete</b>. Specifies a set of flags used to convert business objects into datasources.
/// </summary>
[Flags]
public enum BusinessObjectConverterFlags
{
    /// <summary>
    /// Specifies no actions.
    /// </summary>
    None,

    /// <summary>
    /// Allows using the fields of a business object.
    /// </summary>
    AllowFields,

    /// <summary>
    /// Allows using properties of a business object with <b>BrowsableAttribute</b> only.
    /// </summary>
    BrowsableOnly
}
