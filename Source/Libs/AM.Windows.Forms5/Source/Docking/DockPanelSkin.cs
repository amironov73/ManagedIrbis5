// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global

/* DockPanelSkin.cs -- the skin to use when displaying DockPanel
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;

#endregion

#nullable enable

namespace AM.Windows.Forms.Docking;

#region DockPanelSkin classes

/// <summary>
/// The skin to use when displaying the DockPanel.
/// The skin allows custom gradient color schemes to be used when drawing the
/// DockStrips and Tabs.
/// </summary>
[TypeConverter (typeof (DockPanelSkinConverter))]
public class DockPanelSkin
{
    /// <summary>
    /// The skin used to display the auto hide strips and tabs.
    /// </summary>
    public AutoHideStripSkin AutoHideStripSkin { get; set; } = new ();

    /// <summary>
    /// The skin used to display the Document and ToolWindow style DockStrips and Tabs.
    /// </summary>
    public DockPaneStripSkin DockPaneStripSkin { get; set; } = new ();
}

/// <summary>
/// The skin used to display the auto hide strip and tabs.
/// </summary>
[TypeConverter (typeof (AutoHideStripConverter))]
public class AutoHideStripSkin
{
    /// <summary>
    /// The gradient color skin for the DockStrips.
    /// </summary>
    public DockPanelGradient DockStripGradient { get; set; } = new ();

    /// <summary>
    /// The gradient color skin for the Tabs.
    /// </summary>
    public TabGradient TabGradient { get; set; } = new ();

    /// <summary>
    /// The gradient color skin for the Tabs.
    /// </summary>
    public DockStripBackground DockStripBackground { get; set; } = new ();

    /// <summary>
    /// Font used in AutoHideStrip elements.
    /// </summary>
    [DefaultValue (typeof (SystemFonts), "MenuFont")]
    public Font? TextFont { get; set; } = SystemFonts.MenuFont;
}

/// <summary>
/// The skin used to display the document and tool strips and tabs.
/// </summary>
[TypeConverter (typeof (DockPaneStripConverter))]
public class DockPaneStripSkin
{
    /// <summary>
    /// The skin used to display the Document style DockPane strip and tab.
    /// </summary>
    public DockPaneStripGradient DocumentGradient { get; set; } = new ();

    /// <summary>
    /// The skin used to display the ToolWindow style DockPane strip and tab.
    /// </summary>
    public DockPaneStripToolWindowGradient ToolWindowGradient { get; set; } = new ();

    /// <summary>
    /// Font used in DockPaneStrip elements.
    /// </summary>
    [DefaultValue (typeof (SystemFonts), "MenuFont")]
    public Font? TextFont { get; set; } = SystemFonts.MenuFont;
}

/// <summary>
/// The skin used to display the DockPane ToolWindow strip and tab.
/// </summary>
[TypeConverter (typeof (DockPaneStripGradientConverter))]
public class DockPaneStripToolWindowGradient : DockPaneStripGradient
{
    /// <summary>
    /// The skin used to display the active ToolWindow caption.
    /// </summary>
    public TabGradient ActiveCaptionGradient { get; set; } = new ();

    /// <summary>
    /// The skin used to display the inactive ToolWindow caption.
    /// </summary>
    public TabGradient InactiveCaptionGradient { get; set; } = new ();
}

/// <summary>
/// The skin used to display the DockPane strip and tab.
/// </summary>
[TypeConverter (typeof (DockPaneStripGradientConverter))]
public class DockPaneStripGradient
{
    /// <summary>
    /// The gradient color skin for the DockStrip.
    /// </summary>
    public DockPanelGradient DockStripGradient { get; set; } = new ();

    /// <summary>
    /// The skin used to display the active DockPane tabs.
    /// </summary>
    public TabGradient ActiveTabGradient { get; set; } = new ();

    /// <summary>
    /// The skin used to display the hover DockPane tabs.
    /// </summary>
    public TabGradient HoverTabGradient { get; set; } = new ();

    /// <summary>
    /// The skin used to display the inactive DockPane tabs.
    /// </summary>
    public TabGradient InactiveTabGradient { get; set; } = new ();
}

/// <summary>
/// The skin used to display the dock pane tab
/// </summary>
[TypeConverter (typeof (DockPaneTabGradientConverter))]
public class TabGradient : DockPanelGradient
{
    /// <summary>
    /// The text color.
    /// </summary>
    [DefaultValue (typeof (SystemColors), "ControlText")]
    public Color TextColor { get; set; } = SystemColors.ControlText;
}

/// <summary>
/// The skin used to display the dock pane tab
/// </summary>
[TypeConverter (typeof (DockPaneTabGradientConverter))]
public class DockStripBackground
{
    //private LinearGradientMode m_linearGradientMode = LinearGradientMode.Horizontal;

    /// <summary>
    /// The beginning gradient color.
    /// </summary>
    [DefaultValue (typeof (SystemColors), "Control")]
    public Color StartColor { get; set; } = SystemColors.Control;

    /// <summary>
    /// The ending gradient color.
    /// </summary>
    [DefaultValue (typeof (SystemColors), "Control")]
    public Color EndColor { get; set; } = SystemColors.Control;
}


/// <summary>
/// The gradient color skin.
/// </summary>
[TypeConverter (typeof (DockPanelGradientConverter))]
public class DockPanelGradient
{
    /// <summary>
    /// The beginning gradient color.
    /// </summary>
    [DefaultValue (typeof (SystemColors), "Control")]
    public Color StartColor { get; set; } = SystemColors.Control;

    /// <summary>
    /// The ending gradient color.
    /// </summary>
    [DefaultValue (typeof (SystemColors), "Control")]
    public Color EndColor { get; set; } = SystemColors.Control;

    /// <summary>
    /// The gradient mode to display the colors.
    /// </summary>
    [DefaultValue (LinearGradientMode.Horizontal)]
    public LinearGradientMode LinearGradientMode { get; set; } = LinearGradientMode.Horizontal;
}

#endregion

#region Converters

/// <summary>
///
/// </summary>
public class DockPanelSkinConverter
    : ExpandableObjectConverter
{
    /// <inheritdoc cref="TypeConverter.CanConvertTo(System.ComponentModel.ITypeDescriptorContext?,System.Type?)"/>
    public override bool CanConvertTo
        (
            ITypeDescriptorContext? context,
            Type? destinationType
        )
    {
        return destinationType == typeof (DockPanelSkin)
               || base.CanConvertTo (context, destinationType);
    }

    /// <inheritdoc cref="TypeConverter.ConvertTo(System.ComponentModel.ITypeDescriptorContext?,System.Globalization.CultureInfo?,object?,System.Type)"/>
    public override object? ConvertTo
        (
            ITypeDescriptorContext? context,
            System.Globalization.CultureInfo? culture,
            object? value,
            Type destinationType
        )
    {
        if (destinationType == typeof (string) && value is DockPanelSkin)
        {
            return "DockPanelSkin";
        }

        return base.ConvertTo (context, culture, value, destinationType);
    }
}

/// <summary>
///
/// </summary>
public class DockPanelGradientConverter
    : ExpandableObjectConverter
{
    /// <inheritdoc cref="TypeConverter.CanConvertTo(System.ComponentModel.ITypeDescriptorContext?,System.Type?)"/>
    public override bool CanConvertTo
        (
            ITypeDescriptorContext? context,
            Type? destinationType
        )
    {
        return destinationType == typeof (DockPanelGradient)
               || base.CanConvertTo (context, destinationType);
    }

    /// <inheritdoc cref="TypeConverter.ConvertTo(System.ComponentModel.ITypeDescriptorContext?,System.Globalization.CultureInfo?,object?,System.Type)"/>
    public override object? ConvertTo
        (
            ITypeDescriptorContext? context,
            System.Globalization.CultureInfo? culture,
            object? value,
            Type destinationType
        )
    {
        if (destinationType == typeof (string) && value is DockPanelGradient)
        {
            return "DockPanelGradient";
        }

        return base.ConvertTo (context, culture, value, destinationType);
    }
}

/// <summary>
///
/// </summary>
public class AutoHideStripConverter
    : ExpandableObjectConverter
{
    /// <inheritdoc cref="TypeConverter.CanConvertTo(System.ComponentModel.ITypeDescriptorContext?,System.Type?)"/>
    public override bool CanConvertTo
        (
            ITypeDescriptorContext? context,
            Type? destinationType
        )
    {
        return destinationType == typeof (AutoHideStripSkin)
               || base.CanConvertTo (context, destinationType);
    }

    /// <inheritdoc cref="TypeConverter.ConvertTo(System.ComponentModel.ITypeDescriptorContext?,System.Globalization.CultureInfo?,object?,System.Type)"/>
    public override object? ConvertTo
        (
            ITypeDescriptorContext? context,
            System.Globalization.CultureInfo? culture,
            object? value,
            Type destinationType
        )
    {
        if (destinationType == typeof (String) && value is AutoHideStripSkin)
        {
            return "AutoHideStripSkin";
        }

        return base.ConvertTo (context, culture, value, destinationType);
    }
}

/// <summary>
///
/// </summary>
public class DockPaneStripConverter
    : ExpandableObjectConverter
{
    /// <inheritdoc cref="TypeConverter.CanConvertTo(System.ComponentModel.ITypeDescriptorContext?,System.Type?)"/>
    public override bool CanConvertTo
        (
            ITypeDescriptorContext? context,
            Type? destinationType
        )
    {
        if (destinationType == typeof (DockPaneStripSkin))
        {
            return true;
        }

        return base.CanConvertTo (context, destinationType);
    }

    /// <inheritdoc cref="TypeConverter.ConvertTo(System.ComponentModel.ITypeDescriptorContext?,System.Globalization.CultureInfo?,object?,System.Type)"/>
    public override object? ConvertTo
        (
            ITypeDescriptorContext? context,
            System.Globalization.CultureInfo? culture,
            object? value,
            Type destinationType
        )
    {
        if (destinationType == typeof (String) && value is DockPaneStripSkin)
        {
            return "DockPaneStripSkin";
        }

        return base.ConvertTo (context, culture, value, destinationType);
    }
}

/// <summary>
///
/// </summary>
public sealed class DockPaneStripGradientConverter
    : ExpandableObjectConverter
{
    /// <inheritdoc cref="TypeConverter.CanConvertTo(System.ComponentModel.ITypeDescriptorContext?,System.Type?)"/>
    public override bool CanConvertTo
        (
            ITypeDescriptorContext? context,
            Type? destinationType
        )
    {
        return destinationType == typeof (DockPaneStripGradient)
               || base.CanConvertTo (context, destinationType);
    }

    /// <inheritdoc cref="TypeConverter.ConvertTo(System.ComponentModel.ITypeDescriptorContext?,System.Globalization.CultureInfo?,object?,System.Type)"/>
    public override object? ConvertTo
        (
            ITypeDescriptorContext? context,
            System.Globalization.CultureInfo? culture,
            object? value,
            Type destinationType
        )
    {
        if (destinationType == typeof (String) && value is DockPaneStripGradient)
        {
            return "DockPaneStripGradient";
        }

        return base.ConvertTo (context, culture, value, destinationType);
    }
}

/// <summary>
///
/// </summary>
public class DockPaneTabGradientConverter
    : ExpandableObjectConverter
{
    /// <inheritdoc cref="TypeConverter.CanConvertTo(System.ComponentModel.ITypeDescriptorContext?,System.Type?)"/>
    public override bool CanConvertTo
        (
            ITypeDescriptorContext? context,
            Type? destinationType
        )
    {
        return destinationType == typeof (TabGradient)
               || base.CanConvertTo (context, destinationType);
    }

    /// <inheritdoc cref="TypeConverter.ConvertTo(System.ComponentModel.ITypeDescriptorContext?,System.Globalization.CultureInfo?,object?,System.Type)"/>
    public override object? ConvertTo
        (
            ITypeDescriptorContext? context,
            System.Globalization.CultureInfo? culture,
            object? value,
            Type destinationType
        )
    {
        if (destinationType == typeof (string) && value is TabGradient)
        {
            return "DockPaneTabGradient";
        }

        return base.ConvertTo (context, culture, value, destinationType);
    }
}

#endregion
