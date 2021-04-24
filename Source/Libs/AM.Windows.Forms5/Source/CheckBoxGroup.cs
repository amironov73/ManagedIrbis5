// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local

/* CheckBoxGroup.cs -- группа флажков-переключателей
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms
{
    /// <summary>
    /// Группа флажков-переключателей.
    /// </summary>
    // ReSharper disable RedundantNameQualifier
    [System.ComponentModel.DesignerCategory("Code")]
    // ReSharper restore RedundantNameQualifier
    public class CheckBoxGroup
        : GroupBox
    {
        private CheckBox[]? _buttons;

        /// <summary>
        /// Событие, возникающее при переключении флажков.
        /// </summary>
        [Browsable(true)]
        [Category("Property Changed")]
        [Description("Current CheckButton changed")]
        public event EventHandler? CurrentChanged;

        private const int DefaultLeftIndent = 5;
        private int _leftIndent = DefaultLeftIndent;

        /// <summary>
        /// Left indent value.
        /// </summary>
        [Browsable(true)]
        [DefaultValue(DefaultLeftIndent)]
        public virtual int LeftIndent
        {
            get => _leftIndent;
            set
            {
                if (_leftIndent != value)
                {
                    _leftIndent = value;
                    _CreateButtons(Lines, _current);
                }
            }
        }

        private const int DefaultLineIndent = 0;
        private int _lineIndent = DefaultLineIndent;

        /// <summary>
        /// Line indent value.
        /// </summary>
        [Browsable(true)]
        [DefaultValue(DefaultLineIndent)]
        public virtual int LineIndent
        {
            get => _lineIndent;
            set
            {
                if (_lineIndent != value)
                {
                    _lineIndent = value;
                    _CreateButtons(Lines, _current);
                }
            }
        }

        private const bool DefaultEvenly = true;
        private bool _evenly = DefaultEvenly;

        /// <summary>
        /// Property Evenly (bool)
        /// </summary>
        [Browsable(true)]
        [DefaultValue(DefaultEvenly)]
        public virtual bool Evenly
        {
            get => _evenly;
            set
            {
                if (_evenly != value)
                {
                    _evenly = value;
                    _CreateButtons(Lines, _current);
                }
            }
        }

        /// <summary>
        /// Button count.
        /// </summary>
        public virtual int Count => _buttons?.Length ?? 0;

        private const long DefaultCurrent = 0;
        private long _current = DefaultCurrent;
        private bool _inCurrent;

        /// <summary>
        /// Current CheckBox selection.
        /// </summary>
        [Browsable(true)]
        [DefaultValue(DefaultCurrent)]
        public virtual long Current
        {
            get => _current;
            set
            {
                if (_buttons != null)
                {
                    _inCurrent = true;
                    long mask = 1;
                    for (var i = 0; i < _buttons.Length; i++, mask <<= 1)
                    {
                        _buttons[i].Checked = ((mask & value) != 0);
                    }
                    _inCurrent = false;
                }
                if (_current != value)
                {
                    _current = value;
                    CurrentChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Lines of text.
        /// </summary>
        public virtual string[]? Lines
        {
            get
            {
                if (_buttons == null)
                {
                    return null;
                }
                var result = new string[_buttons.Length];
                for (var i = 0; i < _buttons.Length; i++)
                {
                    result[i] = _buttons[i].Text;
                }
                return result;
            }
            set
            {
                _CreateButtons(value.ThrowIfNull(nameof(value)), _current);
            }
        }

        /// <summary>
        /// Checked buttons
        /// </summary>
        public virtual bool[] Checked
        {
            get
            {
                if (_buttons == null)
                {
                    return Array.Empty<bool>();
                }

                var result = new bool[_buttons.Length];
                for (var i = 0; i < _buttons.Length; i++)
                {
                    result[i] = _buttons[i].Checked;
                }

                return result;
            }
            set
            {
                long curpos = 0;
                long mask = 1;

                if (_buttons != null)
                {
                    _inCurrent = true;
                    for (var i = 0; i < _buttons.Length; i++, mask <<= 1)
                    {
                        if (value[i])
                        {
                            curpos |= mask;
                        }
                        _buttons[i].Checked = value[i];
                    }
                    _inCurrent = false;
                }

                if (_current != curpos)
                {
                    _current = curpos;
                    CurrentChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        private const Appearance DefaultAppearance = Appearance.Normal;
        private Appearance _appearance = DefaultAppearance;

        /// <summary>
        /// Appearance.
        /// </summary>
        [Browsable(true)]
        [DefaultValue(DefaultAppearance)]
        public virtual Appearance Appearance
        {
            get => _appearance;
            set
            {
                if (_appearance != value)
                {
                    if (_buttons == null)
                    {
                        return;
                    }

                    foreach (var button in _buttons)
                    {
                        button.Appearance = value;
                    }
                    _appearance = value;
                }
            }
        }

        private void _DeleteButtons()
        {
            if (_buttons == null)
            {
                return;
            }
            foreach (var button in _buttons)
            {
                Controls.Remove(button);
                button.Dispose();
            }
            _buttons = null;
        }

        private void _CreateButtons
            (
                string[]? lines, long curpos
            )
        {
            _DeleteButtons();
            if (lines == null || lines.Length == 0)
            {
                return;
            }

            _buttons = new CheckBox[lines.Length];
            long mask = 1;
            var topIndent = Font.Height * 3 / 2;
            var delta = (ClientSize.Height - topIndent) / lines.Length;
            for (var i = 0; i < lines.Length; i++, mask <<= 1)
            {
                var button = new CheckBox
                {
                    Text = lines[i],
                    Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                    Checked = (curpos & mask) != 0,
                    ThreeState = false,
                    Visible = true,
                };
                button.Location = new Point(_leftIndent, (button.Height + _lineIndent) * i);
                if (_evenly)
                {
                    button.Top = delta * i + topIndent;
                }

                button.Width = ClientSize.Width - button.Left - _leftIndent;
                button.CheckedChanged += button_CheckedChanged;
                Controls.Add(button);
                _buttons[i] = button;
            }

            if (curpos != _current)
            {
                _current = curpos;
                CurrentChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        private void button_CheckedChanged
            (
                object? sender,
                EventArgs e
            )
        {
            if (_buttons is null)
            {
                return;
            }

            if (_inCurrent)
            {
                return;
            }

            long newcurrent = 0;
            long mask = 1;
            for (var i = 0; i < _buttons.Length; i++, mask <<= 1)
            {
                if (_buttons[i].Checked)
                {
                    newcurrent |= mask;
                }
            }

            if (newcurrent != _current)
            {
                _current = newcurrent;
                CurrentChanged?.Invoke(this, EventArgs.Empty);
            }
        }

    } // class CheckBoxGroup

} // namespace AM.Windows.Forms
