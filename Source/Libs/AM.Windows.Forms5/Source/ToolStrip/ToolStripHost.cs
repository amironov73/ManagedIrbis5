// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable VirtualMemberCallInConstructor

/* ToolStripHost.cs -- обертка над произвольным контролом, позволяющая изобразить тулстрип
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms;

/// <summary>
/// Обертка над произвольным контролом, позволяющая изобразить полосу тулстрип.
/// </summary>
public class ToolStripHost
    : ToolStrip
{
    #region Properties

    /// <summary>
    /// Обернутый контрол.
    /// </summary>
    public Control Control { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public ToolStripHost
        (
            Control control,
            int height = 100
        )
    {
        Sure.NotNull (control);
        Sure.Positive (height);

        Control = control;
        Stretch = true;
        LayoutStyle = ToolStripLayoutStyle.Table;

        var controlHost = new ToolStripControlHost (control)
        {
            Dock = DockStyle.Fill,
            AutoSize = false,
            Height = height
        };

        var layout = (TableLayoutSettings) LayoutSettings;
        layout.ColumnCount = 1;
        layout.RowCount = 1;

        Items.Add (controlHost);
    }

    #endregion
}
