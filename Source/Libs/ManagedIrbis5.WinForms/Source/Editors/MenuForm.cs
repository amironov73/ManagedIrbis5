// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global

/* MenuForm.cs -- форма для отображения меню
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Windows.Forms;

using ManagedIrbis.Menus;

#endregion

#nullable enable

namespace ManagedIrbis.WinForms.Editors;

/// <summary>
/// Форма для отображения меню (MNU-файла).
/// </summary>
public sealed partial class MenuForm
    : Form
{
    #region Properties

    /// <summary>
    /// Текущий элемент меню.
    /// </summary>
    public MenuEntry? CurrentEntry => (MenuEntry?)_grid.CurrentRow?.Data;

    /// <summary>
    /// Элементы меню.
    /// </summary>
    public MenuEntry[] Entries { get; private set; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public MenuForm()
    {
        InitializeComponent();

        Entries = Array.Empty<MenuEntry>();
        _grid.Focus();
        _grid.DoubleClick += _grid_DoubleClick;
        _grid.PreviewKeyDown += _grid_PreviewKeyDown;
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Заполнение грида меню.
    /// </summary>
    public void SetEntries
        (
            MenuEntry[] entries
        )
    {
        Entries = entries;
        _grid.Load (entries);
    }

    /// <summary>
    /// Выбор значения.
    /// </summary>
    public static MenuEntry? SelectEntry
        (
            MenuEntry[] entries,
            MenuEntry? current = default,
            IWin32Window? owner = default
        )
    {
        // TODO: установить первоначальную выбранную строку

        using var form = new MenuForm();
        form.SetEntries (entries);
        if (form.ShowDialog (owner) == DialogResult.OK)
        {
            return form.CurrentEntry;
        }

        return default;
    }

    #endregion

    #region Private members

    private void _grid_DoubleClick
        (
            object? sender,
            EventArgs e
        )
    {
        DialogResult = DialogResult.OK;
    }

    private void _grid_PreviewKeyDown
        (
            object? sender,
            PreviewKeyDownEventArgs e
        )
    {
        if (e.KeyCode == Keys.Enter)
        {
            e.IsInputKey = false;
            DialogResult = DialogResult.OK;
        }
    }

    #endregion
}
