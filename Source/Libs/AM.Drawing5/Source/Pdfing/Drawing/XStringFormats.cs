// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* XStringFormats.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace PdfSharpCore.Drawing;

/// <summary>
/// Represents predefined text layouts.
/// </summary>
public static class XStringFormats
{
    /// <summary>
    /// Gets a new XStringFormat object that aligns the text left on the base line.
    /// This is the same as BaseLineLeft.
    /// </summary>
    public static XStringFormat Default => BaseLineLeft;

    /// <summary>
    /// Gets a new XStringFormat object that aligns the text left on the base line.
    /// This is the same as Default.
    /// </summary>
    public static XStringFormat BaseLineLeft
    {
        get
        {
            // Create new format to allow changes.
            var format = new XStringFormat
            {
                Alignment = XStringAlignment.Near,
                LineAlignment = XLineAlignment.BaseLine
            };
            return format;
        }
    }

    /// <summary>
    /// Gets a new XStringFormat object that aligns the text top left of the layout rectangle.
    /// </summary>
    public static XStringFormat TopLeft
    {
        get
        {
            // Create new format to allow changes.
            var format = new XStringFormat
            {
                Alignment = XStringAlignment.Near,
                LineAlignment = XLineAlignment.Near
            };
            return format;
        }
    }

    /// <summary>
    /// Gets a new XStringFormat object that aligns the text center left of the layout rectangle.
    /// </summary>
    public static XStringFormat CenterLeft
    {
        get
        {
            // Create new format to allow changes.
            var format = new XStringFormat
            {
                Alignment = XStringAlignment.Near,
                LineAlignment = XLineAlignment.Center
            };
            return format;
        }
    }

    /// <summary>
    /// Gets a new XStringFormat object that aligns the text bottom left of the layout rectangle.
    /// </summary>
    public static XStringFormat BottomLeft
    {
        get
        {
            // Create new format to allow changes.
            var format = new XStringFormat
            {
                Alignment = XStringAlignment.Near,
                LineAlignment = XLineAlignment.Far
            };
            return format;
        }
    }

    /// <summary>
    /// Gets a new XStringFormat object that centers the text in the middle of the base line.
    /// </summary>
    public static XStringFormat BaseLineCenter
    {
        get
        {
            // Create new format to allow changes.
            var format = new XStringFormat
            {
                Alignment = XStringAlignment.Center,
                LineAlignment = XLineAlignment.BaseLine
            };
            return format;
        }
    }

    /// <summary>
    /// Gets a new XStringFormat object that centers the text at the top of the layout rectangle.
    /// </summary>
    public static XStringFormat TopCenter
    {
        get
        {
            // Create new format to allow changes.
            var format = new XStringFormat
            {
                Alignment = XStringAlignment.Center,
                LineAlignment = XLineAlignment.Near
            };
            return format;
        }
    }

    /// <summary>
    /// Gets a new XStringFormat object that centers the text in the middle of the layout rectangle.
    /// </summary>
    public static XStringFormat Center
    {
        get
        {
            // Create new format to allow changes.
            var format = new XStringFormat
            {
                Alignment = XStringAlignment.Center,
                LineAlignment = XLineAlignment.Center
            };
            return format;
        }
    }

    /// <summary>
    /// Gets a new XStringFormat object that centers the text at the bottom of the layout rectangle.
    /// </summary>
    public static XStringFormat BottomCenter
    {
        get
        {
            // Create new format to allow changes.
            var format = new XStringFormat
            {
                Alignment = XStringAlignment.Center,
                LineAlignment = XLineAlignment.Far
            };
            return format;
        }
    }

    /// <summary>
    /// Gets a new XStringFormat object that aligns the text in right on the base line.
    /// </summary>
    public static XStringFormat BaseLineRight
    {
        get
        {
            // Create new format to allow changes.
            var format = new XStringFormat
            {
                Alignment = XStringAlignment.Far,
                LineAlignment = XLineAlignment.BaseLine
            };
            return format;
        }
    }

    /// <summary>
    /// Gets a new XStringFormat object that aligns the text top right of the layout rectangle.
    /// </summary>
    public static XStringFormat TopRight
    {
        get
        {
            // Create new format to allow changes.
            var format = new XStringFormat
            {
                Alignment = XStringAlignment.Far,
                LineAlignment = XLineAlignment.Near
            };
            return format;
        }
    }

    /// <summary>
    /// Gets a new XStringFormat object that aligns the text center right of the layout rectangle.
    /// </summary>
    public static XStringFormat CenterRight
    {
        get
        {
            // Create new format to allow changes.
            var format = new XStringFormat
            {
                Alignment = XStringAlignment.Far,
                LineAlignment = XLineAlignment.Center
            };
            return format;
        }
    }

    /// <summary>
    /// Gets a new XStringFormat object that aligns the text at the bottom right of the layout rectangle.
    /// </summary>
    public static XStringFormat BottomRight
    {
        get
        {
            // Create new format to allow changes.
            var format = new XStringFormat
            {
                Alignment = XStringAlignment.Far,
                LineAlignment = XLineAlignment.Far
            };
            return format;
        }
    }
}
