// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* TableLayoutPanelExtensions.cs -- методы расширения для TableLayoutPanel
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms.MarkupExtensions;

/// <summary>
/// Методы расширения для <see cref="TableLayoutPanel"/>.
/// </summary>
public static class TableLayoutPanelExtensions
{
    #region Public methods

    /// <summary>
    ///
    /// </summary>
    public static TTableLayoutPanel ColumnStyles<TTableLayoutPanel>
        (
            this TTableLayoutPanel panel,
            string columnStyles
        )
        where TTableLayoutPanel: TableLayoutPanel
    {
        Sure.NotNull (panel);
        Sure.NotNullNorEmpty (columnStyles);

        foreach (var style in columnStyles.Split('|'))
        {
            panel.ColumnStyles.Add
                (
                    style[^1] == '*'
                        ? new ColumnStyle { SizeType = SizeType.AutoSize, Width = 1 }
                        : style[^1] == '%'
                            ? new ColumnStyle { SizeType = SizeType.Percent, Width = float.Parse(style[..^1]) }
                            : new ColumnStyle { SizeType = SizeType.Absolute, Width = float.Parse(style) }
                );
        }

        return panel;
    }

    /// <summary>
    ///
    /// </summary>
    public static TTableLayoutPanel RowStyles<TTableLayoutPanel>
        (
            this TTableLayoutPanel tableLayoutPanel,
            string rowStyles
        )
        where TTableLayoutPanel: TableLayoutPanel
    {
        Sure.NotNullNorEmpty (rowStyles);

        foreach (var style in rowStyles.Split('|'))
        {
            tableLayoutPanel.RowStyles.Add
                (
                    style[^1] == '*'
                        ? new RowStyle { SizeType = SizeType.AutoSize, Height = 1 }
                        : style[^1] == '%'
                            ? new RowStyle { SizeType = SizeType.Percent, Height = float.Parse(style[..^1]) }
                            : new RowStyle { SizeType = SizeType.Absolute, Height = float.Parse(style) }
                );
        }

        return tableLayoutPanel;
    }

    /// <summary>
    ///
    /// </summary>
    public static TTableLayoutPanel TableControls<TTableLayoutPanel>
        (
            this TTableLayoutPanel panel,
            params TableLocation[] children
        )
        where TTableLayoutPanel: TableLayoutPanel
    {
        Sure.NotNull (panel);
        Sure.NotNull (children);

        panel.SuspendLayout();
        foreach (var child in children)
        {
            panel.Controls.Add (child.Control, child.Column, child.Row);
            if (child.HasSpans)
            {
                if (child.ColumnSpan > 1)
                {
                    panel.SetColumnSpan (child.Control, child.ColumnSpan);
                }

                if (child.RowSpan > 1)
                {
                    panel.SetRowSpan (child.Control, child.RowSpan);
                }
            }
        }

        panel.ResumeLayout();

        return panel;
    }

    /// <summary>
    ///
    /// </summary>
    public static TTableLayoutPanel TableLayout<TTableLayoutPanel>
        (
            this TTableLayoutPanel panel,
            int columnCount,
            int rowCount,
            bool autoSize = true
        )
        where TTableLayoutPanel: TableLayoutPanel
    {
        Sure.NotNull (panel);
        Sure.Positive (columnCount);
        Sure.Positive (rowCount);

        panel.ColumnCount = columnCount;
        panel.RowCount = rowCount;
        panel.AutoSize = autoSize;

        return panel;
    }


    #endregion
}
