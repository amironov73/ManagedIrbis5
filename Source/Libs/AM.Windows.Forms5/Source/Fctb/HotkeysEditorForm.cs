// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement

/* HotkeyEditorForm.cs -- форма для редактирования горячих клавиш
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;

using AM;

#endregion

#nullable enable

namespace Fctb;

/// <summary>
/// Форма для редактирования горячих клавиш.
/// </summary>
public sealed partial class HotkeysEditorForm
    : Form
{
    #region Constructions

    /// <summary>
    /// Конструктор.
    /// </summary>
    public HotkeysEditorForm
        (
            HotkeyMapping hotkey
        )
    {
        InitializeComponent();
        BuildWrappers (hotkey);
        dgv.DataSource = _wrappers;
    }

    #endregion

    #region Private members

    private readonly BindingList<HotkeyWrapper> _wrappers = new ();

    int CompereKeys (Keys key1, Keys key2)
    {
        var res = ((int)key1 & 0xff).CompareTo ((int)key2 & 0xff);
        if (res == 0)
            res = key1.CompareTo (key2);

        return res;
    }

    private void BuildWrappers
        (
            HotkeyMapping mapping
        )
    {
        var keys = new List<Keys> (mapping.Keys);
        keys.Sort (CompereKeys);

        _wrappers.Clear();
        foreach (var k in keys)
        {
            _wrappers.Add (new HotkeyWrapper (k, mapping[k]));
        }
    }

    private void btAdd_Click
        (
            object sender,
            EventArgs e
        )
    {
        _wrappers.Add (new HotkeyWrapper (Keys.None, ActionCode.None));
    }

    private void dgv_RowsAdded
        (
            object sender,
            DataGridViewRowsAddedEventArgs e
        )
    {
        var cell = (dgv[0, e.RowIndex] as DataGridViewComboBoxCell).ThrowIfNull ();
        if (cell.Items.Count == 0)
        {
            foreach (var item in new []
                     {
                         "", "Ctrl", "Ctrl + Shift", "Ctrl + Alt", "Shift", "Shift + Alt", "Alt", "Ctrl + Shift + Alt"
                     })
            {
                cell.Items.Add (item);
            }
        }

        cell = (dgv[1, e.RowIndex] as DataGridViewComboBoxCell).ThrowIfNull ();
        if (cell.Items.Count == 0)
        {
            foreach (var item in Enum.GetValues (typeof (Keys)))
            {
                cell.Items.Add (item);
            }
        }

        cell = (dgv[2, e.RowIndex] as DataGridViewComboBoxCell).ThrowIfNull ();
        if (cell.Items.Count == 0)
        {
            foreach (var item in Enum.GetValues (typeof (ActionCode)))
            {
                cell.Items.Add (item);
            }
        }
    }

    private void btResore_Click
        (
            object sender,
            EventArgs e
        )
    {
        var h = new HotkeyMapping();
        h.InitDefault();
        BuildWrappers (h);
    }

    private void btRemove_Click
        (
            object sender,
            EventArgs e
        )
    {
        for (var i = dgv.RowCount - 1; i >= 0; i--)
            if (dgv.Rows[i].Selected)
                dgv.Rows.RemoveAt (i);
    }

    private void HotkeysEditorForm_FormClosing
        (
            object sender,
            FormClosingEventArgs e
        )
    {
        if (DialogResult == DialogResult.OK)
        {
            var actions = GetUnAssignedActions();
            if (!string.IsNullOrEmpty (actions))
            {
                if (MessageBox.Show (
                        "Some actions are not assigned!\r\nActions: " + actions +
                        "\r\nPress Yes to save and exit, press No to continue editing", "Some actions is not assigned",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                {
                    e.Cancel = true;
                }
            }
        }
    }

    private string GetUnAssignedActions()
    {
        var builder = new StringBuilder();
        var dictionary = new Dictionary<ActionCode, ActionCode>();

        foreach (var w in _wrappers)
        {
            dictionary[w.Action] = w.Action;
        }

        foreach (var item in Enum.GetValues (typeof (ActionCode)))
        {
            if ((ActionCode)item != ActionCode.None
                && !((ActionCode)item).ToString().StartsWith ("CustomAction"))
            {
                if (!dictionary.ContainsKey ((ActionCode)item))
                {
                    builder.Append (item + ", ");
                }
            }
        }

        return builder.ToString().TrimEnd (' ', ',');
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Returns edited hotkey map
    /// </summary>
    /// <returns></returns>
    public HotkeyMapping GetHotkeys()
    {
        var result = new HotkeyMapping();
        foreach (var w in _wrappers)
        {
            result[w.ToKeyData()] = w.Action;
        }

        return result;
    }

    #endregion

}
