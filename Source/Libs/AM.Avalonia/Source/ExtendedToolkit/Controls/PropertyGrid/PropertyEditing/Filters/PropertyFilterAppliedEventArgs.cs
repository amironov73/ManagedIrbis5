// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable NonReadonlyMemberInGetHashCode
// ReSharper disable UnusedMember.Global

/* PropertyFilterAppliedEventArgs.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace Avalonia.ExtendedToolkit.Controls.PropertyGrid.PropertyEditing;

//
// ported from https://github.com/DenisVuyka/WPG
//

/// <summary>
/// Contains state information and data related to FilterApplied event.
/// </summary>
public sealed class PropertyFilterAppliedEventArgs
    : EventArgs
{
    #region Properties

    /// <summary>
    /// Gets the filter.
    /// </summary>
    /// <value>The filter.</value>
    public PropertyFilter? Filter { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Initializes a new instance of the <see cref="PropertyFilterAppliedEventArgs"/> class.
    /// </summary>
    /// <param name="filter">The filter.</param>
    public PropertyFilterAppliedEventArgs
        (
            PropertyFilter? filter
        )
    {
        Filter = filter;
    }

    #endregion
}
