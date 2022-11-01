// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming

#region Using directives

using System;
using System.Windows.Forms;

using AM.Drawing.HtmlRenderer.Adapters.Entities;
using AM.Drawing.HtmlRenderer.Adapters;
using AM.Windows.Forms.HtmlRenderer.Utilities;

#endregion

#nullable enable

namespace AM.Windows.Forms.HtmlRenderer.Adapters;

/// <summary>
/// Adapter for WinForms context menu for core.
/// </summary>
internal sealed class ContextMenuAdapter
    : RContextMenu
{
    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public ContextMenuAdapter()
    {
        _contextMenu = new ContextMenuStrip();
        _contextMenu.ShowImageMargin = false;
    }

    #endregion

    #region Private members

    /// <summary>
    /// the underline win forms context menu
    /// </summary>
    private readonly ContextMenuStrip _contextMenu;

    #endregion

    #region RContextMenu members

    /// <inheritdoc cref="RContextMenu.ItemsCount"/>
    public override int ItemsCount => _contextMenu.Items.Count;

    /// <inheritdoc cref="RContextMenu.AddDivider"/>
    public override void AddDivider()
    {
        _contextMenu.Items.Add ("-");
    }

    /// <inheritdoc cref="RContextMenu.AddItem"/>
    public override void AddItem
        (
            string text,
            bool enabled,
            EventHandler onClick
        )
    {
        Sure.NotNullNorEmpty (text);
        Sure.NotNull (onClick);

        var item = _contextMenu.Items.Add (text, null, onClick);
        item.Enabled = enabled;
    }

    /// <inheritdoc cref="RContextMenu.RemoveLastDivider"/>
    public override void RemoveLastDivider()
    {
        if (_contextMenu.Items[^1].Text.IsEmpty())
        {
            _contextMenu.Items.RemoveAt (_contextMenu.Items.Count - 1);
        }
    }

    /// <inheritdoc cref="RContextMenu.Show"/>
    public override void Show
        (
            RControl parent,
            RPoint location
        )
    {
        Sure.NotNull (parent);

        var control = ((ControlAdapter)parent).Control;

        _contextMenu.Show (control, Utils.ConvertRound (location));
    }

    /// <inheritdoc cref="RContextMenu.Dispose"/>
    public override void Dispose()
    {
        _contextMenu.Dispose();
    }

    #endregion
}
