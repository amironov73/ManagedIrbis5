// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* Epub3Nav.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace ManagedIrbis.Epub.Schema;

/// <summary>
///
/// </summary>
public sealed class Epub3Nav
{
    #region Properties

    /// <summary>
    ///
    /// </summary>
    public StructuralSemanticsProperty? Type { get; set; }

    /// <summary>
    ///
    /// </summary>
    public bool IsHidden { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string? Head { get; set; }

    /// <summary>
    ///
    /// </summary>
    public Epub3NavOl? Ol { get; set; }

    #endregion
}
