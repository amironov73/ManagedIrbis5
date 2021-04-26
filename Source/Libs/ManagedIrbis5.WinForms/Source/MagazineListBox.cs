// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* MagazineListBox.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using ManagedIrbis.Magazines;

#endregion

#nullable enable

namespace ManagedIrbis.WinForms
{
    /// <summary>
    ///
    /// </summary>
    public partial class MagazineListBox
        : UserControl
    {
        #region Events

        /// <summary>
        /// Fired when selected magazine changed.
        /// </summary>
        public event EventHandler? SelectedMagazineChanged;

        #endregion

        #region Properties

        /// <summary>
        /// Selected magazine.
        /// </summary>
        public MagazineInfo? SelectedMagazine => _listBox.SelectedItem as MagazineInfo;

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public MagazineListBox()
        {
            InitializeComponent();

            _textBox.DelayedTextChanged += _textBox_DelayedTextChanged;
            _textBox.KeyDown += _textBox_KeyDown;
            _listBox.SelectedIndexChanged += _listBox_SelectedIndexChanged;
        }

        #endregion

        #region Private members

        private MagazineInfo[]? _magazines;

        private void _listBox_SelectedIndexChanged
            (
                object? sender,
                EventArgs e
            )
        {
            SelectedMagazineChanged?.Invoke(this, EventArgs.Empty);
        }

        private void _textBox_DelayedTextChanged
            (
                object? sender,
                EventArgs e
            )
        {
            if (_magazines is not null)
            {
                var text = _textBox.Text.Trim();
                for (var i = 0; i < _magazines.Length; i++)
                {
                    var candidate = _magazines[i].Title;
                    if (string.Compare(candidate, text,
                            StringComparison.CurrentCultureIgnoreCase)
                        >= 0)
                    {
                        _listBox.SelectedIndex = i;
                        break;
                    }
                }
            }
        }

        private void _textBox_KeyDown
            (
                object? sender,
                KeyEventArgs e
            )
        {
            if (e.KeyData == Keys.Up)
            {
                if (_listBox.SelectedIndex > 0)
                {
                    _listBox.SelectedIndex--;
                }
                e.Handled = true;
            }

            if (e.KeyData == Keys.Down)
            {
                if (_listBox.SelectedIndex < (_listBox.Items.Count - 1))
                {
                    _listBox.SelectedIndex++;
                }
                e.Handled = true;
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Load all magazines from database.
        /// </summary>
        public MagazineManager LoadMagazines
            (
                ISyncProvider connection
            )
        {
            var result = new MagazineManager(connection);

            LoadMagazines(result);

            return result;
        }

        /// <summary>
        /// Load all magazines from database.
        /// </summary>
        public void LoadMagazines
            (
                MagazineManager manager
            )
        {
            var magazines = manager.GetAllMagazines();
            SetMagazines(magazines);
        }

        /// <summary>
        /// Set list of magazines.
        /// </summary>
        public void SetMagazines
            (
                IEnumerable<MagazineInfo> magazines
            )
        {
            _listBox.DataSource = null;

            _magazines = magazines
                .OrderBy(magazine => magazine.ExtendedTitle)
                .ToArray();
            _listBox.DisplayMember = "ExtendedTitle";
            _listBox.DataSource = _magazines;
        }

        #endregion
    }
}
