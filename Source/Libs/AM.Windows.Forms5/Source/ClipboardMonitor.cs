// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* ClipboardMonitor.cs -- следит за изменениями в буфере обмена
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms;

/// <summary>
/// Следит за изменениями в буфере обмена.
/// </summary>
/// <remarks>
/// Заимствовано из ответа
/// https://stackoverflow.com/questions/621577/how-do-i-monitor-clipboard-changes-in-c
/// </remarks>
public sealed class ClipboardMonitor
    : Control
{
    #region Events

    /// <summary>
    /// Clipboard contents changed.
    /// </summary>
    public event EventHandler<ClipboardChangedEventArgs>? ClipboardChanged;

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public ClipboardMonitor()
    {
        Visible = false;
        _nextClipboardViewer = SetClipboardViewer (Handle);
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="container">Контейнер для компонентов</param>
    public ClipboardMonitor (IContainer? container)
        : this() =>
        container?.Add (this);

    #endregion

    #region Private members

    private IntPtr _nextClipboardViewer;

    [DllImport ("User32.dll")]
    private static extern IntPtr SetClipboardViewer (IntPtr hWndNewViewer);

    [DllImport ("User32.dll")]
    private static extern bool ChangeClipboardChain (IntPtr hWndRemove, IntPtr hWndNewNext);

    [DllImport ("User32.dll")]
    private static extern int SendMessage (IntPtr hwnd, int wMsg, IntPtr wParam, IntPtr lParam);

    private void OnClipboardChanged()
    {
        try
        {
            var iData = Clipboard.GetDataObject();
            if (ClipboardChanged is { } handler)
            {
                handler (this, new ClipboardChangedEventArgs (iData));
            }
        }
        catch (Exception e)
        {
            // Swallow or pop-up, not sure
            Trace.Write (e.ToString());

            // MessageBox.Show  (e.ToString());
        }
    }

    #endregion

    #region Control members

    /// <inheritdoc cref="Control.Dispose(bool)"/>
    protected override void Dispose
        (
            bool disposing
        )
    {
        ChangeClipboardChain (Handle, _nextClipboardViewer);
        base.Dispose (disposing);
    }

    /// <inheritdoc cref="Control.WndProc"/>
    protected override void WndProc
        (
            ref Message m
        )
    {
        // defined in winuser.h
        const int WM_DRAWCLIPBOARD = 0x0308;
        const int WM_CHANGECBCHAIN = 0x030D;

        switch (m.Msg)
        {
            case WM_DRAWCLIPBOARD:
                OnClipboardChanged();
                SendMessage (_nextClipboardViewer, m.Msg, m.WParam, m.LParam);
                break;

            case WM_CHANGECBCHAIN:
                if (m.WParam == _nextClipboardViewer)
                {
                    _nextClipboardViewer = m.LParam;
                }
                else
                {
                    SendMessage (_nextClipboardViewer, m.Msg, m.WParam, m.LParam);
                }

                break;

            default:
                base.WndProc (ref m);
                break;
        }
    }

    #endregion
}
