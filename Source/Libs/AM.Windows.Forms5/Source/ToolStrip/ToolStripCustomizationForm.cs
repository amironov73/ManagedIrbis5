// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* ToolStripCustomizationForm.cs -- форма настройки ToolStrip
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms;

/// <summary>
/// Форма настройки ToolStrip.
/// </summary>
partial class ToolStripCustomizationForm
    : Form
{
    #region Properties

    /// <summary>
    /// Подлежащий настройке ToolStrip.
    /// </summary>
    public ToolStrip ToolStrip { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public ToolStripCustomizationForm
        (
            ToolStrip toolStrip
        )
    {
        Sure.NotNull (toolStrip);

        ToolStrip = toolStrip;
        InitializeComponent();
    }

    #endregion

    #region Private members

    private void ToolStripCustomizationForm_Load
        (
            object sender,
            EventArgs e
        )
    {
        try
        {
            _listBox.BeginUpdate();
            _listBox.Items.Clear();
            foreach (ToolStripItem item in ToolStrip.Items)
            {
                _listBox.Items.Add (item, item.Available);
            }
        }
        finally
        {
            _listBox.EndUpdate();
        }
    }

    private void _applyButton_Click
        (
            object sender,
            EventArgs e
        )
    {
        foreach (ToolStripItem item in _listBox.Items)
        {
            var index = _listBox.Items.IndexOf (item);
            item.Available = _listBox.GetItemChecked (index);
        }
    }

    #endregion
}
