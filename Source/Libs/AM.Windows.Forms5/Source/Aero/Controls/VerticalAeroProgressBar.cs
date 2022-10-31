// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* VerticalAeroProgressBar.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#endregion

#nullable enable

namespace AeroSuite.Controls;

/// <summary>
/// A vertical progress bar control with extended features.
/// </summary>
/// <remarks>
/// This progress bar is a vertically displayed version of the <see cref="AeroProgressBar"/>.
/// </remarks>
[DesignerCategory ("Code")]
[DisplayName ("Vertical Aero ProgressBar")]
[Description ("A vertical progress bar control with extended features.")]
[ToolboxItem (true)]
[ToolboxBitmap (typeof (VerticalAeroProgressBar))]
public class VerticalAeroProgressBar
    : AeroProgressBar
{
    private const int PBS_VERTICAL = 0x4;

    /// <summary>
    /// Initializes a new instance of the <see cref="VerticalAeroProgressBar"/> class.
    /// </summary>
    public VerticalAeroProgressBar()
    {
        Size = new Size (Height, Width);
    }

    /// <inheritdoc cref="AeroProgressBar.CreateParams"/>
    protected override CreateParams CreateParams
    {
        get
        {
            var param = base.CreateParams;
            param.Style |= PBS_VERTICAL;
            return param;
        }
    }
}
