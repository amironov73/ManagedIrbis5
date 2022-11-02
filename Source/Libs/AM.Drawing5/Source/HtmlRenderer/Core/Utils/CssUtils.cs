// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming

/* CssUtils.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM.Drawing.HtmlRenderer.Adapters;
using AM.Drawing.HtmlRenderer.Adapters.Entities;
using AM.Drawing.HtmlRenderer.Core.Dom;
using AM.Drawing.HtmlRenderer.Core.Parse;

#endregion

#nullable enable

namespace AM.Drawing.HtmlRenderer.Core.Utils;

/// <summary>
/// Utility method for handling CSS stuff.
/// </summary>
internal static class CssUtils
{
    #region Fields and Consts

    #endregion


    /// <summary>
    /// Brush for selection background
    /// </summary>
    public static RColor DefaultSelectionBackcolor { get; } = RColor.FromArgb (0xa9, 0x33, 0x99, 0xFF);

    /// <summary>
    /// Gets the white space width of the specified box
    /// </summary>
    /// <param name="graphics"></param>
    /// <param name="box"></param>
    /// <returns></returns>
    public static double WhiteSpace
        (
            RGraphics graphics,
            CssBoxProperties box
        )
    {
        var w = box.ActualFont.GetWhitespaceWidth (graphics);
        if (!(string.IsNullOrEmpty (box.WordSpacing) || box.WordSpacing == CssConstants.Normal))
        {
            w += CssValueParser.ParseLength (box.WordSpacing, 0, box, true);
        }

        return w;
    }

    /// <summary>
    /// Get CSS box property value by the CSS name.<br/>
    /// Used as a mapping between CSS property and the class property.
    /// </summary>
    /// <param name="cssBox">the CSS box to get it's property value</param>
    /// <param name="propName">the name of the CSS property</param>
    /// <returns>the value of the property, null if no such property exists</returns>
    public static string? GetPropertyValue
        (
            CssBox cssBox,
            string propName
        )
    {
        return propName switch
        {
            "border-bottom-width" => cssBox.BorderBottomWidth,
            "border-left-width" => cssBox.BorderLeftWidth,
            "border-right-width" => cssBox.BorderRightWidth,
            "border-top-width" => cssBox.BorderTopWidth,
            "border-bottom-style" => cssBox.BorderBottomStyle,
            "border-left-style" => cssBox.BorderLeftStyle,
            "border-right-style" => cssBox.BorderRightStyle,
            "border-top-style" => cssBox.BorderTopStyle,
            "border-bottom-color" => cssBox.BorderBottomColor,
            "border-left-color" => cssBox.BorderLeftColor,
            "border-right-color" => cssBox.BorderRightColor,
            "border-top-color" => cssBox.BorderTopColor,
            "border-spacing" => cssBox.BorderSpacing,
            "border-collapse" => cssBox.BorderCollapse,
            "corner-radius" => cssBox.CornerRadius,
            "corner-nw-radius" => cssBox.CornerNwRadius,
            "corner-ne-radius" => cssBox.CornerNeRadius,
            "corner-se-radius" => cssBox.CornerSeRadius,
            "corner-sw-radius" => cssBox.CornerSwRadius,
            "margin-bottom" => cssBox.MarginBottom,
            "margin-left" => cssBox.MarginLeft,
            "margin-right" => cssBox.MarginRight,
            "margin-top" => cssBox.MarginTop,
            "padding-bottom" => cssBox.PaddingBottom,
            "padding-left" => cssBox.PaddingLeft,
            "padding-right" => cssBox.PaddingRight,
            "padding-top" => cssBox.PaddingTop,
            "page-break-inside" => cssBox.PageBreakInside,
            "left" => cssBox.Left,
            "top" => cssBox.Top,
            "width" => cssBox.Width,
            "max-width" => cssBox.MaxWidth,
            "height" => cssBox.Height,
            "background-color" => cssBox.BackgroundColor,
            "background-image" => cssBox.BackgroundImage,
            "background-position" => cssBox.BackgroundPosition,
            "background-repeat" => cssBox.BackgroundRepeat,
            "background-gradient" => cssBox.BackgroundGradient,
            "background-gradient-angle" => cssBox.BackgroundGradientAngle,
            "content" => cssBox.Content,
            "color" => cssBox.Color,
            "display" => cssBox.Display,
            "direction" => cssBox.Direction,
            "empty-cells" => cssBox.EmptyCells,
            "float" => cssBox.Float,
            "position" => cssBox.Position,
            "line-height" => cssBox.LineHeight,
            "vertical-align" => cssBox.VerticalAlign,
            "text-indent" => cssBox.TextIndent,
            "text-align" => cssBox.TextAlign,
            "text-decoration" => cssBox.TextDecoration,
            "white-space" => cssBox.WhiteSpace,
            "word-break" => cssBox.WordBreak,
            "visibility" => cssBox.Visibility,
            "word-spacing" => cssBox.WordSpacing,
            "font-family" => cssBox.FontFamily,
            "font-size" => cssBox.FontSize,
            "font-style" => cssBox.FontStyle,
            "font-variant" => cssBox.FontVariant,
            "font-weight" => cssBox.FontWeight,
            "list-style" => cssBox.ListStyle,
            "list-style-position" => cssBox.ListStylePosition,
            "list-style-image" => cssBox.ListStyleImage,
            "list-style-type" => cssBox.ListStyleType,
            "overflow" => cssBox.Overflow,
            _ => null
        };
    }

    /// <summary>
    /// Set CSS box property value by the CSS name.<br/>
    /// Used as a mapping between CSS property and the class property.
    /// </summary>
    /// <param name="cssBox">the CSS box to set it's property value</param>
    /// <param name="propName">the name of the CSS property</param>
    /// <param name="value">the value to set</param>
    public static void SetPropertyValue
        (
            CssBox cssBox,
            string propName,
            string value
        )
    {
        switch (propName)
        {
            case "border-bottom-width":
                cssBox.BorderBottomWidth = value;
                break;

            case "border-left-width":
                cssBox.BorderLeftWidth = value;
                break;

            case "border-right-width":
                cssBox.BorderRightWidth = value;
                break;

            case "border-top-width":
                cssBox.BorderTopWidth = value;
                break;

            case "border-bottom-style":
                cssBox.BorderBottomStyle = value;
                break;

            case "border-left-style":
                cssBox.BorderLeftStyle = value;
                break;

            case "border-right-style":
                cssBox.BorderRightStyle = value;
                break;

            case "border-top-style":
                cssBox.BorderTopStyle = value;
                break;

            case "border-bottom-color":
                cssBox.BorderBottomColor = value;
                break;

            case "border-left-color":
                cssBox.BorderLeftColor = value;
                break;

            case "border-right-color":
                cssBox.BorderRightColor = value;
                break;

            case "border-top-color":
                cssBox.BorderTopColor = value;
                break;

            case "border-spacing":
                cssBox.BorderSpacing = value;
                break;

            case "border-collapse":
                cssBox.BorderCollapse = value;
                break;

            case "corner-radius":
                cssBox.CornerRadius = value;
                break;

            case "corner-nw-radius":
                cssBox.CornerNwRadius = value;
                break;

            case "corner-ne-radius":
                cssBox.CornerNeRadius = value;
                break;

            case "corner-se-radius":
                cssBox.CornerSeRadius = value;
                break;

            case "corner-sw-radius":
                cssBox.CornerSwRadius = value;
                break;

            case "margin-bottom":
                cssBox.MarginBottom = value;
                break;

            case "margin-left":
                cssBox.MarginLeft = value;
                break;

            case "margin-right":
                cssBox.MarginRight = value;
                break;

            case "margin-top":
                cssBox.MarginTop = value;
                break;

            case "padding-bottom":
                cssBox.PaddingBottom = value;
                break;

            case "padding-left":
                cssBox.PaddingLeft = value;
                break;

            case "padding-right":
                cssBox.PaddingRight = value;
                break;

            case "padding-top":
                cssBox.PaddingTop = value;
                break;

            case "page-break-inside":
                cssBox.PageBreakInside = value;
                break;

            case "left":
                cssBox.Left = value;
                break;

            case "top":
                cssBox.Top = value;
                break;

            case "width":
                cssBox.Width = value;
                break;

            case "max-width":
                cssBox.MaxWidth = value;
                break;

            case "height":
                cssBox.Height = value;
                break;

            case "background-color":
                cssBox.BackgroundColor = value;
                break;

            case "background-image":
                cssBox.BackgroundImage = value;
                break;

            case "background-position":
                cssBox.BackgroundPosition = value;
                break;

            case "background-repeat":
                cssBox.BackgroundRepeat = value;
                break;

            case "background-gradient":
                cssBox.BackgroundGradient = value;
                break;

            case "background-gradient-angle":
                cssBox.BackgroundGradientAngle = value;
                break;

            case "color":
                cssBox.Color = value;
                break;

            case "content":
                cssBox.Content = value;
                break;

            case "display":
                cssBox.Display = value;
                break;

            case "direction":
                cssBox.Direction = value;
                break;

            case "empty-cells":
                cssBox.EmptyCells = value;
                break;

            case "float":
                cssBox.Float = value;
                break;

            case "position":
                cssBox.Position = value;
                break;

            case "line-height":
                cssBox.LineHeight = value;
                break;

            case "vertical-align":
                cssBox.VerticalAlign = value;
                break;

            case "text-indent":
                cssBox.TextIndent = value;
                break;

            case "text-align":
                cssBox.TextAlign = value;
                break;

            case "text-decoration":
                cssBox.TextDecoration = value;
                break;

            case "white-space":
                cssBox.WhiteSpace = value;
                break;

            case "word-break":
                cssBox.WordBreak = value;
                break;

            case "visibility":
                cssBox.Visibility = value;
                break;

            case "word-spacing":
                cssBox.WordSpacing = value;
                break;

            case "font-family":
                cssBox.FontFamily = value;
                break;

            case "font-size":
                cssBox.FontSize = value;
                break;

            case "font-style":
                cssBox.FontStyle = value;
                break;

            case "font-variant":
                cssBox.FontVariant = value;
                break;

            case "font-weight":
                cssBox.FontWeight = value;
                break;

            case "list-style":
                cssBox.ListStyle = value;
                break;

            case "list-style-position":
                cssBox.ListStylePosition = value;
                break;

            case "list-style-image":
                cssBox.ListStyleImage = value;
                break;

            case "list-style-type":
                cssBox.ListStyleType = value;
                break;

            case "overflow":
                cssBox.Overflow = value;
                break;
        }
    }
}
