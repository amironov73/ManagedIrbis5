// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* PropertyKind.cs --
 * Ars Magna project, http://arsmagna.ru
 */

namespace AM.Reporting.Data;

/// <summary>
/// Specifies a kind of property.
/// </summary>
public enum PropertyKind
{
    /// <summary>
    /// Specifies the property of a simple type (such as integer).
    /// </summary>
    Simple,

    /// <summary>
    /// Specifies the complex property such as class with own properties.
    /// </summary>
    Complex,

    /// <summary>
    /// Specifies the property which is a list of objects (is of IEnumerable type).
    /// </summary>
    Enumerable
}
