// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global

/* DockOutlineBase.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Drawing;
using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms.Docking;

/// <summary>
///
/// </summary>
public abstract class DockOutlineBase
{
    /// <summary>
    ///
    /// </summary>
    protected DockOutlineBase()
    {
        _oldDockTo = null!;
        _dockTo = null!;

        Init();
    }

    private void Init()
    {
        SetValues (Rectangle.Empty, null, DockStyle.None, -1);
        SaveOldValues();
    }

    private Rectangle _oldFloatWindowBounds;

    /// <summary>
    ///
    /// </summary>
    protected Rectangle OldFloatWindowBounds => _oldFloatWindowBounds;

    private Control _oldDockTo;

    /// <summary>
    ///
    /// </summary>
    protected Control OldDockTo => _oldDockTo;

    private DockStyle _oldDock;

    /// <summary>
    ///
    /// </summary>
    protected DockStyle OldDock => _oldDock;

    private int _oldContentIndex;

    /// <summary>
    ///
    /// </summary>
    protected int OldContentIndex => _oldContentIndex;

    /// <summary>
    ///
    /// </summary>
    protected bool SameAsOldValue =>
        FloatWindowBounds == OldFloatWindowBounds &&
        DockTo == OldDockTo &&
        Dock == OldDock &&
        ContentIndex == OldContentIndex;

    private Rectangle _floatWindowBounds;

    /// <summary>
    ///
    /// </summary>
    public Rectangle FloatWindowBounds => _floatWindowBounds;

    private Control _dockTo;

    /// <summary>
    ///
    /// </summary>
    public Control DockTo => _dockTo;

    private DockStyle _dock;

    /// <summary>
    ///
    /// </summary>
    public DockStyle Dock => _dock;

    private int _contentIndex;

    /// <summary>
    ///
    /// </summary>
    public int ContentIndex => _contentIndex;

    /// <summary>
    ///
    /// </summary>
    public bool FlagFullEdge => _contentIndex != 0;

    /// <summary>
    ///
    /// </summary>
    public bool FlagTestDrop { get; set; }

    private void SaveOldValues()
    {
        _oldDockTo = _dockTo;
        _oldDock = _dock;
        _oldContentIndex = _contentIndex;
        _oldFloatWindowBounds = _floatWindowBounds;
    }

    /// <summary>
    ///
    /// </summary>
    protected abstract void OnShow();

    /// <summary>
    ///
    /// </summary>
    protected abstract void OnClose();

    private void SetValues
        (
            Rectangle floatWindowBounds,
            Control? dockTo,
            DockStyle dock,
            int contentIndex
        )
    {
        _floatWindowBounds = floatWindowBounds;
        _dockTo = dockTo!;
        _dock = dock;
        _contentIndex = contentIndex;
        FlagTestDrop = true;
    }

    private void TestChange()
    {
        if (_floatWindowBounds != _oldFloatWindowBounds ||
            _dockTo != _oldDockTo ||
            _dock != _oldDock ||
            _contentIndex != _oldContentIndex)
        {
            OnShow();
        }
    }

    /// <summary>
    ///
    /// </summary>
    public void Show()
    {
        SaveOldValues();
        SetValues (Rectangle.Empty, null, DockStyle.None, -1);
        TestChange();
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="pane"></param>
    /// <param name="dock"></param>
    public void Show (DockPane pane, DockStyle dock)
    {
        SaveOldValues();
        SetValues (Rectangle.Empty, pane, dock, -1);
        TestChange();
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="pane"></param>
    /// <param name="contentIndex"></param>
    public void Show (DockPane pane, int contentIndex)
    {
        SaveOldValues();
        SetValues (Rectangle.Empty, pane, DockStyle.Fill, contentIndex);
        TestChange();
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="dockPanel"></param>
    /// <param name="dock"></param>
    /// <param name="fullPanelEdge"></param>
    public void Show (DockPanel dockPanel, DockStyle dock, bool fullPanelEdge)
    {
        SaveOldValues();
        SetValues (Rectangle.Empty, dockPanel, dock, fullPanelEdge ? -1 : 0);
        TestChange();
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="floatWindowBounds"></param>
    public void Show (Rectangle floatWindowBounds)
    {
        SaveOldValues();
        SetValues (floatWindowBounds, null, DockStyle.None, -1);
        TestChange();
    }

    /// <summary>
    ///
    /// </summary>
    public void Close()
    {
        OnClose();
    }
}
