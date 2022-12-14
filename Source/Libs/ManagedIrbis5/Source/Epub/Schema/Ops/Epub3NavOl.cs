// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* Epub3NavOl.cs --
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
public class Epub3NavOl
{
    /// <summary>
    ///
    /// </summary>
    public bool IsHidden { get; set; }

    /// <summary>
    ///
    /// </summary>
    public List<Epub3NavLi>? Lis { get; set; }
}
