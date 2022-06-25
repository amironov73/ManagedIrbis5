// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming

using System;
using System.Windows.Forms;
using AM.Drawing.HtmlRenderer.Adapters.Entities;
using AM.Drawing.HtmlRenderer.Core.Utils;
using AM.Drawing.HtmlRenderer.Adapters;
using AM.Windows.Forms.HtmlRenderer.Utilities;

namespace AM.Windows.Forms.HtmlRenderer.Adapters;

/// <summary>
/// Adapter for WinForms context menu for core.
/// </summary>
internal sealed class ContextMenuAdapter : RContextMenu
{
    #region Fields and Consts

    /// <summary>
    /// the underline win forms context menu
    /// </summary>
    private readonly ContextMenuStrip _contextMenu;

    #endregion


    /// <summary>
    /// Init.
    /// </summary>
    public ContextMenuAdapter()
    {
        _contextMenu = new ContextMenuStrip();
        _contextMenu.ShowImageMargin = false;
    }

    public override int ItemsCount
    {
        get { return _contextMenu.Items.Count; }
    }

    public override void AddDivider()
    {
        _contextMenu.Items.Add("-");
    }

    public override void AddItem(string text, bool enabled, EventHandler onClick)
    {
        ArgChecker.AssertArgNotNullOrEmpty(text, "text");
        ArgChecker.AssertArgNotNull(onClick, "onClick");

        var item = _contextMenu.Items.Add(text, null, onClick);
        item.Enabled = enabled;
    }

    public override void RemoveLastDivider()
    {
        if (_contextMenu.Items[_contextMenu.Items.Count - 1].Text == string.Empty)
            _contextMenu.Items.RemoveAt(_contextMenu.Items.Count - 1);
    }

    public override void Show(RControl parent, RPoint location)
    {
        _contextMenu.Show(((ControlAdapter)parent).Control, Utils.ConvertRound(location));
    }

    public override void Dispose()
    {
        _contextMenu.Dispose();
    }
}