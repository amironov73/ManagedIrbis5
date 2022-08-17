// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable RedundantNameQualifier

/* ToolStripTrackBar.cs -- TrackBar, хостящийся в ToolStrip
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;

#endregion

#nullable enable

namespace AM.Windows.Forms;

/// <summary>
/// <see cref="T:System.Windows.Forms.TrackBar"/>, хостящийся в
/// <see cref="T:System.Windows.Forms.ToolStrip"/>.
/// </summary>
[System.ComponentModel.DesignerCategory ("Code")]
[ToolStripItemDesignerAvailability
    (ToolStripItemDesignerAvailability.ToolStrip | ToolStripItemDesignerAvailability.StatusStrip)]
public class ToolStripTrackBar
    : ToolStripControlHost
{
    #region Nested classes

    /// <summary>
    /// Версия <see cref="TrackBar"/>, поддерживающая прозрачный фон.
    /// </summary>
    internal sealed class TrackBarWithTransparentBackground
        : TrackBar
    {
        public TrackBarWithTransparentBackground()
        {
            SetStyle (ControlStyles.SupportsTransparentBackColor, true);
        }
    }

    #endregion

    #region Properties

    /// <summary>
    /// Собственно <see cref="TrackBar"/>.
    /// </summary>
    [DesignerSerializationVisibility (DesignerSerializationVisibility.Content)]
    public TrackBar TrackBar => (TrackBar) Control;

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public ToolStripTrackBar()
        : base (new TrackBarWithTransparentBackground())
    {
        TrackBar.BackColor = Color.Transparent;
        TrackBar.BackColor = SystemColors.InactiveCaption;
        TrackBar.AutoSize = false;
        AutoSize = false;
    }

    #endregion
}
