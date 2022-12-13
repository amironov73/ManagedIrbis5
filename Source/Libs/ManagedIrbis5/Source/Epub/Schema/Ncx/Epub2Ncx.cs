// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* Epub2Ncx.cs --
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
public class Epub2Ncx
{
    /// <summary>
    ///
    /// </summary>
    public Epub2NcxHead? Head { get; set; }

    /// <summary>
    ///
    /// </summary>
    public Epub2NcxDocTitle? DocTitle { get; set; }

    /// <summary>
    ///
    /// </summary>
    public List<Epub2NcxDocAuthor>? DocAuthors { get; set; }

    /// <summary>
    ///
    /// </summary>
    public Epub2NcxNavigationMap? NavMap { get; set; }

    /// <summary>
    ///
    /// </summary>
    public Epub2NcxPageList? PageList { get; set; }

    /// <summary>
    ///
    /// </summary>
    public List<Epub2NcxNavigationList>? NavLists { get; set; }
}
