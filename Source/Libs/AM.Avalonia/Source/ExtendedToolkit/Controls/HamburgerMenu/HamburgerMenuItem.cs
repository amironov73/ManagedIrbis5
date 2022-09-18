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
using System.Windows.Input;

using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Media;
using Avalonia.Media.Imaging;

#endregion

#nullable enable

namespace Avalonia.ExtendedToolkit.Controls;

/// <summary>
/// ported from https://github.com/alicanerdogan/HamburgerMenu
/// </summary>
public class HamburgerMenuItem : ListBoxItem
{
    /// <summary>
    /// style key of this control
    /// </summary>
    public Type StyleKey => typeof (HamburgerMenuItem);

    private const string ImageCtrlName = "menuImage";
    private Image? _imageCtl;
    private string _lastSelectedBaseColorScheme;

    /// <summary>
    /// get/set Text
    /// </summary>
    public string Text
    {
        get => GetValue (TextProperty);
        set => SetValue (TextProperty, value);
    }

    /// <summary>
    /// <see cref="Text"/>
    /// </summary>
    public static readonly StyledProperty<string> TextProperty =
        AvaloniaProperty.Register<HamburgerMenuItem, string> (nameof (Text));

    /// <summary>
    /// get/set IconLight
    /// </summary>
    public IBitmap IconLight
    {
        get => GetValue (IconLightProperty);
        set => SetValue (IconLightProperty, value);
    }

    /// <summary>
    /// <see cref="IconLight"/>
    /// </summary>
    public static readonly StyledProperty<IBitmap> IconLightProperty =
        AvaloniaProperty.Register<HamburgerMenuItem, IBitmap> (nameof (IconLight));

    /// <summary>
    /// get/set IconBlack
    /// </summary>
    public IBitmap IconBlack
    {
        get => GetValue (IconBlackProperty);
        set => SetValue (IconBlackProperty, value);
    }

    /// <summary>
    /// <see cref="IconBlack"/>
    /// </summary>
    public static readonly StyledProperty<IBitmap> IconBlackProperty =
        AvaloniaProperty.Register<HamburgerMenuItem, IBitmap> (nameof (IconBlack));

    /// <summary>
    /// get/set SelectionIndicatorColor
    /// </summary>
    public IBrush SelectionIndicatorColor
    {
        get => GetValue (SelectionIndicatorColorProperty);
        set => SetValue (SelectionIndicatorColorProperty, value);
    }

    /// <summary>
    /// <see cref="SelectionIndicatorColor"/>
    /// </summary>
    public static readonly StyledProperty<IBrush> SelectionIndicatorColorProperty =
        AvaloniaProperty.Register<HamburgerMenuItem, IBrush> (nameof (SelectionIndicatorColor));

    /// <summary>
    /// get/sets SelectionCommand
    /// </summary>
    public ICommand SelectionCommand
    {
        get => GetValue (SelectionCommandProperty);
        set => SetValue (SelectionCommandProperty, value);
    }

    /// <summary>
    /// <see cref="SelectionCommand"/>
    /// </summary>
    public static readonly StyledProperty<ICommand> SelectionCommandProperty =
        AvaloniaProperty.Register<HamburgerMenuItem, ICommand> (nameof (SelectionCommand));

    /// <summary>
    /// registered on ThemeManager.Instance.IsThemeChanged
    /// </summary>
    public HamburgerMenuItem()
    {
        ThemeManager.Instance.IsThemeChanged += OnThemeChanged;
    }

    private void OnThemeChanged (object sender, OnThemeChangedEventArgs e)
    {
        UpdateMenuIcon();
    }

    /// <inheritdoc cref="TemplatedControl.OnApplyTemplate"/>
    protected override void OnApplyTemplate
        (
            TemplateAppliedEventArgs eventArgs
        )
    {
        base.OnApplyTemplate (eventArgs);
        _imageCtl = eventArgs.NameScope.Find<Image> (ImageCtrlName);
        var _button = eventArgs.NameScope.Find<Button> ("ListBoxItemButton");
        _button.Tapped += (o, f) => { IsSelected = true; };

        UpdateMenuIcon();
    }

    /// <summary>
    /// updates the icon for black/white theme
    /// </summary>
    private void UpdateMenuIcon()
    {
        if (_imageCtl == null)
        {
            return;
        }

        if (_lastSelectedBaseColorScheme == ThemeManager.Instance?.SelectedTheme.BaseColorScheme)
        {
            return;
        }

        if (ThemeManager.Instance?.SelectedTheme.BaseColorScheme == ThemeManager.BaseColorDark)
        {
            _imageCtl.Source = IconBlack;
        }
        else
        {
            _imageCtl.Source = IconLight;
        }

        _lastSelectedBaseColorScheme = ThemeManager.Instance?.SelectedTheme.BaseColorScheme;
    }
}
