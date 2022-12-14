// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global

/* DockPanelColorPalette.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Drawing;

#endregion

#nullable enable

namespace AM.Windows.Forms.Docking;

/// <summary>
///
/// </summary>
public class DockPanelColorPalette
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="factory"></param>
    public DockPanelColorPalette
        (
            IPaletteFactory factory
        )
    {
        factory.Initialize (this);
    }

    /// <summary>
    ///
    /// </summary>
    public AutoHideStripPalette AutoHideStripDefault { get; } = new ();

    /// <summary>
    ///
    /// </summary>
    public AutoHideStripPalette AutoHideStripHovered { get; } = new ();

    /// <summary>
    ///
    /// </summary>
    public ButtonPalette OverflowButtonDefault { get; } = new ();

    /// <summary>
    ///
    /// </summary>
    public HoveredButtonPalette OverflowButtonHovered { get; } = new ();

    /// <summary>
    ///
    /// </summary>
    public HoveredButtonPalette OverflowButtonPressed { get; } = new ();

    /// <summary>
    ///
    /// </summary>
    public TabPalette TabSelectedActive { get; } = new ();

    /// <summary>
    ///
    /// </summary>
    public TabPalette TabSelectedInactive { get; } = new ();

    /// <summary>
    ///
    /// </summary>
    public UnselectedTabPalette TabUnselected { get; } = new ();

    /// <summary>
    ///
    /// </summary>
    public TabPalette TabUnselectedHovered { get; } = new ();

    /// <summary>
    ///
    /// </summary>
    public HoveredButtonPalette TabButtonSelectedActiveHovered { get; } = new ();

    /// <summary>
    ///
    /// </summary>
    public HoveredButtonPalette TabButtonSelectedActivePressed { get; } = new ();

    /// <summary>
    ///
    /// </summary>
    public HoveredButtonPalette TabButtonSelectedInactiveHovered { get; } = new ();

    /// <summary>
    ///
    /// </summary>
    public HoveredButtonPalette TabButtonSelectedInactivePressed { get; } = new ();

    /// <summary>
    ///
    /// </summary>
    public HoveredButtonPalette TabButtonUnselectedTabHoveredButtonHovered { get; } = new ();

    /// <summary>
    ///
    /// </summary>
    public HoveredButtonPalette TabButtonUnselectedTabHoveredButtonPressed { get; } = new ();

    /// <summary>
    ///
    /// </summary>
    public MainWindowPalette MainWindowActive { get; } = new ();

    /// <summary>
    ///
    /// </summary>
    public MainWindowStatusBarPalette MainWindowStatusBarDefault { get; } = new ();

    /// <summary>
    ///
    /// </summary>
    public ToolWindowCaptionPalette ToolWindowCaptionActive { get; } = new ();

    /// <summary>
    ///
    /// </summary>
    public ToolWindowCaptionPalette ToolWindowCaptionInactive { get; } = new ();

    /// <summary>
    ///
    /// </summary>
    public HoveredButtonPalette ToolWindowCaptionButtonActiveHovered { get; } = new ();

    /// <summary>
    ///
    /// </summary>
    public HoveredButtonPalette ToolWindowCaptionButtonPressed { get; } = new ();

    /// <summary>
    ///
    /// </summary>
    public HoveredButtonPalette ToolWindowCaptionButtonInactiveHovered { get; } = new ();

    /// <summary>
    ///
    /// </summary>
    public ToolWindowTabPalette ToolWindowTabSelectedActive { get; } = new ();

    /// <summary>
    ///
    /// </summary>
    public ToolWindowTabPalette ToolWindowTabSelectedInactive { get; } = new ();

    /// <summary>
    ///
    /// </summary>
    public ToolWindowUnselectedTabPalette ToolWindowTabUnselected { get; } = new ();

    /// <summary>
    ///
    /// </summary>
    public ToolWindowTabPalette ToolWindowTabUnselectedHovered { get; } = new ();

    /// <summary>
    ///
    /// </summary>
    public Color ToolWindowBorder { get; set; }

    /// <summary>
    ///
    /// </summary>
    public Color ToolWindowSeparator { get; set; }

    /// <summary>
    ///
    /// </summary>
    public DockTargetPalette DockTarget { get; } = new ();

    /// <summary>
    ///
    /// </summary>
    public CommandBarMenuPalette CommandBarMenuDefault { get; } = new ();

    /// <summary>
    ///
    /// </summary>
    public CommandBarMenuPopupPalette CommandBarMenuPopupDefault { get; } = new ();

    /// <summary>
    ///
    /// </summary>
    public CommandBarMenuPopupDisabledPalette CommandBarMenuPopupDisabled { get; } = new ();

    /// <summary>
    ///
    /// </summary>
    public CommandBarMenuPopupHoveredPalette CommandBarMenuPopupHovered { get; } = new ();

    /// <summary>
    ///
    /// </summary>
    public CommandBarMenuTopLevelHeaderPalette CommandBarMenuTopLevelHeaderHovered { get; } = new ();

    /// <summary>
    ///
    /// </summary>
    public CommandBarToolbarPalette CommandBarToolbarDefault { get; } = new ();

    /// <summary>
    ///
    /// </summary>
    public CommandBarToolbarButtonCheckedPalette CommandBarToolbarButtonChecked { get; } = new ();

    /// <summary>
    ///
    /// </summary>
    public CommandBarToolbarButtonCheckedHoveredPalette CommandBarToolbarButtonCheckedHovered { get; } = new ();

    /// <summary>
    ///
    /// </summary>
    public CommandBarToolbarButtonPalette CommandBarToolbarButtonDefault { get; } = new ();

    /// <summary>
    ///
    /// </summary>
    public CommandBarToolbarButtonHoveredPalette CommandBarToolbarButtonHovered { get; } = new ();

    /// <summary>
    ///
    /// </summary>
    public CommandBarToolbarButtonPressedPalette CommandBarToolbarButtonPressed { get; } = new ();

    /// <summary>
    ///
    /// </summary>
    public CommandBarToolbarOverflowButtonPalette CommandBarToolbarOverflowHovered { get; } = new ();

    /// <summary>
    ///
    /// </summary>
    public CommandBarToolbarOverflowButtonPalette CommandBarToolbarOverflowPressed { get; } = new ();

    /// <summary>
    ///
    /// </summary>
    public VisualStudioColorTable? ColorTable { get; }
}

/// <summary>
///
/// </summary>
public class CommandBarToolbarOverflowButtonPalette
{
    /// <summary>
    ///
    /// </summary>
    public Color Background { get; set; }

    /// <summary>
    ///
    /// </summary>
    public Color Glyph { get; set; }
}

/// <summary>
///
/// </summary>
public class CommandBarToolbarButtonPressedPalette
{
    /// <summary>
    ///
    /// </summary>
    public Color Arrow { get; set; }

    /// <summary>
    ///
    /// </summary>
    public Color Background { get; set; }

    /// <summary>
    ///
    /// </summary>
    public Color Text { get; set; }
}

/// <summary>
///
/// </summary>
public class CommandBarToolbarButtonHoveredPalette
{
    /// <summary>
    ///
    /// </summary>
    public Color Arrow { get; set; }

    /// <summary>
    ///
    /// </summary>
    public Color Separator { get; set; }
}

/// <summary>
///
/// </summary>
public class CommandBarToolbarButtonPalette
{
    /// <summary>
    ///
    /// </summary>
    public Color Arrow { get; set; }
}

/// <summary>
///
/// </summary>
public class CommandBarToolbarButtonCheckedHoveredPalette
{
    /// <summary>
    ///
    /// </summary>
    public Color Border { get; set; }

    /// <summary>
    ///
    /// </summary>
    public Color Text { get; set; }
}

/// <summary>
///
/// </summary>
public class CommandBarToolbarButtonCheckedPalette
{
    /// <summary>
    ///
    /// </summary>
    public Color Background { get; set; }

    /// <summary>
    ///
    /// </summary>
    public Color Border { get; set; }

    /// <summary>
    ///
    /// </summary>
    public Color Text { get; set; }
}

/// <summary>
///
/// </summary>
public class CommandBarToolbarPalette
{
    /// <summary>
    ///
    /// </summary>
    public Color Background { get; set; }

    /// <summary>
    ///
    /// </summary>
    public Color Border { get; set; }

    /// <summary>
    ///
    /// </summary>
    public Color Grip { get; set; }

    /// <summary>
    ///
    /// </summary>
    public Color OverflowButtonBackground { get; set; }

    /// <summary>
    ///
    /// </summary>
    public Color OverflowButtonGlyph { get; set; }

    /// <summary>
    ///
    /// </summary>
    public Color Separator { get; set; }

    /// <summary>
    ///
    /// </summary>
    public Color SeparatorAccent { get; set; }

    /// <summary>
    ///
    /// </summary>
    public Color Tray { get; set; }
}

/// <summary>
///
/// </summary>
public class CommandBarMenuTopLevelHeaderPalette
{
    /// <summary>
    ///
    /// </summary>
    public Color Background { get; set; }

    /// <summary>
    ///
    /// </summary>
    public Color Border { get; set; }

    /// <summary>
    ///
    /// </summary>
    public Color Text { get; set; }
}

/// <summary>
///
/// </summary>
public class CommandBarMenuPopupHoveredPalette
{
    /// <summary>
    ///
    /// </summary>
    public Color Arrow { get; set; }

    /// <summary>
    ///
    /// </summary>
    public Color Checkmark { get; set; }

    /// <summary>
    ///
    /// </summary>
    public Color CheckmarkBackground { get; set; }

    /// <summary>
    ///
    /// </summary>
    public Color ItemBackground { get; set; }

    /// <summary>
    ///
    /// </summary>
    public Color Text { get; set; }
}

/// <summary>
///
/// </summary>
public class CommandBarMenuPopupDisabledPalette
{
    /// <summary>
    ///
    /// </summary>
    public Color Checkmark { get; set; }

    /// <summary>
    ///
    /// </summary>
    public Color CheckmarkBackground { get; set; }

    /// <summary>
    ///
    /// </summary>
    public Color Text { get; set; }
}

/// <summary>
///
/// </summary>
public class CommandBarMenuPopupPalette
{
    /// <summary>
    ///
    /// </summary>
    public Color Arrow { get; set; }

    /// <summary>
    ///
    /// </summary>
    public Color BackgroundBottom { get; set; }

    /// <summary>
    ///
    /// </summary>
    public Color BackgroundTop { get; set; }

    /// <summary>
    ///
    /// </summary>
    public Color Border { get; set; }

    /// <summary>
    ///
    /// </summary>
    public Color Checkmark { get; set; }

    /// <summary>
    ///
    /// </summary>
    public Color CheckmarkBackground { get; set; }

    /// <summary>
    ///
    /// </summary>
    public Color IconBackground { get; set; }

    /// <summary>
    ///
    /// </summary>
    public Color Separator { get; set; }
}

/// <summary>
///
/// </summary>
public class CommandBarMenuPalette
{
    /// <summary>
    ///
    /// </summary>
    public Color Background { get; set; }

    /// <summary>
    ///
    /// </summary>
    public Color Text { get; set; }
}

/// <summary>
///
/// </summary>
public interface IPaletteFactory
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="palette"></param>
    void Initialize (DockPanelColorPalette palette);
}

/// <summary>
///
/// </summary>
public class DockTargetPalette
{
    /// <summary>
    ///
    /// </summary>
    public Color Background { get; set; }

    /// <summary>
    ///
    /// </summary>
    public Color Border { get; set; }

    /// <summary>
    ///
    /// </summary>
    public Color ButtonBackground { get; set; }

    /// <summary>
    ///
    /// </summary>
    public Color ButtonBorder { get; set; }

    /// <summary>
    ///
    /// </summary>
    public Color GlyphBackground { get; set; }

    /// <summary>
    ///
    /// </summary>
    public Color GlyphArrow { get; set; }

    /// <summary>
    ///
    /// </summary>
    public Color GlyphBorder { get; set; }
}

/// <summary>
///
/// </summary>
public class HoveredButtonPalette
{
    /// <summary>
    ///
    /// </summary>
    public Color Background { get; set; }

    /// <summary>
    ///
    /// </summary>
    public Color Border { get; set; }

    /// <summary>
    ///
    /// </summary>
    public Color Glyph { get; set; }
}

/// <summary>
///
/// </summary>
public class ButtonPalette
{
    /// <summary>
    ///
    /// </summary>
    public Color Glyph { get; set; }
}

/// <summary>
///
/// </summary>
public class MainWindowPalette
{
    /// <summary>
    ///
    /// </summary>
    public Color Background { get; set; }
}

/// <summary>
///
/// </summary>
public class MainWindowStatusBarPalette
{
    /// <summary>
    ///
    /// </summary>
    public Color Background { get; set; }

    /// <summary>
    ///
    /// </summary>
    public Color Highlight { get; set; }

    /// <summary>
    ///
    /// </summary>
    public Color HighlightText { get; set; }

    /// <summary>
    ///
    /// </summary>
    public Color ResizeGrip { get; set; }

    /// <summary>
    ///
    /// </summary>
    public Color ResizeGripAccent { get; set; }

    /// <summary>
    ///
    /// </summary>
    public Color Text { get; set; }
}

/// <summary>
///
/// </summary>
public class ToolWindowTabPalette
{
    /// <summary>
    ///
    /// </summary>
    public Color Background { get; set; }

    /// <summary>
    ///
    /// </summary>
    public Color Text { get; set; }
}

/// <summary>
///
/// </summary>
public class ToolWindowUnselectedTabPalette
{
    /// <summary>
    ///
    /// </summary>
    public Color Background { get; set; }

    /// <summary>
    ///
    /// </summary>
    public Color Text { get; set; }
}

/// <summary>
///
/// </summary>
public class ToolWindowCaptionPalette
{
    /// <summary>
    ///
    /// </summary>
    public Color Background { get; set; }

    /// <summary>
    ///
    /// </summary>
    public Color Button { get; set; }

    /// <summary>
    ///
    /// </summary>
    public Color Grip { get; set; }

    /// <summary>
    ///
    /// </summary>
    public Color Text { get; set; }
}

/// <summary>
///
/// </summary>
public class TabPalette
{
    /// <summary>
    ///
    /// </summary>
    public Color Background { get; set; }

    /// <summary>
    ///
    /// </summary>
    public Color Button { get; set; }

    /// <summary>
    ///
    /// </summary>
    public Color Text { get; set; }
}

/// <summary>
///
/// </summary>
public class UnselectedTabPalette
{
    /// <summary>
    ///
    /// </summary>
    public Color Background { get; set; }

    /// <summary>
    ///
    /// </summary>
    public Color Text { get; set; }
}

/// <summary>
///
/// </summary>
public class AutoHideStripPalette
{
    /// <summary>
    ///
    /// </summary>
    public Color Background { get; set; }

    /// <summary>
    ///
    /// </summary>
    public Color Border { get; set; }

    /// <summary>
    ///
    /// </summary>
    public Color Text { get; set; }
}
