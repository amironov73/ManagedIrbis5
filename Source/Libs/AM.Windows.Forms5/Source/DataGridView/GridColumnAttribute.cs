// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* GridColumnAttribute.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.ComponentModel;

#endregion

#nullable enable

namespace AM.Windows.Forms;

/// <summary>
///
/// </summary>
[AttributeUsage (AttributeTargets.Property)]
public class GridColumnAttribute
    : Attribute
{
    #region Properties

    /// <summary>
    /// Gets or sets the type of the column.
    /// </summary>
    public Type? ColumnType { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this
    /// <see cref="GridColumnAttribute"/> is frozen.
    /// </summary>
    [DefaultValue (false)]
    public bool Frozen { get; set; }

    /// <summary>
    /// Gets or sets the header text.
    /// </summary>
    /// <value>The header text.</value>
    public string? HeaderText { get; set; }

    /// <summary>
    /// Gets or sets the index.
    /// </summary>
    [DefaultValue (-1)]
    public int Index { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether [read only].
    /// </summary>
    [DefaultValue (false)]
    public bool ReadOnly { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this
    /// <see cref="GridColumnAttribute"/> is resizeable.
    /// </summary>
    [DefaultValue (false)]
    public bool Resizeable { get; set; }

    #endregion

    #region Construction

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="GridColumnAttribute"/> class.
    /// </summary>
    public GridColumnAttribute()
    {
        Index = -1;
    }

    /// <summary>
    /// Constructor
    /// </summary>
    public GridColumnAttribute
        (
            string headerText
        )
        : this()
    {
        HeaderText = headerText;
    }

    #endregion
}
