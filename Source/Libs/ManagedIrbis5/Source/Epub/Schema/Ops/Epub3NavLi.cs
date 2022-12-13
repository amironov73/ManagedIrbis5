// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* Epub3NavLi.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace ManagedIrbis.Epub.Schema;

/// <summary>
///
/// </summary>
public class Epub3NavLi
{
    /// <summary>
    ///
    /// </summary>
    public Epub3NavAnchor? Anchor { get; set; }

    /// <summary>
    ///
    /// </summary>
    public Epub3NavSpan? Span { get; set; }

    /// <summary>
    ///
    /// </summary>
    public Epub3NavOl? ChildOl { get; set; }
}
