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
using Avalonia.Data;

#endregion

#nullable enable

namespace Avalonia.ExtendedToolkit.Controls.PropertyGrid.Design;

//
// ported from https://github.com/DenisVuyka/WPG
//

/// <summary>
/// GridEntryLayoutContainer T is GridEntryContainer
/// </summary>
/// <typeparam name="T"></typeparam>
internal class GridEntryLayoutContainer<T>
    : ItemContainerGenerator<T>
    where T : GridEntryContainer, new()
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="owner"></param>
    public GridEntryLayoutContainer
        (
            GridEntryLayout<T> owner
        )
        : base (owner, ContentControl.ContentProperty, ItemsControl.ItemTemplateProperty)
    {
        Owner = owner;
    }

    #endregion

    /// <summary>
    ///
    /// </summary>
    public new GridEntryLayout<T> Owner { get; }

    /// <inheritdoc cref="ItemContainerGenerator{T}.CreateContainer"/>
    protected override IControl CreateContainer
        (
            object element
        )
    {
        if (element is GridEntryContainer item)
        {
            item.DataContext = Owner.DataContext;
            item.Bind(GridEntryContainer.EntryProperty, new Binding());
            return item;
        }
        return base.CreateContainer(element);
    }
}
