// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* CheckedGroupBox.cs -- группа с CheckBox в заголовке
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms;

/// <summary>
/// Группа <see cref="GroupBox"/> с <see cref="CheckBox"/> в заголовке.
/// </summary>
[System.ComponentModel.DesignerCategory ("Code")]
public class CheckedGroupBox
    : GroupBox
{
    #region Events

    /// <summary>
    /// Событие, возникающее при изменении состояния <see cref="CheckBox.Checked"/> у
    /// <see cref="CheckBox"/> в заголовке группы.
    /// </summary>
    public event EventHandler? CheckedChanged;

    #endregion

    #region Properties

    /// <summary>
    /// Получение состояния "отмечено" в заголовке группы.
    /// </summary>
    [System.ComponentModel.DefaultValue (true)]
    public bool Checked
    {
        get => _checkBox.Checked;
        set => _checkBox.Checked = value;
    }

    /// <summary>
    /// Текст заголовка группы.
    /// </summary>
    [AllowNull]
    public override string Text
    {
        get => _checkBox.Text;
        [DebuggerStepThrough]
        set
        {
            _checkBox.Text = value;
            OnTextChanged (EventArgs.Empty);
        }
    }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public CheckedGroupBox()
    {
        _savedState = new Dictionary<Control, bool>();
        _checkBox = new CheckBox
        {
            Text = base.Text,
            Left = 5,
            AutoSize = true,
            ForeColor = SystemColors.ControlText,
            Checked = true,
            ThreeState = false,
            Parent = this
        };
        _checkBox.CheckedChanged += _checkBox_CheckedChanged;
    }

    #endregion

    #region Private members

    private readonly CheckBox _checkBox;
    private readonly Dictionary<Control, bool> _savedState;

    private void _checkBox_CheckedChanged
        (
            object? sender,
            EventArgs eventArgs
        )
    {
        if (!_checkBox.Checked)
        {
            _savedState.Clear();
        }

        foreach (Control control in Controls)
        {
            if (control != _checkBox)
            {
                if (_checkBox.Checked)
                {
                    control.Enabled = !_savedState.TryGetValue (control, out var value) || value;
                }
                else
                {
                    _savedState.Add (control, control.Enabled);
                    control.Enabled = false;
                }
            }
        }

        CheckedChanged?.Invoke (this, eventArgs);
    }

    #endregion
}
