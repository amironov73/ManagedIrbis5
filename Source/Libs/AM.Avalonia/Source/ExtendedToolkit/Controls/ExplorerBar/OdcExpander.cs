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
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;

#endregion

#nullable enable

namespace Avalonia.ExtendedToolkit.Controls;

//ported from https://github.com/jogibear9988/OdysseyWPF.git

/// <summary>
/// an arcodion like control
/// </summary>
public partial class OdcExpander
    : HeaderedContentControl
{
    static OdcExpander()
    {
        MarginProperty.OverrideDefaultValue<OdcExpander> (new Thickness (10, 10, 10, 2));
        FocusableProperty.OverrideDefaultValue<OdcExpander> (false);

        IsMinimizedProperty.Changed.AddClassHandler<OdcExpander> ((o, e) => IsMinimizedChanged (o, e));
        IsExpandedProperty.Changed.AddClassHandler<OdcExpander> ((o, e) => IsExpandedChanged (o, e));
        PressedHeaderBackgroundProperty.Changed.AddClassHandler<OdcExpander> ((o, e) =>
            PressedHeaderBackgroundPropertyChangedCallback (o, e));

        HeaderClassesProperty.Changed.AddClassHandler<OdcExpander> ((o, e) => HeaderClassesChanged (o, e));
    }

    private static void HeaderClassesChanged (OdcExpander o, AvaloniaPropertyChangedEventArgs e)
    {
        if (e.NewValue is Classes && o._header != null)
        {
            var classes = e.NewValue as Classes;

            foreach (var item in classes)
            {
                if (o._header.Classes.Contains (item) == false)
                {
                    o._header.Classes.Add (item);
                }
            }
        }
    }

    /// <summary>
    /// registered PointerPressed, PointerReleased
    /// for setting IsPressed state
    /// </summary>
    public OdcExpander()
    {
        PointerPressed += (o, e) => { IsPressed = true; };
        PointerReleased += (o, e) => { IsPressed = false; };
    }

    private static void PressedHeaderBackgroundPropertyChangedCallback (OdcExpander expander,
        AvaloniaPropertyChangedEventArgs e)
    {
        expander.HasPressedBackground = e.NewValue != null;
    }

    private static void IsExpandedChanged (OdcExpander expander, AvaloniaPropertyChangedEventArgs e)
    {
        var args = new RoutedEventArgs ((bool)e.NewValue ? ExpandedEvent : CollapsedEvent);
        expander.RaiseEvent (args);
    }

    private static void IsMinimizedChanged
        (
            OdcExpander expander,
            AvaloniaPropertyChangedEventArgs eventArgs
        )
    {
        var minimized = (bool) eventArgs.NewValue;

        expander.IsEnabled = !minimized;
        var args = new RoutedEventArgs (minimized ? MinimizedEvent : MaximizedEvent);
        expander.RaiseEvent (args);
    }

    /// <inheritdoc cref="TemplatedControl.OnApplyTemplate"/>
    protected override void OnApplyTemplate
        (
            TemplateAppliedEventArgs eventArgs
        )
    {
        _header = eventArgs.NameScope.Find<OdcExpanderHeader> ("PART_HEADER");

        //ExpanderHeaderHight = _header.Height;
        //ExpanderHeaderWidth = _header.Width;

        base.OnApplyTemplate (eventArgs);

        RaisePropertyChanged (HeaderClassesProperty, null, HeaderClasses);
    }
}
