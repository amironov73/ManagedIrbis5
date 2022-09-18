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
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;

#endregion

#nullable enable

namespace Avalonia.ExtendedToolkit.Controls;

/// <summary>
/// a headered itemscontrol with <see cref="IndexListItem"/>
/// </summary>
public class IndexListHeaderItem : HeaderedItemsControl
{
    /// <summary>
    /// Gets or sets ShowEmptyItems.
    /// </summary>
    public bool ShowEmptyItems
    {
        get => GetValue (ShowEmptyItemsProperty);
        set => SetValue (ShowEmptyItemsProperty, value);
    }

    /// <summary>
    /// Defines the ShowEmptyItems property.
    /// </summary>
    public static readonly StyledProperty<bool> ShowEmptyItemsProperty =

        //AvaloniaProperty.Register<IndexList, bool>(nameof(ShowEmptyItems));
        IndexList.ShowEmptyItemsProperty.AddOwner<IndexListHeaderItem>();

    private static readonly ITemplate<IPanel> DefaultPanel =
        new FuncTemplate<IPanel> (() => new StackPanel());

    /// <summary>
    /// overrides some default values
    /// </summary>
    static IndexListHeaderItem()
    {
        FocusableProperty.OverrideDefaultValue<IndexListHeaderItem> (false);
        ItemsPanelProperty.OverrideDefaultValue<IndexListHeaderItem> (DefaultPanel);
    }

    /// <inheritdoc cref="ItemsControl.CreateItemContainerGenerator"/>
    protected override IItemContainerGenerator CreateItemContainerGenerator()
    {
        return new ItemContainerGenerator<IndexListItem>
            (
                this,
                ContentControl.ContentProperty,
                ContentControl.ContentTemplateProperty
            );
    }
}
