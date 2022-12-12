// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedParameter.Local

/* NamedAreaDefinition.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace GridExtra.Avalonia;

/// <summary>
///
/// </summary>
public class NamedAreaDefinition
    : AreaDefinition
{
    #region Properties

    /// <summary>
    ///
    /// </summary>
    public string Name { get; set; }

    #endregion

    #region Construction

    /// <summary>
    ///
    /// </summary>
    /// <param name="name"></param>
    /// <param name="row"></param>
    /// <param name="column"></param>
    /// <param name="rowSpan"></param>
    /// <param name="columnSpan"></param>
    public NamedAreaDefinition
        (
            string name,
            int row,
            int column,
            int rowSpan,
            int columnSpan
        )
        : base (row, column, rowSpan, columnSpan)
    {
        Name = name;
    }

    #endregion
}
