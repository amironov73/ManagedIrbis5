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

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Avalonia.Controls;
using Avalonia.Controls.Generators;
using Avalonia.ExtendedToolkit.Extensions;
using Avalonia.Input;

#endregion

#nullable enable

namespace Avalonia.ExtendedToolkit.Controls;

//ported from https://github.com/MahApps/MahApps.Metro

/// <summary>
/// A FlyoutsControl is for displaying flyouts in a MetroWindow.
/// <see cref="MetroWindow"/>
/// </summary>
public class FlyoutsControl : ItemsControl
{
    /// <summary>
    /// Gets/sets whether
    /// <see cref="Avalonia.ExtendedToolkit.Controls.Flyout.ExternalCloseButton"/>
    /// is ignored and all flyouts behave as if it was set to the value of this property.
    /// </summary>
    public MouseButton? OverrideExternalCloseButton
    {
        get => GetValue (OverrideExternalCloseButtonProperty);
        set => SetValue (OverrideExternalCloseButtonProperty, value);
    }

    /// <summary>
    /// <see cref="OverrideExternalCloseButton"/>
    /// </summary>
    public static readonly StyledProperty<MouseButton?> OverrideExternalCloseButtonProperty =
        AvaloniaProperty.Register<FlyoutsControl, MouseButton?> (nameof (OverrideExternalCloseButton));

    /// <summary>
    /// Gets/sets whether
    /// <see cref="Avalonia.ExtendedToolkit.Controls.Flyout.IsPinned"/>
    /// is ignored and all flyouts behave as if it was set false.
    /// </summary>
    public bool OverrideIsPinned
    {
        get => GetValue (OverrideIsPinnedProperty);
        set => SetValue (OverrideIsPinnedProperty, value);
    }

    /// <summary>
    /// <see cref="OverrideIsPinned"/>
    /// </summary>
    public static readonly StyledProperty<bool> OverrideIsPinnedProperty =
        AvaloniaProperty.Register<FlyoutsControl, bool> (nameof (OverrideIsPinned));

    /// <summary>
    /// registers changed events
    /// </summary>
    public FlyoutsControl()
    {
        //ItemsPanelProperty.OverrideDefaultValue<FlyoutsControl>(DefaultPanel);
        ItemsProperty.Changed.AddClassHandler<FlyoutsControl> (OnItemsChaned);
    }

    private void OnItemsChaned (FlyoutsControl o, AvaloniaPropertyChangedEventArgs e)
    {
        IsVisible = true;
    }

    /// <inheritdoc cref="ItemsControl.CreateItemContainerGenerator"/>
    protected override IItemContainerGenerator CreateItemContainerGenerator()
    {
        return new FlyoutContainerGenerator (this);
    }

    /// <summary>
    /// called from the <see cref="FlyoutContainerGenerator.CreateContainer(object)"/> only.
    /// </summary>
    /// <param name="flyout"></param>
    internal void AttachHandlers
        (
            Flyout flyout
        )
    {
        flyout.IsOpenChanged -= FlyoutStatusChanged;
        flyout.FlyoutThemeChanged -= FlyoutStatusChanged;

        flyout.IsOpenChanged += FlyoutStatusChanged;
        flyout.FlyoutThemeChanged += FlyoutStatusChanged;

        //var isOpenNotifier = new PropertyChangeNotifier(flyout, Flyout.IsOpenProperty);
        //isOpenNotifier.ValueChanged += FlyoutStatusChanged;
        //flyout.IsOpenPropertyChangeNotifier = isOpenNotifier;

        //var themeNotifier = new PropertyChangeNotifier(flyout, Flyout.FlyoutThemeProperty);
        //themeNotifier.ValueChanged += FlyoutStatusChanged;
        //flyout.ThemePropertyChangeNotifier = themeNotifier;
    }

    private void FlyoutStatusChanged
        (
            object? sender,
            EventArgs eventArgs
        )
    {
        var flyout = GetFlyout (sender); //Get the flyout that raised the handler.

        HandleFlyoutStatusChange (flyout, this.TryFindParent<MetroWindow>());
    }

    internal void HandleFlyoutStatusChange
        (
            Flyout? flyout,
            MetroWindow? parentWindow
        )
    {
        if (flyout == null || parentWindow == null)
        {
            return;
        }

        ReorderZIndices (flyout);

        var visibleFlyouts = GetFlyouts (Items).Where (i => i.IsOpen).OrderBy (x => x.ZIndex).ToList();
        parentWindow.HandleFlyoutStatusChange (flyout, visibleFlyouts);
    }

    private Flyout GetFlyout
        (
            object? item
        )
    {
        var flyout = item as Flyout;
        if (flyout != null)
        {
            return flyout;
        }

        return (Flyout)item;

        //int index = this.ItemContainerGenerator.IndexFromContainer(DefaultPanel);

        //return (Flyout)this.ItemContainerGenerator.ContainerFromIndex(index);
    }

    internal IEnumerable<Flyout> GetFlyouts()
    {
        return GetFlyouts (Items);
    }

    private IEnumerable<Flyout> GetFlyouts (IEnumerable items)
    {
        return from object item in items select GetFlyout (item);
    }

    private void ReorderZIndices (Flyout lastChanged)
    {
        var openFlyouts = GetFlyouts (Items).Where (i => i.IsOpen && i != lastChanged).OrderBy (x => x.ZIndex);
        var index = 0;
        foreach (var openFlyout in openFlyouts)
        {
            openFlyout.ZIndex = index;

            //Panel.SetZIndex(openFlyout, index);
            index++;
        }

        if (lastChanged.IsOpen)
        {
            lastChanged.IsVisible = true;

            lastChanged.ZIndex = index;

            //Panel.SetZIndex(lastChanged, index);
        }
    }
}
