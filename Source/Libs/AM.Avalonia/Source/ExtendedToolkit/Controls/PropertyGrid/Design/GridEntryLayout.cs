// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/*
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using Avalonia.Controls;
using Avalonia.Controls.Generators;

#endregion

#nullable enable

namespace Avalonia.ExtendedToolkit.Controls.PropertyGrid.Design;

//
// ported from https://github.com/DenisVuyka/WPG
//

/// <summary>
/// Defines a basement for GridEntry UI layouts (panels, lists, etc)
/// </summary>
/// <typeparam name="T">The type of elements in the control.</typeparam>
public abstract class GridEntryLayout<T>
    : ItemsControl
    where T : GridEntryContainer, new()
{
    /// <inheritdoc cref="ItemsControl.CreateItemContainerGenerator"/>
    protected override IItemContainerGenerator CreateItemContainerGenerator()
    {
        var result = new GridEntryLayoutContainer<T>(this);

        return result;
    }
}
